---
title: invoke operator - Azure Data Explorer
description: Learn how to use the  invoke operator to invoke a lambda expression that receives the source of `invoke` as a tabular parameter argument
ms.reviewer: alexans
ms.topic: reference
ms.date: 12/28/2022
---
# invoke operator

Invokes a lambda expression that receives the source of `invoke` as a tabular argument.

> [!NOTE]
> For more information on how to declare lambda expressions that can accept tabular arguments, see [let statements](./letstatement.md).

## Syntax

*T* `| invoke` *function*`(`[*param1*`,` *param2*]`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *T*| string | &check; | The tabular source.|
| *function*| string | &check; | The name of the lambda `let` expression or stored function name to be evaluated.|
| *param1*, *param2* ... | string || Any additional lambda arguments to pass to the function.|

## Returns

Returns the result of the evaluated expression.

## Example

The following example shows how to use the `invoke` operator to call lambda `let` expression:

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA42RzU7DMBCE736KOcZSpDQHDk2BZ+ghd7QkS2LVsSPb+RGEd8cphRLKAR/s9Vrz7WicZai06nuun2hkRw0nskBFuho0Bfbo2VVsgtKx1qpTwacgUyO0bNDRKbbJ2HhzEFmGP1ZP3sNG+KpBTYEQ7HUELnMxqdBiJD1EpDJe1XwW/DAgNIffdvGApCySuYC2ppFpPKbjt6So7fCsOcXQ3zSleBPR3spsVdNGULA+2iKXlFjgh64jp143DpJ5i5Ly8MWIc/+N2Hi8MEqs+4IpRsmY8XgmrlHPuD87/Hy/QmlskllCvB+EI9OsohdnO+RrvvluBx+4Ry6WGOdoT3zz03cp9nv5AaTMZq0DAgAA" target="_blank">Run the query</a>

```kusto
// clipped_average(): calculates percentiles limits, and then makes another 
//                    pass over the data to calculate average with values inside the percentiles
let clipped_average = (T:(x: long), lowPercentile:double, upPercentile:double)
{
   let high = toscalar(T | summarize percentiles(x, upPercentile));
   let low = toscalar(T | summarize percentiles(x, lowPercentile));
   T 
   | where x > low and x < high
   | summarize avg(x) 
};
range x from 1 to 100 step 1
| invoke clipped_average(5, 99)
```

**Output**

|avg_x|
|---|
|52|
