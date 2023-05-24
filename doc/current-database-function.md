---
title:  current_database()
description: Learn how to use the current_database() function to return the name of the database in scope as a string type value.
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/23/2022
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
