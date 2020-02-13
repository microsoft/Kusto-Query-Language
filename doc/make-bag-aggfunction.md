# make_bag() (aggregation function)

Returns a `dynamic` (JSON) property-bag (dictionary) of all the values of *Expr* in the group.

* Can be used only in context of aggregation inside [summarize](summarizeoperator.md)

**Syntax**

`summarize` `make_bag(`*Expr* [`,` *MaxSize*]`)`

**Arguments**

* *Expr*: Expression of type `dynamic` that will be used for aggregation calculation.
* *MaxSize* is an optional integer limit on the maximum number of elements returned (default is *1048576*). MaxSize value cannot exceed 1048576.

**Note**

A legacy and obsolete variant of this function: `make_dictionary()` has a default limit of *MaxSize* = 128.

**Returns**

Returns a `dynamic` (JSON) property-bag (dictionary) of all the values of *Expr* in the group which are property-bags (dictionaries).
Non-dictionary values will be skipped.
If a key appears in more than one row, an arbitrary value (out of the possible values for this key) will be chosen.

**See also**

Use the [bag_unpack()](bag-unpackplugin.md) plugin for expanding dynamic JSON objects into columns using property bag keys. 

**Examples**

<!-- csl -->
```
let T = datatable(prop:string, value:string)
[
    "prop01", "val_a",
    "prop02", "val_b",
    "prop03", "val_c",
];
T
| extend p = pack(prop, value)
| summarize dict=make_bag(p)

```

|dict|
|----|
|{ "prop01": "val_a", "prop02": "val_b", "prop03": "val_c" } |

Use [bag_unpack()](bag-unpackplugin.md) plugin for transforming the bag keys in the make_bag() output into columns. 

<!-- csl -->
```
let T = datatable(prop:string, value:string)
[
    "prop01", "val_a",
    "prop02", "val_b",
    "prop03", "val_c",
];
T
| extend p = pack(prop, value)
| summarize bag=make_bag(p)
| evaluate bag_unpack(bag) 

```

|prop01|prop02|prop03|
|---|---|---|
|val_a|val_b|val_c|
