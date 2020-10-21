---
title: welch_test() - Azure Data Explorer
description: This article describes welch_test() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# welch_test()

Computes the p_value of the [Welch-test function](https://en.wikipedia.org/wiki/Welch%27s_t-test)

```kusto
// s1, s2 values are from https://en.wikipedia.org/wiki/Welch%27s_t-test
print
    s1 = dynamic([27.5, 21.0, 19.0, 23.6, 17.0, 17.9, 16.9, 20.1, 21.9, 22.6, 23.1, 19.6, 19.0, 21.7, 21.4]),
    s2 = dynamic([27.1, 22.0, 20.8, 23.4, 23.4, 23.5, 25.8, 22.0, 24.8, 20.2, 21.9, 22.1, 22.9, 20.5, 24.4])
| mv-expand s1 to typeof(double), s2 to typeof(double)
| summarize m1=avg(s1), v1=variance(s1), c1=count(), m2=avg(s2), v2=variance(s2), c2=count()
| extend pValue=welch_test(m1,v1,c1,m2,v2,c2)

// pValue = 0.021
```

## Syntax

`welch_test(`*mean1*`, `*variance1*`, `*count1*`, `*mean2*`, `*variance2*`, `*count2*`)`

## Arguments

* *mean1*: Expression that represents the mean (average) value of the first series
* *variance1*:  Expression that represents the variance value of the first series
* *count1*:  Expression that represents the count of values in the first series
* *mean2*: Expression that represents the mean (average) value of the second series
* *variance2*:  Expression that represents the variance value of the second series
* *count2*:  Expression that represents the count of values in the second series

## Returns

From [Wikipedia](https://en.wikipedia.org/wiki/Welch%27s_t-test):

In statistics, Welch's t-test is a two-sample location test that's used to test the hypothesis that two populations have equal means. 
Welch's t-test is an adaptation of Student's t-test, and is more reliable when the two samples have unequal variances and unequal sample sizes. These tests are often referred to as "unpaired" or "independent samples" t-tests. 
The tests are typically applied when the statistical units underlying the two samples being compared are non-overlapping. 
Welch's t-test is less popular than Student's t-test, and may be less familiar to readers. The test is also called "Welch's unequal variances t-test", or "unequal variances t-test".
