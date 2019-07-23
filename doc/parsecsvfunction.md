---
title: parse_csv() - Azure Data Explorer | Microsoft Docs
description: This article describes parse_csv() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 05/12/2019
---
# parse_csv()

Splits a given string representing a single record of comma separated values and returns a string array with these values.

```kusto
parse_csv("aaa,bbb,ccc") == ["aaa","bbb","ccc"]
```

**Syntax**

`parse_csv(`*source*`)`

**Arguments**

* *source*: The source string representing a single record of comma separated values. Embedded line feeds, commas and quotes 
are supported. If a multiple record CSV text is provided, only the first record is taken

**Returns**

A string array that contains the split values.

**Examples**

```kusto
print result=parse_csv('aa,"b,b,b",cc,"Escaping quotes: ""Title""","line1\nline2"')
```

|result|
|---|
|[<br>  "aa",<br>  "b,b,b",<br>  "cc",<br>  "Escaping quotes: \"Title\"",<br>  "line1\nline2"<br>]|

CSV payload with multiple records:
```kusto
print result_multi_record=parse_csv('record1,a,b,c\nrecord2,x,y,z')
```

|result_multi_record|
|---|
|[<br>  "record1",<br>  "a",<br>  "b",<br>  "c"<br>]|