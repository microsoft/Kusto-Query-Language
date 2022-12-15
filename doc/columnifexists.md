---
title: column_ifexists() - Azure Data Explorer
description: Learn how to use the column_ifexists() function to return a reference to the column if it exists.  
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/27/2022
---
# column_ifexists()

Takes a column name as a string and a default value. Returns a reference to the column if it exists, otherwise - returns the default value.

> **Deprecated aliases:** columnifexists()

## Syntax

`column_ifexists(`*columnName*`,`*defaultValue*)

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *columnName* | string | &check; | The name of the column to check if exists.|
| *defaultValue* | scalar | &check; | The value to use if the column doesn't exist. This value can be any scalar expression. For example, a reference to another column.|

## Returns

If *columnName* exists, then the column it refers to. Otherwise *defaultValue*.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA03NSwrCQBAE0L2nKGajQiCHEE8Q9zImFWyZTIfpTnTh4Y1f3HVBveq6xuHMwrUhK1pN05ARdnEUjylAltC4lmE/M7uFCv5s91r4OtF4dH7dVVLCiZiM3ULNGbvVH8cdY9ELW/+Io/S8ibltfi+r9+T2AZ+ufQiZAAAA" target="_blank">Run the query</a>

```kusto
// There's no column "Capital" in "StormEvents", therefore, the State column will be used instead
StormEvents | project column_ifexists("Capital", State)
```
