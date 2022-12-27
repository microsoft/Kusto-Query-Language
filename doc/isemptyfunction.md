---
title: isempty() - Azure Data Explorer
description: Learn how to use the isempty() function to check if the argument is an empty string.
ms.reviewer: alexans
ms.topic: reference
ms.date: 12/21/2022
---
# isempty()

Returns `true` if the argument is an empty string or is null.

```kusto
isempty("") == true
```

## Syntax

`isempty(`[*value*]`)`

## Returns

Indicates whether the argument is an empty string or is null.

|x|isempty(x)
|---|---
| "" | true
|"x" | false
|parsejson("")|true
|parsejson("[]")|false
|parsejson("{}")|false

## Example

```kusto
T
| where isempty(fieldName)
| count
```
