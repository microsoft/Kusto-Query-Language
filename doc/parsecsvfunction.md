---
title: parse_csv() - Azure Data Explorer
description: Learn how to use the parse_csv() function to split a given string representing a single record of comma-separated values.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/08/2023
---
# parse_csv()

Splits a given string representing a single record of comma-separated values and returns a string array with these values.

## Syntax

`parse_csv(`*csv_text*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *csv_text* | string | &check; | A single record of comma-separated values. |

> [!NOTE]
>
> * Embedded line feeds, commas, and quotes may be escaped using the double quotation mark ('"').
> * This function doesn't support multiple records per row (only the first record is taken).

## Returns

A string array that contains the split values.

## Examples

### Filter by count of values in record

Count ADX conference sessions with more than three participants.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA3POz0tLLUrNS04NTi0uzszPK+aqUSjPAAopJBYVJVbG56TmpZdkaBQkFhWnxicXl4FYJZnJmQWJeSXFmpoKdgrGQB0pmcUlmXnJJQpaAB0oOCtRAAAA" target="_blank">Run the query</a>

```kusto
ConferenceSessions
| where array_length(parse_csv(participants)) > 3
| distinct *
```

**Output**

|sessionid|...|participants|
|--|--|--|
|CON-PRT157|...|Guy Reginiano, Guy Yehudy, Pankaj Suri, Saeed Copty|
|BRK3099|...|Yoni Leibowitz, Eric Fleischman, Robert Pack, Avner Aharoni|

### Use escaping quotes

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAxXFMQqAMAwF0KuUv1Qhi46CozdwFCSWIIVSa5N6fpU3vFJjNldFW7K5cFXZgz6dZyYc9AGFQFg0cIn5dHe7THRywBotCQBCilmGLf+N8P0LWIqWMVMAAAA=" target="_blank">Run the query</a>

```kusto
print result=parse_csv('aa,"b,b,b",cc,"Escaping quotes: ""Title""","line1\nline2"')
```

**Output**

|result|
|---|
|[<br>  "aa",<br>  "b,b,b",<br>  "cc",<br>  "Escaping quotes: \"Title\"",<br>  "line1\nline2"<br>]|

### CSV with multiple records

Only the first record is taken since this function does not support multiple records. 

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUShKLS7NKYnPBRKZ8UWpyflFKbYFiUXFqfHJxWUa6hARQ51EnSSd5Jg8CNdIp0KnUqdKXRMAyO6RzEMAAAA=" target="_blank">Run the query</a>

```kusto
print result_multi_record=parse_csv('record1,a,b,c\nrecord2,x,y,z')
```

**Output**

|result_multi_record|
|---|
|[<br>  "record1",<br>  "a",<br>  "b",<br>  "c"<br>]|
