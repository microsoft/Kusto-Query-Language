---
title: string_size() - Azure Data Explorer | Microsoft Docs
description: This article describes string_size() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 10/23/2018
---
# string_size()

Returns the size, in bytes, of the input string.

**Syntax**

`string_size(`*source*`)`

**Arguments**

* *source*: The source string that will be measured for string size.

**Returns**

Returns the length, in bytes, of the input string.

**Examples**

```kusto
print size = string_size("hello")
```

|size|
|---|
|5|

```kusto
print size = string_size("⒦⒰⒮⒯⒪")
```

|size|
|---|
|15|