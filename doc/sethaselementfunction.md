---
title: set_has_element() - Azure Data Explorer
description: Learn how to use the set_has_element() function to determine if the input set contains the specified value.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/30/2023
---
# set_has_element()

Determines whether the specified set contains the specified element.

## Syntax

`set_has_element(`*set*`,` *value*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *set* | dynamic | &check; | The input array to search.|
| *value* | | &check; | The value for which to search. The value should be of type `long`, `int`, `double`, `datetime`, `timespan`, `decimal`, `string`, `guid`, or `bool`.|

## Returns

`true` or `false` depending on if the value exists in the array.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUUgsKrJNqcxLzM1M1ohWKsnILFbSUVCCkIl5IDK1IjG3ICdVKVZTgatGoaAoPys1uUQhKLW4NKfEtji1JD4jsTg+NSc1NzWvRANoHJIWTQCSW+h8ZAAAAA==" target="_blank">Run the query</a>

```kusto
print arr=dynamic(["this", "is", "an", "example"]) 
| project Result=set_has_element(arr, "example")
```

**Output**

|Result|
|---|
|true|

## See also

Use [`array_index_of(arr, value)`](arrayindexoffunction.md) to find the position at which the value exists in the array. Both functions are equally performant.
