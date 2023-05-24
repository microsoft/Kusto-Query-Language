---
title:  isinf()
description: Learn how to use the isinf() function to check if the input is an infinite value.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/03/2023
---
# isinf()

Returns whether the input is an infinite (positive or negative) value.  

## Syntax

`isinf(`*number*`)`

## Parameters

| Name | Type | Required | Description |
| -- | -- | -- | -- |
|*number*|real|&check;| The value to check if infinite.|

## Returns

`true` if x is a positive or negative infinite and `false` otherwise.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAytKzEtPVahQSCvKz1XQNVQoyVcwVCguSS1QMOTlqlFIrShJzUtRqFSwVTDQM0ASScksA4oZ6hloVehXIolnFmfmpdmCSQ2gGk0AQSRax2AAAAA=" target="_blank">Run the query</a>

```kusto
range x from -1 to 1 step 1
| extend y = 0.0
| extend div = 1.0*x/y
| extend isinf=isinf(div)
```

**Output**

|x|y|div|isinf|
|---|---|---|---|
|-1|0|-∞|true|
|0|0|NaN|false|
|1|0|∞|true|

## See also

* To check if a value is null, see [isnull()](isnullfunction.md).
* To check if a value is finite, see [isfinite()](isfinitefunction.md).
* To check if a value is NaN (Not-a-Number), see [isnan()](isnanfunction.md).
