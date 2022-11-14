---
title: replace_regex() - Azure Data Explorer
description: This article describes replace_regex() in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/09/2022
---
# replace_regex()

Replaces all regex matches with another string.

> **Deprecated aliases:** replace()

## Syntax

`replace_regex(`*text*`,`*regex*`,` *rewrite*`)`

## Arguments

* *text*: A string.
* *regex*: The [regular expression](https://github.com/google/re2/wiki/Syntax) to search *text*. The expression can contain capture groups in parentheses.
* *rewrite*: The replacement regex for any match made by *matchingRegex*. Use `\0` to refer to the whole match, `\1` for the first capture group, `\2` and so on for subsequent capture groups.

## Returns

*source* after replacing all matches of *regex* with evaluations of *rewrite*. Matches do not overlap.

## See also

* For string matching, see [replace_string()](replace-string-function.md).
* For replacing a set of characters, see [translate()](translatefunction.md).

## Example

```kusto
range x from 1 to 5 step 1
| extend str=strcat('Number is ', tostring(x))
| extend replaced=replace_regex(str, @'is (\d+)', @'was: \1')
```

**Output:**

| x    | str | replaced|
|---|---|---|
| 1    | Number is 1.000000  | Number was: 1.000000|
| 2    | Number is 2.000000  | Number was: 2.000000|
| 3    | Number is 3.000000  | Number was: 3.000000|
| 4    | Number is 4.000000  | Number was: 4.000000|
| 5    | Number is 5.000000  | Number was: 5.000000|
 