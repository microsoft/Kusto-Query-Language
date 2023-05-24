---
title:  treepath()
description: This article describes treepath() in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 02/15/2023
---
# treepath()

Enumerates all the path expressions that identify leaves in a dynamic object.

## Syntax

`treepath(`*object*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *object* | dynamic | &check;| A dynamic property bag object for which to enumerate the path expressions.|

## Returns

An array of path expressions.

## Examples

|Expression|Evaluates to|
|---|---|
|`treepath(parse_json('{"a":"b", "c":123}'))` | `["['a']","['c']"]`|
|`treepath(parse_json('{"prop1":[1,2,3,4], "prop2":"value2"}'))`|`["['prop1']","['prop1'][0]","['prop2']"]`|
|`treepath(parse_json('{"listProperty":[100,200,300,"abcde",{"x":"y"}]}'))`|`["['listProperty']","['listProperty'][0]","['listProperty'][0]['x']"]`|
