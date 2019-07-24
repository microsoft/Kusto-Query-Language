# serialize operator

Marks that order of the input row set is safe for window functions usage.

Operator has declarative meaning, and it marks input row set as serialized (ordered) so [window functions](./windowsfunctions.md) could be applied to it.

<!-- csl -->
```
T | serialize rn=row_number()
```

**Syntax**

`serialize` [*Name1* `=` *Expr1* [`,` *Name2* `=` *Expr2*]...]

* The *Name*/*Expr* pairs are similar to those in the [extend operatpr](./extendoperator.md).

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