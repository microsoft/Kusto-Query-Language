# tdigest_merge()

Merges tdigest results (scalar version of the aggregate version [`tdigest_merge()`](tdigest-merge-aggfunction.md)).

Read more about the underlying algorithm (T-Digest) and the estimated error [here](percentiles-aggfunction.md#estimation-error-in-percentiles).

**Syntax**

`merge_tdigests(` *Expr1*`,` *Expr2*`, ...)`

`tdigest_merge(` *Expr1*`,` *Expr2*`, ...)` - An alias.

**Arguments**

* Columns which has the tdigests to be merged.

**Returns**

The result for merging the columns `*Expr1*`, `*Expr2*`, ... `*ExprN*` to one tdigest.

**Examples**

<!-- csl: https://help.kusto.windows.net:443/Samples -->
```
range x from 1 to 10 step 1 
| extend y = x + 10
| summarize tdigestX = tdigest(x), tdigestY = tdigest(y)
| project merged = tdigest_merge(tdigestX, tdigestY)
| project percentile_tdigest(merged, 100, typeof(long))
```

|percentile_tdigest_merged|
|---|
|20|
