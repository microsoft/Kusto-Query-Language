---
title: diffpatterns_text plugin - Azure Data Explorer
description: This article describes diffpatterns_text plugin in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# diffpatterns_text plugin

Compares two data sets of string values and finds text patterns that characterize differences between the two data sets.

```kusto
T | evaluate diffpatterns_text(TextColumn, BooleanCondition)
```

The `diffpatterns_text` returns a set of text patterns that capture different portions of the data in the two sets (i.e. a pattern capturing a large percentage of the rows when the condition is `true` and low percentage of the rows when the condition is `false`). The patterns are built from consecutive tokens (separated by white space), with a token from the text column or a `*` representing a wildcard. Each pattern is represented by a row in the results.

## Syntax

`T | evaluate diffpatterns_text(`TextColumn, BooleanCondition [, MinTokens, Threshold , MaxTokens]`)` 

## Arguments

### Required arguments

* TextColumn - *column_name*

    The text column to analyze, must be of type string.
    
* BooleanCondition - *Boolean expression*

    Defines how to generate the two record subsets to compare to the input table. The algorithm splits the query into two data sets, “True” and “False” according to the condition, then analyzes the (text) differences between them. 

### Optional arguments

All other arguments are optional, but they must be ordered as below. 

* MinTokens  - 0 < *int* < 200 [default: 1]

    Sets the minimal number of non-wildcard tokens per result pattern.

* Threshold - 0.015 < *double* < 1 [default: 0.05]

    Sets the minimal pattern (ratio) difference between the two sets (see [diffpatterns](diffpatternsplugin.md)).

* MaxTokens  - 0 < *int* [default: 20]

    Sets the maximal number of tokens (from the beginning) per result pattern, specifying a lower limit decreases the query runtime.

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

<!-- csl: https://help.kusto.windows.net:443/Samples -->
```kusto
StormEvents     
| where EventNarrative != "" and monthofyear(StartTime) > 1 and monthofyear(StartTime) < 9
| where EventType == "Drought" or EventType == "Extreme Cold/Wind Chill"
| evaluate diffpatterns_text(EpisodeNarrative, EventType == "Extreme Cold/Wind Chill", 2)
```

|Count_of_True|Count_of_False|Percent_of_True|Percent_of_False|Pattern|
|---|---|---|---|---|
|11|0|6.29|0|Winds shifting northwest in * wake * a surface trough brought heavy lake effect snowfall downwind * Lake Superior from|
|9|0|5.14|0|Canadian high pressure settled * * region * produced the coldest temperatures since February * 2006. Durations * freezing temperatures|
|0|34|0|6.24|* * * * * * * * * * * * * * * * * * West Tennessee,|
|0|42|0|7.71|* * * * * * caused * * * * * * * * across western Colorado. *|
|0|45|0|8.26|* * below normal *|
|0|110|0|20.18|Below normal *|
