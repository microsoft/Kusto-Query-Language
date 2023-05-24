---
title:  has_ipv4_prefix()
description: Learn how to use the has_ipv4_prefix() function to check if a specified IPv4 address prefix appears in the text.
ms.reviewer: alexans
ms.topic: reference
ms.date: 12/25/2022
---
# has_ipv4_prefix()

Returns a value indicating whether a specified IPv4 address prefix appears in a text.

A valid IP address prefix is either a complete IPv4 address (`192.168.1.11`) or its prefix ending with a dot (`192.`, `192.168.` or `192.168.1.`).

IP address entrances in a text must be properly delimited with non-alphanumeric characters. For example, properly delimited IP addresses are:

* "These requests came from: 192.168.1.1, 10.1.1.115 and 10.1.1.201"
* "05:04:54 127.0.0.1 GET /favicon.ico 404"

## Syntax

`has_ipv4_prefix(`*source* `,` *ip_address_prefix* `)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *source*| string| &check;| The text to search.|
| *ip_address_prefix*| string| &check;| The IP address prefix for which to search.|

## Returns

`true` if the *ip_address_prefix* is a valid IPv4 address prefix, and it was found in *source*. Otherwise, the function returns `false`.

> [!TIP]
> To search for many IPv4 prefixes at once, use the [has_any_ipv4_prefix()](has-any-ipv4-prefix-function.md) function.

## Examples

### Properly formatted IPv4 prefix

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUShKLS7NKbHNSCyOzywoM4kvKEpNy6zQUDcwtTIwsTI1UTA0MtczAEJDBXfXEAX9tMSyzOT8PD0goWBiYKKuo6AOUaGuCQBk8fTRUQAAAA==" target="_blank">Run the query</a>

```kusto
print result=has_ipv4_prefix('05:04:54 127.0.0.1 GET /favicon.ico 404', '127.0.')
```

|result|
|--|
|true|

### Invalid IPv4 prefix

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUShKLS7NKbHNSCyOzywoM4kvKEpNy6zQUDcwtTIwsTI1UTA0MtczAEJDBXfXEAX9tMSyzOT8PD0goWBiYKKuo6AOVqGuCQDlc4Z2UAAAAA==" target="_blank">Run the query</a>

```kusto
print result=has_ipv4_prefix('05:04:54 127.0.0.1 GET /favicon.ico 404', '127.0')
```

|result|
|--|
|false|

### Invalid IPv4 address

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUShKLS7NKbHNSCyOzywoM4kvKEpNy6zQUDcwtTIwsTI1UTA0MtczAEIjUzMFd9cQBf20xLLM5Pw8PSChYGJgoq6joA5Ro64JAMAcwIpTAAAA" target="_blank">Run the query</a>

```kusto
print result=has_ipv4_prefix('05:04:54 127.0.0.256 GET /favicon.ico 404', '127.0.')
```

|result|
|--|
|false|

### Improperly delimited IPv4 address

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUShKLS7NKbHNSCyOzywoM4kvKEpNy6zQUDcwtTIwsTI1MTQy1zMAQkMFd9cQBf20xLLM5Pw8PSChYGJgoq6joA5Roa4JAD4FydVQAAAA" target="_blank">Run the query</a>

```kusto
print result=has_ipv4_prefix('05:04:54127.0.0.1 GET /favicon.ico 404', '127.0.')
```

|result|
|--|
|false|
