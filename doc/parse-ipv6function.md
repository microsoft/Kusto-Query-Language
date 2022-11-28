---
title: parse_ipv6() - Azure Data Explorer
description: This article describes parse_ipv6() function in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/13/2022
---
# parse_ipv6()

Converts IPv6 or IPv4 string to a canonical IPv6 string representation.

```kusto
parse_ipv6("127.0.0.1") == '0000:0000:0000:0000:0000:ffff:7f00:0001'
parse_ipv6(":fe80::85d:e82c:9446:7994") == 'fe80:0000:0000:0000:085d:e82c:9446:7994'
```

## Syntax

`parse_ipv6(`*`Expr`*`)`

## Arguments

* *`Expr`*: String expression representing IPv6/IPv4 network address that will be converted to canonical IPv6 representation. String may include net-mask using [IP-prefix notation](#ip-prefix-notation).

## IP-prefix notation

IP addresses can be defined with `IP-prefix notation` using a slash (`/`) character.
The IP address to the LEFT of the slash (`/`) is the base IP address. The number (0 to 128) to the RIGHT of the slash (`/`) is the number of contiguous 1 bits in the netmask.

## Returns

If conversion is successful, the result will be a string representing a canonical IPv6 network address.
If conversion isn't successful, the result will be an empty string.

## Example

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
datatable(ipv4:string)
[
 '192.168.255.255',
 '192.168.255.255/24',
 '255.255.255.255'
]
| extend ipv6 = parse_ipv6(ip_string)
```

| ipv4               | ipv6                                    |
|--------------------|-----------------------------------------|
| 192.168.255.255    | 0000:0000:0000:0000:0000:ffff:c0a8:ffff |
| 192.168.255.255/24 | 0000:0000:0000:0000:0000:ffff:c0a8:ff00 |
| 255.255.255.255    | 0000:0000:0000:0000:0000:ffff:ffff:ffff |
