---
title:  ipv6_compare()
description: Learn how to use the ipv6_compare() function to compare two IPv6 or IPv4 network address strings.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/01/2023
---
# ipv6_compare()

Compares two IPv6 or IPv4 network address strings. The two IPv6 strings are parsed and compared while accounting for the combined IP-prefix mask calculated from argument prefixes, and the optional `prefix` argument.

>[!Note]
> The function can accept and compare arguments representing both IPv6 and IPv4 network addresses. However, if the caller knows that arguments are in IPv4 format, use [ipv4_is_compare()](./ipv4-comparefunction.md) function. This function will result in better runtime performance.

## Syntax

`ipv6_compare(`*ip1*`,`*ip2*`[ ,`*prefix*`])`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *ip1*, *ip2*| string | &check; | An expression representing an IPv6 or IPv4 address. IPv6 and IPv4 strings can be masked using [IP-prefix notation](#ip-prefix-notation).|
| *prefix*| int | | An integer from 0 to 128 representing the number of most significant bits that are taken into account.|

[!INCLUDE [ip-prefix-notation](../../includes/ip-prefix-notation.md)]

## Returns

* `0`: If the long representation of the first IPv6 string argument is equal to the second IPv6 string argument.
* `1`: If the long representation of the first IPv6 string argument is greater than the second IPv6 string argument.
* `-1`: If the long representation of the first IPv6 string argument is less than the second IPv6 string argument.
* `null`: If conversion for one of the two IPv6 strings wasn't successful.

## Examples: IPv6/IPv4 comparison equality cases

### Compare IPs using the IP-prefix notation specified inside the IPv6/IPv4 strings

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA61UwU6EMBC98xVzWzdZgVbA0sSjBw8m3o3ZdKE1TRCQwoaDH+/s0igKwkIcDk1m5r2Zvg6Tihq/QyavdEn2pq50/sq7Ywe6pD9dW+fZAc+Dh6djAKKSkBRvJZ4pCHNyRiDStJLGSOPAhsTUJRFziUs2O0AbetCQ7v69ERnif4E8GmBWz0PD0MIQRAM46BpRwXVZSaVb0AYag72oorKNaVPkM40gZ1dnPad34w/6XMNplY0gEZ2ASjKfcxamXDKa8DgIIn4bx2dVJmIwruxfCI9Qf4KRfUuOifYu0aw+K3pntpN/rDZ3NxtfVg2zH3WLgdM7QaHs75Cn59cbmzecOM4VGk98wbhPfNJVnZz8AUqpDvU1UrO6jFQdm9OFnBfN/gLOF+cDZFtLVBCXR5PVcIe75xjt7Xrp7ab+Utp+Aj++uYm9BAAA" target="_blank">Run the query</a>

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
| extend result = ipv6_compare(ip1_string, ip2_string)
```

**Output**

|ip1_string|ip2_string|result|
|---|---|---|
|192.168.1.1|192.168.1.1|0|
|192.168.1.1/24|192.168.1.255|0|
|192.168.1.1|192.168.1.255/24|0|
|192.168.1.1/30|192.168.1.255/24|0|
|fe80::85d:e82c:9446:7994|fe80::85d:e82c:9446:7994|0|
|fe80::85d:e82c:9446:7994/120|fe80::85d:e82c:9446:7998|0|
|fe80::85d:e82c:9446:7994|fe80::85d:e82c:9446:7998/120|0|
|fe80::85d:e82c:9446:7994/120|fe80::85d:e82c:9446:7998/120|0|
|192.168.1.1|::ffff:c0a8:0101|0|
|192.168.1.1/24|::ffff:c0a8:01ff|0|
|::ffff:c0a8:0101|192.168.1.255/24|0|
|::192.168.1.1/30|192.168.1.255/24|0|

### Compare IPs using IP-prefix notation specified inside the IPv6/IPv4 strings and as additional argument of the `ipv6_compare()` function

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA61UTW+DMAy98yt86yp1kITwZWk/YIdJu09TRSGpInWACK162I+fC5kEa7uOauFg6Rm9Z7/YKfOOvs1OPZiGr23XmmqLQ1iBacRPqGmVNkfc1dV26b15EATw/HqQkLcKivqjoVhCbk9gDHlZtspaZcGDBc+Ez+PU5z5frIDOCGE9EvLViS/ksDEdMcjHQQ2Mhb0lXl23TsTYuppyBkISyQgRUUSA4xTyHs6zOgdOIWdygrMphiInN0hEq5QhplGJKhUFZlLGmGRZ38KVXDRUA1wkvTxFpx/f7OmaXEAkv0imlOOCOTn2H3LsplwyV47+fjFHSpzMhVq7gazK3vJLU0J3iqjpYMHyFBnXeiL9d2OnNIxfGsH+zuabiDiuO2Rn1EMv8w179z5BHTtFBtF27ncdPNGmH+K129/RSzB+Ar53f/kFFQW7YjMEAAA=" target="_blank">Run the query</a>

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
| extend result = ipv6_compare(ip1_string, ip2_string, prefix)
```

**Output**

|ip1_string|ip2_string|prefix|result|
|---|---|---|---|
|192.168.1.1|192.168.1.0|31|0|
|192.168.1.1/24|192.168.1.255|31|0|
|192.168.1.1|192.168.1.255|24|0|
|fe80::85d:e82c:9446:7994|fe80::85d:e82c:9446:7995|127|0|
|fe80::85d:e82c:9446:7994/127|fe80::85d:e82c:9446:7998|120|0|
|fe80::85d:e82c:9446:7994/120|fe80::85d:e82c:9446:7998|127|0|
|192.168.1.1/24|::ffff:c0a8:01ff|127|0|
|::ffff:c0a8:0101|192.168.1.255|120|0|
|::192.168.1.1/30|192.168.1.255/24|127|0|
