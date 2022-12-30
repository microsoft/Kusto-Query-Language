---
title: http_request plugin - Azure Data Explorer
description: Learn how to use the http_request plugin to send an HTTP request and convert the response into a table.
services: data-explorer
ms.reviewer: zivc
ms.topic: reference
ms.date: 12/28/2022
zone_pivot_group_filename: data-explorer/zone-pivot-groups.json
zone_pivot_groups: kql-flavors
---
# http_request plugin / http_request_post plugin

::: zone pivot="azuredataexplorer"

The `http_request` (GET) and `http_request_post` (POST) plugins send an HTTP request and convert the response into a table.

> [!IMPORTANT]
> Both plugins are disabled by default, as they allow queries to send data
> and the user's security token to external user-specified network endpoints.
> Once enabled, both plugins are further subject to the configured
> [callout policy](../management/calloutpolicy.md) that cluster admins
> can use to granularly control which URIs the request can be sent to.

## Syntax

`evaluate` `http_request` `(` *Uri* [`,` *RequestHeaders* [`,` *Options*]] `)`

`evaluate` `http_request_post` `(` *Uri* [`,` *RequestHeaders* [`,` *Options* [`,` *Content*]]] `)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *Uri* | string | &check; | The destination URI for the HTTP or HTTPS request. |
| *RequestHeaders* | dynamic |  | A property bag containing [HTTP headers](#headers) to send with the request. |
| *Options* | dynamic |  | A property bag containing additional properties of the request. |
| *Content* | string |  | The body content to send with the request. The content is encoded in `UTF-8` and the media type for the `Content-Type` attribute is `application/json`. |

## Returns

Both plugins return a table that has a single record with the following dynamic columns:

* *ResponseHeaders*: A property bag with the response header.
* *ResponseBody*: The response body parsed as a value of type `dynamic`.

If the HTTP response indicates (via the `Content-Type` response header) that the media type is `application/json`,
the response body is automatically parsed as-if it's a JSON object. Otherwise, it's returned as-is.

## Prerequisites

Before you use the `http_request` and `http_request_post` plugins, make sure that requests meet the following requirements:

* The specified *Uri* value must be a destination that is enabled for `webapi` callout by the [Callout policy](../management/calloutpolicy.md). Otherwise, running the query results in an error.

* If you're using authentication, you must use the HTTPS protocol. Attempts to use HTTP with authentication enabled results in an error.

## Authentication

You can use the query arguments to specify authentication parameters for the `http_request` and `http_request_post` plugins. The following scenarios are supported:

| Argument | Description |
|--|--|
| *Uri* | The URI to authenticate with. |
| *RequestHeaders* | Using the HTTP standard `Authorization` header or any custom header supported by the web service. |

<!--
| *Options* | Using the HTTP standard `Authorization` header.<br />If you want to use Azure Active Directory (Azure AD) authentication, you must use an HTTPS URI for the request and set the following values:<br />* `azure_active_directory` to `Active Directory Integrated`<br />* `AadResourceId` to the Azure AD ResourceId value of the target web service. |
-->

> [!WARNING]
> Be extra careful not to send secret information, such as
> authentication tokens, over HTTP connections. Additionally, if the query includes
> confidential information, make sure that the relevant parts of the
> query text are obfuscated so that they'll be omitted from any tracing.
> For more information, see [obfuscated string literals](./scalar-data-types/string.md#obfuscated-string-literals).

## Headers

The *RequestHeaders* argument can be used to add custom headers
to the outgoing HTTP request. In addition to the standard HTTP request headers
and the user-provided custom headers, the plugin also adds the following
custom headers:

| Name | Description |
|--|--|
| `x-ms-client-request-id` | A correlation ID that identifies the request. Multiple invocations of the plugin in the same query will all have the same ID. |
| `x-ms-readonly` | A flag indicating that the processor of this request shouldn't make any persistent changes. |

> [!WARNING]
> The `x-ms-readonly` flag is set for every HTTP request sent by the plugin
> that was triggered by a query and not a control command. Web services should
> treat any requests with this flag as one that does not make internal
> state changes, otherwise they should refuse the request. This protects users from being
> sent seemingly-innocent queries that end up making unwanted changes by using
> a Kusto query as the launchpad for such attacks.

## Examples

The following example retrieves Azure retails prices for Azure Purview in west Europe:

<!-- csl -->
```kusto
let Uri = "https://prices.azure.com/api/retail/prices?$filter=serviceName eq 'Azure Purview' and location eq 'EU West'";
evaluate http_request(Uri)
| project ResponseBody.Items
| mv-expand ResponseBody_Items
| evaluate bag_unpack(ResponseBody_Items)
```

**Output**

| armRegionName |                   armSkuName                   | currencyCode |  effectiveStartDate  | isPrimaryMeterRegion | location |               meterId                |                      meterName                       |  productId   |                     productName                     | retailPrice | serviceFamily |  serviceId   |  serviceName  |       skuId       |                 skuName                  | tierMinimumUnits |    type     | unitOfMeasure | unitPrice |
|---------------|------------------------------------------------|--------------|----------------------|----------------------|----------|--------------------------------------|------------------------------------------------------|--------------|-----------------------------------------------------|-------------|---------------|--------------|---------------|-------------------|------------------------------------------|------------------|-------------|---------------|-----------|
| westeurope    | Data Insights                                  | USD          | 2022-06-01T00:00:00Z | false                | EU West  | 8ce915f7-20db-564d-8cc3-5702a7c952ab | Data Insights Insights Report Consumption            | DZH318Z08M22 | Azure Purview Data Map                              |        0.21 | Analytics     | DZH318Q66D0F | Azure Purview | DZH318Z08M22/006C | Catalog Insights                         |                0 | Consumption | 1 API Calls   |      0.21 |
| westeurope    | Data Map Enrichment - Data Insights Generation | USD          | 2022-06-01T00:00:00Z | false                | EU West  | 7ce2db1d-59a0-5193-8a57-0431a10622b6 | Data Map Enrichment - Data Insights Generation vCore | DZH318Z08M22 | Azure Purview Data Map                              |        0.82 | Analytics     | DZH318Q66D0F | Azure Purview | DZH318Z08M22/005C | Data Map Enrichment - Insight Generation |                0 | Consumption | 1 Hour        |      0.82 |
| westeurope    |                                                | USD          | 2021-09-28T00:00:00Z | false                | EU West  | 053e2dcb-82c0-5e50-86cd-1f1c8d803705 | Power BI vCore                                        | DZH318Z08M23 | Azure Purview Scanning Ingestion and Classification |           0 | Analytics     | DZH318Q66D0F | Azure Purview | DZH318Z08M23/0005 | Power BI                                  |                0 | Consumption | 1 Hour        |         0 |
| westeurope    |                                                | USD          | 2021-09-28T00:00:00Z | false                | EU West  | a7f57f26-5f31-51e5-a5ed-ffc2b0da37b9 | Resource Set vCore                                   | DZH318Z08M22 | Azure Purview Data Map                              |        0.21 | Analytics     | DZH318Q66D0F | Azure Purview | DZH318Z08M22/000X | Resource Set                             |                0 | Consumption | 1 Hour        |      0.21 |
| westeurope    |                                                | USD          | 2021-09-28T00:00:00Z | false                | EU West  | 5d157295-441c-5ea7-ba7c-5083026dc456 | SQL Server vCore                                     | DZH318Z08M23 | Azure Purview Scanning Ingestion and Classification |           0 | Analytics     | DZH318Q66D0F | Azure Purview | DZH318Z08M23/000F | SQL Server                               |                0 | Consumption | 1 Hour        |         0 |
| westeurope    |                                                | USD          | 2021-09-28T00:00:00Z | false                | EU West  | 0745df0d-ce4f-52db-ac31-ac574d4dcfe5 | Standard Capacity Unit                               | DZH318Z08M22 | Azure Purview Data Map                              |       0.411 | Analytics     | DZH318Q66D0F | Azure Purview | DZH318Z08M22/0002 | Standard                                 |                0 | Consumption | 1 Hour        |     0.411 |
| westeurope    |                                                | USD          | 2021-09-28T00:00:00Z | false                | EU West  | 811e3118-5380-5ee8-a5d9-01d48d0a0627 | Standard vCore                                       | DZH318Z08M23 | Azure Purview Scanning Ingestion and Classification |        0.63 | Analytics     | DZH318Q66D0F | Azure Purview | DZH318Z08M23/0009 | Standard                                 |                0 | Consumption | 1 Hour        |      0.63 |

The following example is for a hypothetical HTTPS web service that accepts additional request headers and must be authenticated to using Azure AD:

<!-- csl -->
```kusto
let uri='https://example.com/node/js/on/eniac';
let headers=dynamic({'x-ms-correlation-vector':'abc.0.1.0', 'authorization':'bearer ...Azure-AD-bearer-token-for-target-endpoint...'});
evaluate http_request_post(uri, headers)
```

::: zone-end

::: zone pivot="azuremonitor"

This capability isn't supported in Azure Monitor

::: zone-end
