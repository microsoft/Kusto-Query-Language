# anyif() (aggregation function)

The `anyif()` aggregation function returns random non-empty value from the specified expression values for which *Predicate* evaluates to `true`.

* This is useful when a column has a large number of values
(e.g., an "error text" column) and you want to sample that column once per a unique value of the compound group key.
* Can only be used in context of aggregation inside [summarize](summarizeoperator.md)

> [!NOTE]
> There are *no guarantees* about which record will be returned; the algorithm for selecting that record is undocumented and one should not assume it is stable.

**Syntax**

`summarize` `anyif(`*Expr*, `*Predicate*`)`

**Arguments**

* *Expr*: Expression that will be used for aggregation calculation.
* *Predicate*: Predicate that if true, the *Expr* will be used for aggregation calculation.

**Returns**

Randomly selects one row of the group and returns the value of the specified expression for which *Predicate* evaluates to `true`.


**Examples**

Show random continent which has a population from 300 million to 600 million:

<!-- csl -->
```
Continents | summarize anyif(Continent, Population between (300000000 .. 600000000))
```

![alt text](./images/aggregations/any1.png "any1")
