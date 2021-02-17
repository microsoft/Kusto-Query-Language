---
title: distinct operator - Azure Data Explorer | Microsoft Docs
description: This article describes distinct operator in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
ms.localizationpriority: high
---
# distinct operator

Produces a table with the distinct combination of the provided columns of the input table. 

```kusto
T | distinct Column1, Column2
```

Produces a table with the distinct combination of all columns in the input table.

```kusto
T | distinct *
```

## Example

Shows the distinct combination of fruit and price.

```kusto
Table | distinct fruit, price
```

:::image type="content" source="images/distinctoperator/distinct.PNG" alt-text="Two tables. One has suppliers, fruit types, and prices, with some fruit-price combinations repeated. The second table lists only unique combinations.":::

**Notes**

* Unlike `summarize by ...`, the `distinct` operator supports providing an asterisk (`*`) as the group key, making it easier to use for wide tables.
* If the group by keys are of high cardinalities, using `summarize by ...` with the [shuffle strategy](shufflequery.md) could be useful.