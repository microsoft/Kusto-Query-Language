# make_list() (aggregation function)

Returns a `dynamic` (JSON) array of all the values of *Expr* in the group.

* Can be used only in context of aggregation inside [summarize](summarizeoperator.md)

**Syntax**

`summarize` `make_list(`*Expr* [`,` *MaxSize*]`)`

**Arguments**

* *Expr*: Expression that will be used for aggregation calculation.
* *MaxSize* is an optional integer limit on the maximum number of elements returned (default is *1048576*). MaxSize value cannot exceed 1048576.

> [!NOTE]
> A legacy and obsolete variant of this function: `makelist()` has a default limit of *MaxSize* = 128.

**Returns**

Returns a `dynamic` (JSON) array of all the values of *Expr* in the group.
If the input to the `summarize` operator is not sorted, the order of elements in the resulting array is undefined.
If the input to the `summarize` operator is sorted, the order of elements in the resulting array tracks that of the input.

> [!TIP]
> Use the [`mv-apply`](./mv-applyoperator.md) operator to create an ordered list by some key. See examples [here](./mv-applyoperator.md#using-mv-apply-operator-to-sort-the-output-of-makelist-aggregate-by-some-key).

**See also**

[`make_list_if`](./makelistif-aggfunction.md) operator is similar to `make_list`, except it also accepts a predicate.
