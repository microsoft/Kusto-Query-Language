# Parsing Kusto queries using Kusto.Language

In its simplest form, one can parse a Kusto query by using `KustoQuery`:

```csharp
var queryText = "T | where a == b";
var query = Kusto.Language.KustoQuery.From(queryText);
var querySyntax = query.Syntas;
```

This parses the query text and produces a syntax tree.

To perform semantic analysis, so that the tables and columns being references may
be retrieved, one also needs to pass the schema of the databases being used:

```csharp
using Kusto.Language;

//...

var database = new DatabaseSynbol(...);
var globals = GlobalState.Default.WithDatabase(database);
var queryText = "T | where a == b";
var query = KustoQuery.From(queryText, globals);

var queryDiagnostics = query.Diagnotics; // Syntax and otherwise
var querySyntax = query.Syntax;
// Each Expression syntax node will have a ResultsSymbol
// and ReferencedSymbol properties populated
```
