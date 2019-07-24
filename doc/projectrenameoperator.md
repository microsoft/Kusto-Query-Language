# project-rename operator

Renames columns in the result output.

<!-- csl -->
```
T | project-rename new_column_name = column_name
```

**Syntax**

*T* `| project-rename` *NewColumnName* = *ExistingColumnName* [`,` ...]

**Arguments**

* *T*: The input table.
* *NewColumnName:* The new name of a column. 
* *ExistingColumnName:* The existing name of a column. 

**Returns**

A table that has the columns in the same order as in an existing table, with columns renamed.


**Examples**

<!-- csl: https://help.kusto.windows.net/Samples -->
```
print a='a', b='b', c='c'
|  project-rename new_b=b, new_a=a
```

|new_a|new_b|c|
|---|---|---|
|a|b|c|
