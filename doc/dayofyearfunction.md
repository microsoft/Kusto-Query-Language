---
title: dayofyear() - Azure Data Explorer
description: Learn how to use the dayofyear() function to return the day number of the given year.
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/24/2022
---
# dayofyear()

Returns the integer number represents the day number of the given year.

## Syntax

`dayofyear(`*date*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *date* | datetime | &check; | The datetime for which to determine the day number.|

## Returns

The day number of the given year.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/kvc9rf7q4d68qcw5sk2d6f.northeurope/databases/MyDatabase?query=H4sIAAAAAAAAAysoyswrUUhJrMxPq0xNLNJISSxJLcnMTdUwMjA01TU00jU00dQEAOQ8/cIlAAAA" target="_blank">Run the query</a>

```kusto
dayofyear(datetime(2015-12-14))
```

**Output**

|result|
|--|
|348|
