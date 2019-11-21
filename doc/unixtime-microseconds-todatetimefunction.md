# unixtime_microseconds_todatetime()

Converts unix-epoch microseconds to UTC datetime.

**Syntax**

`unixtime_microseconds_todatetime(*microseconds*)`

**Arguments**

* *microseconds*: A real number represents epoch timestamp in microseconds. Datetimes that occurred before the epoch time (1970-01-01 00:00:00) would have a negative timestamp value.

**Returns**

If conversion is successful, result will be a [datetime](./scalar-data-types/datetime.md) value, otherwise, result will be null.

**See also**

* For converting unix-epoch seconds to UTC datetime, see [unixtime_seconds_todatetime()](unixtime-seconds-todatetimefunction.md).
* For converting unix-epoch milliseconds to UTC datetime, see [unixtime_milliseconds_todatetime()](unixtime-milliseconds-todatetimefunction.md).
* For converting unix-epoch nanoseconds to UTC datetime, see [unixtime_nanoseconds_todatetime()](unixtime-nanoseconds-todatetimefunction.md).

**Example**

<!-- csl: https://help.kusto.windows.net/Samples  -->
```
print date_time = unixtime_microseconds_todatetime(1546300800000000)
```

|date_time|
|---|
|2019-01-01 00:00:00.0000000|
