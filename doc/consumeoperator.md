---
title: consume operator - Azure Data Explorer | Microsoft Docs
description: This article describes consume operator in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 03/30/2020
---
# consume operator

Consumes the tabular data stream handed to the operator. 

The `consume` operator is mostly used for triggering the query side-effect without actually returning
the results back to the caller.

```kusto
T | consume
```

## Syntax

`consume` [`decodeblocks` `=` *DecodeBlocks*]

## Arguments

* *DecodeBlocks*: A constant Boolean value. If set to `true`, or if the request
  property `perftrace` is set to `true`, the `consume` operator will not just
  enumerate the records at its input, but actually force each value in those
  records to be decompressed and decoded.

The `consume` operator can be used for estimating the
cost of a query without actually delivering the results back to the client.
(The estimation is not exact for a variety of reasons; for example, `consume`
is calculated distributively, so `T | consume` will not transmit the table's
data between the nodes of the cluster.)
