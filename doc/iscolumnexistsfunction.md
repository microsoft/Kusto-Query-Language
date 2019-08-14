# iscolumnexists()

Returns a boolean value indicating if the given string argument exists in the schema produced by the preceding tabular operator.

**Syntax**

`iscolumnexists(`*value*`)

**Arguments**

* *value*: A string

**Returns**

A boolean indicating if the given string argument exists in the schema produced by the preceding tabular operator.
**Examples**

<!-- csl -->
```
.create function with (docstring = "Returns a boolean indicating whether a column exists in a table", folder="My Functions")
DoesColumnExistInTable(tableName:string, columnName:string)
{
	table(tableName) | limit 1 | project ColumnExists = iscolumnexists(columnName) 
}

DoesColumnExistInTable("StormEvents", "StartTime")
```
