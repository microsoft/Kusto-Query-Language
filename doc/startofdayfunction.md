---
title: startofday() - Azure Data Explorer | Microsoft Docs
description: This article describes startofday() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# startofday()

Returns the start of the day containing the date, shifted by an offset, if provided.

## Syntax

`startofday(`*date* [`,`*offset*]`)`

## Arguments

* `date`: The input date.
* `offset`: An optional number of offset days from the input date (integer, default - 0). 

## Returns

A datetime representing the start of the day for the given *date* value, with the offset, if specified.

## Example

```kusto
  range offset from -1 to 1 step 1
 | project dayStart = startofday(datetime(2017-01-01 10:10:17), offset) 
```

|dayStart|
|---|
|2016-12-31 00:00:00.0000000|
|2017-01-01 00:00:00.0000000|
|2017-01-02 00:00:00.0000000|