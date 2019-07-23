---
title: strcat_delim() - Azure Data Explorer | Microsoft Docs
description: This article describes strcat_delim() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 10/23/2018
---
# strcat_delim()

Concatenates between 2 and 64 arguments, with delimiter, provided as first argument.

 * In case if arguments are not of string type, they will be forcibly converted to string.

**Syntax**

`strcat_delim(`*delimiter*,*argument1*,*argument2* [, *argumentN*]`)`

**Arguments**

* *delimiter*: string expression, which will be used as separator.
* *argument1* ... *argumentN* : expressions to be concatenated.

**Returns**

Arguments, concatenated to a single string with *delimiter*.

**Examples**

```kusto
print st = strcat_delim('-', 1, '2', 'A', 1s)

```

|st|
|---|
|1-2-A-00:00:01|