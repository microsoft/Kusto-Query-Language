---
title: strcat_array() - Azure Data Explorer | Microsoft Docs
description: This article describes strcat_array() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# strcat_array()

Creates a concatenated string of array values using specified delimiter.
    
## Syntax

`strcat_array(`*array*, *delimiter*`)`

## Arguments

* *array*: A `dynamic` value representing an array of values to be concatenated.
* *delimeter*: A `string` value that will be used to concatenate the values in *array*

## Returns

Array values, concatenated to a single string.

## Examples
  
```kusto
print str = strcat_array(dynamic([1, 2, 3]), "->")
```

|str|
|---|
|1->2->3|