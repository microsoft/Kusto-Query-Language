---
title: Window functions - Azure Data Explorer | Microsoft Docs
description: This article describes Window functions in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 02/20/2019
---
# Window functions

Window functions operate on multiple rows (records) in a row set at a time.
Unlike aggregation functions, they require that the rows in the row set
be **serialized** (have a specific order to them), as window functions may depend
on the order to determine the result.

Window functions cannot be used on row sets that are not serialized, and will give
an error when used in such a context by a query. The easiest way to serialize
a row set is to use the [serialize operator](./serializeoperator.md),
which simply "freezes" the order of rows (in some unspecified arbitrary manner).
If the order of serialized rows is semantically important, one can use the
[sort operator](./sortoperator.md) to force a particular order.

The serialization process has a non-trivial cost associated with it. For example,
it might prevent query parallelism in many scenarios. Therefore,
it is strongly recommended that serialization will not be applied unnecessarily,
and that if necessary, the query be rearranged so that serialization be performed
on the smallest row set possible.

## Serialized row set

An arbitrary row set (such as a table, or the output of a tabular operator) can
be serialized in one of the following ways:

1. By sorting the row set. See below for a list of operators that emit sorted
   row sets.
2. By using the [serialize operator](./serializeoperator.md).

Note that many tabular operators, while in themselves they don't guarantee that
their result is serialized, do have the property that if the input is serialized, so
is the output. For example, this property is guaranteed for the [extend operator](./extendoperator.md),
the [project operator](./projectoperator.md), and the
[where operator](./whereoperator.md).

## Operators that emit serialized row sets by sorting

* [order operator](./orderoperator.md)
* [sort operator](./sortoperator.md)
* [top operator](./topoperator.md)
* [top-hitters operator](./tophittersoperator.md)
* [top-nested operator](./topnestedoperator.md)

## Operators that preserve the serialized row set property

* [extend operator](./extendoperator.md)
* [mv-expand operator](./mvexpandoperator.md)
* [parse operator](./parseoperator.md)
* [project operator](./projectoperator.md)
* [project-away operator](./projectawayoperator.md)
* [project-rename operator](./projectrenameoperator.md)
* [take operator](./takeoperator.md)
* [where operator](./whereoperator.md)