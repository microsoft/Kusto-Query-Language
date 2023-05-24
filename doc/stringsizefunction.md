---
title:  string_size()
description: Learn how to use the string_size() function to measure the size of the input string.
ms.reviewer: alexans
ms.topic: reference
ms.date: 02/05/2023
---
# string_size()

Returns the size, in bytes, of the input string.

## Syntax

`string_size(`*source*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *source* | string | &check; | The string for which to return the byte size.|

## Returns

Returns the length, in bytes, of the input string.

## Examples

### String of letters

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUSjOrEpVsFUoLgHy0uNBPA2ljNScnHwlTQB9vNZzIQAAAA==" target="_blank">Run the query</a>

```kusto
print size = string_size("hello")
```

**Output**

|size|
|---|
|5|

### String of letters and symbols

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUSjOrEpVsFUoLgHy0uNBPA2lR5OWPZq04dGkdY8mrX80aZWSJgDJzHqdKwAAAA==" target="_blank">Run the query</a>

```kusto
print size = string_size("⒦⒰⒮⒯⒪")
```

**Output**

|size|
|---|
|15|
