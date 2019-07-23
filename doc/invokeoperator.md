---
title: invoke operator - Azure Data Explorer | Microsoft Docs
description: This article describes invoke operator in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 10/23/2018
---
# invoke operator

Invokes lambda that receives the source of `invoke` as tabular parameter argument.

```kusto
T | invoke foo(param1, param2)
```

**Syntax**

`T | invoke` *function*`(`[*param1*`,` *param2*]`)`

**Arguments**

* *T*: The tabular source.
* *function*: The name of the lambda expression or function name to be evaluated.
* *param1*, *param2* ... : additional lambda arguments.

**Returns**

Returns the result of the evaluated expression.

**Notes**

See [let statements](./letstatement.md) for more details how to declare lambda expressions that can accept tabular arguments.

**Example**

The following example shows how to use `invoke` operator to call lambda expression:

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

|avg_x|
|---|
|52|
