---
title:  gzip_compress_to_base64_string 
description: Learn how to use the gzip_compress_to_base64_string() function to gzip-compress an input and encode it into a base64 string.
ms.reviewer: elgevork
ms.topic: reference
ms.date: 12/18/2022
---

# gzip_compress_to_base64_string()

Performs gzip compression and encodes the result to base64.

## Syntax

`gzip_compress_to_base64_string(`*string*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *string* | string | &check; | The value to be compressed and base64 encoded. The function accepts only one argument.|

## Returns

* Returns a `string` that represents gzip-compressed and base64-encoded original string.
* Returns an empty result if compression or encoding failed.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUShKLVawVUivyiyIT87PLQByi+NL8uOTEotTzUzii0uAitI1lAyNjE1MzcwtLA0Ky1OLSipLM/MLlDQBpoplR0IAAAA=" target="_blank">Run the query</a>

```kusto
print res = gzip_compress_to_base64_string("1234567890qwertyuiop")
```

|res|
|--|
|H4sIAAAAAAAA/wEUAOv/MTIzNDU2Nzg5MHF3ZXJ0eXVpb3A6m7f2FAAAAA==|

## See also

* [gzip_decompress_from_base64_string()](gzip-base64-decompress.md)
* [zlib_compress_to_base64_string()](zlib-base64-compress.md)
