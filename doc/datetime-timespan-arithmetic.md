---
title:  Datetime / timespan arithmetic
description: This article describes Datetime / timespan arithmetic in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 03/16/2023
---
# Datetime / timespan arithmetic

Kusto supports performing arithmetic operations on values of types `datetime`
and `timespan`.

## Supported operations

* One can subtract (but not add) two `datetime` values to get a `timespan` value
  expressing their difference.
  For example, `datetime(1997-06-25) - datetime(1910-06-11)` is how old was
  [Jacques-Yves Cousteau](https://en.wikipedia.org/wiki/Jacques_Cousteau) when
  he died.

* One can add or subtract two `timespan` values to get a `timespan` value
  which is their sum or difference.
  For example, `1d + 2d` is three days.

* One can add or subtract a `timespan` value from a `datetime` value.
  For example, `datetime(1910-06-11) + 1d` is the date Cousteau turned one day old.

* One can divide two `timespan` values to get their quotient.
  For example, `1d / 5h` gives `4.8`.
  This gives one the ability to express any `timespan` value as a multiple of
  another `timespan` value. For example, to express an hour in seconds, simply
  divide `1h` by `1s`: `1h / 1s` (with the obvious result, `3600`).

* Conversely, one can multiple a numeric value (such as `double` and `long`)
  by a `timespan` value to get a `timespan` value.
  For example, one can express an hour and a half as `1.5 * 1h`.

## Examples

[Unix time](https://en.wikipedia.org/wiki/Unix_time), which is also known as POSIX time or UNIX Epoch time,
is a system for describing a point in time as the number of seconds that have elapsed since
00:00:00 Thursday, 1 January 1970, Coordinated Universal Time (UTC), minus leap seconds.

If your data includes representation of Unix time as an integer, or you require converting to it,
the following functions are available.

### From Unix time

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA8tJLVFIK8rPDc3LrAjJzE1VsFXQKLHKyc9L1+SqVuBSAIKUxJLUEqCUhqGluYGugSEQaSpoK5QoaCkYFqcmK3DVWnMVFGXmlSgUpRaX5pQAjUA2UcPQ1MTMwtLc1NhQEwCPpAMfbgAAAA==" target="_blank">Run the query</a>

```kusto
let fromUnixTime = (t: long) { 
    datetime(1970-01-01) + t * 1sec 
};
print result = fromUnixTime(1546897531)
```

**Output**

| result |
|---|
| 2019-01-07 21:45:31.0000000 |

### To Unix time

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA8tJLVEoyQ/Ny6wIycxNVbBV0EgpsVJISSxJLQHyNRWqFbgUgAAoqqALF9YwtDQ30DUwBCJNTQV9BcNiBa5aa66Cosy8EoWi1OLSnBKgSQhjNeAajQwMLcEazRWMDK1MTK2MDfUMIEBTEwBVI1K3jAAAAA==" target="_blank">Run the query</a>

```kusto
let toUnixTime = (dt: datetime) { 
    (dt - datetime(1970-01-01)) / 1s 
};
print result = toUnixTime(datetime(2019-01-07 21:45:31.0000000))
```

**Output**

| result |
|---|
| 1546897531 |

## See also

For unix-epoch time conversions, see the following functions:

* [unixtime_seconds_todatetime()](unixtime-seconds-todatetimefunction.md)
* [unixtime_milliseconds_todatetime()](unixtime-milliseconds-todatetimefunction.md)
* [unixtime_microseconds_todatetime()](unixtime-microseconds-todatetimefunction.md)
* [unixtime_nanoseconds_todatetime()](unixtime-nanoseconds-todatetimefunction.md)
