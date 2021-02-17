---
title: split() - Azure Data Explorer | Microsoft Docs
description: This article describes split() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
ms.localizationpriority: high
adobe-target: true
---
# split()

Splits a given string according to a given delimiter and returns a string array with the contained substrings.

Optionally, a specific substring can be returned if exists.

```kusto
split("aaa_bbb_ccc", "_") == ["aaa","bbb","ccc"]
```

## Syntax

`split(`*source*`,` *delimiter* [`,` *requestedIndex*]`)`

## Arguments

* *source*: The source string that will be split according to the given delimiter.
* *delimiter*: The delimiter that will be used in order to split the source string.
* *requestedIndex*: An optional zero-based index `int`. If provided, the returned string array will contain the requested substring if exists. 

## Returns

A string array that contains the substrings of the given source string that are delimited by the given delimiter.

## Examples

```kusto
print
    split("aa_bb", "_"),           // ["aa","bb"]
    split("aaa_bbb_ccc", "_", 1),  // ["bbb"]
    split("", "_"),                // [""]
    split("a__b", "_"),            // ["a","","b"]
    split("aabbcc", "bb")          // ["aa","cc"]
```