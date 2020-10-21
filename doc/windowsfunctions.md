---
title: Window functions - Azure Data Explorer
description: This article describes Window functions in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/20/2019
---
# Window functions overview

Window functions operate on multiple rows (records) in a row set at a time. Unlike aggregation functions, window functions require that the rows in the row set be serialized (have a specific order to them). Window functions may depend on the order to determine the result.

Window functions can only be used on serialized sets. The easiest way to serialize a row set is to use the [serialize operator](./serializeoperator.md). This operator "freezes" the order of rows in an arbitrary manner. If the order of serialized rows is semantically important, use the [sort operator](./sortoperator.md) to force a particular order.

The serialization process has a non-trivial cost associated with it. For example, it might prevent query parallelism in many scenarios. Therefore, don't apply serialization unnecessarily. If necessary, rearrange the query to perform serialization on the smallest row set possible.

## Serialized row set

An arbitrary row set (such as a table, or the output of a tabular operator) can
be serialized in one of the following ways:

1. By sorting the row set. See below for a list of operators that emit sorted
   row sets.
2. By using the [serialize operator](./serializeoperator.md).

Many tabular operators serialize output whenever the input is already serialized, even if the operator doesn't itself guarantee that the result is serialized. For example, this property is guaranteed for the [extend operator](./extendoperator.md), the [project operator](./projectoperator.md), and the [where operator](./whereoperator.md).

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
