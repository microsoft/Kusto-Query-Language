---
title: bag_has_key() - Azure Data Explorer
description: Learn how to use the bag_has_key() function to check if a dynamic bag column contains a given key. 
ms.reviewer: afridman
ms.topic: reference
ms.date: 11/03/2022
---
# bag_has_key()

Checks whether a dynamic bag column contains a given key.

## Syntax

`bag_has_key(`*bag*`,`*key*`)`

## Arguments

| Name | Type | Required | Description |
| -- | -- | -- | -- |
| *bag* | dynamic | &check; | The property bag to search. |
| *key* | string | &check; | The key to search for.  You can search for a nested key using the [JSONPath](jsonpath.md) notation. Array indexing isn't supported. |

## Returns

True or false depending on if the key exists in the bag.

## Example

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
datatable(input: dynamic)
[
    dynamic({'key1' : 123, 'key2': 'abc'}),
    dynamic({'key1' : 123, 'key3': 'abc'}),
]
| extend result = bag_has_key(input, 'key2')
```

|input|result|
|---|---|
|{<br>  "key1": 123,<br>  "key2": "abc"<br>}|true<br>|
|{<br>  "key1": 123,<br>  "key3": "abc"<br>}|false<br>|

### Search using a JSONPath key

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
datatable(input: dynamic)
[
    dynamic({'key1': 123, 'key2': {'prop1' : 'abc', 'prop2': 'xyz'}, 'key3': [100, 200]}),
]
| extend result = bag_has_key(input, '$.key2.prop1')
```

|input|result|
|---|---|
|{<br>  "key1": 123,<br>  "key2": {<br>    "prop1": "abc",<br>    "prop2": "xyz"<br>  },<br>  "key3": [<br>    100,<br>    200<br>  ]<br>}|true<br>|
