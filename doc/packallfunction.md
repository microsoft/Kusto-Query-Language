---
title: pack_all() - Azure Data Explorer | Microsoft Docs
description: This article describes pack_all() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 10/23/2018
---
# pack_all()

Creates a `dynamic` object (property bag) from all the columns of the tabular expression.

**Syntax**

`pack_all()`

**Examples**

Given a table SmsMessages 

|SourceNumber |TargetNumber| CharsCount
|---|---|---
|555-555-1234 |555-555-1212 | 46 
|555-555-1234 |555-555-1213 | 50 
|555-555-1212 |555-555-1234 | 32 

The following query:
```kusto
SmsMessages | extend Packed=pack_all()
``` 

Returns:

|TableName |SourceNumber |TargetNumber | Packed
|---|---|---|---
|SmsMessages|555-555-1234 |555-555-1212 | {"SourceNumber":"555-555-1234", "TargetNumber":"555-555-1212", "CharsCount": 46}
|SmsMessages|555-555-1234 |555-555-1213 | {"SourceNumber":"555-555-1234", "TargetNumber":"555-555-1213", "CharsCount": 50}
|SmsMessages|555-555-1212 |555-555-1234 | {"SourceNumber":"555-555-1212", "TargetNumber":"555-555-1234", "CharsCount": 32}