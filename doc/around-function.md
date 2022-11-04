---
title: around() function - Azure Data Explorer
description: Learn how to use the around() function to indicate if the first argument is within a range around the center value.
ms.reviewer: alexans
ms.topic: reference
ms.date: 09/21/2022
---
# around()

Creates a `bool` value indicating if the first argument is within a range around the center value.

## Syntax

`around(`*value*`,`*center*`,`*delta*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
|*value*| scalar | &check; | A number, date, or [timespan](scalar-data-types/timespan.md) that is evaluated.|
| *center* | scalar | &check; | A number, date, or [timespan](scalar-data-types/timespan.md) representing the center of the range defined as [(`center`-`delta`) .. (`center` + `delta`)]. |
| *delta* | scalar| &check; | A number, or [timespan](scalar-data-types/timespan.md) representing the delta value of the range defined as [(`center`-`delta`) .. (`center` + `delta`)].|

## Returns

Returns `true` if the value is within the range, `false` if the value is outside the range.
Returns `null` if any of the arguments is `null`.

## Example: Filtering values around a specific timestamp

The following example filters rows around specific timestamp.

**\[**[**Click to run query**](https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAytKzEtPVUgpUeBSAIK0ovxchZTEktSSzNxUDSMDI0NdAxBSMDC0MjDQhCgqyceuxAihpLgktUDBMDczj6tGoTwjtShVIbEovzQvRSOlRAeX+cYGmjpgPZoA56xhi5QAAAA=)**\]**

```kusto
range dt 
    from datetime(2021-01-01 01:00) 
    to datetime(2021-01-01 02:00) 
    step 1min
| where around(dt, datetime(2021-01-01 01:30), 1min)
```

**Results**

|dt|
|---|
|2021-01-01 01:29:00.0000000|
|2021-01-01 01:30:00.0000000|
|2021-01-01 01:31:00.0000000|
