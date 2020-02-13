# minif() (aggregation function)

Returns the minimum value across the group for which *Predicate* evaluates to `true`.

* Can be used only in context of aggregation inside [summarize](summarizeoperator.md)

See also - [min()](min-aggfunction.md) function, which returns the minimum value across the group without predicate expression.

**Syntax**

`summarize` `minif(`*Expr*`,`*Predicate*`)`

**Arguments**

* *Expr*: Expression that will be used for aggregation calculation.
* *Predicate*: predicate that if true, the *Expr* calculated value will be checked for minimum.

**Returns**

The minimum value of *Expr* across the group for which *Predicate* evaluates to `true`.

**Examples**

<!-- csl -->
```
range x from 1 to 100 step 1
| summarize minif(x, x > 50)
```

|minif_x|
|---|
|51|
