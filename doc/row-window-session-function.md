---
title: row_window_session() - Azure Data Explorer
description: Learn how to use the row_window_session() function to calculate session start values of a column in a serialized row set.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/18/2023
---
# row_window_session()

Calculates session start values of a column in a [serialized row set](./windowsfunctions.md#serialized-row-set).

## Syntax

`row_window_session` `(` *`Expr`* `,` *`MaxDistanceFromFirst`* `,` *`MaxDistanceBetweenNeighbors`* [`,` *`Restart`*] `)`

* *`Expr`* is an expression whose values are grouped together in sessions.
  Null values produce null values, and the next value starts a new session.
  *`Expr`* must be a scalar expression of type `datetime`.

* *`MaxDistanceFromFirst`* establishes one criterion for starting a new session:
  The maximum distance between the current value of *`Expr`* and the value of
  *`Expr`* at the beginning of the session.
  It's a scalar constant of type `timespan`.

* *`MaxDistanceBetweenNeighbors`* establishes a second criterion for starting a new session:
  The maximum distance from one value of *`Expr`* to the next.
  It's a scalar constant of type `timespan`.

* *Restart* is an optional scalar expression of type `boolean`. If specified,
  every value that evaluates to `true` will immediately restart the session.

## Returns

The function returns the values at the beginning of each session.

The function has the following conceptual calculation model:

1. Goes over the input sequence of *`Expr`* values in order.

1. For every value, determines if it establishes a new session.

1. If it establishes a new session, it emits the value of *`Expr`*. Otherwise, emits the previous value of *`Expr`*.

>[!NOTE]
>The condition that determines if the value represents a new session is a logical OR one of the following conditions:
>
>* If there was no previous session value, or the previous session value was null.
>* If the value of *`Expr`* equals or exceeds the previous session value plus
  *`MaxDistanceFromFirst`*.
>* If the value of *`Expr`* equals or exceeds the previous value of *`Expr`*
  plus *`MaxDistanceBetweenNeighbors`*.
>* If *`Restart`* condition is specified and evaluates to `true`.

## Examples

The following example shows how to calculate the session start values for a table
with two columns: an `ID` column that identifies a sequence, and a `Timestamp`
column that gives the time at which each record occurred. In this example,
a session can't exceed 1 hour, and it continues as long as records are less than
5 minutes apart.

```kusto
datatable (ID:string, Timestamp:datetime) [
    // ...
]
| sort by ID asc, Timestamp asc
| extend SessionStarted = row_window_session(Timestamp, 1h, 5m, ID != prev(ID))
```

## See also

* [scan operator](scan-operator.md)
