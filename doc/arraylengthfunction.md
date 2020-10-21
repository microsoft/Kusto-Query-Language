---
title: array_length() - Azure Data Explorer | Microsoft Docs
description: This article describes array_length() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# array_length()

Calculates the number of elements in a dynamic array.

## Syntax

`array_length(`*array*`)`

## Arguments

* *array*: A `dynamic` value.

## Returns

The number of elements in *array*, or `null` if *array* is not an array.

## Examples

```kusto
print array_length(parse_json('[1, 2, 3, "four"]')) == 4

print array_length(parse_json('[8]')) == 1

print array_length(parse_json('[{}]')) == 1

print array_length(parse_json('[]')) == 0

print array_length(parse_json('{}')) == null

print array_length(parse_json('21')) == null
```