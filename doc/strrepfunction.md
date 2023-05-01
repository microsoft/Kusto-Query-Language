---
title: strrep() - Azure Data Explorer
description: Learn how to use the strrep() function to repeat the input value.
ms.reviewer: alexans
ms.topic: reference
ms.date: 02/05/2023
---
# strrep()

Replicates a [string](scalar-data-types/string.md) the number of times specified.

## Syntax

`strrep(`*value*`,` *multiplier*`,` [ *delimiter* ]`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *value* | string | &check; | The string to replicate. |
| *multiplier* | int | &check; | The amount of times to replicate the string. Must be a value from 1 to 1024.|
| *delimiter* | string | | The delimeter used to separate the string replications. The default delimiter is an empty string.|

> [!NOTE]
> If *value* or *delimiter* isn't a `string`, they'll be forcibly converted to string.

## Returns

The *value* string repeated the number of times as specified by *multiplier*, concatenated with *delimiter*.

If *multiplier* is more than the maximal allowed value of 1024, the input string will be repeated 1024 times.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUUgrys+NLy4pUrBVAJJFqQUa6o5Ozuo6CkaaOhBJkCq4pKGRsY6xjrqeOky2JDM3FSFtXKxjpKOuoK4JAHPzDvRdAAAA" target="_blank">Run the query</a>

```kusto
print from_str = strrep('ABC', 2), from_int = strrep(123,3,'.'), from_time = strrep(3s,2,' ')
```

**Output**

|from_str|from_int|from_time|
|---|---|---|
|ABCABC|123.123.123|00:00:03 00:00:03|
