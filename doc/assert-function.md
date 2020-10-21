---
title: assert() - Azure Data Explorer
description: This article describes assert() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 09/26/2019
---
# assert()

Checks for a condition. If the condition is false, outputs error messages and fails the query.

## Syntax

`assert(`*condition*`, `*message*`)`

## Arguments

* *condition*: The conditional expression to evaluate. If the condition is `false`, the specified message is used to report an error. If the condition is `true`, it returns `true` as an evaluation result. Condition must be evaluated to constant during the query analysis phase.
* *message*: The message used if assertion is evaluated to `false`. The *message* must be a string literal.

> [!NOTE]
> `condition` must be evaluated to constant during the query analysis phase. In other words, it can be constructed from other expressions referencing constants, and can't be bound to row-context.

## Returns

* `true` - if the condition is `true`
* Raises semantic error if the condition is evaluated to `false`.

## Examples

The following query defines a function `checkLength()` that checks input string length, and uses `assert` to validate input length parameter (checks that it is greater than zero).

<!-- csl: https://help.kusto.windows.net:443/Samples -->
```kusto
let checkLength = (len:long, s:string)
{
    assert(len > 0, "Length must be greater than zero") and 
    strlen(s) > len
};
datatable(input:string)
[
    '123',
    '4567'
]
| where checkLength(len=long(-1), input)
```

Running this query yields an error:  
`assert() has failed with message: 'Length must be greater than zero'`


Example of running with valid `len` input:

<!-- csl: https://help.kusto.windows.net:443/Samples -->
```kusto
let checkLength = (len:long, s:string)
{
    assert(len > 0, "Length must be greater than zero") and strlen(s) > len
};
datatable(input:string)
[
    '123',
    '4567'
]
| where checkLength(len=3, input)
```

|input|
|---|
|4567|
