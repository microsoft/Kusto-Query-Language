# endofmonth()

Returns the end of the month containing the date, shifted by an offset, if provided.

**Syntax**

`endofmonth(`*date* [`,`*offset*]`)`

**Arguments**

* `date`: The input date.
* `offset`: An optional number of offset months from the input date (integer, default - 0).

**Returns**

A datetime representing the end of the month for the given *date* value, with the offset, if specified.

**Example**

<!-- csl -->
```
  range offset from -1 to 1 step 1
 | project monthEnd = endofmonth(datetime(2017-01-01 10:10:17), offset) 
```

|monthEnd|
|---|
|2016-12-31 23:59:59.9999999|
|2017-01-31 23:59:59.9999999|
|2017-02-28 23:59:59.9999999|
