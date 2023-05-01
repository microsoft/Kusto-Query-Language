---
title: isempty() - Azure Data Explorer
description: Learn how to use the isempty() function to check if the argument is an empty string.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/03/2023
---
# isempty()

Returns `true` if the argument is an empty string or is null.

## Syntax

`isempty(`*value*`)`

## Parameters

| Name | Type | Required | Description |
| -- | -- | -- | -- |
|*value*|string|&check;| The value to check if empty or null.|

## Returns

A boolean value indicating whether *value* is an empty string or is null.

## Example

|x|isempty(x)|
|---|---|
| "" | true|
|"x" | false|
|parsejson("")|true|
|parsejson("[]")|false|
|parsejson("{}")|false|
