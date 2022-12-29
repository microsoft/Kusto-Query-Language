---
title: diffpatterns_text plugin - Azure Data Explorer
description: Learn how to use the diffpatterns_text plugin to compare two string value data sets to find the differences between the two data sets. 
ms.reviewer: alexans
ms.topic: reference
ms.date: 12/11/2022
---
# diffpatterns_text plugin

Compares two data sets of string values and finds text patterns that characterize differences between the two data sets. The plugin is invoked with the [`evaluate`](evaluateoperator.md) operator.

The `diffpatterns_text` returns a set of text patterns that capture different portions of the data in the two sets. For example, a pattern capturing a large percentage of the rows when the condition is `true` and low percentage of the rows when the condition is `false`. The patterns are built from consecutive tokens separated by white space, with a token from the text column or a `*` representing a wildcard. Each pattern is represented by a row in the results.

## Syntax

`T | evaluate diffpatterns_text(`*TextColumn*, *BooleanCondition* [, *MinTokens*, *Threshold* , *MaxTokens*]`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *TextColumn* | string | &check; | The text column to analyze. |
| *BooleanCondition* | string | &check; | An expression that evaluates to a boolean value. The algorithm splits the query into the two data sets to compare based on this expression.|
| *MinTokens* | int | | An integer value between 0 and 200 that represents the minimal number of non-wildcard tokens per result pattern. The default is 1. |
| *Threshold* | decimal | | A decimal value between 0.015 and 1 that sets the minimal pattern ratio difference between the two sets. Default is 0.05. See [diffpatterns](diffpatternsplugin.md).|
| *MaxTokens* | int | | An integer value between 0 and 20 that sets the maximal number of tokens per result pattern, specifying a lower limit decreases the query runtime.|

## Returns

The result of diffpatterns_text returns the following columns:

* Count_of_True: The number of rows matching the pattern when the condition is `true`.
* Count_of_False: The number of rows matching the pattern when the condition is `false`.
* Percent_of_True: The percentage of rows matching the pattern from the rows when the condition is `true`.
* Percent_of_False: The percentage of rows matching the pattern from the rows when the condition is `false`.
* Pattern: The text pattern containing tokens from the text string and '`*`' for wildcards.

> [!NOTE]
> The patterns aren't necessarily distinct and may not provide full coverage of the data set. The patterns may be overlapping and some rows may not match any pattern.

## Example

The following example uses data from the StormEvents table in the help cluster. To access this data, sign in to [https://dataexplorer.azure.com/clusters/help/databases/Samples](https://dataexplorer.azure.com/clusters/help/databases/Samples). In the left menu, browse to **help** > **Samples** > **Tables** > **Storm_Events**.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA43OvQrCQBAE4F7wHdarEgiIdoKxiWltErCUhdt4B/cTNptowIc3sbBIIU45Ax9TSWRfDhSkgznr1QsehpjgU16QGcUOBJsclAIMGnwMYmIzEnJSCbLU1lMKJ9j9mo9wWNj12BLkE3vm2N+NKIi8WMqnMHmCIjq9vdpJL4x1Ts0SDeh6FAJtm6ZFEeLQ3YSekpSt7aKm7/nsTzaDffoGjOv6LBEBAAA=" target="_blank">Run the query</a>

```kusto
StormEvents     
| where EventNarrative != "" and monthofyear(StartTime) > 1 and monthofyear(StartTime) < 9
| where EventType == "Drought" or EventType == "Extreme Cold/Wind Chill"
| evaluate diffpatterns_text(EpisodeNarrative, EventType == "Extreme Cold/Wind Chill", 2)
```

**Output**

|Count_of_True|Count_of_False|Percent_of_True|Percent_of_False|Pattern|
|---|---|---|---|---|
|11|0|6.29|0|Winds shifting northwest in * wake * a surface trough brought heavy lake effect snowfall downwind * Lake Superior from|
|9|0|5.14|0|Canadian high pressure settled * * region * produced the coldest temperatures since February * 2006. Durations * freezing temperatures|
|0|34|0|6.24|* * * * * * * * * * * * * * * * * * West Tennessee,|
|0|42|0|7.71|* * * * * * caused * * * * * * * * across western Colorado. *|
|0|45|0|8.26|* * below normal *|
|0|110|0|20.18|Below normal *|
