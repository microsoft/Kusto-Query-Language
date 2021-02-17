---
title: row_rank() - Azure Data Explorer
description: This article describes the row_rank() function in Azure Data Explorer.

services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: royo
ms.service: data-explorer

ms.topic: reference
ms.date: 01/28/2021
---
# row_rank()

Returns the current row's rank in a [serialized row set](./windowsfunctions.md#serialized-row-set).
The row index starts by default at `1` for the first row, and is incremented by `1` whenever the provided *Term* is different than the previous row's *Term*.

## Syntax

`row_rank` `(` *Term* `)`

* *Term* is is an expression indicating the value to consider for the rank. The rank is increased whenever the *Term* changes.
  
## Returns

Returns the row rank of the current row as a value of type `long`.


## Example

This example shows how to rank the `Airline` by the number of departures from the SEA `Airport`:


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
| extend Rank=row_rank(Departures)
```

Running this query produces the following result:

Airport  | Airline  | Departures  | Rank
---------|----------|-------------|------
SEA      | BA       | 2           | 1
SEA      | LH       | 3           | 2
SEA      | UA       | 3           | 2
SEA      | EL       | 3           | 2
SEA      | LY       | 100         | 3
