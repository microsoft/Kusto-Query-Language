---
title: project-keep operator - Azure Data Explorer
description: This article describes project-keep operator in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/24/2022
---
# project-keep operator

Select what columns from the input to keep in the output. Only the columns that are specified as arguments will be shown in the result. The other columns are excluded.

## Syntax

*T* `| project-keep` *ColumnNameOrPattern* [`,` ...]

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *T* | string | &check; | Tabular input from which to keep columns.|
| *ColumnNameOrPattern* | string | &check; | Name of the column or column wildcard-pattern to be kept in the output.|

## Returns

A table with columns that were named as arguments. Contains same number of rows as the input table.

> [!TIP]
> You can `project-keep` any columns that are present in the original table or that were computed as part of the query.

> [!NOTE]
> The order of the columns in the result is determined by their original order in the table. Only the columns that were specified as arguments are kept. The other columns are excluded from the result.

## Example

Only show columns from the `ConferenceSessions` table that contain the word "session".

[**Run the query**](https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA3POz0tLLUrNS04NTi0uzszPK+blqlEoKMrPSk0u0c1OTS1QKIZIaAEAWs65FysAAAA=)

```kusto
ConferenceSessions
| project-keep session*
```

The following table displays only the output columns. To see the full content of the output run the above query.

|sessionid| session_title| session_type| session_location|
|--|--|--|--|
||||

## See also

* To choose what columns from the input to exclude from the output, use [project-away](projectawayoperator.md).
* To rename columns, use [`project-rename`](projectrenameoperator.md).
* To reorder columns, use [`project-reorder`](projectreorderoperator.md).
