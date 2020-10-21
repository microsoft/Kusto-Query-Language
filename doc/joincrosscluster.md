---
title: Cross-Cluster Join - Azure Data Explorer
description: This article describes Cross-Cluster Join in Azure Data Explorer.
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
# Cross-cluster join

::: zone pivot="azuredataexplorer"

For general discussion on cross-cluster queries, see [cross-cluster or cross-database queries](cross-cluster-or-database-queries.md)

It's possible to do join operation on datasets residing on different clusters. For example:

```kusto
T | ... | join (cluster("SomeCluster").database("SomeDB").T2 | ...) on Col1 // (1)

cluster("SomeCluster").database("SomeDB").T | ... | join (cluster("SomeCluster2").database("SomeDB2").T2 | ...) on Col1 // (2)
```

In the example above, the join operation is a cross-cluster join, assuming that current cluster isn't "SomeCluster" or "SomeCluster2".

In the following example:

```kusto
cluster("SomeCluster").database("SomeDB").T | ... | join (cluster("SomeCluster").database("SomeDB2").T2 | ...) on Col1 
```

the join operation isn't a cross-cluster join because both its operands originate on the same cluster.

When Kusto encounters a cross-cluster join, it will automatically decide where to execute the join operation itself. This decision can have one of the three possible outcomes:

* Execute join operation on the cluster of the left operand, right operand will be first fetched by this cluster. (join in example **(1)** will be executed on the local cluster)
* Execute join operation on the cluster of the right operand, left operand will be first fetched by this cluster. (join in example **(2)** will be executed on the "SomeCluster2")
* Execute join operation locally (meaning on the cluster that received the query), both operands will be first fetched by the local cluster.

The actual decision depends on the specific query. The automatic join remoting strategy is (simplified version): 
"If one of the operands is local, join will be executed locally. If both operands are remote, join will be executed on the cluster of the right operand".

Sometimes the performance of the query can be improved if automatic remoting strategy is not followed. In this case, execute join operation on the cluster of the largest operand.

If in example **(1)** the dataset produced by `T | ...` is much smaller than one produced by `cluster("SomeCluster").database("SomeDB").T2 | ...`, it is more efficient to execute join operation on "SomeCluster".

This operation can be done by giving Kusto join remoting hint. The syntax is:

```kusto
T | ... | join hint.remote=<strategy> (cluster("SomeCluster").database("SomeDB").T2 | ...) on Col1
```

Following are legal values for `strategy`
* `left` - execute join on the cluster of the left operand 
* `right` - execute join on the cluster of the right operand
* `local` - execute join on the cluster of the current cluster
* `auto` - (default) let Kusto make the automatic remoting decision

> [!Note]
> The join remoting hint will be ignored by Kusto if the hinted strategy isn't applicable to the join operation.

::: zone-end

::: zone pivot="azuremonitor"

This capability isn't supported in Azure Monitor

::: zone-end
