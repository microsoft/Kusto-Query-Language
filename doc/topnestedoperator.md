---
title: top-nested operator - Azure Data Explorer | Microsoft Docs
description: This article describes top-nested operator in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 02/20/2019
---
# top-nested operator

Produces hierarchical top results, where each level is a drill-down based on previous level values. 

```kusto
T | top-nested 3 of Location with others="Others" by sum(MachinesNumber), top-nested 4 of bin(Timestamp,5m) by sum(MachinesNumber)
```

It is useful for dashboard visualization scenarios, or when it is necessary to answer to a question that 
sounds like: "find what are top-N values of K1 (using some aggregation); for each of them, find what are the top-M values of K2 (using another aggregation); ..."

**Syntax**

*T* `|` `top-nested` [*N1*] `of` *Expression1* [`with others=` *ConstExpr1*] `by` [*AggName1* `=`] *Aggregation1* [`asc` | `desc`] [`,`...]

**Arguments**

for each top-nested rule:
* *N1*: The number of top values to return for each hierarchy level. Optional (if omitted, all distinct values will be returned).
* *Expression1*: An expression by which to select the top values. Typically it's either a column name in *T*, or some binning operation (e.g., `bin()`) on such a column. 
* *ConstExpr1*: If specified, then for the applicable nesting level, an additional row will be appended that holds the aggregated result for the other values that are not included in the top values.
* *Aggregation1*: A call to an aggregation function which may be one of:
  [sum()](sum-aggfunction.md),
  [count()](count-aggfunction.md),
  [max()](max-aggfunction.md),
  [min()](min-aggfunction.md),
  [dcount()](dcountif-aggfunction.md),
  [avg()](avg-aggfunction.md),
  [percentile()](percentiles-aggfunction.md),
  [percentilew()](percentiles-aggfunction.md),
  or any algebric combination of these aggregations.
* `asc` or `desc` (the default) may appear to control whether selection is actually from the "bottom" or "top" of the range.

**Returns**

Hierarchial table which includes input columns and for each one a new column is produced to include result of the Aggregation for the same level for each element.
The columns are arranged in the same order of the input columns and the new produced column will be close to the aggregated column. 
Each record has a hierarchial structure where each value is selected after applying all the previous top-nested rules on all the previous levels and then applying the current level's rule on this output.
This means that the top n values for level i are calculated for each value in level i - 1.
 
**Tips**

* Use columns renaming in for *Aggregation* results: T | top-nested 3 of Location by MachinesNumberForLocation = sum(MachinesNumber) ... .

* The number of records returned might be quite large; up to (*N1*+1) * (*N2*+1) * ... * (*Nm*+1) (where m is the number of the levels and *Ni* is the top count for level i).

* The Aggregation must receive a numeric column with aggregation function which is one of the mentioned above.

* Use the `with others=` option in order to get the aggregated value of all other values that was not top N values in some level.

* If you are not interested in getting `with others=` for some level, null values will be appended (for the aggreagated column and the level key, see example below).


* It is possible to return additional columns for the selected top-nested candidates by appending additional top-nested statements like these (see examples below):

```kusto
top-nested 2 of ...., ..., ..., top-nested of <additionalRequiredColumn1> by max(1), top-nested of <additionalRequiredColumn2> by max(1)
```

**Example**

```kusto
StormEvents
| top-nested 2 of State by sum(BeginLat),
  top-nested 3 of Source by sum(BeginLat),
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


* With others example:

```kusto
StormEvents
| top-nested 2 of State with others = "All Other States" by sum(BeginLat),
  top-nested 3 of Source by sum(BeginLat),
  top-nested 1 of EndLocation with others = "All Other End Locations" by  sum(BeginLat)


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


The following query shows the same results for the first level used in the example above:

```kusto
 StormEvents
 | where State !in ('TEXAS', 'KANSAS')
 | summarize sum(BeginLat)
```

|sum_BeginLat|
|---|
|1149279.5923|


Requesting another column (EventType) to the top-nested result: 

```kusto
StormEvents
| top-nested 2 of State by sum(BeginLat),    top-nested 2 of Source by sum(BeginLat),    top-nested 1 of EndLocation by sum(BeginLat), top-nested of EventType  by tmp = max(1)
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

In order to sort the result by the last nested level (in this example by EndLocation) and give an index sort order for each value in this level (per group) :

```kusto
StormEvents
| top-nested 2 of State  by sum(BeginLat),    top-nested 2 of Source by sum(BeginLat),    top-nested 4 of EndLocation by  sum(BeginLat)
| order by State , Source, aggregated_EndLocation
| summarize EndLocations = make_list(EndLocation, 10000) , endLocationSums = make_list(aggregated_EndLocation, 10000) by State, Source
| extend indicies = range(0, array_length(EndLocations) - 1, 1)
| mv-expand EndLocations, endLocationSums, indicies
```

|State|Source|EndLocations|endLocationSums|indicies|
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