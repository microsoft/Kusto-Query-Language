---
title:  strcat()
description: Learn how to use the strcat() function to concatenate between 1 and 64 arguments.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/31/2023
---
# strcat()

Concatenates between 1 and 64 arguments.

## Syntax

`strcat(`*argument1*`,` *argument2* [`,` *argument3* ... ]`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *argument1* ... *argumentN* | scalar | &check; | The expressions to concatenate.|

> [!NOTE]
> If the arguments aren't of string type, they'll be forcibly converted to string.

## Returns

The arguments concatenated to a single string.

## Examples

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUSguKVKwBZHJiSUaShmpOTn5SjoKSgogojy/KCdFSRMA4dg7JykAAAA=" target="_blank">Run the query</a>
  
```kusto
print str = strcat("hello", " ", "world")
```

**Output**

|str|
|---|
|hello world|
