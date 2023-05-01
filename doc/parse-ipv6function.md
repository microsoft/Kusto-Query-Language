---
title: parse_ipv6() - Azure Data Explorer
description: Learn how to use the parse_ipv6() function to convert IPv6 or IPv4 strings to a canonical IPv6 string representation. 
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/08/2023
---
# parse_ipv6()

Converts IPv6 or IPv4 string to a canonical IPv6 string representation.

## Syntax

`parse_ipv6(`*ip*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *ip* | string | &check; | The IPv6/IPv4 network address that will be converted to canonical IPv6 representation. The value may include net-mask using [IP-prefix notation](#ip-prefix-notation).|

[!INCLUDE [ip-prefix-notation](../../includes/ip-prefix-notation.md)]

## Returns

If conversion is successful, the result will be a string representing a canonical IPv6 network address.
If conversion isn't successful, the result will be an empty string.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA0tJLAHCpJxUjcyCMhMrheKSosy8dE2uaC4FIFA3tDTSMzSz0DMyNQVhdR0MIX0jE5AolAdXyBXLVaOQWlGSmpeiADTZTMFWoSCxqDg1HsQB26UJANsJke17AAAA" target="_blank">Run the query</a>

```kusto
datatable(ipv4: string)
[
    '192.168.255.255', '192.168.255.255/24', '255.255.255.255'
]
| extend ipv6 = parse_ipv6(ipv4)
```

**Output**

| ipv4               | ipv6                                    |
|--------------------|-----------------------------------------|
| 192.168.255.255    | 0000:0000:0000:0000:0000:ffff:c0a8:ffff |
| 192.168.255.255/24 | 0000:0000:0000:0000:0000:ffff:c0a8:ff00 |
| 255.255.255.255    | 0000:0000:0000:0000:0000:ffff:ffff:ffff |
