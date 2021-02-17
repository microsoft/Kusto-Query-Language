---
title: gzip_compress_to_base64_string - Azure Data Explorer 
description: This article describes the gzip_compress_to_base64_string() command in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: elgevork
ms.service: data-explorer
ms.topic: reference
ms.date: 11/01/2020
---

# gzip_compress_to_base64_string()

Performs gzip compression and encodes the result to base64.


## Syntax

`gzip_compress_to_base64_string("`*input_string*"`)`

## Arguments

*input_string*: Input `string`, a string to be compressed and base64 encoded. The function accepts one string argument.

## Returns

* Returns a `string` that represents gzip-compressed and base64-encoded original string. 
* Returns an empty result if compression or encoding failed.

## Example
```kusto
print res = gzip_compress_to_base64_string("1234567890qwertyuiop")
```

**Output:** 

|H4sIAAAAAAAA/wEUAOv/MTIzNDU2Nzg5MHF3ZXJ0eXVpb3A6m7f2FAAAAA==|

## Next steps

* Use [gzip_decompress_from_base64_string()](gzip-base64-decompress.md) to retrieve the original uncompressed string.
* See also [zlib_compress_to_base64_string()](zlib-base64-compress.md)
