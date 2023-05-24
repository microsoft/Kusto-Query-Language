---
title:  new_guid()
description: Learn how to use the new_guid() function to return a random GUID (Globally Unique Identifier).
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/08/2023
---
# new_guid()

Returns a random GUID (Globally Unique Identifier).

## Syntax

`new_guid()`

## Returns

A new value of type [`guid`](scalar-data-types/guid.md).

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUUgvzUyxzUstjwcxNDQBGYdeSRUAAAA=" target="_blank">Run the query</a>

```kusto
print guid=new_guid()
```

**Output**

|guid|
|--|
|2157828f-e871-479a-9d1c-17ffde915095|
