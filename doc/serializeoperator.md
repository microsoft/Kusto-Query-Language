---
title:  serialize operator
description: Learn how to use the serialize operator to mark the input row set as serialized and ready for window functions.
ms.reviewer: alexans
ms.topic: reference
ms.date: 03/16/2023
---
# serialize operator

Marks that the order of the input row set is safe to use for window functions.

The operator has a declarative meaning. It marks the input row set as serialized (ordered), so that [window functions](./windowsfunctions.md) can be applied to it.

## Syntax

`serialize` [*Name1* `=` *Expr1* [`,` *Name2* `=` *Expr2*]...]

## Parameters

| Name | Type | Required | Description |
| -- | -- | -- | -- |
| *Name* | string | | The name of the column to add or update. If omitted, the output column name will be automatically generated. |
| *Expr* | string | &check; | The calculation to perform over the input.|

## Examples

### Serialize subset of rows by condition

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/SampleLogs?query=H4sIAAAAAAAAAwspSkxO9clPL+blqlEoz0gtSlVwzslMzSsJSi0sTS0u8UxRsLVVUDJNtDCxSDM30LW0tDTTTU0yNNc1NE1N0TUyTLJITbI0SUozSFUCGVGcWpSZmJNZlQoAv59YuFkAAAA=" target="_blank">Run the query</a>

```kusto
TraceLogs
| where ClientRequestId == "5a848f70-9996-eb17-15ed-21b8eb94bf0e"
| serialize
```

### Add row number to the serialized table

To add a row number to the serialized table, use the [row_number()](rownumberfunction.md) function.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/SampleLogs?query=H4sIAAAAAAAAAwspSkxO9clPL+blqlEoScxOVTA0AADDD5pUFAAAAA==" target="_blank">Run the query</a>

```kusto
TraceLogs
| where ClientRequestId == "5a848f70-9996-eb17-15ed-21b8eb94bf0e"
| serialize rn = row_number()
```

## Serialization behavior of operators

The output row set of the following operators is marked as serialized.

* [getschema](./getschemaoperator.md)
* [range](./rangeoperator.md)
* [sort](./sort-operator.md)
* [top](./topoperator.md)
* [top-hitters](./tophittersoperator.md)

The output row set of the following operators is marked as non-serialized.

* [count](./countoperator.md)
* [distinct](./distinctoperator.md)
* [evaluate](./evaluateoperator.md)
* [facet](./facetoperator.md)
* [join](./joinoperator.md)
* [make-series](./make-seriesoperator.md)
* [mv-expand](./mvexpandoperator.md)
* [reduce by](./reduceoperator.md)
* [sample](./sampleoperator.md)
* [sample-distinct](./sampledistinctoperator.md)
* [summarize](./summarizeoperator.md)
* [top-nested](./topnestedoperator.md)

All other operators preserve the serialization property. If the input row set is serialized, then the output row set is also serialized.
