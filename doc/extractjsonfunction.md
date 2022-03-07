---
title: extract_json() and extractjson() - Azure Data Explorer
description: This article describes extract_json() and extractjson() in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 02/13/2022
---
# extract_json(), extractjson()

Get a specified element out of a JSON text using a path expression.

Optionally convert the extracted string to a specific type.

> [!NOTE]
> The `extract_json()` and `extractjson()` functions are interpreted equivalently.

```kusto
extract_json("$.hosts[1].AvailableMB", EventText, typeof(int))
```

## Syntax

`extract_json(`*jsonPath*`,` *dataSource*`, ` *type*`)` 
 
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
| extend AvailableMB = extract_json("$.hosts[1].AvailableMB", EventText, typeof(int))

T
| extend AvailableMD = extract_json("$['hosts'][1]['AvailableMB']", EventText, typeof(int))
```

**Performance tips**

* Apply where-clauses before using `extract_json()`
* Consider using a regular expression match with [extract](extractfunction.md) instead. This can run very much faster, and is effective if the JSON is produced from a template.
* Use `parse_json()` if you need to extract more than one value from the JSON.
* Consider having the JSON parsed at ingestion by declaring the type of the column to be dynamic.
