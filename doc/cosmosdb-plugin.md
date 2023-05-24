---
title:  cosmosdb_sql_request plugin
description: Learn how to use the cosmosdb_sql_request plugin to send a SQL query to an Azure Cosmos DB SQL network endpoint to query small datasets.
ms.reviewer: miwalia
ms.topic: reference
ms.date: 04/18/2023
zone_pivot_group_filename: data-explorer/zone-pivot-groups.json
zone_pivot_groups: kql-flavors-all
---
# cosmosdb_sql_request plugin

::: zone pivot="azuredataexplorer, fabric"

The `cosmosdb_sql_request` plugin sends a SQL query to an Azure Cosmos DB SQL network endpoint and returns the results of the query. This plugin is primarily designed for querying small datasets, for example, enriching data with reference data stored in [Azure Cosmos DB](/azure/cosmos-db/). The plugin is invoked with the [`evaluate`](evaluateoperator.md) operator.

## Syntax

`evaluate` `cosmosdb_sql_request` `(` *ConnectionString* `,` *SqlQuery* [`,` *SqlParameters* [`,` *Options*]] `)` [`:` *OutputSchema*]

## Parameters

| Name | Type | Required | Description |
|---|---|---|---|
| *ConnectionString* | string | &check; | The connection string that points to the Azure Cosmos DB collection to query. It must include *AccountEndpoint*, *Database*, and *Collection*. It may include *AccountKey* if a master key is used for authentication. For more information, see [Authentication and authorization](#authentication-and-authorization).</br> **Example:** `'AccountEndpoint=https://cosmosdbacc.documents.azure.com/ ;Database=MyDatabase;Collection=MyCollection;AccountKey=' h'R8PM...;'` |
| *SqlQuery*| string | &check; | The query to execute. |
| *SqlParameters* | dynamic | | The property bag object to pass as parameters along with the query. Parameter names must begin with `@`. |
| *OutputSchema* | | | The names and types of the expected columns of the `cosmosdb_sql_request` plugin output. Use the following syntax: `(` *ColumnName* `:` *ColumnType* [`,` ...] `)`. Specifying this parameter enables multiple query optimizations. |
| *Options* | dynamic | | A property bag object of advanced settings. If an `AccountKey` isn't provided in the *ConnectionString*, then the `armResourceId` field of this parameter is required. For more information, see [Supported options](#supported-options). |

### Supported options

The following table describes the supported fields of the *Options* parameter.

|Name|Type|Description|
|--|--|--|
| `armResourceId` | string | The  Azure Resource Manager resource ID of the Cosmos DB database. If an account key isn't provided in the connection string argument, this field is required. In such a case, the `armResourceId` is used to authenticate to Cosmos DB.</br>**Example:** `/subscriptions/a0cd6542-7eaf-43d2-bbdd-b678a869aad1/resourceGroups/ cosmoddbresourcegrouput/providers/Microsoft.DocumentDb/databaseAccounts/cosmosdbacc` |
| `token` | string | An Azure AD access token of a principal with access to the Cosmos DB database. This token is used together with the `armResourceId` to authenticate with the Azure Resource Manager. If unspecified, the token of the principal that made the query is used.|
| `preferredLocations` | string | The region from which to query the data. </br>**Example:** `['East US']` |

## Authentication and authorization

To authorize to an Azure Cosmos DB SQL network endpoint, you need to specify the authorization information. The following table provides the supported authentication methods and the description for how to use that method.

|Authentication method|Description|
|--|--|
|Azure Resource Manager resource ID (Recommended)|For secure authentication, we recommend specifying the `armResourceId` and optionally the `token` the [options](#supported-options). The `armResourceId` identifies the Cosmos DB database account, and the `token` should be a valid Azure AD bearer token for a principal with access permissions to the Cosmos DB database. If no `token` is provided, the Azure AD token of the requesting principal will be used for authentication.|
|Account key|You can add the account key directly to the *ConnectionString* argument. However, this approach is less secure as it involves including the secret in the query text, and is less resilient to future changes in the account key. To enhance security, hide the secret as an [obfuscated string literal](scalar-data-types/string.md#obfuscated-string-literals).|

## Set callout policy

The plugin makes callouts to the Azure Cosmos DB instance. Make sure that the cluster's [callout policy](../management/calloutpolicy.md) enables calls of type `cosmosdb` to the target *CosmosDbUri*.

The following example shows how to define the callout policy for Azure Cosmos DB. It's recommended to restrict it to specific endpoints (`my_endpoint1`, `my_endpoint2`).

```kusto
[
  {
    "CalloutType": "CosmosDB",
    "CalloutUriRegex": "my_endpoint1\\.documents\\.azure\\.com",
    "CanCall": true
  },
  {
    "CalloutType": "CosmosDB",
    "CalloutUriRegex": "my_endpoint2\\.documents\\.azure\\.com",
    "CanCall": true
  }
]
```

The following example shows an alter callout policy command for `cosmosdb` *CalloutType*

```kusto
.alter cluster policy callout @'[{"CalloutType": "cosmosdb", "CalloutUriRegex": "\\.documents\\.azure\\.com", "CanCall": true}]'
```

## Examples

### Query Azure Cosmos DB with a query-defined output schema

The following example uses the *cosmosdb_sql_request* plugin to send a SQL query while selecting only specific columns.
This query uses explicit schema definitions that allow various optimizations before the actual query is run against Cosmos DB.

```kusto
evaluate cosmosdb_sql_request(
  'AccountEndpoint=https://cosmosdbacc.documents.azure.com/;Database=MyDatabase;Collection=MyCollection;AccountKey=' h'R8PM...;',
  'SELECT Id, Name from c') : (Id:long, Name:string) 
```

### Query Azure Cosmos DB

The following example uses the *cosmosdb_sql_request* plugin to send a SQL query to fetch data from Azure Cosmos DB using its Azure Cosmos DB for NoSQL.

```kusto
evaluate cosmosdb_sql_request(
  'AccountEndpoint=https://cosmosdbacc.documents.azure.com/;Database=MyDatabase;Collection=MyCollection;AccountKey=' h'R8PM...;',
  'SELECT * from c') // OutputSchema is unknown, so it is not specified. This may harm the performance of the query.
```

### Query Azure Cosmos DB with parameters

The following example uses SQL query parameters and queries the data from an alternate region. For more information, see [`preferredLocations`](/azure/cosmos-db/tutorial-global-distribution-sql-api?tabs=dotnetv2%2Capi-async#preferred-locations).

```kusto
evaluate cosmosdb_sql_request(
    'AccountEndpoint=https://cosmosdbacc.documents.azure.com/;Database=MyDatabase;Collection=MyCollection;AccountKey=' h'R8PM...;',
    "SELECT c.id, c.lastName, @param0 as Column0 FROM c WHERE c.dob >= '1970-01-01T00:00:00Z'",
    dynamic({'@param0': datetime(2019-04-16 16:47:26.7423305)}),
    dynamic({'preferredLocations': ['East US']})) : (Id:long, Name:string, Column0: datetime) 
| where lastName == 'Smith'
```

::: zone-end

::: zone pivot="azuremonitor"

This capability isn't supported in Azure Monitor

::: zone-end
