---
title: coalesce() - Azure Data Explorer
description: Learn how to use the coalesce() function to evaluate a list of expressions to return the first non-null expression.
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/23/2022
---
# coalesce()

Evaluates a list of expressions and returns the first non-null (or non-empty for string) expression.

```kusto
coalesce(tolong("not a number"), tolong("42"), 33) == 42
```

## Syntax

`coalesce(`*expr_1*`, `*expr_2*`,` ...)

## Arguments

* *expr_i*: A scalar expression, to be evaluated.
- All arguments must be of the same type.
- Maximum of 64 arguments is supported.

## Returns

The value of the first *expr_i* whose value isn't null (or not-empty for string expressions).

## Example

<!-- csl: https://help.kusto.windows.net/Samples  -->
```kusto
print result=coalesce(tolong("not a number"), tolong("42"), 33)
```

|result|
|---|
|42|
