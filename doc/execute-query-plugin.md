# execute_query plugin

The `execute_query` plugin executes a query provided as a `string` value and makes
its first record set available for further processing.

**Syntax**

`evaluate` `execute_queryquery` `(` *KustoConnectionString* `,` *QueryText* `)`

**Arguments**

* *ConnectionStringKusto*: A `string` literal. Currently only a single dot
  (`.`) may appear as a value, indicating the outer query's cluster and database
  in scope.
  
* *QueryText*: A `string` value holding the query to run.
  If the `evaluate` operator has no tabular input, the query has no tabular input;
  Otherwise, the query will have that tabular
  input as its own input (see examples below).

**Examples**

The following example highlights the differences between the use of this plugin
with and without tabular input. The first use of the plugin has no tabular
input, and the second invocation has the output of the first
invocation as its input. The second invocation query is not a valid
Kusto query if it is "standalone". It requires a piped-in tabular input.

<!-- csl -->
```
evaluate execute_query(".", "print First='Hello'")
| evaluate execute_query(".", "extend b='World!'")
```

The following example executes a random query from a set of queries stored
in a table.

<!-- csl -->
```
let Query=toscalar(datatable (Text:string) [
  "print a=1",
  "print 'Hello'",
  "range x from 1 to 10 step 1 | project x*x"
  ]
  | summarize Candidates=make_set(Text)
  | project tostring(Candidates[toint(rand(array_length(Candidates)))]));
evaluate execute_query(".", Query)
```
