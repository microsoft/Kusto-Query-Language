---
title: ingestion_time() - Azure Data Explorer
description: This article describes ingestion_time() in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 02/19/2020
zone_pivot_group_filename: data-explorer/zone-pivot-groups.json
zone_pivot_groups: kql-flavors
---
# ingestion_time()

::: zone pivot="azuredataexplorer"

Returns the approximate time at which the current record was ingested.

This function must be used in the context of a table of ingested data for which the [IngestionTime policy](../management/ingestiontimepolicy.md) was enabled when the data was ingested. Otherwise, this function produces null values.

::: zone-end

::: zone pivot="azuremonitor"

Retrieves the `datetime` when the record was ingested and ready for query.

::: zone-end

> [!NOTE]
> The value returned by this function is only approximate, as the ingestion process may take several minutes to complete and multiple ingestion activities may take place concurrently. To process all records of a table with exactly-once guarantees, use [database cursors](../management/databasecursor.md).

> [!TIP] 
> The ingestion_time() function returns values according to the service clock as measured when ingestion was completed. As a result, this value cannot be used to "order" ingestion operations, as two operations that overlap in time might have any ordering of these values. If ordering records is important for application semantics, one should ensure that the htable has a timestamp column as measured by the source of the data instead of relying on the ingestion_time() value.

## Syntax

`ingestion_time()`

## Returns

A `datetime` value specifying the approximate time of ingestion into a table.

## Example

```kusto
T
| extend ingestionTime = ingestion_time() | top 10 by ingestionTime
```
