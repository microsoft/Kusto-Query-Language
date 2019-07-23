---
title: countof() - Azure Data Explorer | Microsoft Docs
description: This article describes countof() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 10/23/2018
---
# countof()

Counts occurrences of a substring in a string. Plain string matches may overlap; regex matches do not.

```kusto
countof("The cat sat on the mat", "at") == 3
countof("The cat sat on the mat", @"\b.at\b", "regex") == 3
```

**Syntax**

`countof(`*text*`,` *search* [`,` *kind*]`)`

**Arguments**

* *text*: A string.
* *search*: The plain string or [regular expression](./re2.md) to match inside *text*.
* *kind*: `"normal"|"regex"` Default `normal`. 

**Returns**

The number of times that the search string can be matched in the container. Plain string matches may overlap; regex matches do not.

**Examples**

|||
|---|---
|`countof("aaa", "a")`| 3 
|`countof("aaaa", "aa")`| 3 (not 2!)
|`countof("ababa", "ab", "normal")`| 2
|`countof("ababa", "aba")`| 2
|`countof("ababa", "aba", "regex")`| 1
|`countof("abcabc", "a.c", "regex")`| 2
    