---
title: endofyear() - Azure Data Explorer | Microsoft Docs
description: This article describes endofyear() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# endofyear()

Returns the end of the year containing the date, shifted by an offset, if provided.

## Syntax

`endofyear(`*date* [`,`*offset*]`)`

## Arguments

* `date`: The input date.
* `offset`: An optional number of offset years from the input date (integer, default - 0).

## Returns

A datetime representing the end of the year for the given *date* value, with the offset, if specified.

## Example

```kusto
  range offset from -1 to 1 step 1
 | project yearEnd = endofyear(datetime(2017-01-01 10:10:17), offset) 
```

|yearEnd|
|---|
|2016-12-31 23:59:59.9999999|
|2017-12-31 23:59:59.9999999|
|2018-12-31 23:59:59.9999999|