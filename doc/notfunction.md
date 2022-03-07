---
title: not() - Azure Data Explorer
description: This article describes not() in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 02/13/2020
---
# not()

Reverses the value of its `bool` argument.

```kusto
not(false) == true
```

## Syntax

`not(`*expr*`)`

## Arguments

* *expr*: A `bool` expression to be reversed.

## Returns

Returns the reversed logical value of its `bool` argument.