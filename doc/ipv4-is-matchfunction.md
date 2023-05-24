---
title:  ipv4_is_match()
description: Learn how to use the ipv4_is_match() function to match two IPv4 strings.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/01/2023
---
# ipv4_is_match()

Matches two IPv4 strings. The two IPv4 strings are parsed and compared while accounting for the combined IP-prefix mask calculated from argument prefixes, and the optional `prefix` argument.

## Syntax

`ipv4_is_match(`*ip1*`,`*ip2*`[ ,`*prefix*`])`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *ip1*, *ip2*| string | &check; | An expression representing an IPv4 address. IPv4 strings can be masked using [IP-prefix notation](#ip-prefix-notation).|
| *prefix*| int | | An integer from 0 to 32 representing the number of most-significant bits that are taken into account.|

[!INCLUDE [ip-prefix-notation](../../includes/ip-prefix-notation.md)]

## Returns

* `true`: If the long representation of the first IPv4 string argument is equal to the second IPv4 string argument.
* `false`: Otherwise.
* `null`: If conversion for one of the two IPv4 strings wasn't successful.

>[!NOTE]
> When matching against an IPv4 address that's not a range, we recommend using the [equals operator](equals-cs-operator.md) (`==`), for better performance.

## Examples

### IPv4 comparison equality - IP-prefix notation specified inside the IPv4 strings

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA51Quw6CQBDs+YrtkAQ57gSjJpYWdvbGkAMO3YTHeXcYCj/eTbCAUOlssclkZ3YypXQ0ea1WqHlmncH2fhhXCKjFnAq8qwc+34uIb3cRj2I/BMKSITAGp2cvazhf7EzEmUjoasKINP3KSCQSyNGRaq2NqnAAtNBbVULVGSi6RkuDtmvnloscZDm++duSbeJFyt8tb94b1OBUW4JRtq8dHKnWV5KhzRrpisek92nhwQew+y1lmQEAAA==" target="_blank">Run the query</a>

```kusto
datatable(ip1_string:string, ip2_string:string)
[
 '192.168.1.0',    '192.168.1.0',       // Equal IPs
 '192.168.1.1/24', '192.168.1.255',     // 24 bit IP-prefix is used for comparison
 '192.168.1.1',    '192.168.1.255/24',  // 24 bit IP-prefix is used for comparison
 '192.168.1.1/30', '192.168.1.255/24',  // 24 bit IP-prefix is used for comparison
]
| extend result = ipv4_is_match(ip1_string, ip2_string)
```

**Output**

|ip1_string|ip2_string|result|
|---|---|---|
|192.168.1.0|192.168.1.0|true|
|192.168.1.1/24|192.168.1.255|true|
|192.168.1.1|192.168.1.255/24|true|
|192.168.1.1/30|192.168.1.255/24|true|

### IPv4 comparison equality - IP-prefix notation specified inside the IPv4 strings and an additional argument of the `ipv4_is_match()` function

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA52QvQrCMBCA9z7FbbVQWxNT0YIP4OYuUvqT1oM2CUkqHXx4g1WIOundcPANH8fXlNZt1fMFKlIYq1F0+XxiQEU/kdK8xSnvpeii4BRASHY0IZttQhISxuDGI6sHWZMY0tQdqNDC4bicHYAGRsMbaKWGWg6q1GikeFemlDmHR2iWOfBUUvaH8uvLWUnZT8pzcAM+WS4a0NyMvYW9y3VlBZpiKG198Xr6IV8FozvG3oupeQEAAA==" target="_blank">Run the query</a>

```kusto
datatable(ip1_string:string, ip2_string:string, prefix:long)
[
 '192.168.1.1',    '192.168.1.0',   31, // 31 bit IP-prefix is used for comparison
 '192.168.1.1/24', '192.168.1.255', 31, // 24 bit IP-prefix is used for comparison
 '192.168.1.1',    '192.168.1.255', 24, // 24 bit IP-prefix is used for comparison
]
| extend result = ipv4_is_match(ip1_string, ip2_string, prefix)
```

**Output**

|ip1_string|ip2_string|prefix|result|
|---|---|---|---|
|192.168.1.1|192.168.1.0|31|true|
|192.168.1.1/24|192.168.1.255|31|true|
|192.168.1.1|192.168.1.255|24|true|
