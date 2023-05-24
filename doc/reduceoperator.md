---
title:  reduce operator
description: Learn how to use the reduce operator to group a set of strings together based on value similarity.
ms.reviewer: alexans
ms.topic: reference
ms.date: 03/22/2023
---
# reduce operator

Groups a set of strings together based on value similarity.

For each such group, the operator returns a `pattern`, `count`, and `representative`. The `pattern` best describes the group, in which the `*` character represents a wildcard. The `count` is the number of values in the group, and the `representative` is one of the original values in the group.

## Syntax

*T* `|` `reduce` [`kind` `=` *ReduceKind*] `by` *Expr* [`with` [`threshold` `=` *Threshold*] [`,` `characters` `=` *Characters*]]

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *Expr* | string | &check; | The value by which to reduce.|
| *Threshold* | real | | A value between 0 and 1 that determines the minimum fraction of rows required to match the grouping criteria in order to trigger a reduction operation. The default value is 0.1.<br/><br/>We recommend setting a small threshold value for large inputs. With a smaller threshold value, more similar values are grouped together, resulting in fewer but more similar groups. A larger threshold value requires less similarity, resulting in more groups that are less similar. See [Examples](#examples).|
| *Characters* | string | | A list of characters that separate between terms. The default is every non-ascii numeric character. For examples, see [Behavior of Characters parameter](#behavior-of-characters-parameter).|
| *ReduceKind* | string | | The only valid value is `source`. If `source` is specified, the operator appends the `Pattern` column to the existing rows in the table instead of aggregating by `Pattern`.|

## Returns

A table with as many rows as there are groups and columns titled `pattern`, `count`, and `representative`. The `pattern` best describes the group, in which the `*` character represents a wildcard, or placeholder for an arbitrary insertion string. The `count` is the number of values in the group, and the `representative` is one of the original values in the group.

For example, the result of `reduce by city` might include:

|Pattern     |Count |Representative|
|------------|------|--------------|
| San *      | 5182 |San Bernard   |
| Saint *    | 2846 |Saint Lucy    |
| Moscow     | 3726 |Moscow        |
| \* -on- \* | 2730 |One -on- One  |
| Paris      | 2716 |Paris         |

## Examples

### Small threshold value

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAzWNMQ7CMBAEe16xcmWkCNkPyA9IR5H2cI7YCOzochGJxOO5hq1WGu2sUJ0ZOx7S3ojQhhhCwKq8IJ6+WKQ9OSmG48a7ojciidS7gVIula9MUkudR9fZ2KB1r61U9UJ18jGcLSYSnrbEuB9/FT5FMzQLr7m9pj5cQojokDIJJWVZ7c2N7gefKNUWogAAAA==" target="_blank">Run the query</a>

```kusto
range x from 1 to 1000 step 1
| project MyText = strcat("MachineLearningX", tostring(toint(rand(10))))
| reduce by MyText  with threshold=0.001 , characters = "X" 
```

**Output**

|Pattern         |Count|Representative   |
|----------------|-----|-----------------|
|MachineLearning*|1000 |MachineLearningX4|

### Large threshold value

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/SampleIoTData?query=H4sIAAAAAAAAAzWNMQ7CMBAEe16xcmWkCNklRX5AOoq0h3PERmBHl0MkEo/nGrZaabSzQnVmbLhLeyFCG2IIAavygnj4YpH24KQY9itvit6IJFLvBkq5VL4wSS11Hl1nY4PWvbZS1QvVycdwtJhIeHonxm3/q/ApmqFZeM3tOfXhdEaHlEkoKctqX250Px4hVlGgAAAA" target="_blank">Run the query</a>

```kusto
range x from 1 to 1000 step 1
| project MyText = strcat("MachineLearningX", tostring(toint(rand(10))))
| reduce by MyText  with threshold=0.9 , characters = "X" 
```

**Output**

|Pattern         |Count|Representative   |
|----------------|-----|-----------------|
|MachineLearning*|177|MachineLearningX9|
|MachineLearning*|102|MachineLearningX0|
|MachineLearning*|106|MachineLearningX1|
|MachineLearning*|96|MachineLearningX6|
|MachineLearning*|110|MachineLearningX4|
|MachineLearning*|100|MachineLearningX3|
|MachineLearning*|99|MachineLearningX8|
|MachineLearning*|104|MachineLearningX7|
|MachineLearning*|106|MachineLearningX2|

### Behavior of Characters parameter

If the *Characters* parameter is unspecified, then every non-ascii numeric character will be a term separator.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAxWKQQqAMAwEv7L0VMGDfYAf8VZrKgo2JUao4ONNLwMzjMSyExqy8IUAZYQJt1I1+VCFT0pqQTB3pqjeZWY3wi0GZYtH2X0bBvuFticR1re/P7YelSlbAAAA" target="_blank">Run the query</a>

```kusto
range x from 1 to 10 step 1 | project str = strcat("foo", "Z", tostring(x)) | reduce by str
```

**Output**

|Pattern|Count|Representative|
|--|--|--|
|others|10||

However, if you specify that "Z" is a separator, then it's as if each value in `str` is 2 terms: `foo` and `tostring(x)`:

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAx2LSQqAQAwEv9LMyQEP+gA/4m2McQMnEiMq+Hijl4IuujTlkXFhUFlRwwR1hd148/FgU1mYzIWi+UjJijCIhBKhdZi4nPNYXDH6X7k/iNHdf3HONoGmpImMdW+8eAEwpEQbbwAAAA==" target="_blank">Run the query</a>

```kusto
range x from 1 to 10 step 1 | project str = strcat("foo", "Z", tostring(x)) | reduce by str with characters="Z"
```

**Output**

|Pattern|Count|Representative|
|--|--|--|
|foo*|10|fooZ1|

### Apply `reduce` to sanitized input

The following example shows how one might apply the `reduce` operator to a "sanitized"
input, in which GUIDs in the column being reduced are replaced prior to reducing

```kusto
// Start with a few records from the Trace table.
Trace | take 10000
// We will reduce the Text column which includes random GUIDs.
// As random GUIDs interfere with the reduce operation, replace them all
// by the string "GUID".
| extend Text=replace_regex(Text, @"[[:xdigit:]]{8}-[[:xdigit:]]{4}-[[:xdigit:]]{4}-[[:xdigit:]]{4}-[[:xdigit:]]{12}", @"GUID")
// Now perform the reduce. In case there are other "quasi-random" identifiers with embedded '-'
// or '_' characters in them, treat these as non-term-breakers.
| reduce by Text with characters="-_"
```

## See also

[autocluster](./autoclusterplugin.md)

> [!NOTE]
> The implementation of `reduce` operator is largely based on the paper [A Data Clustering Algorithm for Mining Patterns From Event Logs](https://ristov.github.io/publications/slct-ipom03-web.pdf), by Risto Vaarandi.
