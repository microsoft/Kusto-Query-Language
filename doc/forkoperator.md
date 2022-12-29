---
title: fork operator - Azure Data Explorer
description: Learn how to use the fork operator to run multiple consumer operators in parallel.
ms.reviewer: alexans
ms.topic: reference
ms.date: 12/12/2022
zone_pivot_group_filename: data-explorer/zone-pivot-groups.json
zone_pivot_groups: kql-flavors
---
# fork operator

::: zone pivot="azuredataexplorer"

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
* [`sort`](sortoperator.md)
* [`mv-expand`](mvexpandoperator.md)
* [`reduce`](reduceoperator.md)

## Returns

Multiple result tables, one for each of the *subquery* arguments.

## Tips

* Use [`materialize`](materializefunction.md) as a replacement for [`join`](joinoperator.md) or [`union`](unionoperator.md) on fork legs. The input stream will be cached by materialize and then the cached expression can be used in join/union legs.

* Use [batch](batches.md) with [`materialize`](materializefunction.md) of tabular expression statements instead of the `fork` operator.

## Examples

In the following example, the result tables will be named "GenericResult",  "GenericResult_2" and "GenericResult_3":

```kusto
KustoLogs
| where Timestamp > ago(1h)
| fork
    ( where Level == "Error" | project EventText | limit 100 )
    ( project Timestamp, EventText | top 1000 by Timestamp desc)
    ( summarize min(Timestamp), max(Timestamp) by ActivityID )
```

In the following examples, the result tables will be named "Errors", "EventsTexts" and "TimeRangePerActivityID":

```kusto
KustoLogs
| where Timestamp > ago(1h)
| fork
    ( where Level == "Error" | project EventText | limit 100 | as Errors )
    ( project Timestamp, EventText | top 1000 by Timestamp desc | as EventsTexts )
    ( summarize min(Timestamp), max(Timestamp) by ActivityID | as TimeRangePerActivityID )
```

```kusto
KustoLogs
| where Timestamp > ago(1h)
| fork
    Errors = ( where Level == "Error" | project EventText | limit 100 )
    EventsTexts = ( project Timestamp, EventText | top 1000 by Timestamp desc )
    TimeRangePerActivityID = ( summarize min(Timestamp), max(Timestamp) by ActivityID )
```

::: zone-end

::: zone pivot="azuremonitor"

This capability isn't supported in Azure Monitor

::: zone-end
