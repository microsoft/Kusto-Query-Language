---
title: rand() - Azure Data Explorer
description: This article describes rand() in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 02/13/2020
---
# rand()

Returns a random number.

```kusto
rand()
rand(1000)
```

## Syntax

* `rand()` - returns a value of type `real`
  with a uniform distribution in the range [0.0, 1.0).
* `rand(` *N* `)` - returns a value of type `real`
  chosen with a uniform distribution from the set {0.0, 1.0, ..., *N* - 1}.