---
title: bag_zip() - Azure Data Explorer 
description: Learn how to use bag_zip() to merge two dynamic arrays into a single property-bag of keys and values.
ms.reviewer: elgevork
ms.topic: reference
ms.date: 05/01/2023
---
# bag_zip()

Creates a dynamic property-bag from two input dynamic arrays. In the resulting property-bag, the values from the first input array are used as the property keys, while the values from the second input array are used as corresponding property values.

## Syntax

`bag_zip(`*KeysArray*`,` *ValuesArray*`)`

## Parameters

|Name|Type|Required|Description|
|--|--|--|--|
| *KeysArray* | dynamic | &check; | An array of strings. These strings represent the property names for the resulting property-bag.|
| *ValuesArray* | dynamic | &check; | An array whose values will be the property values for the resulting property-bag.|

> [!NOTE]
>
> * If there are more keys than values, missing values are filled with null.
> * If there are more values than keys, values with no matching keys are ignored.
> * Keys that aren't strings are ignored.

## Returns

Returns a [dynamic](scalar-data-types/dynamic.md) property-bag.

## Examples

In the following example, the array of keys and the array of values are the same length and are zipped together into a dynamic property bag.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA8tJLVFwSSxJVLBVSAFSJYlJOaka3qmVxY5FRYmVVgoplXmJuZnJOgphiTmlqWiimgrRvFwKQADla0SrJ6rrKKgngYhk9VhNHYSMIVDICChurGcSq8nLFWvNywWyl5erRiG1oiQ1L0XBL7XcKTEd6JCkxPT4qswChDNQbNcEAJQm8tKzAAAA" target="_blank">Run the query</a>
```kusto
let Data = datatable(KeysArray: dynamic, ValuesArray: dynamic) [
    dynamic(['a', 'b', 'c']), dynamic([1, '2', 3.4])
];
Data
| extend NewBag = bag_zip(KeysArray, ValuesArray)
```

| KeysArray | ValuesArray | NewBag |
|--|--|--|
| ['a','b','c'] | [1,'2',3.4] | {'a': 1,'b': '2','c': 3.4} |

### More keys than values

In the following example, the array of keys is longer than the array of values. The missing values are filled with nulls.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA8tJLVFwSSxJVLBVSAFSJYlJOaka3qmVxY5FRYmVVgoplXmJuZnJOgphiTmlqWiimgrRvFwKQADla0SrJ6rrKKgngYhk9VhNHYSMIVDICCjEyxVrzcsFspKXq0YhtaIkNS9FwS+13CkxHeiGpMT0+KrMAoQLUCzWBAB4QDzurgAAAA==" target="_blank">Run the query</a>

```kusto
let Data = datatable(KeysArray: dynamic, ValuesArray: dynamic) [
    dynamic(['a', 'b', 'c']), dynamic([1, '2'])
];
Data
| extend NewBag = bag_zip(KeysArray, ValuesArray)
```

| KeysArray | ValuesArray | NewBag |
|--|--|--|
| ['a','b','c'] | [1,'2'] | {'a': 1,'b': '2','c': null} |

### More values than keys

In the following example, the array of values is longer than the array of keys. Values with no matching keys are ignored.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA8tJLVFwSSxJVLBVSAFSJYlJOaka3qmVxY5FRYmVVgoplXmJuZnJOgphiTmlqWiimgrRvFwKQADla0SrJ6rrKKgnqcdq6iAEDYFCRkBxIz3TWE1erlhrXi6QlbxcNQqpFSWpeSkKfqnlTonpQDckJabHV2UWIFyAYrEmAMOF9yWuAAAA" target="_blank">Run the query</a>

```kusto
let Data = datatable(KeysArray: dynamic, ValuesArray: dynamic) [
    dynamic(['a', 'b']), dynamic([1, '2', 2.5])
];
Data
| extend NewBag = bag_zip(KeysArray, ValuesArray)
```

| KeysArray | ValuesArray | NewBag |
|--|--|--|
| ['a','b'] | [1,'2',2.5] | {'a': 1,'b': '2'} |

### Non-string keys

In the following example, there are some values in they keys array that aren't of type string. The non-string values are ignored.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA8tJLVFwSSxJVLBVSAFSJYlJOaka3qmVxY5FRYmVVimVeYm5mck6CmGJOaWpqIIKmgrRUKZGtHqiuo6FjnqSeqymjgJc1FBHQd1IXUfBSM80VjPWmpcLZBUvV41CakVJal6Kgl9quVNiOtDupMT0+KrMAoTNKDZqAgBG9LZkpgAAAA==" target="_blank">Run the query</a>

```kusto
let Data = datatable(KeysArray: dynamic, ValuesArray: dynamic) [
    dynamic(['a', 8, 'b']), dynamic([1, '2', 2.5])
];
Data
| extend NewBag = bag_zip(KeysArray, ValuesArray)
```

| KeysArray | ValuesArray | NewBag |
|--|--|--|
| ['a',8,'b'] | [1,'2',2.5] | {'a': 1,'b': 2.5} |

### Fill values with null

In the following example, the parameter that is supposed to be an array of values isn't an array, so all values are filled with nulls.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA8tJLVFwSSxJVLBVSAFSJYlJOaka3qmVxY5FRYmVVgoplXmJuZnJOgphiTmlqWiimgrRvFwKQADla0SrJ6rrKFjoKKgnqcdq6sDFDTV5uWKteblANvFy1SikVpSk5qUo+KWWOyWmA61OSkyPr8osQFiMYp8mAJSpx+ClAAAA" target="_blank">Run the query</a>

```kusto
let Data = datatable(KeysArray: dynamic, ValuesArray: dynamic) [
    dynamic(['a', 8, 'b']), dynamic(1)
];
Data
| extend NewBag = bag_zip(KeysArray, ValuesArray)
```

| KeysArray | ValuesArray | NewBag |
|--|--|--|
| ['a',8,'b'] | 1 | {'a': null,'b': null} |

### Null property-bag

In the following example, the parameter that is supposed to be an array of keys isn't an array, so the resulting property-bag is null.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA8tJLVFwSSxJVLBVSAFSJYlJOaka3qmVxY5FRYmVVgoplXmJuZnJOgphiTmlqWiimgrRvFwKQADla6gnqmvqwHnRhjoK6kbqOgpGeqaxmrxcsda8XCC7eLlqFFIrSlLzUhT8UsudEtOBliclpsdXZRYgrEaxURNJi2cxRJNrbkFJpW1mcV5pTo4GREgTACOl4ijOAAAA" target="_blank">Run the query</a>

```kusto
let Data = datatable(KeysArray: dynamic, ValuesArray: dynamic) [
    dynamic('a'), dynamic([1, '2', 2.5])
];
Data
| extend NewBag = bag_zip(KeysArray, ValuesArray)
| extend IsNewBagEmpty=isnull(NewBag)
```

| KeysArray | ValuesArray | NewBag | IsNewBagEmpty |
|--|--|--|
| a | [1,'2',2.5] | | TRUE |

## See also

* [zip function](zipfunction.md)
