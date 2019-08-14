# sum() (aggregation function)

Calculates the sum of *Expr* across the group. 

* Can be used only in context of aggregation inside [summarize](summarizeoperator.md)

**Syntax**

summarize `sum(`*Expr*`)`

**Arguments**

* *Expr*: Expression that will be used for aggregation calculation. 

**Returns**

The sum value of *Expr* across the group.
 
