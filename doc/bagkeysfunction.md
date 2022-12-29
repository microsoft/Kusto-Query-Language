---
title: bag_keys() - Azure Data Explorer
description: Learn how to use the bag_keys() function to enumerate the root keys in a dynamic property bag object.
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/23/2022
---
# bag_keys()

Enumerates all the root keys in a dynamic property bag object.

## Syntax

`bag_keys(`*object*`)`

## Parameters

| Name | Type | Required | Description |
| -- | -- | -- | -- |
| *object* | dynamic | &check; | The property bag object for which to enumerate keys. |

## Returns

An array of keys, order is undetermined.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA3XMSwrDIBCA4b2nmN0ouEnSBwg9iUjRjIRQaxdNICHt3WvzqN1kZjP8fAzZLq0LnreR/KDCIzYSSNEY7b2tBWgGaYrUlsIntKjQoQSsURVl9RYSZlTuoQlpkYnOstqT+kfNZg/Zxj6EtR7/PmzylFt6usZzjtoIZtgL/ND5SHDz4xMu4Gxz/Z6cxAfYZXgQDQEAAA==" target="_blank">Run the query</a>

```kusto
datatable(index:long, d:dynamic) [
    1, dynamic({'a':'b', 'c':123}), 
    2, dynamic({'a':'b', 'c':{'d':123}}),
    3, dynamic({'a':'b', 'c':[{'d':123}]}),
    4, dynamic(null),
    5, dynamic({}),
    6, dynamic('a'),
    7, dynamic([])
]
| extend keys = bag_keys(d)
```

**Output**

|index|d|keys|
|---|---|---|
|1|{<br>  "a": "b",<br>  "c": 123<br>}|[<br>  "a",<br>  "c"<br>]|
|2|{<br>  "a": "b",<br>  "c": {<br>    "d": 123<br>  }<br>}|[<br>  "a",<br>  "c"<br>]|
|3|{<br>  "a": "b",<br>  "c": [<br>    {<br>      "d": 123<br>    }<br>  ]<br>}|[<br>  "a",<br>  "c"<br>]|
|4|||
|5|{}|[]|
|6|a||
|7|[]||
