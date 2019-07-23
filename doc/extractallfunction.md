---
title: extract_all() - Azure Data Explorer | Microsoft Docs
description: This article describes extract_all() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 11/02/2018
---
# extract_all()

Get all matches for a [regular expression](./re2.md) from a text string.

Optionally, a subset of matching groups can be retrieved.

```kusto
print extract_all(@"(\d+)", "a set of numbers: 123, 567 and 789") == dynamic(["123", "567", "789"])
```

**Syntax**

`extract_all(`*regex*`,` [*captureGroups*`,`] *text*`)`

**Arguments**

* *regex*: A [regular expression](./re2.md). Regular 
expression must have at least one capturing group, and less-or-equal than 16 capturing groups.
* *captureGroups*: (optional). A dynamic array constant indicating the capture group to extract. Valid 
values are from 1 to number of capturing groups in the regular expression. Named capture groups are allowed as
well (see examples section).
* *text*: A `string` to search.

**Returns**

If *regex* finds a match in *text*: 
returns dynamic array including all matches against the indicated capture groups *captureGroups* (or all of capturing groups in the *regex*).
If number of *captureGroups* is 1: the returned array has a single dimension of matched values.
If number of *captureGroups* is more than 1: the returned array is a two-dimensional collection of multi-value matches per *captureGroups* selection (or all capture groups present in the *regex* if *captureGroups* is omitted) 

If there's no match: `null`. 

**Examples**

### Extracting single capture group
The example below returns hex-byte representation (two hex-digits) of the GUID.

```kusto
print Id="82b8be2d-dfa7-4bd1-8f63-24ad26d31449"
| extend guid_bytes = extract_all(@"([\da-f]{2})", Id) 
```

|Id|guid_bytes|
|---|---|
|82b8be2d-dfa7-4bd1-8f63-24ad26d31449|["82","b8","be","2d","df","a7","4b","d1","8f","63","24","ad","26","d3","14","49"]|

### Extracting several capture groups 
Next example uses a regular expression with 3 capturing groups to split each GUID part into first letter, last letter and whatever in the middle.

```kusto
print Id="82b8be2d-dfa7-4bd1-8f63-24ad26d31449"
| extend guid_bytes = extract_all(@"(\w)(\w+)(\w)", Id) 
```

|Id|guid_bytes|
|---|---|
|82b8be2d-dfa7-4bd1-8f63-24ad26d31449|[["8","2b8be2","d"],["d","fa","7"],["4","bd","1"],["8","f6","3"],["2","4ad26d3144","9"]]|

### Extracting subset of capture groups

Next example shows how to select a subset of capturing groups: in this case the regular expression 
matches into first letter, last letter and all the rest - while the *captureGroups* parameter is used to select only first and the last part. 

```kusto
print Id="82b8be2d-dfa7-4bd1-8f63-24ad26d31449"
| extend guid_bytes = extract_all(@"(\w)(\w+)(\w)", dynamic([1,3]), Id) 
```

|Id|guid_bytes|
|---|---|
|82b8be2d-dfa7-4bd1-8f63-24ad26d31449|[["8","d"],["d","7"],["4","1"],["8","3"],["2","9"]]|


### Using named capture groups

You can utilize named capture groups of RE2 in extract_all(). 
In the example below - the *captureGroups* uses both capture group indexes and named capture group reference to fetch matching values.

```kusto
print Id="82b8be2d-dfa7-4bd1-8f63-24ad26d31449"
| extend guid_bytes = extract_all(@"(?P<first>\w)(?P<middle>\w+)(?P<last>\w)", dynamic(['first',2,'last']), Id) 
```

|Id|guid_bytes|
|---|---|
|82b8be2d-dfa7-4bd1-8f63-24ad26d31449|[["8","2b8be2","d"],["d","fa","7"],["4","bd","1"],["8","f6","3"],["2","4ad26d3144","9"]]|