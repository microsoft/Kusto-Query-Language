# External tables

An **external table** is a Kusto schema entity that references data stored outside Kusto database.

Similar to [tables](tables.md), an external table has a well-defined schema (an ordered list of column name and data type pairs). Unlike tables, data is stored and managed outside Kusto Cluster. Most commonly the data is stored in some standard format such as CSV, Parquet, Avro, and is not ingested by Kusto.

An **external table** is created once (see [External Table control commands](../../management/externaltables.md))
and can be referenced by its name using [external_table()](../../query/externaltablefunction.md) function. 

**Notes**

* External table names are case-sensitive.
* External table names canâ€™t overlap with Kusto table names.
* External table names follow the rules for [entity names](./entity-names.md).
* Maximum limit of external tables per database is 1,000.
* Kusto supports [exporting data to an external table](../../management/data-export/export-data-to-an-external-table.md) as well as [querying external tables](https://docs.microsoft.com/azure/data-explorer/data-lake-query-data).
