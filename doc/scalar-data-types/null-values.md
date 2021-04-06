---
title: Null Values - Azure Data Explorer | Microsoft Docs
description: This article describes Null Values in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 04/04/2021
zone_pivot_group_filename: data-explorer/zone-pivot-groups.json
zone_pivot_groups: kql-flavors
---
# Null Values

All scalar data types in Kusto have a special value that represents a missing value.
This value is called the **null value**, or **null**.

> [!NOTE]
> The `string` data type doesn't support null values.

## Null literals

The null value of a scalar type *T* is represented in the query language by the null literal `T(null)`.
The following query returns a single row full of null values:

```kusto
print bool(null), datetime(null), dynamic(null), guid(null), int(null), long(null), real(null), double(null), time(null)
```

## Predicates on null values

The scalar function [`isnull()`](../isnullfunction.md) can be used to determine if a scalar value
is the null value. The corresponding function [`isnotnull()`](../isnotnullfunction.md) can be used
to determine if a scalar value isn't the null value.

> [!NOTE]
> Because the `string` type doesn't support null values, it's recommended to use
> the [`isempty()`](../isemptyfunction.md) and the [`isnotempty()`](../isnotemptyfunction.md)
> functions.

## Equality and inequality of null values

* **Equality** (`==`): Applying the equality operator to two null values yields `bool(null)`.
  Applying the equality operator to a null value and a non-null value yields `bool(false)`.
* **Inequality** (`!=`): Applying the inequality operator to two null values yields `bool(null)`.
  Applying the inequality operator to a null value and a non-null value yields `bool(true)`.

For example:

```kusto
datatable(val:int)[5, int(null)]
| extend IsBiggerThan3 = val > 3
| extend IsBiggerThan3OrNull = val > 3 or isnull(val)
| extend IsEqualToNull = val == int(null)
| extend IsNotEqualToNull = val != int(null)
```

Results:

|val |IsBiggerThan3|IsBiggerThan3OrNull|IsEqualToNull|IsNotEqualToNull|
|----|-------------|-------------------|-------------|----------------|
|5   |true         |true               |false        |true            |
|null|null         |true               |null         |null            |

## Null values and the where query operator

The [where operator](../whereoperator.md) uses Boolean expressions to determine
if to emit each input record to the output. This operator treats null values as if
they're `bool(false)`. Records for which the predicate returns the null value are dropped and don't appear in the output.

For example:

```kusto
datatable(ival:int, sval:string)[5, "a", int(null), "b"]
| where ival != 5
```

Results:

|ival|sval|
|----|----|
|null|b   |

## Binary operators and null values

Binary operators are scalar operators that accept two scalar values and produce a third value. For example, greater-than (&gt;) and Boolean AND (&amp;&amp;) are binary operators.

For all binary operators except as noted [below](#exceptions-to-this-rule), the rule is as follows: <br>
If one or both of the values input to the binary operator are null values, then the output of the binary operator is also the null value.
In other words, the null value is "sticky".

### Exceptions to this rule

* For the equality (`==`) and inequality (`!=`) operators,
  if one of the values is null and the other value isn't null, then the result is either `bool(false)` or `bool(true)`, respectively.
* For the logical AND (&amp;&amp;) operator, if one of
  the values is `bool(false)`, the result is also `bool(false)`.
* For the logical OR (`||`) operator, if one of
  the values is `bool(true)`, the result is also `bool(true)`.

For example:

```kusto
datatable(val:int)[5, int(null)]
| extend Add = val + 10
| extend Multiply = val * 10
```

Results:

|val |Add |Multiply|
|----|----|--------|
|5   |15  |50      |
|null|null|null    |

## Null values and the logical NOT operator

The logical NOT operator [not()](../notfunction.md) yields the value `bool(null)` if the argument is the null value.

## Null values and the in operator

* The [in operator](../inoperator.md) behaves like a logical OR of equality comparisons.
* The !in operator behaves like a logical AND
of inequality comparisons.

## Data ingestion and null values

For most data types, a missing value in the data source produces a null value in the corresponding table cell. However, columns of type `string` and CSV (or CSV-like) data formats are an exception to this rule, and a missing value produces an empty string.

For example:

```kusto
.create table T(a:string, b:int)

.ingest inline into table T
[,]
[ , ]
[a,1]
```

|a     |b     |isnull(a)|isempty(a)|strlen(a)|isnull(b)|
|------|------|---------|----------|---------|---------|
|&nbsp;|&nbsp;|false    |true      |0        |true     |
|&nbsp;|&nbsp;|false    |false     |1        |true     |
|a     |1     |false    |false     |1        |false    |

::: zone pivot="azuredataexplorer"

> [!NOTE]
> * If you run the above query in Kusto.Explorer, all `true` values will be displayed as `1`, and all `false` values will be displayed as `0`.
> * Kusto doesn't offer a way to constrain a table's column from having null values. In other words, there's no equivalent to SQL's `NOT NULL` constraint.

::: zone-end

::: zone pivot="azuremonitor"

> [!NOTE]
> Kusto doesn't offer a way to constrain a table's column from having null values. In other words, there's no equivalent to SQL's `NOT NULL` constraint.

::: zone-end
