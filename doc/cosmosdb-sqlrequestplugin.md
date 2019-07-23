<#ifdef MICROSOFT># cosmosdb_sql_request plugin

The `cosmosdb_sql_request` plugin sends a SQL query to a CosmosDB SQL network endpoint
and returns the first rowset in the results.

`evaluate` `cosmosdb_sql_request` `(` *CosmosDbUri* `,` *authorization_key* `,` *database_name* `,` *collection_name* `,` *SqlQuery* `)`

**Arguments**

* *CosmosDbUri*: A `string` literal indicating the URI of the Cosmos DB endpoint
  to connect to.

* *authorization_key*: A `string` literal specifying CosmosDB resource or master token.
  It is **strongly recommended** that this argument use the
  [obfuscated string literals](./scalar-data-types/string.md#obfuscated-string-literals)
  form to prevent secret leakage.

* *database_name*: A `string` literal specifying CosmosDB Database name.

* *collection_name*: A `string` literal specifying CosmosDB collection name.

* *SqlQuery*: A `string` literal indicating the query that is to be executed
  against the SQL endpoint. Must return one or more rowsets, but only the
  first one is made available for the rest of the Kusto query.

**Restrictions**

The plugin makes callouts to Cosmos DB and therefore one must make sure that the
cluster's [Callout policy](../concepts/calloutpolicy.md) enables calls of type
`cosmosdb` to the target *CosmosDbUri*.

Below you can find an example of defining call-out policy for CosmosDB. It is recommended to restrict it to specific endpoints (`my_endpoint1`, `my_endpoint2`).

```json
[
  {
    "CalloutType": "CosmosDB",
    "CalloutUriRegex": "my_endpoint1.documents.azure.com",
    "CanCall": true
  },
  {
    "CalloutType": "CosmosDB",
    "CalloutUriRegex": "my_endpoint2.documents.azure.com",
    "CanCall": true
  }
]
```

**Examples**

Using SQL query to fetch all rows from Cosmos DB:

<!-- csl -->
```
evaluate cosmosdb_sql_request(
  'CosmosDbUri',
  h@'AuthKey',
  'MyDatabaseName',
  'MyCollectionName',
  'SELECT * from c')
```
<#endif>