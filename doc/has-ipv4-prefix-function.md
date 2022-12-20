---
title: has_ipv4_prefix() - Azure Data Explorer
description: Learn how to use the has_ipv4_prefix() function to check if a specified IPv4 address prefix appears in the text.
ms.reviewer: alexans
ms.topic: reference
ms.date: 12/18/2022
---
# has_ipv4_prefix()

Returns a value indicating whether a specified IPv4 address prefix appears in a text.

A valid IP address prefix is either a complete IPv4 address (`192.168.1.11`) or its prefix ending with a dot (`192.`, `192.168.` or `192.168.1.`).

IP address entrances in a text must be properly delimited with non-alphanumeric characters. For example, properly delimited IP addresses are:

* "These requests came from: 192.168.1.1, 10.1.1.115 and 10.1.1.201"
* "05:04:54 127.0.0.1 GET /favicon.ico 404"

## Syntax

`has_ipv4_prefix(`*source* `,` *ip_address_prefix* `)`

## Arguments

* *source*: The value containing the text to search in.
* *ip_address_prefix*: String value containing the IP address prefix to look for.

## Returns

`true` if the *ip_address_prefix* is a valid IPv4 address prefix, and it was found in *source*. Otherwise, the function returns `false`.

> [!TIP]
> To search for many IPv4 prefixes at once, use the [has_any_ipv4_prefix()](has-any-ipv4-prefix-function.md) function.

## Examples

```kusto
has_ipv4_prefix('05:04:54 127.0.0.1 GET /favicon.ico 404', '127.0.') // true

has_ipv4_prefix('05:04:54 127.0.0.1 GET /favicon.ico 404', '127.0') // false, invalid IPv4 prefix

has_ipv4_prefix('05:04:54 127.0.0.256 GET /favicon.ico 404', '127.0.') // false, invalid IPv4 address

has_ipv4_prefix('05:04:54127.0.0.1 GET /favicon.ico 404', '127.0.') // false, improperly delimited IP address
```
