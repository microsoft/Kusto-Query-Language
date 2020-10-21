---
title: Alias statement - Azure Data Explorer
description: This article describes Alias statement in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
zone_pivot_group_filename: data-explorer/zone-pivot-groups.json
zone_pivot_groups: kql-flavors
---
# Alias statement

::: zone pivot="azuredataexplorer"

Alias statements allow you to define an alias for databases, which can be used later in the same query.

This is useful when you're working with several clusters but want to appear as if you're working on fewer clusters.
The alias must be defined according to the following syntax, where *clustername* and *databasename* are existing and valid entities.

## Syntax

`alias` database[*'DatabaseAliasName'*] `=` cluster("https://*clustername*.kusto.windows.net:443").database("*databasename*")

`alias` database *DatabaseAliasName* `=` cluster("https://*clustername*.kusto.windows.net:443").database("*databasename*")

* *'DatabaseAliasName'* can be either an existing name or a new name.
* The mapped cluster-uri and the mapped database-name must appear inside double-quotes(") or single-quotes(')

## Examples

```kusto
alias database["wiki"] = cluster("https://somecluster.kusto.windows.net:443").database("somedatabase");
database("wiki").PageViews | count 
```

```kusto
alias database Logs = cluster("https://othercluster.kusto.windows.net:443").database("otherdatabase");
database("Logs").Traces | count 
```

::: zone-end

::: zone pivot="azuremonitor"

This capability isn't supported in Azure Monitor

::: zone-end
