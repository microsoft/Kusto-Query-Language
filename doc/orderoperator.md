# order operator 

Sort the rows of the input table into order by one or more columns.

<!-- csl -->
```
T | order by country asc, price desc
```

**Alias**

[sort operator](sortoperator.md)

**Syntax**

*T* `| sort by` *column* [`asc` | `desc`] [`nulls first` | `nulls last`] [`,` ...]

**Arguments**

* *T*: The table input to sort.
* *column*: Column of *T* by which to sort. The type of the values must be numeric, date, time or string.
* `asc` Sort by into ascending order, low to high. The default is `desc`, descending high to low.
* `nulls first` (the default for `asc` order) will place the null values at the beginning and `nulls last` (the default for `desc` order) will place the null values at the end.

**Example**

<!-- csl -->
```
Traces
| where ActivityId == "479671d99b7b"
| sort by Timestamp asc nulls first
```

All rows in table Traces that have a specific `ActivityId`, sorted by their timestamp. If `Timestamp` column contains null values, those will appear at the first lines of the result.

In order to exclude null values from the result add a filter before the call to sort:

<!-- csl -->
```
Traces
| where ActivityId == "479671d99b7b" and isnotnull(Timestamp)
| sort by Timestamp asc
```