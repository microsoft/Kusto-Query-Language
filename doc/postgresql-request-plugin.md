---
title: postgresql_request plugin - Azure Data Explorer
description: Learn how to use the postgresql_request plugin to send a SQL query to a PostgreSql server network endpoint.
ms.reviewer: alexans
ms.topic: reference
ms.date: 05/08/2023
zone_pivot_group_filename: data-explorer/zone-pivot-groups.json
zone_pivot_groups: kql-flavors
---
# postgresql_request plugin

::: zone pivot="azuredataexplorer"

The `postgresql_request` plugin sends a SQL query to a PostgreSql Server network endpoint and returns the first rowset in the results. The query may return more than one rowset, but only the first rowset is made available for the rest of the Kusto query.

The plugin is invoked with the [`evaluate`](evaluateoperator.md) operator.

> [!IMPORTANT]
> The `postgresql_request` plugin is disabled by default.
> To enable the plugin, run the [`.enable plugin postgresql_request` command](../management/enable-plugin.md). To see which plugins are enabled, use [`.show plugin` management commands](../management/show-plugins.md).

## Syntax

`evaluate` `postgresql_request` `(` *ConnectionString* `,` *SqlQuery* [`,` *SqlParameters*] `)` [`:` *OutputSchema*]

## Parameters

| Name | Type | Required| Description |
|---|---|---|---|
| *ConnectionString* | string | &check; | The connection string that points at the PostgreSQL Server network endpoint. See [authentication](#username-and-password-authentication) and how to specify the [network endpoint](#specify-the-network-endpoint). |
| *SqlQuery* | string | &check; | The query that is to be executed against the SQL endpoint. Must return one or more row sets. Only the first set is made available for the rest of the query. |
| *SqlParameters* | dynamic | | A property bag object that holds key-value pairs to pass as parameters along with the query. |
| *OutputSchema* | | | The names and types for the expected columns of the `postgresql_request` plugin output.<br /><br />**Syntax**: `(` *ColumnName* `:` *ColumnType* [`,` ...] `)`|

> [!NOTE]
>
> * Specifying the *OutputSchema* is highly recommended, as it allows the plugin to be used in scenarios that might otherwise not work without it, such as a cross-cluster query. The *OutputSchema* can also enable multiple query optimizations.
> * An error is raised if the run-time schema of the first row set returned by the SQL network endpoint doesn't match the *OutputSchema* schema.

## Authentication and authorization

To authorize a PostgreSql Server network endpoint, you must specify the authorization information in the connection string. The supported authorization method is via username and password.

## Set callout policy

The plugin makes callouts to the PostgreSql database. Make sure that the cluster's [callout policy](../management/calloutpolicy.md) enables calls of type `postgresql` to the target *PostgreSqlDbUri*.

The following example shows how to define the callout policy for PostgreSql databases. We recommend restricting the callout policy to specific endpoints (`my_endpoint1`, `my_endpoint2`).

```kusto
[
  {
    "CalloutType": "postgresql",
    "CalloutUriRegex": "my_endpoint1\\.postgres\\.database\\.azure\\.com",
    "CanCall": true
  },
  {
    "CalloutType": "postgresql",
    "CalloutUriRegex": "my_endpoint2\\.postgres\\.database\\.azure\\.com",
    "CanCall": true
  }
]
```

The following example shows a `.alter callout policy` command for `postgresql` *CalloutType*:

```kusto
.alter cluster policy callout @'[{"CalloutType": "postgresql", "CalloutUriRegex": "\\.postgresql\\.database\\.azure\\.com", "CanCall": true}]'
```

## Username and password authentication

The `postgresql_request` plugin only supports username and password authentication to the PostgreSQL server endpoint and doesn't integrate with Azure Active Directory authentication.

The username and password are provided as part of the connections string using the following parameters:

`User ID=...; Password=...;`

> [!WARNING]
> Confidential or guarded information should be obfuscated from connection strings and queries so that they are omitted from any Kusto tracing.
> For more information, see [obfuscated string literals](scalar-data-types/string.md#obfuscated-string-literals).

## Encryption and server validation

For security, `SslMode` is unconditionally set to `Required` when connecting to a PostgreSQL server network endpoint. As a result, the server must be configured with a valid SSL/TLS server certificate.

## Specify the network endpoint

Specify the PostgreSQL network endpoint as part of the connection string.

**Syntax**:

`Host` `=` *FQDN* [`Port` `=` *Port*]

Where:

* *FQDN* is the fully qualified domain name of the endpoint.
* *Port* is the TCP port of the endpoint.

## Examples

### SQL query to Azure PostgreSql DB

The following example sends a SQL query to an Azure PostgreSQL database. It retrieves all records from `public."Table"`, and then processes the results.

> [!NOTE]
> This example shouldn't be taken as a recommendation to filter or project data in this manner. SQL queries should be constructed to return the smallest data set possible.

```kusto
evaluate postgresql_request(
    'Host=contoso.postgres.database.azure.com; Port = 5432;'
    'Database=Fabrikam;'
    h'User Id=USERNAME;'
    h'Password=PASSWORD;',
    'select * from public."Table"') : (Id: int, Name: string)
| where Id > 0
| project Name
```

### SQL query to an Azure PostgreSql database with modifications

The following example sends a SQL query to an Azure PostgreSql database
retrieving all records from `public."Table"`, while appending another `datetime` column,
and then processes the results on the Azure Data Explorer side.
It specifies a SQL parameter (`@param0`) to be used in the SQL query.

```kusto
evaluate postgresql_request(
    'Server=contoso.postgres.database.azure.com; Port = 5432;'
    'Database=Fabrikam;'
    h'User Id=USERNAME;'
    h'Password=PASSWORD;',
    'select *, @param0 as dt from public."Table"',
    dynamic({'param0': datetime(2020-01-01 16:47:26.7423305)})) : (Id: int, Name: string, dt: datetime)
| where Id > 0
| project Name
```

### SQL query to an Azure PostgreSql database without a query-defined output schema

The following example sends a SQL query to an Azure PostgreSQL database without an output schema. This is not recommended unless the schema is unknown, as it may impact the performance of the query

```kusto
evaluate postgresql_request(
    'Host=contoso.postgres.database.azure.com; Port = 5432;'
    'Database=Fabrikam;'
    h'User Id=USERNAME;'
    h'Password=PASSWORD;',
    'select * from public."Table"')
| where Id > 0
| project Name
```

::: zone-end

::: zone pivot="azuremonitor"

This capability isn't supported in Azure Monitor.

::: zone-end
