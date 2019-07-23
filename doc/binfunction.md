---
title: bin() - Azure Data Explorer | Microsoft Docs
description: This article describes bin() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 01/20/2019
---
# bin()

Rounds values down to an integer multiple of a given bin size. 

Used frequently in combination with [`summarize by ...`](./summarizeoperator.md).
If you have a scattered set of values, they will be grouped into a smaller set of specific values.

Null values, a null bin size, or a negative bin size will result in null. 

Alias to `floor()` function.

**Syntax**

`bin(`*value*`,`*roundTo*`)`

**Arguments**

* *value*: A number, date, or timespan. 
* *roundTo*: The "bin size". A number, date or timespan that divides *value*. 

**Returns**

The nearest multiple of *roundTo* below *value*.  
 
```kusto
(toint((value/roundTo))) * roundTo`
```

**Examples**

Expression | Result
---|---
`bin(4.5, 1)` | `4.0`
`bin(time(16d), 7d)` | `14d`
`bin(datetime(1970-05-11 13:45:07), 1d)`|  `datetime(1970-05-11)`


The following expression calculates a histogram of durations,
with a bucket size of 1 second:

```kusto
T | summarize Hits=count() by bin(Duration, 1s)
```