---
title:  pow()
description: Learn how to use the pow() function to calculate the base raised to the power of the exponent.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/12/2023
---
# pow()

Returns a result of raising to power

## Syntax

`pow(`*base*`,` *exponent* `)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *base*| int, real, or long | &check; | The base value.|
| *exponent*| int, real, or long | &check; | The exponent value.|

## Returns

Returns base raised to the power exponent: base ^ exponent.

## Example 

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUShKLS7NKbEtyC/XMNJRMNYEAGG04SkWAAAA" target="_blank">Run the query</a>

```kusto
print result=pow(2, 3)
```

**Output**

|result|
|--|
|8|
