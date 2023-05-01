---
title: ipv4_netmask_suffix() - Azure Data Explorer
description: Learn how to use the ipv4_netmask_suffix() function to return the value of the IPv4 netmask suffix from an IPv4 string address.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/01/2023
---
# ipv4_netmask_suffix()

Returns the value of the IPv4 netmask suffix from an IPv4 string address.

## Syntax

`ipv4_netmask_suffix(`*ip*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
|*ip*| string | &check;| An expression representing an IPv4 address. IPv4 strings can be masked using [IP-prefix notation](#ip-prefix-notation).|

[!INCLUDE [ip-prefix-notation](../../includes/ip-prefix-notation.md)]

## Returns

* The value of the netmask suffix the IPv4 address. If the suffix isn't present in the input, a value of `32` (full netmask suffix) is returned.
* `null`: If parsing the input as an IPv4 address string wasn't successful.

## Example: Resolve IPv4 mask suffix

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA0tJLAHCpJxUjcyC+OKSosy8dCsIpckVzaWgbmigZ6hnpGesrgPiWBrpGZpZAEUM9Y1MIEJG5noGQGiob2gGFIjlqlFIrShJzUtRSM5MKYovLk1Ly6xQsFXILCgzic9LLclNLM6GiiJs1AQAK1xCiYYAAAA=" target="_blank">Run the query</a>

```kusto
datatable(ip_string:string)
[
 '10.1.2.3',
 '192.168.1.1/24',
 '127.0.0.1/16',
]
| extend cidr_suffix = ipv4_netmask_suffix(ip_string)
```

**Output**

|ip_string|cidr_suffix|
|---|---|
|10.1.2.3|32|
|192.168.1.1/24|24|
|127.0.0.1/16|16|
