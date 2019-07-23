---
title: autocluster plugin - Azure Data Explorer | Microsoft Docs
description: This article describes autocluster plugin in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 05/26/2019
---
# autocluster plugin

```kusto
T | evaluate autocluster()
```

AutoCluster finds common patterns of discrete attributes (dimensions) in the data and will reduce the results of the original query (whether it's 100 or 100k rows) to a small number of patterns. AutoCluster was developed to help analyze failures (e.g. exceptions, crashes) but can potentially work on any filtered data set. 

**Syntax**

`T | evaluate autocluster(` *arguments* `)`

**Returns**

AutoCluster returns a (usually small) set of patterns that capture portions of the data with shared common values across multiple discrete attributes. Each pattern is represented by a row in the results. 

The first column is the segment Id. The next two columns are the count and percentage of rows out of the original query that are captured by the pattern. The remaining columns are from the original query and their value is either a specific value from the column or a wildcard value (which are by default null) meaning variable values. 

Note that the patterns are not distinct: they may be overlapping, and usually do not cover all the original rows. Some rows may not fall under any pattern.

> [!TIP]
> Use [where](./whereoperator.md) and [project](./projectoperator.md) in the input pipe to reduce the data to just what you're interested in.
>
> When you find an interesting row, you might want to drill into it further by adding its specific values to your `where` filter.

**Arguments (all optional)**

`T | evaluate autocluster(`[*SizeWeight*, *WeightColumn*, *NumSeeds*, *CustomWildcard*, *CustomWildcard*, ...]`)`

All arguments are optional, but they must be ordered as above. To indicate that the default value should be used, put the string tilde value - '~' (see examples below).

Available arguments:

* SizeWeight - 0 < *double* < 1 [default: 0.5]

    Gives you some control over the balance between generic (high coverage) and informative (many shared values). Increasing the value usually reduces the number of patterns, and each pattern tends to cover a larger percentage. Decreasing the value usually produces more specific patterns with more shared values and smaller percentage coverage. The under the hood formula is a weighted geometric mean between the normalized generic score and informative score with `SizeWeight` and `1-SizeWeight` as the weights. 

    Example: `T | evaluate autocluster(0.8)`

* WeightColumn - *column_name*

    Considers each row in the input according to the specified weight (by default each row has a weight of '1'). The argument must be a name of a numeric column (e.g. int, long, real). A common usage of a weight column is to take into account sampling or bucketing/aggregation of the data that is already embedded into each row.
    
    Example: `T | evaluate autocluster('~', sample_Count)` 

* NumSeeds - *int* [default: 25] 

    The number of seeds determines the number of initial local search points of the algorithm. In some cases, depending on the structure of the data, increasing the number of seeds increases the number (or quality) of the results through increased search space at slower query tradeoff. The value has diminishing results in both directions so decreasing it below 5 will achieve negligible performance improvements and increasing above 50 will rarely generate additional patterns.

    Example:  `T | evaluate autocluster('~', '~', 15)`

* CustomWildcard - *"any_value_per_type"*

    Sets the wildcard value for a specific type in the result table that will indicate that the current pattern doesn't have a restriction on this column.
    Default is null, for string default is an empty string. If the default is a viable value in the data, a different wildcard value should be used (e.g. `*`).

    Example: `T | evaluate autocluster('~', '~', '~', '*', int(-1), double(-1), long(0), datetime(1900-1-1))`

**Example**

```kusto
StormEvents 
| where monthofyear(StartTime) == 5
| extend Damage = iff(DamageCrops + DamageProperty > 0 , "YES" , "NO")
| project State , EventType , Damage
| evaluate autocluster(0.6)
```
|SegmentId|Count|Percent|State|EventType|Damage|
|---|---|---|---|---|---|---|---|---|
|0|2278|38.7||Hail|NO
|1|512|8.7||Thunderstorm Wind|YES
|2|898|15.3|TEXAS||

**Example with custom wildcards**
```kusto
StormEvents 
| where monthofyear(StartTime) == 5
| extend Damage = iff(DamageCrops + DamageProperty > 0 , "YES" , "NO")
| project State , EventType , Damage 
| evaluate autocluster(0.2, '~', '~', '*')
```
|SegmentId|Count|Percent|State|EventType|Damage|
|---|---|---|---|---|---|---|---|---|
|0|2278|38.7|\*|Hail|NO
|1|512|8.7|\*|Thunderstorm Wind|YES
|2|898|15.3|TEXAS|\*|\*

**See also**

* [basket](./basketplugin.md)
* [reduce](./reduceoperator.md)

**Additional information**

* AutoCluster is largely based on the Seed-Expand algorithm from the following paper: [Algorithms for Telemetry Data Mining using Discrete Attributes](https://www.scitepress.org/DigitalLibrary/PublicationsDetail.aspx?ID=d5kcrO+cpEU=&t=1). 

