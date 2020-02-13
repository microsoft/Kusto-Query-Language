# tobool()

Converts input to boolean (signed 8-bit) representation.

<!-- csl -->
```
tobool("true") == true
tobool("false") == false
tobool(1) == true
tobool(123) == true
```

**Syntax**

`tobool(`*Expr*`)`
`toboolean(`*Expr*`)` (alias)

**Arguments**

* *Expr*: Expression that will be converted to boolean. 

**Returns**

If conversion is successful, result will be a boolean.
If conversion is not successful, result will be `null`.
 
