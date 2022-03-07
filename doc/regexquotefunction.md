---
title: regex_quote() - Azure Data Explorer
description: This article describes regex_quote() in Azure Data Explorer.
ms.reviewer: shanisolomon
ms.topic: reference
ms.date: 05/19/2021
---
# regex_quote()

Returns a string that escapes all regular expression characters.

## Syntax

`regex_quote(`*value*`)`

## Arguments

*value*: The string to escape.

## Returns

Returns *string* where all regex expression characters are escaped.

## Example

This statement:

```kusto
print result = regex_quote('(so$me.Te^xt)')
```

Returns the following results:

| result |
|---|
| `\(so\\$me\\.Te\\^xt\\)` |
