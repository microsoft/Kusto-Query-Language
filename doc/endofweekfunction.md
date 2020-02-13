# endofweek()

Returns the end of the week containing the date, shifted by an offset, if provided.

Last day of the week is considered to be a Saturday.

**Syntax**

`endofweek(`*date* [`,`*offset*]`)`

**Arguments**

* `date`: The input date.
* `offset`: An optional number of offset weeks from the input date (integer, default - 0).

**Returns**

A datetime representing the end of the week for the given *date* value, with the offset, if specified.

**Example**

<!-- csl -->
```
  range offset from -1 to 1 step 1
 | project weekEnd = endofweek(datetime(2017-01-01 10:10:17), offset)  

```

|weekEnd|
|---|
|2016-12-31 23:59:59.9999999|
|2017-01-07 23:59:59.9999999|
|2017-01-14 23:59:59.9999999|
