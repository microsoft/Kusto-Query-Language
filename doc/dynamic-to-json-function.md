---
title: dynamic_to_json() - Azure Data Explorer 
description: This article describes dynamic_to_json() in Azure Data Explorer.
ms.reviewer: elgevork
ms.topic: reference
ms.date: 07/05/2021
---
# dynamic_to_json()

Converts a scalar value of type `dynamic` to a canonical `string` representation.

## Syntax

`dynamic_to_json(Expr)`

## Arguments

* *Expr*: Expression of `dynamic` type. The function accepts one argument.

## Returns

Returns a canonical representation of the input as a value of type `string`,
according to the following rules:

* If the input is a scalar value of type other than `dynamic`,
   the output is the application of `tostring()` to that value.

* If the input in an array of values, the output is composed of the
   characters `[`, `,`, and `]` interspersed with the canonical representation
   described here of each array element.

* If the input is a property bag, the output is composed of the characters
   `{`, `,`, and `}` interspersed with the colon (`:`)-delimited name/value pairs
   of the properties. The pairs are sorted by the names, and the values
   are in the canonical representation described here of each array element.

## Examples

Expression:

```kusto
let bag1 = dynamic_to_json(
  dynamic({
    'Y10':dynamic({}),
    'X8': dynamic({
      'c3':1,
      'd8':5,
      'a4':6
    }),
    'D1':114,
    'A1':12,
    'B1':2,
    'C1':3,
    'A14':[15, 13, 18]
}));
let bag2 = dynamic_to_json(
  dynamic({
    'X8': dynamic({
      'a4':6,
      'c3':1,
      'd8':5
    }),
    'A14':[15, 13, 18],
    'C1':3,
    'B1':2,
    'Y10': dynamic({}),
    'A1':12, 'D1':114
  }));
print AreEqual=bag1 == bag2, Result=bag1
```
  
Result:

|AreEqual|Result|
|---|---|
|true|{"A1":12,"A14":[15,13,18],"B1":2,"C1":3,"D1":114,"X8":{"a4":6,"c3":1,"d8":5},"Y10":{}}|

