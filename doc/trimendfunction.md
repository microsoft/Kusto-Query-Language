---
title: trim_end() - Azure Data Explorer
description: This article describes trim_end() in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 02/13/2020
---
# trim_end()

Removes trailing match of the specified regular expression.

## Syntax

`trim_end(`*regex*`,` *source*`)`

## Arguments

* *regex*: String or [regular expression](re2.md) to be trimmed from the end of *source*.  
* *source*: A string.

## Returns

*source* after trimming matches of *regex* found in the end of *source*.

## Example

Statement bellow trims *substring*  from the end of *string_to_trim*:

```kusto
let string_to_trim = @"bing.com";
let substring = ".com";
print string_to_trim = string_to_trim,trimmed_string = trim_end(substring,string_to_trim)
```

**Output**

|string_to_trim|trimmed_string|
|--------------|--------------|
|bing.com      |bing          |

Next statement trims all non-word characters from the end of the string:

```kusto
print str = strcat("-  ","Te st",x,@"// $")
| extend trimmed_str = trim_end(@"[^\w]+",str)
```

**Output**

|str          |trimmed_str|
|-------------|-----------|
|-  Te st1// $|-  Te st1  |
|-  Te st2// $|-  Te st2  |
|-  Te st3// $|-  Te st3  |
|-  Te st4// $|-  Te st4  |
|-  Te st5// $|-  Te st5  |