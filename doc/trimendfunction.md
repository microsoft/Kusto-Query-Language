---
title:  trim_end()
description: Learn how to use the trim_end() function to remove the trailing match of the specified regular expression.
ms.reviewer: alexans
ms.topic: reference
ms.date: 02/27/2023
---
# trim_end()

Removes trailing match of the specified regular expression.

## Syntax

`trim_end(`*regex*`,` *source*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *regex* | string | &check; | The string or [regular expression](re2.md) to be trimmed from the end of *source*.|
| *source* | string | &check; | The source string from which to trim *regex*.|

## Returns

*source* after trimming matches of *regex* found in the end of *source*.

## Examples

The following statement trims *substring*  from the end of *string_to_trim*.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA8tJLVEoLinKzEuPL8mPBzJyFWwVHJSSgAJ6yfm5Sta8XDkgJaVJEFVAWSWYRAFQAItuVAEdEJGbmhIP1w8SiE/NS9GAG6qDqkUTAKQH9A2VAAAA" target="_blank">Run the query</a>

```kusto
let string_to_trim = @"bing.com";
let substring = ".com";
print string_to_trim = string_to_trim,trimmed_string = trim_end(substring,string_to_trim)
```

**Output**

|string_to_trim|trimmed_string|
|--------------|--------------|
|bing.com      |bing          |

The next statement trims all non-word characters from the end of the string.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUSguKVKwBZHJiSUaSroKCko6SiGpQAElHQclfX0FFSVNXq4ahdSKktS8FIWSoszc3NSUeIguEC8eKKzhoBQdF1Meq62kA5TQBADbLZbMWQAAAA==" target="_blank">Run the query</a>

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
