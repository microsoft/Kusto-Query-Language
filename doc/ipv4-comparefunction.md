---
title:  ipv4_compare()
description: Learn how to use the ipv4_compare() function to compare two IPv4 strings.
ms.reviewer: alexans
ms.topic: reference
ms.date: 12/28/2022
---
# ipv4_compare()

Compares two IPv4 strings. The two IPv4 strings are parsed and compared while accounting for the combined IP-prefix mask calculated from argument prefixes, and the optional `PrefixMask` argument.

## Syntax

`ipv4_compare(`*Expr1*`,`*Expr2*`[ ,`*PrefixMask*`])`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
|*Expr1*, *Expr2*| string | &check; | A string expression representing an IPv4 address. IPv4 strings can be masked using [IP-prefix notation](#ip-prefix-notation).
|*PrefixMask*| int | | An integer from 0 to 32 representing the number of most-significant bits that are taken into account.

[!INCLUDE [ip-prefix-notation](../../includes/ip-prefix-notation.md)]

## Returns

* `0`: If the long representation of the first IPv4 string argument is equal to the second IPv4 string argument
* `1`: If the long representation of the first IPv4 string argument is greater than the second IPv4 string argument
* `-1`: If the long representation of the first IPv4 string argument is less than the second IPv4 string argument
* `null`: If conversion for one of the two IPv4 strings wasn't successful.

## Examples: IPv4 comparison equality cases

### Compare IPs using the IP-prefix notation specified inside the IPv4 strings

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA51QsQ6CMBDd+YrbkAQprWDUxNHBzd0YAlLIJUhrWwyDH+8ldYAw6d1wyct7715eXTraqpMr1LywzmDfHvyJAbWYQ1FwDSDke5Hw7S7hSRrGQLNEaBiD03MoOzhf7EzEmciINUFEnn9lJBIZVOhItdZGNjgCWhisrKFRBu7qoUuDVvVzy0UOsvRv/rZkm3SR8nfLW/AGOTrZ12CkHToHR6r1lRWeM6192nf0AaIgALOYAQAA" target="_blank">Run the query</a>

```kusto
datatable(ip1_string:string, ip2_string:string)
[
 '192.168.1.0',    '192.168.1.0',       // Equal IPs
 '192.168.1.1/24', '192.168.1.255',     // 24 bit IP-prefix is used for comparison
 '192.168.1.1',    '192.168.1.255/24',  // 24 bit IP-prefix is used for comparison
 '192.168.1.1/30', '192.168.1.255/24',  // 24 bit IP-prefix is used for comparison
]
| extend result = ipv4_compare(ip1_string, ip2_string)
```

**Output**

|ip1_string|ip2_string|result|
|---|---|---|
|192.168.1.0|192.168.1.0|0|
|192.168.1.1/24|192.168.1.255|0|
|192.168.1.1|192.168.1.255/24|0|
|192.168.1.1/30|192.168.1.255/24|0|

### Compare IPs using IP-prefix notation specified inside the IPv4 strings and as additional argument of the `ipv4_compare()` function

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA52QQQrCMBBF9z3F7GqhtiamRQsewJ17kdLatAzUJCSpdOHhDUYh6kpnFh8ew2P4XWPdtiNfoCK1sRrFUPlIARX9RErzHudqlGJIomMEMdnSjJSbjGQkTsFNQFYPsiYp5LkLaNHC/rD0DkADk+Ed9FLDWV5Uo9FI8a7MKXOOgNCicOCppOwP5deXXknZT8pTdAM+Wy460NxMo4Wdq+vKan8T1hn2+CowuQNRhO/LeAEAAA==" target="_blank">Run the query</a>

```kusto
datatable(ip1_string:string, ip2_string:string, prefix:long)
[
 '192.168.1.1',    '192.168.1.0',   31, // 31 bit IP-prefix is used for comparison
 '192.168.1.1/24', '192.168.1.255', 31, // 24 bit IP-prefix is used for comparison
 '192.168.1.1',    '192.168.1.255', 24, // 24 bit IP-prefix is used for comparison
]
| extend result = ipv4_compare(ip1_string, ip2_string, prefix)
```

**Output**

|ip1_string|ip2_string|prefix|result|
|---|---|---|---|
|192.168.1.1|192.168.1.0|31|0|
|192.168.1.1/24|192.168.1.255|31|0|
|192.168.1.1|192.168.1.255|24|0|
