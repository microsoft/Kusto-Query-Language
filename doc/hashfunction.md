---
title:  hash()
description: Learn how to use the hash() function to return the hash value of the input.
ms.reviewer: alexans
ms.topic: reference
ms.date: 12/25/2022
---
# hash()

Returns a hash value for the input value.

> [!NOTE]
>
> * The function calculates hashes using the xxhash64 algorithm, but this may change. It's recommended to only use this function within a single query.
> * If you need to persist a combined hash, it's recommended to use [hash_sha256()](sha256hashfunction.md), [hash_sha1()](sha1-hash-function.md), or [hash_md5()](md5hashfunction.md) and combine the hashes with a [bitwise operator](binoperators.md). These functions are more complex to calculate than `hash()`.

## Syntax

`hash(`*source* [`,` *mod*]`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *source* | scalar | &check; | The value to be hashed.|
| *mod* | int | | A modulo value to be applied to the hash result, so that the output value is between `0` and `mod - 1`. This parameter is useful for limiting the range of possible output values or for compressing the output of the hash function into a smaller range.|

## Returns

The hash value of *source*. If *mod* is specified, the function returns the hash value modulo the value of *mod*, meaning that the output of the function will be the remainder of the hash value divided by *mod*. The output will be a value between `0` and `mod - 1`, inclusive.

## Examples

### String input

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUShKLS7NKbHNSCzO0FAKzy/KSVHSBADZZgKmGgAAAA==" target="_blank">Run the query</a>

```kusto
print result=hash("World")
```

|result|
|--|
|1846988464401551951|

### String input with mod

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUShKLS7NKbHNSCzO0FAKzy/KSVHSUTA0MNAEAJfnV8cfAAAA" target="_blank">Run the query</a>

```kusto
print result=hash("World", 100)
```

|result|
|--|
|51|

### Datetime input

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUShKLS7NKbHNSCzO0EhJLEktycxN1VAyMjA01TUwBCIlTU0AvUZeXikAAAA=" target="_blank">Run the query</a>

```kusto
print result=hash(datetime("2015-01-01"))
```

|result|
|--|
|1380966698541616202|

### Use hash to check data distribution

Use the `hash()` function for sampling data if the values in one of its columns is uniformly distributed. In the following example, *StartTime* values are uniformly distributed and the function is used to run a query on 10% of the data.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAz2NOw6DMBBEe04xJZYooKB0FaVOYS7gwCJTGCPvkgjE4bFBohpp3nyMhOjfP5qFURz4O4oEZ9mVRmyUbvJUoakVtEadArx6b+O0E0xuvsI6CzT6rKWq0G0LfcaLcfKHG1wHGSl8t9S0QvlNwoL2dp6tgbg/AeFM8LmVAAAA" target="_blank">Run the query</a>

```kusto
StormEvents 
| where hash(StartTime, 10) == 0
| summarize StormCount = count(), TypeOfStorms = dcount(EventType) by State 
| top 5 by StormCount desc
```
