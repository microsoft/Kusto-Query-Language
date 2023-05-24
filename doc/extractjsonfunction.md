---
title:  extract_json()
description: Learn how to use the extract_json() function to get a specified element out of a JSON text using a path expression.
ms.reviewer: alexans
ms.topic: reference
ms.date: 03/09/2023
---
# extract_json()

Get a specified element out of a JSON text using a path expression.

Optionally convert the extracted string to a specific type.

> The `extract_json()` and `extractjson()` functions are equivalent

```kusto
extract_json("$.hosts[1].AvailableMB", EventText, typeof(int))
```

## Syntax

`extract_json(`*jsonPath*`,` *dataSource*`,` *type*`)`

## Parameters

|Name|Type|Required|Description|
|--|--|--|--|
| *jsonPath* | string | &check; | A [JSONPath](jsonpath.md) that defines an accessor into the JSON document.|
| *dataSource* | string | &check; | A JSON document.|
| *type* | string | | An optional type literal. If provided, the extracted value is converted to this type. For example, `typeof(long)` will convert the extracted value to a `long`.|

## Performance tips

* Apply where-clauses before using `extract_json()`.
* Consider using a regular expression match with [extract](extractfunction.md) instead. This can run very much faster, and is effective if the JSON is produced from a template.
* Use `parse_json()` if you need to extract more than one value from the JSON.
* Consider having the JSON parsed at ingestion by declaring the type of the column to be [dynamic](scalar-data-types/dynamic.md).

## Returns

This function performs a [JSONPath](jsonpath.md) query into dataSource, which contains a valid JSON string, optionally converting that value to another type depending on the third argument.

## Example

The `[`bracket`]` notation and dot (`.`) notation are equivalent:

```kusto
T
| extend AvailableMB = extract_json("$.hosts[1].AvailableMB", EventText, typeof(int))

T
| extend AvailableMB = extract_json("$['hosts'][1]['AvailableMB']", EventText, typeof(int))
```
