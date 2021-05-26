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
---
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
---
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
---
## Finding all the database table columns referenced in a query

You can use the parsed query to discover all the columns explicitly referenced by the query that originate from a database table.

```csharp
    public static HashSet<ColumnSymbol> GetDatabaseTableColumns(KustoCode code)
    {
        var columns = new HashSet<ColumnSymbol>();

        SyntaxElement.WalkNodes(code.Syntax,
            n =>
            {
                if (n.ReferencedSymbol is ColumnSymbol c
                    && code.Globals.GetTable(c) != null)
                {
                    columns.Add(c);
                }
            });

        return columns;
    }
```

This function uses the `SyntaxElement.WalkNodes` method to traverse all the nodes in the syntax tree in lexical order, top down.
It checks each node to see if it references a `ColumnSymbol` and then uses the `GetTable` method on the `GlobalState`
object to check if the column is a member of a known database table.

The following examples shows using the `GetDatabaseTableColumns` function on a simple query.

```csharp
    var globals = GlobalState.Default.WithDatabase(
        new DatabaseSymbol("db",
            new TableSymbol("Shapes", "(id: string, width: double, height: double)"),
            new FunctionSymbol("TallShapes", "{ Shapes | where width < height; }")
            ));

    var query = "TallShapes | where width > 5 | project id, width";
    var code = KustoCode.ParseAndAnalyze(query, globals);
    var dbColumns = GetDatabaseTableColumns(code);
```

The result `dbColumns` contains the column symbols for the `width` and `id` columns, because these
are the only columns explicitly referenced within the body of the query.

However, since the query is actually over the function `TallShapes` instead of the `Shapes` table directly,
this naive approach to finding column references does not include any additional columns referenced by the function.

You can improve on this approach with a somewhat more elaborate function that recursively analyzes the function's body too.

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
                    if (n.ReferencedSymbol is ColumnSymbol c
                        && code.Globals.GetTable(c) != null)
                    {
                        columns.Add(c);
                    }
                    else if (n.GetExpansion() is SyntaxNode expansion)
                    {
                        GatherColumns(expansion);
                    }
                },
                fnDescend: n =>
                    // skip function declarations since expansion will already take care of it
                    !(n is FunctionDeclaration)
                );
        }
    }
```

This advanced function uses the `GetExpansion` method found on syntax nodes.
For nodes that refer to user or database functions (like a function call node), 
it will return the root node of the parsed function body.

You can use this expansion of the function to analyze its body for additional column references
by recursively calling the `GatherColumns` method.

The function also supplies the `fnDescend` delegate to the `SyntaxElemenet.WalkNodes` method,
using it to skip over the bodies of any let statement declared functions inside the query.
This will avoid duplicating work now handled by the recursion.

Now when the function is used to find all the columns referenced by the query, 
it will include the column `height` referenced by the `TallShapes` function itself.

&nbsp;
---
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

This simple approach checks for all tables explicitly named in the query and any table
that might be indirectly introduced into the query via a function call 
by checking the result type of expressions.

However, like with columns, this approach is naive and will not discover that
the `TallShapes` function references the `Shapes` table on your behalf.

You can improve upon it by using the same recursive technique used to find the columns.

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
                    else if (n.GetExpansion() is SyntaxNode expansion)
                    {
                        GatherTables(expansion);
                    }
                },
                fnDescend: n =>
                    // skip function declarations since expansion will already take care of it
                    !(n is FunctionDeclaration)
                );
        }
    }
```
&nbsp;
---
## Finding the table for a column
If a column is part of a database table, you can discover that table using the `GetTable` method 
on the `GlobalState` that includes the database table definition.

```csharp
    var table = code.Globals.GetTable(column);
```

&nbsp;
---
## Finding the database for a table
If a table symbol is declared as part of a database, you can discover which database is associated with the table
using the `GetDatabase` method on the `GlobalState` that includes the definition of the database and table.

```csharp
    var database = code.Globals.GetDatabase(table);
```

&nbsp;
---
## Finding the cluster for a database
You can discover the cluster that a database belongs to using the `GetCluster` method on the `GlobalState`
that includes the definition of both the cluster and database symbols.

```csharp
    var cluster = code.Globals.GetCluster(database);
```

&nbsp;
---
## Using existing database schemas
In order to get the most/best use of the Kusto.Language library, it is necessary to have a
`GlobalState` defined with all the of necessary database schemas that your query may reference.

Declaring all these symbols manually is a bit tedious and unnecessary if you already have an existing Kusto database.
It is possible to construct all the necessary symbols by using Kusto itself to query the database and request all the schema information.

Unfortunately, since the language library is used in scenarios that preclude including direct network calls to actual databases
there are no API's provided in the library to do this for you.

There is, however, another project on Github that includes the necessary code to load the symbols 
directly from the database into a form you can use.

https://github.com/mattwar/Kushy/blob/master/README.md

You can copy/paste the SymbolLoader.cs file from this project into yours.


