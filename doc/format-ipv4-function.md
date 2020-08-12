---
title: format_ipv4() - Azure Data Explorer
description: This article describes format_ipv4() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 08/09/2020
---
# format_ipv4()

Parses input with a netmask and returns string representing IPv4 address.

```kusto
print format_ipv4('192.168.1.255', 24) == '192.168.1.0'
print format_ipv4(3232236031, 24) == '192.168.1.0'
```

## Syntax

`format_ipv4(`*Expr* [`,` *PrefixMask*`])`

## Arguments

* *`Expr`*: A string or number representation of the IPv4 address.
* *`PrefixMask`*: (Optional) An integer from 0 to 32 representing the number of most-significant bits that are taken into account. If argument isn't specified, all bit-masks are used (32).

## Returns

If conversion is successful, the result will be a string representing IPv4 address.
If conversion isn't successful, the result will be an empty string.

## See also

- [format_ipv4_mask()](format-ipv4-mask-function.md): For IPv4 address formatting including CIDR notation.
- [IPv4 and IPv6 functions](scalarfunctions.md#ipv4ipv6-functions)

## Examples

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
datatable(address:string, mask:long)
[
 '192.168.1.1', 24,          
 '192.168.1.1', 32,          
 '192.168.1.1/24', 32,       
 '192.168.1.1/24', long(-1), 
]
| extend result = format_ipv4(address, mask), 
         result_mask = format_ipv4_mask(address, mask)
```

|address|mask|result|result_mask|
|---|---|---|---|
|192.168.1.1|24|192.168.1.0|192.168.1.0/24|
|192.168.1.1|32|192.168.1.1|192.168.1.1/32|
|192.168.1.1/24|32|192.168.1.0|192.168.1.0/24|
|192.168.1.1/24|-1|||
