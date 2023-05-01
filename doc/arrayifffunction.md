---
title: array_iff() - Azure Data Explorer
description: Learn how to use the array_iff() function to scan and evaluate elements in an array.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/09/2023
---
# array_iff()

Element-wise iif function on dynamic arrays.

> The `array_iff()` and `array_iif()` functions are equivalent

## Syntax

`array_iff(`*condition_array*, *when_true*, *when_false*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *condition_array*| dynamic | &check;| An array of *boolean* or numeric values.|
| *when_true* | dynamic or scalar | &check; | An array of values or primitive value. This will be the result when *condition_array* is *true*.|
| *when_false* | dynamic or scalar | &check; | An array of values or primitive value. This will be the result when *condition_array* is *false*.|

> [!NOTE]
>
> * The length of the return value will be the same as the input *condition_array*.
> * Numeric condition values are considered `true` if not equal to 0.
> * Non-numeric and non-boolean condition values will be null in the corresponding index of the return value.
> * If *when_true* or *when_false* is shorter than *condition_array*, missing values will be treated as null.

## Returns

Returns a dynamic array of the values taken either from the *when_true* or *when_false* array values, according to the corresponding value of the condition array.

## Examples

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUUjOz0vJLMnMz7NNqcxLzM1M1oguKSpN1UlLzClO1QExYzV1FHIQsoY6RjrGILEihJiJjqmOWaymAi9XjUJqRUlqXopCUWqxbWJRUWJlfGZmmgbcFqBRQJ2aACda2uZ8AAAA" target="_blank">Run the query</a>

```kusto
print condition=dynamic([true,false,true]), if_true=dynamic([1,2,3]), if_false=dynamic([4,5,6]) 
| extend res= array_iff(condition, if_true, if_false)
```

**Output**

|condition|if_true|if_false|res|
|---|---|---|---|
|[true, false, true]|[1, 2, 3]|[4, 5, 6]|[1, 5, 3]|

### Numeric condition values

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUUjOz0vJLMnMz7NNqcxLzM1M1og21DHQMTWI1dRRyEyLLykqTbVVqkwtVgJz0xJzioH8vHwlBa4ahdSKktS8FIWi1GLbxKKixMr4zMw0DbiJcP0InZoAPCLjbHUAAAA=" target="_blank">Run the query</a>

```kusto
print condition=dynamic([1,0,50]), if_true="yes", if_false="no" 
| extend res= array_iff(condition, if_true, if_false)
```

**Output**

|condition|if_true|if_false|res|
|---|---|---|---|
|[1, 0, 50]|yes|no|[yes, no, yes]|

### Non-numeric and non-boolean condition values

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA0WNwQrDIBBE7/2KxZOBFIx3v6SUsMS1LOhadFMa6MdXekhhYAYej3k2FoWtSmTlKiEegoU3ezO9FoKugz/ghXknM0NEJeVC1rjlOuKd92aaQfac76M5rdp2CstvJsydgrt8gN5KEqFRD9gaHitzsufp6f2t6QsxYYlomAAAAA==" target="_blank">Run the query</a>

```kusto
print condition=dynamic(["some string value", datetime("01-01-2022"), null]), if_true=1, if_false=0
| extend res= array_iff(condition, if_true, if_false)
```

**Output**

|condition|if_true|if_false|res|
|---|---|---|---|
|[true, false, true]|1|0|[null, null, null]|

### Mismatched array lengths

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUUjOz0vJLMnMz7NNqcxLzM1M1oguKSpN1YETsZo6Cplp8SAmQomhjhFUPC0xpxhJwljHJFZTgatGIbWiJDUvRaEotdg2sagosTI+MzNNA24Z3EiEGZoACxaCE5AAAAA=" target="_blank">Run the query</a>

```kusto
print condition=dynamic([true,true,true]), if_true=dynamic([1,2]), if_false=dynamic([3,4]) 
| extend res= array_iff(condition, if_true, if_false)
```

**Output**

|condition|if_true|if_false|res|
|---|---|---|---|
|[true, true, true]|[1, 2]|[3, 4]|[1, 2, null]|
