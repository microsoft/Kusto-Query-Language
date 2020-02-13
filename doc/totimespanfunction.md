# totimespan()

Converts input  to [timespan](./scalar-data-types/timespan.md) scalar.

<!-- csl -->
```
totimespan("0.00:01:00") == time(1min)
```

**Syntax**

`totimespan(`*Expr*`)`

**Arguments**

* *Expr*: Expression that will be converted to [timespan](./scalar-data-types/timespan.md). 

**Returns**

If conversion is successful, result will be a [timespan](./scalar-data-types/timespan.md) value.
If conversion is not successful, result will be null.
 
