# avg() (aggregation function)

Calculates the average of *Expr* across the group. 

* Can only be used in context of aggregation inside [summarize](summarizeoperator.md)

**Syntax**

summarize `avg(`*Expr*`)`

**Arguments**

* *Expr*: Expression that will be used for aggregation calculation. Records with `null` values are ignored and not included in the calculation.

**Returns**

The average value of *Expr* across the group.
 
