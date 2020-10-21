---
title: url_encode() - Azure Data Explorer | Microsoft Docs
description: This article describes url_encode() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 03/17/2020
---
# url_encode()

The function converts characters of the input URL into a format that can be transmitted over the Internet. 

Detailed information about URL encoding and decoding can be found [here](https://en.wikipedia.org/wiki/Percent-encoding).
Differs from [url_encode_component](./urlencodecomponentfunction.md) by encoding spaces as '+' and not as '20%' (see application/x-www-form-urlencoded [here](https://en.wikipedia.org/wiki/Percent-encoding)).

## Syntax

`url_encode(`*url*`)`

## Arguments

* *url*: input URL (string).  

## Returns

URL (string) converted into a format that can be transmitted over the Internet.

## Examples

```kusto
let url = @'https://www.bing.com/hello word';
print original = url, encoded = url_encode(url)
```

|original|encoded|
|---|---|
|https://www.bing.com/hello word/|https%3a%2f%2fwww.bing.com%2fhello+word|


 