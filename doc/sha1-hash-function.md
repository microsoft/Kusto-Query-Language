---
title: hash_sha1() - Azure Data Explorer
description: Learn how to use the hash_sha1() function to return a sha1 hash value of the source input.
ms.reviewer: atefsawaed
ms.topic: reference
ms.date: 01/30/2023
---
# hash_sha1()

Returns a sha1 hash value of the source input.

## Syntax

`hash_sha1(`*source*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *source* | scalar | &check; | The value to be hashed.|

## Returns

The sha1 hash value of the given scalar, encoded as a hex string (a string
of characters, each two of which represent a single Hex number between 0
and 255).

> [!WARNING]
> The algorithm used by this function (SHA1) is guaranteed
> to not be modified in the future, but is very complex to calculate. If you
> need a "lightweight" hash function for the duration of a single query, consider using [hash()](./hashfunction.md).

## Examples

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUeBSAIIMQ9uMxOKM+OKMREMNpfD8opwUJU0diJQRklRKYklqSWZuqoaRgZGBroEhEGlqAgBM1jIESAAAAA==" target="_blank">Run the query</a>

```kusto
print 
    h1=hash_sha1("World"),
    h2=hash_sha1(datetime(2020-01-01))
```

**Output**

|h1|h2|
|---|---|
|70c07ec18ef89c5309bbb0937f3a6342411e1fdd|e903e533f4d636b4fc0dcf3cf81e7b7f330de776|

The following example uses the `hash_sha1()` function to aggregate StormEvents based on State's SHA1 hash value. 

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSspVuCqUSguzc1NLMqsSlUIBkk455fmlSjYKiSDaA1NhaRKoHhiSaoOhPJILM6wzQAS8cUZiYYaYDFNoCkl+QUKphDFcENSUouTAeqGdyxtAAAA" target="_blank">Run the query</a>

```kusto
StormEvents 
| summarize StormCount = count() by State, StateHash=hash_sha1(State)
| top 5 by StormCount desc
```

**Output**

|State|StateHash|StormCount|
|---|---|---|
|TEXAS|3128d805194d4e6141766cc846778eeacb12e3ea|4701|
|KANSAS|ea926e17098148921e472b1a760cd5a8117e84d6|3166|
|IOWA|cacf86ec119cfd5b574bde5b59604774de3273db|2337|
|ILLINOIS|03740763b16dae9d799097f51623fe635d8c4852|2022|
|MISSOURI|26d938907240121b54d9e039473dacc96e712f61|2016|
