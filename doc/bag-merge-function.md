---
title: bag_merge() - Azure Data Explorer 
description: This article describes bag_merge() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: elgevork
ms.service: data-explorer
ms.topic: reference
ms.date: 06/18/2020
---
# bag_merge()

Merges `dynamic` property-bags into a `dynamic` property-bag with all properties merged.

## Syntax

`bag_merge(`*bag1*`, `*bag2*`[`,` *bag3*, ...])`

## Arguments

* *bag1...bagN*: Input `dynamic` property-bags. The function accepts between 2 to 64 arguments.

## Returns

Returns a `dynamic` property-bag. Results from merging all of the input property-bag objects. If a key appears in more than one input object, an arbitrary value (out of the possible values for this key) will be chosen.

## Example

Expression:

<!-- csl: https://help.kusto.windows.net:443/Samples -->
```kusto
print result = bag_merge(
   dynamic({'A1':12, 'B1':2, 'C1':3}),
   dynamic({'A2':81, 'B2':82, 'A1':1}))
```

|result|
|---|
|{<br>  "A1": 12,<br>  "B1": 2,<br>  "C1": 3,<br>  "A2": 81,<br>  "B2": 82<br>}|
