---
title:  current_cluster_endpoint()
description: Learn how to use the current_cluster_endpoint() function to return the network endpoint of the cluster being queried as a string type value.
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/23/2022
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
