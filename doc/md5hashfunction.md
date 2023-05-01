---
title: hash_md5() - Azure Data Explorer
description: Learn how to use the hash_md5() function to return the MD5 hash value of the input.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/08/2023
---
# hash_md5()

Returns an MD5 hash value of the input.

## Syntax

`hash_md5(`*source*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *source* | scalar | &check; | The value to be hashed.|

## Returns

The MD5 hash value of the given scalar, encoded as a hex string (a string
of characters, each two of which represent a single Hex number between 0
and 255).

> [!WARNING]
> The algorithm used by this function (MD5) is guaranteed
> to not be modified in the future, but is very complex to calculate. Users that
> need a "lightweight" hash function for the duration of a single query are advised
> to use the function [hash()](./hashfunction.md) instead.

## Examples

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUeDKMLTNSCzOiM9NMdVQCs8vyklR0tThyjBCiKYklqSWZOamahgZGBnoGhgCkaYmADVybf0+AAAA" target="_blank">Run the query</a>

```kusto
print 
h1=hash_md5("World"),
h2=hash_md5(datetime(2020-01-01))
```

**Output**

|h1|h2|
|---|---|
|f5a7924e621e84c9280a9a27e1bcb7f6|786c530672d1f8db31fee25ea8a9390b|

The following example uses the `hash_md5()` function to aggregate StormEvents based on State's MD5 hash value.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSsp5qpRKC7NzU0syqxKVQgGiTvnl+aVKNgqJINoDU2FpEqgeGJJqg6E8kgszrDNABLxuSmmGmAhTaAhJfkFCqYQtTAzAJ9n7vdmAAAA" target="_blank">Run the query</a>

```kusto
StormEvents
| summarize StormCount = count() by State, StateHash=hash_md5(State)
| top 5 by StormCount
```

**Output**

|State|StateHash|StormCount|
|---|---|---|
|TEXAS|3b00dbe6e07e7485a1c12d36c8e9910a|4701|
|KANSAS|e1338d0ac8be43846cf9ae967bd02e7f|3166|
|IOWA|6d4a7c02942f093576149db764d4e2d2|2337|
|ILLINOIS|8c00d9e0b3fcd55aed5657e42cc40cf1|2022|
|MISSOURI|2d82f0c963c0763012b2539d469e5008|2016|
