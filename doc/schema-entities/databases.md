---
title: Databases - Azure Data Explorer | Microsoft Docs
description: This article describes Databases in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: rkarlin
ms.service: data-explorer
ms.topic: reference
ms.date: 10/30/2019
---
# Databases

Kusto follows a relation model of storing the data where upper-level entity is a `database`. 

Kusto cluster can host several databases, where each database will host its own  collection of [tables](tables.md), [stored functions](stored-functions.md), and [external tables](externaltables.md).
Each database has its own permissions set, based on [Role Based Access Control (RBAC) model](../../management/access-control/index.md)

**Notes**  

* Database names must follow the rules for [entity names](./entity-names.md).
* Maximum limit of databases per cluster is 10,000.
* Queries combining data from multiple tables in the same database and queries combining data from multiple databases in the same cluster have comparable performance. 
