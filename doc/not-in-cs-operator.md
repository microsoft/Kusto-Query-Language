---
title:  The case-sensitive !in string operator
description: Learn how to use the !in string operator to filter records for data without a case-sensitive string.
ms.reviewer: alexans
ms.topic: reference
ms.date: 03/29/2023
---
# !in operator

Filters a record set for data without a case-sensitive string.

[!INCLUDE [in-operator-comparison](../../includes/in-operator-comparison.md)]

## Performance tips

[!INCLUDE [performance-tip-note](../../includes/performance-tip-note.md)]

## Syntax

*T* `|` `where` *col* `!in` `(`*expression*`,` ... `)`

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

## Example

### List of scalars

The following query shows how to use `!in` with a comma-separated list of scalar values.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSspVuCqUSjPSC1KVQguSSxJVVDMzFPQUHLz8Q/ydHFU0lFQcnf1D3L3BDP9XMMVIv2DvJU0QbqS80vzSgDNxq+9SgAAAA==" target="_blank">Run the query</a>

```kusto
StormEvents 
| where State !in ("FLORIDA", "GEORGIA", "NEW YORK") 
| count
```

**Output**

|Count|
|---|
|54291|

### Dynamic array

The following query shows how to use `!in` with a dynamic array.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSspVuCqUSjPSC1KVQguSSxJVVDMzFPQSKnMS8zNTNaIVnLz8Q/ydHFU0lFQcnf1D3L3BDP9XMMVIv2DvJViNTVBBiTnl+aVAAD4lvyYVQAAAA==" target="_blank">Run the query</a>

```kusto
StormEvents 
| where State !in (dynamic(["FLORIDA", "GEORGIA", "NEW YORK"])) 
| count
```

**Output**

|Count|
|---|
|54291|  

The same query can also be written with a [let statement](letstatement.md).

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA8tJLVEoLkksSS1WsFVIqcxLzM1M1ohWcvPxD/J0cVTSUVByd/UPcvcEM/1cwxUi/YO8lWI1rbmCS/KLcl3LUvNKihW4ahTKM1KLUhWCQUYpKGbmKWhATNUEShWX5uYmFmVWpSok55fmlWhoKiRVQlQCAKFqvAF+AAAA" target="_blank">Run the query</a>

```kusto
let states = dynamic(["FLORIDA", "GEORGIA", "NEW YORK"]);
StormEvents 
| where State !in (states)
| summarize count() by State
```

**Output**

|Count|
|---|
|54291|

### Tabular expression

The following query shows how to use `!in` with an inline tabular expression. Notice that an inline tabular expression must be enclosed with double parentheses.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSspVuCqUSjPSC1KVQguSSxJVVDMzFPQ0AjILyjNSSzJzM9zSSxJVIApQQgr2CmYGoABULKgKD8rNbkEYoKmJtDE4tLc3MSizKpUheT80rwSDU2FpEqINABw+yCTewAAAA==" target="_blank">Run the query</a>

```kusto
StormEvents 
| where State !in ((PopulationData | where Population > 5000000 | project State))
| summarize count() by State
```

**Output**

|State|Count|
|--|--|
|KANSAS|3166|
|IOWA|2337|
|NEBRASKA|1766|
|OKLAHOMA|1716|
|SOUTH DAKOTA|1567|
|...|...|

The same query can also be written with a [let statement](letstatement.md). Notice that the double parentheses as provided in the last example aren't necessary in this case.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA02NvQrCQBCE+zzF2CWdjZXEKvZCHkDWsCQnd7dhby+i+PD5ESFTznwf49ngSXu+JyPjhBo3GbMncxIbMsIXr4GVdzUuOB23LOOo8uTO0K76uWhNNFwnjpZQ/NVtw8FFlPuvagFSDoHUfRid5Ghlhcf7x89eufmimgAAAA==" target="_blank">Run the query</a>

```kusto
let large_states = PopulationData | where Population > 5000000 | project State;
StormEvents 
| where State !in (large_states)
| summarize count() by State
```

**Output**

|State|Count|
|--|--|
|KANSAS|3166|
|IOWA|2337|
|NEBRASKA|1766|
|OKLAHOMA|1716|
|SOUTH DAKOTA|1567|
|...|...|
