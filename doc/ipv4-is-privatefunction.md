---
title:  ipv4_is_private()
description: Learn how to use the ipv4_is_private() function to check if the IPv4 string address belongs to a set of private network IPs.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/01/2023
---
# ipv4_is_private()

Checks if the IPv4 string address belongs to a set of private network IPs.

[Private network addresses](https://en.wikipedia.org/wiki/Private_network) were originally defined to assist in delaying IPv4 address exhaustion. IP packets originating from or addressed to a private IP address can't be routed through the public internet.

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

`ipv4_is_private(`*ip*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
|*ip*| string| &check; | An expression representing an IPv4 address. IPv4 strings can be masked using [IP-prefix notation](#ip-prefix-notation).|

[!INCLUDE [ip-prefix-notation](../../includes/ip-prefix-notation.md)]

## Returns

* `true`: If the IPv4 address belongs to any of the private network ranges.
* `false`: Otherwise.
* `null`: If parsing of the input as IPv4 address string wasn't successful.

## Example: Check if IPv4 belongs to a private network

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA0tJLAHCpJxUjcyC+OKSosy8dCsIpckVzaWgbmigZ6hnpGesrgPiWBrpGZpZAEUM9Y1MIEJG5noGQGgI5MVy1SikVpSk5qUoFKUWl+aUKNgqZBaUmcRnFscXFGWWJZYg2aIJAAFWRs16AAAA" target="_blank">Run the query</a>

```kusto
datatable(ip_string:string)
[
 '10.1.2.3',
 '192.168.1.1/24',
 '127.0.0.1',
]
| extend result = ipv4_is_private(ip_string)
```

**Output**

|ip_string|result|
|---|---|
|10.1.2.3|true|
|192.168.1.1/24|true|
|127.0.0.1|false|
