---
title:  percentile(), percentiles()
description: Learn how to use the percentile(), percentiles() functions to calculate estimates for nearest rank percentiles in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 03/16/2023
---
# percentile(), percentiles() (aggregation function)

The `percentile()` function calculates an estimate for the specified [nearest-rank percentile](#nearest-rank-percentile) of the population defined by *expr*.
The accuracy depends on the density of population in the region of the percentile.

`percentiles()` works similarly to `percentile()`. However, `percentiles()` can calculate multiple percentile values at once, which is more efficient than calculating each percentile value separately.

To calculate weighted percentiles, see [percentilesw()](percentilesw-aggfunction.md).

[!INCLUDE [data-explorer-agg-function-summarize-note](../../includes/data-explorer-agg-function-summarize-note.md)]

## Syntax

`percentile(`*expr*`,` *percentile*`)`

`percentiles(`*expr*`,` *percentiles*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
|*expr* | string | &check; | The expression to use for aggregation calculation.|
|*percentile*| int or long | &check;| A constant that specifies the percentile.|
|*percentiles* | int or long | &check; | One or more comma-separated percentiles.|

## Returns

Returns a table with the estimates for *expr* of the specified percentiles in the group, each in a separate column.

> [!NOTE]
> To return the percentiles in a single column, see [Return percentiles as an array](#return-percentiles-as-an-array).

## Examples

### Calculate single percentile

The following example shows the value of `DamageProperty` being larger than 95% of the sample set and smaller than 5% of the sample set.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSspVqhRKC7NzU0syqxKVShILUoGCmbmpGq4JOYmpqcGFOUDxUoqdRQsTTUVkioVgksSS1IBgwSa1j8AAAA=" target="_blank">Run the query</a>

```kusto
StormEvents | summarize percentile(DamageProperty, 95) by State
```

**Output**

The results table shown includes only the first 10 rows.

| State | percentile_DamageProperty_95 |
|--|--|
| ATLANTIC SOUTH | 0 |
| FLORIDA | 40000 |
| GEORGIA | 143333 |
| MISSISSIPPI | 80000 |
| AMERICAN SAMOA | 250000 |
| KENTUCKY | 35000 |
| OHIO | 150000 |
| KANSAS | 51392 |
| MICHIGAN | 49167 |
| ALABAMA | 50000 |

### Calculate multiple percentiles

The following example shows the value of `DamageProperty` simultaneously calculated using 5, 50 (median) and 95.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSspVqhRKC7NzU0syqxKVShILUoGCmbmpBZruCTmJqanBhTlAwVLKnUUTIHIQEfB0lRTIalSIbgksSQVAL6yeg1HAAAA" target="_blank">Run the query</a>

```kusto
StormEvents | summarize percentiles(DamageProperty, 5, 50, 95) by State
```

**Output**

The results table shown includes only the first 10 rows.

| State | percentile_DamageProperty_5 | percentile_DamageProperty_50 | percentile_DamageProperty_95 |
|--|--|--|--|
| ATLANTIC SOUTH | 0 | 0 | 0 |
| FLORIDA | 0 | 0 | 40000 |
| GEORGIA | 0 | 0 | 143333 |
| MISSISSIPPI | 0 | 0 | 80000 |
| AMERICAN SAMOA | 0 | 0 | 250000 |
| KENTUCKY | 0 | 0 | 35000 |
| OHIO | 0 | 2000 | 150000 |
| KANSAS | 0 | 0 | 51392 |
| MICHIGAN | 0 | 0 | 49167 |
| ALABAMA | 0 | 0 | 50000 |
|...|...|

## Return percentiles as an array

Instead of returning the values in individual columns, use the `percentiles_array()` function to return the percentiles in a single column of dynamic array type.

### Syntax

`percentiles_array(`*expr*`,` *percentiles*`)`

### Parameters

| Name | Type | Required | Description |
|--|--|--|--|
|*expr* | string | &check; | The expression to use for aggregation calculation.|
|*percentiles*| int, long, or dynamic | &check;| One or more comma-separated percentiles or a dynamic array of percentiles. Each percentile can be an integer or long value.|

### Returns

Returns an estimate for *expr* of the specified percentiles in the group as a single column of dynamic array type.

### Examples

#### Comma-separated percentiles

Multiple percentiles can be obtained as an array in a single dynamic column, instead of in multiple columns as with [percentiles()](percentiles-aggfunction.md).

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/SampleIoTData?query=H4sIAAAAAAAAAwspSswrTssvyk1NCU7NK84vKnZJLEnk5apRKC7NzU0syqxKVShILUpOzSvJzEktjk8sKkqs1AhLzClN1VEw1VEwAmJTAx0FcyBtaaqpo5BYlg6R1lRIqlSAmOmXmJsKANY0tLFpAAAA" target="_blank">Run the query</a>

```kusto
TransformedSensorsData
| summarize percentiles_array(Value, 5, 25, 50, 75, 95), avg(Value) by SensorName
```

**Output**

The results table displays only the first 10 rows.

|SensorName|percentiles_Value|avg_Value |
|--|--|--|
|sensor-82|["0.048141473520867069","0.24407515500271132","0.48974511106780577","0.74160998970950343","0.94587903204190071"]|0.493950914|
|sensor-130|["0.049200214398937764","0.25735850440187535","0.51206374010048239","0.74182335059053839","0.95210342463616771"]|0.505111463|
|sensor-56|["0.04857779335488676","0.24709868149337144","0.49668762923789589","0.74458470404241883","0.94889104840865857"]|0.497955018|
|sensor-24|["0.051507199150534679","0.24803904945640423","0.50397070213183581","0.75653888126010793","0.9518782718727431"]|0.501084379|
|sensor-47|["0.045991246974755672","0.24644331118208851","0.48089197707088743","0.74475142784472248","0.9518322864959039"]|0.49386228|
|sensor-135|["0.05132897529660399","0.24204987641954018","0.48470113942206461","0.74275730068433621","0.94784079559229406"]|0.494817619|
|sensor-74|["0.048914714739047828","0.25160926036445724","0.49832498850160978","0.75257887767110776","0.94932261924236094"]|0.501627252|
|sensor-173|["0.048333149363009836","0.26084250046756496","0.51288012531934613","0.74964772791583412","0.95156058795294"]|0.505401226|
|sensor-28|["0.048511161184567046","0.2547387968731824","0.50101318228599656","0.75693845702682039","0.95243122486483989"]|0.502066244|
|sensor-34|["0.049980293859462954","0.25094722564949412","0.50914023067384762","0.75571549713447961","0.95176564809278674"]|0.504309494|
|...|...|...|

#### Dynamic array of percentiles

Percentiles for `percentiles_array` can be specified in a dynamic array of integer or floating-point numbers. The array must be constant but doesn't have to be literal.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/SampleIoTData?query=H4sIAAAAAAAAAwspSswrTssvyk1NCU7NK84vKnZJLEnkqlEoLs3NTSzKrEpVKEgtSk7NK8nMSS2OTywqSqzUCEvMKU3VUUipzEvMzUzWiDbVUTACYlMDHQVzIG1pGqupqaOQWJYOUampkFSpADHdLzE3FQAgg+RhcwAAAA==" target="_blank">Run the query</a>

```kusto
TransformedSensorsData
| summarize percentiles_array(Value, dynamic([5, 25, 50, 75, 95])), avg(Value) by SensorName
```

**Output**

The results table displays only the first 10 rows.

|SensorName|percentiles_Value|avg_Value |
|--|--|--|
|sensor-82|["0.048141473520867069","0.24407515500271132","0.48974511106780577","0.74160998970950343","0.94587903204190071"]|0.493950914|
|sensor-130|["0.049200214398937764","0.25735850440187535","0.51206374010048239","0.74182335059053839","0.95210342463616771"]|0.505111463|
|sensor-56|["0.04857779335488676","0.24709868149337144","0.49668762923789589","0.74458470404241883","0.94889104840865857"]|0.497955018|
|sensor-24|["0.051507199150534679","0.24803904945640423","0.50397070213183581","0.75653888126010793","0.9518782718727431"]|0.501084379|
|sensor-47|["0.045991246974755672","0.24644331118208851","0.48089197707088743","0.74475142784472248","0.9518322864959039"]|0.49386228|
|sensor-135|["0.05132897529660399","0.24204987641954018","0.48470113942206461","0.74275730068433621","0.94784079559229406"]|0.494817619|
|sensor-74|["0.048914714739047828","0.25160926036445724","0.49832498850160978","0.75257887767110776","0.94932261924236094"]|0.501627252|
|sensor-173|["0.048333149363009836","0.26084250046756496","0.51288012531934613","0.74964772791583412","0.95156058795294"]|0.505401226|
|sensor-28|["0.048511161184567046","0.2547387968731824","0.50101318228599656","0.75693845702682039","0.95243122486483989"]|0.502066244|
|sensor-34|["0.049980293859462954","0.25094722564949412","0.50914023067384762","0.75571549713447961","0.95176564809278674"]|0.504309494|
|...|...|...|

## Nearest-rank percentile

*P*-th percentile (0 < *P* <= 100) of a list of ordered values, sorted in ascending order, is the smallest value in the list. The *P* percent of the data is less or equal to *P*-th percentile value ([from Wikipedia article on percentiles](https://en.wikipedia.org/wiki/Percentile#The_Nearest_Rank_method)).

Define *0*-th percentiles to be the smallest member of the population.

>[!NOTE]
> Given the approximating nature of the calculation, the actual returned value may not be a member of the population.
> Nearest-rank definition means that *P*=50 does not conform to the [interpolative definition of the median](https://en.wikipedia.org/wiki/Median). When evaluating the significance of this discrepancy for the specific application, the size of the population and an [estimation error](#estimation-error-in-percentiles) should be taken into account.

## Estimation error in percentiles

The percentiles aggregate provides an approximate value using [T-Digest](https://github.com/tdunning/t-digest/blob/master/docs/t-digest-paper/histo.pdf).

>[!NOTE]
>
> * The bounds on the estimation error vary with the value of the requested percentile. The best accuracy is at both ends of the [0..100] scale. Percentiles 0 and 100 are the exact minimum and maximum values of the distribution. The accuracy gradually decreases towards the middle of the scale. It's worst at the median and is capped at 1%.
> * Error bounds are observed on the rank, not on the value. Suppose percentile(X, 50) returned a value of Xm. The estimate guarantees that at least 49% and at most 51% of the values of X are less or equal to Xm. There is no theoretical limit on the difference between Xm and the actual median value of X.
> * The estimation may sometimes result in a precise value but there are no reliable conditions to define when it will be the case.