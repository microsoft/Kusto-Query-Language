---
title: zip() - Azure Data Explorer | Microsoft Docs
description: This article describes zip() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# zip()

The `zip` function accepts any number of `dynamic` arrays, and returns an
array whose elements are each an array holding the elements of the input
arrays of the same index.

## Syntax

`zip(`*array1*`,` *array2*`, ... )`

## Arguments

Between 2 and 16 dynamic arrays.

## Examples

The following example returns `[[1,2],[3,4],[5,6]]`:

```kusto
print zip(dynamic([1,3,5]), dynamic([2,4,6]))
```

The following example returns `[["A",{}], [1,"B"], [1.5, null]]`:

```kusto
print zip(dynamic(["A", 1, 1.5]), dynamic([{}, "B"]))
```

The following example returns `[[1,"one"],[2,"two"],[3,"three"]]`:

```kusto
datatable(a:int, b:string) [1,"one",2,"two",3,"three"]
| summarize a = make_list(a), b = make_list(b)
| project zip(a, b)
```