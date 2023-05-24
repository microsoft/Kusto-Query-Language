---
title:  fork operator
description: Learn how to use the fork operator to run multiple consumer operators in parallel.
ms.reviewer: alexans
ms.topic: reference
ms.date: 03/15/2023
zone_pivot_group_filename: data-explorer/zone-pivot-groups.json
zone_pivot_groups: kql-flavors-all
---
# fork operator

::: zone pivot="azuredataexplorer, fabric"

Runs multiple consumer operators in parallel.

## Syntax

*T* `|` `fork` [*name*`=`]`(`*subquery*`)` [*name*`=`]`(`*subquery*`)` ...

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *subquery* | string | &check; | A downstream pipeline of [supported query operators](#supported-query-operators).|
| *name* | string | | A temporary name for the subquery result table.|

> [!NOTE]
>
> * Avoid using `fork` with a single *subquery*.
> * The name of the results tab will be the same name as provided with the `name` parameter or the [`as` operator](asoperator.md).

### Supported query operators

* [`as`](asoperator.md)
* [`count`](countoperator.md)
* [`extend`](extendoperator.md)
* [`parse`](parseoperator.md)
* [`where`](whereoperator.md)
* [`take`](takeoperator.md)
* [`project`](projectoperator.md)
* [`project-away`](projectawayoperator.md)
* [`project-keep`](project-keep-operator.md)
* [`project-rename`](projectrenameoperator.md)
* [`project-reorder`](projectreorderoperator.md)
* [`summarize`](summarizeoperator.md)
* [`top`](topoperator.md)
* [`top-nested`](topnestedoperator.md)
* [`sort`](./sort-operator.md)
* [`mv-expand`](mvexpandoperator.md)
* [`reduce`](reduceoperator.md)

## Returns

Multiple result tables, one for each of the *subquery* arguments.

## Tips

* Use [`materialize`](materializefunction.md) as a replacement for [`join`](joinoperator.md) or [`union`](unionoperator.md) on fork legs. The input stream will be cached by materialize and then the cached expression can be used in join/union legs.

* Use [batch](batches.md) with [`materialize`](materializefunction.md) of tabular expression statements instead of the `fork` operator.

## Examples

### Unnamed subqueries

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSsp5uWqUSjPSC1KVQguSSxJVbC1VVBy8/EP8nRxVALJpeUXZfNyKQCBBlSdS2piSUaxS2ZRanKJgjaU65mXAhGwUzDURFXvmZdVWpSZitABE0DRAwCWU8oSkwAAAA==" target="_blank">Run the query</a>

```kusto
StormEvents
| where State == "FLORIDA"
| fork
    ( where DeathsDirect + DeathsIndirect > 1)
    ( where InjuriesDirect + InjuriesIndirect > 1)
```

### Named subqueries

In the following examples, the result tables will be named "StormsWithDeaths" and "StormsWithInjuries".

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSsp5uWqUSjPSC1KVQguSSxJVbC1VVBy8/EP8nRxVALJpeUXZfNyKQCBBkSZS2piSUaxS2ZRanKJgjaU65mXAhGwUzBUqFFILAaaBrSiODyzJAOiQhPFEM+8rNKizFSEMTABfAbB1GgCAM0zVJu/AAAA" target="_blank">Run the query</a>

```kusto
StormEvents
| where State == "FLORIDA"
| fork
    (where DeathsDirect + DeathsIndirect > 1 | as StormsWithDeaths)
    (where InjuriesDirect + InjuriesIndirect > 1 | as StormsWithInjuries)
```

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSsp5uWqUSjPSC1KVQguSSxJVbC1VVBy8/EP8nRxVALJpeUXZfNyKQBBMEhTcXhmSYZLamJJRrGCrYIGRCeE75JZlJpcoqAN5XrmpUAE7BQMNdFN8MzLKi3KTEUyAyYCNwUmgGIOAGODP2W5AAAA" target="_blank">Run the query</a>

```kusto
StormEvents
| where State == "FLORIDA"
| fork
    StormsWithDeaths = (where DeathsDirect + DeathsIndirect > 1)
    StormsWithInjuries = (where InjuriesDirect + InjuriesIndirect > 1)
```

::: zone-end

::: zone pivot="azuremonitor"

This capability isn't supported in Azure Monitor

::: zone-end