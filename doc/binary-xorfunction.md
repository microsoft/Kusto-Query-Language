---
title:  binary_xor()
description: Learn how to use the binary_xor() function to perform the bitwise xor operation on a pair of values.
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/22/2022
---
# binary_xor()

Returns a result of the bitwise `xor` operation of the two values.

## Syntax

`binary_xor(`*value1*`,`*value2*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *value1* | int | &check; | The left-side value of the XOR operation. |
| *value2* | int | &check; | The right-side value of the XOR operation. |

## Returns

Returns logical XOR operation on a pair of numbers: value1 ^ value2.

## Examples

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswr0UjKzEssqoyvyC/SMNQx1NQEAKWP8zEWAAAA" target="_blank">Run the query</a>

```kusto
binary_xor(1,1)
```

**Output**

|Result|
|------|
|0 |

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswr0UjKzEssqoyvyC/SMNQx0tQEAPwxtTMWAAAA" target="_blank">Run the query</a>

```kusto
binary_xor(1,2)
```

**Output**

|Result|
|------|
|3 |
