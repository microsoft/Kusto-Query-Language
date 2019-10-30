# Tables

Tables are named entities that hold data. A table has an ordered set
of [columns](./columns.md), and zero or more rows of data, each row holding one data value
for each of the columns of the table. The order of rows in the table is unknown
and does not in general affect queries, except for some tabular operators (such as
the [top operator](../topoperator.md)) that are inherently indetermined.

Tables occupy the same namespace as [stored functions](./stored-functions.md),
so if a stored function and a table both have the same name, the stored function
will be chosen.

**Notes**  

* Table names are case-sensitive.
* Table names follow the rules for [entity names](./entity-names.md).
* Maximum limit of tables per database is 10,000.


Details on how to create and manage tables can be found under [managing tables](../../management/tables.md)

## Table References

The simplest way to reference a table is by using its name. This can be done
for all tables that are in the database in context. So, for example, the following
query counts the records of the current database's `StormEvents` table:

<!-- csl:https://help.kusto.windows.net/Samples -->
```
StormEvents
| count
```

Note that an equivalent way to write the query above is by escaping the table
name:

<!-- csl:https://help.kusto.windows.net/Samples -->
```
["StormEvents"]
| count
```

Tables may also be referenced by explicitly noting the database (or database and
cluster) they are in, allowing one to author queries that combine data from
multiple databases and clusters. For example, the following query will work
with any database in context, as long as the caller has access to the target
database:

<!-- csl:https://help.kusto.windows.net/Samples -->
```
cluster("https://help.kusto.windows.net").database("Samples").StormEvents
| count
```

It is also possible to reference a table by using the [table() special function](../tablefunction.md),
as long as the argument to that function evaluates to a constant. For example:

<!-- csl:https://help.kusto.windows.net/Samples -->
```
let counter=(TableName:string) { table(TableName) | count };
counter("StormEvents")
```
