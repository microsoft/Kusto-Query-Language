---
title: set_difference() - Azure Data Explorer
description: Learn how to use the set_difference() function to create a difference set of all distinct values in the first array that aren't in the other array inputs.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/30/2023
---
# set_difference()

Returns a `dynamic` (JSON) array of the set of all distinct values that are in the first array but aren't in other arrays - (((arr1 \ arr2) \ arr3) \ ...).

## Syntax

`set_difference(`*set1*`,` *set2* [`,`*set3*, ...]`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *set1...setN* | dynamic | &check; | Arrays used to create a difference set. A minimum of 2 arrays are required. See [pack_array](packarrayfunction.md).|

## Returns

Returns a dynamic array of the set of all distinct values that are in *set1* but aren't in other arrays.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA23MsQ7CMAwE0J2vuLFBWdLOfEtltQ4CRBK5kZpE/XicCVWweHi+O6FwZxR4iW845IgJW+YEdznAJXNYUXHTxBXjl5pSPdOu1M5ETi3R8ppJhOpQLKqF3mYsaPx9dp7+dXajq0nik5eMjfO8Prxn4bDwQK6P9ab5AIGKxtfOAAAA" target="_blank">Run the query</a>

```kusto
range x from 1 to 3 step 1
| extend y = x * 2
| extend z = y * 2
| extend w = z * 2
| extend a1 = pack_array(x,y,x,z), a2 = pack_array(x, y), a3 = pack_array(x,y,w)
| project set_difference(a1, a2, a3)
```

**Output**

|Column1|
|---|
|[4]|
|[8]|
|[12]|

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUUgsKlKwVShOLYlPyUxLSy1KzUtO1UipzEvMzUzWiDbUMdIxjtXUUUAX0QQAej8Kqz4AAAA=" target="_blank">Run the query</a>

```kusto
print arr = set_difference(dynamic([1,2,3]), dynamic([1,2,3]))
```

**Output**

|arr|
|---|
|[]|

## See also

* [`set_union()`](setunionfunction.md)
* [`set_intersect()`](setintersectfunction.md)
