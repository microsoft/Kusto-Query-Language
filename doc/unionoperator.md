---
title: union operator - Azure Data Explorer | Microsoft Docs
description: This article describes union operator in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
ms.localizationpriority: high
zone_pivot_group_filename: data-explorer/zone-pivot-groups.json
zone_pivot_groups: kql-flavors
---
# union operator

Takes two or more tables and returns the rows of all of them. 

```kusto
Table1 | union Table2, Table3
```

## Syntax

*T* `| union` [*UnionParameters*] [`kind=` `inner`|`outer`] [`withsource=`*ColumnName*] [`isfuzzy=` `true`|`false`] *Table* [`,` *Table*]...  

Alternative form with no piped input:

`union` [*UnionParameters*] [`kind=` `inner`|`outer`] [`withsource=`*ColumnName*] [`isfuzzy=` `true`|`false`] *Table* [`,` *Table*]...  

## Arguments

::: zone pivot="azuredataexplorer"

* `Table`:
    *  The name of a table, such as `Events`; or
    *  A query expression that must be enclosed with parenthesis, such as `(Events | where id==42)` or `(cluster("https://help.kusto.windows.net:443").database("Samples").table("*"))`; or
    *  A set of tables specified with a wildcard. For example, `E*` would form the union of all the tables in the database whose names begin `E`.
* `kind`: 
    * `inner` - The result has the subset of columns that are common to all of the input tables.
    * `outer` - (default). The result has all the columns that occur in any of the inputs. Cells that weren't defined by an input row are set to `null`.
* `withsource`=*ColumnName*: If specified, the output will include a column
called *ColumnName* whose value indicates which source table has contributed each row.
If the query effectively (after wildcard matching) references tables from more than one database (default database always counts) the value of this column will have a table name qualified with the database.
Similarly __cluster and database__ qualifications will be present in the value if more than one cluster is referenced. 
* `isfuzzy=` `true` | `false`: If `isfuzzy` is set to `true` - allows fuzzy resolution of union legs. `Fuzzy` applies to the set of `union` sources. It means that while analyzing the query and preparing for execution, the set of union sources is reduced to the set of table references that exist and are accessible at the time. If at least one such table was found, any resolution failure will yield a warning in the query status results (one for each missing reference), but will not prevent the query execution; if no resolutions were successful - the query will return an error.
The default is `isfuzzy=` `false`.
* *UnionParameters*: Zero or more (space-separated) parameters in the form of
  *Name* `=` *Value* that control the behavior
  of the row-match operation and execution plan. The following parameters are supported: 

  |Name           |Values                                        |Description                                  |
  |---------------|----------------------------------------------|---------------------------------------------|
  |`hint.concurrency`|*Number*|Hints the system how many concurrent subqueries of the `union` operator should be executed in parallel. *Default*: Amount of CPU cores on the single node of the cluster (2 to 16).|
  |`hint.spread`|*Number*|Hints the system how many nodes should be used by the concurrent `union` subqueries execution. *Default*: 1.|

::: zone-end

::: zone pivot="azuremonitor"

* `Table`:
    *  The name of a table, such as `Events`
    *  A query expression that must be enclosed with parenthesis, such as `(Events | where id==42)`
    *  A set of tables specified with a wildcard. For example, `E*` would form the union of all the tables in the database whose names begin `E`.

> [!NOTE]
> Whenever the list of tables is known, refrain from using wildcards. Some workspaces contains very large number of tables that would lead to inefficient execution. Tables may also be added over time leading to unpredicted results.
    
* `kind`: 
    * `inner` - The result has the subset of columns that are common to all of the input tables.
    * `outer` - (default). The result has all the columns that occur in any of the inputs. Cells that weren't defined by an input row are set to `null`.
* `withsource`=*ColumnName*: If specified, the output will include a column
called *ColumnName* whose value indicates which source table contributed each row.
If the query effectively (after wildcard matching) references tables from more than one database (default database always counts) the value of this column will have a table name qualified with the database.
Similarly, the __cluster and database__ qualifications will be present in the value if more than one cluster is referenced. 
* `isfuzzy=` `true` | `false`: If `isfuzzy` is set to `true` - allows fuzzy resolution of union legs. `Fuzzy` applies to the set of `union` sources. It means that while analyzing the query and preparing for execution, the set of union sources is reduced to the set of table references that exist and are accessible at the time. If at least one such table was found, any resolution failure will yield a warning in the query status results (one for each missing reference), but will not prevent the query execution; if no resolutions were successful - the query will return an error.
The default is `isfuzzy=false`.

::: zone-end

## Returns

A table with as many rows as there are in all the input tables.

**Notes**

::: zone pivot="azuredataexplorer"

1. `union` scope can include [let statements](./letstatement.md) if those are 
attributed with [view keyword](./letstatement.md)
2. `union` scope will not include [functions](../management/functions.md). To include a function in the union scope, define a [let statement](./letstatement.md) with [view keyword](./letstatement.md)
3. If the `union` input is [tables](../management/tables.md) (as oppose to [tabular expressions](./tabularexpressionstatements.md)), and the `union` is followed by a [where operator](./whereoperator.md), for better performance, consider replacing both with [find](./findoperator.md). Note the different [output schema](./findoperator.md#output-schema) produced by the `find` operator. 
4. `isfuzzy=true` only applies to the `union` sources resolution phase. Once the set of source tables is determined, possible additional query failures will not be suppressed.
5. When using `outer union`, the result has all the columns that occur in any of the inputs, one column for each name and type occurrences. This means that if a column appears in multiple tables and has multiple types, it will have a corresponding column for each type in the `union`'s result. This column name will be suffixed with a '_' followed by the origin column [type](./scalar-data-types/index.md).

::: zone-end

::: zone pivot="azuremonitor"

1. `union` scope can include [let statements](./letstatement.md) if those are 
attributed with [view keyword](./letstatement.md)
2. `union` scope will not include functions. To include 
function in the union scope - define a [let statement](./letstatement.md) 
with [view keyword](./letstatement.md)
3. If the `union` input is tables (as oppose to [tabular expressions](./tabularexpressionstatements.md)), and the `union` is followed by a [where operator](./whereoperator.md), consider replacing both with [find](./findoperator.md) for better performance. Please note the different [output schema](./findoperator.md#output-schema) produced by the `find` operator. 
4. `isfuzzy=` `true` applies only to the phase of the `union` sources resolution. Once the set of source tables was determined, possible additional query failures will not be suppressed.
5. When using `outer union`, the result has all the columns that occur in any of the inputs, one column for each name and type occurrences. This means that if a column appears in multiple tables and has multiple types, it will have a corresponding column for each type in the `union`'s result. This column name will be suffixed with a '_' followed by the origin column [type](./scalar-data-types/index.md).

::: zone-end


## Example: Tables with string in name or column

```kusto
union K* | where * has "Kusto"
```

Rows from all tables in the database whose name starts with `K`, and in which any column includes the word `Kusto`.

## Example: Distinct count

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

**Example: Using `isfuzzy=true`**
 
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

|Count|
|---|
|3|

Observing Query Status - the following warning returned:
`Failed to resolve entity 'SomeView*'`

**Example: source columns types mismatch**
 
```kusto     
let View_1 = view () { print x=1 };
let View_2 = view () { print x=toint(2) };
union withsource=TableName View_1, View_2
```

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

|TableName|x_long1|x_int |x_long|
|---------|-------|------|------|
|View_1   |1      |      |      |
|View_2   |       |2     |      |
|View_3   |       |      |3     |

Column `x` from `View_1` received the suffix `_long`, and as a column named `x_long` already exists in the result schema, the column names were de-duplicated, producing a new column- `x_long1`
