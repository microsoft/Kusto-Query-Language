---
title: base64_decode_toarray() - Azure Data Explorer | Microsoft Docs
description: This article describes base64_decode_toarray() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 06/22/2019
---
# base64_decode_toarray()

Decodes a base64 string to an array of long values.

**Syntax**

`base64_decode_toarray(`*String*`)`

**Arguments**

* *String*: Input string to be decoded from base64 to UTF8-8 string.

**Returns**

Returns array of long values ecoded from base64 string.

* For decoding base64 strings to a UTF-8 string see [base64_decode_tostring()](base64_decode_tostringfunction.md)
* For encoding strings to base64 string see [base64_encode_tostring()](base64_encode_tostringfunction.md)

**Example**

```kusto
print Quine=base64_decode_toarray("S3VzdG8=")  
// 'K', 'u', 's', 't', 'o'
```

|Quine|
|-----|
|[75,117,115,116,111]|

Trying to decode a base64 string which was generated from invalid UTF-8 encoding will return null:

```kusto
print Empty=base64_decode_toarray("U3RyaW5n0KHR0tGA0L7Rh9C60LA=")
```

|Empty|
|-----|
||