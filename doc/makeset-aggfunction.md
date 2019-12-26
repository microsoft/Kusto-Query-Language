# make_set() (aggregation function)

Returns a `dynamic` (JSON) array of the set of distinct values that *Expr* takes in the group.

* Can be used only in context of aggregation inside [summarize](summarizeoperator.md)

**Syntax**

`summarize` `make_set(`*Expr* [`,` *MaxSize*]`)`

**Arguments**

* *Expr*: Expression that will be used for aggregation calculation.
* *MaxSize* is an optional integer limit on the maximum number of elements returned (default is *1048576*). MaxSize value cannot exceed 1048576.

**Note**

A legacy and obsolete variant of this function: `makeset()` has a default limit of *MaxSize* = 128.

**Returns**

Returns a `dynamic` (JSON) array of the set of distinct values that *Expr* takes in the group.
The array's sort order is undefined.

> [!TIP]
> To just count the distinct values, use [dcount()](dcount-aggfunction.md)

**Example**

<!--csl -->
```
PageViewLog 
| summarize countries=make_set(country) by continent
```

![alt text](./images/aggregations/makeset.png "makeset")

See also the [`mv-expand` operator](mvexpandoperator.md) for the opposite function.
