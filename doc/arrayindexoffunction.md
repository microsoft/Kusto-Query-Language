# array_index_of()

Searches the array for the specified item, and returns its position.

**Syntax**

`array_index_of(`*array*,*value*`)`

**Arguments**

* *array*: Input array to search.
* *value*: Value to search for. The value should be of type long, integer, double, datetime, timespan, decimal, string or guid.

**Returns**

Zero-based index position of lookup.
Returns -1 if the value is not found in the array.

**Example**

<!-- csl: https://help.kusto.windows.net:443/Samples -->
```
print arr=dynamic(["this", "is", "an", "example"]) 
| project Result=array_index_of(arr, "example")
```

|Result|
|---|
|3|

**See also**

If you only want to check whether a value exists in an array,
but you are not interested in its position, you can use
[set_has_element(arr, value)](sethaselementfunction.md) -
this will improve the readability of your query, but performance-wise
both functions are the same.
