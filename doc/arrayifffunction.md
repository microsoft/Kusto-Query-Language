---
title: array_iif() - Azure Data Explorer | Microsoft Docs
description: This article describes array_iif() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 04/28/2019
---
# array_iif()

Element-wise iif function on dynamic arrays.

Another alias: array_iff().

## Syntax

`array_iif(`*ConditionArray*, *IfTrue*, *IfFalse*]`)`

## Arguments

* *conditionArray*: Input array of *boolean* or numeric values, must be dynamic array.
* *ifTrue*: Input array of values or primitive value - the result value(s) when the corresponding value of *ConditionArray* is *true*.
* *ifFalse*: Input array of values or primitive value - the result value(s) when the corresponding value of *ConditionArray* is *false*.

**Notes**

* The result length is the length of *conditionArray*.
* Numeric condition value is treated as *condition* != *0*.
* Non-numeric/null condition value will have null in the corresponding index of the result.
* Missing values (in shorter length arrays) are treated as null.

## Returns

Dynamic array of the values taken either from the *IfTrue* or *IfFalse* [array] values, according to the corresponding value of the Condition array.

## Example

```kusto
print condition=dynamic([true,false,true]), l=dynamic([1,2,3]), r=dynamic([4,5,6]) 
| extend res=array_iif(condition, l, r)
```

|condition|l|r|res|
|---|---|---|---|
|[true, false, true]|[1, 2, 3]|[4, 5, 6]|[1, 5, 3]|
