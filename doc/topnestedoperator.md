---
title: top-nested operator - Azure Data Explorer
description: This article describes top-nested operator in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# top-nested operator

Produces a hierarchical aggregation and top values selection, where each level is a refinement of the previous one.

```kusto
T | top-nested 3 of Location with others="Others" by sum(MachinesNumber), top-nested 4 of bin(Timestamp,5m) by sum(MachinesNumber)
```

The `top-nested` operator accepts tabular data as input, and one or more aggregation clauses.
The first aggregation clause (left-most) subdivides the input records into partitions, according
to the unique values of some expression over those records. The clause then keeps a certain number of records
that maximize or minimize this expression over the records. The next aggregation clause then
applies a similar function, in a nested fashion. Each following clause is applied to the partition produced
by the previous clause. This process continues for all aggregation clauses.

For example, the `top-nested` operator can be used to answer the following question: "For a table containing sales
figures, such as country, salesperson, and amount sold: what are the top five countries by sales? What are the top three salespeople in each of these countries?"

## Syntax

*T* `|` `top-nested` *TopNestedClause2* [`,` *TopNestedClause2*...]

Where *TopNestedClause* has the following syntax:

[*N*] `of` [*`ExprName`* `=`] *`Expr`* [`with` `others` `=` *`ConstExpr`*] `by` [*`AggName`* `=`] *`Aggregation`* [`asc` | `desc`]

## Arguments

For each *TopNestedClause*:

* *`N`*: A literal of type `long` indicating how many top values to return
  for this hierarchy level.
  If omitted, all distinct values will be returned.

* *`ExprName`*: If specified, sets the name of the output column corresponding
  to the values of *`Expr`*.

* *`Expr`*: An expression over the input record indicating which value to return
  for this hierarchy level.
  Typically it's a column reference for the tabular input (*T*), or some
  calculation (such as `bin()`) over such a column.

* *`ConstExpr`*: If specified, for each hierarchy level, 1 record will be added
  with the value that is the aggregation over all records that didn't
  "make it to the top".

* *`AggName`*: If specified, this identifier sets the column name
  in the output for the value of *Aggregation*.

* *`Aggregation`*: A numeric expression indicating the aggregation to apply
  to all records sharing the same value of *`Expr`*. The value of this aggregation
  determines which of the resulting records are "top".
  
  The following aggregation functions are supported:
   * [sum()](sum-aggfunction.md),
   * [count()](count-aggfunction.md),
   * [max()](max-aggfunction.md),
   * [min()](min-aggfunction.md),
   * [dcount()](dcountif-aggfunction.md),
   * [avg()](avg-aggfunction.md),
   * [percentile()](percentiles-aggfunction.md), and
   * [percentilew()](percentiles-aggfunction.md). Any algebraic combination of the aggregations is also supported.

* `asc` or `desc` (the default) may appear to control whether selection is actually from the "bottom" or "top" of the range of aggregated values.

## Returns

This operator returns a table that has two columns for each aggregation clause:

* One column holds the distinct values of the clause's *`Expr`* calculation (having the
  column name *ExprName* if specified)

* One column holds the result of the *Aggregation*
  calculation (having the column name *AggregationName* if specified)

## Notes

Input columns that aren't specified as *`Expr`* values aren't outputted.
To get all values at a certain level, add an aggregation count that:

* Omits the value of *N*
* Uses the column name as the value of *`Expr`*
* Uses `Ignore=max(1)` as the aggregation, and then ignore (or project-away)
   the column `Ignore`.

The number of records may grow exponentially with the number of aggregation clauses
((N1+1) \* (N2+1) \* ...). Record growth is even faster if no *N* limit is specified. Take into account that this operator may consume a considerable amount of resources.

If the distribution of the aggregation is considerably non-uniform,
limit the number of distinct values to return (by using *N*) and use the
`with others=` *ConstExpr* option to get an indication for the "weight" of all other
cases.

## Examples

<!-- csl: https://help.kusto.windows.net:443/Samples -->
```kusto
StormEvents
| top-nested 2 of State       by sum(BeginLat),
  top-nested 3 of Source      by sum(BeginLat),
  top-nested 1 of EndLocation by sum(BeginLat)
```

|State|aggregated_State|Source|aggregated_Source|EndLocation|aggregated_EndLocation|
|---|---|---|---|---|---|
|KANSAS|87771.2355000001|Law Enforcement|18744.823|FT SCOTT|264.858|
|KANSAS|87771.2355000001|Public|22855.6206|BUCKLIN|488.2457|
|KANSAS|87771.2355000001|Trained Spotter|21279.7083|SHARON SPGS|388.7404|
|TEXAS|123400.5101|Public|13650.9079|AMARILLO|246.2598|
|TEXAS|123400.5101|Law Enforcement|37228.5966|PERRYTON|289.3178|
|TEXAS|123400.5101|Trained Spotter|13997.7124|CLAUDE|421.44|

Use the option 'with others':

<!-- csl: https://help.kusto.windows.net:443/Samples -->
```kusto
StormEvents
| top-nested 2 of State with others = "All Other States" by sum(BeginLat),
  top-nested 3 of Source by sum(BeginLat),
  top-nested 1 of EndLocation with others = "All Other End Locations" by sum(BeginLat)
```

|State|aggregated_State|Source|aggregated_Source|EndLocation|aggregated_EndLocation|
|---|---|---|---|---|---|
|KANSAS|87771.2355000001|Law Enforcement|18744.823|FT SCOTT|264.858|
|KANSAS|87771.2355000001|Public|22855.6206|BUCKLIN|488.2457|
|KANSAS|87771.2355000001|Trained Spotter|21279.7083|SHARON SPGS|388.7404|
|TEXAS|123400.5101|Public|13650.9079|AMARILLO|246.2598|
|TEXAS|123400.5101|Law Enforcement|37228.5966|PERRYTON|289.3178|
|TEXAS|123400.5101|Trained Spotter|13997.7124|CLAUDE|421.44|
|KANSAS|87771.2355000001|Law Enforcement|18744.823|All Other End Locations|18479.965|
|KANSAS|87771.2355000001|Public|22855.6206|All Other End Locations|22367.3749|
|KANSAS|87771.2355000001|Trained Spotter|21279.7083|All Other End Locations|20890.9679|
|TEXAS|123400.5101|Public|13650.9079|All Other End Locations|13404.6481|
|TEXAS|123400.5101|Law Enforcement|37228.5966|All Other End Locations|36939.2788|
|TEXAS|123400.5101|Trained Spotter|13997.7124|All Other End Locations|13576.2724|
|KANSAS|87771.2355000001|||All Other End Locations|24891.0836|
|TEXAS|123400.5101|||All Other End Locations|58523.2932000001|
|All Other States|1149279.5923|||All Other End Locations|1149279.5923|

The following query shows the same results for the first level used in the example above.

<!-- csl: https://help.kusto.windows.net:443/Samples -->
```kusto
 StormEvents
 | where State !in ('TEXAS', 'KANSAS')
 | summarize sum(BeginLat)
```

|sum_BeginLat|
|---|
|1149279.5923|

Request another column (EventType) to the top-nested result.

<!-- csl: https://help.kusto.windows.net:443/Samples -->
```kusto
StormEvents
| top-nested 2 of State       by sum(BeginLat),
  top-nested 2 of Source      by sum(BeginLat),
  top-nested 1 of EndLocation by sum(BeginLat),
  top-nested   of EventType   by tmp = max(1)
| project-away tmp
```

|State|aggregated_State|Source|aggregated_Source|EndLocation|aggregated_EndLocation|EventType|
|---|---|---|---|---|---|---|
|KANSAS|87771.2355000001|Trained Spotter|21279.7083|SHARON SPGS|388.7404|Thunderstorm Wind|
|KANSAS|87771.2355000001|Trained Spotter|21279.7083|SHARON SPGS|388.7404|Hail|
|KANSAS|87771.2355000001|Trained Spotter|21279.7083|SHARON SPGS|388.7404|Tornado|
|KANSAS|87771.2355000001|Public|22855.6206|BUCKLIN|488.2457|Hail|
|KANSAS|87771.2355000001|Public|22855.6206|BUCKLIN|488.2457|Thunderstorm Wind|
|KANSAS|87771.2355000001|Public|22855.6206|BUCKLIN|488.2457|Flood|
|TEXAS|123400.5101|Trained Spotter|13997.7124|CLAUDE|421.44|Hail|
|TEXAS|123400.5101|Law Enforcement|37228.5966|PERRYTON|289.3178|Hail|
|TEXAS|123400.5101|Law Enforcement|37228.5966|PERRYTON|289.3178|Flood|
|TEXAS|123400.5101|Law Enforcement|37228.5966|PERRYTON|289.3178|Flash Flood|

Give an index sort order for each value in this level (per group) to sort the result by the last nested level (in this example by EndLocation):

<!-- csl: https://help.kusto.windows.net:443/Samples -->
```kusto
StormEvents
| top-nested 2 of State  by sum(BeginLat),    top-nested 2 of Source by sum(BeginLat),    top-nested 4 of EndLocation by  sum(BeginLat)
| order by State , Source, aggregated_EndLocation
| summarize EndLocations = make_list(EndLocation, 10000) , endLocationSums = make_list(aggregated_EndLocation, 10000) by State, Source
| extend indicies = range(0, array_length(EndLocations) - 1, 1)
| mv-expand EndLocations, endLocationSums, indicies
```

|State|Source|EndLocations|endLocationSums|indices|
|---|---|---|---|---|
|TEXAS|Trained Spotter|CLAUDE|421.44|0|
|TEXAS|Trained Spotter|AMARILLO|316.8892|1|
|TEXAS|Trained Spotter|DALHART|252.6186|2|
|TEXAS|Trained Spotter|PERRYTON|216.7826|3|
|TEXAS|Law Enforcement|PERRYTON|289.3178|0|
|TEXAS|Law Enforcement|LEAKEY|267.9825|1|
|TEXAS|Law Enforcement|BRACKETTVILLE|264.3483|2|
|TEXAS|Law Enforcement|GILMER|261.9068|3|
|KANSAS|Trained Spotter|SHARON SPGS|388.7404|0|
|KANSAS|Trained Spotter|ATWOOD|358.6136|1|
|KANSAS|Trained Spotter|LENORA|317.0718|2|
|KANSAS|Trained Spotter|SCOTT CITY|307.84|3|
|KANSAS|Public|BUCKLIN|488.2457|0|
|KANSAS|Public|ASHLAND|446.4218|1|
|KANSAS|Public|PROTECTION|446.11|2|
|KANSAS|Public|MEADE STATE PARK|371.1|3|

The following example returns the two most-recent events
for each US state, with some information per event.
Note the use of the `max(1)` (which is then projected away)
for columns which just require propagation through the operator
without any selection logic.

<!-- csl: https://help.kusto.windows.net:443/Samples -->
```kusto
StormEvents
| top-nested of State by Ignore0=max(1),
  top-nested 2 of StartTime by Ignore1=max(StartTime),
  top-nested of EndTime by Ignore2=max(1),
  top-nested of EpisodeId by Ignore3=max(1)
| project-away Ignore*
| order by State asc, StartTime desc
```
