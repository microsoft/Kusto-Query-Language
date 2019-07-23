---
title: Set statement - Azure Data Explorer | Microsoft Docs
description: This article describes Set statement in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 10/23/2018
---
# Set statement

The `set` statement is used to set a query option for the duration of the query.
Query options control how a query executes and returns results. They can be
Boolean flags (off by default), or have some integer value. A query may contain
zero, one, or more set statements. Set statements affect only the tabular expression
statements that trail them in program order.

> Query options can also be enabled programmatically, by setting them in the
  `ClientRequestProperties` object. See [here](../api/netfx/request-properties.md).
  
> Query options are not formally a part of the Kusto language, and may be
  modified without being considered as a breaking language change.

**Syntax**

`set` *OptionName* [`=` *OptionValue*]

**Example**

```kusto
set querytrace;
Events | take 100
```