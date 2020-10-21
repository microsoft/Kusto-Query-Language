---
title: pack_all() - Azure Data Explorer | Microsoft Docs
description: This article describes pack_all() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# pack_all()

Creates a `dynamic` object (property bag) from all the columns of the tabular expression.

> [!NOTE]
> The representation of the returned object isn't guaranteed to be byte-level-compatible between runs. For example, properties that appear in the bag may appear in a different order.

## Syntax

`pack_all()`

## Examples

Given a table SmsMessages 

|SourceNumber |TargetNumber| CharsCount
|---|---|---
|555-555-1234 |555-555-1212 | 46 
|555-555-1234 |555-555-1213 | 50 
|555-555-1212 |555-555-1234 | 32 

The following query:

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
datatable(SourceNumber:string,TargetNumber:string,CharsCount:long)
[
'555-555-1234','555-555-1212',46,
'555-555-1234','555-555-1213',50,
'555-555-1212','555-555-1234',32
]
| extend Packed=pack_all()
```

Returns:

|TableName |SourceNumber |TargetNumber | Packed
|---|---|---|---
|SmsMessages|555-555-1234 |555-555-1212 | {"SourceNumber":"555-555-1234", "TargetNumber":"555-555-1212", "CharsCount": 46}
|SmsMessages|555-555-1234 |555-555-1213 | {"SourceNumber":"555-555-1234", "TargetNumber":"555-555-1213", "CharsCount": 50}
|SmsMessages|555-555-1212 |555-555-1234 | {"SourceNumber":"555-555-1212", "TargetNumber":"555-555-1234", "CharsCount": 32}
