# execute_show_command plugin

::: zone pivot="azuredataexplorer"

The `execute_show_command` plugin executes a Kusto `.show` command
on a cluster and returns its results.

**Syntax**

`evaluate` `execute_show_command` `(` *KustoConnectionString* `,` *ShowCommand* `)`

**Arguments**

* *ConnectionStringKusto*: A `string` literal containing a valid
  [Kusto connection string](../api/connection-strings/kusto.md) that
  points at the target Kusto endpoint.
  The literal may be set to a single dot (`.`), in which case the connection
  string is used to issue the query (same cluster and database).
  
    > [!NOTE]
    > The connection string is only used to indicate the target Kusto endpoint
    > (the cluster) and optionally the database. Other connection
    > string properties are not used.

* *ShowCommand*: A `string` literal indicating the `.show` command that is executed
  against the specified Kusto endpoint.

> [!NOTE]
> Authentication on the target endpoint is done with the credentials
> used to run the query using this plugin. If the credentials can't be propagated
> to the target endpoint, the plugin fails.

**Examples**

The following example queries the `help` cluster for two records of queries
and two records of commands.

```
union
  (evaluate execute_show_command(
    "https://help.kusto.windows.net/$systemdb",
    ".show queries  | take 2 | project What='Query', StartedOn, Text")),
  (evaluate execute_show_command(
    "https://help.kusto.windows.net/$systemdb",
    ".show commands | take 2 | project What='Command', StartedOn, Text"))
```

::: zone-end

::: zone pivot="azuremonitor"

This isn't supported in Azure Monitor

::: zone-end
