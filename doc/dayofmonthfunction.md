---
title: dayofmonth() - Azure Data Explorer
description: Learn how to use the dayofmonth() function to return an integer representing the day of the month.
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/24/2022
---
# dayofmonth()

Returns an integer representing the day number of the given datetime.

## Syntax

`dayofmonth(`*date*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *date* | datetime | &check; | The datetime used to extract the day number.|

## Returns

An integer representing the day number of the given datetime.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/kvc9rf7q4d68qcw5sk2d6f.northeurope/databases/MyDatabase?query=H4sIAAAAAAAAAysoyswrUUhJrMxPy83PK8nQSEksSS3JzE3VMDIwNNU1NNI1NNHUBAAj3TtIJgAAAA==" target="_blank">Run the query</a>

```kusto
dayofmonth(datetime(2015-12-14))
```

**Output**

|result|
|--|
|14|
