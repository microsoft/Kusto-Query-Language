---
title: hash_many() - Azure Data Explorer
description: Learn how to use the hash_many() function to return a combined hash value of multiple values.
ms.reviewer: alexans
ms.topic: reference
ms.date: 12/25/2022
---
# hash_many()

Returns a combined hash value of multiple values.

## Syntax

`hash_many(`*s1* `,` *s2* [`,` *s3* ...]`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *s1*, *s2*, ..., *sN* | scalar | &check; | The values to hash together.|

## Returns

The [hash()](hashfunction.md) function is applied to each of the specified scalars. The resulting hashes are combined into a single hash and returned.

> [!WARNING]
> The function uses the *xxhash64* algorithm to calculate the hash for each scalar, but this may change. We therefore only recommend using this function within a single query where all invocations of the function will use the same algorithm.
>
> If you need to persist a combined hash, we recommend using [hash_sha256()](sha256hashfunction.md), [hash_sha1()](sha1-hash-function.md), or [hash_md5()](md5hashfunction.md) and combining the hashes into a single hash with a [bitwise operator](binoperators.md). Note that these functions are more complex to calculate than `hash()`.

## Examples

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUShLzClNNVSwVVDySM3JyVfSgYgYgUTC84tyUpS4ahRSK0pS81IUkvNzkzLzUlOAchmJxRnxuYl5lRoQA2DaNAGiXIb7VgAAAA==" target="_blank">Run the query</a>

```kusto
print value1 = "Hello", value2 = "World"
| extend combined = hash_many(value1, value2)
```

**Output**

|value1|value2|combined|
|---|---|---|
|Hello|World|-1440138333540407281|
