---
title:  regex_quote()
description: Learn how to use the regex_quote() function to return a string that escapes all regular expression characters.
ms.reviewer: shanisolomon
ms.topic: reference
ms.date: 01/17/2023
---
# regex_quote()

Returns a string that escapes all regular expression characters.

## Syntax

`regex_quote(`*string*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *string* | string | &check; | The string to escape.|

## Returns

Returns *string* where all regex expression characters are escaped.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUShKLS7NKVGwBTLSUyviC0vzS1I11DWK81VyU/VCUuMqSjTVNQH+BIvaKwAAAA==" target="_blank">Run the query</a>

```kusto
print result = regex_quote('(so$me.Te^xt)')
```

**Output**

| result |
|---|
| `\(so\\$me\\.Te\\^xt\\)` |
