---
title:  hash_xxhash64()
description: Learn how to use the hash_xxhash64() function to return the xxhash64 value of the input.
ms.reviewer: slneimer
ms.topic: reference
ms.date: 12/25/2022
---
# hash_xxhash64()

Returns an xxhash64 value for the input value.

## Syntax

`hash_xxhash64(`*source* [`,` *mod*]`)`

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
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUShKLS7NKbHNSCzOiK+oAFFmJhpK4flFOSlKmgDB9B3HIwAAAA==" target="_blank">Run the query</a>

```kusto
print result=hash_xxhash64("World")
```

|result|
|--|
|1846988464401551951|

### String input with mod

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUShKLS7NKbHNSCzOiK+oAFFmJhpK4flFOSlKOgqGBgaaAFCsEusoAAAA" target="_blank">Run the query</a>

```kusto
print result=hash_xxhash64("World", 100)
```

|result|
|--|
|51|

### Datetime input

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUShKLS7NKbHNSCzOiK+oAFFmJhopiSWpJZm5qRpKRgaGproGhkCkpKkJAN5RmvEyAAAA" target="_blank">Run the query</a>

```kusto
print result=hash_xxhash64(datetime("2015-01-01"))
```

|result|
|--|
|1380966698541616202|
