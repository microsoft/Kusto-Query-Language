---
title:  Syntax conventions for reference documentation
description: Learn about the syntax conventions for the Kusto Query Language and management command documentation.
ms.topic: reference
ms.date: 04/03/2023
---
# Syntax conventions for reference documentation

This article outlines the syntax conventions followed in the [Kusto Query Language (KQL)](index.md) and [management commands](../management/index.md) reference documentation.

## Syntax conventions

|Convention|Description|
|--|--|
|`Block`|String literals to be entered exactly as shown.|
|*Italic*|Parameters to be provided a value upon use of the function or command.|
|[ ] (square brackets)|Denotes that the enclosed item is optional.|
|[`,` ...]|Indicates that the preceding parameter can be repeated multiple times, separated by commas.|
|\| (pipe)|Indicates that you can only use one of the syntax items separated by the pipe(s).|
|`;`|Query statement terminator.|

## Examples

### Scalar function

This example shows the syntax and an example usage of the [hash function](hashfunction.md), followed by an explanation of how each syntax component translates into the example usage.

#### Syntax

`hash(`*source* [`,` *mod*]`)`

#### Example usage

```kusto
hash("World")
```

* The name of the function, `hash`, and the opening parenthesis are entered exactly as shown.
* "World" is passed as an argument for the required *source* parameter.
* No argument is passed for the *mod* parameter, which is optional as indicated by the square brackets.
* The closing parenthesis is entered exactly as shown.

### Tabular operator

This example shows the syntax and an example usage of the [sort operator](sort-operator.md), followed by an explanation of how each syntax component translates into the example usage.

#### Syntax

*T* `| sort by` *column* [`asc` | `desc`] [`nulls first` | `nulls last`] [`,` ...]

#### Example usage

```kusto
StormEvents
| sort by State asc, StartTime desc
```

* The StormEvents table is passed as an argument for the required *T* parameter.
* `| sort by` is entered exactly as shown. In this case, the pipe character is part of the [tabular expression statement](tabularexpressionstatements.md) syntax, as represented by the block text. To learn more, see [What is a query statement](index.md#what-is-a-query-statement).
* The State column is passed as an argument for the required *column* parameter with the optional `asc` flag.
* After a comma, another set of arguments is passed: the StartTime column with the optional `desc` flag. The [`,` ...] syntax indicates that more argument sets may be passed but aren't required.

## Working with optional parameters

To provide an argument for an optional parameter that comes after another optional parameter, you must provide an argument for the prior parameter. This requirement is because arguments must follow the order specified in the syntax. If you don't have a specific value to pass for the parameter, use an empty value of the same type.

### Example of sequential optional parameters

Consider the syntax for the [http_request plugin](http-request-plugin.md):

`evaluate` `http_request` `(` *Uri* [`,` *RequestHeaders* [`,` *Options*]] `)`

*RequestHeaders* and *Options* are optional parameters of type [dynamic](scalar-data-types/dynamic.md). To provide an argument for the *Options* parameter, you must also provide an argument for the *RequestHeaders* parameter. The following example shows how to provide an empty value for the first optional parameter, *RequestHeaders*, in order to be able to specify a value for the second optional parameter, *Options*.

```kusto
evaluate http_request ( "https://contoso.com/", dynamic({}), dynamic({ EmployeeName: Nicole }) )
```

## See also

* [KQL overview](index.md)
* [KQL quick reference](../../kql-quick-reference.md)
