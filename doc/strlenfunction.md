---
title: strlen() - Azure Data Explorer
description: Learn how to use the strlen() function to measure the length of the input string.
ms.reviewer: alexans
ms.topic: reference
ms.date: 02/05/2023
---
# strlen()

Returns the length, in characters, of the input string.

> [!NOTE]
> This function counts Unicode [code points](https://en.wikipedia.org/wiki/Code_point).

## Syntax

`strlen(`*source*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *source* | string | &check; | The string for which to return the length.|

## Returns

Returns the length, in characters, of the input string.

## Examples

### String of letters

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUchJzUsvyVCwVSguKQKyNZQyUnNy8pU0ATlWTM8eAAAA" target="_blank">Run the query</a>

```kusto
print length = strlen("hello")
```

**Output**

|length|
|---|
|5|

### String of letters and symbols

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUchJzUsvyVCwVSguKQKyNZQeTVr2aNKGR5PWPZq0/tGkVUqaAIvtpvwoAAAA" target="_blank">Run the query</a>

```kusto
print length = strlen("⒦⒰⒮⒯⒪")
```

**Output**

|length|
|---|
|5|

### String with grapheme

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA3WNsQ3DMAwE+0zxnePKQFbwJIT8iQgokkzSM2UZD2Uhva+8A/67aQ14WGF9Tuv546alyDRjWRCZeKt5IGUxSUGDOgQfk575JVI5fNgHbviPSMC4H2p0vJDaRvQ2bh3RRunDs8YF+EPCH4oAAAA=" target="_blank">Run the query</a>

```kusto
print strlen('Çedilla') // the first character is a grapheme cluster
                        // that requires 2 code points to represent
```

**Output**

|length|
|---|
|8|
