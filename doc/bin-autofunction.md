---
title: bin_auto() - Azure Data Explorer | Microsoft Docs
description: This article describes bin_auto() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# bin_auto()

Rounds values down to a fixed-size "bin", with control over the bin size and starting point provided by a query property.

## Syntax

`bin_auto` `(` *Expression* `)`

## Arguments

* *Expression*: A scalar expression of a numeric type indicating the value to round.

**Client Request Properties**

* `query_bin_auto_size`: A numeric literal indicating the size of each bin.
* `query_bin_auto_at`: A numeric literal indicating one value of *Expression* which is a "fixed point" (that is, a value `fixed_point`
  for which `bin_auto(fixed_point)` == `fixed_point`.)

## Returns

The nearest multiple of `query_bin_auto_at` below *Expression*, shifted so that `query_bin_auto_at`
will be translated into itself.

## Examples

```kusto
set query_bin_auto_size=1h;
set query_bin_auto_at=datetime(2017-01-01 00:05);
range Timestamp from datetime(2017-01-01 00:05) to datetime(2017-01-01 02:00) step 1m
| summarize count() by bin_auto(Timestamp)
```

|Timestamp                    | count_|
|-----------------------------|-------|
|2017-01-01 00:05:00.0000000  | 60    |
|2017-01-01 01:05:00.0000000  | 56    |