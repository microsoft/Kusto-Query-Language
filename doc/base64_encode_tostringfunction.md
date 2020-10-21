---
title: base64_encode_tostring() - Azure Data Explorer
description: This article describes base64_encode_tostring() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 06/22/2019
---
# base64_encode_tostring()

Encodes a string as base64 string.

## Syntax

`base64_encode_tostring(`*String*`)`

## Arguments

* *String*: Input string to be encoded as base64 string.

## Returns

Returns the string encoded as base64 string.

* To decode base64 strings to UTF-8 strings, see [base64_decode_tostring()](base64_decode_tostringfunction.md)
* To decode base64 strings to an array of long values, see [base64_decode_toarray()](base64_decode_toarrayfunction.md)


## Example

<!-- csl: https://help.kusto.windows.net:443/Samples -->
```kusto
print Quine=base64_encode_tostring("Kusto")
```

|Quine   |
|--------|
|S3VzdG8=|

