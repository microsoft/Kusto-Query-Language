---
title: hash_combine() - Azure Data Explorer
description: learn how to use the hash_combine() function to combine hash values of two or more hashes.
ms.reviewer: alexans
ms.topic: reference
ms.date: 12/25/2022
---
# hash_combine()

Combines hash values of two or more hashes.

## Syntax

`hash_combine(`*h1* `,` *h2* [`,` *h3* ...]`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *h1*, *h2*, ... *hN* | long | &check; | The hash values to combine.|

## Returns

The combined hash value of the given scalars.

## Examples

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUShLzClNNVSwVVDySM3JyVfSgYgYgUTC84tyUpS4ahRSK0pS81IUMkDqMhKLMzQgujR1FDKMbBECRpoItcn5uUmZeakpUB3xUL5GhiFIkyYAFnd56X0AAAA=" target="_blank">Run the query</a>

```kusto
print value1 = "Hello", value2 = "World"
| extend h1 = hash(value1), h2=hash(value2)
| extend combined = hash_combine(h1, h2)
```

**Output**

|value1|value2|h1|h2|combined|
|---|---|---|---|---|
|Hello|World|753694413698530628|1846988464401551951|-1440138333540407281|
