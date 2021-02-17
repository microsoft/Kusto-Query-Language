---
title: rolling_percentile plugin - Azure Data Explorer
description: This article describes rolling_percentile plugin in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# rolling_percentile() plugin

Returns an estimate for the specified percentile of the *ValueColumn* population in a rolling (sliding) *BinsPerWindow* size window per *BinSize*.

```kusto
T | evaluate rolling_percentile(ValueColumn, Percentile, IndexColumn, BinSize, BinsPerWindow)
```

## Syntax

*T* `| evaluate` `rolling_percentile(`*ValueColumn*`,` *Percentile*`,` *IndexColumn*`,` *BinSize*`,` *BinsPerWindow*  [`,` *dim1*`,` *dim2*`,` ...] `)`

## Arguments

* *T*: The input tabular expression.
* *ValueColumn*: The name of the column with values to calculate the percentile of. 
* *Percentile*: Scalar with the percentile to calculate.
* *IndexColumn*: The name of the column to run the rolling window over.
* *BinSize*: Scalar with size of the bins to apply over the *IndexColumn*.
* *BinsPerWindow*: Scalar with number of bins included in each window.
* *dim1*, *dim2*, ... : (optional) list of the dimensions columns to slice by.

## Returns

Returns a table with a row per each bin (and combination of dimensions if specified) that has the rolling percentile of values in the window ending at the bin (inclusive). 
Output table schema is:


|IndexColumn|dim1|...|dim_n|rolling_BinsPerWindow_percentile_ValueColumn_Pct
|---|---|---|---|---|


## Examples

### Rolling 3-day median value per day 

The next query calculates a 3-day median value in daily granularity. Each row in the output represents the median value for the last 3 bins (days), including the bin itself.

<!-- csl: https://help.kusto.windows.net:443/Samples -->
```kusto
let T = 
range idx from 0 to 24*10-1 step 1
| project Timestamp = datetime(2018-01-01) + 1h*idx, val=idx+1
| extend EvenOrOdd = iff(val % 2 == 0, "Even", "Odd");
 T  
 | evaluate rolling_percentile(val, 50, Timestamp, 1d, 3)
```

|Timestamp|rolling_3_percentile_val_50|
|---|---|
|2018-01-01 00:00:00.0000000|	12|
|2018-01-02 00:00:00.0000000|	24|
|2018-01-03 00:00:00.0000000|	36|
|2018-01-04 00:00:00.0000000|	60|
|2018-01-05 00:00:00.0000000|	84|
|2018-01-06 00:00:00.0000000|	108|
|2018-01-07 00:00:00.0000000|	132|
|2018-01-08 00:00:00.0000000|	156|
|2018-01-09 00:00:00.0000000|	180|
|2018-01-10 00:00:00.0000000|	204|

### Rolling 3-day median value per day by dimension

Same example from above, but now also calculates the rolling window partitioned for each value of the dimension.

<!-- csl: https://help.kusto.windows.net:443/Samples -->
```kusto
let T = 
range idx from 0 to 24*10-1 step 1
| project Timestamp = datetime(2018-01-01) + 1h*idx, val=idx+1
| extend EvenOrOdd = iff(val % 2 == 0, "Even", "Odd");
 T  
 | evaluate rolling_percentile(val, 50, Timestamp, 1d, 3, EvenOrOdd)
```

|Timestamp|	EvenOrOdd|	rolling_3_percentile_val_50|
|---|---|---|
|2018-01-01 00:00:00.0000000|	Even|	12|
|2018-01-02 00:00:00.0000000|	Even|	24|
|2018-01-03 00:00:00.0000000|	Even|	36|
|2018-01-04 00:00:00.0000000|	Even|	60|
|2018-01-05 00:00:00.0000000|	Even|	84|
|2018-01-06 00:00:00.0000000|	Even|	108|
|2018-01-07 00:00:00.0000000|	Even|	132|
|2018-01-08 00:00:00.0000000|	Even|	156|
|2018-01-09 00:00:00.0000000|	Even|	180|
|2018-01-10 00:00:00.0000000|	Even|	204|
|2018-01-01 00:00:00.0000000|	Odd|	11|
|2018-01-02 00:00:00.0000000|	Odd|    23|
|2018-01-03 00:00:00.0000000|	Odd|	35|
|2018-01-04 00:00:00.0000000|	Odd|	59|
|2018-01-05 00:00:00.0000000|	Odd|	83|
|2018-01-06 00:00:00.0000000|	Odd|	107|
|2018-01-07 00:00:00.0000000|	Odd|	131|
|2018-01-08 00:00:00.0000000|	Odd|	155|
|2018-01-09 00:00:00.0000000|	Odd|	179|
|2018-01-10 00:00:00.0000000|	Odd|	203|
