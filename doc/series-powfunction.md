---
title: series_pow() - Azure Data Explorer
description: Learn how to use the series_pow() function to calculate the element-wise power of two numeric series inputs.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/30/2023
---
# series_pow()

Calculates the element-wise power of two numeric series inputs.

## Syntax

`series_pow(`*series1*`,` *series2*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *series1*, *series2* | dynamic | &check; | Arrays of numeric values. The first array, or base, is element-wise raised to the power of the second array, or power, into a dynamic array result.|

## Returns

A dynamic array of calculated element-wise power operation between the two inputs. Any non-numeric element or non-existing element, such as in the case of arrays of different sizes, yields a `null` element value.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUahQsFVIqcxLzM1M1og21FEw0lEw1lEwidXUUai0xZQw0DON1eSqUUitKEnNS1GoiC/IL4+vBJpRnFqUmVoM4mpUALVqAgA61Qq1XgAAAA==" target="_blank">Run the query</a>

```kusto
print x = dynamic([1, 2, 3, 4]), y=dynamic([1, 2, 3, 0.5])
| extend x_pow_y = series_pow(x, y) 
```

**Output**

|x|y|x_pow_y|
|---|---|---|
|[1,2,3,4]|[1,2,3,0.5]|[1.0,4.0,27.0,2.0]|