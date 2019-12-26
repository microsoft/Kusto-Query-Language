# Query parameters declaration statement

::: zone pivot="azuredataexplorer"

Queries sent to Kusto may include, in addition to the query text itself,
a set of name/value pairs called **query parameters**. The query text may then
reference one or more query parameters values by specifying their names and
type through a **query parameters declaration statement**.

Query parameters have two main uses:

1. As a protection mechanism against **injection attacks**.
2. As a way to parameterize queries.

In particular, client applications that combine user-provided input in queries
that they then send to Kusto should use this mechanism to protect against the
Kusto equivalent of [SQL Injection](https://en.wikipedia.org/wiki/SQL_injection)
attacks.

## Declaring query parameters

To be able to reference query parameters, the query text (or functions it uses)
must first declare which query parameter it uses. For each parameter, the
declaration provides the name and scalar type. Optionally, the parameter can
also have a default value to be used if the request doesn't provide a concrete
value for the parameter. Kusto then parses the query parameter's
value according to its normal parsing rules for that type.

**Syntax**

`declare` `query_parameters` `(` *Name1* `:` *Type1* [`=` *DefaultValue1*] [`,`...] `);`

* *Name1*: The name of a query parameter used in the query.
* *Type1*: The the corresponding type (e.g. `string`, `datetime`, etc.)
  The values provided by the user are encoded as strings, to Kusto will
  apply the appropriate parse method to the query parameter to get
  a strongly-typed value.
* *DefaultValue1*: An optional default value for the parameter. This must be
  a literal of the appropriate scalar type.

> [!NOTE]
> Like [user defined functions](functions/user-defined-functions.md),
> query parameters of type `dynamic` cannot have default values.

**Examples**

<!-- csl -->
```
declare query_parameters(UserName:string, Password:string);
print n=UserName, p=hash(Password)
```

<!-- csl -->
```
declare query_parameters(percentage:long = 90);
T | where Likelihood > percentage
```

## Specifying query parameters in a client application

The names and values of query parameters are provided as `string` values
by the application making the query. No name may repeat.

The interpretation of the values is done according to the query parameters
declaration statement. Every value is parsed as if it were a literal in the
body of a query according to the type specified by the query parameters
declaration statement.

### REST API

Query parameters are provided by client applications through the `properties`
slot of the request body's JSON object, in a nested property bag called
`Parameters`. For example, here's the body of a REST API call to Kusto
that calculates the age of some user (presumably by having the application
ask the user for his or her birthday):

``` json
{
    "ns": null,
    "db": "myDB",
    "csl": "declare query_parameters(birthday:datetime); print strcat(\"Your age is: \", tostring(now() - birthday))",
    "properties": "{\"Options\":{},\"Parameters\":{\"birthday\":\"datetime(1970-05-11)\",\"courses\":\"dynamic(['Java', 'C++'])\"}}"
}
```

### Kusto .NET SDK

To provide the names and values of query parameters when using the Kusto .NET
client library, one creates a new instance of the `ClientRequestProperties`
object and then uses the `HasParameter`, `SetParameter`, and `ClearParameter`
methods to manipulate query parameters. Note that this class provides a number
of strongly-typed overloads for `SetParameter`; internally, they generate the
appropriate literal of the query language and send it as a `string` through
the REST API, as described above. The query text itself must still [declare the query paramters](#declaring-query-parameters).

### Kusto.Explorer

To set the query parameters sent when making a request to the service,
use the **Query parameters** "wrench" icon (`ALT` + `P`).

::: zone-end

::: zone pivot="azuremonitor"

This isn't supported in Azure Monitor

::: zone-end
