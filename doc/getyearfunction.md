---
title: getyear() - Azure Data Explorer
description: Learn how tow use the getyear() function to return the year of the `datetime` input.
ms.reviewer: alexans
ms.topic: reference
ms.date: 12/18/2022
---
# getyear()

Returns the year part of the `datetime` argument.

## Example

```kusto
T
| extend year = getyear(datetime(2015-10-12))
// year == 2015
```
