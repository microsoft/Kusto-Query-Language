---
title: strcat_delim() - Azure Data Explorer
description: Learn how to use the strcat_delim() function to concatenate between 2 and 64 arguments using a specified delimiter as the first argument.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/31/2023
---
# strcat_delim()

Concatenates between 2 and 64 arguments, using a specified delimiter as the first argument.

## Syntax

`strcat_delim(`*delimiter*, *argument1*, *argument2*[ , *argumentN*]`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *delimiter* | string | &check; | The string to be used as separator in the concatenation.|
| *argument1* ... *argumentN* | scalar | &check; | The expressions to concatenate.|

> [!NOTE]
> If the arguments aren't of string type, they'll be forcibly converted to string.

## Returns

The arguments concatenated to a single string with *delimiter*.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUSguUbAFEkXJiSXxKak5mbka6rrqOgqGOgrqRkBa3RHEKdYEAPL2A8YtAAAA" target="_blank">Run the query</a>

```kusto
print st = strcat_delim('-', 1, '2', 'A', 1s)
```

**Output**

|st|
|---|
|1-2-A-00:00:01|
