# anyif() (aggregation function)

Arbitrarily chooses one record for each group in a [summarize operator](summarizeoperator.md) for which the predicate
is true, and returns the value of an expression over each such record.

**Syntax**

`summarize` `anyif` `(` *Expr*, *Predicate* )`

**Arguments**

* *Expr*: An expression over each record selected from the input to return.
* *Predicate*: Predicate to indicate which records may be
  considered for evaluation.

**Returns**

The `anyif` aggregation function returns the value of the expression calculated
for each of the records selected randomly from each group
of the summarize operator. Only records for which *Predicate* returns true may be selected (if the predicate doesn't return
true, a null value is produced).

**Remarks**

This function is useful when you want to get a sample value of one column
per value of the compound group key, subject to some predicate
being true.

The function attempts to
return a non-null/non-empty value, if such value is present.

**Examples**

Show random continent which has a population from 300 million to 600 million:

<!-- csl -->
```
Continents | summarize anyif(Continent, Population between (300000000 .. 600000000))
```

![alt text](./images/aggregations/any1.png "any1")
