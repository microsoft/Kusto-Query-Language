---
title: extractjson() - Azure Data Explorer | Microsoft Docs
description: This article describes extractjson() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 08/29/2021
---
# extractjson()

Get a specified element out of a JSON text using a path expression.

Optionally convert the extracted string to a specific type.

```kusto
extractjson("$.hosts[1].AvailableMB", EventText, typeof(int))
```

## Syntax

`extractjson(`*jsonPath*`,` *dataSource*`, ` *type*`)`

## Arguments

* *jsonPath*: [JSONPath](jsonpath.md) string that defines an accessor into the JSON document.
* *dataSource*: A JSON document.
* *type*: An optional type literal (for example, typeof(long)). If provided, the extracted value is converted to this type.

## Returns

This function performs a [JSONPath](jsonpath.md) query into dataSource, which contains a valid JSON string, optionally converting that value to another type depending on the third argument.

## Example

The `[`bracket`]` notation and dot (`.`) notation are equivalent:

```kusto
T
| extend AvailableMB = extractjson("$.hosts[1].AvailableMB", EventText, typeof(int))

T
| extend AvailableMD = extractjson("$['hosts'][1]['AvailableMB']", EventText, typeof(int))
```

**Performance tips**

* Apply where-clauses before using `extractjson()`
* Consider using a regular expression match with [extract](extractfunction.md) instead. This can run very much faster, and is effective if the JSON is produced from a template.
* Use `parse_json()` if you need to extract more than one value from the JSON.
* Consider having the JSON parsed at ingestion by declaring the type of the column to be dynamic.
