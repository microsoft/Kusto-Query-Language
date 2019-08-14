# avg() (aggregation function)

Calculates the average of *Expr* across the group. 

* Can be used only in context of aggregation inside [summarize](summarizeoperator.md)

**Syntax**

summarize `avg(`*Expr*`)`

**Arguments**

* *Expr*: Expression that will be used for aggregation calculation. 

**Returns**

The average value of *Expr* across the group.
 
