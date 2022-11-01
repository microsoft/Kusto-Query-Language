---
title: Functions - Azure Data Explorer
description: This article describes Functions in Azure Data Explorer.
ms.reviewer: orspodek
ms.topic: reference
ms.date: 10/23/2018
adobe-target: true
---

# Function types

**Functions** are reusable queries or query parts. Kusto supports several
kinds of functions:

* **Stored functions** are user-defined functions that are stored and managed
  database schema entities.
  See [Stored functions](../schema-entities/stored-functions.md).
* **Query-defined functions** are user-defined functions that are defined
  and used within the scope of a single query. The definition of such functions
  is done through a [let statement](../letstatement.md).
  See [User-defined functions](./user-defined-functions.md).
* **Built-in functions** are hard-coded functions defined by Kusto that cannot be
  modified by users.
