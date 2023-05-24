---
title:  totimespan()
description: Learn how to use the totimespan() function to convert the input to a `timespan` scalar value.
ms.reviewer: alexans
ms.topic: reference
ms.date: 02/23/2023
---
# totimespan()

Converts the input to a [timespan](./scalar-data-types/timespan.md) scalar value.

> **Deprecated aliases:** totime()

## Syntax

`totimespan(`*value*`)`

## Parameters

|Name|Type|Required|Description|
|--|--|--|--|
| *value* | string | &check; | The value to convert to a [timespan](./scalar-data-types/timespan.md).|

## Returns

If conversion is successful, result will be a [timespan](./scalar-data-types/timespan.md) value.
Else, result will be null.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUSjJL8nMTS0uSMzTUDLQMzCwMjC0MjBQ0lSwtVUAyWgY5mbmaQIAkicpMSwAAAA=" target="_blank">Run the query</a>

```kusto
totimespan("0.00:01:00") == time(1min)
```
