# set_union()

Returns a `dynamic` (JSON) array of the set of all distinct values that are in any of arrays - (arr1 âˆª arr2 âˆª ...).

**Syntax**

`set_union(`*arr1*`, `*arr2*`[`,` *arr3*, ...]``)`

**Arguments**

* *arr1...arrN*: Input arrays to create a union set (at least two arrays). All arguments must be dynamic arrays (see [pack_array](packarrayfunction.md)). 

**Returns**

Returns a dynamic array of the set of all distinct values that are in any of arrays. See [`set_intersect()`](setintersectfunction.md)  and [`set_difference()`](setdifferencefunction.md).

**Example**

<!-- csl: https://help.kusto.windows.net:443/Samples -->
```
range x from 1 to 3 step 1
| extend y = x * 2
| extend z = y * 2
| extend w = z * 2
| extend a1 = pack_array(x,y,x,z), a2 = pack_array(x, y), a3 = pack_array(w)
| project set_union(a1, a2, a3)
```

|Column1|
|---|
|[1,2,4,8]|
|[2,4,8,16]|
|[3,6,12,24]|
