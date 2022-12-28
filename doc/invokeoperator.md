---
title: invoke operator - Azure Data Explorer
description: Learn how to use the  invoke operator to invoke a lambda expression that receives the source of `invoke` as a tabular parameter argument
ms.reviewer: alexans
ms.topic: reference
ms.date: 12/20/2022
---
# invoke operator

Invokes a lambda expression that receives the source of `invoke` as a tabular parameter argument.

```kusto
T | invoke foo(param1, param2)
```

> [!NOTE]
> For more information on how to declare lambda expressions that can accept tabular arguments, see [let statements](./letstatement.md).

## Syntax

`T | invoke` *function*`(`[*param1*`,` *param2*]`)`

## Arguments

* *T*: The tabular source.
* *function*: The name of the lambda `let` expression or stored function name to be evaluated.
* *param1*, *param2* ... : additional lambda arguments.

## Returns

Returns the result of the evaluated expression.

## Example

The following example shows how to use `invoke` operator to call lambda `let` expression:

<!-- csl: https://help.kusto.windows.net/Samples -->
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
