---
title:  union operator
description: This article describes union operator in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 02/13/2020
zone_pivot_group_filename: data-explorer/zone-pivot-groups.json
zone_pivot_groups: kql-flavors-all
---
# union operator

Takes two or more tables and returns the rows of all of them.

## Syntax

[ *T* `|` ] `union` [ *UnionParameters* ] [`kind=` `inner`|`outer`] [`withsource=` *ColumnName*] [`isfuzzy=` `true`|`false`] *Tables*

> [!NOTE]
> The operation of the `union` operator can be altered by setting the `best_effort` request property to `true`, using either a [set statement](./setstatement.md) or through [client request properties](../api/netfx/request-properties.md). When this property is set to `true`, the `union` operator will disregard fuzzy resolution and connectivity failures to execute any of the sub-expressions being “unioned” and yield a warning in the query status results.

## Parameters

::: zone pivot="azuredataexplorer, fabric"

|Name|Type|Required|Description|
|--|--|--|--|
|*T*|string||The input tabular expression.|
|*UnionParameters*|string||Zero or more space-separated parameters in the form of *Name* `=` *Value* that control the behavior of the row-match operation and execution plan. See [supported union parameters](#supported-union-parameters).|
|`kind`|string||Either `inner` or `outer`. `inner` causes the result to have the subset of columns that are common to all of the input tables. `outer` causes the result to have all the columns that occur in any of the inputs. Cells that aren't defined by an input row are set to `null`. The default is `outer`.<br/><br/>With `outer`, the result has all the columns that occur in any of the inputs, one column for each name and type occurrences. This means that if a column appears in multiple tables and has multiple types, it has a corresponding column for each type in the union's result. This column name is suffixed with a '_' followed by the origin column [type](./scalar-data-types/index.md).|
|`withsource=`*ColumnName*|string||If specified, the output includes a column called *ColumnName* whose value indicates which source table has contributed each row. If the query effectively references tables from more than one database including the default database, then the value of this column has a table name qualified with the database. __cluster and database__ qualifications are present in the value if more than one cluster is referenced.|
|`isfuzzy`|bool||If set to `true`, allows fuzzy resolution of union legs. The set of union sources is reduced to the set of table references that exist and are accessible at the time while analyzing the query and preparing for execution. If at least one such table was found, any resolution failure yields a warning in the query status results, but won't prevent the query execution. If no resolutions were successful, the query returns an error. The default is `false`.<br/><br/>`isfuzzy=true` only applies to the `union` sources resolution phase. Once the set of source tables is determined, possible additional query failures won't be suppressed.|
|*Tables*|string||One or more comma-separated table references, a query expression enclosed with parenthesis, or a set of tables specified with a wildcard. For example, `E*` would form the union of all the tables in the database whose names begin `E`.|

### Supported union parameters

|Name|Type|Required|Description|
|--|--|--|--|
|`hint.concurrency`|int||Hints the system how many concurrent subqueries of the `union` operator should be executed in parallel. The default is the number of CPU cores on the single node of the cluster (2 to 16).|
|`hint.spread`|int||Hints the system how many nodes should be used by the concurrent `union` subqueries execution. The default is 1.|

::: zone-end

::: zone pivot="azuremonitor"

|Name|Type|Required|Description|
|--|--|--|--|
|*T*|string||The input tabular expression.|
|`kind`|string||Either `inner` or `outer`. `inner` causes the result to have the subset of columns that are common to all of the input tables. `outer` causes the result to have all the columns that occur in any of the inputs. Cells that aren't defined by an input row are set to `null`. The default is `outer`.<br/><br/>With `outer`, the result has all the columns that occur in any of the inputs, one column for each name and type occurrences. This means that if a column appears in multiple tables and has multiple types, it has a corresponding column for each type in the union's result. This column name is suffixed with a '_' followed by the origin column [type](./scalar-data-types/index.md).|
|`withsource=`*ColumnName*|string||If specified, the output includes a column called *ColumnName* whose value indicates which source table has contributed each row. If the query effectively references tables from more than one database including the default database, then the value of this column has a table name qualified with the database. __cluster and database__ qualifications are present in the value if more than one cluster is referenced.|
|`isfuzzy`|bool||If set to `true`, allows fuzzy resolution of union legs. The set of union sources is reduced to the set of table references that exist and are accessible at the time while analyzing the query and preparing for execution. If at least one such table was found, any resolution failure yields a warning in the query status results, but won't prevent the query execution. If no resolutions were successful, the query returns an error. However, in cross-workspace and cross-app queries, if any of the workspaces or apps is not found, the query will fail. The default is `false`.<br/><br/>`isfuzzy=true` only applies to the `union` sources resolution phase. Once the set of source tables is determined, possible additional query failures won't be suppressed.|
|*Tables*|string||One or more comma-separated table references, a query expression enclosed with parenthesis, or a set of tables specified with a wildcard. For example, `E*` would form the union of all the tables in the database whose names begin `E`.<br/><br/>Whenever the list of tables is known, refrain from using wildcards. Some workspaces contains very large number of tables that would lead to inefficient execution. Tables may also be added over time leading to unpredicted results.|

::: zone-end

> [!NOTE]
>
> * The `union` scope can include [let statements](./letstatement.md) if attributed with the `view` keyword.
> * The `union` scope will not include [functions](../management/functions.md). To include a function, define a [let statement](./letstatement.md) with the `view` keyword.
> * There's no guarantee of the order in which the union legs will appear, but if each leg has an `order by` operator, then each leg will be sorted.

## Performance considerations

If the `union` input is [tables](../management/tables.md) as opposed to [tabular expressions](./tabularexpressionstatements.md), and the `union` is followed by a [where operator](./whereoperator.md), consider replacing both with [find](./findoperator.md).

## Returns

A table with as many rows as there are in all the input tables.

## Examples

### Tables with string in name or column

```kusto
union K* | where * has "Kusto"
```

Rows from all tables in the database whose name starts with `K`, and in which any column includes the word `Kusto`.

### Distinct count

```kusto
union withsource=SourceTable kind=outer Query, Command
| where Timestamp > ago(1d)
| summarize dcount(UserId)
```

The number of distinct users that have produced
either a `Query` event or a `Command` event over the past day. In the result, the 'SourceTable' column will indicate either "Query" or "Command".

```kusto
Query
| where Timestamp > ago(1d)
| union withsource=SourceTable kind=outer 
   (Command | where Timestamp > ago(1d))
| summarize dcount(UserId)
```

This more efficient version produces the same result. It filters each table before creating the union.

### Using `isfuzzy=true`

```kusto     
// Using union isfuzzy=true to access non-existing view:                                     
let View_1 = view () { print x=1 };
let View_2 = view () { print x=1 };
let OtherView_1 = view () { print x=1 };
union isfuzzy=true
(View_1 | where x > 0), 
(View_2 | where x > 0),
(View_3 | where x > 0)
| count 
```

**Output**

|Count|
|---|
|2|

Observing Query Status - the following warning returned:
`Failed to resolve entity 'View_3'`

```kusto
// Using union isfuzzy=true and wildcard access:
let View_1 = view () { print x=1 };
let View_2 = view () { print x=1 };
let OtherView_1 = view () { print x=1 };
union isfuzzy=true View*, SomeView*, OtherView*
| count 
```

**Output**

|Count|
|---|
|3|

Observing Query Status - the following warning returned:
`Failed to resolve entity 'SomeView*'`

### Source columns types mismatch

```kusto     
let View_1 = view () { print x=1 };
let View_2 = view () { print x=toint(2) };
union withsource=TableName View_1, View_2
```

**Output**

|TableName|x_long|x_int|
|---------|------|-----|
|View_1   |1     |     |
|View_2   |      |2    |

```kusto     
let View_1 = view () { print x=1 };
let View_2 = view () { print x=toint(2) };
let View_3 = view () { print x_long=3 };
union withsource=TableName View_1, View_2, View_3 
```

**Output**

|TableName|x_long1|x_int |x_long|
|---------|-------|------|------|
|View_1   |1      |      |      |
|View_2   |       |2     |      |
|View_3   |       |      |3     |

Column `x` from `View_1` received the suffix `_long`, and as a column named `x_long` already exists in the result schema, the column names were de-duplicated, producing a new column- `x_long1`
