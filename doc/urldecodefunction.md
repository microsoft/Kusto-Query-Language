---
title: url_decode() - Azure Data Explorer
description: This article describes url_decode() in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 12/27/2022
---
# url_decode()

The function converts encoded URL into a regular URL representation.

For more details information about URL encoding and decoding, see [Percent-encoding](https://en.wikipedia.org/wiki/Percent-encoding).

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

**Output**

|original|decoded|
|---|---|
|https%3a%2f%2fwww.bing.com%2f|https://www.bing.com/|
