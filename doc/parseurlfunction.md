---
title: parse_url() - Azure Data Explorer | Microsoft Docs
description: This article describes parse_url() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# parse_url()

Parses an absolute URL `string` and returns a `dynamic` object contains URL parts.


## Syntax

`parse_url(`*url*`)`

## Arguments

* *url*: A string represents a URL or the query part of the URL.

## Returns

An object of type [dynamic](./scalar-data-types/dynamic.md) that included the URL components: Scheme, Host, Port, Path, Username, Password, Query Parameters, Fragment.

## Example

```kusto
T | extend Result = parse_url("scheme://username:password@host:1234/this/is/a/path?k1=v1&k2=v2#fragment")
```

will result

```
 {
 	"Scheme":"scheme",
 	"Host":"host",
 	"Port":"1234",
 	"Path":"this/is/a/path",
 	"Username":"username",
 	"Password":"password",
 	"Query Parameters":"{"k1":"v1", "k2":"v2"}",
 	"Fragment":"fragment"
 }
```