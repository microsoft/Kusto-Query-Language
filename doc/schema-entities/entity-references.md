---
title: Entity references - Azure Data Explorer
description: This article describes Entity references in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: rkarlin
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# Entity references

Reference Kusto schema entities in a query by using their names. Valid entity names include *databases*, *tables*, *columns*, and stored functions. *Clusters* can't be referenced by their names.
If the entity's container is unambiguous in the current context, use the entity name without additional qualifications. For example, when running a query against a
database called `DB`, you may reference a table called `T` in that database by its name, `T`.

If the entity's container isn't available from the context, or you want to reference an entity from a container different than the container in context, use the entity's **qualified name**.
The name is the concatenation of the entity name to the container's, and potentially its container's, and so on. In this way, a query running against database `DB` may refer to a table `T1` in a different database `DB1` of the same cluster, by using `database("DB1").T1`. If the query wants to reference a table from another cluster it can do so, for example, by using `cluster("https://C2.kusto.windows.net/").database("DB2").T2`.

Entity references can also use the entity pretty name, as long as it's unique
in the context of the entity's container. For more information, see [entity pretty names](./entity-names.md#entity-pretty-names).

## Wildcard matching for entity names

In some contexts, you may use a wildcard (`*`) to match all or part of an entity
name. For example, the following query references all tables in the current database,
and all tables in database `DB` whose name starts with a `T`:

```kusto
union *, database("DB1").T*
```

> [!NOTE]
> Wildcard matching can't match entity names that start with a dollar sign (`$`).
Such names are system-reserved.

## Next steps

* [schema entity types](./index.md)
* [schema entity names](./entity-names.md)