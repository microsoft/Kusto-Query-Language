---
title: array_iif() - Azure Data Explorer
description: Learn how to use the array_iif() function to scan and evaluate elements in an array.
ms.reviewer: alexans
ms.topic: reference
ms.date: 09/21/2022
---
# array_iif()

Element-wise iif function on dynamic arrays.

Another alias: array_iff().

## Syntax

`array_iif(`*ConditionArray*, *IfTrue*, *IfFalse*`)`

## Arguments

| Name | Type | Required | Description |
|--|--|--|--|
| *conditionArray*| dynamic | &check;| Array of *boolean* or numeric values.|
| *ifTrue* |  | &check; | Array of values or primitive value. This will be the result when *ConditionArray* is *true*.|
| *ifFalse* |  | &check; | Array of values or primitive value. This will be the result when *ConditionArray* is *false*.|

### Notes

* The result length is the length of *conditionArray*.
* Numeric condition value is treated as *condition* != *0*.
* Non-numeric/null condition value will have null in the corresponding index of the result.
* Missing values (in shorter length arrays) are treated as null.

## Returns

Returns a dynamic array of the values taken either from the *IfTrue* or *IfFalse* [array] values, according to the corresponding value of the Condition array.

## Example

**\[**[**Click to run query**](https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUUjOz0vJLMnMz7NNqcxLzM1M1oguKSpN1UlLzClO1QExYzV1FHIQsoY6RjrGILEihJiJjqmOWaymAi9XjUJqRUlqXopCUWqxbWJRUWJlfGZmmgbcFqBRQJ2aACda2uZ8AAAA)**\]**

```kusto
print condition=dynamic([true,false,true]), l=dynamic([1,2,3]), r=dynamic([4,5,6]) 
| extend res=array_iif(condition, l, r)
```

**Results** 

|condition|l|r|res|
|---|---|---|---|
|[true, false, true]|[1, 2, 3]|[4, 5, 6]|[1, 5, 3]|
