---
title:  punycode_to_string 
description: This article describes the punycode_to_string() command in Azure Data Explorer.
ms.topic: reference
ms.date: 04/16/2023
---

# punycode_to_string()

Decodes input string from [punycode](https://en.wikipedia.org/wiki/Punycode) form. Punycode is a representation of Unicode with the limited ASCII character subset used for Internet hostnames.

## Syntax

`punycode_to_string('input_string')`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *input_string* | `string` | &check; | A string to be decoded from punycode form. The function accepts one string argument.

## Returns

* Returns a `string` that represents the original, decoded string.
* Returns an empty result if decoding failed.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUUhJTc5PSU1RsFUoKM2rBLHjS/Lji0uAkuka6onJiSm5mam6aUWJeYmZxam6eQVJhonqmgCaLR+2PAAAAA==" target="_blank">Run the query</a>

```kusto
 print decoded = punycode_to_string('acadmie-franaise-npb1a')
```

|decoded|
|---|
|académie-française|

## Next steps

Use [punycode_from_string()](punycode-from-string.md) to encode a string to punycode form.
