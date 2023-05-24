---
title:  count_distinctif() (aggregation function) - (preview)
description: Learn how to use the count_distinctif() function to count unique values of a scalar expression in records for which the predicate evaluates to true.
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/27/2022
---
# count_distinctif() (aggregation function) - (preview)

Conditionally counts unique values specified by the scalar expression per summary group, or the total number of unique values if the summary group is omitted. Only records for which *predicate* evaluates to `true` are counted.

[!INCLUDE [data-explorer-agg-function-summarize-note](../../includes/data-explorer-agg-function-summarize-note.md)]

If you only need an estimation of unique values count, we recommend using the less resource-consuming [dcountif](dcountif-aggfunction.md) aggregation function.

> [!NOTE]
> * This function is limited to 100M unique values. An attempt to apply the function on an expression returning too many values will produce a runtime error (HRESULT: 0x80DA0012).
> * Function performance can be degraded when operating on multiple data sources from different clusters.

## Syntax

`count_distinctif` `(`*expr*`,` *predicate*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *expr*| scalar | &check; | The expression whose unique values are to be counted. |
| *predicate* | string | &check; | The expression used to filter records to be aggregated. |

## Returns

Integer value indicating the number of unique values of *expr* per summary group, for all records for which the *predicate* evaluates to `true`.

## Example

This example shows how many types of death-causing storm events happened in each state. Only storm events with a nonzero count of deaths will be counted.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA22NsQ6CQBAFe79iS4gWNJZQoYk1Wpv1WMMm3h3cvdNA/HgRSi3fzCSvgQ/28BSHuHlTTNZy0Eno4nRIcmTwY7Wl8cnh2mqEOgO9Zws/j73ssloYXaw1iAFtaZ0n1y4gr4qcbiM1YMh88uok/DmgiorZwve0/+Y/wQetTCWoqwAAAA==" target="_blank">Run the query</a>

```kusto
StormEvents
| summarize UniqueFatalEvents=count_distinctif(EventType,(DeathsDirect + DeathsIndirect)>0) by State
| where UniqueFatalEvents > 0
| top 5 by UniqueFatalEvents
```

**Output**

| State           | UniqueFatalEvents |
| --------------- | ----------------- |
| TEXAS           | 12                |
| CALIFORNIA      | 12                |
| OKLAHOMA        | 10                |
| NEW YORK        | 9                 |
| KANSAS          | 9                 |
