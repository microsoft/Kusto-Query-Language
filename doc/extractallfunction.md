---
title:  extract_all()
description: Lean how to use the extract_all() to extract all matches for a regular expression from a source string.
ms.reviewer: alexans
ms.topic: reference
ms.date: 12/12/2022
---
# extract_all()

Get all matches for a [regular expression](./re2.md) from a source string.
Optionally, retrieve a subset of matching groups.

```kusto
print extract_all(@"(\d+)", "a set of numbers: 123, 567 and 789") // results with the dynamic array ["123", "567", "789"]
```

> **Deprecated aliases:** extractall()

## Syntax

`extract_all(`*regex*`,` [*captureGroups*`,`] *source*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *regex* | string | &check; | A [regular expression](./re2.md) containing between one and 16 capture groups.|
| *captureGroups* | dynamic | | An array that indicates the capture groups to extract. Valid values are from 1 to the number of capturing groups in the regular expression. Named capture groups are allowed as well. See [examples](#examples).|
| *source* | string | &check;| The string to search.|

## Returns

* If *regex* finds a match in *source*: Returns dynamic array including all matches against the indicated capture groups *captureGroups*, or all of capturing groups in the *regex*.
* If number of *captureGroups* is 1: The returned array has a single dimension of matched values.
* If number of *captureGroups* is more than 1: The returned array is a two-dimensional collection of multi-value matches per *captureGroups* selection, or all capture groups present in the *regex* if *captureGroups* is omitted.
* If there's no match: `null`.

## Examples

### Extract a single capture group

The following query returns hex-byte representation (two hex-digits) of the GUID.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUfBMsVWyMEqySEo1StFNSUs01zVJSjHUtUgzM9Y1MklMMTJLMTY0MbFU4qpRSK0oSc1LUUgvzUyJT6osSS1WsAWJFSUml8Qn5uRoOChpRMekJOqmxVYb1Woq6QAN1wQA6/wKuGYAAAA=" target="_blank">Run the query</a>

```kusto
print Id="82b8be2d-dfa7-4bd1-8f63-24ad26d31449"
| extend guid_bytes = extract_all(@"([\da-f]{2})", Id) 
```

**Output**

|ID|guid_bytes|
|---|---|
|82b8be2d-dfa7-4bd1-8f63-24ad26d31449|["82","b8","be","2d","df","a7","4b","d1","8f","63","24","ad","26","d3","14","49"]|

### Extract several capture groups

The following query uses a regular expression with three capturing groups to split each GUID part into first letter, last letter, and whatever is in the middle.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUfBMsVWyMEqySEo1StFNSUs01zVJSjHUtUgzM9Y1MklMMTJLMTY0MbFU4qpRSK0oSc1LUUgvzUyJT6osSS1WsAWJFSUml8Qn5uRoOChpxJRrArE2iNBU0gGargkAiT0FmGcAAAA=" target="_blank">Run the query</a>

```kusto
print Id="82b8be2d-dfa7-4bd1-8f63-24ad26d31449"
| extend guid_bytes = extract_all(@"(\w)(\w+)(\w)", Id)
```

**Output**

|ID|guid_bytes|
|---|---|
|82b8be2d-dfa7-4bd1-8f63-24ad26d31449|[["8","2b8be2","d"],["d","fa","7"],["4","bd","1"],["8","f6","3"],["2","4ad26d3144","9"]]|

### Extract a subset of capture groups

The following query selects a subset of capturing groups.

The regular expression matches the first letter, last letter, and all the rest.

The *captureGroups* parameter is used to select only the first and the last parts.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUfBMsVWyMEqySEo1StFNSUs01zVJSjHUtUgzM9Y1MklMMTJLMTY0MbFU4qpRSK0oSc1LUUgvzUyJT6osSS1WsAWJFSUml8Qn5uRoOChpxJRrArE2iNBU0lFIqcxLzM1M1og21DGO1dQB2qYJAHPOX8l3AAAA" target="_blank">Run the query</a>

```kusto
print Id="82b8be2d-dfa7-4bd1-8f63-24ad26d31449"
| extend guid_bytes = extract_all(@"(\w)(\w+)(\w)", dynamic([1,3]), Id) 
```

**Output**

|ID|guid_bytes|
|---|---|
|82b8be2d-dfa7-4bd1-8f63-24ad26d31449|[["8","d"],["d","7"],["4","1"],["8","3"],["2","9"]]|

### Using named capture groups

The *captureGroups* in the following query uses both capture group indexes and named capture group references to fetch matching values.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAyWOsQrCMBRFd78iZEmLydA01AhWXd3cVcpLX1ICaZE2ogU/3qaO5x4O3Ofoh0guWFMtjTZWokAHO6EMFkK7qhRSAcoKy0KpPd18if1EOyDpXh4bM0c7kTptI7SxgRCyM81O14Pz4xSP93eeoPeIwS60XTHAX1FOcB6g9212Y2vAuOQsafbI+fIq/wFEwbznnwAAAA==" target="_blank">Run the query</a>

```kusto
print Id="82b8be2d-dfa7-4bd1-8f63-24ad26d31449"
| extend guid_bytes = extract_all(@"(?P<first>\w)(?P<middle>\w+)(?P<last>\w)", dynamic(['first',2,'last']), Id) 
```

**Output**

|ID|guid_bytes|
|---|---|
|82b8be2d-dfa7-4bd1-8f63-24ad26d31449|[["8","2b8be2","d"],["d","fa","7"],["4","bd","1"],["8","f6","3"],["2","4ad26d3144","9"]]|
