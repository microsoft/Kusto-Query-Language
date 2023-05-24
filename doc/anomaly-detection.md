---
title: Time series anomaly detection & forecasting
description: Learn how to analyze time series data for anomaly detection and forecasting.
ms.reviewer: adieldar
ms.topic: how-to
ms.date: 05/01/2023
---

# Anomaly detection and forecasting

Cloud services and IoT devices generate telemetry data that can be used to gain insights such as monitoring service health, physical production processes, and usage trends. Performing time series analysis is one way to identify deviations in the pattern of these metrics compared to their typical baseline pattern.

Kusto Query Language (KQL) contains native support for creation, manipulation, and analysis of multiple time series. With KQL, you can create and analyze thousands of time series in seconds, enabling near real time monitoring solutions and workflows.

This article details time series anomaly detection and forecasting capabilities of KQL. The applicable time series functions are based on a robust well-known decomposition model, where each original time series is decomposed into seasonal, trend, and residual components. Anomalies are detected by outliers on the residual component, while forecasting is done by extrapolating the seasonal and trend components. The KQL implementation significantly enhances the basic decomposition model by automatic seasonality detection, robust outlier analysis, and vectorized implementation to process thousands of time series in seconds.

## Prerequisites

* A Microsoft account or an Azure Active Directory user identity. An Azure subscription isn't required.
* Read [Time series analysis](time-series-analysis.md) for an overview of time series capabilities.

## Time series decomposition model

The KQL native implementation for time series prediction and anomaly detection uses a well-known decomposition model. This model is applied to time series of metrics expected to manifest periodic and trend behavior, such as service traffic, component heartbeats, and IoT periodic measurements to forecast future metric values and detect anomalous ones. The assumption of this regression process is that other than the previously known seasonal and trend behavior, the time series is randomly distributed. You can then forecast future metric values from the seasonal and trend components, collectively named baseline, and ignore the residual part. You can also detect anomalous values based on outlier analysis using only the residual portion.
To create a decomposition model, use the function [`series_decompose()`](series-decomposefunction.md). The `series_decompose()` function takes a set of time series and automatically decomposes each time series to its seasonal, trend, residual, and baseline components. 

For example, you can decompose traffic of an internal web service by using the following query:

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA3WQ3WrDMAyF7/sUukvCnDXJGIOVPEULuwxqoixm/gm2+jf28JObFjbYrmyho3M+yRCD1a5jaGFAJtaW8qaqX8qqLqvnYrMySYHnvxRNWT1B07xW1U03JFEzbVYDWd9Z/KAuUtAUm9UXpLJcSnAH2+LxPZe3AO9gJ6ZbRjvDGLy9EbG/BUemOXnvLxD1AOJ1mijQtWhbyHbbOgOA9RogkqGeAaXn3g1BooVb6OiDNHpD6CjAUccDGv2JrL0TSzozuQHyPYqHdqRkDKN3aBRwkJaCQJIoQ4VsuXh2A/Xezj5SWkVBWSvI0vSoOSsWpLtEpyDwY4KTW8nnJ5ws+2+eAhSyOxjkd+HDVVcIfHplp2TYTxgYTpqnnDUbarM32gPO86PY4jjqfmGw3vGkftNlCi5xNprbWW5kYvENQQnqDh8CAAA=" target="_blank">Run the query</a>

```kusto
let min_t = datetime(2017-01-05);
let max_t = datetime(2017-02-03 22:00);
let dt = 2h;
demo_make_series2
| make-series num=avg(num) on TimeStamp from min_t to max_t step dt by sid 
| where sid == 'TS1'   //  select a single time series for a cleaner visualization
| extend (baseline, seasonal, trend, residual) = series_decompose(num, -1, 'linefit')  //  decomposition of a set of time series to seasonal, trend, residual, and baseline (seasonal+trend)
| render timechart with(title='Web app. traffic of a month, decomposition', ysplit=panels)
```

![Time series decomposition.](../../media/anomaly-detection/series-decompose-timechart.png)

* The original time series is labeled **num** (in red). 
* The process starts by auto detection of the seasonality by using the function [`series_periods_detect()`](series-periods-detectfunction.md) and extracts the **seasonal** pattern (in purple).
* The seasonal pattern is subtracted from the original time series and a linear regression is run using the function [`series_fit_line()`](series-fit-linefunction.md) to find the **trend** component (in light blue).
* The function subtracts the trend and the remainder is the **residual** component (in green).
* Finally, the function adds the seasonal and trend components to generate the **baseline** (in blue).

## Time series anomaly detection

The function [`series_decompose_anomalies()`](series-decompose-anomaliesfunction.md) finds anomalous points on a set of time series. This function calls `series_decompose()` to build the decomposition model and then runs [`series_outliers()`](series-outliersfunction.md) on the residual component. `series_outliers()` calculates anomaly scores for each point of the residual component using Tukey's fence test. Anomaly scores above 1.5 or below -1.5 indicate a mild anomaly rise or decline respectively. Anomaly scores above 3.0 or below -3.0 indicate a strong anomaly.

The following query allows you to detect anomalies in internal web service traffic:

> [!div class="nextstepaction"]
> > [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA3WR3W7CMAyF73mKI25KpRbaTmjSUJ8CpF1WoXVptPxUifmb9vBLoGO7GFeR7ePv2I4ihpamYdToBBNLTYuqKF/zosyLdbqZqagQl/8UVV68oKreimLSdVFUDZtZR9o2WnxQ48lJ8tXsCzHM7yHMUdfidFiEN4U12AXoloUe0Turp4nYTsaeaYzs/RVedgis80CObkFdI9ltywTAagV4UtQyRKiZgyLEaTGZ9taFQqtIGHI4SX8USn4KltYEJF2YTIeFMFaHPPkMvrWOMuxFoEpDaVjujmo6aq0erafmIY+7ZCiX6wx5mSGJHb3kJA1sF8jB8q69toNwjLPkYfGTseqoja//eLNkRXXyTnuIcVyCneh72cL2YQdtDQ8ZHvIkDcsfPWH+3AvPvObx0FMXD/RLhfDYW9VhtNKwj/8U69M1b2S//AbRUQMWQQIAAA==" target="_blank">Run the query</a>

```kusto
let min_t = datetime(2017-01-05);
let max_t = datetime(2017-02-03 22:00);
let dt = 2h;
demo_make_series2
| make-series num=avg(num) on TimeStamp from min_t to max_t step dt by sid 
| where sid == 'TS1'   //  select a single time series for a cleaner visualization
| extend (anomalies, score, baseline) = series_decompose_anomalies(num, 1.5, -1, 'linefit')
| render anomalychart with(anomalycolumns=anomalies, title='Web app. traffic of a month, anomalies') //use "| render anomalychart with anomalycolumns=anomalies" to render the anomalies as bold points on the series charts.
```

![Time series anomaly detection.](../../media/anomaly-detection/series-anomaly-detection.png)

* The original time series (in red). 
* The baseline (seasonal + trend) component (in blue).
* The anomalous points (in purple) on top of the original time series. The anomalous points significantly deviate from the expected baseline values.

## Time series forecasting

The function [`series_decompose_forecast()`](series-decompose-forecastfunction.md) predicts future values of a set of time series. This function calls `series_decompose()` to build the decomposition model and then, for each time series, extrapolates the baseline component into the future.

The following query allows you to predict next week's web service traffic:

> [!div class="nextstepaction"]
> > [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA22QzW6DMBCE73mKuQFqKISqitSIW98gkXpEDl5iK9hG9uanUR++dqE99YRGO8x845EYRtuO0UIKJtaG8qbebMt6U9avxW41Joe4/+doyvoFTfNW14tPJlOjZqGc1w9n263crSQZ1xlxpi6Q1xSa1ReSLGcJezGtuJ7y+C3gLA6xZM/CTBi8MwshuxnkaUlGYJpS5/ETQUvEzJsiTz+ibZEd9psMQFUBgUbqGSLe7GkkpBVYygfn46EfSVjyuOpwEaN+CNbOxki6M1mZTNSLkAbOv3WSemcmF6j7vSX8dcTUlvOFsZJcFDHFx4wYnmp7JTzjplnlrHmkNvugI8Q0PYO9GAbdww0RyDjLav1XHLnBimAjEG5E5zQ7vRP284x36hOOTtxZ8Q3The8P2QEAAA==" target="_blank">Run the query</a>

```kusto
let min_t = datetime(2017-01-05);
let max_t = datetime(2017-02-03 22:00);
let dt = 2h;
let horizon=7d;
demo_make_series2
| make-series num=avg(num) on TimeStamp from min_t to max_t+horizon step dt by sid 
| where sid == 'TS1'   //  select a single time series for a cleaner visualization
| extend forecast = series_decompose_forecast(num, toint(horizon/dt))
| render timechart with(title='Web app. traffic of a month, forecasting the next week by Time Series Decomposition')
```

![Time series forecasting.](../../media/anomaly-detection/series-forecasting.png)

* Original metric (in red). Future values are missing and set to 0, by default.
* Extrapolate the baseline component (in blue) to predict next week's values.

## Scalability

Kusto Query Language syntax enables a single call to process multiple time series. Its unique optimized implementation allows for fast performance, which is critical for effective anomaly detection and forecasting when monitoring thousands of counters in near real-time scenarios.

The following query shows the processing of three time series simultaneously:

> [!div class="nextstepaction"]
> > [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA21Qy26DMBC85yvmFlChcUirSI34ikTqETl4KVawjfDmqX587UCaHuqLtePxPLYjhtG2YpRQkom1oaQQy3Uulrl4TzezLjLk5T9GkYsViuJDiImnIqlox6F1g745W67VZqbIuMrIA1WeBk2+mH0jjvk4wh5NKU9fSbhTOItdMNmyND2awZkpIbsxyMukDM/UR8/9FV6rIEkXJqvgmsYTl7X0lISHspzvtqt5hjdxPxkeYBHA4gGKFMBiAUilIAfWja617CY1NG4ASX/FSfuj7PRNsg4ZXANz7Fj3HSGuBmOjZ5hYbcSqIBwbZpNk+iQFcQpx4/omrqLamd55qh5v41d22nIybWChOI0qQ9Cg4e5ftyE6zprbhDV3VM4/aQ/Z96/gQTahU4wsYZzlNvs11vYL3BJsCIQz0eHed/W30jz9AUEBI0ktAgAA" target="_blank">Run the query</a>

```kusto
let min_t = datetime(2017-01-05);
let max_t = datetime(2017-02-03 22:00);
let dt = 2h;
let horizon=7d;
demo_make_series2
| make-series num=avg(num) on TimeStamp from min_t to max_t+horizon step dt by sid
| extend offset=case(sid=='TS3', 4000000, sid=='TS2', 2000000, 0)   //  add artificial offset for easy visualization of multiple time series
| extend num=series_add(num, offset)
| extend forecast = series_decompose_forecast(num, toint(horizon/dt))
| render timechart with(title='Web app. traffic of a month, forecasting the next week for 3 time series')
```

![Time series scalability.](../../media/anomaly-detection/series-scalability.png)

## Summary

This document details native KQL functions for time series anomaly detection and forecasting. Each original time series is decomposed into seasonal, trend and residual components for detecting anomalies and/or forecasting. These functionalities can be used for near real-time monitoring scenarios, such as fault detection, predictive maintenance, and demand and load forecasting.

## Next steps

Learn about [Machine learning capabilities](./machine-learning-clustering.md) with KQL.
