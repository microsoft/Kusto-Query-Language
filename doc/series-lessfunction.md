---
title: series_less() - Azure Data Explorer | Microsoft Docs
description: This article describes series_less() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 01/15/2019
---
# series_less()

Calculates the element-wise less (`<`) logic operation of two numeric series inputs.

**Syntax**

`series_less (`*Series1*`,` *Series2*`)`

**Arguments**

* *Series1, Series2*: Input numeric arrays to be element-wise compared. All arguments must be dynamic arrays. 

**Returns**

Dynamic array of booleans containing the calculated element-wise less logic operation between the two inputs. Any non-numeric element or non-existing element (arrays of different sizes) yields a `null` element value.

**Example**

```kusto
print s1 = dynamic([1,2,4]), s2 = dynamic([4,2,1])
| extend s1_less_s2 = series_less(s1, s2)
```

|s1|s2|s1_less_s2|
|---|---|---|
|[1,2,4]|[4,2,1]|[true,false,false]|