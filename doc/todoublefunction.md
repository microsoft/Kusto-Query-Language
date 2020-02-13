# todouble()/toreal()

Converts the input to a value of type `real`. (`todouble()` and `toreal()` are synonyms.)

<!-- csl -->
```
toreal("123.4") == 123.4
```

**Syntax**

`toreal(`*Expr*`)`
`todouble(`*Expr*`)`

**Arguments**

* *Expr*: An expression whose value will be converted to a value of type `real`.

**Returns**

If conversion is successful, the result is a value of type `real`.
If conversion is not successful, the result is the value `real(null)`.

*Note*: Prefer using [double() or real()](./scalar-data-types/real.md) when possible.
