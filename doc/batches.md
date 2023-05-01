---
title: Batches - Azure Data Explorer
description: This article describes Batches in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 03/14/2023
---
# Batches

A query can include multiple tabular expression statements, as long as they're delimited by a semicolon (`;`) character. The query then returns multiple tabular results. Results are produced by the tabular expression statements and ordered according to the order of the statements in the query text.

> [!NOTE]
>
> * Prefer batching and [`materialize`](materializefunction.md) over using the [fork operator](forkoperator.md).
> * Any two statements must be separated by a semicolon.

## Examples

### Name tabular results

The following query produces two tabular results. User agent tools can then display those results with the appropriate name associated with each (`Count of events in Florida` and `Count of events in Guam`, respectively).

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSspVqhRKM9ILUpVCC5JLElVsLVVUHLz8Q/ydHFUAkol55fmlQDpxGKFaHVnMCc/TSEVojMzT8EtJ78oMyVRPdaalysYr6HuoY6+xJjoXpqYqx4LAI0euu6hAAAA" target="_blank">Run the query</a>

```kusto
StormEvents | where State == "FLORIDA" | count | as ['Count of events in Florida'];
StormEvents | where State == "GUAM" | count | as ['Count of events in Guam']
```

### Share a calculation

Batching is useful for scenarios where a common calculation is shared by multiple subqueries, such as for dashboards. If the common calculation is complex, use the [materialize() function](./materializefunction.md) and construct the query so that it will be executed only once:

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA8tJLVHIVbBVyE0sSS3KTMzJrErVCC7JL8p1LUvNKylWqFEoLs3NTSwCiivk2Sbnl+aVaGgqJFUqBJcAdWha83LlAtWUZ6QWAeUV7BSMDAwM0ARtFAwNAArTIWBnAAAA" target="_blank">Run the query</a>

```kusto
let m = materialize(StormEvents | summarize n=count() by State);
m | where n > 2000;
m | where n < 10
```
