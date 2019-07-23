---
title: getschema operator  - Azure Data Explorer | Microsoft Docs
description: This article describes getschema operator  in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 10/23/2018
---
# getschema operator 

Produce a table that represents a tabular schema of the input.

```kusto
T | summarize MyCount=count() by Country | getschema 
```

**Syntax**

*T* `| ` `getschema`

**Example**

```kusto
StormEvents
| top 10 by Timestamp
| getschema
```

|ColumnName|ColumnOrdinal|DataType|ColumnType|
|---|---|---|---|
|Timestamp|0|System.DateTime|datetime|
|Language|1|System.String|string|
|Page|2|System.String|string|
|Views|3|System.Int64|long
|BytesDelivered|4|System.Int64|long