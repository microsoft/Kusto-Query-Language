---
title: ipv6_is_in_range() - Azure Data Explorer
description: This article describes ipv6_is_in_range() in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 02/12/2021
---
# ipv6_is_in_range()

Checks if an IPv6 string address is in IPv6-prefix notation range.

```kusto
ipv6_is_in_range("a5e:f127:8a9d:146d:e102:b5d3:c755:f6cd", "a5e:f127:8a9d:146d:e102:b5d3:c755:f6cd/112") == true
ipv6_is_in_range("a5e:f127:8a9d:146d:e102:b5d3:c755:f6cd", "a5e:f127:8a9d:146d:e102:b5d3:c755:f6cd/200") == false
```

## Syntax

`ipv6_is_in_range(`*Ipv6Address*`, `*Ipv6Range*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *Ipv6Address* | string | &check; | An expression representing an IPv6 address. 
| *Ipv6Range*| string | &check; | An expression representing an IPv6 range using [IP-prefix notation](#ip-prefix-notation).

## IP-prefix notation

IP addresses can be defined with `IP-prefix notation` using a slash (`/`) character.
The IP address to the LEFT of the slash (`/`) is the base IP address. The number (0 to 128) to the RIGHT of the slash (`/`) is the number of contiguous 1 bit in the netmask. 

For example, fe80::85d:e82c:9446:7994/120 will have an associated net/subnetmask containing 120 contiguous bits.

## Returns

* `true`: If the long representation of the first IPv6 string argument is in range of the second IPv6 string argument.
* `false`: Otherwise.
* `null`: If conversion for one of the two IPv6 strings wasn't successful.

## Example

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
datatable(ip_address:string, ip_range:string)
[
 'a5e:f127:8a9d:146d:e102:b5d3:c755:abcd',    'a5e:f127:8a9d:146d:e102:b5d3:c755:0000/112',
 'a5e:f127:8a9d:146d:e102:b5d3:c755:abcd',    'a5e:f127:8a9d:146d:e102:b5d3:c755:abcd',
 'a5e:f127:8a9d:146d:e102:b5d3:c755:abcd',    '0:0:0:0:0:ffff:c0a8:ac/60',
]
| extend result = ipv6_is_in_range(ip_address, ip_range)
```

|ip_address|ip_range|result|
|---|---|---|
|a5e:f127:8a9d:146d:e102:b5d3:c755:abcd|a5e:f127:8a9d:146d:e102:b5d3:c755:0000/112|True|
|a5e:f127:8a9d:146d:e102:b5d3:c755:abcd|a5e:f127:8a9d:146d:e102:b5d3:c755:abcd|True|
|a5e:f127:8a9d:146d:e102:b5d3:c755:abcd|0:0:0:0:0:ffff:c0a8:ac/60|False|
