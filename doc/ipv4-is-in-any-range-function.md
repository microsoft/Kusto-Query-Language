---
title: ipv4_is_in_any_range() - Azure Data Explorer
description: This article describes ipv4_is_in_any_range() in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 04/14/2022
---
# ipv4_is_in_any_range()

Checks whether IPv4 string address is in any of the specified IPv4 address ranges.

```kusto
ipv4_is_in_any_range("127.0.0.1", dynamic(["127.0.0.1", "192.168.1.1"])) == true
ipv4_is_in_any_range('192.168.1.6', '192.168.1.1/24', '10.0.0.1/8', '127.1.0.1/16') == true
ipv4_is_in_any_range('192.168.1.1', '192.168.2.1/24', '10.0.0.1/8', '127.1.0.1/16') == false
```

## Syntax

`ipv4_is_in_any_range(`*Ipv4Address* `,` *Ipv4Range* [ `,` *Ipv4Range* ...] `)`

`ipv4_is_in_any_range(`*Ipv4Address* `,` *Ipv4Ranges* `)`

## Arguments

* *Ipv4Address*: A string expression representing an IPv4 address.
* *Ipv4Range*: A string expression representing an IPv4 range using [IP-prefix notation](#ip-prefix-notation).
* *Ipv4Ranges*: A dynamic array containing IPv4 ranges using [IP-prefix notation](#ip-prefix-notation).

## IP-prefix notation

IP addresses can be defined with `IP-prefix notation` using a slash (`/`) character. The IP address to the LEFT of the slash (`/`) is the base IP address. The number (0 to 32) to the RIGHT of the slash (`/`) is the number of contiguous 1 bit in the netmask.

For example, 192.168.2.0/24 will have an associated net/subnetmask containing 24 contiguous bits or 255.255.255.0 in dotted decimal format.

## Returns

* `true`: If the IPv4 address is in the range of any of the specified IPv4 networks.
* `false`: Otherwise.
* `null`: If conversion for one of the two IPv4 strings wasn't successful.

## Examples

### IPv4 range check

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
let LocalNetworks=dynamic([
    "192.168.1.1/16",
    "127.0.0.1/8",
    "10.0.0.1/8"
]);
let IPs=datatable(IP:string) [
    "10.1.2.3",
    "192.168.1.5",
    "123.1.11.21",
    "1.1.1.1"
];
IPs
| extend IsLocal=ipv4_is_in_any_range(IP, LocalNetworks)
```

|IP|IsLocal|
|---|---|
|10.1.2.3|1|
|192.168.1.5|1|
|123.1.11.21|0|
|1.1.1.1|0|
