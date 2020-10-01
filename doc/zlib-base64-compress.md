---
title: zlib_compress_to_base64_string - Azure Data Explorer 
description: This article describes the zlib_compress_to_base64_string() command in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: elgevork
ms.service: data-explorer
ms.topic: reference
ms.date: 09/29/2020
---

# zlib_compress_to_base64_string()

Performs zlib compression and encodes the result to base64.

> [!NOTE]
> The only supported windows size is 15.

## Syntax

`zlib_compress_to_base64_string('input_string')`

## Arguments

*input_string*: Input `string`, a string to be compressed and base64 encoded. The function accepts one string argument.

## Returns

* Returns a `string` that represents zlib-compressed and base64-encoded original string. 
* Returns an empty result if compression or encoding failed.

## Example

### Using Kusto

```kusto
print zcomp = zlib_compress_to_base64_string("1234567890qwertyuiop")
```

**Output:** 
|"eAEBFADr/zEyMzQ1Njc4OTBxd2VydHl1aW9wOAkGdw=="|

### Using Python

Compression can be done using other tools, for example Python: 

```python
print(base64.b64encode(zlib.compress(b'<original_string>')))
```

## Next steps

Use [zlib_decompress_from_base64_string()](zlib-base64-decompress.md) to retrieve the original uncompressed string.