---
title:  parse_json() function
description: Learn how to use the parse_json() function to return an object of type `dynamic`.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/08/2023
---
# parse_json()

Interprets a `string` as a JSON value and returns the value as `dynamic`. If possible, the value is converted into relevant [data types](scalar-data-types/index.md).  For strict parsing with no data type conversion, use [extract()](extractfunction.md) or [extract_json()](extractjsonfunction.md) functions.

It's better to use the parse_json() function over the [extract_json()](./extractjsonfunction.md) function when you need to extract more than one element of a JSON compound object. Use [dynamic()](./scalar-data-types/dynamic.md) when possible.

> **Deprecated aliases:** parsejson(), toobject(), todynamic()

## Syntax

`parse_json(`*json*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *json* | string | &check; | The string in the form of a [JSON-formatted value](https://json.org/) or a [dynamic](./scalar-data-types/dynamic.md) property bag to parse as JSON.|

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

then the following query retrieves the value of the `duration` slot
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
