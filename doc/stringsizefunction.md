---
title: string_size() - Azure Data Explorer
description: This article describes string_size() in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 02/13/2020
---
# string_size()

Returns the size, in bytes, of the input string.

## Syntax

`string_size(`*source*`)`

## Arguments

* *source*: The source string that will be measured for string size.

## Returns

Returns the length, in bytes, of the input string.

## Examples

```kusto
print size = string_size("hello")
```

**Output**

|size|
|---|
|5|

```kusto
print size = string_size("⒦⒰⒮⒯⒪")
```

**Output**

|size|
|---|
|15|