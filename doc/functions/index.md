---
title: Functions - Azure Data Explorer
description: This article describes Functions in Azure Data Explorer.
ms.reviewer: orspodek
ms.topic: reference
ms.date: 11/27/2022
adobe-target: true
---

# Function types

**Functions** are reusable queries or query parts. Kusto supports two
kinds of functions:

* **Built-in functions** are hard-coded functions defined by Kusto that can't be
  modified by users.

* **User-defined functions**, which are divided into two types:

  * Stored functions: are user-defined functions that are stored and managed database schema entities (such as tables).
For more information on how to create and manage stored functions, see [Stored functions management overview](../../management/functions.md).

  * Query-defined functions: are user-defined functions that are defined and used within the scope of a single query. The definition of such functions is done through a let statement. For more information on how to create query-defined functions, see [Create a user defined function](../letstatement.md#create-a-user-defined-function-with-scalar-calculation).

    For more information on user-defined functions, see [User-defined functions](./user-defined-functions.md).
