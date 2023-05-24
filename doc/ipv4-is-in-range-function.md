---
title:  ipv4_is_in_range()
description: Learn how to use the ipv4_is_in_range() function to check if the IPv4 string address is in the IPv4-prefix notation range.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/01/2023
---
# ipv4_is_in_range()

Checks if IPv4 string address is in IPv4-prefix notation range.

## Syntax

`ipv4_is_in_range(`*Ipv4Address*`,`*Ipv4Range*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *Ipv4Address*| string | &check; | An expression representing an IPv4 address.|
| *Ipv4Range*| string | &check; | An IPv4 range or list of IPv4 ranges written with [IP-prefix notation](#ip-prefix-notation).|

[!INCLUDE [ip-prefix-notation](../../includes/ip-prefix-notation.md)]


## Returns

* `true`: If the long representation of the first IPv4 string argument is in range of the second IPv4 string argument.
* `false`: Otherwise.
* `null`: If conversion for one of the two IPv4 strings wasn't successful.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA4WPsQrCQBBE+/uK7WIgJtwRRQVLCzt7kXDxNmEhXuLtRVL48a4oKFg40z14MONslNYdzmiorHMBmTccA/k2A0HB+hbfIFVHBYlem1wvV7nOdZKB5JdIigJ219F2sD/wH8ksFoUpn1gkU0JNUaz5ELChCYhhZHTQ9AHO/WWwgbj36qTugFNE70Amj12Ercy9lRVxRf41++vS50v6AE4amhHyAAAA" target="_blank">Run the query</a>

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
|192.168.1.1|192.168.1.1|true|
|192.168.1.1|192.168.1.255/24|true|
