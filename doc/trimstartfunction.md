---
title: trim_start() - Azure Data Explorer | Microsoft Docs
description: This article describes trim_start() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# trim_start()

Removes leading match of the specified regular expression.

## Syntax

`trim_start(`*regex*`,` *text*`)`

## Arguments

* *regex*: String or [regular expression](re2.md) to be trimmed from the beginning of *text*.  
* *text*: A string.

## Returns

*text* after trimming match of *regex* found in the beginning of *text*.

## Example

Statement bellow trims *substring*  from the start of *string_to_trim*:

```kusto
let string_to_trim = @"https://bing.com";
let substring = "https://";
print string_to_trim = string_to_trim,trimmed_string = trim_start(substring,string_to_trim)
```

|string_to_trim|trimmed_string|
|---|---|
|https://bing.com|bing.com|

Next statement trims all non-word characters from the beginning of the string:

```kusto
range x from 1 to 5 step 1
| project str = strcat("-  ","Te st",x,@"// $")
| extend trimmed_str = trim_start(@"[^\w]+",str)
```

|str|trimmed_str|
|---|---|
|-  Te st1// $|Te st1// $|
|-  Te st2// $|Te st2// $|
|-  Te st3// $|Te st3// $|
|-  Te st4// $|Te st4// $|
|-  Te st5// $|Te st5// $|

 