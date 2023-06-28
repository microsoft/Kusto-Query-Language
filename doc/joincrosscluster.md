---
title:  Cross-cluster join
description: Learn how to perform the Cross-cluster join operation to join datasets residing on different clusters.
ms.reviewer: alexans
ms.topic: reference
ms.date: 06/27/2023
zone_pivot_group_filename: data-explorer/zone-pivot-groups.json
zone_pivot_groups: kql-flavors-all
---
# Cross-cluster join

::: zone pivot="azuredataexplorer, fabric"

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

* Execute join operation on the cluster of the left operand. The right operand is first fetched by this cluster. (join in example **(1)** will be executed on the local cluster)
* Execute join operation on the cluster of the right operand. The left operand is first fetched by this cluster. (join in example **(2)** will be executed on the "SomeCluster2")
* Execute join operation locally (meaning on the cluster that received the query). Both operands are first fetched by the local cluster.

The actual decision depends on the specific query. The automatic join remoting strategy is (simplified version):
"If one of the operands is local, join will be executed locally. If both operands are remote, join will be executed on the cluster of the right operand".

Sometimes the performance of the query can be improved if automatic remoting strategy isn't followed. In this case, execute join operation on the cluster of the largest operand.

"Example 1" is set to run on the local cluster, but if the dataset produced by `T | ...` is smaller than one produced by `cluster("SomeCluster").database("SomeDB").T2 | ...` then it would be more efficient to execute the join operation on `SomeCluster` instead of on the local cluster.

To execute the join on `SomeCluster`, specify the remote strategy as `right`. Then, the cluster will execute the join on the right cluster even if the left cluster is the local cluster.

```kusto
T | ... | join hint.remote=right (cluster("SomeCluster").database("DB").T2 | ...) on Col1
```

Following are legal values for `strategy`:

* `left` - execute join on the cluster of the left operand
* `right` - execute join on the cluster of the right operand
* `local` - execute join on the cluster of the current cluster
* `auto` - (default) let Kusto make the automatic remoting decision

> [!NOTE]
> The join remoting hint will be ignored by Kusto if the hinted strategy isn't applicable to the join operation.

::: zone-end

::: zone pivot="azuremonitor"

This capability isn't supported in Azure Monitor

::: zone-end
