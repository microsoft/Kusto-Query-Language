---
title: strcat() - Azure Data Explorer
description: This article describes strcat() in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 02/13/2020
---
# strcat()

Concatenates between 1 and 64 arguments.

* If the arguments aren't of string type, they'll be forcibly converted to string.

## Syntax

`strcat(`*argument1*, *argument2*[, *argumentN*]`)`

## Arguments

* *argument1* ... *argumentN*: Expressions to be concatenated.

## Returns

Arguments, concatenated to a single string.

## Examples
  
```kusto
print str = strcat("hello", " ", "world")
```

**Output**

|str|
|---|
|hello world|
