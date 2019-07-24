# current_database()

Returns the name of the database in scope (database that all query
entities are resolved against if no other database is specified).

**Syntax**

`current_database()`

**Returns**

The name of the database in scope as a value of type `string`.

**Example**

<!-- csl -->
```
print strcat("Database in scope: ", current_database())
```
