# facet operator

Returns a set of tables, one for each specified column.
Each table specifies the list of values taken by its column.
An additional table can be created by using the `with` clause.

**Syntax**

*T* `| facet by` *ColumnName* [`, ` ...] [`with (` *filterPipe* `)`

**Arguments**

* *ColumnName:* The name of column in the input, to be summarized as an output table.
* *filterPipe:* A query expression applied to the input table to produce one of the outputs.

**Returns**

Multiple tables: one for the `with` clause, and one for each column.

**Example**

<!-- csl -->
```
MyTable 
| facet by city, eventType 
    with (where timestamp > ago(7d) | take 1000)
```
