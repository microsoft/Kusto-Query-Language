---
title: The between operator - Azure Data Explorer
description: Learn how to use the between operator to return a record set of values in an inclusive range for which the predicate evaluates to true. 
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/20/2022
---
# between operator

Filters a record set for data matching the values in an inclusive range.

`between` can operate on any numeric, datetime, or timespan expression.

## Syntax

*T* `|` `where` *expr* `between` `(`*leftRange*` .. `*rightRange*`)`

If *expr* expression is datetime - another syntactic sugar syntax is provided:

*T* `|` `where` *expr* `between` `(`*leftRangeDateTime*` .. `*rightRangeTimespan*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *T* | string | &check; |  The tabular input whose records are to be matched. For example, the table name. |
| *expr* | string | &check; |  The expression used to filter. |
| *leftRange* | string | &check; |  The expression of the left range (inclusive). |
| *rightRange* | string | &check; |  The expression of the right range (inclusive). |

## Returns

Rows in *T* for which the predicate of (*expr* >= *leftRange* and *expr* <= *rightRange*) evaluates to `true`.

## Examples

### Filter numeric values

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAytKzEtPVahQSCvKz1UwVCjJVzA0MFAoLkktUDDk5apRKM9ILQLJJ6WWlKem5ilomBoo6OkpmJpqAgBfXYZBOgAAAA==" target="_blank">Run the query</a>

```kusto
range x from 1 to 100 step 1
| where x between (50 .. 55)
```

**Output**

|x|
|---|
|50|
|51|
|52|
|53|
|54|
|55|

### Filter datetime

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSsp5uWqUSjPSC1KVQguSSwqCcnMTVVISi0pT03NU9BISSxJLQGKaBgZGJjrApGRuaaCnp4ChrixgaYmyKTk/NK8EgBluyagXgAAAA==" target="_blank">Run the query</a>

```kusto
StormEvents
| where StartTime between (datetime(2007-07-27) .. datetime(2007-07-30))
| count
```

**Output**

|Count|
|---|
|476|

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSsp5uWqUSjPSC1KVQguSSwqCcnMTVVISi0pT03NU9BISSxJLQGKaBgZGJjrApGRuaaCnp6CcYomSF9yfmleCQCGAqjRTAAAAA==" target="_blank">Run the query</a>

```kusto
StormEvents
| where StartTime between (datetime(2007-07-27) .. 3d)
| count
```

**Output**

|Count|
|---|
|476|
