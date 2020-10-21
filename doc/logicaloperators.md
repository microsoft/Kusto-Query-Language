---
title: Logical (binary) operators - Azure Data Explorer | Microsoft Docs
description: This article describes Logical (binary) operators in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 11/14/2018
---
# Logical (binary) operators

The following logical operators are supported between two values of the `bool`
type:

> [!NOTE]
> These logical operators are sometimes referred-to as Boolean operators,
> and sometimes as binary operators. The names are all synonyms.

|Operator name|Syntax|Meaning|
|-------------|------|-------|
|Equality     |`==`  |Yields `true` if both operands are non-null and equal to each other. Otherwise, `false`.|
|Inequality   |`!=`  |Yields `true` if either one (or both) of the operands are null, or they are not equal to each other. Otherwise, `true`.|
|Logical and  |`and` |Yields `true` if both operands are `true`.|
|Logical or   |`or`  |Yields `true` if one of the operands is `true`, regardless of the other operand.|

> [!NOTE]
> Due to the behavior of the Boolean null value `bool(null)`, two Boolean null
> values are neither equal nor non-equal (in other words, `bool(null) == bool(null)`
> and `bool(null) != bool(null)` both yield the value `false`).
>
> On the other hand, `and`/`or` treat the null value as equivalent to `false`,
> so `bool(null) or true` is `true`, and `bool(null) and true` is `false`.