---
title: Let statement - Azure Data Explorer
description: This article describes the Let statement in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 08/09/2020
ms.localizationpriority: high
---
# Let statement

Let statements bind names to expressions. 
For the rest of the scope, where the let statement appears, the name can be used to refer to its bound value. The let statement may be within a global scope or a function body scope.
If that name was previously bound to another value, the "innermost" let statement binding is used.

Let statements improve modularity and reuse, since they let you break a potentially complex expression into multiple parts.
Each part is bound to a name through the let statement, and together they compose the whole. 
They can also be used to create user-defined functions and views. The views are expressions over tables whose results look like a new table.

> [!NOTE]
> Names bound by let statements must be valid entity names.

Expressions bound by let statements can be:
* Scalar types
* Tabular types
* User-defined functions (lambdas)

## Syntax

`let` *Name* `=` *ScalarExpression* | *TabularExpression* | *FunctionDefinitionExpression*

|Field  |Definition  |Example  |
|---------|---------|---------|
|*Name*   | The name to bind. The name must be a valid entity name.    |Entity name escaping, such as `["Name with spaces"]`, is permitted.      |
|*ScalarExpression*     |  An expression with a scalar result whose value is bound to the name.  | `let one=1;`  |
|*TabularExpression*    | An expression with a tabular result whose value is bound to the name.   | `Logs | where Timestamp > ago(1h)`    |
|*FunctionDefinitionExpression*   | An expression that yields a lambda, an anonymous function declaration that is to be bound to the name.   |  `let f=(a:int, b:string) { strcat(b, ":", a) }`  |


### Lambda expressions syntax

[`view`] `(`[*TabularArguments*][`,`][*ScalarArguments*]`)` `{` *FunctionBody* `}`

`TabularArguments` - [*TabularArgName* `:` `(`[*AtrName* `:` *AtrType*] [`,` ... ]`)`] [`,` ... ][`,`]

 or:

 [*TabularArgName* `:` `(` `*` `)`] - indicating tabular expressions with variable and unknown columns.

`ScalarArguments` - [*ArgName* `:` *ArgType*] [`,` ... ]


|Field  |Definition  |Example  |
|---------|---------|---------|
| **view** | May appear only in a parameterless let statement that has no arguments. When the 'view' keyword is used, the let statement will be included in queries that use a `union` operator with wildcard selection of the tables/views. |  |
| ***TabularArguments*** | The list of the formal tabular expression arguments. 
| Each tabular argument has:||
|<ul><li> *TabularArgName*</li></ul> | The name of the formal tabular argument. The name may appear in the *FunctionBody* and is bound to a particular value when the lambda is invoked. ||
|<ul><li>Table schema definition </li></ul> | A list of attributes with their types| AtrName : AtrType|
| ***ScalarArguments*** | The list of the formal scalar arguments. 
|Each scalar argument has:||
|<ul><li>*ArgName*</li></ul> | The name of the formal scalar argument. The name may appear in the *FunctionBody* and is bound to a particular value when the lambda is invoked.  |
| <ul><li>*ArgType* </li></ul>| The type of the formal scalar argument. | Currently only the following types are supported as a lambda argument type: `bool`, `string`, `long`, `datetime`, `timespan`, `real`, and `dynamic` (and aliases to these types).

> [!NOTE]
>The tabular expression that is used in the lambda invocation must include (but is not limited to) all the attributes with the matching types.
>
>`(*)` can be used as the tabular expression. 
>
> Any tabular expression can be used in the lambda invocation and none of its columns can be accessed in the lambda expression. 
>
> All tabular arguments should appear before the scalar arguments.

## Multiple and nested let statements

Multiple let statements can be used with the semicolon, `;`, delimiter between them, like in the following example.

> [!NOTE]
> The last statement must be a valid query expression. 

```kusto
let start = ago(5h); 
let period = 2h; 
T | where Time > start and Time < start + period | ...
```

Nested let statements are permitted, and can be used inside a lambda expression.
Let statements and arguments are visible in the current and inner scope of the function body.

```kusto
let start_time = ago(5h); 
let end_time = start_time + 2h; 
T | where Time > start_time and Time < end_time | ...
```

## Examples

### Use let function to define constants

The following example binds the name `x` to the scalar literal `1`, and then uses it in a tabular expression statement.

```kusto
let x = 1;
range y from x to x step x
```

This example is similar to the previous one, only the name of the let statement is given using the `['name']` notion.

```kusto
let ['x'] = 1;
range y from x to x step x
```

### Use let for scalar values

```kusto
let n = 10;  // number
let place = "Dallas";  // string
let cutoff = ago(62d); // datetime
Events 
| where timestamp > cutoff 
    and city == place 
| take n
```

### Use let statement with arguments for scalar calculation

This example uses the let statement with arguments for scalar calculation. The query defines function `MultiplyByN` for multiplying two numbers.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
let MultiplyByN = (val:long, n:long) { val * n };
range x from 1 to 5 step 1 
| extend result = MultiplyByN(x, 5)
```

|x|result|
|---|---|
|1|5|
|2|10|
|3|15|
|4|20|
|5|25|

The following example removes leading/trailing ones (`1`) from the input.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
let TrimOnes = (s:string) { trim("1", s) };
range x from 10 to 15 step 1 
| extend result = TrimOnes(tostring(x))
```

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
// Result: 50
```

### Use the `view` keyword in a let statement

This example shows you how to use let statement with the `view` keyword.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
let Range10 = view () { range MyColumn from 1 to 10 step 1 };
let Range20 = view () { range MyColumn from 1 to 20 step 1 };
search MyColumn == 5
```

|$table|MyColumn|
|---|---|
|Range10|5|
|Range20|5|


### Use materialize function

The [`materialize`](materializefunction.md) function lets you cache subquery results during the time of query execution. 

<!-- csl: https://help.kusto.windows.net:443/Samples -->
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

|Day1|Day2|Percentage|
|---|---|---|
|2016-05-01 00:00:00.0000000|2016-05-02 00:00:00.0000000|34.0645725975255|
|2016-05-01 00:00:00.0000000|2016-05-03 00:00:00.0000000|16.618368960101|
|2016-05-02 00:00:00.0000000|2016-05-03 00:00:00.0000000|14.6291376489636|
