---
title:  isnull()
description: Learn how to use the isnull() function to check if the argument value is null.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/03/2023
---
# isnull()

Evaluates its sole argument and returns a `bool` value indicating if the argument evaluates to a null value.

> [!NOTE]
> String values can't be null. Use [isempty](./isemptyfunction.md) to determine if a value of type `string` is empty or not.

## Syntax

`isnull(`*Expr*`)`

## Parameters

| Name | Type | Required | Description |
| -- | -- | -- | -- |
|*value*|scalar|&check;| The value to check if not null.|

## Returns

`true` if *value* is not null and `false` otherwise.

|x                |`isnull(x)`|
|-----------------|-----------|
|`""`             |`false`    |
|`"x"`            |`false`    |
|`parse_json("")`  |`true`     |
|`parse_json("[]")`|`false`    |
|`parse_json("{}")`|`false`    |

## Example

Find the storm events for which there's not a begin location.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSsp5uWqUSjPSC1KVcgszivNydFwSk3PzPNJLNFUSMxLQRXMz9MEABMUXTY6AAAA" target="_blank">Run the query</a>

```kusto
StormEvents
| where isnull(BeginLat) and isnull(BeginLon)
```
