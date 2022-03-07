---
title: bag_remove_keys() - Azure Data Explorer
description: This article describes bag_remove_keys() in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/21/2021
---
# bag_remove_keys()

Removes keys and associated values from a `dynamic` property-bag.

## Syntax

`bag_remove_keys(`*bag*`, `*keys*`)`

## Arguments

* *bag*: `dynamic` property-bag input.
* *keys*: `dynamic` array includes keys to be removed from the input. Keys refer to the first level of the property bag.
You can specify keys on the nested levels using [JSONPath](jsonpath.md) notation.

## Returns

Returns a `dynamic` property-bag without specified keys and their values.

## Example

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
datatable(input:dynamic)
[
    dynamic({'key1' : 123,     'key2': 'abc'}),
    dynamic({'key1' : 'value', 'key3': 42.0}),
]
| extend result=bag_remove_keys(input, dynamic(['key2', 'key4']))
```

|input|result|
|---|---|
|{<br>  "key1": 123,<br>  "key2": "abc"<br>}|{<br>  "key1": 123<br>}|
|{<br>  "key1": "value",<br>  "key3": 42.0<br>}|{<br>  "key1": "value",<br>  "key3": 42.0<br>}|

### Remove inner properties of dynamic values using JSONPath notation

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
datatable(input:dynamic)
[
    dynamic({'key1': 123, 'key2': {'prop1' : 'abc', 'prop2': 'xyz'}, 'key3': [100, 200]}),
]
| extend result=bag_remove_keys(input, dynamic(['$.key2.prop1', 'key3']))
```

|input|result|
|---|---|
|{<br>  "key1": 123,<br>  "key2": {<br>    "prop1": "abc",<br>    "prop2": "xyz"<br>  },<br>  "key3": [<br>    100,<br>    200<br>  ]<br>}|{<br>  "key1": 123,<br>  "key2": {<br>    "prop2": "xyz"<br>  }<br>}|
