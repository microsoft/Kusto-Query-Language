---
title: series_periods_validate() - Azure Data Explorer
description: Learn how to use the series_periods_validate() function to check whether a time series contains periodic patterns of given lengths.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/30/2023
---
# series_periods_validate()

Checks whether a time series contains periodic patterns of given lengths.  

Often a metric measuring the traffic of an application is characterized by a weekly or daily period. This period can be confirmed by running `series_periods_validate()` that checks for a weekly and daily period.

## Syntax

`series_periods_validate(`*series*`,` *period1* [ `,` *period2* `,` . . . ] `)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *series* | dynamic | &check; | An array of numeric values, typically the resulting output of [make-series](make-seriesoperator.md) or [make_list](makelist-aggfunction.md) operators.|
| *period1*, *period2*, etc.| real | &check; | The periods to validate in units of the bin size. For example, if the series is in 1h bins, a weekly period is 168 bins. At least one period is required.|

> [!IMPORTANT]
>
> * The minimal value for each of the *period* parameters is **4** and the maximal is half of the length of the input series. For a *period* argument outside these bounds, the output score will be **0**.
> * The input time series must be regular, that is, aggregated in constant bins, and is always the case if it has been created using [make-series](make-seriesoperator.md). Otherwise, the output is meaningless.
> * The function accepts up to 16 periods to validate.
> 

## Returns

The function outputs a table with two columns:

* *periods*: A dynamic array that contains the periods to validate as supplied in the input.
* *scores*: A dynamic array that contains a score between 0 and 1. The score shows the significance of a period in its respective position in the *periods* array.

## Example

The following query embeds a snapshot of a month of an application’s traffic, aggregated twice a day (the bin size is 12 hours).

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAz2OvW7DMAyE9z4FRwfwINrW35AnKYJCcITEQaIGgocKyMP3LnY7kDiS95F81qWs0o7nVtJjmbvPYHrRMfYSPISicqEXOyGgrSJGOpCCg5jQibRaVHFiB3C0FAA8Rk53GNPJ77AfyAx/DG6oIeTYN0pltifcuN20dluhfEINvwiRimxgqUze/isMIn+An2vf8b7BZMgGrlKWgXcH3TFVHvLmdPh4ybN+3/K8ys+xpnLJHUyp1tS+7rlc1mvXDvAimgjcNZdzrnJfSp6vqa6/5GXgcmIBAAA=" target="_blank">Run the query</a>

```kusto
print y=dynamic([80, 139, 87, 110, 68, 54, 50, 51, 53, 133, 86, 141, 97, 156, 94, 149, 95, 140, 77, 61, 50, 54, 47, 133, 72, 152, 94, 148, 105, 162, 101, 160, 87, 63, 53, 55, 54, 151, 103, 189, 108, 183, 113, 175, 113, 178, 90, 71, 62, 62, 65, 165, 109, 181, 115, 182, 121, 178, 114, 170])
| project x=range(1, array_length(y), 1), y  
| render linechart
```

:::image type="content" source="images/series-periods/series-periods.png" alt-text="Series periods.":::

If you run `series_periods_validate()` on this series to validate a weekly period (14 points long) it results in a high score, and with a **0** score when you validate a five-day period (10 points long).

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA01Qy26DMBC89yt8BAlFXsCvQ76kqpAVrJQqJcigqJb68Z0JtOphl9ndmZ01S57mTZXzWOb4OV2qV68bJV1olHcAgsr6RpkeAWwE0ZGB5C1Aj04g1aAKPTsQB0MAgcPIyiHGtHeH2LXUtL8aeIimyLKvhUjvR9hu9zRmXyE8QjSv8IGIWs9SmJz5QxgE3gA+1z7j6cGkqfVcJSw9fVs5ZCI0cvqtfvlWS75/pMumvs45ztdUgRRzjmW4pfm6vVelBhdRlPrHXlOe0jos+NzHdXjE2zTGLVWFzz3xL+uTrn8ATfvv2oABAAA=" target="_blank">Run the query</a>

```kusto
print y=dynamic([80, 139, 87, 110, 68, 54, 50, 51, 53, 133, 86, 141, 97, 156, 94, 149, 95, 140, 77, 61, 50, 54, 47, 133, 72, 152, 94, 148, 105, 162, 101, 160, 87, 63, 53, 55, 54, 151, 103, 189, 108, 183, 113, 175, 113, 178, 90, 71, 62, 62, 65, 165, 109, 181, 115, 182, 121, 178, 114, 170])
| project x=range(1, array_length(y), 1), y  
| project series_periods_validate(y, 14.0, 10.0)
```

**Output**

| series\_periods\_validate\_y\_periods  | series\_periods\_validate\_y\_scores |
|-------------|-------------------|
| [14.0, 10.0] | [0.84, 0.0]  |
