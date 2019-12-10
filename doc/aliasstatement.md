# Alias statement

::: zone pivot="azuredataexplorer"

Alias statements allow to define alias for databases which can be used later in the same query.

This is useful when working with several clusters while trying to appear as working on less clusters or only on one cluster.
The alias must be defined according to the following syntax where *clustername* and *databasename* must be an existing and valid entities.

**Syntax**

`alias` database[*'DatabaseAliasName'*] `=` cluster("https://*clustername*.kusto.windows.net:443").database("*databasename*")

`alias` database *DatabaseAliasName* `=` cluster("https://*clustername*.kusto.windows.net:443").database("*databasename*")

* *'DatabaseAliasName'* can be either en axisting name or a new name.
* The mapped cluster-uri and the mapped database-name must appear inside double-quotes(") or single-quotes(')

**Examples**

<!-- csl -->
```
alias database["wiki"] = cluster("https://somecluster.kusto.windows.net:443").database("somedatabase");
database("wiki").PageViews | count 
```

<!-- csl -->
```
alias database Logs = cluster("https://othercluster.kusto.windows.net:443").database("otherdatabase");
database("Logs").Traces | count 
```

::: zone-end

::: zone pivot="azuremonitor"

This isn't supported in Azure Monitor

::: zone-end
