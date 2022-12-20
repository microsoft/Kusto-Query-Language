---
title: has_any_ipv4() - Azure Data Explorer
description: Learn how to use the has_any_ipv4() function to check if any IPv4 addresses appear in the text.
ms.reviewer: alexans
ms.topic: reference
ms.date: 12/18/2022
---
# has_any_ipv4()

Returns a value indicating whether one of specified IPv4 addresses appears in a text.

IP address entrances in a text must be properly delimited with non-alphanumeric characters. For example, properly delimited IP addresses are:

* "These requests came from: 192.168.1.1, 10.1.1.115 and 10.1.1.201"
* "05:04:54 127.0.0.1 GET /favicon.ico 404"

## Syntax

`has_any_ipv4(`*text* `,` *ip_address* [ `,` *ip_address* ...] `)`

`has_any_ipv4(`*text* `,` *ip_addresses* `)`

## Arguments

* *text*: The value containing the text to search in.
* *ip_address*: String value containing the IP address to look for.
* *ip_addresses*: Dynamic array containing the list of IP addresses to look for.

## Returns

`true` if one of specified IP addresses is a valid IPv4 address, and it was found in *text*. Otherwise, the function returns `false`.

## Examples

```kusto
has_any_ipv4('05:04:54 127.0.0.1 GET /favicon.ico 404', '127.0.0.1', '127.0.0.2') // true

has_any_ipv4('05:04:54 127.0.0.256 GET /favicon.ico 404', dynamic(["127.0.0.256", "192.168.1.1"])) // false, invalid IPv4 address

has_any_ipv4('05:04:54127.0.0.1 GET /favicon.ico 404', '127.0.0.1', '192.168.1.1') // false, improperly delimited IP address
```
