---
title: assert() - Azure Data Explorer | Microsoft Docs
description: This article describes assert() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 12/16/2018
---
# assert()

Checks for a condition; if the condition is false, outputs error messages and fails the query.

**Syntax**

`assert(`*condition*`, `*message*`)`

**Arguments**

* *condition*: The conditional expression to evaluate. If the condition is `false`, the specified message is used to report an error. If the condition is `true` - returns `true` as evaluation result. Condition must be evalauted to constant during query analysis phase.
* *message*: The message to be used in case of assertion is evaluated to `false`. *message* must be a string literal.


**Returns**

* `true` - if the condition is `true`
* Raises semantic error if the condition is evaluated to `false`.

**Notes**

* `condition` must be evaluated to constant during qyery analysis phase. In other words, it can be constructed from other expressions referencing constants, and cannot be bound to row-context.

**Examples**

The following query defines a function `checkLength()` which checks input string length, and uses `assert` to validate input length parameter (checks it is greater than zero).

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