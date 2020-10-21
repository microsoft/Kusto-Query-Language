---
title: radians() - Azure Data Explorer | Microsoft Docs
description: This article describes radians() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# radians()

Converts angle value in degrees into value in radians, using formula `radians = (PI / 180 ) * angle_in_degrees`

## Syntax

`radians(`*a*`)`

## Arguments

* *a*: Angle in degrees (a real number).

## Returns

* The corresponding angle in radians for an angle specified in degrees. 

## Examples

```kusto
print radians0 = radians(90), radians1 = radians(180), radians2 = radians(360) 

```

|radians0|radians1|radians2|
|---|---|---|
|1.5707963267949|3.14159265358979|6.28318530717959|