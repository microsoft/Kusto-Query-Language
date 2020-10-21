---
title: reduce operator - Azure Data Explorer
description: This article describes reduce operator in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# reduce operator

Groups a set of strings together based on values similarity.

```kusto
T | reduce by LogMessage with threshold=0.1
```

For each such group, it outputs a **pattern** that best describes the group (possibly using the
asterix (`*`) character to represent wildcards), a **count** of the number of values in the group,
and a **representative** of the group (one of the original values in the group).

## Syntax

*T* `|` `reduce` [`kind` `=` *ReduceKind*] `by` *Expr* [`with` [`threshold` `=` *Threshold*] [`,` `characters` `=` *Characters*] ]

## Arguments

* *Expr*: An expression that evaluates to a `string` value.
* *Threshold*: A `real` literal in the range (0..1). Default is 0.1. For large inputs, threshold should be small. 
* *Characters*: A `string` literal containing a list of characters to add to the list of characters
  that don't break a term. (For example, if you want `aaa=bbbb` and `aaa:bbb` to each be a whole term,
  rather than break on `=` and `:`, use `":="` as the string literal.)
* *ReduceKind*: Specifies the reduce flavor. The only valid value for the time being is `source`.

## Returns

This operator returns a table with three columns (`Pattern`, `Count`, and `Representative`),
and as many rows as there are groups. `Pattern` is the pattern value for the group, with `*`
being used as a wildcard (representing arbitrary insertion strings), `Count` counts how
many rows in the input to the operator are represented by this pattern, and `Representative`
is one value from the input that falls into this group.

If `[kind=source]` is specified, the operator will append the `Pattern` column to the existing table structure.
Note that the syntax an schema of this flavor might be subjected to future changes.

For example, the result of `reduce by city` might include: 

|Pattern     |Count |Representative|
|------------|------|--------------|
| San *      | 5182 |San Bernard   |
| Saint *    | 2846 |Saint Lucy    |
| Moscow     | 3726 |Moscow        |
| \* -on- \* | 2730 |One -on- One  |
| Paris      | 2716 |Paris         |

Another example with customized tokenization:

<!-- csl: https://help.kusto.windows.net:443/Samples -->
```kusto
range x from 1 to 1000 step 1
| project MyText = strcat("MachineLearningX", tostring(toint(rand(10))))
| reduce by MyText  with threshold=0.001 , characters = "X" 
```

|Pattern         |Count|Representative   |
|----------------|-----|-----------------|
|MachineLearning*|1000 |MachineLearningX4|

## Examples

The following example shows how one might apply the `reduce` operator to a "sanitized"
input, in which GUIDs in the column being reduced are replaced prior to reducing

```kusto
// Start with a few records from the Trace table.
Trace | take 10000
// We will reduce the Text column which includes random GUIDs.
// As random GUIDs interfere with the reduce operation, replace them all
// by the string "GUID".
| extend Text=replace(@"[[:xdigit:]]{8}-[[:xdigit:]]{4}-[[:xdigit:]]{4}-[[:xdigit:]]{4}-[[:xdigit:]]{12}", @"GUID", Text)
// Now perform the reduce. In case there are other "quasi-random" identifiers with embedded '-'
// or '_' characters in them, treat these as non-term-breakers.
| reduce by Text with characters="-_"
```

## See also

[autocluster](./autoclusterplugin.md)

**Notes**

The implementation of `reduce` operator is largely based on the paper [A Data Clustering Algorithm for Mining Patterns From Event Logs](https://ristov.github.io/publications/slct-ipom03-web.pdf), by Risto Vaarandi.
