---
title: http_request_post plugin - Azure Data Explorer
description: Learn how to use the http_request_post plugin to send an HTTP request and convert the response into a table.
services: data-explorer
ms.reviewer: zivc
ms.topic: reference
ms.date: 04/03/2023
zone_pivot_group_filename: data-explorer/zone-pivot-groups.json
zone_pivot_groups: kql-flavors
---
# http_request_post plugin

::: zone pivot="azuredataexplorer"

The `http_request_post` plugin sends an HTTP POST request and converts the response into a table.

## Prerequisites

* Run `.enable plugin http_request_post` to [enable the plugin](../management/enable-plugin.md)
* Set the URI to access as an allowed destination for `webapi` in the [Callout policy](../management/calloutpolicy.md)

## Syntax

`evaluate` `http_request_post` `(` *Uri* [`,` *RequestHeaders* [`,` *Options* [`,` *Content*]]] `)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *Uri* | string | &check; | The destination URI for the HTTP or HTTPS request. |
| *RequestHeaders* | dynamic |  | A property bag containing [HTTP headers](#headers) to send with the request. |
| *Options* | dynamic |  | A property bag containing additional properties of the request. |
| *Content* | string |  | The body content to send with the request. The content is encoded in `UTF-8` and the media type for the `Content-Type` attribute is `application/json`. |

> [!NOTE]
>
> * To specify an optional parameter that follows an optional parameter, make sure to provide a value for the preceding optional parameter. For more information, see [Working with optional parameters](syntax-conventions.md#working-with-optional-parameters).
> * If you're using authentication, use the HTTPS protocol. Attempts to use HTTP with authentication results in an error.

## Authentication and authorization

To authenticate, use the HTTP standard `Authorization` header or any custom header supported by the web service.

> [!NOTE]
> If the query includes confidential information, make sure that the relevant parts of the query text are obfuscated so that they'll be omitted from any tracing. For more information, see [obfuscated string literals](./scalar-data-types/string.md#obfuscated-string-literals).

## Returns

The plugin returns a table that has a single record with the following dynamic columns:

* *ResponseHeaders*: A property bag with the response header.
* *ResponseBody*: The response body parsed as a value of type `dynamic`.

If the HTTP response indicates (via the `Content-Type` response header) that the media type is `application/json`,
the response body is automatically parsed as-if it's a JSON object. Otherwise, it's returned as-is.

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

## Example

The following example is for a hypothetical HTTPS web service that accepts additional request headers and must be authenticated to using Azure AD:

<!-- csl -->
```kusto
let uri='https://example.com/node/js/on/eniac';
let headers=dynamic({'x-ms-correlation-vector':'abc.0.1.0', 'authorization':'bearer ...Azure-AD-bearer-token-for-target-endpoint...'});
evaluate http_request_post(uri, headers)
```

::: zone-end

::: zone pivot="azuremonitor"

This capability isn't supported in Azure Monitor.

::: zone-end
