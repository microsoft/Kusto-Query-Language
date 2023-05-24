---
title:  rolling_percentile plugin
description: Learn how to use the rolling_percentile plugin to calculate an estimate of the rolling percentile per bin for the specified value column.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/17/2023
---
# rolling_percentile() plugin

Returns an estimate for the specified percentile of the *ValueColumn* population in a rolling (sliding) *BinsPerWindow* size window per *BinSize*.

The plugin is invoked with the [`evaluate`](evaluateoperator.md) operator.

## Syntax

*T* `| evaluate` `rolling_percentile(`*ValueColumn*`,` *Percentile*`,` *IndexColumn*`,` *BinSize*`,` *BinsPerWindow*  [`,` *dim1*`,` *dim2*`,` ...] `)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *T*| string | &check; | The input tabular expression.|
| *ValueColumn*| string | &check;| The name of the column used to calculate the percentiles.|
| *Percentile*| int, long, or real | &check;| Scalar with the percentile to calculate.|
| *IndexColumn*| string | &check;| The name of the column over which to run the rolling window.|
| *BinSize*| int, long, real, datetime, or timespan | &check;| Scalar with size of the bins to apply over the *IndexColumn*.|
| *BinsPerWindow*| int | &check;| The number of bins included in each window.|
| *dim1*, *dim2*, ... | string | | A list of the dimensions columns to slice by.|

## Returns

Returns a table with a row per each bin (and combination of dimensions if specified) that has the rolling percentile of values in the window ending at the bin (inclusive).
Output table schema is:

|IndexColumn|dim1|...|dim_n|rolling_BinsPerWindow_percentile_ValueColumn_Pct
|---|---|---|---|---|

## Examples

### Rolling 3-day median value per day

The next query calculates a 3-day median value in daily granularity. Each row in the output represents the median value for the last 3 bins (days), including the bin itself.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAz2OwQqDMAyG7z7FjzDQWaF1GwyGx529eB9io+uotdRueNjDL+5gCYSQ7/9SSxEtaiTgFzo3EoxeMYR5gkScUZ1xhJIoobBE8lB/9Asf5hf1nDYTLbGbPFt0FynynFVSXUupuHIUUE92sFbg09l68xe7htZITuP+IdeERmu2mGHIGMQBFeoaUiDd1il3BtL8lrRAwlGG3nwRYbbWuPHhKfTkorG05QUuHN2/J6C0wCn/AfIZwjfyAAAA" target="_blank">Run the query</a>

```kusto
let T = 
    range idx from 0 to 24 * 10 - 1 step 1
    | project Timestamp = datetime(2018-01-01) + 1h * idx, val=idx + 1
    | extend EvenOrOdd = iff(val % 2 == 0, "Even", "Odd");
T  
| evaluate rolling_percentile(val, 50, Timestamp, 1d, 3)
```

**Output**

|Timestamp|rolling_3_percentile_val_50|
|---|---|
|2018-01-01 00:00:00.0000000| 12|
|2018-01-02 00:00:00.0000000| 24|
|2018-01-03 00:00:00.0000000| 36|
|2018-01-04 00:00:00.0000000| 60|
|2018-01-05 00:00:00.0000000| 84|
|2018-01-06 00:00:00.0000000| 108|
|2018-01-07 00:00:00.0000000| 132|
|2018-01-08 00:00:00.0000000| 156|
|2018-01-09 00:00:00.0000000| 180|
|2018-01-10 00:00:00.0000000| 204|

### Rolling 3-day median value per day by dimension

Same example from above, but now also calculates the rolling window partitioned for each value of the dimension.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA0WOQQuDMAyF7/6KhzDQWaF1GwyGx529eB9io+uoVWonHvbjF3dwIRBC3vteLAXUKBGByzeuJxi9ovPjAIkwojjjCCWRQ2EONEH9pB9MfnxRy24z0ByaYWKKbgIF3pNCqmsuFXeKDOrJDMYKLI0tN362Y2gN5DTuC7nKV1ozxXRdwkIcUKAsIQXi7RzzZEGc3qIaiNjKojcnwo/WGtc/JvItuWAsbX6BC1v39wSUFjiJf1T6BZy1q2z9AAAA" target="_blank">Run the query</a>

```kusto
let T = 
    range idx from 0 to 24 * 10 - 1 step 1
    | project Timestamp = datetime(2018-01-01) + 1h * idx, val=idx + 1
    | extend EvenOrOdd = iff(val % 2 == 0, "Even", "Odd");
T  
| evaluate rolling_percentile(val, 50, Timestamp, 1d, 3, EvenOrOdd)
```

**Output**

|Timestamp| EvenOrOdd| rolling_3_percentile_val_50|
|---|---|---|
|2018-01-01 00:00:00.0000000| Even| 12|
|2018-01-02 00:00:00.0000000| Even| 24|
|2018-01-03 00:00:00.0000000| Even| 36|
|2018-01-04 00:00:00.0000000| Even| 60|
|2018-01-05 00:00:00.0000000| Even| 84|
|2018-01-06 00:00:00.0000000| Even| 108|
|2018-01-07 00:00:00.0000000| Even| 132|
|2018-01-08 00:00:00.0000000| Even| 156|
|2018-01-09 00:00:00.0000000| Even| 180|
|2018-01-10 00:00:00.0000000| Even| 204|
|2018-01-01 00:00:00.0000000| Odd| 11|
|2018-01-02 00:00:00.0000000| Odd|    23|
|2018-01-03 00:00:00.0000000| Odd| 35|
|2018-01-04 00:00:00.0000000| Odd| 59|
|2018-01-05 00:00:00.0000000| Odd| 83|
|2018-01-06 00:00:00.0000000| Odd| 107|
|2018-01-07 00:00:00.0000000| Odd| 131|
|2018-01-08 00:00:00.0000000| Odd| 155|
|2018-01-09 00:00:00.0000000| Odd| 179|
|2018-01-10 00:00:00.0000000| Odd| 203|
