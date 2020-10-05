---
title: bag_remove_keys() - Azure Data Explorer 
description: This article describes bag_remove_keys() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 10/04/2020
---
# bag_remove_keys()

Removes keys and associated values from a `dynamic` property-bag.

## Syntax

`bag_remove_keys(`*bag*`, `*keys*`)`

## Arguments

* *bag*: `dynamic` property-bag input.
* *keys*: `dynamic` array includes keys to be removed from the input. Keys refer to the first level of the property bag.
Specifying keys on the nested levels isn't supported.

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
