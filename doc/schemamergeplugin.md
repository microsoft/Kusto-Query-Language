# schema_merge plugin

Merges tabular schema definitions into unified schema. 

Schema definitions are expected to be in format as produced by [getschema](./getschemaoperator.md) operator.

The schema merge operation unions columns that appear in input schemas and tries to reduce
data types to a common data type (in case data types cannot be reduced, an error is shown on a problematic column).

<!-- csl -->
```
let Schema1=Table1 | getschema;
let Schema2=Table2 | getschema;
union Schema1, Schema2 | evaluate schema_merge()
```

**Syntax**

`T` `|` `evaluate` `schema_merge(` *PreserveOrder* `)`

**Arguments**

* *PreserveOrder*: (Optional) When set to `true`, directs to the plugin to validate that column order as defined
by the first tabular schema is kept. In other words if the same column appears in several schemas, the column ordinal
must be as in the first schema it appeared. Default value is `true`.

**Returns**

The `schema_merge` plugin returns output simiar to what [getschema](./getschemaoperator.md) operator returns.

**Examples**

Merge with a schema that has a new column appended:

<!-- csl -->
```
let schema1 = datatable(Uri:string, HttpStatus:int)[] | getschema;
let schema2 = datatable(Uri:string, HttpStatus:int, Referrer:string)[] | getschema;
union schema1, schema2 | evaluate schema_merge()
```

*Result*

|ColumnName | ColumnOrdinal | DataType | ColumnType|
|---|---|---|---|
|Uri|0|System.String|string|
|HttpStatus|1|System.Int32|int|
|Referrer|2|System.String|string|

Merge with a schema that has different column ordering (`HttpStatus` ordinal changes from `1` to `2` in the new variant):

<!-- csl -->
```
let schema1 = datatable(Uri:string, HttpStatus:int)[] | getschema;
let schema2 = datatable(Uri:string, Referrer:string, HttpStatus:int)[] | getschema;
union schema1, schema2 | evaluate schema_merge()
```

*Result*

|ColumnName | ColumnOrdinal | DataType | ColumnType|
|---|---|---|---|
|Uri|0|System.String|string|
|Referrer|1|System.String|string|
|HttpStatus|-1|ERROR(unknown CSL type:ERROR(columns are out of order))|ERROR(columns are out of order)|

Merge with a schema that has different column ordering, but with `PreserveOrder` set to `false` this time:

<!-- csl -->
```
let schema1 = datatable(Uri:string, HttpStatus:int)[] | getschema;
let schema2 = datatable(Uri:string, Referrer:string, HttpStatus:int)[] | getschema;
union schema1, schema2 | evaluate schema_merge(PreserveOrder = false)
```

*Result*

|ColumnName | ColumnOrdinal | DataType | ColumnType|
|---|---|---|---|
|Uri|0|System.String|string
|Referrer|1|System.String|string
|HttpStatus|2|System.Int32|int|
