---
title: jaccard_index() - Azure Data Explorer
description: This article describes jaccard_index() in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 03/20/2022
---
# jaccard_index()

Calculates the [Jaccard index](https://en.wikipedia.org/wiki/Jaccard_index) of two sets

## Syntax

`jaccard_index`(*set1*, *set2*)

## Arguments

* *set1*: Input array representing the first set for the calculation.
* *set2*: Input array representing the second set for the calculation.

> [!NOTE]
> Duplicate values in the input arrays are ignored.

## Returns

The [Jaccard index](https://en.wikipedia.org/wiki/Jaccard_index) of the two input sets. The Jaccard index formula is |*set1* ∩ *set2*| / |*set1* ∪ *set2*|.

## Examples

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
print set1=dynamic([1,2,3]), set2=dynamic([1,2,3,4])
| extend jaccard=jaccard_index(set1, set2)
```
|`set1`|`set2`|`jaccard`|
|---|---|---|
|[1,2,3]|[1,2,3,4]|0.75|
