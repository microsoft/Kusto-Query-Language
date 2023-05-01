---
title: set_intersect() - Azure Data Explorer
description: Learn how to use the set_intersect() function to create a set of the distinct values that are in all the array inputs.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/30/2023
---
# set_intersect()

Returns a `dynamic` array of the set of all distinct values that are in all arrays - (arr1 ∩ arr2 ∩ ...).

## Syntax

`set_intersect(`*set1*`,` *set2* [`,` *set3*, ...]`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *set1...setN* | dynamic | &check; | Arrays used to create an intersect set. A minimum of 2 arrays are required. See [pack_array](packarrayfunction.md).|

## Returns

Returns a dynamic array of the set of all distinct values that are in all arrays.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA13MsQ7CMAwE0J2vuLFBWdLOfEtlFYMAkUSOpSYVH48zoTDeO/uE4p1RcZP0RoAmLCjKGeH0AVfleEXDxS7OmH90GLWRdqNjJApmmbbXSiLUpuqbr/5wHjT/N2idl5F3X53NZUlP3hSFdX1EZSmWJgp9pz+5L5hXyQHGAAAA" target="_blank">Run the query</a>

```kusto
range x from 1 to 3 step 1
| extend y = x * 2
| extend z = y * 2
| extend w = z * 2
| extend a1 = pack_array(x,y,x,z), a2 = pack_array(x, y), a3 = pack_array(w,x)
| project set_intersect(a1, a2, a3)
```

**Output**

|Column1|
|---|
|[1]|
|[2]|
|[3]|

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUUgsKlKwVShOLYkH8lKLilOTSzRSKvMSczOTNaINdRSMdBSMYzV1FOBiJjqmsZqaAI3W9uo9AAAA" target="_blank">Run the query</a>

```kusto
print arr = set_intersect(dynamic([1, 2, 3]), dynamic([4,5]))
```

**Output**

|arr|
|---|
|[]|

## See also

* [`set_union()`](setunionfunction.md)
* [`set_difference()`](setdifferencefunction.md)
