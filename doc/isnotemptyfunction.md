---
title:  isnotempty()
description: Learn how to use the isnotempty() function to check if the argument isn't an empty string.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/03/2023
---
# isnotempty()

Returns `true` if the argument isn't an empty string, and it isn't null.

> **Deprecated aliases:** notempty()

## Syntax

`isnotempty(`*value*`)`

## Parameters

| Name | Type | Required | Description |
| -- | -- | -- | -- |
|*value*|scalar|&check;| The value to check if not empty or null.|

## Returns

`true` if *value* is not null and `false` otherwise.

## Example

Find the storm events for which there's a begin location.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSsp5uWqUSjPSC1KVcgszssvSc0tKKnUcEpNz8zzSSzRVEjMS8GUyM/TBAAbLqnSQgAAAA==" target="_blank">Run the query</a>

```kusto
StormEvents
| where isnotempty(BeginLat) and isnotempty(BeginLon)
```
