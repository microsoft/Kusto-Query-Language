---
title: degrees() - Azure Data Explorer | Microsoft Docs
description: This article describes degrees() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# degrees()

Converts angle value in radians into value in degrees, using formula `degrees = (180 / PI ) * angle_in_radians`

## Syntax

`degrees(`*a*`)`

## Arguments

* *a*: Angle in radians (a real number).

## Returns

* The corresponding angle in degrees for an angle specified in radians. 

## Examples

```kusto
print degrees0 = degrees(pi()/4), degrees1 = degrees(pi()*1.5), degrees2 = degrees(0)

```

|degrees0|degrees1|degrees2|
|---|---|---|
|45|270|0|
