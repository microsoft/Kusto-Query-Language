---
title:  strcmp()
description: Learn how to use the strcmp() function to compare two strings.
ms.reviewer: alexans
ms.topic: reference
ms.date: 02/05/2023
---
# strcmp()

Compares two strings.

The function starts comparing the first character of each string. If they're equal to each other, it continues with the following pairs until the characters differ or until the end of shorter string is reached.

## Syntax

`strcmp(`*string1*`,` *string2*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *string1* | string | &check; | The first input string for comparison.|
| *string2* | string | &check; | The second input string for comparison.|

## Returns

Returns an integer value indicating the relationship between the strings:

* *<0* - the first character that doesn't match has a lower value in *string1* than in *string2*
* *0* - the contents of both strings are equal
* *>0* - the first character that doesn't match has a greater value in *string1* than in *string2*

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA0tJLAHCpJxUjeKSosy8dEMrCK2jAKGNoHxNhWguBSBQcnRyVtKBkBCBxKRkVAGICrAwXEVKKlSIK5arRiG1oiQ1L0WhKLW4NKdEwRZkVXJuAcwFOlCbNQEUhY2inAAAAA==" target="_blank">Run the query</a>

```kusto
datatable(string1:string, string2:string) [
    "ABC","ABC",
    "abc","ABC",
    "ABC","abc",
    "abcde","abc"
]
| extend result = strcmp(string1,string2)
```

**Output**

|string1|string2|result|
|---|---|---|
|ABC|ABC|0|
|abc|ABC|1|
|ABC|abc|-1|
|abcde|abc|1|
