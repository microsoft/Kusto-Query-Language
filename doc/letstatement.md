---
title: Let statement - Azure Data Explorer
description: This article describes the Let statement in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 09/04/2022
ms.localizationpriority: high
---
# Let statement

Use the `let` statement to set a variable name equal to an expression or a function, or to create [views](schema-entities/views.md).

`let` statements are useful for:

* Breaking up a complex expression into multiple parts, each represented by a variable. 
* Defining constants outside of the query body for readability.
* Defining a variable once and using it multiple times within a query.

If the variable previously represented another value, for example in nested statements, the innermost `let` statement applies.

To optimize multiple uses of the `let` statement within a single query, see [Optimize queries that use named expressions](../../named-expressions.md).

## Syntax: Scalar or tabular expressions

`let` *Name* `=` *ScalarExpression* 

`let` *Name* `=` *TabularExpression* 


### Arguments 

|Argument  |Description  |Example  |
|---------|---------|---------|
|*Name*   | The variable name must be valid. | You can escape the name, for example `["Name with spaces"]` |
|*ScalarExpression* | An expression with a scalar result.| `let one=1;`  |
|*TabularExpression*  | An expression with a tabular result. |  `let RecentLog = Logs  \| where Timestamp > ago(1h)`  |

## Syntax: View or function

`let` *Name* `=` [`view`] `(` [*TabularArgName* `:` `(` `*` `)` `,`   [*ArgName* `:` *ArgType* ]`,` ... ]  `)` `{` *FunctionBody* `}`

`let` *Name* `=` [`view`] `(` [  [*TabularArgName* `:` `(`[*AttributeName* `:` *AttributeType*] [`,` ... ] `)` ] `,` [  [*ArgName* `:` *ArgType* , ...]  ] `)` `{` *FunctionBody* `}

### Arguments

|Argument |Description  |
|---------|---------|
|*FunctionBody* | An expression that yields a user defined function. | 
|*view* | Appears only in a parameterless `let` statement with no arguments. When used, the `let` statement is included in queries with a `union` operator with wildcard selection of the tables/views. | 
| *TabularArgName*| The name of the tabular argument. Can appear in the *FunctionBody* and is bound to a particular value when the user defined function is invoked. | 
| *AttributeName*: *AttributeType*| The name and type of the attribute. Part of the table schema definition, which includes a set of attributes with their types. |  
|*ArgName* | The name of the scalar argument. Can appear in the *FunctionBody* and is bound to a particular value when the user defined function is invoked.  | 
|*ArgType* | The type of the scalar argument. Currently the following are supported for user defined functions: `bool`, `string`, `long`, `datetime`, `timespan`, `real`, and `dynamic` (and aliases to these types).|  

> [!NOTE]
>
> * You can use `(*)` for the tabular expression.
> * When using a tabular expression as part of a user defined function, the columns can't be accessed as part of the function.
> * Tabular arguments appear before scalar arguments.
> * Any two statements must be separated by a semicolon.

## Examples

### Define scalar values

The following example uses a scalar expression statement.

```kusto

let n = 10;  // number
let place = "Dallas";  // string
let cutoff = ago(62d); // datetime 
Events 
| where timestamp > cutoff 
    and city == place 
| take n
```

The following example binds the name `some number` using the `['name']` notation, and then uses it in a tabular expression statement.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
let ['some number'] = 20;
range y from 0 to ['some number'] step 5
```

### Create a user defined function with scalar calculation

This example uses the let statement with arguments for scalar calculation. The query defines function `MultiplyByN` for multiplying two numbers.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
let MultiplyByN = (val:long, n:long) { val * n };
range x from 1 to 5 step 1 
| extend result = MultiplyByN(x, 5)
```

**Output**

|x|result|
|---|---|
|1|5|
|2|10|
|3|15|
|4|20|
|5|25|

### Create a user defined function that trims input

The following example removes leading and trailing ones from the input.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
let TrimOnes = (s:string) { trim("1", s) };
range x from 10 to 15 step 1 
| extend result = TrimOnes(tostring(x))
```

**Output**

|x|result|
|---|---|
|10|0|
|11||
|12|2|
|13|3|
|14|4|
|15|5|

### Use multiple let statements

This example defines two let statements where one statement (`foo2`) uses another (`foo1`).

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
let foo1 = (_start:long, _end:long, _step:long) { range x from _start to _end step _step};
let foo2 = (_step:long) { foo1(1, 100, _step)};
foo2(2) | count
```

**Output**

|result|
|---|
|50|

### Create a view or virtual table 

This example shows you how to use a let statement to create a [`view` or virtual table](schema-entities/views.md).

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
let Range10 = view () { range MyColumn from 1 to 10 step 1 };
let Range20 = view () { range MyColumn from 1 to 20 step 1 };
search MyColumn == 5
```

**Output**

|$table|MyColumn|
|---|---|
|Range10|5|
|Range20|5|

### Use a materialize function

The [`materialize()`](materializefunction.md) function lets you cache subquery results during the time of query execution. When you use the `materialize()` function, the data is cached, and any subsequent invocation of the result uses cached data.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
let totalPagesPerDay = PageViews
| summarize by Page, Day = startofday(Timestamp)
| summarize count() by Day;
let materializedScope = PageViews
| summarize by Page, Day = startofday(Timestamp);
let cachedResult = materialize(materializedScope);
cachedResult
| project Page, Day1 = Day
| join kind = inner
(
    cachedResult
    | project Page, Day2 = Day
)
on Page
| where Day2 > Day1
| summarize count() by Day1, Day2
| join kind = inner
    totalPagesPerDay
on $left.Day1 == $right.Day
| project Day1, Day2, Percentage = count_*100.0/count_1
```

**Output**

|Day1|Day2|Percentage|
|---|---|---|
|2016-05-01 00:00:00.0000000|2016-05-02 00:00:00.0000000|34.0645725975255|
|2016-05-01 00:00:00.0000000|2016-05-03 00:00:00.0000000|16.618368960101|
|2016-05-02 00:00:00.0000000|2016-05-03 00:00:00.0000000|14.6291376489636|

### Using nested let statements

Nested let statements are permitted, including within a user defined function expression. Let statements and arguments apply in both the current and inner scope of the function body.

```kusto
let start_time = ago(5h); 
let end_time = start_time + 2h; 
T | where Time > start_time and Time < end_time | ...
```
