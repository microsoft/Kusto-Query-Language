---
title: ipv4_is_match() - Azure Data Explorer
description: This article describes ipv4_is_match() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/24/2020
---
# ipv4_is_match()

Matches two IPv4 strings. The two IPv4 strings are parsed and compared while accounting for the combined IP-prefix mask calculated from argument prefixes, and the optional `PrefixMask` argument.

```kusto
ipv4_is_match("127.0.0.1", "127.0.0.1") == true
ipv4_is_match('192.168.1.1', '192.168.1.255') == false
ipv4_is_match('192.168.1.1/24', '192.168.1.255/24') == true
ipv4_is_match('192.168.1.1', '192.168.1.255', 24) == true
```

## Syntax

`ipv4_is_match(`*Expr1*`, `*Expr2*`[ ,`*PrefixMask*`])`

## Arguments

* *Expr1*, *Expr2*: A string expression representing an IPv4 address. IPv4 strings can be masked using [IP-prefix notation](#ip-prefix-notation).
* *PrefixMask*: An integer from 0 to 32 representing the number of most-significant bits that are taken into account.

## IP-prefix notation

IP addresses can be defined with `IP-prefix notation` using a slash (`/`) character. The IP address to the LEFT of the slash (`/`) is the base IP address. The number (1 to 32) to the RIGHT of the slash (`/`) is the number of contiguous 1 bit in the netmask. 

For example, 192.168.2.0/24 will have an associated net/subnetmask containing 24 contiguous bits or 255.255.255.0 in dotted decimal format.

## Returns

* `true`: If the long representation of the first IPv4 string argument is equal to the second IPv4 string argument.
*  `false`: Otherwise.
* `null`: If conversion for one of the two IPv4 strings wasn't successful.

## Examples

### IPv4 comparison equality - IP-prefix notation specified inside the IPv4 strings

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
datatable(ip1_string:string, ip2_string:string)
[
 '192.168.1.0',    '192.168.1.0',       // Equal IPs
 '192.168.1.1/24', '192.168.1.255',     // 24 bit IP-prefix is used for comparison
 '192.168.1.1',    '192.168.1.255/24',  // 24 bit IP-prefix is used for comparison
 '192.168.1.1/30', '192.168.1.255/24',  // 24 bit IP-prefix is used for comparison
]
| extend result = ipv4_is_match(ip1_string, ip2_string)
```

|ip1_string|ip2_string|result|
|---|---|---|
|192.168.1.0|192.168.1.0|1|
|192.168.1.1/24|192.168.1.255|1|
|192.168.1.1|192.168.1.255/24|1|
|192.168.1.1/30|192.168.1.255/24|1|

### IPv4 comparison equality - IP-prefix notation specified inside the IPv4 strings and an additional argument of the `ipv4_is_match()` function

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
datatable(ip1_string:string, ip2_string:string, prefix:long)
[
 '192.168.1.1',    '192.168.1.0',   31, // 31 bit IP-prefix is used for comparison
 '192.168.1.1/24', '192.168.1.255', 31, // 24 bit IP-prefix is used for comparison
 '192.168.1.1',    '192.168.1.255', 24, // 24 bit IP-prefix is used for comparison
]
| extend result = ipv4_is_match(ip1_string, ip2_string, prefix)
```

|ip1_string|ip2_string|prefix|result|
|---|---|---|---|
|192.168.1.1|192.168.1.0|31|1|
|192.168.1.1/24|192.168.1.255|31|1|
|192.168.1.1|192.168.1.255|24|1|
