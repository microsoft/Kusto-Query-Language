---
title: basket plugin - Azure Data Explorer | Microsoft Docs
description: This article describes basket plugin in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 05/26/2019
---
# basket plugin

```kusto
T | evaluate basket()
```

Basket finds all frequent patterns of discrete attributes (dimensions) in the data and will return all frequent patterns that passed the frequency threshold in the original query. Basket is guaranteed to find all frequent patterns in the data but is not guaranteed to have polynomial run-time, the run-time of the query is linear in the number of rows but in some cases might be exponential in the number of columns (dimensions). Basket is based on the Apriori algorithm originally developed for basket analysis data mining.

**Syntax**

`T | evaluate basket(` *arguments* `)`

**Returns**

Basket returns all frequent patterns appearing in above the ratio threshold (default: 0.05) of the rows Each pattern is represented by a row in the results.

The first column is the segment Id. The next two columns are the count and percentage of rows out of the original query that are captured by the pattern. The remaining columns are from the original query and their value is either a specific value from the column or a wildcard value (which are by default null) meaning variable values.

**Arguments (all optional)***

`T | evaluate basket(`[*Threshold*, *WeightColumn*, *MaxDimensions*, *CustomWildcard*, *CustomWildcard*, ...]`)`

All arguments are optional, but they must be ordered as above. To indicate that the default value should be used, put the string tilde value - '~' (see examples below).

Available arguments:

* Threshold - 0.015 < *double* < 1 [default: 0.05]

    Sets the minimal ratio of the rows to be considered frequent (patterns with smaller ratio will not be returned).
    
    Example: `T | evaluate basket(0.02)`

* WeightColumn - *column_name*

    Considers each row in the input according to the specified weight (by default each row has a weight of '1'). The argument must be a name of a numeric column (e.g. int, long, real). A common usage of a weight column is to take into account sampling or bucketing/aggregation of the data that is already embedded into each row.
    
    Example: `T | evaluate basket('~', sample_Count)`

* MaxDimensions - 1 < *int* [default: 5]

    Sets the maximal number of uncorrelated dimensions per basket, limited by default to decrease the query runtime.

    Example: `T | evaluate basket('~', '~', 3)`

* CustomWildcard - *"any_value_per_type"*

    Sets the wildcard value for a specific type in the result table that will indicate that the current pattern doesn't have a restriction on this column.
    Default is null, for string default is an empty string. If the default is a viable value in the data, a different wildcard value should be used (e.g. `*`).
    See an example below.

    Example: `T | evaluate basket('~', '~', '~', '*', int(-1), double(-1), long(0), datetime(1900-1-1))`


**Example**

```kusto
StormEvents 
| where monthofyear(StartTime) == 5
| extend Damage = iff(DamageCrops + DamageProperty > 0 , "YES" , "NO")
| project State, EventType, Damage, DamageCrops
| evaluate basket(0.2)
```
|SegmentId|Count|Percent|State|EventType|Damage|DamageCrops|
|---|---|---|---|---|---|---|---|---|
|0|4574|77.7|||NO|0
|1|2278|38.7||Hail|NO|0
|2|5675|96.4||||0
|3|2371|40.3||Hail||0
|4|1279|21.7||Thunderstorm Wind||0
|5|2468|41.9||Hail||
|6|1310|22.3|||YES|
|7|1291|21.9||Thunderstorm Wind||

**Example with custom wildcards**
```kusto
StormEvents 
| where monthofyear(StartTime) == 5
| extend Damage = iff(DamageCrops + DamageProperty > 0 , "YES" , "NO")
| project State, EventType, Damage, DamageCrops
| evaluate basket(0.2, '~', '~', '*', int(-1))
```
|SegmentId|Count|Percent|State|EventType|Damage|DamageCrops|
|---|---|---|---|---|---|---|---|---|
|0|4574|77.7|\*|\*|NO|0
|1|2278|38.7|\*|Hail|NO|0
|2|5675|96.4|\*|\*|\*|0
|3|2371|40.3|\*|Hail|\*|0
|4|1279|21.7|\*|Thunderstorm Wind|\*|0
|5|2468|41.9|\*|Hail|\*|-1
|6|1310|22.3|\*|\*|YES|-1
|7|1291|21.9|\*|Thunderstorm Wind|\*|-1

