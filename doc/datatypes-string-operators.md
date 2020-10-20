---
title: String operators - Azure Data Explorer
description: This article describes String operators in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: rkarlin
ms.service: data-explorer
ms.topic: reference
ms.date: 10/19/2020
---
# String operators

Kusto offers a variety of query operators for searching string data types. The following article describes how string terms are indexed, lists the string query operators, and gives tips for optimizing performance.

## Understanding string terms

Kusto indexes all columns, including columns of type `string`. Multiple indexes are built for such columns, depending on the actual data. These indexes aren't directly exposed, but are used in queries with the `string` operators that have `has` as part of their name, such as `has`, `!has`, `hasprefix`, `!hasprefix`. The semantics of these operators are dictated by the way the column is encoded. Instead of doing a "plain" substring match, these operators match *terms*.

### What is a term? 

By default, each `string` value is broken into maximal sequences of ASCII alphanumeric characters, and each of those sequences is made into a term.
For example, in the following `string`, the terms are `Kusto`, `WilliamGates3rd`, and the following substrings: `ad67d136`, `c1db`, `4f9f`, `88ef`, `d94f3b6b0b5a`.

```
Kusto:  ad67d136-c1db-4f9f-88ef-d94f3b6b0b5a;;WilliamGates3rd
```

Kusto builds a term index consisting of all terms that are *four characters or more*, and this index is used by `has`, `!has`, and so on. If the query looks for a term that is smaller than four characters, or uses a `contains` operator, Kusto will revert to scanning the values in the column if it can't determine a match. This method is much slower than looking up the term in the term index.

## Operators on strings

> [!NOTE]
> The following abbreviations are used in the table below:
> * RHS = right hand side of the expression
> * LHS = left hand side of the expression

Operator        |Description                                                       |Case-Sensitive|Example (yields `true`)
----------------|------------------------------------------------------------------|--------------|-----------------------
`==`            |Equals                                                            |Yes           |`"aBc" == "aBc"`
`!=`            |Not equals                                                        |Yes           |`"abc" != "ABC"`
`=~`            |Equals                                                            |No            |`"abc" =~ "ABC"`
`!~`            |Not equals                                                        |No            |`"aBc" !~ "xyz"`
`has`           |Right-hand-side (RHS) is a whole term in left-hand-side (LHS)     |No            |`"North America" has "america"`
`!has`          |RHS isn't a full term in LHS                                     |No            |`"North America" !has "amer"` 
`has_cs`        |RHS is a whole term in LHS                                        |Yes           |`"North America" has_cs "America"`
`!has_cs`       |RHS isn't a full term in LHS                                     |Yes           |`"North America" !has_cs "amer"` 
`hasprefix`     |RHS is a term prefix in LHS                                       |No            |`"North America" hasprefix "ame"`
`!hasprefix`    |RHS isn't a term prefix in LHS                                   |No            |`"North America" !hasprefix "mer"` 
`hasprefix_cs`  |RHS is a term prefix in LHS                                       |Yes           |`"North America" hasprefix_cs "Ame"`
`!hasprefix_cs` |RHS isn't a term prefix in LHS                                   |Yes           |`"North America" !hasprefix_cs "CA"` 
`hassuffix`     |RHS is a term suffix in LHS                                       |No            |`"North America" hassuffix "ica"`
`!hassuffix`    |RHS isn't a term suffix in LHS                                   |No            |`"North America" !hassuffix "americ"`
`hassuffix_cs`  |RHS is a term suffix in LHS                                       |Yes           |`"North America" hassuffix_cs "ica"`
`!hassuffix_cs` |RHS isn't a term suffix in LHS                                   |Yes           |`"North America" !hassuffix_cs "icA"`
`contains`      |RHS occurs as a subsequence of LHS                                |No            |`"FabriKam" contains "BRik"`
`!contains`     |RHS doesn't occur in LHS                                         |No            |`"Fabrikam" !contains "xyz"`
`contains_cs`   |RHS occurs as a subsequence of LHS                                |Yes           |`"FabriKam" contains_cs "Kam"`
`!contains_cs`  |RHS doesn't occur in LHS                                         |Yes           |`"Fabrikam" !contains_cs "Kam"`
`startswith`    |RHS is an initial subsequence of LHS                              |No            |`"Fabrikam" startswith "fab"`
`!startswith`   |RHS isn't an initial subsequence of LHS                          |No            |`"Fabrikam" !startswith "kam"`
`startswith_cs` |RHS is an initial subsequence of LHS                              |Yes           |`"Fabrikam" startswith_cs "Fab"`
`!startswith_cs`|RHS isn't an initial subsequence of LHS                          |Yes           |`"Fabrikam" !startswith_cs "fab"`
`endswith`      |RHS is a closing subsequence of LHS                               |No            |`"Fabrikam" endswith "Kam"`
`!endswith`     |RHS isn't a closing subsequence of LHS                           |No            |`"Fabrikam" !endswith "brik"`
`endswith_cs`   |RHS is a closing subsequence of LHS                               |Yes           |`"Fabrikam" endswith_cs "kam"`
`!endswith_cs`  |RHS isn't a closing subsequence of LHS                           |Yes           |`"Fabrikam" !endswith_cs "brik"`
`matches regex` |LHS contains a match for RHS                                      |Yes           |`"Fabrikam" matches regex "b.*k"`
`in`            |Equals to one of the elements                                     |Yes           |`"abc" in ("123", "345", "abc")`
`!in`           |Not equals to any of the elements                                 |Yes           |`"bca" !in ("123", "345", "abc")`
`in~`           |Equals to one of the elements                                     |No            |`"abc" in~ ("123", "345", "ABC")`
`!in~`          |Not equals to any of the elements                                 |No            |`"bca" !in~ ("123", "345", "ABC")`
`has_any`       |Same as `has` but works on any of the elements                    |No            |`"North America" has_any("south", "north")`

> [!TIP]
> All operators containing `has` search on indexed *terms* of four or more characters, and not on substring matches. A term is created by breaking up the string into sequences of ASCII alphanumeric characters. See [understanding string terms](#understanding-string-terms).

## Performance tips

For better performance, when there are two operators that do the same task, use the case-sensitive one.
For example:

* instead of `=~`, use `==`
* instead of `in~`, use `in`
* instead of `contains`, use `contains_cs`

For faster results, if you're testing for the presence of a symbol or alphanumeric word that is bound by non-alphanumeric characters, or the start or end of a field, use `has` or `in`. 
`has` works faster than `contains`, `startswith`, or `endswith`.

For example, the first of these queries will run faster:

```kusto
EventLog | where continent has "North" | count;
EventLog | where continent contains "nor" | count
```
