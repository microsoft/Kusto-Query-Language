---
title: has_ipv4() - Azure Data Explorer
description: Learn how to use the has_ipv4() function to check if a specified IPv4 address appears in the text.
ms.reviewer: alexans
ms.topic: reference
ms.date: 12/18/2022
---
# has_ipv4()

Returns a value indicating whether a specified IPv4 address appears in a text.

IP address entrances in a text must be properly delimited with non-alphanumeric characters. For example, properly delimited IP addresses are:

* "These requests came from: 192.168.1.1, 10.1.1.115 and 10.1.1.201"
* "05:04:54 127.0.0.1 GET /favicon.ico 404"

## Syntax

`has_ipv4(`*source* `,` *ip_address* `)`

## Arguments

* *source*: The value containing the text to search in.
* *ip_address*: String value containing the IP address to look for.

## Returns

`true`  if the *ip_address* is a valid IPv4 address, and it was found in *source*. Otherwise, the function returns `false`.

> [!TIP]
>
> * To search for many IPv4 addresses at once, use [has_any_ipv4()](has-any-ipv4-function.md) function.
> * To search for IPv4 addresses prefix, use [has_ipv4_prefix()](has-ipv4-prefix-function.md) function.

## Examples

```kusto
has_ipv4('05:04:54 127.0.0.1 GET /favicon.ico 404', '127.0.0.1') // true

has_ipv4('05:04:54 127.0.0.256 GET /favicon.ico 404', '127.0.0.256') // false, invalid IPv4 address

has_ipv4('05:04:54127.0.0.1 GET /favicon.ico 404', '127.0.0.1') // false, improperly delimited IP address
```
