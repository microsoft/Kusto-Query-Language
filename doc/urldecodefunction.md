---
title: url_decode() - Azure Data Explorer | Microsoft Docs
description: This article describes url_decode() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# url_decode()

The function converts encoded URL into a to regular URL representation. 

Detailed information about URL decoding and encoding can be found [here](https://en.wikipedia.org/wiki/Percent-encoding).

## Syntax

`url_decode(`*encoded url*`)`

## Arguments

* *encoded url*: encoded URL (string).  

## Returns

URL (string) in a regular representation.

## Examples

```kusto
let url = @'https%3a%2f%2fwww.bing.com%2f';
print original = url, decoded = url_decode(url)
```

|original|decoded|
|---|---|
|https%3a%2f%2fwww.bing.com%2f|https://www.bing.com/|



 