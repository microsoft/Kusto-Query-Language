---
title: basket plugin - Azure Data Explorer
description: Learn how to use the basket plugin to find frequent patterns in data that exceed a frequency threshold. 
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/07/2022
---
# basket plugin

The `basket` plugin finds frequent patterns of attributes in the data and returns the patterns that pass a frequency threshold in that data. A pattern represents a subset of the rows that have the same value across one or more columns. The `basket` plugin is based on the [Apriori algorithm](https://en.wikipedia.org/wiki/Association_rule_learning#Apriori_algorithm) originally developed for basket analysis data mining.

## Syntax

*T* | `evaluate` `basket` `(` [*Threshold*, *WeightColumn*, *MaxDimensions*, *CustomWildcard*, *CustomWildcard*, ...]`)`

## Arguments

All arguments are optional, but they must be ordered as above. To indicate that the default value should be used, use the string tilde value - '~'. See examples below.

Available arguments:

* Threshold - 0.015 < *double* < 1 [default: 0.05]

    Sets the minimal ratio of the rows to be considered frequent. Patterns with a smaller ratio won't be returned.

    Example: `T | evaluate basket(0.02)`

* WeightColumn - *column_name*

    Considers each row in the input according to the specified weight. By default, each row has a weight of '1'. The argument must be a name of a numeric column, such as int, long, real. A common use of a weight column, is to take into account sampling or bucketing/aggregation of the data that is already embedded into each row.

    Example: `T | evaluate basket('~', sample_Count)`

* MaxDimensions - 1 < *int* [default: 5]

    Sets the maximal number of uncorrelated dimensions per basket, limited by default, to minimize the query runtime.

    Example: `T | evaluate basket('~', '~', 3)`

* CustomWildcard - *"any_value_per_type"*

    Sets the wildcard value for a specific type in the result table that will indicate that the current pattern doesn't have a restriction on this column.
    Default is null. The default for a string is an empty string. If the default is a good value in the data, a different wildcard value should be used, such as `*`.

    For example:

     `T | evaluate basket('~', '~', '~', '*', int(-1), double(-1), long(0), datetime(1900-1-1))`

## Returns

The `basket` plugin returns frequent patterns that pass a ratio threshold. The default threshold is 0.05.

Each pattern is represented by a row in the results. The first column is the segment ID. The next two columns are the *count* and *percentage of rows*, from the original query that match the pattern. The remaining columns relate to the original query, with either a specific value from the column or a wildcard value, which is by default null, meaning a variable value.

**Notes**
The algorithm uses sampling to determine the initial frequent values. Therefore, the results could slightly differ between multiple runs for patterns whose frequency is close to the threshold.

## Example

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
StormEvents
| where monthofyear(StartTime) == 5
| extend Damage = iff(DamageCrops + DamageProperty > 0 , "YES" , "NO")
| project State, EventType, Damage, DamageCrops
| evaluate basket(0.2)
```

**Output**

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

## Example with custom wildcards

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
StormEvents
| where monthofyear(StartTime) == 5
| extend Damage = iff(DamageCrops + DamageProperty > 0 , "YES" , "NO")
| project State, EventType, Damage, DamageCrops
| evaluate basket(0.2, '~', '~', '*', int(-1))
```

**Output**

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
