# Using Kusto.Language
You'll want to include these namespaces in your C# code for the following examples to work.
```csharp
    using Kusto.Language;
    using Kusto.Language.Symbols;
    using Kusto.Language.Syntax;
```
&nbsp;
## Parsing a query

You can parse a Kusto query or control command by using the `KustoCode.Parse` method.

```csharp
    // parse only
    var query = "T | project a = a + b | where a > 10.0";
    var code = KustoCode.Parse(query);
```

This produces a `KustoCode` instance that contains the parsed syntax tree.
The tree can be navigated using a variety of API's
like `GetDescendants`, `GetAncestors`, `GetChild`, `Parent`, `WalkNodes`, `GetTokenAt`, or `GetNodeAt`.

If you wanted to find all the places where a particular name was referenced in the syntax tree
you can search the tree for all places where a `NameReference` node has that particular name.
```csharp
    // search syntax for a name reference of "a"
    var referencesToA = code.Syntax.GetDescendants<NameReference>(n => n.SimpleName == "a");

    // there are two NameReference nodes with name "a" and one NameDeclaration
    Assert.AreEqual(2, referencesToA.Count);
```

However, notice that the query contains two different columns named `a`.
The column `a` supposedly defined by table `T` and the column `a` declared by the `project` operator.
Given only syntax, it is not possible to distinguish that the column `a` referred to in the `where` operator 
is not the same as the column declared by the table.

In order to correctly distinguish between the two, you can request that the parser perform semantic analysis
and identify for you which column is which.

&nbsp;
## Parsing a query with semantic analysis enabled

Semantic analysis is the process that determines what exactly all the names refer to 
and checks the query for semantic errors.

When semantic analysis has been performed you will be able to determine which piece of syntax
refers to which exact column, variable, function or table.

To parse with semantic analysis enabled, use the `KustoCode.ParseAndAnalyze` method.
This method allows you to specify a `GlobalState` instance that contains the definition for the database tables 
and functions that the query may reference. These definitions are called symbols.

```csharp
    var globals = GlobalState.Default.WithDatabase(
        new DatabaseSymbol("db",
            new TableSymbol("T", "(a: real, b: real)")));

    var query = "T | project a = a + b | where a > 10.0";
    var code = KustoCode.ParseAndAnalyze(query, globals);
```

Now when you navigate the syntax tree you can access the `ReferencedSymbol` and `ResultType` properties
that tell you what is being referenced and the type of any expression.

For this example, you can simply check the `ReferencedSymbol` property to see if it exactly matches the 
instance of the `ColumnSymbol` that was defined as part of table `T` when you declared the schema.

```csharp
    // search syntax tree for references to specific columns
    var columnA = globals.Database.Tables.First(t => t.Name == "db").GetColumn("a");
    var referencesToA = code.Syntax.GetDescendants<NameReference>(n => n.ReferencedSymbol == columnA);

    // there is only one reference to the column named "a" from the table "T"
    Assert.AreEqual(1, referencesToA.Count);
```
&nbsp;
## Discovering errors identified during parsing

If you wanted to see if any errors were detected during parsing or semantic analysis, 
you can call the `GetDiagnostics` method.

```csharp
    var diagnostics = code.GetDiagnostics();
    if (diagnostics.Count > 0) { ... }
```

The method `GetDiagnostics` will return all the syntactic and semantic errors found in the query.
It is possible to get diagnostics for a query that has not undergone semantic analysis.
In this case, you would only get the syntax errors found during parsing.

Not all diagnostics are errors. Check the `Severity` property to see if it is an error, warning or other type of diagnostic.

&nbsp;
## Finding all the database table columns referenced in a query

You can find all the columns referenced in a query by walking over the syntax nodes of the parsed-and-analyzed query 
and checking each node if the symbol it references is a column.

```csharp
    var isAColumn = node.ReferencedSymbol is ColumnSymbol;
```

But that by itself will not tell you if the column being referenced is a database table column.
Even though most columns you will find will have originated from the table schema you provided to the `ParseAndAnalyze` method,
some will have been introduced explicitly by the query author adding or renaming columns 
or implicitly by the query operators themselves.

It is easy to determine if a column originated from the schema by checking if the `GlobalState` instance knows about it.

```chsarp
    var isDatabaseTableColumn = globals.GetTable(column) != null;
```

However, some `ColumnSymbol` instances may not be recognized by the `GlobalState` even when they represent columns from the schema.
This may be due to columns being renamed in the query or by `ColumnSymbol` instances that represent columns from multiple sources
such as columns that are the result of union operations.

If a new `ColumnSymbol` has been introduced during analysis because it represents one or more other columns,
you will be able to find the columns it represents in the `OriginalColumns` property.

To simplify things, you can define some methods that will help you collect all the database table columns.

```csharp
    public static bool IsDatabaseTableColumn(ColumnSymbol column, GlobalState globals)
    {
        return globals.GetTable(column) != null;
    }

    public static void AddDatabaseTableColumns(ColumnSymbol column, GlobalState globals, HashSet<ColumnSymbol> columns)
    {
        if (IsDatabaseTableColumn(column, globals))
        {
            columns.Add(column);
        }
        else if (column.OriginalColumns.Count > 0)
        {
            foreach (var originalColumn in column.OriginalColumns)
            {
                AddDatabaseTableColumns(originalColumn, globals, columns);
            }
        }
    }
```

Now you can make a method that finds all the database table columns using the `SyntaxElement.WalkNodes` method.
This method does a stack safe, lexical order, top down traversal of the syntax nodes.

```csharp
    public static HashSet<ColumnSymbol> GetDatabaseTableColumns(KustoCode code)
    {
        var columns = new HashSet<ColumnSymbol>();

        SyntaxElement.WalkNodes(code.Syntax,
            n =>
            {
                if (n.ReferencedSymbol is ColumnSymbol c)
                {
                    AddDatabaseTableColumns(c, code.Globals, columns);
                }
            });

        return columns;
    }
```

The following examples shows using the `GetDatabaseTableColumns` function on a simple query.

```csharp
    var globals = GlobalState.Default.WithDatabase(
        new DatabaseSymbol("db",
            new TableSymbol("Shapes", "(id: string, width: real, height: real)"),
            new FunctionSymbol("TallShapes", "{ Shapes | where width < height; }")
            ));

    var query = "TallShapes | where width > 5 | project id, width";
    var code = KustoCode.ParseAndAnalyze(query, globals);
    var dbColumns = GetDatabaseTableColumns(code);
```

The result `dbColumns` contains the column symbols for the `width` and `id` columns, because these
are the columns explicitly referenced within the body of the query.

You might be wondering about the `height` column, since it was referenced too.
It was not included because it was referenced inside the body of the database function `TallShapes`.
Therefore, it did not appear in any of the syntax nodes that were traversed.

You probably wanted to find all the columns referenced, even the ones referenced inside database functions.
To do this you will also need to examine the bodies of any functions called by the query.
You can do this using the `GetCalledFunctionBody()` method available on syntax nodes.
It will return the root of a different syntax tree defining the body of the function.
You can use it to recursively drill down into any function call, even function calls within function calls.

Here is the improved version.

```csharp
    public static HashSet<ColumnSymbol> GetDatabaseTableColumns(KustoCode code)
    {
        var columns = new HashSet<ColumnSymbol>();
        GatherColumns(code.Syntax);
        return columns;

        void GatherColumns(SyntaxNode root)
        {
            SyntaxElement.WalkNodes(root,
                fnBefore: n =>
                {
                    if (n.ReferencedSymbol is ColumnSymbol c)
                    {
                        AddDatabaseTableColumns(c, code.Globals, columns);
                    }
                    else if (n.GetCalledFunctionBody() is SyntaxNode body)
                    {
                        GatherColumns(body);
                    }
                },
                fnDescend: n =>
                    // skip descending into function declarations since their bodies will be examined by the code above
                    !(n is FunctionDeclaration)
                );
        }
    }
```

Now when the function is used to find all the columns referenced by the query, 
it will include the column `height` referenced inside the `TallShapes` function.

*Note: This functionality and others can be found in the **Kusto.Toolkit** library availble on nuget:*  
https://www.nuget.org/packages/Kusto.Toolkit/

&nbsp;
## Finding all the database tables referenced in a query

In a manner similar to finding all the columns referenced in a query
you can find all the database tables too.

```csharp
    public static HashSet<TableSymbol> GetDatabaseTables(KustoCode code)
    {
        var tables = new HashSet<TableSymbol>();

        SyntaxElement.WalkNodes(code.Syntax,
            n =>
            {
                if (n.ReferencedSymbol is TableSymbol t
                    && code.Globals.IsDatabaseTable(t))
                {
                    tables.Add(t);
                }
                else if (n is Expression e
                    && e.ResultType is TableSymbol ts
                    && code.Globals.IsDatabaseTable(ts))
                {
                    tables.Add(ts);
                }
            });

        return tables;
    }
```

This approach checks for all tables explicitly named in the query and any table
that might be indirectly introduced into the query via a function call 
by checking the result type of expressions.

However, like with columns, this approach does not consider tables referenced inside the body of called functions.
Since the `TallShapes` function references the `Shapes` table, you probably want to include it.

You can improve upon it by using the same recursive technique used to find columns.

```csharp
    public static HashSet<TableSymbol> GetDatabaseTables(KustoCode code)
    {
        var tables = new HashSet<TableSymbol>();
        GatherTables(code.Syntax);
        return tables;

        void GatherTables(SyntaxNode root)
        {
            SyntaxElement.WalkNodes(root,
                fnBefore: n =>
                {
                    if (n.ReferencedSymbol is TableSymbol t
                        && code.Globals.IsDatabaseTable(t))
                    {
                        tables.Add(t);
                    }
                    else if (n is Expression e
                        && e.ResultType is TableSymbol ts
                        && code.Globals.IsDatabaseTable(ts))
                    {
                        tables.Add(ts);
                    }
                    else if (n.GetCalledFunctionBody() is SyntaxNode body)
                    {
                        GatherTables(body);
                    }
                },
                fnDescend: n =>
                    // skip descending into function declarations since their bodies will be examined by the code above
                    !(n is FunctionDeclaration)
                );
        }
    }
```
  
*Note: This functionality and others can be found in the **Kusto.Toolkit** library availble on nuget:*  
https://www.nuget.org/packages/Kusto.Toolkit/


&nbsp;
## Finding the table for a column

If a column is part of a database table, you can discover that table using the `GetTable` method 
on the `GlobalState` that includes the database table definition.

```csharp
    var table = code.Globals.GetTable(column);
```

&nbsp;
## Finding the database for a table 

If a table symbol is declared as part of a database, you can discover which database is associated with the table
using the `GetDatabase` method on the `GlobalState` that includes the definition of the database and table.

```csharp
    var database = code.Globals.GetDatabase(table);
```

&nbsp;
## Finding the cluster for a database

You can discover the cluster that a database belongs to using the `GetCluster` method on the `GlobalState`
that includes the definition of both the cluster and database symbols.

```csharp
    var cluster = code.Globals.GetCluster(database);
```

&nbsp;
---
## Declaring database schemas

In order to have the parser understand the existence of any particular database tables or functions, it must be told about them first.
You tell the parser about the tables and functions by adding `DatabaseSymbol` instances to the `GlobalState` instance you use with the `ParseAndAnalyze` method.

You can declare tables by constructing `TableSymbol` instances.

```csharp
    var shapes = new TableSymbol("Shapes", "(id: string, width: real, height: real)");
```

You can declare functions by constructing `FunctionSymbol` instances.
Functions can be declared with our without parameters.

```csharp
    var tallshapes = new FunctionSymbol("TallShapes", "{ Shapes | width < height; }");
    var shortshapes = new FunctionSymbol("ShortShapes", "(maxHeight: real)", "{ Shapes | height < maxHeight; }");
```

Once you have all the tables and function symbols you can create a `DatabaseSymbol`.

```csharp
    var mydb = new DatabaseSymbol("mydb", shapes, tallshapes, shortshapes);
```

In order to use your database symbol it must be in the `GlobalState` instance.
Since `GlobalState` is an immutable type, you add to it by making new instances using the `With` methods.

One way to get a `GlobalState` to know about your database is to modify the default database symbol that is in scope when you query is analyzed.

```csharp
    var globalsWithMyDb = GlobalState.Default.WithDatabase(mydb);
```

Another way is to add a cluster definition. It can contain multiple databases.

```csharp
    var mycluster = new ClusterSymbol("mycluster.kusto.windows.net", mydb);
```

Once you have the cluster, you can either add it as the default cluster and the database as the default database or
add it as a separate cluster (not the default).

```csharp
    var globalsWithMyDefaultCluster = GlobalState.Globals.WithCluster(mycluster).WithDatabase(mydb);
    var globalsWithMyClusterAdded = GlobalState.Globals.AddOrReplaceCluster(mycluster);
```

If you add the cluster that is not he default, you must use the `cluster()` function to access it in the query.
Likewise, if you have a database that is not the default, you must use the `database()` function to access it.


&nbsp;
## Using database schemas from the server

Declaring all these symbols manually is a bit tedious and unnecessary if you already have an existing Kusto database defined.
It is possible to construct all the necessary symbols by using Kusto itself to query the database and request all the schema information.

Unfortunately, there are no API's provided in the library to do this for you.

There is, however, a library available on nuget that includes the necessary API's to load the symbols:  
https://www.nuget.org/packages/Kusto.Toolkit/

The source code to this library is available at:  
https://github.com/mattwar/Kusto.Toolkit


&nbsp;
---
## Adding built-in functions and aggregates

You can give the parser additional functions and aggregates to know about even though they might not really exist in the server 
by adding them to the global state instance you use when you call the `ParseAndAnalyze` method.

```csharp
    var fnFake = new FunctionSymbol("fake", ScalarTypes.Real, new Parameter("x", ScalarTypes.Long), new Parameter("y", ScalarTypes.Long));
    var globals = GlobalState.Default.WithFunctions(globals.Functions.Concat(new [] {fnFake}).ToArray());
    var code = KustoCode.ParseAndAnalyze("print fake(10)", globals);
```

```csharp
    var fnMinMax = new FunctionSymbol("minmax", ScalarTypes.Real, new Parameter("x", ScalarTypes.Real));
    var globals = GlobalState.Default.WithAggregates(globals.Aggregates.Concat(new [] {fnMinMax}).ToArray());
    var code = KustoCode.ParseAndAnalyze("T | summarize minmax(c)", globals);
```

Likewise, if you remove functions or aggregates from the global state's list the parser will produce an error when they are used.

