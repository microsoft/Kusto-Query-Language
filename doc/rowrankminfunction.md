---
title: row_rank_min() - Azure Data Explorer
description: Learn how to use the row_rank_min() function to return the current row's minimal rank in a serialized row set.
ms.reviewer: royo
ms.topic: reference
ms.date: 03/22/2023
---
# row_rank_min()

Returns the current row's minimal rank in a [serialized row set](./windowsfunctions.md#serialized-row-set).

The rank is the minimal row number that the current row's *Term* appears in.

## Syntax

`row_rank_min` `(` *Term* `)`

## Parameters

|Name|Type|Required|Description|
|--|--|--|--|
|*Term*|string|&check;|An expression indicating the value to consider for the rank. The rank is the minimal row number for *Term*.|
  
## Returns

Returns the row rank of the current row as a value of type `long`.

## Example

The following query shows how to rank the `Airline` by the number of departures from the SEA `Airport`.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA0tJLAHCpJxUBQ3HzKKC/KISq+KSosy8dB0FID8nMy8VzndJLUgsKiktSi22ysnPS9fk5Yrm5VJQUAp2dVTSUVDy8QCSxjooQpFA0tDAAEUw1BFDnROINEIRcvUBqeLliuXlqlEoBjpLIakSyQUKicXJIJnUipLUvBSFoMS8bNui/PL4IiAjPjczTwOhVBMA9lGyTeMAAAA=" target="_blank">Run the query</a>

```kusto
datatable (Airport:string, Airline:string, Departures:long)
[
  "SEA", "LH", 3,
  "SEA", "LY", 100,
  "SEA", "UA", 3,
  "SEA", "BA", 2,
  "SEA", "EL", 3
]
| sort by Departures asc
| extend Rank=row_rank_min(Departures)
```

**Output**

Airport  | Airline  | Departures  | Rank
---------|----------|-------------|------
SEA      | BA       | 2           | 1
SEA      | LH       | 3           | 2
SEA      | UA       | 3           | 2
SEA      | EL       | 3           | 2
SEA      | LY       | 100         | 5
