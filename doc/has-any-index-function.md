---
title: "The has_any_index operator - Azure Data Explorer"
description: "This article describes the has_any_index in Azure Data Explorer."
ms.reviewer: atefsawaed
ms.topic: reference
ms.date: 12/22/2021
---
# has_any_index()

Searches the string for items specified in the array and returns the position in the array of the first item found in the string. `has` searches for indexed terms, where a [term](datatypes-string-operators.md#what-is-a-term) is three or more characters. If your term is fewer than three characters, the query scans the values in the column, which is slower than looking up the term in the term index.

## Syntax

`has_any_index` `(`*string*`,` *lookup_array*`)`

## Arguments

* *string*: Input string to search.
* *lookup_array*: Array of scalar or literal expressions to look up. The value should be of type long, integer, double, decimal, string, or guid.

## Returns

Zero-based index position of the first item in *lookup_array* that is found in *string*.
Returns -1 if none of the array items were found in the string or if *lookup_array* is empty.

## Example

```kusto
print
 idx1 = has_any_index("this is an example", dynamic(['this', 'example']))  // first lookup found in input string
 , idx2 = has_any_index("this is an example", dynamic(['not', 'example'])) // last lookup found in input string
 , idx3 = has_any_index("this is an example", dynamic(['not', 'found'])) // no lookup found in input string
 , idx4 = has_any_index("Example number 2", range(1, 3, 1)) // Lookup array of integers
 , idx5 = has_any_index("this is an example", dynamic([]))  // Empty lookup array
```

|idx1|idx2|idx3|idx4|idx5|
|----|----|----|----|----|
| 0  | 1 | -1 |1 | -1 |
