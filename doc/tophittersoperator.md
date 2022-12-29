---
title: top-hitters operator - Azure Data Explorer
description: This article describes top-hitters operator in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 02/13/2020
---
# top-hitters operator

Returns an approximation for the most popular distinct values, or the values
with the largest sum, in the input.

```kusto
Events | top-hitters 5 of EventId

PageViews | top-hitters 25 of Page by NumViews
```

> [!NOTE]
> `top-hitters` uses an approximation algorithm optimized for performance
> when the input data is large.
> The approximation is based on the [Count-Min-Sketch](https://en.wikipedia.org/wiki/Count%E2%80%93min_sketch) algorithm.  

## Syntax

*T* `|` `top-hitters` *NumberOfValues* `of` *ValueExpression*

*T* `|` `top-hitters` *NumberOfValues* `of` *ValueExpression* `by` *SummingExpression*

## Arguments

* *NumberOfValues*: The number of distinct values of *ValueExpression*.
  Expressions of type `int`, `long`, and `real` are valid (rounded down).

* *ValueExpression*: An expression over the input table *T* whose distinct
  values are returned.

* *SummingExpression*: If specified, a numeric expression over the input table *T*
  whose sum per distinct value of *ValueExpression* establishes which values
  to emit. If not specified, the count of each distinct value of *ValueExpression*
  will be used instead.

## Remarks

The first syntax (no *SummingExpression*) is conceptually equivalent to:

*T*
`|` `summarize` `C``=``count()` `by` *ValueExpression*
`|` `top` *NumberOfValues* by `C` `desc`

The second syntax (with *SummingExpression*) is conceptually equivalent to:

*T*
`|` `summarize` `S``=``sum(*SummingExpression*)` `by` *ValueExpression*
`|` `top` *NumberOfValues* by `S` `desc`

## Examples

### Get most frequent items

The next example shows how to find top-5 languages with most pages in Wikipedia (accessed after during April 2016).

```kusto
PageViews
| where Timestamp > datetime(2016-04-01) and Timestamp < datetime(2016-05-01) 
| top-hitters 5 of Language 
```

**Output**

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

**Output**

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
