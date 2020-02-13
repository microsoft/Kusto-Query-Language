# make_bag_if() (aggregation function)

Returns a `dynamic` (JSON) property-bag (dictionary) of all the values of *Expr* in the group, for which *Predicate* evaluates to `true`.

* Can be used only in context of aggregation inside [summarize](summarizeoperator.md)

**Syntax**

`summarize` `make_bag_if(`*Expr*, *Predicate* [`,` *MaxSize*]`)`

**Arguments**

* *Expr*: Expression of type `dynamic` that will be used for aggregation calculation.
* *Predicate*: Predicate that has to evaluate to `true`, in order for *Expr* to be added to the result.
* *MaxSize* is an optional integer limit on the maximum number of elements returned (default is *1048576*). MaxSize value cannot exceed 1048576.

**Returns**

Returns a `dynamic` (JSON) property-bag (dictionary) of all the values of *Expr* in the group
which are property-bags (dictionaries), for which *Predicate* evaluates to `true`.
Non-dictionary values will be skipped.
If a key appears in more than one row, an arbitrary value (out of the possible values for this key) will be chosen.

**See also**

[`make_bag`](./make-bag-aggfunction.md) function, which does the same, without predicate expression.

**Examples**

<!-- csl -->
```
let T = datatable(prop:string, value:string, predicate:bool)
[
    "prop01", "val_a", true,
    "prop02", "val_b", false,
    "prop03", "val_c", true
];
T
| extend p = pack(prop, value)
| summarize dict=make_bag_if(p, predicate)

```

|dict|
|----|
|{ "prop01": "val_a", "prop03": "val_c" } |

Use [bag_unpack()](bag-unpackplugin.md) plugin for transforming the bag keys in the make_bag_if() output into columns. 

<!-- csl -->
```
let T = datatable(prop:string, value:string, predicate:bool)
[
    "prop01", "val_a", true,
    "prop02", "val_b", false,
    "prop03", "val_c", true
];
T
| extend p = pack(prop, value)
| summarize bag=make_bag_if(p, predicate)
| evaluate bag_unpack(bag) 

```

|prop01|prop03|
|---|---|
|val_a|val_c|
