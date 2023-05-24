---
title:  jaccard_index()
description: Learn how to use the jaccard_index() function to calculate the Jaccard index of two input sets.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/03/2023
---
# jaccard_index()

Calculates the [Jaccard index](https://en.wikipedia.org/wiki/Jaccard_index) of two input sets.

## Syntax

`jaccard_index`(*set1*, *set2*)

## Parameters

| Name | Type | Required | Description |
| -- | -- | -- | -- |
| *set1*| dynamic | &check; | The array representing the first set for the calculation.|
| *set2*| dynamic | &check; | The array representing the second set for the calculation.|

> [!NOTE]
> Duplicate values in the input arrays are ignored.

## Returns

The [Jaccard index](https://en.wikipedia.org/wiki/Jaccard_index) of the two input sets. The Jaccard index formula is |*set1* ∩ *set2*| / |*set1* ∪ *set2*|.

## Examples

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUShOLTG0TanMS8zNTNaINtQx0jGO1dQBCRuhCeuYxGryctUopFaUpOalKGQlJicnFqXYQun4zLyU1AoNkHEQ3ZoAjvvou2AAAAA=" target="_blank">Run the query</a>

```kusto
print set1=dynamic([1,2,3]), set2=dynamic([1,2,3,4])
| extend jaccard=jaccard_index(set1, set2)
```

**Output**

|`set1`|`set2`|`jaccard`|
|---|---|---|
|[1,2,3]|[1,2,3,4]|0.75|
