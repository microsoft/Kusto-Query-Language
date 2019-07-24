# execute_show_command plugin

The `execute_show_command` plugin executes a Kusto `.show` command
on some cluster and returns its results.

**Syntax**

`evaluate` `execute_show_command` `(` *KustoConnectionString* `,` *ShowCommand* `)`

**Arguments**

* *ConnectionStringKusto*: A `string` literal containing a valid
  [Kusto connection string](../api/connection-strings/kusto.md) that
  points at the target Kusto endpoint. See notes below for limitations.

* *ShowCommand*: A `string` literal indicating the `.show` command that is to be executed
  against the specified Kusto endpoint.

> [!NOTE]
> The connection string is only used to indicate the target Kusto endpoint
> (the cluster) and optionally the database in context. Other connection
> string properties are not used.
>
> Authentication against the target endpoint is done with the credentials
> used to run the query itself. If those credentials cannot be propagated
> to the target endpoint then the plugin fails.

**Examples**

The following example queries the `help` cluster for two records of queries
that were run on it and two records of commands.

```
union
  (evaluate execute_show_command(
    "https://help.kusto.windows.net/$systemdb",
    ".show queries  | take 2 | project What='Query', StartedOn, Text")),
  (evaluate execute_show_command(
    "https://help.kusto.windows.net/$systemdb",
    ".show commands | take 2 | project What='Command', StartedOn, Text"))
```
