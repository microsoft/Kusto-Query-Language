# column_ifexists()

Takes a column name as a string and a default value. Returns a reference to the column if it exists, 
otherwise - returns the default value.

**Syntax**

`column_ifexists(`*columnName*`, `*defaultValue*)

**Arguments**

* *columnName*: The name of the column
* *defaultValue*: The value to use if the column doesn't exist in the context that the function was used in.
				  This value can be any scalar expression (e.g. a reference to another column).

**Returns**

If *columnName* exists, then the column it refers to. Otherwise - *defaultValue*.

**Examples**

<!-- csl -->
```
.create function with (docstring = "Wraps a table query that allows querying the table even if columnName doesn't exist ", folder="My Functions")
ColumnOrDefault(tableName:string, columnName:string)
{
	// There's no column "Capital" in "StormEvents", therefore, the State column will be used instead
	table(tableName) | project column_ifexists(columnName, State)
}


ColumnOrDefault("StormEvents", "Captial");
```
