---
title:  replace_regex()
description: Learn how to use the replace_regex() function to replace all regex matches with another string.
ms.reviewer: alexans
ms.topic: reference
ms.date: 05/21/2023
---
# replace_regex()

Replaces all regex matches with a specified pattern.

> **Deprecated aliases:** replace()

## Syntax

`replace_regex(`*source*`,`*lookup_regex*`,` *rewrite_pattern*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *source*| string | &check; | The text to search and replace.|
| *lookup_regex*| string | &check; | The [regular expression](re2.md) to search for in *text*. The expression can contain capture groups in parentheses.|
| *rewrite_pattern*| string | &check; | The replacement regex for any match made by *matchingRegex*. Use `\0` to refer to the whole match, `\1` for the first capture group, `\2` and so on for subsequent capture groups.|

## Returns

Returns the *source* after replacing all matches of *lookup_regex* with evaluations of *rewrite_pattern*. Matches do not overlap.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAytKzEtPVahQSCvKz1UwVCjJVzBVKC5JLVAw5KpRSK0oSc1LAfKLbIE4ObFEQ92vNDcptUghs1hBXQeoGiicmZeuUaGpiVBelFqQk5icmmILZcQXpaanVmgAleooOKgDdWrEpGhrqoM45YnFVgoxhuqaAA84qqaHAAAA" target="_blank">Run the query</a>

```kusto
range x from 1 to 5 step 1
| extend str=strcat('Number is ', tostring(x))
| extend replaced=replace_regex(str, @'is (\d+)', @'was: \1')
```

**Output**

| x | str | replaced|
|---|---|---|
| 1    | Number is 1.000000  | Number was: 1.000000|
| 2    | Number is 2.000000  | Number was: 2.000000|
| 3    | Number is 3.000000  | Number was: 3.000000|
| 4    | Number is 4.000000  | Number was: 4.000000|
| 5    | Number is 5.000000  | Number was: 5.000000|

## See also

* To replace a single string, see [replace_string()](replace-string-function.md).
* To replace multiple strings, see [replace_strings()](replace-strings-function.md).
* To replace a set of characters, see [translate()](translatefunction.md).
