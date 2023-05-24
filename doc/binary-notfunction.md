---
title:  binary_not()
description: Learn how to use the binary_not() function to return a bitwise negation of the input value.
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/21/2022
---
# binary_not()

Returns a bitwise negation of the input value.

## Syntax

`binary_not(`*value*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *value* | long | &check; | The value to negate. |

## Returns

Returns logical NOT operation on a number: value.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUUjKzEssqozPyy/RMDQw0AQAChXSgRUAAAA=" target="_blank">Run the query</a>

```kusto
binary_not(100)
```

**Output**

|result|
|------|
|-101|
