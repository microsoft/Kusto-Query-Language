---
title:  asin()
description: Learn how to use the asin() function to calculate the angle from a sine input.
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/03/2022
---
# asin()

Calculates the angle whose sine is the specified number, or the arc sine. This is the inverse operation of [`sin()`](sinfunction.md).

## Syntax

`asin(`*x*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
|*x* | real | &check;| A real number in range [-1, 1] used to calculate the arc sine.|

## Returns

Returns the value of the arc sine of `x`. Returns `null` if `x` < -1 or `x` > 1.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUShKLS7NKbFNLM7M0zDQszTVBAC0CzxqFwAAAA==" target="_blank">Run the query</a>

```kusto
asin(0.5)
```

**Output**

|result|
|---|
|1.2532358975033751|
