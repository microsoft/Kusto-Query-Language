# isnull()

Evaluates its sole argument and returns a `bool` value indicating if the argument evaluates to a null value.

<!-- csl -->
```
isnull(parse_json("")) == true
```

**Syntax**

`isnull(`*Expr*`)`

**Returns**

True or false depending on the whether the value is null or not null.

**Notes**

* `string` values cannot be null. Use [isempty](./isemptyfunction.md)
  to determine if a value of type `string` is empty or not.

|x                |`isnull(x)`|
|-----------------|-----------|
|`""`             |`false`    |
|`"x"`            |`false`    |
|`parse_json("")`  |`true`     |
|`parse_json("[]")`|`false`    |
|`parse_json("{}")`|`false`    |

**Example**

<!-- csl -->
```
T | where isnull(PossiblyNull) | count
```
