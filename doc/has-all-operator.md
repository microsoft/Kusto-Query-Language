---
title: The case-insensitive has_all string operator - Azure Data Explorer
description: Learn how to use the has_all string operator to filter a record set for data with one or more case-insensitive search strings.
ms.reviewer: alexans
ms.topic: reference
ms.date: 12/18/2022
---
# has_all operator

Filters a record set for data with one or more case-insensitive search strings. `has` searches for indexed terms, where a [term](datatypes-string-operators.md#what-is-a-term) is three or more characters. If your term is fewer than three characters, the query scans the values in the column, which is slower than looking up the term in the term index.

For more information about other operators and to determine which operator is most appropriate for your query, see [datatype string operators](datatypes-string-operators.md).

## Performance tips

> [!NOTE]
> Performance depends on the type of search and the structure of the data.

For faster results, use the case-sensitive version of an operator, for example, `has_cs`, not `has`. For best practices, see [Query best practices](best-practices.md).

## Syntax

*T* `|` `where` *Column* `has_all` `(`*list of scalar expressions*`)`
*T* `|` `where` *Column* `has_all` `(`*tabular expression*`)`

## Arguments

* *T*: Tabular input whose records are to be filtered.
* *Column*: Column to filter.
* *list of expressions*: Comma separated list of tabular, scalar, or literal expressions.  
* *tabular expression*: Tabular expression that has a set of values (if expression has multiple columns, the first column is used).

## Returns

Rows in *T* for which the predicate is `true`

> [!NOTE]
>
> * The expression list can produce up to `256` values.
> * For tabular expressions, the first column of the result set is selected.

## Examples

### Use has_all operator with a list

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
StormEvents 
| where EpisodeNarrative has_all ("cold", "strong", "afternoon", "hail")
| summarize Count=count() by EventType
| top 3 by Count
```

**Output**

|EventType|Count|
|---|---|
|Thunderstorm Wind|517|
|Hail|392|
|Flash Flood|24|

### Use has_all operator with a dynamic array

The same result can be achieved using a dynamic array notation:

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
let keywords = dynamic(["cold", "strong", "afternoon", "hail"]);
StormEvents 
| where EpisodeNarrative has_all (keywords)
| summarize Count=count() by EventType
| top 3 by Count
```

**Output**

|EventType|Count|
|---|---|
|Thunderstorm Wind|517|
|Hail|392|
|Flash Flood|24|
