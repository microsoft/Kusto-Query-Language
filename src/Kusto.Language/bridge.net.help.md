
Kusto.Language uses Bridge.Net to translate the C# source into JavaScript in the Kusto.Language.Bridge project.

Things that trip up Bridge.Net and cause it to crash.

1) its stuck at C# 7.0 syntax, but not happy with tuples, switch expressions, new uses of default, etc
2) its had some bugs with #endregion and lack of whitespace, so be careful here
3) it has trouble with understanding overriding abstract properties. It is happier with virtual properties.
4) it does not like use of named parameters, so just avoid it
5) avoid goto's especially in switch (will not crash but turns it into a state-machine loop)
6) Don't use 'out var' with primitive types (like Int32.TryParse) on nested in lambdas. Normal out works but 'out var' causes translation errors that only fail at runtime.
7) Don't use 'is var xxx' or 'is {} xxx', use 'is type xxx' instead.
8) use of LINQ any operator with a lambda after using OfType<XXX> causes translator to fail
9) Use of same lambda parameter name as name in outer scope.

Debugging Bridge.Net:
To debug bridge.net when you get an build exception (without any useful information).

1) Open Visual Studio (elevated permissions) w/o project or solution
2) Open project pointing to msbuild.exe on your machine
3) Once project is open, set these values in general properties:  
     >Executable:     Path to msbuild.exe (should be already set)  
     Arguments:      path to Kusto.Language.Bridge.csproj  
     DebuggerType:   Managed (.Net 4x)  
     Working Dir:    path to Kusto project root  
4) Make sure "Just My Code" debug option is disabled
5) Make sure all CLR exceptions are enabled
6) Debug project (F5):  
   **You should stop at actual thrown exception and be able to deduce 
     what part of Kusto.Language source code causes the problem by 
     actual exception message or other data on call stack.**
   - The Line info may be off due to bridge.net shenanigans, but location is probably nearby.
7) If a new problem is discovered, add it to the list above.
