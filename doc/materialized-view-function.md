---
title: materialized_view() (scope function) - Azure Data Explorer
description: This article describes materialized_view() function in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: yifats
ms.service: data-explorer
ms.topic: reference
ms.date: 08/30/2020
---

# materialized_view() function

References the materialized part of a [materialized view](../management/materialized-views/materialized-view-overview.md). 

The `materialized_view()` function supports a way of querying the *materialized* part only of the view, while specifying the max latency the user is willing to tolerate. This option isn't guaranteed to return the most up-to-date records, but should always be more performant than querying the entire view. This function is useful for scenarios in which you're willing to sacrifice some freshness for performance, for example in telemetry dashboards.

<!--- csl --->
```
materialized_view('ViewName')
```

## Syntax

`materialized_view(`*ViewName*`,` [*max_age*]`)`

## Arguments

* *ViewName*: The name of the `materialized view`.
* *max_age*: Optional. If not provided, only the *materialized* part of the view is returned. If provided, function will return the 
_materialized_ part of the view if last materialization time is greater than [@now -  max_age]. Otherwise, the entire view is returned (identical 
to querying *ViewName* directly. 

## Examples

Query the *materialized* part of the view only, independent on when it was last materialized.

<!-- csl -->
```
materialized_view("ViewName")
```

Query the *materialized* part only if it was materialized in the last 10 minutes. If the materialized part is older than 10 minutes, return the full view. This option is expected to be less performant than querying the materialized part.

<!-- csl -->
```
materialized_view("ViewName", 10m)
```

## Notes

* Once a view is created, it can be queried just as any other table in the database, including participate in cross-cluster / cross-database queries.
* Materialized views aren't included in wildcard unions or searches.
* Syntax for querying the view is the view name (like a table reference).
* Querying the materialized view will always return the most up-to-date results, based on all records ingested to the source table. The query combines the materialized part of the view with all unmaterialized records in the source table. For more information, see [behind the scenes](../management/materialized-views/materialized-view-overview.md#how-materialized-views-work) for details.
