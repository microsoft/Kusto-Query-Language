# repeat()

Generates a dynamic array holding a series of equal values.

**Syntax**

`repeat(`*value*`,` *count*`)` 

**Arguments**

* *value*: The value of the element in the resulting array. The type of *value* can be boolean, integer, long, real, datetime, or timespan.   
* *count*: The count of the elements in the resulting array. The *count* must be an integer number.
If *count* is equal to zero, a empty array is returned.
If *count* is less than zero, a null value is returned. 

**Examples**

The following example returns `[1, 1, 1]`:

<!-- csl -->
```
T | extend r = repeat(1, 3)
```
