---
title:  The has_any_index operator
description: Learn how to use the has_any_index operator to search the input string for items specified in the array.
ms.reviewer: atefsawaed
ms.topic: reference
ms.date: 12/18/2022
---
# has_any_index()

Searches the string for items specified in the array and returns the position in the array of the first item found in the string. `has_any_index` searches for indexed terms, where an indexed [term](datatypes-string-operators.md#what-is-a-term) is three or more characters. If your term is fewer than three characters, the query scans the values in the column, which is slower than looking up the term in the term index.

## Syntax

`has_any_index` `(`*source*`,` *values*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *source*| string | &check;| The value to search.|
| *values*| dynamic | &check;| An array of scalar or literal expressions to look up. |

## Returns

Zero-based index position of the first item in *values* that is found in *source*.
Returns -1 if none of the array items were found in the string or if *values* is empty.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA53RsQ6CMBDG8d2n+MICJk0IqKMjm29gDDmlYCNcm7Yk8PYWkEUXNbntkv/vmhqr2G+gqiHDEXdyJfFYKq7kkET+rhzCEEMO1JlWRgLVyNSpW3KOp3UsEL928WW7BdIUtbLOo9X60RvUuucKisOY3sP5ADYbiInMfyZZ+3cxgC195+3+9eboqrH+xtp/WsUCgPvuKi3yIFniRiaZwE4gW/KnpU3W0ghdh7aXjbTu1T38+ob1V4rO+HG9fK4/AfXYgrf8AQAA" target="_blank">Run the query</a>

```kusto
print
 idx1 = has_any_index("this is an example", dynamic(['this', 'example']))  // first lookup found in input string
 , idx2 = has_any_index("this is an example", dynamic(['not', 'example'])) // last lookup found in input string
 , idx3 = has_any_index("this is an example", dynamic(['not', 'found'])) // no lookup found in input string
 , idx4 = has_any_index("Example number 2", range(1, 3, 1)) // Lookup array of integers
 , idx5 = has_any_index("this is an example", dynamic([]))  // Empty lookup array
```

**Output**

|idx1|idx2|idx3|idx4|idx5|
|----|----|----|----|----|
| 0  | 1 | -1 |1 | -1 |
