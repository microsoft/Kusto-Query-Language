# Cross-Database and Cross-Cluster Queries

Every Kusto query operates in the context of the current cluster and the default database.
* When using [Kusto Client Library](../api/netfx/about-kusto-data.md) the current cluster and the default database are specified by the `Data Source` and `Initial Catalog` properties of 
  the [Kusto connection strings](../api/connection-strings/kusto.md) respectively.

## Queries
To access tables from any database other than the default the *qualified name* syntax must be used:
To access database in the current cluster:
<!-- csl -->
```
database("<database name>").<table name>
```
Database in remote cluster:
<!-- csl -->
```
cluster("<cluster name>").database("<database name>").<table name>
```

*database name* is case-sensitive

*cluster name* is case-insensitive and can be of one of the following forms:
* Well-formed URL: example `http://contoso.kusto.windows.net:1234/`, only HTTP and HTTPS schemes are supported.
* Fully qualified domain name (FQDN): for instance `contoso.kusto.windows.net` - which will be equivalent to `https://`**`contoso.kusto.windows.net`**`:443/`
* Short name (host name [and region] without the domain part): for instance `contoso` - which is interpreted as `https://`**`contoso`**`.kusto.windows.net:443/`, or `contoso.westus` - which is interpreted as `https://`**`contoso.westus`**`.kusto.windows.net:443/`

>**Note:** cross-database access is subject to the usual permission checks.
So to be able to excute query user must have read permission to the default database and
to every other database referenced in the query (in the current and the remote clusters).

*Qualified name* can be used in any context a table name can be.
All of the following are valid:

<!-- csl -->
```
database("OtherDb").Table | where ...

union Table1, cluster("OtherCluster").database("OtherDb").Table2 | project ...

database("OtherDb1").Table1 | join cluster("OtherCluster").database("OtherDb2").Table2 on Key | join Table3 on Key | extend ...
```

When *qualified name* appears as an operand of the [union operator](./unionoperator.md) wildcards can be used to specify multiple tables
and multiple databases, wildcards are not allowed in cluster names:

<!-- csl -->
```
union withsource=TableName *, database("OtherDb*").*Table, cluster("OtherCluster").database("*").*
```

>**Note:** name of the default database is also a potential match, so database("&#42;").* specifies all tables of all databases
including the default.

>**Note:** for the discussion on how shema changes affect cross-cluster queries see [Cross-cluster queries and schema changes](../concepts/crossclusterandschemachanges.md)

## Restriction of Access
Qualified names or patterns can also be included in [restrict access](./restrictstatement.md) statement (wildcards in cluster names are not allowed)
<!-- csl -->
```
restrict access to (my*, database("MyOther*").*, cluster("OtherCluster").database("my2*").*);
```

The above will restrict the query to access the following entites:
* any entity with name starting with *my...* in the default database 
* any table in all the databases named *MyOther...* of the current cluster
* any table in all the databases named *my2...* in the cluster *OtherCluster.kusto.windows.net*

## Functions and Views
Functions and views (persistent and created inline) can refernce tables across database and cluster boundaries. The following is valid:

<!-- csl -->
```
let MyView = Table1 join database("OtherDb").Table2 on Key | join cluster("OtherCluster").database("SomeDb").Table3 on Key;
MyView | where ...
```

Persistent functions and views themselves can be accessed from another database in the same cluster:

Tabular function (view) in `OtherDb`:

<!-- csl -->
```
.create function MyView(v:string) { Table1 | where Column1 has v ...  }  
```

Scalar function in `OtherDb`:
<!-- csl -->
```
.create function MyCalc(a:double, b:double, c:double) { (a + b) / c }  
```

In default database:

<!-- csl -->
```
database("OtherDb").MyView("exception") | extend CalCol=database("OtherDb").MyCalc(Col1, Col2, Col3) | limit 10
```

Tabular functions or views can be referenced across clusters. So this is **valid**:
<!-- csl -->
```
cluster("OtherCluster").database("SomeDb").MyView("exception") | count
```

Scalar functions can only be accessed in the same cluster. So the following is **not valid**:

<!-- csl -->
```
MyTable | extend CalCol=cluster("OtherCluster").database("OtherDb").MyCalc(Col1, Col2, Col3) | limit 10
```

Functions that get one or more table arguments can only be accessed in the same cluster. So the following is **not valid**:

<!-- csl -->
```
cluster("OtherCluster").database("OtherDb").MyCalc(datatable(x:string, y:string)["x","y"] ) 
```

The schema of the remote function being called must be known and invariant of its parameters (see also [Cross-cluster queries and schema changes](../concepts/crossclusterandschemachanges.md)) . So the following is **not valid**:

Tabular function in `OtherDb`:
<!-- csl -->
```
.create function SomeTable(tablename:string) { table(tablename)  }  
```

In default database:
<!-- csl -->
```
cluster("OtherCluster").database("OtherDb").SomeTable("MyTable")
```


## Displaying data

Statements that return data to the client are implicitly limited
by the number of records returned, even if there's no specific use
of the `take` operator. To lift this limit, use the `notruncation`
client request option.

To display data in graphical form, use the [render operator](renderoperator.md).
