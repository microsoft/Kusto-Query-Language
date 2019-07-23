---
title: indexof_regex() - Azure Data Explorer | Microsoft Docs
description: This article describes indexof_regex() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 07/07/2019
---
# indexof_regex()

Function reports the zero-based index of the first occurrence of a specified string within input string. Plain string matches do not overlap. 

See [`indexof()`](indexoffunction.md).

**Syntax**

`indexof_regex(`*source*`,`*lookup*`[,`*start_index*`[,`*length*`[,`*occurrence*`]]])`

**Arguments**

* *source*: input string.  
* *lookup*: string to seek.
* *start_index*: search start position (optional).
* *length*: number of character positions to examine, -1 defining unlimited length (optional).
* *occurrence*: is the of occurrence Default 1 (optional).

**Returns**

Zero-based index position of *lookup*.

Returns -1 if the string is not found in the input.
In case of irrelevant (less than 0) *start_index*, *occurrence* or (less than -1) *length* parameter - returns *null*.


**Examples**
```kusto
print
 idx1 = indexof_regex("abcabc", "a.c") // lookup found in input string
 , idx2 = indexof_regex("abcabcdefg", "a.c", 0, 9, 2)  // lookup found in input string
 , idx3 = indexof_regex("abcabc", "a.c", 1, -1, 2)  // there is no second occurrence in the search range
 , idx4 = indexof_regex("ababaa", "a.a", 0, -1, 2)  // Plain string matches do not overlap so full lookup can't be found
 , idx5 = indexof_regex("abcabc", "a|ab", -1)  // invalid input
```

|idx1|idx2|idx3|idx4|idx5
|----|----|----|----|----
|0   |3   |-1  |-1  |    