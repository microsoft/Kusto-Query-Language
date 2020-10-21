---
title: parse_urlquery() - Azure Data Explorer | Microsoft Docs
description: This article describes parse_urlquery() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# parse_urlquery()

Returns a `dynamic` object contains the Query parameters.

## Syntax

`parse_urlquery(`*query*`)`

## Arguments

* *query*: A string represents a url query.

## Returns

An object of type [dynamic](./scalar-data-types/dynamic.md) that includes the query parameters.

## Example

```kusto
parse_urlquery("k1=v1&k2=v2&k3=v3")
```

will result:

```kusto
 {
 	"Query Parameters":"{"k1":"v1", "k2":"v2", "k3":"v3"}",
 }
```

**Notes**

* Input format should follow URL query standards (key=value& ...)
 