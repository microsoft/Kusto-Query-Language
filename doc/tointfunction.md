# toint()

Converts input to integer (signed 32-bit) number representation.

<!-- csl -->
```
toint("123") == 123
```

**Syntax**

`toint(`*Expr*`)`

**Arguments**

* *Expr*: Expression that will be converted to integer. 

**Returns**

If conversion is successful, result will be a integer number.
If conversion is not successful, result will be `null`.
 
*Note*: Prefer using [int()](./scalar-data-types/int.md) when possible.
