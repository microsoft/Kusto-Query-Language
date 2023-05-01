---
title: Aggregation Functions - Azure Data Explorer 
description: Learn how to use aggregation functions to perform calculations on a set of values and return a single value.
ms.reviewer: alexans
ms.topic: reference
ms.date: 09/20/2022
---

# Aggregation function types at a glance

An aggregation function performs a calculation on a set of values, and returns a single value. These functions are used in conjunction with the [summarize operator](summarizeoperator.md). This article lists all available aggregation functions grouped by type. For scalar functions, see [Scalar function types](scalarfunctions.md).

## Binary functions

| Function Name | Description |
|--|--|
| [binary_all_and()](binary-all-and-aggfunction.md) | Returns aggregated value using the binary AND of the group. |
| [binary_all_or()](binary-all-or-aggfunction.md) | Returns aggregated value using the binary OR of the group. |
| [binary_all_xor()](binary-all-xor-aggfunction.md) | Returns aggregated value using the binary XOR of the group. |

## Dynamic functions

| Function Name | Description |
|--|--|
| [buildschema()](buildschema-aggfunction.md) | Returns the minimal schema that admits all values of the dynamic input. |
| [make_bag()](make-bag-aggfunction.md), [make_bag_if()](make-bag-if-aggfunction.md) | Returns a property bag of dynamic values within the group without/with a predicate. |
| [make_list()](makelist-aggfunction.md), [make_list_if()](makelistif-aggfunction.md) | Returns a list of all the values within the group without/with a predicate. |
| [make_list_with_nulls()](make-list-with-nulls-aggfunction.md) | Returns a list of all the values within the group, including null values. |
| [make_set()](makeset-aggfunction.md), [make_set_if()](makesetif-aggfunction.md) | Returns a set of distinct values within the group without/with a predicate. |

## Row selector functions

| Function Name | Description |
|--|--|
| [arg_max()](arg-max-aggfunction.md) | Returns one or more expressions when the argument is maximized. |
| [arg_min()](arg-min-aggfunction.md) | Returns one or more expressions when the argument is minimized. |
| [take_any()](take-any-aggfunction.md), [take_anyif()](take-anyif-aggfunction.md) | Returns a random non-empty value for the group without/with a predicate. |

## Statistical functions

| Function Name | Description |
|--|--|
| [avg()](avg-aggfunction.md) | Returns an average value across the group. |
| [avgif()](avgif-aggfunction.md) | Returns an average value across the group (with predicate). |
| [count()](count-aggfunction.md), [countif()](countif-aggfunction.md) | Returns a count of the group without/with a predicate. |
| [count_distinct()](count-distinct-aggfunction.md), [count_distinctif()](count-distinctif-aggfunction.md) | Returns a count of unique elements in the group without/with a predicate. |
| [dcount()](dcount-aggfunction.md), [dcountif()](dcountif-aggfunction.md) | Returns an approximate distinct count of the group elements without/with a predicate. |
| [hll()](hll-aggfunction.md) | Returns the HyperLogLog (HLL) results of the group elements, an intermediate value of the `dcount` approximation. |
| [hll_if()](hll-if-aggregation-function.md) | Returns the HyperLogLog (HLL) results of the group elements, an intermediate value of the `dcount` approximation (with predicate). |
| [hll_merge()](hll-merge-aggfunction.md) | Returns a value for merged HLL results. |
| [max()](max-aggfunction.md), [maxif()](maxif-aggfunction.md) | Returns the maximum value across the group without/with a predicate. |
| [min()](min-aggfunction.md), [minif()](minif-aggfunction.md) | Returns the minimum value across the group without/with a predicate. |
| [percentile()](percentiles-aggfunction.md) | Returns a percentile estimation of the group. |
| [percentiles()](percentiles-aggfunction.md) | Returns percentile estimations of the group. |
| [percentiles_array()](percentiles-aggfunction.md) | Returns the percentile approximates of the array. |
| [percentilesw()](percentiles-aggfunction.md) | Returns the weighted percentile approximate of the group. |
| [percentilesw_array()](percentiles-aggfunction.md) | Returns the weighted percentile approximate of the array. |
| [stdev()](stdev-aggfunction.md), [stdevif()](stdevif-aggfunction.md) | Returns the standard deviation across the group for a population that is considered a sample without/with a predicate. |
| [stdevp()](stdevp-aggfunction.md) | Returns the standard deviation across the group for a population that is considered representative. |
| [sum()](sum-aggfunction.md), [sumif()](sumif-aggfunction.md) | Returns the sum of the elements within the group without/with a predicate. |
| [tdigest()](tdigest-aggfunction.md) | Returns an intermediate result for the percentiles approximation, the weighted percentile approximate of the group. |
| [tdigest_merge()](tdigest-merge-aggfunction.md) | Returns the merged `tdigest` value across the group. |
| [variance()](variance-aggfunction.md), [varianceif()](varianceif-aggfunction.md) | Returns the variance across the group without/with a predicate. |
| [variancep()](variancep-aggfunction.md) | Returns the variance across the group for a population that is considered representative. |
