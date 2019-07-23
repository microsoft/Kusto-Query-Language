---
title: series_iir() - Azure Data Explorer | Microsoft Docs
description: This article describes series_iir() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 02/20/2019
---
# series_iir()

Applies a Infinite Impulse Response filter on a series.  

Takes an expression containing dynamic numerical array as input and applies an [Infinite Impulse Response](https://en.wikipedia.org/wiki/Infinite_impulse_response) filter. By specifying the filter coefficients, it can be used, for example, to calculate the cumulative sum of the series, to apply smoothing operations, as well as various [high-pass](https://en.wikipedia.org/wiki/High-pass_filter), [band-pass](https://en.wikipedia.org/wiki/Band-pass_filter) and [low-pass](https://en.wikipedia.org/wiki/Low-pass_filter) filters. The function takes as input the column containing the dynamic array and two static dynamic arrays of the filter's *a* and *b* coefficients, and applies the filter on the column. It outputs a new dynamic array column, containing the filtered output.  
 

**Syntax**

`series_iir(`*x*`,` *b* `,` *a*`)`

**Arguments**

* *x*: Dynamic array cell which is an array of numeric values, typically the resulting output of [make-series](make-seriesoperator.md) or [make_list](makelist-aggfunction.md) operators.
* *b*: A constant expression containing the numerator coefficients of the filter (stored as a dynamic array of numeric values).
* *a*: A constant expression, like *b*. Containing the denominator coefficients of the filter.

> [!IMPORTANT]
> The first element of `a` (i.e. `a[0]`) mustn't be zero (to avoid division by 0; see the formula below).

**More about the filter's recursive formula**

* Given an input array X and coefficients arrays a, b of lengths n_a and n_b respectively, the transfer function of the filter, generating the output array Y, is defined by (see also in Wikipedia):

<div align="center">
Y<sub>i</sub> = a<sub>0</sub><sup>-1</sup>(b<sub>0</sub>X<sub>i</sub>
 + b<sub>1</sub>X<sub>i-1</sub> + ... + b<sub>n<sub>b</sub>-1</sub>X<sub>i-n<sub>b</sub>-1</sub>
 - a<sub>1</sub>Y<sub>i-1</sub>-a<sub>2</sub>Y<sub>i-2</sub> - ... - a<sub>n<sub>a</sub>-1</sub>Y<sub>i-n<sub>a</sub>-1</sub>)
</div>

**Example**

Calculating cumulative sum can be performed by iir filter with coefficients *a*=[1,-1] and *b*=[1]:  

```kusto
let x = range(1.0, 10, 1);
print x=x, y = series_iir(x, dynamic([1]), dynamic([1,-1]))
| mv-expand x, y
```

| x | y |
|:--|:--|
|1.0|1.0|
|2.0|3.0|
|3.0|6.0|
|4.0|10.0|

Here's how to wrap it in a function:

```kusto
let vector_sum=(x:dynamic)
{
  let y=array_length(x) - 1;
  toreal(series_iir(x, dynamic([1]), dynamic([1, -1]))[y])
};
print d=dynamic([0, 1, 2, 3, 4])
| extend dd=vector_sum(d)
```

|d            |dd  |
|-------------|----|
|`[0,1,2,3,4]`|`10`|