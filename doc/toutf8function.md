---
title: to_utf8() - Azure Data Explorer | Microsoft Docs
description: This article describes to_utf8() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# to_utf8()

Returns a dynamic array of the unicode characters of an input string (the inverse operation of make_string).

## Syntax

`to_utf8(`*source*`)`

## Arguments

* *source*: The source string to convert.

## Returns

Returns a dynamic array of the unicode characters that make up the string provided to this function.
See [`make_string()`](makestringfunction.md))

## Examples

```kusto
print arr = to_utf8("⒦⒰⒮⒯⒪")
```

|arr|
|---|
|[9382, 9392, 9390, 9391, 9386]|

```kusto
print arr = to_utf8("קוסטו - Kusto")
```

|arr|
|---|
|[1511, 1493, 1505, 1496, 1493, 32, 45, 32, 75, 117, 115, 116, 111]|

```kusto
print str = make_string(to_utf8("Kusto"))
```

|str|
|---|
|Kusto|
