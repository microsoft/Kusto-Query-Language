---
title:  Numerical operators
description: Learn how to use Numerical operators to calculate a value from two or more numbers.
ms.reviewer: alexans
ms.topic: reference
ms.date: 05/16/2023
---
# Numerical operators

The types `int`, `long`, and `real` represent numerical types.
The following operators can be used between pairs of these types:

Operator       |Description                         |Example
---------------|------------------------------------|-----------------------
`+`	           |Add                                 |`3.14 + 3.14`, `ago(5m) + 5m`
`-`	           |Subtract                            |`0.23 - 0.22`,
`*`            |Multiply                            |`1s * 5`, `2 * 2`
`/`	           |Divide                              |`10m / 1s`, `4 / 2`
`%`            |Modulo                              |`4 % 2`
`<`	           |Less                                |`1 < 10`, `10sec < 1h`, `now() < datetime(2100-01-01)`
`>`	           |Greater                             |`0.23 > 0.22`, `10min > 1sec`, `now() > ago(1d)`
`==`           |Equals                              |`1 == 1`
`!=`	       |Not equals                          |`1 != 0`
`<=`           |Less or Equal                       |`4 <= 5`
`>=`           |Greater or Equal                    |`5 >= 4`
`in`           |Equals to one of the elements       |[see here](inoperator.md)
`!in`          |Not equals to any of the elements   |[see here](inoperator.md)

> [!NOTE]
> To convert from one numerical type to another, use `to*()` functions. For example, see [`tolong()`](tolongfunction.md) and [`toint()`](tointfunction.md).

## Type rules for arithmetic operations

The data type of the result of an arithmetic operation is determined by the data types of the operands. If one of the operands is of type `real`, the result will be of type `real`. If both operands are of type `int`, the result will also be of type `int`.

Due to these rules, the result of division operations that only involve integers will be truncated to an integer, which may not always be what you want. To avoid truncation, convert at least one of the `int` values to `real` using the [real() function](scalar-data-types/real.md) before performing the operation.

The following examples illustrate how the operand types affect the result type in division operations.

| Operation | Result | Description |
|--|--|--|
| `1.0` `/` `2` | `0.5` | One of the operands is of type `real`, so the result is `real`. |
| `1` `/` `2.0` | `0.5` | One of the operands is of type `real`, so the result is `real`. |
| `1` `/` `2` | `0` | Both of the operands are of type `int`, so the result is `int`. Integer division occurs and the decimal is truncated, resulting in `0` instead of `0.5`, as one might expect. |
| `real(1)` `/` `2` | `0.5` | To avoid truncation due to integer division, one of the `int` operands was first converted to `real` using the `real()` function. |

## Comment about the modulo operator

The modulo of two numbers always returns in Kusto a "small non-negative number".
Thus, the modulo of two numbers, *N* % *D*, is such that:
0 &le; (*N* % *D*) &lt; abs(*D*).

For example, the following query:

```kusto
print plusPlus = 14 % 12, minusPlus = -14 % 12, plusMinus = 14 % -12, minusMinus = -14 % -12
```

Produces this result:

|plusPlus  | minusPlus  | plusMinus  | minusMinus|
|----------|------------|------------|-----------|
|2         | 10         | 2          | 10        |
