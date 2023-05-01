---
title: punycode_from_string - Azure Data Explorer 
description: This article describes the punycode_from_string() command in Azure Data Explorer.
ms.topic: reference
ms.date: 04/16/2023
---

# punycode_from_string()

Encodes input string to [Punycode](https://en.wikipedia.org/wiki/Punycode) form.
Punycode is a representation of Unicode with the limited ASCII character subset used for Internet hostnames.


## Syntax

`punycode_from_string('input_string')`

## Parameters
| Name | Type | Required | Description |
|--|--|--|--|
| *input_string* |  `string` | &check; | A string to be encoded to punycode form. The function accepts one string argument.

## Returns

* Returns a `string` that represents punycode-encoded original string.
* Returns an empty result if encoding failed.

## Example


> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUUjNS85PSU1RsFUoKM2rBLHj04ryc+OLS4DS6RrqicmJKYdX5mam6qYVJeYdXp6YWZyqrgkA2GFbdjwAAAA=" target="_blank">Run the query</a>

```kusto
 print encoded = punycode_from_string('académie-française')
```

|encoded|
|---|
|acadmie-franaise-npb1a|

## Next steps

Use [punycode_to_string()](punycode-to-string.md) to retrieve the original decoded string.
