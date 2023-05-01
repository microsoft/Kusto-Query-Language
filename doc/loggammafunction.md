---
title: loggamma() - Azure Data Explorer
description: Learn how to use the loggamma() function to compute the log of the absolute value of the gamma function.
ms.reviewer: alexans
ms.topic: reference
ms.date: 12/26/2022
---
# loggamma()

Computes log of the absolute value of the [gamma function](https://en.wikipedia.org/wiki/Gamma_function)

## Syntax

`loggamma(`*number*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
|*number*| real | &check; | The number for which to calculate the gamma.|

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUShKLS7NKbHNyU9PT8zNTdQw1QQAjpO9/xgAAAA=" target="_blank">Run the query</a>

```kusto
print result=loggamma(5)
```

**Output**

|result|
|--|
|3.1780538303479458|

## Returns

* Returns the natural logarithm of the absolute value of the gamma function of x.
* For computing gamma function, see [gamma()](gammafunction.md).
