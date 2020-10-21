---
title: serialize operator - Azure Data Explorer
description: This article describes serialize operator in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# serialize operator

Marks that the order of the input row set is safe to use for window functions.

The operator has a declarative meaning. It marks the input row set as serialized (ordered), so that [window functions](./windowsfunctions.md) can be applied to it.

```kusto
T | serialize rn=row_number()
```

## Syntax

`serialize` [*Name1* `=` *Expr1* [`,` *Name2* `=` *Expr2*]...]

* The *Name*/*Expr* pairs are similar to those pairs in the [extend operator](./extendoperator.md).

## Example

```kusto
Traces
| where ActivityId == "479671d99b7b"
| serialize

Traces
| where ActivityId == "479671d99b7b"
| serialize rn = row_number()
```

The output row set of the following operators is marked as serialized.

[range](./rangeoperator.md), [sort](./sortoperator.md), [order](./orderoperator.md), [top](./topoperator.md), [top-hitters](./tophittersoperator.md), [getschema](./getschemaoperator.md).

The output row set of the following operators is marked as non-serialized.

[sample](./sampleoperator.md), [sample-distinct](./sampledistinctoperator.md), [distinct](./distinctoperator.md), [join](./joinoperator.md), 
[top-nested](./topnestedoperator.md), [count](./countoperator.md), [summarize](./summarizeoperator.md), [facet](./facetoperator.md), [mv-expand](./mvexpandoperator.md), 
[evaluate](./evaluateoperator.md), [reduce by](./reduceoperator.md), [make-series](./make-seriesoperator.md)

All other operators preserve the serialization property. 
If the input row set is serialized, then the output row set is also serialized.
