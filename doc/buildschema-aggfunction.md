---
title: buildschema() (aggregation function) - Azure Data Explorer
description: Learn how to use the buildschema() function to build a table schema from a dynamic expression.
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/23/2022
---
# buildschema() (aggregation function)

Builds the minimal schema that admits all values of *DynamicExpr*.

[!INCLUDE [data-explorer-agg-function-summarize-note](../../includes/data-explorer-agg-function-summarize-note.md)]

## Syntax

`buildschema` `(`*DynamicExpr*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
|*DynamicExpr*| dynamic | &check; | Expression used for the aggregation calculation.

## Returns

Returns the minimal schema that admits all values of *DynamicExpr*.

> [!TIP]
> If `buildschema(json_column)` gives a syntax error:
>
> > *Is your `json_column` a string rather than a dynamic object?*
>
> then use `buildschema(parsejson(json_column))`.

## Example

The following example builds a schema based on:

* `{"x":1, "y":3.5}`
* `{"x":"somevalue", "z":[1, 2, 3]}`
* `{"y":{"w":"zzz"}, "t":["aa", "bb"], "z":["foo"]}`

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA2WOQQrCMBBF9z3F8FctBEGLm1ylZDFpIgYSA7ZVm9q7O2p3nVkN/73POB5lbfT1g+PkNbn5xin0DXUVyWxnveAFfVSEGbo9nNdG7WMMOflfDQQs0J0IJ0Wt2eNSs+ApTikFq+Cj4GD+mtbCbAW45AzRK1O9aZhS4nsonuwUohv6q0/8/7v5AOnXbR3IAAAA" target="_blank">Run the query</a>

```kusto
datatable(value: dynamic) [
    dynamic({"x":1, "y":3.5}),
    dynamic({"x":"somevalue", "z":[1, 2, 3]}),
    dynamic({"y":{"w":"zzz"}, "t":["aa", "bb"], "z":["foo"]})
]
| summarize buildschema(value)
```

**Results**

|schema_value|
|--|
|{"x":["long","string"],"y":["double",{"w":"string"}],"z":{"`indexer`":["long","string"]},"t":{"`indexer`":"string"}}|

The resulting schema tells us that:

* The root object is a container with four properties named x, y, z, and t.
* The property called `x` is of type *long* or of type *string*.
* The property called `y` ii of type *double*, or another container with a property called `w` of type *string*.
* The `indexer` keyword indicates that `z` and `t` are arrays.
* Each item in the array `z` is of type *long* or of type *string*.
* `t` is an array of strings.
* Every property is implicitly optional, and any array may be empty.

### Schema model

The syntax of the returned schema is:

Container ::= '{' Named-type* '}';
Named-type: := (name | '"`indexer`"') ':' Type;
Type ::= Primitive-type | Union-type | Container;
Union-type ::= '[' Type* ']';
Primitive-type ::= "long" | "string" | ...;

The values are equivalent to a subset of TypeScript type annotations, encoded as a Kusto dynamic value.
In TypeScript, the example schema would be:

```typescript
var someobject:
{
    x?: (number | string),
    y?: (number | { w?: string}),
    z?: { [n:number] : (long | string)},
    t?: { [n:number]: string }
}
```
