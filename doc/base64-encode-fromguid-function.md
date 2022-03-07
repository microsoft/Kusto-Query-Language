---
title: base64_encode_fromguid() - Azure Data Explorer
description: This article describes base64_encode_fromguid() in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference 
ms.date: 08/31/2021
---
# base64_encode_fromguid()

Encodes a [GUID](./scalar-data-types/guid.md) to a base64 string.

## Syntax

`base64_encode_fromguid(`*GUID*`)`

## Arguments

* *GUID*: Input [GUID](./scalar-data-types/guid.md) to be encoded to a base64 string. 

## Returns

Returns a base64 string encoded from a GUID.

* To decode a base64 string to a [GUID](./scalar-data-types/guid.md), see [base64_decode_toguid()](base64-decode-toguid-function.md)
* To create a [GUID](./scalar-data-types/guid.md) from a string, see [toguid()](toguidfunction.md)

## Example

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
print Quine = base64_encode_fromguid(toguid("ae3133f2-6e22-49ae-b06a-16e6a9b212eb"))  
```

|Quine|
|-----|
|8jMxriJurkmwahbmqbIS6w==|

If you try to encode anything that isn't a [GUID](./scalar-data-types/guid.md) as below, an error will be thrown:

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
print Empty = base64_encode_fromguid("abcd1231")
```
