---
title:  series_log()
description: Learn how to use the series_log() function to calculate the element-wise natural logarithm function (base-e) of the numeric series input.
ms.reviewer: afridman
ms.topic: reference
ms.date: 01/30/2023
---
# series_log()

Calculates the element-wise natural logarithm function (base-e) of the numeric series input.

## Syntax

`series_log(`*series*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *series* | dynamic | &check; | An array of numeric values on which the natural logarithm function is applied.|

## Returns

Dynamic array of the calculated natural logarithm function. Any non-numeric element yields a `null` element value.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUShWsFVIqcxLzM1M1og21DHSMY7V5KpRSK0oSc1LUSiOz8lPB6ooTi3KTAVzNIo1AcJsOMY5AAAA" target="_blank">Run the query</a>

```kusto
print s = dynamic([1,2,3])
| extend s_log = series_log(s)
```

**Output**

|s|s_log|
|---|---|
|[1,2,3]|[0.0,0.69314718055994529,1.0986122886681098]|
