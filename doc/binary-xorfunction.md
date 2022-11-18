---
title: binary_xor() - Azure Data Explorer
description: This article describes binary_xor() in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 02/13/2020
---
# binary_xor()

Returns a result of the bitwise `xor` operation of the two values.

## Syntax

`binary_xor(`*value1*`,`*value2*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *value1* | int | &check; | The left-side value of the XOR operation. |
| *value2* | int | &check; | The right-side value of the XOR operation. |

## Returns

Returns logical XOR operation on a pair of numbers: value1 ^ value2.

## Examples

[**Run the Query**](https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswr0UjKzEssqoyvyC/SMNQx1NQEAKWP8zEWAAAA)

```kusto
binary_xor(1,1)
```

|Result|
|------|
|0 |

[**Run the Query**](https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswr0UjKzEssqoyvyC/SMNQx0tQEAPwxtTMWAAAA)

```kusto
binary_xor(1,2)
```

|Result|
|------|
|3 |
