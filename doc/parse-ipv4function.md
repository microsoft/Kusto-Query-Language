---
title: parse_ipv4() - Azure Data Explorer | Microsoft Docs
description: This article describes parse_ipv4() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 10/23/2018
---
# parse_ipv4()

Converts input to integer (signed 64-bit) number representation.

```kusto
parse_ipv4("127.0.0.1") == 2130706433
parse_ipv4('192.1.168.1') < parse_ipv4('192.1.168.2') == true
```

**Syntax**

`parse_ipv4(`*Expr*`)`

**Arguments**

* *Expr*: Expression that will be converted to long. 

**Returns**

If conversion is successful, result will be a long number.
If conversion is not successful, result will be `null`.
 