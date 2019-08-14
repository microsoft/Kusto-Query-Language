# count operator

Returns the number of records in the input record set.

**Syntax**

`T | count`

**Arguments**

* *T*: The tabular data whose records are to be counted.

**Returns**

This function returns a table with a single record and column of type
`long`. The value of the only cell is the number of records in *T*. 

**Example**

<!-- csl: https://help.kusto.windows.net/Samples -->
```
StormEvents | count
```
