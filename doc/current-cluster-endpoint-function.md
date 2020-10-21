---
title: current_cluster_endpoint() - Azure Data Explorer | Microsoft Docs
description: This article describes current_cluster_endpoint() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# current_cluster_endpoint()

Returns the network endpoint (DNS name) of the current cluster being queried.

## Syntax

`current_cluster_endpoint()`

## Returns

The network endpoint (DNS name) of the current cluster being queried,
as a value of type `string`.

## Example

```kusto
print strcat("This query executed on: ", current_cluster_endpoint())
```