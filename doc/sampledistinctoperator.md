---
title:  sample-distinct operator
description: Learn how to use the sample-distinct operator to return a column that contains up to the specified number of distinct values of the requested columns.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/19/2023
---
# sample-distinct operator

Returns a single column that contains up to the specified number of distinct values of the requested column.

The operator tries to return an answer as quickly as possible rather than trying to make a fair sample.

## Syntax

*T* `| sample-distinct` *NumberOfValues* `of` *ColumnName*

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *T*| string | &check; | The input tabular expression. |
| *NumberOfValues*| int, long, or real | &check; | The number distinct values of *T* to return. You can specify any numeric expression.|
| *ColumnName*| string | &check; | The name of the column from which to sample.|

> [!TIP]
>
> * Use the [top-hitters](tophittersoperator.md) operator to get the top values.
> * Refer to the [sample operator](sampleoperator.md) to sample data rows.

## Examples  

Get 10 distinct values from a population

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSspVqhRKE7MLchJ1U3JLC7JzEsuUTA0UMhPU3AtyCzOT0n1TAEAXIVALi0AAAA=" target="_blank">Run the query</a>

```kusto
StormEvents | sample-distinct 10 of EpisodeId
```

Sample a population and do further computation without exceeding the query limits in the summarize

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA1WNuw6CUBBEe75iSihIsDZ0UlDzBVfuGpfcB9ldNBo+Xk0waDlzTmYCGdTFOVA3s2ZPihaDZYndjZIp1g3XntU4jYZDg3zBpvf+WPzqxYr7lYR2Dk4o/y+qj6ZLjE74SbBsLvRpWoRJ23dffsOJhUarcH7sey97JpXgsgAAAA==" target="_blank">Run the query</a>

```kusto
let sampleEpisodes = StormEvents | sample-distinct 10 of EpisodeId;
StormEvents 
| where EpisodeId in (sampleEpisodes) 
| summarize totalInjuries=sum(InjuriesDirect) by EpisodeId
```
