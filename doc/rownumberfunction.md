---
title: row_number() - Azure Data Explorer | Microsoft Docs
description: This article describes row_number() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# row_number()

Returns the current row's index in a [serialized row set](./windowsfunctions.md#serialized-row-set).
The row index starts by default at `1` for the first row, and is incremented by `1` for each additional row.
Optionally, the row index can start at a different value than `1`.
Additionally, the row index may be reset according to some provided predicate.

## Syntax

`row_number` `(` [*StartingIndex* [`,` *Restart*]] `)`

* *StartingIndex* is a constant expression of type `long` indicating the value
  of the row index to start at (or to restart to). The default value is `1`.
* *Restart* is an optional argument of type `bool` that indicates when the
  numbering is to be restarted to the *StartingIndex* value. If not provided,
  the default value of `false` is used.

## Returns

The function returns the row index of the current row as a value of type `long`.

## Examples

The following example returns a table with two columns, the first column (`a`)
with numbers from `10` down to `1`, and the second column (`rn`) with numbers
from `1` up to `10`:

```kusto
range a from 1 to 10 step 1
| sort by a desc
| extend rn=row_number()
```

The following example is similar to the above, only the second column (`rn`)
starts at `7`:

```kusto
range a from 1 to 10 step 1
| sort by a desc
| extend rn=row_number(7)
```

The last example shows how one can partition the data and number the rows
per each partition. Here, we partition the data by `Airport`:

```kusto
datatable (Airport:string, Airline:string, Departures:long)
[
  "TLV", "LH", 1,
  "TLV", "LY", 100,
  "SEA", "LH", 1,
  "SEA", "BA", 2,
  "SEA", "LY", 0
]
| sort by Airport asc, Departures desc
| extend Rank=row_number(1, prev(Airport) != Airport)
```

Running this query produces the following result:

Airport  | Airline  | Departures  | Rank
---------|----------|-------------|------
SEA      | BA       | 2           | 1
SEA      | LH       | 1           | 2
SEA      | LY       | 0           | 3
TLV      | LY       | 100         | 1
TLV      | LH       | 1           | 2