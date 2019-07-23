---
title: strcat() - Azure Data Explorer | Microsoft Docs
description: This article describes strcat() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 10/23/2018
---
# strcat()

Concatenates between 1 and 64 arguments.

* In case if arguments are not of string type, they will be forcibly converted to string.

**Syntax**

`strcat(`*argument1*,*argument2* [, *argumentN*]`)`

**Arguments**

* *argument1* ... *argumentN* : expressions to be concatenated.

**Returns**

Arguments, concatenated to a single string.

**Examples**
  
   ```kusto
print str = strcat("hello", " ", "world")
```

|str|
|---|
|hello world|