---
title: Alias statement - Azure Data Explorer | Microsoft Docs
description: This article describes Alias statement in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 10/23/2018
---
# Alias statement

Alias statements allow to define alias for databases which can be used later in the same query.

This is useful when working with several clusters while trying to appear as working on less clusters or only on one cluster.
The alias must be defined according to the following syntax where *clustername* and *databasename* must be an existing and valid entities.

**Syntax**

`alias` database[*'DatabaseAliasName'*] `=` cluster("https://*clustername*.kusto.windows.net:443").database("*databasename*")

`alias` database *DatabaseAliasName* `=` cluster("https://*clustername*.kusto.windows.net:443").database("*databasename*")

* *'DatabaseAliasName'* can be either en axisting name or a new name.
* The mapped cluster-uri and the mapped database-name must appear inside double-quotes(") or single-quotes(')

**Examples**

```kusto
alias database["wiki"] = cluster("https://somecluster.kusto.windows.net:443").database("somedatabase");
database("wiki").PageViews | count 
```

```kusto
alias database Logs = cluster("https://othercluster.kusto.windows.net:443").database("otherdatabase");
database("Logs").Traces | count 
```