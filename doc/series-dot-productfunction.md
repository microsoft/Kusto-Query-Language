---
title: series_dot_product() - Azure Data Explorer
description: This article describes series_dot_product() in Azure Data Explorer.
ms.reviewer: adieldar
ms.topic: reference
ms.date: 03/12/2023
---
# series_dot_product()

Calculates the dot product of two numeric series.

The function `series_dot_product()` takes two numeric series as input, and calculates their [dot product](https://en.wikipedia.org/wiki/Dot_product).

## Syntax

`series_dot_product(`*series1*`,` *series2*`)`

## Alternate syntax

`series_dot_product(`*series*`, `*numeric*`)`

`series_dot_product(`*numeric*`, `*series*`)`

> [!NOTE]
> The alternate syntax shows that one of the two function arguments can be a numerical scalar.
>
> This numerical scalar will be broadcasted to a vector whose length equals the length of the corresponding numeric series.
>
> For example, `series_dot_product([1, 2, 3], 10)` will be treated as `series_dot_product([1, 2, 3], [10, 10, 10])`.

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *series1, series2* | dynamic |  &check; | Input arrays with numeric data, to be element-wise multiplied and then summed into a value of type `real`.

## Returns

Returns a value of type `real` whose value is the sum over the product of each element of *series1* with the corresponding element of *series2*.
In case both series length isn't equal, the longer series will be truncated to the length of the shorter one.
Any non-numeric element of the input series will be ignored.

> [!NOTE]
> If one or both input arrays are empty, the result will be `null`.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA1XMQQoCMQyF4b2neMupdNNx7VlKaeOg4rQkEdri4a2DMLoLH3k/h3UhVFw4P+CgGSeIUhn34QWqSmtCw3m8HDHv1Ae1LxXON4oKcQNLiHcfmEObqm22GwuZ/71bNItq9po4n7L6EUrPqH4bCPGV5NcncZ+YeQNovYwctAAAAA==" target="_blank">Run the query</a>

```kusto
range x from 1 to 3 step 1 
| extend y = x * 2
| extend z = y * 2
| project s1 = pack_array(x,y,z), s2 = pack_array(z, y, x)
| extend s1_dot_product_s2 = series_dot_product(s1, s2)
```

|s1|s2|s1_dot_product_s2|
|---|---|---|
|[1,2,4]|[4,2,1]|12|
|[2,4,8]|[8,4,2]|48|
|[3,6,12]|[12,6,3]|108|

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA03LQQrCMBCF4b2neMtWsklde5YQklGq2ISZEZLi4Z2K0O6Gb97PcbkTGm5cXvDQggtEqdp9+oCa0pLRcbXJGdNOq1H/U+XyoKQQb1hjeobIHPvQXHfr6CDT1u+t+JCLBsvyO2n4vYV4Jjn6IH5Lxy/j/FcsogAAAA==" target="_blank">Run the query</a>

```kusto
range x from 1 to 3 step 1 
| extend y = x * 2
| extend z = y * 2
| project s1 = pack_array(x,y,z), s2 = x
| extend s1_dot_product_s2 = series_dot_product(s1, s2)
```

|s1|s2|s1_dot_product_s2|
|---|---|---|
|[1,2,4]|1|7|
|[2,4,8]|2|28|
|[3,6,12]|3|63|
