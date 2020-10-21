---
title: arg_max() (aggregation function) - Azure Data Explorer | Microsoft Docs
description: This article describes arg_max() (aggregation function) in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 10/23/2018
---
# arg_max() (aggregation function)

Finds a row in the group that maximizes *ExprToMaximize*, and returns the value of *ExprToReturn* (or `*` to return the entire row).

* Can be used only in context of aggregation inside [summarize](summarizeoperator.md)

## Syntax

`summarize` [`(`*NameExprToMaximize* `,` *NameExprToReturn* [`,` ...] `)=`] `arg_max` `(`*ExprToMaximize*, `*` | *ExprToReturn*  [`,` ...]`)`

## Arguments

* *ExprToMaximize*: Expression that will be used for aggregation calculation. 
* *ExprToReturn*: Expression that will be used for returning the value when *ExprToMaximize* is
  maximum. Expression to return may be a wildcard (*) to return all columns of the input table.
* *NameExprToMaximize*: An optional name for the result column representing *ExprToMaximize*.
* *NameExprToReturn*: Additional optional names for the result columns representing *ExprToReturn*.

## Returns

Finds a row in the group that maximizes *ExprToMaximize*, and 
returns the value of *ExprToReturn* (or `*` to return the entire row).

## Examples

See examples for [arg_min()](arg-min-aggfunction.md) aggregation function