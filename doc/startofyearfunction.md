# startofyear()

Returns the start of the year containing the date, shifted by an offset, if provided.

**Syntax**

`startofyear(`*date* [`,`*offset*]`)`

**Arguments**

* `date`: The input date.
* `offset`: An optional number of offset years from the input date (integer, default - 0). 

**Returns**

A datetime representing the start of the year for the given *date* value, with the offset, if specified.

**Example**

<!-- csl -->
```
  range offset from -1 to 1 step 1
 | project yearStart = startofyear(datetime(2017-01-01 10:10:17), offset) 
```

|yearStart|
|---|
|2016-01-01 00:00:00.0000000|
|2017-01-01 00:00:00.0000000|
|2018-01-01 00:00:00.0000000|
