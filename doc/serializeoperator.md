---
title: serialize operator - Azure Data Explorer | Microsoft Docs
description: This article describes serialize operator in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 10/23/2018
---
# serialize operator

Marks that order of the input row set is safe for window functions usage.

Operator has declarative meaning, and it marks input row set as serialized (ordered) so [window functions](./windowsfunctions.md) could be applied to it.

```kusto
T | serialize rn=row_number()
```

**Syntax**

`serialize` [*Name1* `=` *Expr1* [`,` *Name2* `=` *Expr2*]...]

* The *Name*/*Expr* pairs are similar to those in the [extend operatpr](./extendoperator.md).

**Example**

```kusto
Traces
| where ActivityId == "479671d99b7b"
| serialize

Traces
| where ActivityId == "479671d99b7b"
| serialize rn = row_number()
```