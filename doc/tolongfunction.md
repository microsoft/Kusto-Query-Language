# tolong()

Converts input to long (signed 64-bit) number representation.

<!-- csl -->
```
tolong("123") == 123
```

**Syntax**

`tolong(`*Expr*`)`

**Arguments**

* *Expr*: Expression that will be converted to long. 

**Returns**

If conversion is successful, result will be a long number.
If conversion is not successful, result will be `null`.
 
*Note*: Prefer using [long()](./scalar-data-types/long.md) when possible.
