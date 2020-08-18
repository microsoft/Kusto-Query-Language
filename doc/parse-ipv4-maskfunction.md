---
title: parse_ipv4_mask() - Azure Data Explorer
description: This article describes parse_ipv4_mask() function in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 05/27/2020
---
# parse_ipv4_mask()

Converts the input string of IPv4 and netmask to long number representation (signed 64-bit).

```kusto
parse_ipv4_mask("127.0.0.1", 24) == 2130706432
parse_ipv4_mask('192.1.168.2', 31) == parse_ipv4_mask('192.1.168.3', 31)
```

## Syntax

`parse_ipv4_mask(`*`Expr`*`, `*`PrefixMask`*`)`

## Arguments

* *`Expr`*: A string representation of the IPv4 address that will be converted to long. 
* *`PrefixMask`*: An integer from 0 to 32 representing the number of most-significant bits that are taken into account.

## Returns

If conversion is successful, the result will be a long number.
If conversion isn't successful, the result will be `null`.
