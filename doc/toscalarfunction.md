---
title: toscalar() - Azure Data Explorer | Microsoft Docs
description: This article describes toscalar() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 10/23/2018
---
# toscalar()

Returns a scalar constant value of the evaluated expression. 

This function is useful for queries that require staged calculations, as for example
calculating a total count of events and then use it for for filtering groups
that exceed certain percent of all events. 

**Syntax**

`toscalar(`*Expression*`)`

**Arguments**

* *Expression*: Expression that will be evaluated for scalar conversion  

**Returns**

A scalar constant value of the evaluated expression.
If expression result is a tabular, then the first column and first row will be taken for conversion.

> [!TIP]
> You can use a [let statement](letstatement.md) for readability of the query when using `toscalar()`.

**Notes**

`toscalar()` can be calculated a constant number of times during the query execution.
In other words, `toscalar()` function cannot be applied on row-level of (for-each-row scenario).

**Examples**

The following query evaluates `Start`, `End` and `Step` as scalar constants - and
use it for `range` evaluation. 

```kusto
let Start = toscalar(print x=1);
let End = toscalar(range x from 1 to 9 step 1 | count);
let Step = toscalar(2);
range z from Start to End step Step | extend start=Start, end=End, step=Step
```

|z|start|end|step|
|---|---|---|---|
|1|1|9|2|
|3|1|9|2|
|5|1|9|2|
|7|1|9|2|
|9|1|9|2|