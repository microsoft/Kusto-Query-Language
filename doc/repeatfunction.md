---
title: repeat() - Azure Data Explorer
description: Learn how to use the repeat() function to generate a dynamic array containing a series comprised of repeated numbers.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/17/2023
---
# repeat()

Generates a dynamic array containing a series comprised of repeated numbers.

## Syntax

`repeat(`*value*`,` *count*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *value* | bool, int, long, real, datetime, string or timespan | &check; | The value of the element in the resulting array.|  
| *count* | int | &check; | The count of the elements in the resulting array.|

## Returns

If *count* is equal to zero, an empty array is returned.
If *count* is less than zero, a null value is returned.

## Examples

The following example returns `[1, 1, 1]`:

```kusto
T | extend r = repeat(1, 3)
```
