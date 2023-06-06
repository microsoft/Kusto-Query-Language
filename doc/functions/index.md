---
title:  Functions
description: This article describes Functions in Azure Data Explorer.
ms.reviewer: orspodek
ms.topic: reference
ms.date: 06/05/2023
adobe-target: true
---

# Function types

Functions are reusable queries or query parts. Kusto supports two
kinds of functions:

* *Built-in functions* are hard-coded functions defined by Kusto that can't be
  modified by users.

* *User-defined functions*, which are divided into two types:

  * *Stored functions*: user-defined functions that are stored and managed database schema entities, similar to tables. For more information, see [Stored functions](../../query/schema-entities/stored-functions.md). To create a stored function, use the [.create function command](../../management/create-function.md).

  * *Query-defined functions*: user-defined functions that are defined and used within the scope of a single query. The definition of such functions is done through a let statement. For more information on how to create query-defined functions, see [Create a user defined function](../letstatement.md#create-a-user-defined-function-with-scalar-calculation).

  For more information on user-defined functions, see [User-defined functions](./user-defined-functions.md).
