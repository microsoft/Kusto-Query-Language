---
title: getmonth() - Azure Data Explorer | Microsoft Docs
description: This article describes getmonth() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 07/18/2019
---
# getmonth()

Get the month number (1-12) from a datetime.

Another alias: monthoyear()

**Example**

```kusto
T 
| extend month = getmonth(datetime(2015-10-12))
// month == 10
```