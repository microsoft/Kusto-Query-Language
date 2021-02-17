---
title: todynamic(), parse_json() functions - Azure Data Explorer
description: This article describes the todynamic(), parse_json() functions in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 01/25/2021
ms.localizationpriority: high
---
# todynamic(), parse_json()

Interprets a `string` as a JSON value and returns the value as `dynamic`. 

> [!NOTE]
> The `todynamic()` and `parse_json()` functions are interpreted equivalently.

This function is better than [extractjson() function](./extractjsonfunction.md) when you need to extract more than one element of a JSON compound object. Prefer using [dynamic()](./scalar-data-types/dynamic.md) when possible.

## Syntax

`parse_json(`*json*`)`
`todynamic(`*json*`)`

<!-- deprecated aliases: `toobject()` and parsejson() -->

## Arguments

* *json*: An expression of type `string`. It represents a [JSON-formatted value](https://json.org/), or an expression of type [dynamic](./scalar-data-types/dynamic.md), representing the actual `dynamic` value.

## Returns

An object of type `dynamic` that is determined by the value of *json*:
* If *json* is of type `dynamic`, its value is used as-is.
* If *json* is of type `string`, and is a [properly formatted JSON string](https://json.org/), then the string is parsed, and the value produced is returned.
* If *json* is of type `string`, but it isn't a [properly formatted JSON string](https://json.org/), then the returned value is an object of type `dynamic` that holds the original `string` value.

## Example

In the following example, when `context_custom_metrics` is a `string`
that looks like this:

```json
{"duration":{"value":118.0,"count":5.0,"min":100.0,"max":150.0,"stdDev":0.0,"sampledValue":118.0,"sum":118.0}}
```

then the following CSL Fragment retrieves the value of the `duration` slot
in the object, and from that it retrieves two slots, `duration.value` and
 `duration.min` (`118.0` and `110.0`, respectively).

```kusto
T
| extend d=parse_json(context_custom_metrics) 
| extend duration_value=d.duration.value, duration_min=d["duration"]["min"]
```

**Notes**

It's common to have a JSON string describing a property bag in which
one of the "slots" is another JSON string. 

For example:

```kusto
let d='{"a":123, "b":"{\\"c\\":456}"}';
print d
```

In such cases, it isn't only necessary to invoke `parse_json` twice, but also
to make sure that in the second call, `tostring` is used. Otherwise, the
second call to `parse_json` will just pass on the input to the output as-is,
because its declared type is `dynamic`.

```kusto
let d='{"a":123, "b":"{\\"c\\":456}"}';
print d_b_c=parse_json(tostring(parse_json(d).b)).c
```
