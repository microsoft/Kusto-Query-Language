---
title: url_encode() - Azure Data Explorer
description: This article describes url_encode() in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 12/27/2022
---
# url_encode()

The function converts characters of the input URL into a format that can be transmitted over the internet.
Differs from [url_encode_component](./urlencodecomponentfunction.md) by encoding spaces as '+' and not as '%20' (see application/x-www-form-urlencoded [here](https://en.wikipedia.org/wiki/Percent-encoding)).

For more details information about URL encoding and decoding, see [Percent-encoding](https://en.wikipedia.org/wiki/Percent-encoding).

## Syntax

`url_encode(`*url*`)`

## Arguments

* *url*: input URL (string).

## Returns

URL (string) converted into a format that can be transmitted over the Internet.

## Examples

```kusto
let url = @'https://www.bing.com/hello world';
print original = url, encoded = url_encode(url)
```

**Output**

|original|encoded|
|---|---|
|https://www.bing.com/hello world/|https%3a%2f%2fwww.bing.com%2fhello+world|
