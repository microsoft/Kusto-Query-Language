---
title: url_encode_component() - Azure Data Explorer
description: This article describes url_encode_component() in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 12/27/2022
---
# url_encode_component()

The function converts characters of the input URL into a format that can be transmitted over the internet. Differs from [url_encode](./urlencodefunction.md) by encoding spaces as '%20' and not as '+'.

For more details information about URL encoding and decoding, see [Percent-encoding](https://en.wikipedia.org/wiki/Percent-encoding).

## Syntax

`url_encode_component(`*url*`)`

## Arguments

* *url*: input URL (string).

## Returns

URL (string) converted into a format that can be transmitted over the Internet.

## Examples

```kusto
let url = @'https://www.bing.com/hello world/';
print original = url, encoded = url_encode_component(url)
```

**Output**

|original|encoded|
|---|---|
|https://www.bing.com/hello world/|https%3a%2f%2fwww.bing.com%2fhello%20world|
