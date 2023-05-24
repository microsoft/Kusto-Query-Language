---
title:  series_stats()
description: Learn how to use the series_stats() function to calculate the statistics for a numerical series using multiple columns.
ms.reviewer: alexans
ms.topic: reference
ms.date: 03/29/2023
---
# series_stats()

Returns statistics for a numerical series in a table with a column for each statistic.

> [!NOTE]
> This function returns multiple values. If you only need a single value, such as the average, consider using [series_stats_dynamic](./series-stats-dynamicfunction.md).

## Syntax

`...` `|` `extend` `(` *Name*`,` ... `)` `=` `series_stats` `(` *series* [`,` *ignore_nonfinite*] `)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *Name* | string | | The column labels for the output table. If not provided, the system will generate them. If you provide a limited number of names, the table will show only those columns.|
| *series* | dynamic | &check; | An array of numeric values.|
| *ignore_nonfinite* | bool | | Determines if the calculation includes non-finite values like `null`, `NaN`, `inf`, and so on. The default is `false`, which will result in `null` if non-finite values are present.|

## Returns

A table with a column for each of the statistics displayed in the following table.

|Statistic | Description|
|--|--|
| `min` | The minimum value in the input array.|
| `min_idx`| The first position of the minimum value in the input array.|
| `max` | The maximum value in the input array.|
| `max_idx`| The first position of the maximum value in the input array.|
| `avg`| The average value of the input array.|
| `variance` | The sample variance of input array.|
| `stdev`| The sample standard deviation of the input array.|

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUaiwTanMS8zNTNaINjLWUTAx01EA0RbmQDaQ0lEAcsxNgYI6CqZAOUMo1xjINwSptYzVVOCqUSgoys9KTS5RKE4tykwtji8uSSwp1qjQBADbRN1SZAAAAA==" target="_blank">Run the query</a>

```kusto
print x=dynamic([23, 46, 23, 87, 4, 8, 3, 75, 2, 56, 13, 75, 32, 16, 29]) 
| project series_stats(x)
```

**Output**

|series_stats_x_min|series_stats_x_min_idx|series_stats_x_max|series_stats_x_max_idx|series_stats_x_avg|series_stats_x_stdev|series_stats_x_variance|
|---|---|---|---|---|---|---|
|2|8|87|3|32.8|28.5036338535483|812.457142857143|
