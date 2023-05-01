---
title: replace_string() - Azure Data Explorer
description: Learn how to use the replace_string() function to replace all string matches with another string.
ms.reviewer: alexans
ms.topic: reference
ms.date: 03/12/2023
---
# replace_string()

Replaces all string matches with another string.

> **Deprecated aliases:** replace()

## Syntax

`replace_string(`*text*`,` *lookup*`,` *rewrite*`)`

## Parameters

|Name|Type|Required|Description|
|--|--|--|--|
|*text*|string|&check;|The source string.|
|*lookup*|string|&check;|The string to be replaced.|
|*rewrite*|string|&check;|The replacement string.|

## Returns

*text* after replacing all matches of *lookup* with evaluations of *rewrite*. Matches don't overlap.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA0WLOwqAMBBEe08x3RqwsbD0Cl5Boq4hoEnYREzh4d1CsBjmwxuxwTEqdoknepSIAblwQt884Fo4bNplVK22tDRd58ICn0Gd0jr74NpqzI8Lp8OuvI1fmD9IrQP5rEe6bSbzAkZqfYp8AAAA" target="_blank">Run the query</a>

```kusto
range x from 1 to 5 step 1
| extend str=strcat('Number is ', tostring(x))
| extend replaced=replace_string(str, 'is', 'was')
```

**Output:**

| x    | str | replaced|
|---|---|---|
| 1    | Number is 1.000000  | Number was 1.000000|
| 2    | Number is 2.000000  | Number was 2.000000|
| 3    | Number is 3.000000  | Number was 3.000000|
| 4    | Number is 4.000000  | Number was 4.000000|
| 5    | Number is 5.000000  | Number was 5.000000|

## See also

* For regex matching, see [replace_regex()](replace-regex-function.md).
* For replacing a set of characters, see [translate()](translatefunction.md).
