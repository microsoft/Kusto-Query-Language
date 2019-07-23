---
title: series_greater_equals() - Azure Data Explorer | Microsoft Docs
description: This article describes series_greater_equals() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 01/15/2019
---
# series_greater_equals()

Calculates the element-wise greater or equals (`>=`) logic operation of two numeric series inputs.

**Syntax**

`series_greater_equals (`*Series1*`,` *Series2*`)`

**Arguments**

* *Series1, Series2*: Input numeric arrays to be element-wise compared. All arguments must be dynamic arrays. 

**Returns**

Dynamic array of booleans containing the calculated element-wise greater or equal logic operation between the two inputs. Any non-numeric element or non-existing element (arrays of different sizes) yields a `null` element value.

**Example**

```kusto
print s1 = dynamic([1,2,4]), s2 = dynamic([4,2,1])
| extend s1_greater_equals_s2 = series_greater_equals(s1, s2)
```

|s1|s2|s1_greater_equals_s2|
|---|---|---|
|[1,2,4]|[4,2,1]|[false,true,true]|