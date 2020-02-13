# serialize operator

Marks that order of the input row set is safe for window functions usage.

Operator has declarative meaning, and it marks input row set as serialized (ordered) so [window functions](./windowsfunctions.md) could be applied to it.

<!-- csl -->
```
T | serialize rn=row_number()
```

**Syntax**

`serialize` [*Name1* `=` *Expr1* [`,` *Name2* `=` *Expr2*]...]

* The *Name*/*Expr* pairs are similar to those in the [extend operator](./extendoperator.md).

**Example**

<!-- csl -->
```
Traces
| where ActivityId == "479671d99b7b"
| serialize

Traces
| where ActivityId == "479671d99b7b"
| serialize rn = row_number()
```

The output row set of the following operators is marked as serialized:

[range](./rangeoperator.md), [sort](./sortoperator.md), [order](./orderoperator.md), [top](./topoperator.md), [top-hitters](./tophittersoperator.md), [getschema](./getschemaoperator.md).

The output row set of the following operators is marked as non-serialized:

[sample](./sampleoperator.md), [sample-distinct](./sampledistinctoperator.md), [distinct](./distinctoperator.md), [join](./joinoperator.md), 
[top-nested](./topnestedoperator.md), [count](./countoperator.md), [summarize](./summarizeoperator.md), [facet](./facetoperator.md), [mv-expand](./mvexpandoperator.md), 
[evaluate](./evaluateoperator.md), [reduce by](./reduceoperator.md), [make-series](./make-seriesoperator.md)

All other operators preserve the serialization property (if the input row set is serialized, so is the output row set).
