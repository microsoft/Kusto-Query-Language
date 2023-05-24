---
title:  ipv6_is_match()
description: Learn how to use the ipv6_is_match() function to match two IPv6 or IPv4 network address strings.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/01/2023
---
# ipv6_is_match()

Matches two IPv6 or IPv4 network address strings. The two IPv6/IPv4 strings are parsed and compared while accounting for the combined IP-prefix mask calculated from argument prefixes, and the optional `prefix` argument.

> [!NOTE]
> The function can accept and compare arguments representing both IPv6 and IPv4 network addresses. If the caller knows that arguments are in IPv4 format, use the [ipv4_is_match()](./ipv4-is-matchfunction.md) function. This function will result in better runtime performance.

## Syntax

`ipv6_is_match(`*ip1*`,`*ip2*`[ ,`*prefix*`])`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *ip1*, *ip2*| string | &check; | An expression representing an IPv6 or IPv4 address. IPv6 and IPv4 strings can be masked using [IP-prefix notation](#ip-prefix-notation).|
| *prefix*| int | | An integer from 0 to 128 representing the number of most-significant bits that are taken into account.|

[!INCLUDE [ip-prefix-notation](../../includes/ip-prefix-notation.md)]

## Returns

* `true`: If the long representation of the first IPv6/IPv4 string argument is equal to the second IPv6/IPv4 string argument.
* `false`: Otherwise.
* `null`: If conversion for one of the two IPv6/IPv4 strings wasn't successful.

## Examples

### IPv6/IPv4 comparison equality case - IP-prefix notation specified inside the IPv6/IPv4 strings

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA61UwW6DMAy98xW+dZW6QjJgIdKOO+wwafdpQilJtkgUGIGKwz5+bok2ttKyoplDJNvv2XkxlqLBb5OrK1OR1Da1KV55f6zAVPSna+k9e+D78PC0C0HUCrJyW+EpQdi9MwYhZa2sVdaDBUnomsRsTdZksQK0Yw8a0t2/tyJH/C+QT0PMGnhoFDkYgmgIG9MgKryuaqVNB8ZCa7EXXdauMWPLYqIR5OzrzOf0b4KjPudwOmVjyEQvoFYs4JxFkitGM56EYcxvk+SgypkYjCt7CuETGpxhZN+SY6K7Szypz4zemevkH6tN3c3FL6uG2Y+mw8D+naDU7nco5OH1xuYNJ45zjcazQDAekID0Vc9O/hFK6x71NVKTuoxUHZvTCzn/NPsXcL54H6C6RqGCuDzavIE73D27ODU23Yomexssp+FWWn4C162p674EAAA=" target="_blank">Run the query</a>

```kusto
datatable(ip1_string:string, ip2_string:string)
[
 // IPv4 are compared as IPv6 addresses
 '192.168.1.1',    '192.168.1.1',       // Equal IPs
 '192.168.1.1/24', '192.168.1.255',     // 24 bit IP4-prefix is used for comparison
 '192.168.1.1',    '192.168.1.255/24',  // 24 bit IP4-prefix is used for comparison
 '192.168.1.1/30', '192.168.1.255/24',  // 24 bit IP4-prefix is used for comparison
  // IPv6 cases
 'fe80::85d:e82c:9446:7994', 'fe80::85d:e82c:9446:7994',         // Equal IPs
 'fe80::85d:e82c:9446:7994/120', 'fe80::85d:e82c:9446:7998',     // 120 bit IP6-prefix is used for comparison
 'fe80::85d:e82c:9446:7994', 'fe80::85d:e82c:9446:7998/120',     // 120 bit IP6-prefix is used for comparison
 'fe80::85d:e82c:9446:7994/120', 'fe80::85d:e82c:9446:7998/120', // 120 bit IP6-prefix is used for comparison
 // Mixed case of IPv4 and IPv6
 '192.168.1.1',      '::ffff:c0a8:0101', // Equal IPs
 '192.168.1.1/24',   '::ffff:c0a8:01ff', // 24 bit IP-prefix is used for comparison
 '::ffff:c0a8:0101', '192.168.1.255/24', // 24 bit IP-prefix is used for comparison
 '::192.168.1.1/30', '192.168.1.255/24', // 24 bit IP-prefix is used for comparison
]
| extend result = ipv6_is_match(ip1_string, ip2_string)
```

**Output**

|ip1_string|ip2_string|result|
|---|---|---|
|192.168.1.1|192.168.1.1|1|
|192.168.1.1/24|192.168.1.255|1|
|192.168.1.1|192.168.1.255/24|1|
|192.168.1.1/30|192.168.1.255/24|1|
|fe80::85d:e82c:9446:7994|fe80::85d:e82c:9446:7994|1|
|fe80::85d:e82c:9446:7994/120|fe80::85d:e82c:9446:7998|1|
|fe80::85d:e82c:9446:7994|fe80::85d:e82c:9446:7998/120|1|
|fe80::85d:e82c:9446:7994/120|fe80::85d:e82c:9446:7998/120|1|
|192.168.1.1|::ffff:c0a8:0101|1|
|192.168.1.1/24|::ffff:c0a8:01ff|1|
|::ffff:c0a8:0101|192.168.1.255/24|1|
|::192.168.1.1/30|192.168.1.255/24|1|

### IPv6/IPv4 comparison equality case- IP-prefix notation specified inside the IPv6/IPv4 strings and as additional argument of the `ipv6_is_match()` function

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA61UTW+DMAy98yt86yp1kKSBgqX9gB0m7T5NVQpJF6kFRGjFYT9+5mMSXdu1TAsHS8/oPfvFTqZq+jY7/WBLvnZ1ZfMt9mEBthQ/obLSxja4K/Lt3HvzIAjg+fUoQVUa0mJfUsxAuRaMQGVZpZ3TDjyY8UT4PIp97vPZAuiMENYhS75o+ZYcNrYmBvnYq4F1cHDEa4pqELGuyE85AyGJZISIMCRg4BTyL5xndfacQk7khMGmCFJFbpCI0TFDjMMMdSxSTKSMcJUkXQtXcmFfDXCx6uQpDvrRzZ6uyQVE8otkTDku2CDH/kOO3ZRbTZWjv19sQ4nWXCjMMJB51ll+aUroThENHUyZipFxY06k7zf2lIbxSyPY3dl0ExHHdS/ZGXXfy3TD3r1P0E2tySDazsOuhifa9GO0tm69V3X6MXoKxm/A9/LPvwDOD5PYNAQAAA==" target="_blank">Run the query</a>

```kusto
datatable(ip1_string:string, ip2_string:string, prefix:long)
[
 // IPv4 are compared as IPv6 addresses 
 '192.168.1.1',    '192.168.1.0',   31, // 31 bit IP4-prefix is used for comparison
 '192.168.1.1/24', '192.168.1.255', 31, // 24 bit IP4-prefix is used for comparison
 '192.168.1.1',    '192.168.1.255', 24, // 24 bit IP4-prefix is used for comparison
   // IPv6 cases
 'fe80::85d:e82c:9446:7994', 'fe80::85d:e82c:9446:7995',     127, // 127 bit IP6-prefix is used for comparison
 'fe80::85d:e82c:9446:7994/127', 'fe80::85d:e82c:9446:7998', 120, // 120 bit IP6-prefix is used for comparison
 'fe80::85d:e82c:9446:7994/120', 'fe80::85d:e82c:9446:7998', 127, // 120 bit IP6-prefix is used for comparison
 // Mixed case of IPv4 and IPv6
 '192.168.1.1/24',   '::ffff:c0a8:01ff', 127, // 127 bit IP6-prefix is used for comparison
 '::ffff:c0a8:0101', '192.168.1.255',    120, // 120 bit IP6-prefix is used for comparison
 '::192.168.1.1/30', '192.168.1.255/24', 127, // 120 bit IP6-prefix is used for comparison
]
| extend result = ipv6_is_match(ip1_string, ip2_string, prefix)
```

**Output**

|ip1_string|ip2_string|prefix|result|
|---|---|---|---|
|192.168.1.1|192.168.1.0|31|1|
|192.168.1.1/24|192.168.1.255|31|1|
|192.168.1.1|192.168.1.255|24|1|
|fe80::85d:e82c:9446:7994|fe80::85d:e82c:9446:7995|127|1|
|fe80::85d:e82c:9446:7994/127|fe80::85d:e82c:9446:7998|120|1|
|fe80::85d:e82c:9446:7994/120|fe80::85d:e82c:9446:7998|127|1|
|192.168.1.1/24|::ffff:c0a8:01ff|127|1|
|::ffff:c0a8:0101|192.168.1.255|120|1|
|::192.168.1.1/30|192.168.1.255/24|127|1|
