---
title: Entity references - Azure Data Explorer | Microsoft Docs
description: This article describes Entity references in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 10/23/2018
---
# Entity references

Named Kusto schema entities (databases, tables, columns, and stored functions,
but not clusters) are referenced in the query by using their names. If the entity's
container is unambiguous in the current context, the entity name can be used
without additional qualifications. For example, when running a query against a
database called `DB`, one may reference a table called `T` in that database
simply using its name, `T`.

If, on the other hand, the entity's container is not available from the
context, or one wants to reference an entity from a container different than
the container in context, one has to use the entity's **qualified name**,
which is the concatenation of the entity name to the container's (and potentially
the its container's, etc.) Thus, a query running against database `DB` may
refer to a table `T1` in a different database `DB1` of the same cluster by using
`database("DB1").T1`, and if it wants to reference a table from another cluster
it can do so, for example, by using `cluster("https://C2.kusto.windows.net/").database("DB2").T2`.

Entity references can also the entity pretty name, as long as it is unique
in the context of the entity's container. See [entity pretty names](./entity-names.md#entity-pretty-names).

## Wildcard matching for entity names

In some contexts, one may use a wildcard (`*`) to match all or part of an entity
name. For example, the following query references all tables in the current database,
as well as all tables in database `DB` whose name starts with a `T`:

```kusto
union *, database("DB1").T*
```

Note: Wildcard matching does not match entity names that start with a dollar sign (`$`).
Such names are system-reserved.



