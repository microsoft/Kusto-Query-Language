---
title: series_periods_validate() - Azure Data Explorer | Microsoft Docs
description: This article describes series_periods_validate() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 02/19/2019
---
# series_periods_validate()

Checks whether a time series contains periodic patterns of given lengths.  

Very often a metric measuring the traffic of an application is characterized by a weekly and/or daily periods. This can be confirmed by running `series_periods_validate()` checking for a weekly and daily periods.

The function takes as input a column containing a dynamic array of time series (typically the resulting output of [make-series](make-seriesoperator.md) operator), and one or more `real` numbers that define the lengths of the periods to validate. 

The function outputs 2 columns:
* *periods*: a dynamic array containing the periods to validate (supplied in the input)
* *scores*: a dynamic array containing a score between 0 and 1 that measures the significance of a period in its respective position in the *periods* array

**Syntax**

`series_periods_validate(`*x*`,` *period1* [ `,` *period2* `,` . . . ] `)`

**Arguments**

* *x*: Dynamic array scalar expression which is an array of numeric values, typically the resulting output of [make-series](make-seriesoperator.md) or [make_list](makelist-aggfunction.md) operators.
* *period1*, *period2*, etc.: `real` numbers specifying the periods to validate, in units of the bin size. For example, if the series is in 1h bins, a weekly period is 168 bins.

> [!IMPORTANT]
> * The minimal value for each of the *period* arguments is **4** and the maximal is half of the length of the input series; for a *period* argument outside these bounds, the output score will be **0**.
>
> * The input time series must be regular, i.e. aggregated in constant bins (which is always the case if it has been created using [make-series](make-seriesoperator.md)). Otherwise, the output is meaningless.
> 
> * The function accepts up to 16 periods to validate.


**Example**

The following query embeds a snapshot of a month of an application’s traffic, aggregated twice a day (i.e. the bin size is 12 hours).

```kusto
print y=dynamic([80,139,87,110,68,54,50,51,53,133,86,141,97,156,94,149,95,140,77,61,50,54,47,133,72,152,94,148,105,162,101,160,87,63,53,55,54,151,103,189,108,183,113,175,113,178,90,71,62,62,65,165,109,181,115,182,121,178,114,170])
| project x=range(1, array_length(y), 1), y  
| render linechart 
```

![alt text](./Images/samples/series-periods.png "series-periods")

Running `series_periods_validate()` on this series to validate a weekly period (14 points long) results in a high score,  and with a **0** score  when validating a five days period (10 points long).

```kusto
print y=dynamic([80,139,87,110,68,54,50,51,53,133,86,141,97,156,94,149,95,140,77,61,50,54,47,133,72,152,94,148,105,162,101,160,87,63,53,55,54,151,103,189,108,183,113,175,113,178,90,71,62,62,65,165,109,181,115,182,121,178,114,170])
| project x=range(1, array_length(y), 1), y  
| project series_periods_validate(y, 14.0, 10.0)
```

| series\_periods\_validate\_y\_periods  | series\_periods\_validate\_y\_scores |
|-------------|-------------------|
| [14.0, 10.0] | [0.84,0.0]  |