# array_split()

Splits an array to multiple arrays according to the split indices and packs the generated array in a dynamic array.

**Syntax**

`array_split(`*arr*, *indices*`)`

**Arguments**

* *arr*: Input array to split, must be dynamic array.
* *indices*: Integer or dynamic array of integers with the split indices (zero based), negative values are converted to array_length + value.

**Returns**

Dynamic array containing N+1 arrays with the values in the range [0..i1), [i1..i2), ... [iN..array_length) from arr, where N is the number of input indices and i1..iN are the indices.

**Examples**

1.
<!-- csl: https://help.kusto.windows.net:443/Samples -->
```
print arr=dynamic([1,2,3,4,5]) 
| extend arr_split=array_split(arr, 2)
```
|arr|arr_split|
|---|---|
|[1,2,3,4,5]|[[1,2],[3,4,5]]|



2.
<!-- csl: https://help.kusto.windows.net:443/Samples -->
```
print arr=dynamic([1,2,3,4,5]) 
| extend arr_split=array_split(arr, dynamic([1,3]))
```
|arr|arr_split|
|---|---|
|[1,2,3,4,5]|[[1],[2,3],[4,5]]|
