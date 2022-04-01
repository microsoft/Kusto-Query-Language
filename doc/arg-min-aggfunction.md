---
title: arg_min() (aggregation function) - Azure Data Explorer
description: This article describes arg_min() (aggregation function) in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 04/12/2019
---
# arg_min() (aggregation function)

Finds a row in the group that minimizes *ExprToMinimize*, and returns the value of *ExprToReturn* (or `*` to return the entire row).

* Can be used only in context of aggregation inside [summarize](summarizeoperator.md)

## Syntax

`arg_min` `(`*ExprToMinimize*`,` *\** | *ExprToReturn*  [`,` ...]`)`

## Arguments

* *ExprToMinimize*: Expression that will be used for aggregation calculation.
* *ExprToReturn*: Expression that will be used for returning the value when *ExprToMinimize* is
  minimum. Expression to return may be a wildcard (*) to return all columns of the input table.
  
## Null handling

When *ExprToMinimize* is null for all rows in a group, one row in the group is picked. Otherwise, rows where *ExprToMinimize* is null are ignored.

## Returns

Finds a row in the group that minimizes *ExprToMinimize*, and returns the value of *ExprToReturn* (or `*` to return the entire row).

## Examples

Show cheapest supplier of each product:

```kusto
Supplies | summarize arg_min(Price, Supplier) by Product
```

Show all the details, not just the supplier name:

```kusto
Supplies | summarize arg_min(Price, *) by Product
```

Find the southernmost city in each continent, with its country:

```kusto
PageViewLog 
| summarize (latitude, min_lat_City, min_lat_country)=arg_min(latitude, City, country) 
    by continent
```

:::image type="content" source="images/arg-min-aggfunction/arg-min.png" alt-text="Table showing the southernmost city with its country as calculated by the query.":::

Null handling example:

```kusto
datatable(Fruit: string, Color: string, Version: int) [
    "Apple", "Red", 1,
    "Apple", "Green", int(null),
    "Banana", "Yellow", int(null),
    "Banana", "Green", int(null),
    "Pear", "Brown", 1,
    "Pear", "Green", 2,
]
| summarize arg_min(Version, *) by Fruit
```

|Fruit|Version|Color|
|---|---|---|
|Apple|1|Red|
|Banana||Yellow|
|Pear|1|Brown|
