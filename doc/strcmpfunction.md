---
title: strcmp() - Azure Data Explorer | Microsoft Docs
description: This article describes strcmp() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 10/23/2018
---
# strcmp()

Compares two strings.

The function starts comparing the first character of each string. If they are equal to each other, it continues with the following pairs until the characters differ or until the end of shorter string is reached.

## Syntax

`strcmp(`*string1*`,` *string2*`)` 

## Arguments

* *string1*: first input string for comparison. 
* *string2*: second input string for comparison.

## Returns

Returns an integral value indicating the relationship between the strings:
* *<0* - the first character that does not match has a lower value in string1 than in string2
* *0* - the contents of both strings are equal
* *>0* - the first character that does not match has a greater value in string1 than in string2

## Examples

```
datatable(string1:string, string2:string)
["ABC","ABC",
"abc","ABC",
"ABC","abc",
"abcde","abc"]
| extend result = strcmp(string1,string2)
```

|string1|string2|result|
|---|---|---|
|ABC|ABC|0|
|abc|ABC|1|
|ABC|abc|-1|
|abcde|abc|1|