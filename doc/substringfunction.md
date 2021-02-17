---
title: substring() - Azure Data Explorer | Microsoft Docs
description: This article describes substring() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
ms.localizationpriority: high
---
# substring()

Extracts a substring from a source string starting from some index to the end of the string.

Optionally, the length of the requested substring can be specified.

```kusto
substring("abcdefg", 1, 2) == "bc"
```

## Syntax

`substring(`*source*`,` *startingIndex* [`,` *length*]`)`

## Arguments

* *source*: The source string that the substring will be taken from.
* *startingIndex*: The zero-based starting character position of the requested substring.
* *length*: An optional parameter that can be used to specify the requested number of characters in the substring. 

**Notes**

*startingIndex* can be a negative number, in which case the substring will be retrieved from the end of the source string.

## Returns

A substring from the given string. The substring starts at startingIndex (zero-based) character position and continues to the end of the string or length characters if specified.

## Examples

```kusto
substring("123456", 1)        // 23456
substring("123456", 2, 2)     // 34
substring("ABCD", 0, 2)       // AB
substring("123456", -2, 2)    // 56
```