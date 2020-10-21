---
title: format_bytes() - Azure Data Explorer
description: This article describes format_bytes() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# format_bytes()

Formats a number as a string representing data size in bytes.

```kusto
format_bytes(1024) == '1 KB'"
```

## Syntax

`format_bytes(`*value* [`,` *precision* [`,` *units*]]`)`

## Arguments

* `value`: a number to be formatted as data size in bytes.
* `precision`: (optional) Number of digits the value will be rounded to. (default value is 0).
* `units`: (optional) Units of the target data size the string formatting will use (`Bytes`, `KB`, `MB`, `GB`, `TB`, `PB`). If parameter is empty - the units will be auto-selected based on input value.

## Returns

The string with the format result.

## Examples

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
print 
v1 = format_bytes(564),
v2 = format_bytes(10332, 1),
v3 = format_bytes(20010332),
v4 = format_bytes(20010332, 2),
v5 = format_bytes(20010332, 0, "KB")
```

|v1|v2|v3|v4|v5|
|---|---|---|---|---|
|564 Bytes|10.1 KB|19 MB|19.08 MB|19541 KB|
