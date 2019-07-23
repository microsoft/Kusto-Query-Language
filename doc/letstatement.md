---
title: Let statement - Azure Data Explorer | Microsoft Docs
description: This article describes Let statement in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 10/23/2018
---
# Let statement

Let statements bind names to expressions. For the rest of the scope in which
the let statement appears (global scope or in a function body scope), the name
can be used to refer to its bound value. If that name was previously bound to
another value, the "innermost" let statement binding is used.

Let statements improve modularity and reuse, as they allow one to break a
potentially complex expression into multiple parts, each
bound to a name through the let statement, and compose them together. They
can also be used to create user-defined functions and views (expressions over tables whose results look
like a new table).

Names bound by let statements must be valid entity names.

Expressions bound by let statements can be:
* Of scalar type
* Of tabular type
* User-defined functions (lambdas)

**Syntax**

`let` *Name* `=` *ScalarExpression* | *TabularExpression* | *FunctionDefinitionExpression*

* *Name*: The name to bind. The name must be a valid entity name,
  and entity name escaping (e.g., `["Name with spaces"]`) is allowed. 
* *ScalarExpression*: An expression with a scalar result whose value will
  be bound to the name. For example: `let one=1;`.
* *TabularExpression*: An expression with a tabular result whose value will
  be bound to the name. For example: `Logs | where Timestamp > ago(1h)`.
* *FunctionDefinitionExpression*: An expression that yields a lambda (an
  anonymous function declaration) which is to be bound to the name.
  For example: `let f=(a:int, b:string) { strcat(b, ":", a) }`.

Lambda expressions have the following syntax:

[`view`] `(`[*TabularArguments*][`,`][*ScalarArguments*]`)` `{` *FunctionBody* `}`

`TabularArguments` - [*TabularArgName* `:` `(`[*AtrName* `:` *AtrType*] [`,` ... ]`)`] [`,` ... ][`,`]

 or:			   - [*TabularArgName* `:` `(` `*` `)`]

`ScalarArguments` - [*ArgName* `:` *ArgType*] [`,` ... ]

* `view` may appear only in a parameterless lambda (one that has no arguments)
  and indicates that the bound name will be included when "all tables" are
  queries (for example, when using `union *`).
* *TabularArguments* are the list of the formal tabular expression arguments.
  Each argument has:
  * *TabularArgName* - the name of the formal tabular argument. The name then may appear
  in the *FunctionBody* and is bound to a particular value when the lambda is
  invoked. 
  * Table schema definition - a list of attributes with their types
  (AtrName : AtrType).
  The tabular expression that is used in the lambda invocation must have all
  these attributes with the matching types, but is not limited to them. 
  '(*)' can be used as the tabular expression. In this case any tabular expression 
  can be used in the lambda invocation and none of its columns can be accessed
  in the lambda expression.
  All tabular arguments should appear before the scalar arguments.
* *ScalarArguments* are the list of the formal scalar arguments. 
  Each argument has:
  * *ArgName* - the name of the formal scalar argument. The name then may appear
  in the *FunctionBody* and is bound to a particular value when the lambda is
  invoked.  
  * *ArgType* - the type of the formal scalar argument. Currently only the following
  types are supported as a lambda argument type: `bool`, `string`, `long`,
  `datetime`, `timespan`, `real`, and `dynamic` (and aliases to these types).

**Multiple and nested let statements**

Multiple let statements can be used with `;` delimiter between them as shown in the following example.
The last statement must be a valid query expression: 

```kusto
let start = ago(5h); 
let period = 2h; 
T | where Time > start and Time < start + period | ...
```

Nested let statements are allowed and can be used inside a lambda expression.
Let statements and Arguments are visible in the current and inner scope of the Function body.

```kusto
let start = ago(5h); 
let period = 2h; 
T | where Time > start and Time < start + period | ...
```

**Examples**

### Using let to define constants

The following example binds the name `x` to the scalar literal `1`,
and then uses it in a tabular expression statement:

```kusto
let x = 1;
range y from x to x step x
```

Same example, but in this case - the name of the let statement is given using `['name']` notion:

```kusto
let ['x'] = 1;
range y from x to x step x
```

Yet another example that uses let for scalar values:

```kusto
let n = 10;  // number
let place = "Dallas";  // string
let cutoff = ago(62d); // datetime
Events 
| where timestamp > cutoff 
    and city == place 
| take n
```

### Using multiple let statements

The following example defines two let statements where one statement (`foo2`) uses another (`foo1`).

```kusto
let foo1 = (_start:long, _end:long, _step:long) { range x from _start to _end step _step};
let foo2 = (_step:long) { foo1(1, 100, _step)};
foo2(2) | count
// Result: 50
```

### Using materialize function

[`materialize`](materializefunction.md) function allows caching sub-query results during the time of query execution. 

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