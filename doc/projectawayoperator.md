---
title:  project-away operator
description: Learn how to use the project-away operator to select columns from the input table to exclude from the output table.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/12/2023
---
# project-away operator

Select what columns from the input table to exclude from the output table.

## Syntax

*T* `| project-away` *ColumnNameOrPattern* [`,` ...]

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *T* | string | &check; | The tabular input from which to remove columns. |
| *ColumnNameOrPattern* | string | &check; | One or more column names or column wildcard-patterns to be removed from the output.|

## Returns

A table with columns that weren't named as arguments. Contains same number of rows as the input table.

> [!TIP]
> You can `project-away` any columns that are present in the original table or that were computed as part of the query.

> [!NOTE]
> The order of the columns in the result is determined by their original order in the table. Only the columns that were specified as arguments are dropped. The other columns are included in the result.

## Examples

The input table `PopulationData` has 2 columns: `State` and `Population`. Project-away the `Population` column and you're left with a list of state names.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwvILyjNSSzJzM9zSSxJ5OWqUSgoys9KTS7RTSxPrFQIgEsDAH2sb1kpAAAA" target="_blank">Run the query</a>

```kusto
PopulationData
| project-away Population
```

The following table shows only the first 10 results.

|State|
|---|
|ALABAMA|
|ALASKA|
|ARIZONA|
|ARKANSAS|
|CALIFORNIA|
|COLORADO|
|CONNECTICUT|
|DELAWARE|
|DISTRICT OF COLUMBIA|
|FLORIDA|
|...|

### Project-away using a column name pattern

The following query removes columns starting with the word "session".

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA3POz0tLLUrNS04NTi0uzszPK+blqlEoKMrPSk0u0U0sT6xUKIZIaAEAV4MJgSsAAAA=" target="_blank">Run the query</a>

```kusto
ConferenceSessions
| project-away session*
```

The following table shows only the first 10 results.

|conference|owner|participants|URL|level|starttime|duration|time_and_duration|kusto_affinity|
|---|---|---|---|---|---|---|---|---|
|PASS Summit 2019| Avner Aharoni| |<https://www.eventbrite.com/e/near-real-time-interact-analytics-on-big-data-using-azure-data-explorer-fg-tickets-77532775619>| |2019-11-07T19:15:00Z|  |Thu, Nov 7, 11:15 AM-12:15 PM PST |Focused|
|PASS Summit| Rohan Kumar| Ariel Pisetzky|<https://www.pass.org/summit/2018/Learn/Keynotes.aspx>| |2018-11-07T08:15:00Z| 90 |Wed, Nov 7, 8:15-9:45 am |Mention|
|Intelligent Cloud 2019| Rohan Kumar| Henning Rauch| | |2019-04-09T09:00:00Z| 90| Tue, Apr 9, 9:00-10:30 AM |Mention|
|Ignite 2019| Jie Feng|   | `https://myignite.techcommunity.microsoft.com/sessions/83940` | 100| 2019-11-06T14:35:00Z| 20 |Wed, Nov 6, 9:35 AM - 9:55 AM| Mention|
|Ignite 2019| Bernhard Rode| Le Hai Dang, Ricardo Niepel |`https://myignite.techcommunity.microsoft.com/sessions/81596` | 200 |2019-11-06T16:45:00Z| 45| Wed, Nov 6, 11:45 AM-12:30 PM |Mention|
|Ignite 2019| Tzvia Gitlin| Troyna| `https://myignite.techcommunity.microsoft.com/sessions/83933` |  400 |2019-11-06T17:30:00Z| 75| Wed, Nov 6, 12:30 PM-1:30 PM |Focused|
|Ignite 2019| Jie Feng | `https://myignite.techcommunity.microsoft.com/sessions/81057` | 300| 2019-11-06T20:30:00Z| 45 |Wed, Nov 6, 3:30 PM-4:15 PM |Mention|
|Ignite 2019| Manoj Raheja|  | `https://myignite.techcommunity.microsoft.com/sessions/83939` | 300| 2019-11-07T18:15:00Z| 20 |Thu, Nov 7, 1:15 PM-1:35 PM|  Focused|
|Ignite 2019| Uri Barash|  | `https://myignite.techcommunity.microsoft.com/sessions/81060` |  300| 2019-11-08T17:30:00Z| 45 |Fri, Nov8,  10:30 AM-11:15 AM|  Focused|
|Ignite 2018| Manoj Raheja|  |<https://azure.microsoft.com/resources/videos/ignite-2018-azure-data-explorer-%E2%80%93-query-billions-of-records-in-seconds/>| 200|  |20|  |Focused|
|...|...|...|...|...|...|...|...|...|

## See also

* To choose what columns from the input to keep in the output, use [project-keep](project-keep-operator.md).
* To rename columns, use [`project-rename`](projectrenameoperator.md).
* To reorder columns, use [`project-reorder`](projectreorderoperator.md).
