---
title: getyear() - Azure Data Explorer | Microsoft Docs
description: This article describes getyear() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 10/23/2018
---
# getyear()

Returns the year part of the `datetime` argument.

**Example**

```kusto
T
| extend year = getyear(datetime(2015-10-12))
// year == 2015
```