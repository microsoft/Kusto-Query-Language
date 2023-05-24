---
title:  ipv6_is_in_range()
description: Learn how to use the ipv6_is_in_range() function to check if an IPv6 string address is in the Ipv6-prefix notation range.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/01/2023
---
# ipv6_is_in_range()

Checks if an IPv6 string address is in the IPv6-prefix notation range.

## Syntax

`ipv6_is_in_range(`*Ipv6Address*`,`*Ipv6Range*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *Ipv6Address* | string | &check; | An expression representing an IPv6 address.|
| *Ipv6Range*| string | &check; | An expression representing an IPv6 range using [IP-prefix notation](#ip-prefix-notation).|

[!INCLUDE [ip-prefix-notation](../../includes/ip-prefix-notation.md)]

## Returns

* `true`: If the long representation of the first IPv6 string argument is in range of the second IPv6 string argument.
* `false`: Otherwise.
* `null`: If conversion for one of the two IPv6 strings wasn't successful.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA62QwQrCMBBE7/2K3Gqh0KSati74JSJlm92WQAnSRPHgx7tQRY8KztyG4R0eYZIOM2/8uUeihWOEmBYfplLJtGCY+DkU2TFTOVqG0dQtdLgnMLuGgI2uYbC0BddaCzg4yksl+eKsJZUxdV7+n72ef+VqeHWUgNPYAbqq0YI6ZXfFt8SBlJi6zEkdxNK16X3sfVhtfZh8KywepKIru2kBAAA=" target="_blank">Run the query</a>

```kusto
datatable(ip_address:string, ip_range:string)
[
 'a5e:f127:8a9d:146d:e102:b5d3:c755:abcd',    'a5e:f127:8a9d:146d:e102:b5d3:c755:0000/112',
 'a5e:f127:8a9d:146d:e102:b5d3:c755:abcd',    'a5e:f127:8a9d:146d:e102:b5d3:c755:abcd',
 'a5e:f127:8a9d:146d:e102:b5d3:c755:abcd',    '0:0:0:0:0:ffff:c0a8:ac/60',
]
| extend result = ipv6_is_in_range(ip_address, ip_range)
```

**Output**

|ip_address|ip_range|result|
|---|---|---|
|a5e:f127:8a9d:146d:e102:b5d3:c755:abcd|a5e:f127:8a9d:146d:e102:b5d3:c755:0000/112|True|
|a5e:f127:8a9d:146d:e102:b5d3:c755:abcd|a5e:f127:8a9d:146d:e102:b5d3:c755:abcd|True|
|a5e:f127:8a9d:146d:e102:b5d3:c755:abcd|0:0:0:0:0:ffff:c0a8:ac/60|False|
