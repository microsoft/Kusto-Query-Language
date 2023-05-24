---
title:  top-nested operator
description: Learn how to use the top-nested operator to produce a hierarchical aggregation.
ms.reviewer: alexans
ms.topic: reference
ms.date: 03/12/2023
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
figures, such as country/region, salesperson, and amount sold: what are the top five countries/regions by sales? What are the top three salespeople in each of these countries/regions?"

## Syntax

*T* `|` `top-nested` *TopNestedClause* [`,` `top-nested` *TopNestedClause2*]...

Where *TopNestedClause* has the following syntax:

[ *N* ] `of` [*ExprName* `=`] *Expr* [`with` `others` `=` *ConstExpr*] `by` [*AggName* `=`] *Aggregation* [`asc` | `desc`]

## Parameters

|Name|Type|Required|Description|
|--|--|--|--|
|*T*|string|&check;|The input tabular expression.|
|*N*|long||The number of top values to return for this hierarchy level. If omitted, all distinct values will be returned.|
|*ExprName*|string||If specified, sets the name of the output column corresponding to the values of *Expr*.|
|*Expr*|string|&check;|An expression over the input record indicating which value to return for this hierarchy level. Typically it's a column reference from *T*, or some calculation, such as `bin()`, over such a column.
|*ConstExpr*|string||If specified, for each hierarchy level, 1 record will be added with the value that is the aggregation over all records that didn't "make it to the top".|
|*AggName*|string||If specified, this identifier sets the column name in the output for the value of *Aggregation*.|
|*Aggregation*|string||The aggregation function to apply to all records sharing the same value of *Expr*. The value of this aggregation determines which of the resulting records are "top". For the possible values, see [supported aggregation functions](#supported-aggregation-functions).|
|`asc` or `desc`|string||Controls whether selection is actually from the "bottom" or "top" of the range of aggregated values. The default is `desc`.|

### Supported aggregation functions

The following aggregation functions are supported:
* [sum()](sum-aggfunction.md)
* [count()](count-aggfunction.md)
* [max()](max-aggfunction.md)
* [min()](min-aggfunction.md)
* [dcount()](dcountif-aggfunction.md)
* [avg()](avg-aggfunction.md)
* [percentile()](percentiles-aggfunction.md)
* [percentilew()](percentiles-aggfunction.md)

> [!NOTE]
> Any algebraic combination of the aggregations is also supported.

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

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSsp5qpRKMkv0M1LLS5JTVEwUshPUwguSSxJVUiqVCguzdVwSk3PzPNJLNHU4UJSZwxWl19alExAoSFIoWteik9+cmJJZn4ehmoAdn/LsYsAAAA=" target="_blank">Run the query</a>

```kusto
StormEvents
| top-nested 2 of State       by sum(BeginLat),
  top-nested 3 of Source      by sum(BeginLat),
  top-nested 1 of EndLocation by sum(BeginLat)
```

**Output**

|State|aggregated_State|Source|aggregated_Source|EndLocation|aggregated_EndLocation|
|---|---|---|---|---|---|
|KANSAS|87771.2355000001|Law Enforcement|18744.823|FT SCOTT|264.858|
|KANSAS|87771.2355000001|Public|22855.6206|BUCKLIN|488.2457|
|KANSAS|87771.2355000001|Trained Spotter|21279.7083|SHARON SPGS|388.7404|
|TEXAS|123400.5101|Public|13650.9079|AMARILLO|246.2598|
|TEXAS|123400.5101|Law Enforcement|37228.5966|PERRYTON|289.3178|
|TEXAS|123400.5101|Trained Spotter|13997.7124|CLAUDE|421.44|

Use the option 'with others':

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSsp5qpRKMkv0M1LLS5JTVEwUshPUwguSSxJVSjPLMlQyC/JSC0qVrBVUHLMyVHwB/Eg0sVKCkmVCsWluRpOqemZeT6JJZo6XArIRhmDjcovLUpOJaTUEKTUNS/FJz85sSQzPw+33UBFCjBVWJwAABtuhnPYAAAA" target="_blank">Run the query</a>

```kusto
StormEvents
| top-nested 2 of State with others = "All Other States" by sum(BeginLat),
  top-nested 3 of Source by sum(BeginLat),
  top-nested 1 of EndLocation with others = "All Other End Locations" by sum(BeginLat)
```

**Output**

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

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSsp5qpRKM9ILUpVCC5JLElVUMzMU9BQD3GNcAxW11FQ93b0CwayNIGqiktzcxOLMqtSQSwNp9T0zDyfxBJNAPC7f85LAAAA" target="_blank">Run the query</a>

```kusto
StormEvents
| where State !in ('TEXAS', 'KANSAS')
| summarize sum(BeginLat)
```

**Output**

|sum_BeginLat|
|---|
|1149279.5923|

Request another column (EventType) to the top-nested result.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA43OMQ6CQBQE0J5TTAmJFNjbmNDR4QW+y9dgsvs3u4NK4uEVKC102nmZTE9Lvr1rYC5eoMU6aKYO2MMu6ClUbDnPyJMvj3odQyesdgW+vU3J6V++WXwbhs6ccLTwy2P1y9HTHHXbp484wMuzbKrP+5jspo61PGTt3mt/gc7cAAAA" target="_blank">Run the query</a>

```kusto
StormEvents
| top-nested 2 of State       by sum(BeginLat),
  top-nested 2 of Source      by sum(BeginLat),
  top-nested 1 of EndLocation by sum(BeginLat),
  top-nested   of EventType   by tmp = max(1)
| project-away tmp
```

**Output**

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

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA4WPwYoCMQyG7z5FjjPQAV326kXw5m0eYIjTbC3aVtKM6OLDm7KrVlzYHNP///K1l8RhfaIoeXYFSccuUhay8AHpC3pBIYDtBfIUmhU5HzcorQGdt2yaeKR/s58lu452k0YUn2IpvDbUI7ElLi8/AuYXbgCdY3K6s0PF0IYSArL/ppqdYQkB9zQcfJamejCwmOu0Sqbntp/Ca+Pva4/y3e+upxp0FgWCj9aPngqNMTpq5qrOjJfhQNHJrnbJLXSwUGj5eDh1dD6iIurEm6V5XLgB1CPS3MABAAA=" target="_blank">Run the query</a>

```kusto
StormEvents
| top-nested 2 of State  by sum(BeginLat),    top-nested 2 of Source by sum(BeginLat),    top-nested 4 of EndLocation by  sum(BeginLat)
| order by State , Source, aggregated_EndLocation
| summarize EndLocations = make_list(EndLocation, 10000) , endLocationSums = make_list(aggregated_EndLocation, 10000) by State, Source
| extend indicies = range(0, array_length(EndLocations) - 1, 1)
| mv-expand EndLocations, endLocationSums, indicies
```

**Output**

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

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA22PMQ7CMAxF957CI6BWomVm7NC5XCDEBhUpceRYBSQOTwoFAmL1f0//u1cW147kNRY3UA6Vp6iEwAfo1SjB/grd0bPQeuvMZVEvywJysJlR0d3gMrx+4O/gV0tS6/Fbaf43TGgYIiN1+IE3M5xmB+ETWa3M2bzSVTqzIMkkPB8x0ZbZUKRo7wy+F8H+AAAA" target="_blank">Run the query</a>

```kusto
StormEvents
| top-nested of State by Ignore0=max(1),
  top-nested 2 of StartTime by Ignore1=max(StartTime),
  top-nested of EndTime by Ignore2=max(1),
  top-nested of EpisodeId by Ignore3=max(1)
| project-away Ignore*
| order by State asc, StartTime desc
```

### Retrieve the latest records per identity

If you have a table with an ID column and a timestamp column, you can use the top-nested operator to query the latest two records for each unique value of ID. The latest records are defined by the highest value of timestamp.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA43QzW4CIRAH8DtPMeG0JmyyYLVq4sV48RlMD2PBigpscIya+PCFZpc2JrYNBAL5/fkYjZTa5mgqq2dwomj9hwCyzpwIXTsDjWTyUkCgnYkrvw3RIdngez4AALZmaQS+wIgHLkqqUo0c1Y1MfSCASy6eu/G3U7+51+xUk92wd8vg8aifw5e/oZxkOPoHnGY45uyN3YFCW/tUK6MhbMFq2NxAn527NXOH1yr/BtgPpDIr5S1afumy/5hKmcfql6jqLkqPaWPYm3eq8YL9K0R3fjerT8+D6uvwAQAA" target="_blank">Run the query</a>

```kusto
datatable(id: string, timestamp: datetime, otherInformation: string)   
[
    "Barak", datetime(2015-01-01), "1",
    "Barak", datetime(2016-01-01), "2",
    "Barak", datetime(2017-01-20), "3",
    "Donald", datetime(2017-01-20), "4",
    "Donald", datetime(2017-01-18), "5",
    "Donald", datetime(2017-01-19), "6"
]
| top-nested of id by dummy0=max(1),  
top-nested 2 of timestamp by dummy1=max(timestamp),  
top-nested of otherInformation by dummy2=max(1)
| project-away dummy0, dummy1, dummy2 
```

**Output**

| id | timestamp | otherInformation |
|---|---|---|
| Barak | 2016-01-01T00:00:00Z | 2 |
| Donald | 2017-01-19T00:00:00Z | 6 |
| Barak | 2017-01-20T00:00:00Z | 3 |
| Donald | 2017-01-20T00:00:00Z | 4 |

Here's a step-by-step explanation of the query:

1. The `datatable` creates a test dataset.
1. The first `top-nested` clause returns all distinct values of `id`.
1. The second `top-nested` clause selects the top two records with the highest `timestamp` for each id.
1. The third `top-nested` clause adds the `otherInformation` column for each record.
1. The `project-away` operator removes the dummy columns introduced by the top-nested operator.
