---
title:  bag_merge() 
description: Learn how to use the bag_merge() function to merge property bags.
ms.reviewer: elgevork
ms.topic: reference
ms.date: 11/23/2022
---
# bag_merge()

Merges `dynamic` property bags into a `dynamic` property bag object with all properties merged.

## Syntax

`bag_merge(`*bag1*`,`*bag2*`[`,`*bag3*, ...])`

## Parameters

| Name | Type | Required | Description |
| -- | -- | -- | -- |
| *bag1...bagN* | dynamic | &check; | The property bags to merge. The function accepts between 2 to 64 arguments. |

## Returns

Returns a `dynamic` property bag. Results from merging all of the input property bag objects. If a key appears in more than one input object, an arbitrary value out of the possible values for this key will be chosen.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUShKLS7NKVGwVUhKTI/PTS1KT9XgUlBQSKnMS8zNTNaoVnc0VLcyNNJRUHcCMkC0M5A2rtXUQVNmpG5lYQhSBmKA1IE11mpqAgDRMHuwaAAAAA==" target="_blank">Run the query</a>

```kusto
print result = bag_merge(
   dynamic({'A1':12, 'B1':2, 'C1':3}),
   dynamic({'A2':81, 'B2':82, 'A1':1}))
```

**Output**

|result|
|---|
|{<br>  "A1": 12,<br>  "B1": 2,<br>  "C1": 3,<br>  "A2": 81,<br>  "B2": 82<br>}|
