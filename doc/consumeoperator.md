---
title: consume operator - Azure Data Explorer | Microsoft Docs
description: This article describes consume operator in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 10/23/2018
---
# consume operator

The `consume` operator consumes the tabular data stream handed to it. It is
mostly used for triggering the query side-effect without actually returning
the results back to the caller.

```kusto
T | consume
```

**Syntax**

`consume` [`decodeblocks` `=` *DecodeBlocks*]

**Arguments**

* *DecodeBlocks*: A constant Boolean value. If set to `true`, or if the request
  property `perftrace` is set to `true`, the `consume` operator will not just
  enumerate the records at its input, but actually force each value in those
  records to be decompressed and decoded.

The `consume` operator can be used for estimating the
cost of a query without actually delivering the results back to the client.
(The estimation is not exact for a variety of reasons; for example, `consume`
is calculated distributively, so `T | consume` will not transmit the table's
data between the nodes of the cluster.)

<!--
* *WithStats*: A constant Boolean value. If set to `true` (or if the global
  property `perftrace` is set), the operator will return a single
  row with a single column called `Stats` of type `dynamic` holding the statistics
  of the data source fed to the `consume` operator.
-->