---
title: endofday() - Azure Data Explorer | Microsoft Docs
description: This article describes endofday() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# endofday()

Returns the end of the day containing the date, shifted by an offset, if provided.

## Syntax

`endofday(`*date* [`,`*offset*]`)`

## Arguments

* `date`: The input date.
* `offset`: An optional number of offset days from the input date (integer, default - 0).

## Returns

A datetime representing the end of the day for the given *date* value, with the offset, if specified.

## Example

```kusto
  range offset from -1 to 1 step 1
 | project dayEnd = endofday(datetime(2017-01-01 10:10:17), offset) 
```

|dayEnd|
|---|
|2016-12-31 23:59:59.9999999|
|2017-01-01 23:59:59.9999999|
|2017-01-02 23:59:59.9999999|