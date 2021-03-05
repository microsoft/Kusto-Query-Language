---
title: gzip_decompress_from_base64_string() - Azure Data Explorer 
description: This article describes the gzip_decompress_from_base64_string() command in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: elgevork
ms.service: data-explorer
ms.topic: reference
ms.date: 11/01/2020
---
# gzip_decompress_from_base64_string()

Decodes the input string from base64 and performs gzip decompression.

## Syntax

`gzip_decompress_from_base64_string("`*input_string*`")`

## Arguments

*input_string*: Input `string` that was compressed with gzip and then base64-encoded. The function accepts one string argument.

> [!NOTE]
> This function checks mandatory gzip header fields (ID1, ID2, and CM) and returns an empty output if any of these fields have incorrect values.
> Optional header fields are not supported, and FLG is expected to be zero.


## Returns

* Returns a `string` that represents the original string. 
* Returns an empty result if decompression or decoding failed. 
    * For example, invalid gzip-compressed and base 64-encoded strings will return an empty output.

## Examples

```kusto
print res=gzip_decompress_from_base64_string("H4sIAAAAAAAA/wEUAOv/MTIzNDU2Nzg5MHF3ZXJ0eXVpb3A6m7f2FAAAAA==")
```

**Output:**

|"1234567890qwertyuiop"|

Example of invalid input:

```kusto
print res=gzip_decompress_from_base64_string("x0x0x0")
```

**Output:**
>||

## Next steps

* Create a compressed input string with [gzip_compress_to_base64_string()](gzip-base64-compress.md).
* See also [zlib_decompress_from_base64_string](zlib-base64-decompress.md).
