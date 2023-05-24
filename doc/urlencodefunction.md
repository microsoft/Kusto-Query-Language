---
title:  url_encode()
description: Learn how to use the url_encode() function to convert characters of the input URL into a transmittable format.
ms.reviewer: alexans
ms.topic: reference
ms.date: 03/12/2023
---
# url_encode()

The function converts characters of the input URL into a format that can be transmitted over the internet.
Differs from [url_encode_component](./urlencodecomponentfunction.md) by encoding spaces as '+' and not as '%20' (see application/x-www-form-urlencoded [here](https://en.wikipedia.org/wiki/Percent-encoding)).

For more information about URL encoding and decoding, see [Percent-encoding](https://en.wikipedia.org/wiki/Percent-encoding).

## Syntax

`url_encode(`*url*`)`

## Parameters

|Name|Type|Required|Parameters|
|--|--|--|--|
|*url*|string|&check;|The URL to encode.|

## Returns

URL (string) converted into a format that can be transmitted over the Internet.

## Examples

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA8tJLVEoLcpRsFVwUM8oKSkottLXLy8v10vKzEvXS87P1c9IzcnJVyjPL8pJUbfmKijKzCtRyC/KTM/MSwTpAurVUUjNS85PSU2BcOMhPA0gUxMAGwCJQ14AAAA=" target="_blank">Run the query</a>

```kusto
let url = @'https://www.bing.com/hello world';
print original = url, encoded = url_encode(url)
```

**Output**

|original|encoded|
|---|---|
|https://www.bing.com/hello world/|https%3a%2f%2fwww.bing.com%2fhello+world|
