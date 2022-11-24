---
title: countof() - Azure Data Explorer
description: Learn how to use the countof() function to count the occurrences of a substring in a string.
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/23/2022
---
# countof()

Counts occurrences of a substring in a string. Plain string matches may overlap; regex matches don't.

```kusto
countof("The cat sat on the mat", "at") == 3
countof("The cat sat on the mat", @"\b.at\b", "regex") == 3
```

## Syntax

`countof(`*source*`,` *search* [`,` *kind*]`)`

## Arguments

* *source*: A string.
* *search*: The plain string or [regular expression](./re2.md) to match inside *source*.
* *kind*: `"normal"|"regex"` Default `normal`.

## Returns

The number of times that the search string can be matched in the container. Plain string matches may overlap; regex matches don't.

## Examples

|Function call|Result|
|---|---
|`countof("aaa", "a")`| 3
|`countof("aaaa", "aa")`| 3 (not 2!)
|`countof("ababa", "ab", "normal")`| 2
|`countof("ababa", "aba")`| 2
|`countof("ababa", "aba", "regex")`| 1
|`countof("abcabc", "a.c", "regex")`| 2
