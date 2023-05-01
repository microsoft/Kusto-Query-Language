---
title: top-hitters operator - Azure Data Explorer
description: Learn how to use the top-hitters operator  to return an approximation for the most popular distinct values in the input.
ms.reviewer: alexans
ms.topic: reference
ms.date: 03/16/2023
---
# top-hitters operator

Returns an approximation for the most popular distinct values, or the values
with the largest sum, in the input.

> [!NOTE]
> `top-hitters` uses an approximation algorithm optimized for performance
> when the input data is large.
> The approximation is based on the [Count-Min-Sketch](https://en.wikipedia.org/wiki/Count%E2%80%93min_sketch) algorithm.  

## Syntax

*T* `|` `top-hitters` *NumberOfValues* `of` *ValueExpression* [ `by` *SummingExpression* ]

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *T* | string | &check; | The input tabular expression.|
| *NumberOfValues* | int, long, or real | &check; | The number of distinct values of *ValueExpression*.|
| *ValueExpression* | string | &check; | An expression over the input table *T* whose distinct values are returned.|
| *SummingExpression* | string | | If specified, a numeric expression over the input table *T* whose sum per distinct value of *ValueExpression* establishes which values to emit. If not specified, the count of each distinct value of *ValueExpression*  is used instead.|

## Remarks

The first syntax (no *SummingExpression*) is conceptually equivalent to:

*T*
`|` `summarize` `C``=``count()` `by` *ValueExpression*
`|` `top` *NumberOfValues* by `C` `desc`

The second syntax (with *SummingExpression*) is conceptually equivalent to:

*T*
`|` `summarize` `S``=``sum(*SummingExpression*)` `by` *ValueExpression*
`|` `top` *NumberOfValues* by `S` `desc`

## Examples

### Get most frequent items

The next example shows how to find top-5 types of storms.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSsp5qpRKMkv0M3ILClJLSpWMFXIT1MAy4RUFqQCAFP10SMoAAAA" target="_blank">Run the query</a>

```kusto
StormEvents
| top-hitters 5 of EventType 
```

**Output**

| EventType | approximate_count_EventType |
|---|---|
| Thunderstorm Wind | 13015 |
| Hail | 12711 |
| Flash Flood | 3688 |
| Drought | 3616 |
| Winter Weather | 3349 |

### Get top hitters based on column value

The next example shows how to find the States with the most "Thunderstorm Wind" events.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSsp5qpRKM9ILUpVAHNDKgtSFWxtFZRCMkrzUlKLikEKFcIz81KUgApL8gt0MzJLSoDiCoYGCvlpCsEliSWpAPMxVM5OAAAA" target="_blank">Run the query</a>

```kusto
StormEvents
| where EventType == "Thunderstorm Wind"
| top-hitters 10 of State 
```

**Output**

| State | approximate_sum_State |
|---|---|
| TEXAS | 830 |
| GEORGIA | 609 |
| MICHIGAN | 602 |
| IOWA | 585 |
| PENNSYLVANIA | 549 |
| ILLINOIS | 533 |
| NEW YORK | 502 |
| VIRGINIA | 482 |
| KANSAS | 476 |
| OHIO | 455 |
