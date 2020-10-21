---
title: sum() (aggregation function) - Azure Data Explorer | Microsoft Docs
description: This article describes sum() (aggregation function) in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 10/23/2018
---
# sum() (aggregation function)

Calculates the sum of *Expr* across the group. 

* Can be used only in context of aggregation inside [summarize](summarizeoperator.md)

## Syntax

summarize `sum(`*Expr*`)`

## Arguments

* *Expr*: Expression that will be used for aggregation calculation. 

## Returns

The sum value of *Expr* across the group.
 