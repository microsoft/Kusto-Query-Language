---
title: ipv4_netmask_suffix() - Azure Data Explorer
description: This article describes the ipv4_netmask_suffix() function in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/12/2021
---
# ipv4_netmask_suffix()

Returns the value of the IPv4 netmask suffix from IPv4 string address.

```kusto
ipv4_netmask_suffix('192.168.1.1/24') == 24
ipv4_netmask_suffix('192.168.1.1') == 32
```

## Syntax

`ipv4_netmask_suffix(`*Expr*`)`

## Arguments

*Expr*: A string expression representing an IPv4 address. IPv4 strings can be masked using [IP-prefix notation](#ip-prefix-notation).

### IP-prefix notation

IP addresses can be defined with `IP-prefix notation` using a slash (`/`) character. The IP address to the left of the slash (`/`) is the base IP address. The number (1 to 32) to the right of the slash (`/`) is the number of contiguous 1 bit in the netmask. 

For example, 192.168.2.0/24 will have an associated net/subnetmask containing 24 contiguous bits or 255.255.255.0 in dotted decimal format.

## Returns

* The value of the netmask suffix the IPv4 address. If suffix is not present in the input, a value of `32` (full netmask suffix) is returned.
* `null`: If parsing of the input as IPv4 address string wasn't successful.

## Example: Resolve IPv4 mask suffix

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
datatable(ip_string:string)
[
 '10.1.2.3',
 '192.168.1.1/24',
 '127.0.0.1/16',
]
| extend cidr_suffix = ipv4_netmask_suffix(ip_string)
```

|ip_string|cidr_suffix|
|---|---|
|10.1.2.3|32|
|192.168.1.1/24|24|
|127.0.0.1/16|16|
