---
title: coalesce() - Azure Data Explorer | Microsoft Docs
description: This article describes coalesce() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 10/23/2018
---
# coalesce()

Evaluates a list of expressions and returns the first non-null (or non-empty for string) expression.

```kusto
coalesce(tolong("not a number"), tolong("42"), 33) == 42
```

**Syntax**

`coalesce(`*expr_1*`, `*expr_2*`,` ...)

**Arguments**

* *expr_i*: A scalar expression, to be evaluated.
- All arguments must be of the same type.
- Maximum of 64 arguments is supported.


**Returns**

The value of the first *expr_i* whose value is not null (or not-empty for string expressions).

**Example**

```kusto
print result=coalesce(tolong("not a number"), tolong("42"), 33)
```

|result|
|---|
|42|