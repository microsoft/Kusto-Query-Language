---
title: url_encode() - Azure Data Explorer | Microsoft Docs
description: This article describes url_encode() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 10/23/2018
---
# url_encode()

The function converts characters of the input URL into a format that can be transmitted over the Internet. 

Detailed information about URL encoding and decoding can be found [here](https://en.wikipedia.org/wiki/Percent-encoding).

**Syntax**

`url_encode(`*url*`)`

**Arguments**

* *url*: input URL (string).  

**Returns**

URL (string) converted into a format that can be transmitted over the Internet.

**Examples**

```kusto
let url = @'https://www.bing.com/';
print original = url, encoded = url_encode(url)
```

|original|encoded|
|---|---|
|https://www.bing.com/|https%3a%2f%2fwww.bing.com%2f|


 