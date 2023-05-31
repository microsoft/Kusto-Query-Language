---
title:  punycode_from_string 
description: This article describes the punycode_from_string() command in Azure Data Explorer.
ms.topic: reference
ms.date: 04/16/2023
---

# punycode_from_string()

Encodes input string to [Punycode](https://en.wikipedia.org/wiki/Punycode) form.
The result string contains only ASCII characters. The result string doesn't start with "xn--".


## Syntax

`punycode_from_string('input_string')`

## Parameters
| Name | Type | Required | Description |
|--|--|--|--|
| *input_string* |  `string` | &check; | A string to be encoded to punycode form. The function accepts one string argument.

## Returns

* Returns a `string` that represents punycode-encoded original string.
* Returns an empty result if encoding failed.

## Examples


> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUUjNS85PSU1RsFUoKM2rBLHj04ryc+OLS4DS6RrqicmJKYdX5mam6qYVJeYdXp6YWZyqrgkA2GFbdjwAAAA=" target="_blank">Run the query</a>

```kusto
 print encoded = punycode_from_string('académie-française')
```

|encoded|
|---|
|acadmie-franaise-npb1a|

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUUjJz03MzLNVf9G569mc9XrJ+bnqvFw1CqkVJal5KVDZ+LLUZAVbheKCnMwSDYiQjoK6nromksrUvOT8lNSU+Iz84hKg2oLSvEqQQHxaUX5ufHEJ0K50jZJ8KANhbLRBrCY2YyAqQJaWFCUnlmioV+Tp6qrroFgDdoMOkhujDWM1AQ07DZjWAAAA" target="_blank">Run the query</a>

```kusto
 print domain='艺术.com'
| extend domain_vec = split(domain, '.')
| extend encoded_host = punycode_from_string(tostring(domain_vec[0]))
| extend encoded_domain = strcat('xn--', encoded_host, '.', domain_vec[1])
```

|domain|	domain_vec |	encoded_host |	encoded_domain|
|---|---|---|---|
|艺术.com | ["艺术","com"] | cqv902d | xn--cqv902d.com|


## Next steps

Use [punycode_to_string()](punycode-to-string.md) to retrieve the original decoded string.
