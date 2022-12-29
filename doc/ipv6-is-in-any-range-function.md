---
title: ipv6_is_in_any_range() - Azure Data Explorer
description: Learn how to use the ipv6_is_in_any_range function to check if an IPv6 string address is in any of the IPv6 address ranges.
ms.reviewer: alexans
ms.topic: reference
ms.date: 12/21/2022
---
# ipv6_is_in_any_range()

Checks whether an IPv6 string address is in any of the specified IPv6 address ranges.

```kusto
ipv6_is_in_any_range("a5e:f127:8a9d:146d:e102:b5d3:c755:f6cd", dynamic(["a5e:f127:8a9d:146d:e102:b5d3:c755:f6cd/112", "0:0:0:0:0:ffff:c0a8:ac/60"])) == true
ipv6_is_in_any_range("a5e:f127:8a9d:146d:e102:b5d3:c755:f6cd", "a5e:f127:8a9d:146d:e102:b5d3:c755:f6cd/112", "0:0:0:0:0:ffff:c0a8:ac/60") == true
ipv6_is_in_any_range("a5e:f127:8a9d:146d:e102:b5d3:c755:f6cd", "a5e:f127:8a9d:146d:e102:b5d3:c755:f6cd/120", "0:0:0:0:0:ffff:c0a8:ac/60") == false
```

## Syntax

`ipv6_is_in_any_range(`*Ipv6Address* `,` *Ipv6Range* [ `,` *Ipv6Range* ...] `)`

`ipv6_is_in_any_range(`*Ipv6Address* `,` *Ipv6Ranges* `)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *Ipv6Address* | string | &check; | An expression representing an IPv6 address.
| *Ipv6Range* | string | &check; | An expression representing an IPv6 range using [IP-prefix notation](#ip-prefix-notation).
| *Ipv6Ranges* | dynamic array | &check; | An array containing IPv6 ranges using [IP-prefix notation](#ip-prefix-notation).

## IP-prefix notation

IP addresses can be defined with `IP-prefix notation` using a slash (`/`) character.
The IP address to the LEFT of the slash (`/`) is the base IP address. The number (0 to 128) to the RIGHT of the slash (`/`) is the number of contiguous 1 bit in the netmask.

For example, fe80::85d:e82c:9446:7994/120 will have an associated net/subnetmask containing 120 contiguous bits.

## Returns

* `true`: If the IPv6 address is in the range of any of the specified IPv6 networks.
* `false`: Otherwise.
* `null`: If conversion for one of the two IPv6 strings wasn't successful.

## Example

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
let LocalNetworks=dynamic([
    "a5e:f127:8a9d:146d:e102:b5d3:c755:f6cd/112",
    "0:0:0:0:0:ffff:c0a8:ac/60"
]);
let IPs=datatable(IP:string) [
    "a5e:f127:8a9d:146d:e102:b5d3:c755:abcd",
    "a5e:f127:8a9d:146d:e102:b5d3:c755:abce",
    "a5e:f127:8a9d:146d:e102:b5d3:c755:abcf",
    "a5e:f127:8a9d:146d:e102:b5d3:c756:abd1",
];
IPs
| extend IsLocal=ipv6_is_in_any_range(IP, LocalNetworks)
```

**Output**

|IP|IsLocal|
|---|---|
|a5e:f127:8a9d:146d:e102:b5d3:c755:abcd|	True|
|a5e:f127:8a9d:146d:e102:b5d3:c755:abce|	True|
|a5e:f127:8a9d:146d:e102:b5d3:c755:abcf|	True|
|a5e:f127:8a9d:146d:e102:b5d3:c756:abd1|	False|
