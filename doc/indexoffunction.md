---
title: indexof() - Azure Data Explorer | Microsoft Docs
description: This article describes indexof() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 07/07/2019
---
# indexof()

Function reports the zero-based index of the first occurrence of a specified string within input string.

If lookup or input string is not of string type - forcibly casts the value to string.

See [`indexof_regex()`](indexofregexfunction.md).

**Syntax**

`indexof(`*source*`,`*lookup*`[,`*start_index*`[,`*length*`[,`*occurrence*`]]])`

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
 idx1 = indexof("abcdefg","cde")    // lookup found in input string
 , idx2 = indexof("abcdefg","cde",1,4) // lookup found in researched range 
 , idx3 = indexof("abcdefg","cde",1,2) // search starts from index 1, but stops after 2 chars, so full lookup can't be found
 , idx4 = indexof("abcdefg","cde",3,4) // search starts after occurrence of lookup
 , idx5 = indexof("abcdefg","cde",-1)  // invalid input
 , idx6 = indexof(1234567,5,1,4)       // two first parameters were forcibly casted to strings "12345" and "5"
 , idx7 = indexof("abcdefg","cde",2,-1)  // lookup found in input string
 , idx8 = indexof("abcdefgabcdefg", "cde", 1, 10, 2)   // lookup found in input range
 , idx9 = indexof("abcdefgabcdefg", "cde", 1, -1, 3)   // the third occurrence of lookup is not in researched range
```

|idx1|idx2|idx3|idx4|idx5|idx6|idx7|idx8|idx9|
|----|----|----|----|----|----|----|----|----|
|2   |2   |-1  |-1  |    |4   |2   |9   |-1  |
