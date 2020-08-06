---
title: ipv6_is_match() - Azure Data Explorer
description: This article describes ipv6_is_match() function in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 05/27/2020
---
# ipv6_is_match()

Matches two IPv6 or IPv4 network address strings. The two IPv6/IPv4 strings are parsed and compared while accounting for the combined IP-prefix mask calculated from argument prefixes, and the optional `PrefixMask` argument.

```kusto
ipv6_is_match('::ffff:7f00:1', '127.0.0.1') == true
ipv6_is_match('fe80::85d:e82c:9446:7994', 'fe80::85d:e82c:9446:7995') == false
ipv6_is_match('192.168.1.1/24', '192.168.1.255/24') == true
ipv6_is_match('fe80::85d:e82c:9446:7994/127', 'fe80::85d:e82c:9446:7995/127') == true
ipv6_is_match('fe80::85d:e82c:9446:7994', 'fe80::85d:e82c:9446:7995', 127) == true
```

> [!NOTE]
> The function can accept and compare arguments representing both IPv6 and IPv4 network addresses. If the caller knows that arguments are in IPv4 format, use the [ipv4_is_match()](./ipv4-is-matchfunction.md) function. This function will result in better runtime performance.

## Syntax

`ipv6_is_match(`*Expr1*`, `*Expr2*`[ ,`*PrefixMask*`])`

## Arguments

* *Expr1*, *Expr2*: A string expression representing an IPv6 or IPv4 address. IPv6 and IPv4 strings can be masked using [IP-prefix notation](#ip-prefix-notation).
* *PrefixMask*: An integer from 0 to 128 representing the number of most-significant bits that are taken into account.

## IP-prefix notation
 
IP addresses can be defined with `IP-prefix notation` using a slash (`/`) character.
The IP address to the LEFT of the slash (`/`) is the base IP address. The number (1 to 127) to the RIGHT of the slash (`/`) is the number of contiguous 1 bit in the netmask. 

For example, fe80::85d:e82c:9446:7994/120 will have an associated net/subnetmask containing 120 contiguous bits.

## Returns

* `true`: If the long representation of the first IPv6/IPv4 string argument is equal to the second IPv6/IPv4 string argument.
* `false`: Otherwise.
* `null`: If conversion for one of the two IPv6/IPv4 strings wasn't successful.

## Examples

### IPv6/IPv4 comparison equality case - IP-prefix notation specified inside the IPv6/IPv4 strings

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
datatable(ip1_string:string, ip2_string:string)
[
 // IPv4 are compared as IPv6 addresses
 '192.168.1.1',    '192.168.1.1',       // Equal IPs
 '192.168.1.1/24', '192.168.1.255',     // 24 bit IP4-prefix is used for comparison
 '192.168.1.1',    '192.168.1.255/24',  // 24 bit IP4-prefix is used for comparison
 '192.168.1.1/30', '192.168.1.255/24',  // 24 bit IP4-prefix is used for comparison
  // IPv6 cases
 'fe80::85d:e82c:9446:7994', 'fe80::85d:e82c:9446:7994',         // Equal IPs
 'fe80::85d:e82c:9446:7994/120', 'fe80::85d:e82c:9446:7998',     // 120 bit IP6-prefix is used for comparison
 'fe80::85d:e82c:9446:7994', 'fe80::85d:e82c:9446:7998/120',     // 120 bit IP6-prefix is used for comparison
 'fe80::85d:e82c:9446:7994/120', 'fe80::85d:e82c:9446:7998/120', // 120 bit IP6-prefix is used for comparison
 // Mixed case of IPv4 and IPv6
 '192.168.1.1',      '::ffff:c0a8:0101', // Equal IPs
 '192.168.1.1/24',   '::ffff:c0a8:01ff', // 24 bit IP-prefix is used for comparison
 '::ffff:c0a8:0101', '192.168.1.255/24', // 24 bit IP-prefix is used for comparison
 '::192.168.1.1/30', '192.168.1.255/24', // 24 bit IP-prefix is used for comparison
]
| extend result = ipv6_is_match(ip1_string, ip2_string)
```

|ip1_string|ip2_string|result|
|---|---|---|
|192.168.1.1|192.168.1.1|1|
|192.168.1.1/24|192.168.1.255|1|
|192.168.1.1|192.168.1.255/24|1|
|192.168.1.1/30|192.168.1.255/24|1|
|fe80::85d:e82c:9446:7994|fe80::85d:e82c:9446:7994|1|
|fe80::85d:e82c:9446:7994/120|fe80::85d:e82c:9446:7998|1|
|fe80::85d:e82c:9446:7994|fe80::85d:e82c:9446:7998/120|1|
|fe80::85d:e82c:9446:7994/120|fe80::85d:e82c:9446:7998/120|1|
|192.168.1.1|::ffff:c0a8:0101|1|
|192.168.1.1/24|::ffff:c0a8:01ff|1|
|::ffff:c0a8:0101|192.168.1.255/24|1|
|::192.168.1.1/30|192.168.1.255/24|1|


### IPv6/IPv4 comparison equality case- IP-prefix notation specified inside the IPv6/IPv4 strings and as additional argument of the `ipv6_is_match()` function

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
datatable(ip1_string:string, ip2_string:string, prefix:long)
[
 // IPv4 are compared as IPv6 addresses 
 '192.168.1.1',    '192.168.1.0',   31, // 31 bit IP4-prefix is used for comparison
 '192.168.1.1/24', '192.168.1.255', 31, // 24 bit IP4-prefix is used for comparison
 '192.168.1.1',    '192.168.1.255', 24, // 24 bit IP4-prefix is used for comparison
   // IPv6 cases
 'fe80::85d:e82c:9446:7994', 'fe80::85d:e82c:9446:7995',     127, // 127 bit IP6-prefix is used for comparison
 'fe80::85d:e82c:9446:7994/127', 'fe80::85d:e82c:9446:7998', 120, // 120 bit IP6-prefix is used for comparison
 'fe80::85d:e82c:9446:7994/120', 'fe80::85d:e82c:9446:7998', 127, // 120 bit IP6-prefix is used for comparison
 // Mixed case of IPv4 and IPv6
 '192.168.1.1/24',   '::ffff:c0a8:01ff', 127, // 127 bit IP6-prefix is used for comparison
 '::ffff:c0a8:0101', '192.168.1.255',    120, // 120 bit IP6-prefix is used for comparison
 '::192.168.1.1/30', '192.168.1.255/24', 127, // 120 bit IP6-prefix is used for comparison
]
| extend result = ipv6_is_match(ip1_string, ip2_string, prefix)
```

|ip1_string|ip2_string|prefix|result|
|---|---|---|---|
|192.168.1.1|192.168.1.0|31|1|
|192.168.1.1/24|192.168.1.255|31|1|
|192.168.1.1|192.168.1.255|24|1|
|fe80::85d:e82c:9446:7994|fe80::85d:e82c:9446:7995|127|1|
|fe80::85d:e82c:9446:7994/127|fe80::85d:e82c:9446:7998|120|1|
|fe80::85d:e82c:9446:7994/120|fe80::85d:e82c:9446:7998|127|1|
|192.168.1.1/24|::ffff:c0a8:01ff|127|1|
|::ffff:c0a8:0101|192.168.1.255|120|1|
|::192.168.1.1/30|192.168.1.255/24|127|1|
