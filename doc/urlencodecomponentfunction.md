---
title: url_encode_component() - Azure Data Explorer
description: Learn how to use the url_encode_component() function to convert characters of the input URL into a transmittable format.
ms.reviewer: alexans
ms.topic: reference
ms.date: 03/02/2023
---
# url_encode_component()

The function converts characters of the input URL into a format that can be transmitted over the internet. Differs from [url_encode](./urlencodefunction.md) by encoding spaces as '%20' and not as '+'.

For more information about URL encoding and decoding, see [Percent-encoding](https://en.wikipedia.org/wiki/Percent-encoding).

## Syntax

`url_encode_component(`*url*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *url* | string | &check; | The URL to encode.|

## Returns

URL (string) converted into a format that can be transmitted over the Internet.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAyXLMQqAMAxA0V3wDtmqIO2uCN5E1AZbiEmpkV7fiuP/8AgVnkwww2KCarpH50opdo982kMuF5BIoEgm78zUNilHVpAcz8jb56oeAPkQj/7P9a+18iSMrF2d/QtxbpMUagAAAA==" target="_blank">Run the query</a>

```kusto
let url = @'https://www.bing.com/hello world/';
print original = url, encoded = url_encode_component(url)
```

**Output**

|original|encoded|
|---|---|
|https://www.bing.com/hello world/|https%3a%2f%2fwww.bing.com%2fhello%20world|
