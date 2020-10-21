---
title: buildschema() (aggregation function) - Azure Data Explorer
description: This article describes buildschema() (aggregation function) in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 10/23/2018
---
# buildschema() (aggregation function)

Returns the minimal schema that admits all values of *DynamicExpr*.

* Can be used only in context of aggregation, inside [summarize](summarizeoperator.md)

## Syntax

summarize `buildschema(`*DynamicExpr*`)`

## Arguments

* *DynamicExpr*: Expression that is used for the aggregation calculation. The parameter column type must be `dynamic`. 

## Returns

The maximum value of *`Expr`* across the group.

> [!TIP] 
> If `buildschema(json_column)` gives a syntax error:
>
> > *Is your `json_column` a string rather than a dynamic object?*
>
> then use `buildschema(parsejson(json_column))`.

## Example

Assume the input column has three dynamic values.

* `{"x":1, "y":3.5}`
* `{"x":"somevalue", "z":[1, 2, 3]}`
* `{"y":{"w":"zzz"}, "t":["aa", "bb"], "z":["foo"]}`

The resulting schema would be:

```kusto
{ 
    "x":["int", "string"],
    "y":["double", {"w": "string"}],
    "z":{"`indexer`": ["int", "string"]},
    "t":{"`indexer`": "string"}
}
```

The schema tells us that:

* The root object is a container with four properties named x, y, z, and t.
* The property called "x" that could be of type "int" or of type "string".
* The property called "y" that could be of type "double", or another container with a property called "w" of type "string".
* The ``indexer`` keyword indicates that "z" and "t" are arrays.
* Each item in the array "z" is of type "int" or of type "string".
* "t" is an array of strings.
* Every property is implicitly optional, and any array may be empty.

### Schema model

The syntax of the returned schema is:

```output
Container ::= '{' Named-type* '}';
Named-type ::= (name | '"`indexer`"') ':' Type;
Type ::= Primitive-type | Union-type | Container;
Union-type ::= '[' Type* ']';
Primitive-type ::= "int" | "string" | ...;
```

The values are equivalent to a subset of the TypeScript type annotations, encoded as a Kusto dynamic value. 
In Typescript, the example schema would be:

```typescript
var someobject: 
{
    x?: (number | string),
    y?: (number | { w?: string}),
    z?: { [n:number] : (int | string)},
    t?: { [n:number]: string }
}
```
