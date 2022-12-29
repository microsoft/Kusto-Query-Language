---
title: ipv4_is_in_range() - Azure Data Explorer
description: Learn how to use the ipv4_is_in_range() function to check if the IPv4 string address is in the IPv4-prefix notation range.
ms.reviewer: alexans
ms.topic: reference
ms.date: 12/20/2022
---
# ipv4_is_in_range()

Checks if IPv4 string address is in IPv4-prefix notation range.

```kusto
ipv4_is_in_range("127.0.0.1", "127.0.0.1") == true
ipv4_is_in_range('192.168.1.6', '192.168.1.1/24') == true
ipv4_is_in_range('192.168.1.1', '192.168.2.1/24') == false
```

## Syntax

`ipv4_is_in_range(`*Ipv4Address*`,`*Ipv4Range*`)`

## Arguments

* *Ipv4Address*: A string expression representing an IPv4 address.
* *Ipv4Range*: A string expression representing an IPv4 range using [IP-prefix notation](#ip-prefix-notation).

## IP-prefix notation

IP addresses can be defined with `IP-prefix notation` using a slash (`/`) character. The IP address to the LEFT of the slash (`/`) is the base IP address. The number (0 to 32) to the RIGHT of the slash (`/`) is the number of contiguous 1 bit in the netmask.

For example, 192.168.2.0/24 will have an associated net/subnetmask containing 24 contiguous bits or 255.255.255.0 in dotted decimal format.

## Returns

* `true`: If the long representation of the first IPv4 string argument is in range of the second IPv4 string argument.
* `false`: Otherwise.
* `null`: If conversion for one of the two IPv4 strings wasn't successful.

## Examples

### IPv4 range check

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
datatable(ip_address:string, ip_range:string)
[
 '192.168.1.1',    '192.168.1.1',       // Equal IPs
 '192.168.1.1',    '192.168.1.255/24',  // 24 bit IP-prefix is used for comparison
]
| extend result = ipv4_is_in_range(ip_address, ip_range)
```

**Output**

|ip_address|ip_range|result|
|---|---|---|
|192.168.1.1|192.168.1.1|1|
|192.168.1.1|192.168.1.255/24|1|
