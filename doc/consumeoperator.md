---
title:  consume operator
description: Learn how to use the consume operator to consume the tabular data stream handed to the operator.
ms.reviewer: alexans
ms.topic: reference
ms.date: 03/09/2023
---
# consume operator

Consumes the tabular data stream handed to the operator.

The `consume` operator is mostly used for triggering the query side-effect without actually returning
the results back to the caller.

The `consume` operator can be used for estimating the
cost of a query without actually delivering the results back to the client.
(The estimation isn't exact for various reasons; for example, `consume`
is calculated distributively, so `T | consume` won't transmit the table's
data between the nodes of the cluster.)

## Syntax

`consume` [`decodeblocks` `=` *DecodeBlocks*]

## Parameters

|Name|Type|Required|Description|
|--|--|--|--|
| *DecodeBlocks* | bool | | If set to `true`, or if the request property `perftrace` is set to `true`, the `consume` operator won't just enumerate the records at its input, but actually force each value in those records to be decompressed and decoded.|
