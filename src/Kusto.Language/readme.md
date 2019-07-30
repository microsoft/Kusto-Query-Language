# Parsing Kusto queries using Kusto.Language

You can parse a Kusto query or control command by using the `KustoCode.Parse` method.

```csharp
// parse only
var query = KustoCode.Parse("T | project a = a + b | where a > 10.0");
```

This produces a `KustoCode` instance that contains the parsed syntax tree.
The tree can be navigated using a variety of API's
like `GetDescendants`, `GetAncestors`, `GetChild`, `Parent`, `WalkElements`, `GetTokenAt`, or `GetNodeAt`.

If you wanted to find all the places where a particular name was referenced in the syntax tree
you can search the tree for all places where a `NameReference` node has that particular name.
```csharp
// search syntax for a name reference of "a"
var referencesToA = query.Syntax.GetDescendants<NameReference>(n => n.SimpleName == "a");

// there are two NameReference nodes with name "a" and one NameDeclaration
Assert.AreEqual(2, referencesToA.Count);
```

---
This contrived query example `T | project a = a + b | where a > 10.0` includes a redeclaration of the name `a` in the `project` operator. This new column `a` is not the same
column as the original column `a` from table `T`. When searching just syntax, it is not easy to distinguish between the two.

You can, however, quickly tell which column is being referenced if semantic analysis has been performed.
Semantic analysis is the process that determines what exactly all the names refer to 
and checks the query for errors, like names that don't refer to any known table, column or variable.

To get the query analyzed, you instead call the `KustoCode.ParseAndAnalyze` method, passing in a `GlobalState` instance
that contains the schema for the database tables and functions that the query may reference.

```csharp
using Kusto.Language;
using Kusto.Language.Syntax;
using Kusto.Language.Symbols;
...

// semantic analysis needs any schema of tables and functions the query might reference
var database = 
  new DatabaseSymbol("db",
    new TableSymbol("T",
      new ColumnSymbol("a", ScalarTypes.Real),
      new ColumnSymbol("b", ScalarTypes.Real)));

// create new globals with default database set
var globals = GlobalState.Default.WithDatabase(database);

// parse query and perform semantic analysis
var query = KustoCode.ParseAndAnalyze("T | project a = a + b | where a > 10.0", globals);
```

Now when you navigate the syntax tree you can access the `ReferencedSymbol` and `ResultType` properties
that tell you what is being referenced and the type of any expression.

For this example, you can simply check the `ReferencedSymbol` property to see if it exactly matches the 
instance of the `ColumnSymbol` that was defined as part of table `T` when you declared the schema.

```csharp
// search syntax tree for references to specific columns
var columnA = database.Tables.First(t => t.Name == "db").GetColumn("a");
var referencesToA = query.Syntax.GetDescendants<NameReference>(n => n.ReferencedSymbol == columnA);

// there is only one reference to the column named "a" from the table "T"
Assert.AreEqual(1, referencesToA.Count);
```

---
If you wanted to see if any errors were detected, you can call the `GetDiagnostics` method.

```csharp
var dx = query.GetDiagnostics();
if (dx.Count > 0) { ... }
```

The method `GetDiagnostics` will gather all the syntax and semantic errors found in the query into a single collection.
It is possible to get diagnostics for a query that has not undergone semantic analysis.
In this case, you would only get the syntax errors found during parsing.