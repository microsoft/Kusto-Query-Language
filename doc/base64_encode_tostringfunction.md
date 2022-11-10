---
title: base64_encode_tostring() - Azure Data Explorer
description: Learn how to use the base64_encode_tostring() function to return an input string as a base64 string. 
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/07/2022
---
# base64_encode_tostring()

Encodes a string as base64 string.

> **Deprecated aliases:** base64_encodestring()

## Syntax

`base64_encode_tostring(`*String*`)`

## Arguments

* *String*: Input string to be encoded as base64 string.

## Returns

Returns the string encoded as base64 string.

* To decode base64 strings to UTF-8 strings, see [base64_decode_tostring()](base64_decode_tostringfunction.md)
* To decode base64 strings to an array of long values, see [base64_decode_toarray()](base64_decode_toarrayfunction.md)

## Example

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
print Quine=base64_encode_tostring("Kusto")
```

|Quine   |
|--------|
|S3VzdG8=|
