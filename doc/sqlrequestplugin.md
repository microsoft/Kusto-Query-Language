---
title: sql_request plugin - Azure Data Explorer
description: This article describes sql_request plugin in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 05/19/2022
zone_pivot_group_filename: data-explorer/zone-pivot-groups.json
zone_pivot_groups: kql-flavors
---
# sql_request plugin

::: zone pivot="azuredataexplorer"

The `sql_request` plugin sends a SQL query to a SQL Server network endpoint and returns the results.
If more than one rowset is returned by SQL, only the first one is used.
The plugin is invoked with the [`evaluate`](evaluateoperator.md) operator.

## Syntax

  `evaluate` `sql_request` `(` *ConnectionString* `,` *SqlQuery* [`,` *SqlParameters* [`,` *Options*]] `)` [`:` *OutputSchema*]

## Arguments

| Name | Type | Required| Description |
|---|---|---|---|
| *ConnectionString* | string | &check; | Indicates the connection string that points at the SQL Server network endpoint. See [valid methods of authentication](#authentication) and how to specify the [network endpoint](#specify-the-network-endpoint). |
| *SqlQuery* | string | &check; | Indicates the query that is to be executed against the SQL endpoint. Must return one or more row sets, but only the first one is made available for the rest of the Kusto query. |
| *SqlParameters* | dynamic | | Holds key-value pairs to pass as parameters along with the query. |
|*Options* | dynamic | |Holds more advanced settings as key-value pairs. Currently, only `token` can be set, to pass a caller-provided Azure AD access token that is forwarded to the SQL endpoint for authentication.
| *OutputSchema* | | | The names and types for the expected columns of the `sql_request` plugin output.|

The optional *OutputSchema* argument has the following syntax:

`(` *ColumnName* `:` *ColumnType* [`,` ...] `)`

Specifying this argument allows the plugin to be used
in scenarios (such as a cross-cluster query) which would otherwise prevent it from running,
and enables multiple query optimizations.
It is therefore recommended to always specify it.
An error is raised if the run-time schema of the first rowset returned by the SQL network endpoint
doesn't match the *OutputSchema* schema.

## Examples

### Send a SQL query using Azure AD-integrated authentication

The following example sends a SQL query to an Azure SQL DB database. It
retrieves all records from `[dbo].[Table]`, and then processes the results on the
 Kusto side. Authentication reuses the calling user's Azure AD token.

> [!NOTE]
> This example should not be taken as a recommendation to filter or project data in this manner. SQL queries should be constructed to return the smallest data set possible, since the Kusto optimizer doesn't attempt to optimize queries between Kusto and SQL.

```kusto
evaluate sql_request(
  'Server=tcp:contoso.database.windows.net,1433;'
    'Authentication="Active Directory Integrated";'
    'Initial Catalog=Fabrikam;',
  'select * from [dbo].[Table]')
| where Id > 0
| project Name
```

### Send a SQL query using Username/Password authentication

The following example is identical to the previous one, except that SQL
authentication is done by username/password. For confidentiality,
we use obfuscated strings here.

```kusto
evaluate sql_request(
  'Server=tcp:contoso.database.windows.net,1433;'
    'Initial Catalog=Fabrikam;'
    h'User ID=USERNAME;'
    h'Password=PASSWORD;',
  'select * from [dbo].[Table]')
| where Id > 0
| project Name
```

### Send a SQL query using an Azure AD access token

The following example sends a SQL query to an Azure SQL DB database
retrieving all records from `[dbo].[Table]`, while appending another `datetime` column,
and then processes the results on the Kusto side.
It specifies a SQL parameter (`@param0`) to be used in the SQL query.

```kusto
evaluate sql_request(
  'Server=tcp:contoso.database.windows.net,1433;'
    'Authentication="Active Directory Integrated";'
    'Initial Catalog=Fabrikam;',
  'select *, @param0 as dt from [dbo].[Table]',
  dynamic({'param0': datetime(2020-01-01 16:47:26.7423305)}))
| where Id > 0
| project Name
```

### Send a SQL query with a query-defined output schema

The following example sends a SQL query to an Azure SQL DB database
retrieving all records from `[dbo].[Table]`, while selecting only specific columns.
It uses explicit schema definitions that allow various optimizations to be evaluated before the
actual query against SQL is run.

```kusto
evaluate sql_request(
  'Server=tcp:contoso.database.windows.net,1433;'
    'Authentication="Active Directory Integrated";'
    'Initial Catalog=Fabrikam;',
  'select Id, Name') : (Id:long, Name:string)
| where Id > 0
| project Name
```

## Authentication

The sql_request plugin supports three methods of authentication to the
SQL Server endpoint:

### Azure AD-integrated authentication

`Authentication="Active Directory Integrated"`

  Azure AD-integrated authentication is the preferred method. This method has the user or application authenticate via Azure AD to Kusto. The same token is then used to access the SQL Server network endpoint.

### Username/Password authentication

`User ID=...; Password=...;`

  Username and password authentication support is provided when Azure AD-integrated authentication can't be done. Avoid this method, when possible, as secret information is sent through Kusto.

### Azure AD access token

`dynamic({'token': h"eyJ0..."})`

   With the Azure AD access token authentication method, the caller generates the access token, which is forwarded by Kusto to the SQL endpoint. The connection string shouldn't include authentication information like `Authentication`, `User ID`, or `Password`. Instead, the access token is passed as `token` property in the `Options` argument of the sql_request plugin.

> [!WARNING]
> Connection strings and queries that include confidential information or information that should be guarded should be obfuscated to be omitted from any Kusto tracing.
> For more informations, see [obfuscated string literals](scalar-data-types/string.md#obfuscated-string-literals).

## Encryption and server validation

The following connection properties are forced when connecting to a SQL Server network
endpoint, for security reasons.

* `Encrypt` is set to `true` unconditionally.
* `TrustServerCertificate` is set to `false` unconditionally.

As a result, the SQL Server must be configured with a valid SSL/TLS server certificate.

## Specify the network endpoint

Specifying the SQL network endpoint as part of the connection string is mandatory.
The appropriate syntax is:

`Server` `=` `tcp:` *FQDN* [`,` *Port*]

Where:

* *FQDN* is the fully qualified domain name of the endpoint.
* *Port* is the TCP port of the endpoint. By default, `1433` is assumed.

> [!NOTE]
> Other forms of specifying the network endpoint are not supported.
> One cannot omit, for example, the prefix `tcp:` even though it is possible to
> do so when using the SQL client libraries programmatically.

::: zone-end

::: zone pivot="azuremonitor"

This capability isn't supported in Azure Monitor

::: zone-end
