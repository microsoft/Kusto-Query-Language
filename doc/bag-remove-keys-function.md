---
title: bag_remove_keys() - Azure Data Explorer
description: Learn how to use the bag_remove_keys() function to remove keys and associated values from property bags. 
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/23/2022
---
# bag_remove_keys()

Removes keys and associated values from a `dynamic` property bag.

## Syntax

`bag_remove_keys(`*bag*`,`*keys*`)`

## Parameters

| Name | Type | Required | Description |
| -- | -- | -- | -- |
| *bag* | dynamic | &check; | The property bag from which to remove keys. |
| *keys* | string | &check; | The array keys to be removed from the input. The keys are the first level of the property bag. You can specify keys on the nested levels using [JSONPath](jsonpath.md) notation. Array indexing isn't supported. |

## Returns

Returns a `dynamic` property bag without specified keys and their values.

## Examples

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA0tJLAHCpJxUjcy8gtISq5TKvMTczGRNrmguBSCAcjWq1bNTKw3VFawUDI2MdUAyCiARI3UrBfXEpGT1Wk0dHOrVyxJzSlPVdcDqjYHqTYz0DEDKY7lqFFIrSlLzUhSKUotLc0pskxLT44tSc/PLUuOBaoshLtKBmxkNsRFikol6rKYmAD1YmXe9AAAA" target="_blank">Run the query</a>

```kusto
datatable(input:dynamic)
[
    dynamic({'key1' : 123,     'key2': 'abc'}),
    dynamic({'key1' : 'value', 'key3': 42.0}),
]
| extend result=bag_remove_keys(input, dynamic(['key2', 'key4']))
```

**Output**

|input|result|
|---|---|
|{<br>  "key1": 123,<br>  "key2": "abc"<br>}|{<br>  "key1": 123<br>}|
|{<br>  "key1": "value",<br>  "key3": 42.0<br>}|{<br>  "key1": "value",<br>  "key3": 42.0<br>}|

### Remove inner properties of dynamic values using JSONPath notation

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAz2O3QqDMAyF7/sUuRjEQpFW74Q9iRSpNgyZf2gVnfPd19JtydWXk5wTa5zvuqOkHabVFfYYTN82nJUMfH0xOfFJh8ICVJYLCJB5OHGax0khFICmbtArYRAk3I8XXnE191wqKQVkUuqLC6bZG2h3NFiYaVk7d6/No5qpHzeq/MUSvxH//BJvaQhNY+DPV3P+AU1I1W7BAAAA" target="_blank">Run the query</a>

```kusto
datatable(input:dynamic)
[
    dynamic({'key1': 123, 'key2': {'prop1' : 'abc', 'prop2': 'xyz'}, 'key3': [100, 200]}),
]
| extend result=bag_remove_keys(input, dynamic(['$.key2.prop1', 'key3']))
```

**Output**

|input|result|
|---|---|
|{<br>  "key1": 123,<br>  "key2": {<br>    "prop1": "abc",<br>    "prop2": "xyz"<br>  },<br>  "key3": [<br>    100,<br>    200<br>  ]<br>}|{<br>  "key1": 123,<br>  "key2": {<br>    "prop2": "xyz"<br>  }<br>}|
