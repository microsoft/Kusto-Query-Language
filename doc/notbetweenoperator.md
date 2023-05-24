---
title:  The !between operator
description: Learn how to use the !between operator to match the input that is outside of the inclusive range.
ms.reviewer: alexans
ms.topic: reference
ms.date: 03/12/2023
---
# !between operator

Matches the input that is outside of the inclusive range.

`!between` can operate on any numeric, datetime, or timespan expression.

## Syntax

*T* `|` `where` *expr* `!between` `(`*leftRange*` .. `*rightRange*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *T* | string | &check;| The tabular input whose records are to be matched.|
| *expr* | scalar | &check; | The expression to filter.|
| *leftRange* | int, long, real, or datetime | &check; | The expression of the left range. The range is inclusive.|
| *rightRange* | int, long, real, datetime, or timespan | &check; | The expression of the right range. The range is inclusive.<br/><br/>This value can only be of type [timespan](scalar-data-types/timespan.md) if *expr* and *leftRange* are both of type `datetime`. See [example](#filter-datetime-using-a-timespan-range).|

## Returns

Rows in *T* for which the predicate of (*expr* < *leftRange* or *expr* > *rightRange*) evaluates to `true`.

## Examples  

### Filter numeric values

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAytKzEtPVahQSCvKz1UwVCjJVzA0UCguSS1QMOSqUSjPSC0CySompZaUp6bmKWiYKujpKVhqAgAyiN4KNwAAAA==" target="_blank">Run the query</a>

```kusto
range x from 1 to 10 step 1
| where x !between (5 .. 9)
```

**Output**

|x|
|---|
|1|
|2|
|3|
|4|
|10|

### Filter datetime  

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSsp5qpRKM9ILUpVCC5JLCoJycxNVVBMSi0pT03NU9BISSxJLQEKaRgZGJjrApGRuaaCnp4ChrixgaYm0KTk/NK8EgDn7tLlXQAAAA==" target="_blank">Run the query</a>

```kusto
StormEvents
| where StartTime !between (datetime(2007-07-27) .. datetime(2007-07-30))
| count 
```

**Output**

|Count|
|---|
|58590|

### Filter datetime using a timespan range

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSsp5qpRKM9ILUpVCC5JLCoJycxNVVBMSi0pT03NU9BISSxJLQEKaRgZGJjrApGRuaaCnp6CcYomUF9yfmleCQDBjXU5SwAAAA==" target="_blank">Run the query</a>

```kusto
StormEvents
| where StartTime !between (datetime(2007-07-27) .. 3d)
| count 
```

**Output**

|Count|
|---|
|58590|
