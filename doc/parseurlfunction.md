---
title:  parse_url()
description: Learn how to use the parse_url() function to parse a URL string.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/12/2023
---
# parse_url()

Parses an absolute URL `string` and returns a `dynamic` object contains URL parts.

> **Deprecated aliases:** parseurl()

## Syntax

`parse_url(`*url*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *url* | string | &check; | An absolute URL, including its scheme, or the query part of the URL. For example, use the absolute `https://bing.com` instead of `bing.com`.|

## Returns

An object of type [dynamic](./scalar-data-types/dynamic.md) that included the URL components: Scheme, Host, Port, Path, Username, Password, Query Parameters, Fragment.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAw3GUQpAQBAA0KuIEl/TLl9q4wwuoIlhhbXNzHJ96n28yHvQbCRJp7qILDQlPqtcZk8XdQBJiAP+jSjy3rwM/hbtjG1aUL8L/BAiqu8P4x5THtY9tlgZt4uC5vUH0Z3WuWIAAAA=" target="_blank">Run the query</a>

```kusto
print Result=parse_url("scheme://username:password@host:1234/this/is/a/path?k1=v1&k2=v2#fragment")
```

**Output**

|Result|
|--|
|{"Scheme":"scheme", "Host":"host", "Port":"1234", "Path":"this/is/a/path", "Username":"username", "Password":"password", "Query Parameters":"{"k1":"v1", "k2":"v2"}", "Fragment":"fragment"}|
