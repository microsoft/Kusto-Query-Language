---
title:  dynamic_to_json() 
description: Learn how to use the dynamic_to_json() function to convert a scalar value of type `dynamic` to a canonical string representation.
ms.reviewer: elgevork
ms.topic: reference
ms.date: 03/09/2023
---
# dynamic_to_json()

Converts a scalar value of type `dynamic` to a canonical `string` representation.

## Syntax

`dynamic_to_json(`*expr*`)`

## Parameters

|Name|Type|Required|Description|
|--|--|--|--|
| *expr* | dynamic | &check; | The expression to convert to string representation.|

## Returns

Returns a canonical representation of the input as a value of type `string`,
according to the following rules:

* If the input is a scalar value of type other than `dynamic`,
   the output is the application of `tostring()` to that value.

* If the input is an array of values, the output is composed of the
   characters `[`, `,`, and `]` interspersed with the canonical representation
   described here of each array element.

* If the input is a property bag, the output is composed of the characters
   `{`, `,`, and `}` interspersed with the colon (`:`)-delimited name/value pairs
   of the properties. The pairs are sorted by the names, and the values
   are in the canonical representation described here of each array element.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA8tJLVFISkw3VLBVSKnMS8zNTI4vyY/PKs7P0+BSgAlpVAPZCgrqkYYG6lZwsVpNHYhwhIW6FZpSoGiysbqVoQ6MlwJUYwrnJZqoW5mBOXBDXAyByg1NoDxHEM8IynECcmBsZyDbGK4IaEy0oamOgqExEFvEctVqalpz5UC8ZESUl7C7Hew+HTw+QXU7hkOwOBbZE+BwVMAMSKiv4YHBBbID6KOCosy8EgXHolTXwtLEHFtIfNmCPamjEJRaXJpTAhYEAM5EMCHNAQAA" target="_blank">Run the query</a>

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
  
**Output**

|AreEqual|Result|
|---|---|
|true|{"A1":12,"A14":[15,13,18],"B1":2,"C1":3,"D1":114,"X8":{"a4":6,"c3":1,"d8":5},"Y10":{}}|
