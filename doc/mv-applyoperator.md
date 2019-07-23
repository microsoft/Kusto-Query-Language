---
title: mv-apply operator - Azure Data Explorer | Microsoft Docs
description: This article describes mv-apply operator in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 07/17/2019
---
# mv-apply operator

The mv-apply operator expands each record in its input table into a sub-table,
applies a sub-query to each sub-table, and returns the union of the results of
all sub-queries.

For example, assume a table `T` has a column `Metric` of type `dynamic`
whose values are arrays of `real` numbers. The following query will locate the
two biggest values in each `Metric` value, and return the records corresponding
to these values.

```kusto
T | mv-apply Metric to typeof(real) on (top 2 by Metric desc)
```

In general, the mv-apply operator can be thought of as having the following
processing steps:

1. It uses the [mv-expand operator](./mvexpandoperator.md) to expand each record
   in the input into sub-tables.
2. It applies the sub-query for each of the sub-tables.
3. It prepends zero or more columns to each resulting sub-table, containing the
   (repeated if necessary) values of the source columns that are not being expanded.
4. It returns the union of the results.

The mv-expand operator gets the following inputs:

1. One or more expressions that evaluate into dynamic arrays to expand.
   The number of records in each expanded sub-table is the maximum length of
   each of those dynamic arrays. (If multiple expressions are specified,
   but corresponding arrays are of different lengths, null values are introduced
   if necessary.)

2. Optionally, the names to assign the values of the expressions, after expansion.
   These become the names of the columns in the sub-tables.
   If not specified, the original name of the column is used (if the expression
   is a column reference), or a random name is used (otherwise).

   > [!NOTE]
   > It is recommended to use the default column names.

3. The data types of the elements of those dynamic arrays, after expansion.
   These become the column types of the columns in the sub-tables.
   If not specified, `dynamic` is used.

4. Optionally, the name of a column to add to the sub-tables which specifies the
   0-based index of the element in the array that resulted in the sub-table record.

5. Optionally, the maximum number of array elements to expand.

The mv-apply operator can be thought of as a generalization of the
[mv-expand](./mvexpandoperator.md) operator (in fact, the latter can be implemented
by the former, if the sub-query includes only projections.)

**Syntax**

*T* `|` `mv-apply` [*ItemIndex*] *ColumnsToExpand* [*RowLimit*] `on` `(` *SubQuery* `)`

Where *ItemIndex* has the syntax:

`with_itemindex` `=` *IndexColumnName*

*ColumnsToExpand* is a comma-separated list of one or more elements of the form:

[*Name* `=`] *ArrayExpression* [`to` `typeof` `(`*Typename*`)`]

*RowLimit* is simply:

`limit` *RowLimit*

and *SubQuery* has the same syntax of any query statement.

**Arguments**

* *ItemIndex*: If used, indicates the name of a column of type `long` that is appended to the input as part of the array-expansion phase and indicates the 0-based array index of the
  expanded value.

* *Name*: If used, the name to assign the array-expanded values of each
  array-expanded expression.
  (If unspecified, the name of the column will be used if available,
  or a random name generated if *ArrayExpression* is not a simple column name.)

* *ArrayExpression*: An expression of type `dynamic` whose values will be array-expanded.
  If the expression is the name of a column in the input, the input column is
  removed from the input and a new column of the same name (or *ColumnName* if
  specified) will appear in the output.

* *Typename*: If used, the name of the type that the individual elements of the
  `dynamic` array *ArrayExpression* takes. Elements that do not conform to this
  type will be replaced by a null value.
  (If unspecified, `dynamic` is used by default.)

* *RowLimit*: If used, a limit on the number of records to generate from each
  record of the input.
  (If unspecified, 2147483647 is used.)

* *SubQuery*: A tabular query expression with an implicit tabular source that gets
  applied to each array-expanded sub-table.

**Notes**

* Unlike the [mv-expand](./mvexpandoperator.md) operator, the mv-apply operator
  supports array expansion only. There's no support for expanding property bags.

**Examples**

## Getting the largest element from the array

```kusto
let _data =
range x from 1 to 8 step 1
| summarize l=make_list(x) by xMod2 = x % 2;
_data
| mv-apply element=l to typeof(long) on 
(
   top 1 by element
)
```

|xMod2|l           |element|
|-----|------------|-------|
|1    |[1, 3, 5, 7]|7      |
|0    |[2, 4, 6, 8]|8      |

## Calculating sum of largest two elments in an array

```kusto
let _data =
range x from 1 to 8 step 1
| summarize l=make_list(x) by xMod2 = x % 2;
_data
| mv-apply l to typeof(long) on
(
   top 2 by l
   | summarize SumOfTop2=sum(l)
)
```

|xMod2|l        |SumOfTop2|
|-----|---------|---------|
|1    |[1,3,5,7]|12       |
|0    |[2,4,6,8]|14       |


## Using `with_itemindex` for working with subset of the array

```kusto
let _data =
range x from 1 to 10 step 1
| summarize l=make_list(x) by xMod2 = x % 2;
_data
| mv-apply with_itemindex=index element=l to typeof(long) on 
(
   // here you have 'index' column
   where index >= 3
)
| project index, element
```

|index|element|
|---|---|
|3|7|
|4|9|
|3|8|
|4|10|

## Using `mv-apply` operator to sort the output of `makelist` aggregate by some key

```kusto
datatable(command:string, command_time:datetime, user_id:string)
[
	'chmod',		datetime(2019-07-15),	"user1",
	'ls',			datetime(2019-07-02),	"user1",
	'dir',			datetime(2019-07-22),	"user1",
	'mkdir',		datetime(2019-07-14),	"user1",
	'rm',			datetime(2019-07-27),	"user1",
	'pwd',			datetime(2019-07-25),	"user1",
	'rm',			datetime(2019-07-23),	"user2",
	'pwd',			datetime(2019-07-25),	"user2",
]
| summarize commands_details = make_list(pack('command', command, 'command_time', command_time)) by user_id
| mv-apply command_details = commands_details on
(
    order by todatetime(command_details['command_time']) asc
    | summarize make_list(tostring(command_details['command']))
)
| project-away commands_details 




```

|user_id|list_command_details_command|
|---|---|
|user1|[<br>  "ls",<br>  "mkdir",<br>  "chmod",<br>  "dir",<br>  "pwd",<br>  "rm"<br>]|
|user2|[<br>  "rm",<br>  "pwd"<br>]|



**See also**

* [mv-expand](./mvexpandoperator.md) operator.