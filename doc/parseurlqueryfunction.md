---
title: parse_urlquery() - Azure Data Explorer
description: This article describes parse_urlquery() in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/03/2022
---
# parse_urlquery()

Returns a `dynamic` object contains the Query parameters.

> **Deprecated aliases:** parseurlquery()

## Syntax

`parse_urlquery(`*query*`)`

## Arguments

* *query*: A string represents a url query.

## Returns

An object of type [dynamic](./scalar-data-types/dynamic.md) that includes the query parameters.

## Examples

```kusto
parse_urlquery("k1=v1&k2=v2&k3=v3")
```

result:

```kusto
 {
 	"Query Parameters":"{"k1":"v1", "k2":"v2", "k3":"v3"}",
 }
```

The following example uses a function to extract specific query parameters:

```kusto
let getQueryParamValue = (querystring: string, param: string)
{
    let params = parse_urlquery(querystring);
    tostring(params["Query Parameters"].[param])
};
print UrlQuery = 'view=vs-2019&preserve-view=true'
| extend view = getQueryParamValue(UrlQuery, 'view')
| extend preserve = getQueryParamValue(UrlQuery, 'preserve-view')
```

result:

| UrlQuery | view | preserve |
|--|--|--|
|view=vs-2019&preserve-view=true|vs-2019|true|

**Notes**

* Input format should follow URL query standards (key=value& ...)
 
