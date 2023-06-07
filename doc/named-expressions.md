---
title: Named expressions in Azure Data Explorer
description: Learn how to optimally use named expressions in Azure Data Explorer.
ms.reviewer: zivc
ms.topic: reference
ms.date: 04/16/2023

---
# Optimize queries that use named expressions

This article discusses how to optimize repeat use of named expressions in a query.

In Kusto Query Language, you can bind names to complex expressions in several different ways:

* In a [let statement](letstatement.md)
* In the [as operator](asoperator.md)
* In the formal parameters list of [user-defined functions](functions/user-defined-functions.md)

When you reference these named expressions in a query, the following steps occur:

1. The calculation within the named expression is evaluated. This calculation produces either a scalar or tabular value.
1. The named expression is replaced with the calculated value.

If the same bound name is used multiple times, then the underlying calculation will be repeated multiple times. When is this a concern?

* When the calculations consume many resources and are used many times.
* When the calculation is non-deterministic, but the query assumes all invocations to return the same value.

## Mitigation

To mitigate these concerns, you can materialize the calculation results in memory during the query. Depending on the way the named calculation is defined, you'll use different materialization strategies:

### Tabular functions

Use the following strategies for tabular functions:

* **let statements and function parameters**: Use the [materialize()](materializefunction.md) function.
* **as operator**: Set the `hint.materialized` hint value to `true`.

For example, the following query uses the non-deterministic tabular [sample operator](sampleoperator.md):

> [!NOTE]
> Tables aren't sorted in general, so any table reference in a query is, by definition, non-deterministic.

**Behavior without using the materialize function**

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAytKzEtPVahQSCvKz1UwVCjJVzA0MFAoLkktUDDk5apRKE7MLchJhbATixVCQHRpXmZ+nkIIAOuM1MA7AAAA" target="_blank">Run the query</a>

```kusto
range x from 1 to 100 step 1
| sample 1
| as T
| union T
```

**Output**

|x|
|---|
|63|
|92|

**Behavior using the materialize function**

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAytKzEtPVahQSCvKz1UwVCjJVzA0MFAoLkktUDDk5apRKE7MLchJhbATixUyMvNK9HITS1KLMhNzMqtSU2xLikpTFUJA0qV5mfl5CiEAbnko81IAAAA=" target="_blank">Run the query</a>

```kusto
range x from 1 to 100 step 1
| sample 1
| as hint.materialized=true T
| union T
```

**Output**

|x|
|---|
|95|
|95|

### Scalar functions

Non-deterministic scalar functions can be forced to calculate exactly once by using [toscalar()](toscalarfunction.md).

For example, the following query uses the non-deterministic function, [rand()](randfunction.md):

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA8tJLVGoULBV0NBUqC5KzEvRMDQwMNCsteblygHKVEJlSvKLkxNzEos0EErAagqKMvOA+nVAqBKIAFmz1YBOAAAA" target="_blank">Run the query</a>

```kusto
let x = () {rand(1000)};
let y = () {toscalar(rand(1000))};
print x, x, y, y
```

**Output**

|print_0|print_1|print_2|print_3|
|---|---|---|---|
|166 |137 |70 |70|

## See also

* [Let statement](letstatement.md)
* [as operator](asoperator.md)
* [toscalar()](toscalarfunction.md)
