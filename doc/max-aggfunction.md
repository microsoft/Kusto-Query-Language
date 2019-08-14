# max() (aggregation function)

Returns the maximum value across the group. 

* Can be used only in context of aggregation inside [summarize](summarizeoperator.md)

**Syntax**

`summarize` `max(`*Expr*`)`

**Arguments**

* *Expr*: Expression that will be used for aggregation calculation. 

**Returns**

The maximum value of *Expr* across the group.
 
> [!TIP]
> This gives you the min or max on its own - for example, the highest or lowest price.
> But if you want other columns in the row - for example, the name of the supplier with the lowest
> price - use [arg_max](arg-max-aggfunction.md) or [arg_min](arg-min-aggfunction.md).
