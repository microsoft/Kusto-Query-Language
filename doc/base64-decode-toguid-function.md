---
title: base64_decode_toguid() - Azure Data Explorer
description: This article describes base64_decode_toguid() in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference 
ms.date: 08/31/2021
---
# base64_decode_toguid()

Decodes a base64 string to a [GUID](./scalar-data-types/guid.md).

## Syntax

`base64_decode_toguid(`*String*`)`

## Arguments

* *String*: Input string to be decoded from base64 to a [GUID](./scalar-data-types/guid.md). 

## Returns

Returns a [GUID](./scalar-data-types/guid.md) decoded from a base64 string.

* To encode a [GUID](./scalar-data-types/guid.md) to a base64 string, see [base64_encode_fromguid()](base64-encode-fromguid-function.md)

## Example

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
print Quine = base64_decode_toguid("JpbpECu8dUy7Pv5gbeJXAA==")  
```

|Quine|
|-----|
|10e99626-bc2b-754c-bb3e-fe606de25700|

If you try to decode an invalid base64 string, "null" will be returned:

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
print Empty = base64_decode_toarray("abcd1231")
```

|Empty|
|-----|
||
