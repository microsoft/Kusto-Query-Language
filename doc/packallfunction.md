---
title: pack_all() - Azure Data Explorer
description: This article describes pack_all() in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 10/24/2021
---
# pack_all()

Creates a `dynamic` object (property bag) from all the columns of the tabular expression.

> [!NOTE]
> The representation of the returned object isn't guaranteed to be byte-level-compatible between runs. For example, properties that appear in the bag may appear in a different order.

## Syntax

`pack_all(`[*ignore_null_empty*]`)`

## Arguments

* *ignore_null_empty*: An optional `bool` indicating whether to ignore null/empty columns and exclude them from the resulting property bag. Default: `false`.

## Examples

Given a table SmsMessages 

|SourceNumber |TargetNumber| CharsCount
|---|---|---
|555-555-1234 |555-555-1212 | 46 
|555-555-1234 |555-555-1213 | 50 
|555-555-1313 | | 42 
| |555-555-3456 | 74 

The following query:

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
datatable(SourceNumber:string,TargetNumber:string,CharsCount:long)
[
'555-555-1234','555-555-1212',46,
'555-555-1234','555-555-1213',50,
'555-555-1313','',42, 
'','555-555-3456',74 
]
| extend Packed=pack_all(), PackedIgnoreNullEmpty=pack_all(true)
```

Returns:

|SourceNumber |TargetNumber | CharsCount | Packed |PackedIgnoreNullEmpty
|---|---|---|---|---
|555-555-1234 |555-555-1212 | 46 |{"SourceNumber":"555-555-1234", "TargetNumber":"555-555-1212", "CharsCount": 46} | {"SourceNumber":"555-555-1234", "TargetNumber":"555-555-1212", "CharsCount": 46}
|555-555-1234 |555-555-1213 | 50 |{"SourceNumber":"555-555-1234", "TargetNumber":"555-555-1213", "CharsCount": 50} | {"SourceNumber":"555-555-1234", "TargetNumber":"555-555-1213", "CharsCount": 50}
|555-555-1313 | | 42 | {"SourceNumber":"555-555-1313", "TargetNumber":"", "CharsCount": 42} | {"SourceNumber":"555-555-1313", "CharsCount": 42}
| |555-555-3456 | 74 | {"SourceNumber":"", "TargetNumber":"555-555-3456", "CharsCount": 74} | {"TargetNumber":"555-555-3456", "CharsCount": 74}

> [!NOTE]
> There is a difference between the *Packed* and the *PackedIgnoreNullEmpty* columns in the last two rows of the above example. These two rows included empty values that were ignored by *pack_all(true)*.   
