
Kusto.Language uses Bridge.Net to translate the C# source into JavaScript in the Kusto.Language.Bridge project.

Things that trip up Bridge.Net and cause it to crash.

1) its stuck at C# 7.0 syntax, but not happy with tuples, switch expressions, new uses of default, etc
2) its had some bugs with #endregion and lack of whitespace, so be careful here
3) it has trouble with understanding overriding abstract properties. It is happier with virtual properties.
4) it does not like use of named parameters, so just avoid it
5) avoid goto's especially in switch (will not crash but turns it into a state-machine loop)
6) Don't use out var with primitive types (like Int32.TryParse). Normal out works but out var causes translation errors that only fail at runtime.
7) use of LINQ any operator with a lambda after using OfType<XXX> causes translator to fail

