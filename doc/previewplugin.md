# preview plugin

Returns a table with up to the specified number of rows from the input record set, and the total number of records in the input record set.

<!-- csl -->
```
T | evaluate preview(50)
```

**Syntax**

`T` `|` `evaluate` `preview(` *NumberOfRows* `)`

**Returns**

The `preview` plugin returns two result tables:
* A table with up to the specified number of rows.
  For example, the sample query above is equivalent to running `T | take 50`.
* A table with a single row/column, holding the number of records in the
  input record set.
  For example, the sample query above is equivalent to running `T | count`.

**Tips**

If `evaluate` is preceded by a tabular source that includes a complex filter, or a filter that references most of the source table columns, prefer to use the [`materialize`](materializefunction.md) function. For example:

<!-- csl -->
```
let MaterializedT = materialize(T);
MaterializedT | evaluate preview(50)
```
