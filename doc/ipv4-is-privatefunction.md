---
title: ipv4_is_private() - Azure Data Explorer
description: This article describes the ipv4_is_private() function in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 11/20/2020
---
# ipv4_is_private()

Checks if IPv4 string address belongs to a set of private network IPs.

[Private network addresses](https://en.wikipedia.org/wiki/Private_network) were originally defined to assist in delaying IPv4 address exhaustion. IP packets originating from or addressed to a private IP address cannot be routed through the public internet.

## Private IPv4 addresses

The Internet Engineering Task Force (IETF) has directed the Internet Assigned Numbers Authority (IANA) to reserve the following IPv4 address ranges for private networks:

| IP address range|Number of addresses|Largest CIDR block (subnet mask)|
|-----------------|-------------------|--------------------------------|
|10.0.0.0 – 10.255.255.255|16777216|10.0.0.0/8 (255.0.0.0)|
|172.16.0.0 – 172.31.255.255|1048576|172.16.0.0/12 (255.240.0.0)|
|192.168.0.0 – 192.168.255.255|65536|192.168.0.0/16 (255.255.0.0)|


```kusto
ipv4_is_private('192.168.1.1/24') == true
ipv4_is_private('10.1.2.3/24') == true
ipv4_is_private('202.1.2.3') == false
ipv4_is_private("127.0.0.1") == false
```

## Syntax

`ipv4_is_private(`*Expr*`)`

## Arguments

*Expr*: A string expression representing an IPv4 address. IPv4 strings can be masked using [IP-prefix notation](#ip-prefix-notation).

### IP-prefix notation

IP addresses can be defined with `IP-prefix notation` using a slash (`/`) character. The IP address to the left of the slash (`/`) is the base IP address. The number (1 to 32) to the right of the slash (`/`) is the number of contiguous 1 bit in the netmask. 

For example, 192.168.2.0/24 will have an associated net/subnetmask containing 24 contiguous bits or 255.255.255.0 in dotted decimal format.

## Returns

* `true`: If the IPv4 address belongs to any of the private network ranges.
* `false`: Otherwise.
* `null`: If parsing of the input as IPv4 address string wasn't successful.

## Example: Check if IPv4 belongs to a private network

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
datatable(ip_string:string)
[
 '10.1.2.3',
 '192.168.1.1/24',
 '127.0.0.1',
]
| extend result = ipv4_is_private(ip_string)
```

|ip_string|result|
|---|---|
|10.1.2.3|1|
|192.168.1.1/24|1|
|127.0.0.1|0|
