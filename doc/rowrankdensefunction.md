---
title:  row_rank_dense()
description: Learn how to use the row_rank_dense() function to return the current row's dense rank in a serialized row set.
ms.reviewer: royo
ms.topic: reference
ms.date: 03/22/2023
---
# row_rank_dense()

Returns the current row's dense rank in a [serialized row set](./windowsfunctions.md#serialized-row-set).

The row rank starts by default at `1` for the first row, and is incremented by `1` whenever the provided *Term* is different than the previous row's *Term*.

## Syntax

`row_rank_dense` `(` *Term* `)`

## Parameters

|Name|Type|Required|Description|
|--|--|--|--|
|*Term*|string|&check;|An expression indicating the value to consider for the rank. The rank is increased whenever the *Term* changes.|
| *restart*| bool | | Indicates when the numbering is to be restarted to the *StartingIndex* value. The default is `false`.|
  
## Returns

Returns the row rank of the current row as a value of type `long`.

## Example

The following query shows how to rank the `Airline` by the number of departures from the SEA `Airport` using dense rank.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA0tJLAHCpJxUBQ3HzKKC/KISq+KSosy8dB0FID8nMy8VzndJLUgsKiktSi22ysnPS9fk5Yrm5VJQUAp2dVTSUVDy8QCSxjooQpFA0tDAAEUw1BFDnROINEIRcvUBqeLliuXlqlEoBjpLIakSyQUKicXJIJnUipLUvBSFoMS8bNui/PL4IiAjPiU1rzhVA6FYEwC7n6cO5QAAAA==" target="_blank">Run the query</a>

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
| extend Rank=row_rank_dense(Departures)
```

**Output**

Airport  | Airline  | Departures  | Rank
---------|----------|-------------|------
SEA      | BA       | 2           | 1
SEA      | LH       | 3           | 2
SEA      | UA       | 3           | 2
SEA      | EL       | 3           | 2
SEA      | LY       | 100         | 3

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA2WQTQuCQBCG7wv+h8mTggetm+DBSOhgl6RDRIi6g0iyyuz2Bf341khtiYGXmYd3hpnhhdJRtghO3FDfkQqlokbUHui6bQRO9Qb7gtSVUIZtJ2rXYieLAdhZEtse2OlW68oz0FFr4PsGPMR/vvWgSwMl6eyKd9mEAgN9GgOLnS32AqmXh/IJ3zuAo6x+t4ZCVoMPHwoFh30hLhF195x0knMUEp3Z7EFPeBt/4sIiGue6b3gIxbszAQAA" target="_blank">Run the query</a>

The following example shows how to rank the `Airline` by the number of departures per each partition. Here, we partition the data by `Airport`: 

```kusto
datatable (Airport:string, Airline:string, Departures:long)
[
  "SEA", "LH", 3,
  "SEA", "LY", 100,
  "SEA", "UA", 3,
  "SEA", "BA", 2,
  "SEA", "EL", 3,
  "AMS", "EL", 1,
  "AMS", "BA", 1
]
| sort by Airport desc, Departures asc
| extend Rank=row_rank_dense(Departures, prev(Airport) != Airport)
```

**Output**

Airport  | Airline  | Departures  | Rank
---------|----------|-------------|------
SEA      | BA       | 2           | 1
SEA      | LH       | 3           | 2
SEA      | UA       | 3           | 2
SEA      | EL       | 3           | 2
SEA      | LY       | 100         | 3
AMS      | EL       | 1           | 1
AMS      | BA       | 1           | 1
