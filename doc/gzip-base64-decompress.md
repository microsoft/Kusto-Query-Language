---
title: gzip_decompress_from_base64_string() - Azure Data Explorer 
description: Learn how to use the gzip_decompress_from_base64_string() function to decode an input string from base64 and perform a gzip-decompression.
ms.reviewer: elgevork
ms.topic: reference
ms.date: 12/18/2022
---
# gzip_decompress_from_base64_string()

Decodes the input string from base64 and performs gzip decompression.

## Syntax

`gzip_decompress_from_base64_string(`*string*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *string* | string | &check; | The value that was compressed with gzip and then base64-encoded. The function accepts only one argument.|

> [!NOTE]
>
> * This function checks mandatory gzip header fields (ID1, ID2, and CM) and returns an empty output if any of these fields have incorrect values.
> * The FLG byte is expected to be zero.
> * Optional header fields are not supported.

## Returns

* Returns a `string` that represents the original string.
* Returns an empty result if decompression or decoding failed.
  * For example, invalid gzip-compressed and base 64-encoded strings will return an empty output.

## Examples

### Valid input

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUShKLbZNr8osiE9JTc7PLQByi+PTivJz45MSi1PNTOKLS4DK0jWUPEyKPR2hQL/cNdTRv0zfN8Szys8l1MivKt3U18PNOCrCyyA1IqwgydjRLNc8zcgNrNrWVkkTALBjGHhsAAAA" target="_blank">Run the query</a>

```kusto
print res=gzip_decompress_from_base64_string("H4sIAAAAAAAA/wEUAOv/MTIzNDU2Nzg5MHF3ZXJ0eXVpb3A6m7f2FAAAAA==")
```

|res|
|--|
|"1234567890qwertyuiop"|

### Invalid input

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUShKLbZNr8osiE9JTc7PLQByi+PTivJz45MSi1PNTOKLS4DK0jWUKgxAUEkTAPzuZ/E2AAAA" target="_blank">Run the query</a>

```kusto
print res=gzip_decompress_from_base64_string("x0x0x0")
```

|res|
|--|
||

## See also

* [gzip_compress_to_base64_string()](gzip-base64-compress.md)
* [zlib_decompress_from_base64_string](zlib-base64-decompress.md)
