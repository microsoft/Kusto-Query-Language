---
title: bag_set_key() - Azure Data Explorer
description: Learn how to use the bag_set_key() function to set a given key to a given value in a dynamic property-bag. 
ms.reviewer: afridman
ms.topic: reference
ms.date: 04/16/2023
---
# bag_set_key()

bag_set_key() receives a `dynamic` property-bag, a key and a value. The function sets the given key in the bag to the given value. The function will override any existing value in case the key already exists.

## Syntax

`bag_set_key(`*bag*`,`*key*`,`*value*`)`

## Parameters

| Name | Type | Required | Description |
| -- | -- | -- | -- |
| *bag* | dynamic | &check; | The property bag to modify. |
| *key* | string | &check; | The key to set. Either a JSON path (you can specify a key on the nested levels using [JSONPath](jsonpath.md) notation) or the key name for a root level key. Array indexing or root JSON path aren't supported. |
| *value* | any scalar data type | &check; | The value to which the key is set. |

## Returns

Returns a `dynamic` property-bag with specified key-value pairs. If the input bag isn't a property-bag, a `null` value is returned.

> [!NOTE]
> To treat `null`s as empty bags, use `coalesce(x, dynamic({}))`​.

## Examples

### Use a root-level key

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA0tJLAHCpJxUjcy8gtISK4WUyrzE3MxkTYVoXi4FIIDyNarVs1MrDdWtFAx1FEBMIyDTqFZTRwG/MmMgUz0xKVkdqJSXK5aXq0YhtaIkNS9FoSi1uDSnRMFWISkxPb44tSQeqBriCJhOHQVjTQAoaFq+oAAAAA==" target="_blank">Run the query</a>

```kusto
datatable(input: dynamic) [
    dynamic({'key1': 1, 'key2': 2}), 
    dynamic({'key1': 1, 'key3': 'abc'}),
]
| extend result = bag_set_key(input, 'key3', 3)
```

|input|result|
|---|---|
|{<br>  "key1": 1,<br>  "key2": 2<br>}|{<br>  "key1": 1,<br>  "key2": 2,<br>  "key3": 3<br>}|
|{<br>  "key1": 1,<br>  "key3": "abc"<br>}|{<br>  "key1": 1,<br>  "key3": 3<br>}|

### Use a JSONPath key

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA0tJLAHCpJxUjcy8gtISK4WUyrzE3MxkzWheLgUggHI1qtWzUysN1a0UDI2MdRRAHCMgp1q9oCi/ACEM4oHE1Ssqq9RrazV1cBtSq8nLFcvLVaOQWlGSmpeiUJRaXJpTomCrkJSYHl+cWhIPVApxE9BcFT2QhXoQy4D8xKRkdU0AFVPFlLsAAAA=" target="_blank">Run the query</a>

```kusto
datatable(input: dynamic)[
    dynamic({'key1': 123, 'key2': {'prop1': 123, 'prop2': 'xyz'}}),
    dynamic({'key1': 123})
]
| extend result = bag_set_key(input, '$.key2.prop1', 'abc')
```

|input|result|
|---|---|
|{<br>  "key1": 123,<br>  "key2": {<br>  "prop1": 123,<br>  "prop2": "xyz"<br>}<br>}|{<br>  "key1": 123,<br>  "key2": {<br>  "prop1": "abc",<br>  "prop2": "xyz"<br>}<br>}|
|{<br>  "key1": 123<br>}|{<br>  "key1": 123,<br>  "key2": {<br>  "prop1": "abc"<br>}<br>}|
