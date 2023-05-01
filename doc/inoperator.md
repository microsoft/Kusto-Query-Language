---
title: The case-insensitive in~ string operator - Azure Data Explorer
description: Learn how to use the in~ operator to filter data with a case-insensitive string.
ms.reviewer: alexans
ms.topic: reference
ms.date: 03/29/2023
---
# in~ operator

Filters a record set for data with a case-insensitive string.

[!INCLUDE [in-operator-comparison](../../includes/in-operator-comparison.md)]

## Performance tips

[!INCLUDE [performance-tip-note](../../includes/performance-tip-note.md)]

When possible, use the case-sensitive [in](in-cs-operator.md).

## Syntax

*T* `|` `where` *col* `in~` `(`*expression*`,` ... `)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *T* | string | &check; | The tabular input to filter.|
| *col* | string | &check; | The column by which to filter.|
| *expression* | scalar or tabular | &check; | An expression that specifies the values for which to search. Each expression can be a [scalar](scalar-data-types/index.md) value or a [tabular expression](tabularexpressionstatements.md) that produces a set of values. If a tabular expression has multiple columns, the first column is used. The search will consider up to 1,000,000 distinct values.|

> [!NOTE]
> An inline tabular expression must be enclosed with double parentheses. See [example](#tabular-expression).

## Returns

Rows in *T* for which the predicate is `true`.

## Examples

### List of scalars

The following query shows how to use `in~` with a comma-separated list of scalar values.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSspVuCqUSjPSC1KVQguSSxJVcjMq1PQUHLz8Q/ydHFU0lFQSk/NL0rPTAQx/VzDFSL9g7yVNEG6kvNL80oAl8ORJUoAAAA=" target="_blank">Run the query</a>

```kusto
StormEvents 
| where State in~ ("FLORIDA", "georgia", "NEW YORK") 
| count
```

**Output**

|Count|
|---|
|4775|  

### Dynamic array

The following query shows how to use `in~` with a dynamic array.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSspVuCqUSjPSC1KVQguSSxJVcjMq1PQSKnMS8zNTNaIVnLz8Q/ydHFU0lFQSk/NL0rPTAQx/VzDFSL9g7yVYjU1QQYk55fmlQAAcLCM41UAAAA=" target="_blank">Run the query</a>

```kusto
StormEvents 
| where State in~ (dynamic(["FLORIDA", "georgia", "NEW YORK"])) 
| count
```

**Output**

|Count|
|---|
|4775|  

The same query can also be written with a [let statement](letstatement.md).

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA8tJLVEoLkksSS1WsFVIqcxLzM1M1ohWcvPxD/J0cVTSUVBKT80vSs9MBDH9XMMVIv2DvJViNa25gkvyi3Jdy1LzSooVuGoUyjNSi1IVgkFGKWQkFscn5lUqaEBM1gRKF5fm5iYWZValKiTnl+aVaGgqJFVCVAMACG2BiYIAAAA=" target="_blank">Run the query</a>

```kusto
let states = dynamic(["FLORIDA", "georgia", "NEW YORK"]);
StormEvents 
| where State has_any (states)
| summarize count() by State
```

**Output**

|Count|
|---|
|4775|

### Tabular expression

The following query shows how to use `in~` with an inline tabular expression. Notice that an inline tabular expression must be enclosed with double parentheses.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSspVuCqUSjPSC1KVQguSSxJVcjMq1PQ0AjILyjNSSzJzM9zSSxJVIApQQgr2CmYGoABULKgKD8rNbkEYoKmJtDE4tLc3MSizKpUheT80rwSDU2FpEqINACuenXZewAAAA==" target="_blank">Run the query</a>

```kusto
StormEvents 
| where State in~ ((PopulationData | where Population > 5000000 | project State))
| summarize count() by State
```

**Output**

|State|count_|
|--|--|
|TEXAS |4701|
|ILLINOIS |2022|
|MISSOURI |2016|
|GEORGIA |1983|
|MINNESOTA |1881|
|...|...|

The same query can also be written with a [let statement](letstatement.md). Notice that the double parentheses as provided in the last example aren't necessary in this case.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA02NsQ6CQBBEe75iSuhorIxW0pvwAWYlGz1yd0v29jAaw7eDEBOmnHkv49ngSR98S0bGCSdcZciezEm8kBG+eD1ZeVfjjEO9ZhkHlZ47Q/vTj0VroqEZOVpC8VfXDS5OKPdf1QKkHAKp+zA6ydHKCvf3xs9WLYRGmgAAAA==" target="_blank">Run the query</a>

```kusto
let large_states = PopulationData | where Population > 5000000 | project State;
StormEvents 
| where State in~ (large_states)
| summarize count() by State
```

**Output**

|State|count_|
|--|--|
|TEXAS |4701|
|ILLINOIS |2022|
|MISSOURI |2016|
|GEORGIA |1983|
|MINNESOTA |1881|
|...|...|
