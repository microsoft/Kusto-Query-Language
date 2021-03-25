---
title: replace() - Azure Data Explorer | Microsoft Docs
description: This article describes replace() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 10/23/2018
---
# replace()

Replace all regex matches with another string. 

> [!NOTE]
> If you do not need regex matching, use [translate()](translatefunction.md).

## Syntax

`replace(`*regex*`,` *rewrite*`,` *text*`)`

## Arguments

* *regex*: The [regular expression](https://github.com/google/re2/wiki/Syntax) to search *text*. It can contain capture groups in '('parentheses')'. 
* *rewrite*: The replacement regex for any match made by *matchingRegex*. Use `\0` to refer to the whole match, `\1` for the first capture group, `\2` and so on for subsequent capture groups.
* *text*: A string.

## Returns

*text* after replacing all matches of *regex* with evaluations of *rewrite*. Matches do not overlap.

## Example

This statement:

```kusto
range x from 1 to 5 step 1
| extend str=strcat('Number is ', tostring(x))
| extend replaced=replace(@'is (\d+)', @'was: \1', str)
```

Has the following results:

| x    | str | replaced|
|---|---|---|
| 1    | Number is 1.000000  | Number was: 1.000000|
| 2    | Number is 2.000000  | Number was: 2.000000|
| 3    | Number is 3.000000  | Number was: 3.000000|
| 4    | Number is 4.000000  | Number was: 4.000000|
| 5    | Number is 5.000000  | Number was: 5.000000|
 