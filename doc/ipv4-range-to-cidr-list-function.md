---
title: ipv4_range_to_cidr_list() - Azure Data Explorer
description: Learn how to use the ipv4_range_to_cidr_list() function to convert IPv4 address range to a list of CIDR ranges.
ms.reviewer: alexans
ms.topic: reference
ms.date: 05/10/2023
---
# ipv4_range_to_cidr_list()

Converts a IPv4 address range denoted by starting and ending IPv4 addresses to a list of IPv4 ranges in CIDR notation.

## Syntax

`ipv4_range_to_cidr_list(`*StartAddress* `,` *EndAddress* `)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *StartAddress*| string | &check; | An expression representing a starting IPv4 address of the range.|
| *EndAddress*| string | &check; | An expression representing an ending IPv4 address of the range.|

## Returns

A dynamic array object containing the list of ranges in CIDR notation.

[!INCLUDE [CIDR notation](../../includes/ip-prefix-notation.md)]

## Examples


```kusto
print start_IP="1.1.128.0", end_IP="1.1.140.255"
 | project ipv4_range_list = ipv4_range_to_cidr_list(start_IP, end_IP)
```

**Output**

|ipv4_range_list|
|--|
|`["1.1.128.0/21", "1.1.136.0/22","1.1.140.0/24"]`|
