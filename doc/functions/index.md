---
title: Functions - Azure Data Explorer | Microsoft Docs
description: This article describes Functions in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: rkarlin
ms.service: data-explorer
ms.topic: reference
ms.date: 10/23/2018
ms.localizationpriority: high
adobe-target: true
---
# Function types

**Functions** are reusable queries or query parts. Kusto supports several
kinds of functions:

* **Stored functions**, which are user-defined functions that are stored and managed
  a one kind of a database's schema entities.
  See [Stored functions](../schema-entities/stored-functions.md).
* **Query-defined functions**, which are user-defined functions that are defined
  and used within the scope of a single query. The definition of such functions
  is done through a [let statement](../letstatement.md).
  See [User-defined functions](./user-defined-functions.md).
* **Built-in functions**, which are hard-coded (defined by Kusto and cannot be
  modified by users).