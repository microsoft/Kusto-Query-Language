---
title: bag_has_key() - Azure Data Explorer
description: Learn how to use the bag_has_key() function to check if a dynamic property bag object contains a given key. 
ms.reviewer: afridman
ms.topic: reference
ms.date: 11/23/2022
---
# bag_has_key()

Checks whether a dynamic property bag object contains a given key.

## Syntax

`bag_has_key(`*bag*`,`*key*`)`

## Parameters

| Name | Type | Required | Description |
| -- | -- | -- | -- |
| *bag* | dynamic | &check; | The property bag to search. |
| *key* | string | &check; | The key for which to search.  Search for a nested key using the [JSONPath](jsonpath.md) notation. Array indexing isn't supported. |

## Returns

True or false depending on if the key exists in the bag.

## Examples

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA0tJLAHCpJxUjcy8gtISK4WUyrzE3MxkTa5oLgUggHI1qtWzUysN1RWsFAyNjHUUQDwjdSsF9cSkZPVaTR1Cao2R1cZy1SikVpSk5qUoFKUWl+aUKNgqJCWmx2ckFscD1UJcArNDEwDPKMflogAAAA==" target="_blank">Run the query</a>

```kusto
datatable(input: dynamic)
[
    dynamic({'key1' : 123, 'key2': 'abc'}),
    dynamic({'key1' : 123, 'key3': 'abc'}),
]
| extend result = bag_has_key(input, 'key2')
```

**Output**

|input|result|
|---|---|
|{<br>  "key1": 123,<br>  "key2": "abc"<br>}|true<br>|
|{<br>  "key1": 123,<br>  "key3": "abc"<br>}|false<br>|

### Search using a JSONPath key

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAy2OwQqDMBBE7/mKPRTWQJAk3gL9EhGJGlqpTUUjGK3/3g3p7unN7DA72EDbTa4Y/bwFA0P09j32nNUMaP5YnPhyUaEBpSsBCTTBifPymRWCAbRdj+QkIVm4xwOvfFoR10pKAVrK5uKCNewLbg/OD7C4dZsC3KGzj/Zp15YC+RkK38rUVOYW/gOk4uu+rQAAAA==" target="_blank">Run the query</a>

```kusto
datatable(input: dynamic)
[
    dynamic({'key1': 123, 'key2': {'prop1' : 'abc', 'prop2': 'xyz'}, 'key3': [100, 200]}),
]
| extend result = bag_has_key(input, '$.key2.prop1')
```

**Output**

|input|result|
|---|---|
|{<br>  "key1": 123,<br>  "key2": {<br>    "prop1": "abc",<br>    "prop2": "xyz"<br>  },<br>  "key3": [<br>    100,<br>    200<br>  ]<br>}|true<br>|
