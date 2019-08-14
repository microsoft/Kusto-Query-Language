# Functions

**Functions** are reusable queries or query parts. Kusto supports several
kinds of functions:

* **Stored functions**, which are user-defined functions that are stored and managed
  a one kind of a database's schema entities.
  See [Stored functions](../schema-entities/stored-functions.md).
* **Query-defined functions**, which are user-defined functions that are defined
  and used within the scope of a single query. The definition of such functions
  is done through a [let statement](../letstatement.md).
  See [User-defined functions](./user-defined-functions.md).
* **Built-in functions**, which are hard-coded (defined by Kusto and cannot be
  modified by users).
