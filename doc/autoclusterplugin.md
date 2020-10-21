---
title: autocluster plugin - Azure Data Explorer
description: This article describes autocluster plugin in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# autocluster plugin

```kusto
T | evaluate autocluster()
```

`autocluster` finds common patterns of discrete attributes (dimensions) in the data. It then reduces the results of the original query, whether it's 100 or 100k rows, to a small number of patterns. The plugin was developed to help analyze failures (such as exceptions or crashes) but can potentially work on any filtered data set.

> [!NOTE]
> `autocluster` is largely based on the Seed-Expand algorithm from the following paper: [Algorithms for Telemetry Data Mining using Discrete Attributes](https://www.scitepress.org/DigitalLibrary/PublicationsDetail.aspx?ID=d5kcrO+cpEU=&t=1). 


## Syntax

`T | evaluate autocluster(` *arguments* `)`

## Returns

The `autocluster` plugin returns a (usually small) set of patterns. The patterns capture portions of the data with shared common values across multiple discrete attributes. Each pattern in the results is represented by a row.

The first column is the segment ID. The next two columns are the count and percentage of rows out of the original query that are captured by the pattern. The remaining columns are from the original query. Their value is either a specific value from the column, or a wildcard value (which are by default null) meaning variable values.

The patterns aren't distinct, may be overlapping, and usually don't cover all the original rows. Some rows may not fall under any pattern.

> [!TIP]
> Use [where](./whereoperator.md) and [project](./projectoperator.md) in the input pipe to reduce the data to just what you're interested in.
>
> When you find an interesting row, you might want to drill into it further by adding its specific values to your `where` filter.

## Arguments 

> [!NOTE] 
> All arguments are optional.

`T | evaluate autocluster(`[*SizeWeight*, *WeightColumn*, *NumSeeds*, *CustomWildcard*, *CustomWildcard*, ...]`)`

All arguments are optional, but they must be ordered as above. To indicate that the default value should be used, put the string tilde value '~' (see the "Example" column in the table).

|Argument        | Type, range, default              |Description                | Example                                        |
|----------------|-----------------------------------|---------------------------|------------------------------------------------|
| SizeWeight     | 0 < *double* < 1 [default: 0.5]   | Gives you some control over the balance between generic (high coverage) and informative (many shared) values. If you increase the value, it usually reduces the number of patterns, and each pattern tends to cover a larger percentage coverage. If you decrease the value, it usually produces more specific patterns with more shared values, and a smaller percentage coverage. The under-the-hood formula is a weighted geometric mean, between the normalized generic score and the informative score with weights `SizeWeight` and `1-SizeWeight`                   | `T | evaluate autocluster(0.8)`                |
|WeightColumn    | *column_name*                     | Considers each row in the input according to the specified weight (by default each row has a weight of '1'). The argument must be a name of a numeric column (such as int, long, real). A common usage of a weight column is to take into account sampling or bucketing/aggregation of the data that is already embedded into each row.                                                                                                       | `T | evaluate autocluster('~', sample_Count)` | 
| NumSeeds        | *int* [default: 25]              | The number of seeds determines the number of initial local search points of the algorithm. In some cases, depending on the structure of the data and if you increase the number of seeds, then the number (or quality) of the results increases through the expanded search space with a slower query tradeoff. The value has diminishing results in both directions, so if you decrease it to below five, it will achieve negligible performance improvements. If you increase to above 50, it will rarely generate additional patterns.                                         | `T | evaluate autocluster('~', '~', 15)`       |
| CustomWildcard  | *"any_value_per_type"*           | Sets the wildcard value for a specific type in the results table. It will indicate that the current pattern doesn't have a restriction on this column. The default is null, since the string default is an empty string. If the default is a good value in the data, a different wildcard value should be used (such as `*`).                                                                                                                | `T | evaluate autocluster('~', '~', '~', '*', int(-1), double(-1), long(0), datetime(1900-1-1))` |

## Examples

### Using autocluster

<!-- csl: https://help.kusto.windows.net:443/Samples -->
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

### Using custom wildcards

<!-- csl: https://help.kusto.windows.net:443/Samples -->
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

## See also

* [basket](./basketplugin.md)
* [reduce](./reduceoperator.md)
