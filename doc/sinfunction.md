---
title:  sin()
description: Learn how to use the sin() function to return the sine value of the input.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/30/2023
---
# sin()

Returns the sine function value of the specified angle. The angle is specified in radians.

## Syntax

`sin(`*number*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *number* | real | &check; | The value in radians for which to calculate the sine.|

## Returns

The sine of *number* of radians.

## Example

```kusto
print sin(1)
```

**Output**

|result|
|--|
|0.841470984807897|
