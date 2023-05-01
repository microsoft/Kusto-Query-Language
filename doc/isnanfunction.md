---
title: isnan() - Azure Data Explorer
description: Learn how to use the isnan() function to check if the input is a not-a-number (NaN) value.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/03/2023
---
# isnan()

Returns whether the input is a Not-a-Number (NaN) value.  

## Syntax

`isnan(`*number*`)`

## Parameters

| Name | Type | Required | Description |
| -- | -- | -- | -- |
|*number*|scalar|&check;| The value to check if NaN.|

## Returns

`true` if x is NaN and `false` otherwise.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAytKzEtPVahQSCvKz1XQNVQoyVcwVCguSS1QMOTlqlFIrShJzUtRqFSwVdDQNdSq0FRAEk3JLAOKG+oZaFXoVyKJZxbnJebZgkkNoBpNAG+vyvhkAAAA" target="_blank">Run the query</a>

```kusto
range x from -1 to 1 step 1
| extend y = (-1*x) 
| extend div = 1.0*x/y
| extend isnan=isnan(div)
```

**Output**

|x|y|div|isnan|
|---|---|---|---|
|-1|1|-1|false|
|0|0|NaN|true|
|1|-1|-1|false|

## See also

* To check if a value is null, see [isnull()](isnullfunction.md).
* To check if a value is finite, see [isfinite()](isfinitefunction.md).
* To check if a value is infinite, see [isinf()](isinffunction.md).
