---
title: parse_json() - Azure Data Explorer | Microsoft Docs
description: This article describes parse_json() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 01/10/2019
---
# parse_json()

Interprets a `string` as a JSON value and returns the value as `dynamic`. 

It is superior to using [extractjson() function](./extractjsonfunction.md)
when you need to extract more than one element of a JSON compound object.

**Syntax**

`parse_json(`*json*`)`

Aliases:
- [todynamic()](./todynamicfunction.md)
- [toobject()](./todynamicfunction.md)

**Arguments**

* *json*: An expression of type `string`, representing a [JSON-formatted value](https://json.org/),
  or an expression of type [dynamic](./scalar-data-types/dynamic.md), representing the actual `dynamic` value.

**Returns**

An object of type `dynamic` that is determined by the value of *json*:
* If *json* is of type `dynamic`, its value is used as-is.
* If *json* is of type `string`, and is a [properly-formatted JSON string](https://json.org/),
  the string is parsed and the value produced is returned.
* If *json* is of type `string`, but it is **not** a [properly-formatted JSON string](https://json.org/),
  then the returned value is an object of type `dynamic` that holds the original
  `string` value.

**Example**

In the following example, when `context_custom_metrics` is a `string`
that looks like this: 

```
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

It is somewhat common to have a JSON string describing a property bag in which
one of the "slots" is another JSON string. For example:

```kusto
let d='{"a":123, "b":"{\\"c\\":456}"}';
print d
```

In such cases, it is not only necessary to invoke `parse_json` twice, but also
to make sure that in the second call, `tostring` will be used. Otherwise, the
second call to `parse_json` will simply pass-on the input to the output as-is,
because its declared type is `dynamic`:

```kusto
let d='{"a":123, "b":"{\\"c\\":456}"}';
print d_b_c=parse_json(tostring(parse_json(d).b)).c
```