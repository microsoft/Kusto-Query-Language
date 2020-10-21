---
title: avg() (aggregation function) - Azure Data Explorer | Microsoft Docs
description: This article describes avg() (aggregation function) in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 09/26/2019
---
# avg() (aggregation function)

Calculates the average of *Expr* across the group. 

* Can only be used in context of aggregation inside [summarize](summarizeoperator.md)

## Syntax

summarize `avg(`*Expr*`)`

## Arguments

* *Expr*: Expression that will be used for aggregation calculation. Records with `null` values are ignored and not included in the calculation.

## Returns

The average value of *Expr* across the group.
 