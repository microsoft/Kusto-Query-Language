---
title:  'Tutorial: Create geospatial visualizations'
description: This tutorial gives examples of geospatial visualizations in the Kusto Query Language.
ms.topic: tutorial
ms.date: 01/18/2023
---

# Tutorial: Create geospatial visualizations

This tutorial is for those who want to leverage [Kusto Query Language (KQL)](../index.md) for geospatial visualization. Geospatial clustering is a way to organize and analyze data based on geographical location. KQL offers multiple methods for performing [geospatial clustering](../geospatial-grid-systems.md), as well as tools for [geospatial visualizations](../geospatial-visualizations.md).

In this tutorial, you'll learn how to:

> [!div class="checklist"]
>
> * [Plot points on a map](#plot-points-on-a-map)
> * [Plot multiple series of points](#plot-multiple-series-of-points)
> * [Use GeoJSON values to plot points on a map](#use-geojson-values-to-plot-points-on-a-map)
> * [Represent data points with variable-sized bubbles](#represent-data-points-with-variable-sized-bubbles)
> * [Display points within a specific area](#display-points-within-a-specific-area)

## Plot points on a map

To visualize points on a map, use [project](../projectoperator.md) to select the column containing the longitude and then the column containing the latitude. Then, use [render](../renderoperator.md) to see your results in a scatter chart with `kind` set to `map`.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSsp5qpRKEnMTlUwNDAAMguK8rNSk0sUnFLTM/N88vN0oKzEEqBkUWpeSmqRQnFyYklJalFyRmJRiUJ5ZkmGgkZ2Zl6Kgq1CbmKBJgAmnyYWWwAAAA==" target="_blank">Run the query</a>

```kusto
StormEvents
| take 100
| project BeginLon, BeginLat
| render scatterchart with (kind = map)
```

:::image type="content" source="../images/kql-tutorials/geospatial-storm-events-scatterchart.png" alt-text="Screenshot of sample storm events on a map.":::

## Plot multiple series of points

To visualize multiple series of points, use [project](../projectoperator.md) to select the longitude and latitude along with a third column, which defines the series.

In the following query, the series is `EventType`. The points will be colored differently according to their `EventType`, and when selected will display the content of the `EventType` column.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSsp5qpRKEnMTlUwNDAAMguK8rNSk0sUnFLTM/N88vN0oKzEEh0FsPqQyoJUoLqi1LyU1CKF4uTEkpLUouSMxKIShfLMkgwFjezMvBQFW4XcxAJNAKZVk/hmAAAA" target="_blank">Run the query</a>

```kusto
StormEvents
| take 100
| project BeginLon, BeginLat, EventType
| render scatterchart with (kind = map)
```

:::image type="content" source="../images/kql-tutorials/geospatial-storm-events-by-type.png" alt-text="Screenshot of sample storm events on a map by type.":::

You may also explicitly specify the `xcolumn` (Longitude), `ycolumn` (Latitude), and `series` when performing the `render`. This is necessary when there are more columns in the result than just the longitude, latitude, and series columns.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSsp5qpRKEnMTlUwNDAAMotS81JSixSKkxNLSlKLkjMSi0oUyjNLMhQ0sjPzUhRsFXITC3QUKpLzc0pz84Bcp9T0zDyf/DwdhUqIWDFcMLFER6E4tSgzFSQEtiuksiBVEwDmTUhSewAAAA==" target="_blank">Run the query</a>

```kusto
StormEvents
| take 100
| render scatterchart with (kind = map, xcolumn = BeginLon, ycolumns = BeginLat, series = EventType)
```

## Use GeoJSON values to plot points on a map

A dynamic GeoJSON value can change or be updated and are often used for real-time mapping applications. Mapping points using dynamic GeoJSON values allows for more flexibility and control over the representation of the data on the map that may not be possible with plain latitude and longitude values.

The following query uses the [geo_point_to_s2cell](../geo-point-to-s2cell-function.md) and [geo_s2cell_to_central_point](../geo-s2cell-to-central-point-function.md) to map storm events in a scatter chart.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA33OvQrCQBAE4N6n2DKBICpYphHs7CwlhPWy5E7vJ+ytiuLDe5dYBAS7hZ1vmKMEdvs7eYmLNwwcLqQEdtQbfwi++l4o6RlvziGbF8H5CRqjrnsK7RCMl1ZCGzeKrC1+aQXbctY9Aqgh48lkrdICRjvVFbl9bqzvk0hTCW0xRpYqBO6MR6F4WjVlBRblX2bd5EIm3xFDVChCrDSywMOIhuJqfJe8w6H8ABkqA1kUAQAA" target="_blank">Run the query</a>

```kusto
StormEvents
| project BeginLon, BeginLat
| summarize by hash=geo_point_to_s2cell(BeginLon, BeginLat, 5)
| project point = geo_s2cell_to_central_point(hash)
| project lng = toreal(point.coordinates[0]), lat = toreal(point.coordinates[1])
| render scatterchart with (kind = map)
```

:::image type="content" source="../images/kql-tutorials/geospatial-storm-events-centered.png" alt-text="Screenshot of sample storm events displayed using geojson.":::

## Represent data points with variable-sized bubbles

Visualize the distribution of data points by performing an aggregation in each cluster and then plotting the central point of the cluster.

For example, the following query filters for all storm events of the "Tornado" event type. It then groups the events into clusters based on their longitude and latitude, counts the number of events in each cluster, and projects the central point of the cluster, and renders a map to visualize the result. The regions with the most tornados become clearly detected based on their large bubble size.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA2VQsU7DQAzd+QorU07KhFhvqdSNje7RcbV6B4l98jktQXw8TtJCEZuf/d7zs1+UZdyfkbQ+fMEloSCs8DAXBO+hObBQOHJj4yL8hlFhh6dMz0zdtQr6o82VWGkahvY2chDo+K/P5ExTp3EMkj8RIk+k/YZnv6LWwesMKdQEHk7IfeFsHOW+Pka8c/qN0cGTu8u5iDbuoop2lYRhs2kXX9f93WtS/FC0uNtHbG+zEpbjxfooUDLGFEThkjVB+56N7WEMxX0DtXW+QEsBAAA=" target="_blank">Run the query</a>

```kusto
StormEvents
| where EventType == "Tornado"
| project BeginLon, BeginLat
| where isnotnull(BeginLat) and isnotnull(BeginLon)
| summarize count_summary=count() by hash = geo_point_to_s2cell(BeginLon, BeginLat, 4)
| project geo_s2cell_to_central_point(hash), count_summary
| extend Events = "count"
| render piechart with (kind = map)
```

:::image type="content" source="../images/kql-tutorials/tornado-geospatial-map.png" alt-text="Screenshot of Azure Data Explorer web UI showing a geospatial map of tornado storms.":::

## Display points within a specific area


Use a polygon to define the region and the [geo_point_in_polygon](../geo-point-in-polygon-function.md) function to filter for events that occur within that region.

The following query defines a polygon representing the southern California region and filters for storm events within this region. It then groups the events into clusters, counts the number of events in each cluster, projects the central point of the cluster, and renders a map to visualize the clusters.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA21QTU+EQAy98yuaOUGCm+wqiavZi4k3DyYeCSHj0IVRmJKZ4oof/90CiR+ROfW17/W9TosMgQZu0LvS6NYeyTur4QDV6HRnTfwegTzFY4/qCtQ9tWNNTqVL2xD5yjrNGGSa5/nZdrvfZCmcX2yyIoUJZ6t49433q3jiF7NJ8ZlcRw9Mvrt9Qcch+oCT5EWokcqerOPSOinmYPEN1tbdkUthqTSnaxcmsqX39ISG4b9EhmHoOu3tG4KhQSwWPMrPzDhO4HGERodGOj9BmMqwM9i2qzkuf7tOooU7qYxc5nW7rImnvUn611mk+MroKli+QXzVTFAy8dJHD71F02jPcLLcQPxshX2ATvfJF6/vcb7pAQAA" target="_blank">Run the query</a>

```kusto
let southern_california = dynamic({
    "type": "Polygon",
    "coordinates": [[[-119.5, 34.5], [-115.5, 34.5], [-115.5, 32.5], [-119.5, 32.5], [-119.5, 34.5]]
    ]});
StormEvents
| where geo_point_in_polygon(BeginLon, BeginLat, southern_california)
| project BeginLon, BeginLat
| summarize count_summary = count() by hash = geo_point_to_s2cell(BeginLon, BeginLat, 8)
| project geo_s2cell_to_central_point(hash), count_summary
| extend Events = "count"
| render piechart with (kind = map)
```

:::image type="content" source="../images/kql-tutorials/geospatial-southern-california-polygon.png" alt-text="Screenshot of Azure Data Explorer web UI showing a geospatial map of southern California storms.":::

## Next steps

* Read more about the [Kusto Query Language](../index.md)
* Learn how to perform [cross-database and cross-cluster queries](../cross-cluster-or-database-queries.md)
* Learn how to [ingest data](../../../ingest-sample-data.md)
* Get a comprehensive understanding by reading the [white paper](https://azure.microsoft.com/mediahandler/files/resourcefiles/azure-data-explorer/Azure_Data_Explorer_white_paper.pdf)
