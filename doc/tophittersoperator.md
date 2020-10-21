---
title: top-hitters operator - Azure Data Explorer | Microsoft Docs
description: This article describes top-hitters operator in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# top-hitters operator

Returns an approximation of the first *N* results (assuming skewed distribution of the input).

```kusto
T | top-hitters 25 of Page by Views 
```

> [!NOTE]
> `top-hitters` is an approximation algorithm and should be used when running with large data. 
> The approximation of the the top-hitters is based on the [Count-Min-Sketch](https://en.wikipedia.org/wiki/Count%E2%80%93min_sketch) algorithm.  

## Syntax

*T* `| top-hitters` *NumberOfRows* `of` *sort_key* `[` `by` *expression* `]`

## Arguments

* *NumberOfRows*: The number of rows of *T* to return. You can specify any numeric expression.
* *sort_key*: The name of the column by which to sort the rows.
* *expression*: (optional) An expression which will be used for the top-hitters estimation. 
    * *expression*: top-hitters will return *NumberOfRows* rows which have an approximated maximum of sum(*expression*). Expression can be a column, or any other expression that evaluates to a number. 
    *  If *expression* is not mentioned, top-hitters algorithm will count the occurrences of the *sort-key*.  

## Examples

### Get most frequent items 

The next example shows how to find top-5 languages with most pages in Wikipedia (accessed after during April 2016). 

```kusto
PageViews
| where Timestamp > datetime(2016-04-01) and Timestamp < datetime(2016-05-01) 
| top-hitters 5 of Language 
```

|Language|approximate_count_Language|
|---|---|
|en|1539954127|
|zh|339827659|
|de|262197491|
|ru|227003107|
|fr|207943448|

### Get top hitters based on column value

The next example shows how to find most viewed English pages of Wikipedia of the year 2016. 
The query uses 'Views' (integer number) to calculate page popularity (number of views). 

```kusto
PageViews
| where Timestamp > datetime(2016-01-01)
| where Language == "en"
| where Page !has 'Special'
| top-hitters 10 of Page by Views
```

|Page|approximate_sum_Views|
|---|---|
|Main_Page|1325856754|
|Web_scraping|43979153|
|Java_(programming_language)|16489491|
|United_States|13928841|
|Wikipedia|13584915|
|Donald_Trump|12376448|
|YouTube|11917252|
|The_Revenant_(2015_film)|10714263|
|Star_Wars:_The_Force_Awakens|9770653|
|Portal:Current_events|9578000|
