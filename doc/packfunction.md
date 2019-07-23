---
title: pack() - Azure Data Explorer | Microsoft Docs
description: This article describes pack() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 01/10/2019
---
# pack()

Creates a `dynamic` object (property bag) from a list of names and values.

Alias to `pack_dictionary()` function.

**Syntax**

`pack(`*key1*`,` *value1*`,` *key2*`,` *value2*`,... )`

**Arguments**

* An alternating list of keys and values (the total length of the list must be even)
* All keys must be non-empty constant strings

**Examples**

The following example returns `{"Level":"Information","ProcessID":1234,"Data":{"url":"www.bing.com"}}`:

```kusto
pack("Level", "Information", "ProcessID", 1234, "Data", pack("url", "www.bing.com"))
```

Lets take 2 tables, SmsMessages and MmsMessages:

Table SmsMessages 

|SourceNumber |TargetNumber| CharsCount
|---|---|---
|555-555-1234 |555-555-1212 | 46 
|555-555-1234 |555-555-1213 | 50 
|555-555-1212 |555-555-1234 | 32 

Table MmsMessages 

|SourceNumber |TargetNumber| AttachmnetSize | AttachmnetType | AttachmnetName
|---|---|---|---|---
|555-555-1212 |555-555-1213 | 200 | jpeg | Pic1
|555-555-1234 |555-555-1212 | 250 | jpeg | Pic2
|555-555-1234 |555-555-1213 | 300 | png | Pic3

The following query:
```kusto
SmsMessages 
| extend Packed=pack("CharsCount", CharsCount) 
| union withsource=TableName kind=inner 
( MmsMessages 
  | extend Packed=pack("AttachmnetSize", AttachmnetSize, "AttachmnetType", AttachmnetType, "AttachmnetName", AttachmnetName))
| where SourceNumber == "555-555-1234"
``` 

Returns:

|TableName |SourceNumber |TargetNumber | Packed
|---|---|---|---
|SmsMessages|555-555-1234 |555-555-1212 | {"CharsCount": 46}
|SmsMessages|555-555-1234 |555-555-1213 | {"CharsCount": 50}
|MmsMessages|555-555-1234 |555-555-1212 | {"AttachmnetSize": 250, "AttachmnetType": "jpeg", "AttachmnetName": "Pic2"}
|MmsMessages|555-555-1234 |555-555-1213 | {"AttachmnetSize": 300, "AttachmnetType": "png", "AttachmnetName": "Pic3"}