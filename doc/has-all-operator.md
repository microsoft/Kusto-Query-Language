---
title:  The case-insensitive has_all string operator
description: Learn how to use the has_all string operator to filter a record set for data with one or more case-insensitive search strings.
ms.reviewer: alexans
ms.topic: reference
ms.date: 03/29/2023
---
# has_all operator

Filters a record set for data with one or more case-insensitive search strings. `has_all` searches for indexed terms, where an indexed [term](datatypes-string-operators.md#what-is-a-term) is three or more characters. If your term is fewer than three characters, the query scans the values in the column, which is slower than looking up the term in the term index.

For more information about other operators and to determine which operator is most appropriate for your query, see [datatype string operators](datatypes-string-operators.md).

## Syntax

*T* `|` `where` *col* `has_all` `(`*expression*`,` ... `)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *T* | string | &check; | The tabular input to filter.|
| *col* | string | &check; | The column by which to filter.|
| *expression* | scalar or tabular | &check; |An expression that specifies the values for which to search. Each expression can be a [scalar value](scalar-data-types/index.md) or a [tabular expression](tabularexpressionstatements.md) that produces a set of values. If a tabular expression has multiple columns, the first column is used. The search will consider up to 256 distinct values.|

## Returns

Rows in *T* for which the predicate is `true`.

## Examples

### Set of scalars

The following query shows how to use `has_all` with a comma-separated set of scalar values.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAx2NsQ7CMAxEd77CytRKbMxMqCsL7Mi0hkRK7Mh2i4r4eJoup7vT093NRcuwELvB4QefSEow1GQy0RVV0dNCENEemDN0YZQ8hSMEcxV+N4cvJ2URbiFiyqHfhmwuBTV9CS4ys5/Hpl0PzxX2t/taacNcKpxauVN/jkW/jI0AAAA=" target="_blank">Run the query</a>

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

### Dynamic array

The same result can be achieved using a dynamic array notation.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAx2NsQoCMRBEe8F/WFJdwM7aSq610U4OWXOrF0h2j83eScSPl1wzzAyPmauJ5n4ltgL73Q8+EylBP8ciI11QFS2uBBOWB6YE3VgZcwzd3QVJozuAK6bC7+bwZaQswi1MGJMbvG+jZckZNX4JzrKwnULTzsOzwnZ9qzM1zmSGY2s37A8IXzPSmwAAAA==" target="_blank">Run the query</a>

```kusto
StormEvents 
| where EpisodeNarrative has_all (dynamic(["cold", "strong", "afternoon", "hail"]))
| summarize Count=count() by EventType
| top 3 by Count
```

**Output**

|EventType|Count|
|---|---|
|Thunderstorm Wind|517|
|Hail|392|
|Flash Flood|24|

The same query can also be written with a [let statement](letstatement.md).

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAzWNsQrCQBBE+3zFclUCdpaSStLaaCci62U1C3e3YW8Tifjx5gI2w8zwmAlk4JWNlBFa6JeEkX19dV5C73bgsqmkV3H4XKkkkkoYkIO7NYfqbKKxmylZhuoL74GUoBs5S08nVEXjmWDAfMcQoP5/NSubpxhR+UNwlClZ64vWDTwW2AYvy0grZjLCvpQb9QMQPEgrsQAAAA==" target="_blank">Run the query</a>

```kusto
let criteria = dynamic(["cold", "strong", "afternoon", "hail"]);
StormEvents 
| where EpisodeNarrative has_all (criteria)
| summarize Count=count() by EventType
| top 3 by Count
```

|EventType|Count|
|---|---|
|Thunderstorm Wind|517|
|Hail|392|
|Flash Flood|24|
