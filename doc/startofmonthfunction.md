---
title: startofmonth() - Azure Data Explorer | Microsoft Docs
description: This article describes startofmonth() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# startofmonth()

Returns the start of the month containing the date, shifted by an offset, if provided.

## Syntax

`startofmonth(`*date* [`,`*offset*]`)`

## Arguments

* `date`: The input date.
* `offset`: An optional number of offset months from the input date (integer, default - 0).

## Returns

A datetime representing the start of the month for the given *date* value, with the offset, if specified.

## Example

```kusto
  range offset from -1 to 1 step 1
 | project monthStart = startofmonth(datetime(2017-01-01 10:10:17), offset) 
```

|monthStart|
|---|
|2016-12-01 00:00:00.0000000|
|2017-01-01 00:00:00.0000000|
|2017-02-01 00:00:00.0000000|