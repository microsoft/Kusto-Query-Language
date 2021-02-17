---
title: Entity names - Azure Data Explorer | Microsoft Docs
description: This article describes Entity names in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: rkarlin
ms.service: data-explorer
ms.topic: reference
ms.date: 02/24/2019
---
# Entity names

Kusto entities (databases, tables, columns, and stored functions; clusters
are an exception) are named. The name of an entity identifies the entity,
and is guaranteed to be unique in the scope of its container given its type.
(So, for example, two tables in the same database can't have the same name,
but a table and a database may have the same name because they are not in
the same scope, and a table and a stored function may have the same name
because they are not of the same entity type.)

Entity names are **case-sensitive** for resolving purposes
(so, for example, you can't refer to a table called `ThisTable` as `thisTABLE`).

Entity names are one example of **identifiers**. Other identifiers include the names of
parameters to functions and binding a name through a [let statement](../letstatement.md).

## Entity pretty names

Some entities (such as databases) may have, in addition to their entity name,
a **pretty name**. Pretty names can be used to reference the entity in queries
(like entity names), but, unlike entity names, pretty names aren't necessarily unique
in the context of their container. When a container has multiple entities with the
same pretty name, the pretty name can't be used to reference the entity.

Pretty names allow middle-tier applications to map automatically-create entity names
(such as UUIDs) to names that are human-readable for display and referencing purposes.

## Identifier naming rules

Identifiers are used to name various entities (entities or otherwise).
Valid identifier names follow these rules:
* They have between 1 and 1024 characters long.
* They may contain letters, digits, underscores (`_`), spaces, dots (`.`), and dashes (`-`).
  * Identifiers consisting only of letters, digits, and underscores
    do not require quoting when the identifier is being referenced.
  * Identifiers containing at last one of (spaces, dots, or dashes) do
    require quoting (see below).
* They are case-sensitive.

## Identifier quoting

Identifiers that are identical to some query language
keywords, or have one of the special characters noted above,
require quoting when they are referenced directly by a query:

|Query text         |Comments                          |
|-------------------|----------------------------------|
| `entity`          |Entity names (`entity`) that do not include special characters or map to some language keyword require no quoting|
|`['entity-name']`  |Entity names that include special characters (here: `-`) must be quoted using `['` and `']` or using `["` and `"]`|
|`["where"]`        |Entity names that are language keywords must be quoted quoted using `['` and `']` or using `["` and `"]`|

## Naming your entities to avoid collisions with Kusto language keywords

As the Kusto query language includes a number of keywords that have the same
naming rules as identifiers, it is possible to have entity names that are actually
keywords, but then referring to these names becomes difficult (one must quote them).

Alternatively, one might want to choose entity names that are guaranteed to never
"collide" with a Kusto keyword. The following guarantees are made:

1. The Kusto query language will not define a keyword that starts with a capital letter (`A` to `Z`).
2. The Kusto query language will not define a keyword that starts with a single underscore (`_`).us
3. The Kusto query language will not define a keyword that ends with a single underscore (`_`).

The Kusto query language reserves all identifiers that start or end with a
sequence of two underscore characters (`__`); users cannot define such names
for their own use.








