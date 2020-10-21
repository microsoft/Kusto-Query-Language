---
title: current_database() - Azure Data Explorer | Microsoft Docs
description: This article describes current_database() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# current_database()

Returns the name of the database in scope (database that all query
entities are resolved against if no other database is specified).

## Syntax

`current_database()`

## Returns

The name of the database in scope as a value of type `string`.

## Example

```kusto
print strcat("Database in scope: ", current_database())
```