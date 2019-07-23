---
title: cursor_current(), current_cursor() - Azure Data Explorer | Microsoft Docs
description: This article describes cursor_current(), current_cursor() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 10/23/2018
---
# cursor_current(), current_cursor()

Retrieves the current value of the cursor of the database in scope. (The names `cursor_current`
and `current_cursor` are synonyms.)

**Syntax**

`cursor_current()`

**Returns**

Returns a single value of type `string` which encodes the current value of the
cursor of the database in scope.

**Remarks**

See [database cursors](../management/databasecursor.md) for additional
details on database cursors.