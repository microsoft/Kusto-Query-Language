---
title: reverse() - Azure Data Explorer
description: This article describes reverse() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# reverse()

Function reverses the order of the input string.
If the input value isn't of type `string`, then the function forcibly casts the value to type `string`.

## Syntax

`reverse(`*source*`)`

## Arguments

* *source*: input value.  

## Returns

The reverse order of a string value.

## Examples

```kusto
print str = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
| extend rstr = reverse(str)
```

|str|rstr|
|---|---|
|ABCDEFGHIJKLMNOPQRSTUVWXYZ|ZYXWVUTSRQPONMLKJIHGFEDCBA|


```kusto
print ['int'] = 12345, ['double'] = 123.45, 
['datetime'] = datetime(2017-10-15 12:00), ['timespan'] = 3h
| project rint = reverse(['int']), rdouble = reverse(['double']), 
rdatetime = reverse(['datetime']), rtimespan = reverse(['timespan'])
```

|rint|rdouble|rdatetime|rtimespan|
|---|---|---|---|
|54321|54.321|Z0000000.00:00:21T51-01-7102|00:00:30|
