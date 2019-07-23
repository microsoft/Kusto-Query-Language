---
title: External tables (preview) - Azure Data Explorer | Microsoft Docs
description: This article describes External tables (preview) in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 07/14/2019
---
# External tables (preview)

An **external table** is a Kusto schema entity that references data stored outside Azure Data Explorer.
Similar to [tables](tables.md), an external table has a well-defined schema (an ordered list of column name/type pairs). Unlike tables, data is stored and managed outside Azure Data Explorer. Most commonly the data is stored in some standard format such as CSV or Parquet, and is not ingested by Azure Data Explorer.

An external table is created once (see [External Table control commands](../../management/externaltables.md))
and can be referenced by its name. External table names can’t overlap with Kusto table names.

> [!NOTE]
> Kusto supports [exporting data to an external table](../../management/data-export/export-data-to-an-external-table.md).
> as well as [querying external tables](https://docs.microsoft.com/en-us/azure/data-explorer/data-lake-query-data).