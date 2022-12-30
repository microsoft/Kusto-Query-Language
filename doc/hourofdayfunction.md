---
title: hourofday() - Azure Data Explorer
description: Learn how to use the hourofday() function to return an integer representing the hour of the given date.
ms.reviewer: alexans
ms.topic: reference
ms.date: 12/28/2022
---
# hourofday()

Returns the integer number representing the hour number of the given date.

## Syntax

`hourofday(`*date*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
|*date*|datetime|&check;|The date for which to return the hour number.|

## Returns

An integer between 0-23 representing the hour number of the day for *date*.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUcjILy2yBRH5aSmJlRopiSWpJZm5qRpGBoamuoZGuoYmCoYWVqYmmpoAPkfViTAAAAA=" target="_blank">Run the query</a>

```kusto
print hour=hourofday(datetime(2015-12-14 18:54))
```

|hour|
|--|
|18|
