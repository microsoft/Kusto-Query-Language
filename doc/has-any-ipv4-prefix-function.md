---
title: has_any_ipv4_prefix() - Azure Data Explorer
description: This article describes has_any_ipv4_prefix() in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 05/31/2021
---
# has_any_ipv4_prefix()

Returns a value indicating whether one of specified IPv4 address prefixes appears in a text.

A valid IP address prefix is either a complete IPv4 address (`192.168.1.11`) or its prefix ending with a dot (`192.`, `192.168.` or `192.168.1.`).

IP address entrances in a text must be properly delimited with non-alphanumeric characters. For example, properly delimited IP addresses are:

 * "These requests came from: 192.168.1.1, 10.1.1.115 and 10.1.1.201"
 * "05:04:54 127.0.0.1 GET /favicon.ico 404"

## Syntax

`has_any_ipv4_prefix(`*text* `,` *ip_address_prefix* [`,` *ip_address_prefix* ...] `)`     

`has_any_ipv4_prefix(`*text* `,` *ip_address_prefixes* `)`

## Arguments

* *text*: The value containing the text to search in.
* *ip_address_prefix*: String value containing the IP address prefix to look for.
* *ip_address_prefixes*: Dynamic array containing IP address prefixes to look for.

## Returns

`true` if the one of specified IP address prefixes is a valid IPv4 address prefix, and it was found in *text*. Otherwise, the function returns `false`.

## Examples

```kusto
has_any_ipv4_prefix('05:04:54 127.0.0.1 GET /favicon.ico 404', '127.0.', '192.168.') // true

has_any_ipv4_prefix('05:04:54 127.0.0.1 GET /favicon.ico 404', dynamic(["127.0", "192.168."])) // false, invalid IPv4 prefix

has_any_ipv4_prefix('05:04:54 127.0.0.256 GET /favicon.ico 404', '127.0.', '192.168.') // false, invalid IPv4 address

has_any_ipv4_prefix('05:04:54127.0.0.1 GET /favicon.ico 404', '127.0.', '192.') // false, improperly delimited IP address
```
