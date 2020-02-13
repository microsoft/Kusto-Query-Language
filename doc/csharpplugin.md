

### Syntax


### Arguments

* *output_schema*: A `type` literal that defines the output schema of the tabular data, returned by the C# code.
    * The format is: `typeof(`*ColumnName*`:` *ColumnType* [, ...]`)`, for example: `typeof(col1:string, col2:long)`.
    * To extend the input schema, use the following syntax: `typeof(*, col1:string, col2:long)`.
* *script*: A `string` literal that is the valid C# script to be executed.
* *script_parameters*: An optional `dynamic` literal which is a property bag of name/value pairs to be passed to the
   C# script as a property bag (see [C# variables](#csharp-interface)).
* *hint.distribution*: An optional hint for the plugin's execution to be distributed across multiple cluster nodes.
   Default: `single`.
    * `single`: A single instance of the script will run over the entire query data.
    * `per_node`: If the query before the C# block is distributed, an instance of the script will run on each node over the data that it contains.

### CSharp interface

The C# script can include any number of classes and methods, but must implement a method with the following signature:

```c#
IEnumerable<TOutput> Process(IEnumerable<TInput> input, ICSharpSandboxContext context) 
{
}
``` 

Where:

* `input`: The input tabular data (the values of `T` above), as an `IEnumerable<TInput>`.
    * `TInput` is an automatically generated C# class, which is based on the input schema for the plugin.
        * For example: if the input schema is `(a:string, b:bool: c:dynamic)`, then `TInput` will be:

        ```c#
        public class TInput
        {
            public String   @a;
            public Boolean? @b;
            public String   @c;
        }
        ```

* `context`: An `ICSharpSandboxContext` object, which provides the following methods to retrieve any argument specified in *script_parameters*:

    ```c#
    string GetArgument(string argumentName);
    
    T GetArgument<T>(string argumentName, Func<string, T> conversionFunction);
    ```

* `result`: An `IEnumerable<TOutput>`returned by the by the `Process()` method in the C# script, whose value becomes the tabular data that gets sent to
            any Kusto query operator that follows the plugin.

    * `TOutput` is an automatically generated C# class, which is based on the output schema for the plugin (as defined in *output_schema*).
        * For example: if the output schema is `(a:int, b:long)`, then `TOutput` will be:

        ```c#
        public class TOutput
        {
            public Int32? @a;
            public Int64? @b;
        }
        ```

The names of the arguments/variables (`input` and `context` in the example above) can be custom.

### Onboarding

* The plugin is disabled by default.
    * *Interested in enabling the plugin on your cluster?*
        * Disabling the plugin requires opening a support ticket as well.

### Notes and Limitations

* The `dynamic` datatype is treated as `String` by the plugin, for both `TInput` and `TOutput`.One is required to de/serialize these as part of the script.
* The C# sandbox limits accessing the network, therefore the C# code can't dynamically install additional packages that are

### Examples

<!-- csl -->
```
range x from 1 to 360 step 1
| as T
| evaluate csharp(
    typeof(*, fx:double), //  Output schema: append a new fx column to original table 
    'IEnumerable<TOutput> Process(IEnumerable<TInput> input, ICSharpSandboxContext context)' //  The C# decorated script
    '{'
    '    var n = context.GetArgument("count", int.Parse);'
    '    var g = context.GetArgument("gain", int.Parse);'
    '    var f = context.GetArgument("cycles", int.Parse);'
    ''
    '    foreach (var row in input) '
    '    {'
    '       yield return new TOutput { x = row.x, fx = MyCalculator.Calculate(g, n, f, row.x) };'
    '    }'
    '}'
    ''
    'public static class MyCalculator'
    '{'
    '    public static double Calculate(int g, int n, int f, long? value)'
    '    {'
    '        return value.HasValue ? g * Math.Sin((double)value / n * 2 * Math.PI * f) : 0.0;'
    '    }'
    '}',
    pack('gain', 5, 'cycles', 8, 'count', toscalar(T | count)) // dictionary of parameters
)
| render linechart
```

![alt text](./images/samples/sine-demo.png "sine-demo")

### Performance tips

* Reduce the plugin's input data set to the minimum amount required (columns/rows).
    * Use filters on the source data set, when possible, using Kusto's query language.
    * To perform a calculation on a subset of the source columns, project only those column before invoking the plugin.
* Use `hint.distribution = per_node` whenever the logic in your script is distributable.
    * You can also use the [partition operator](partitionoperator.md) for partitioning the input data set.
* Use Kusto's query language, whenever possible, to implement the logic of your C# script.

    For example:

    <!-- csl -->
    ```
    .show operations
    | where StartedOn > ago(7d) // Filtering out irrelevant records before invoking the plugin
    | project d = Duration / 1s // Projecting only a subset of the necessary columns
    | evaluate hint.distribution = per_node csharp( // Using per_node distribution, as the script's logic allows it
        typeof(*, d2:double),
        'IEnumerable<TOutput> Process(IEnumerable<TInput> input, ICSharpSandboxContext context)' //  The C# decorated script
        '{'
        '    foreach (var row in input) '
        '    {'
        '       yield return new TOutput { d = row.d, d2 = 2 * row.d };'  // Negative example: this logic should have been written using Kusto's query language
        '    }'
        '}'
    )
    ```

### Usage tips

* To avoid conflicts between Kusto string delimiters and C# ones, we recommend using single quote characters (`'`) for Kusto string 
  literals in Kusto queries, and double quote characters (`"`) for C# string literals in C# scripts.

* Use [externaldata operator](externaldata-operator.md) to obtain the content of
  a script that you've stored in an external location, such as Azure blob storage, a public GitHub repository, etc.
  
  For example:

    <!-- csl -->
    ```    
    let script = 
        externaldata(script:string)
        [h'https://raw.githubusercontent.com/yonileibowitz/kusto.blog/master/resources/csharp/sample_script.cs']
        with(format = raw);
    range x from 1 to 360 step 1
    | evaluate csharp(
        typeof(*, fx:double),
        toscalar(script), 
        pack('gain', 100, 'cycles', 4, 'count', toscalar(T | count)))
    | render linechart
    ```

* One can run the plugin in `debug` mode, in order to to get the auto-generated code as a string literal.
  For example:

    <!-- csl -->
    ```
    print c_string = "hello world!", c_datetime = now(), c_timespan = 10sec, c_double = 0.5, c_decimal = decimal(1.23)
    | evaluate csharp(typeof(c_bool:bool), 'debug')
    ```

    Will return a table with a single column, named `GeneratedCode`, with the following content:

    ```c#
    IEnumerable<TOutput> Process(IEnumerable<TInput> input, ICSharpSandboxContext context)
    {
    }

    public class TInput
    {
        public String @c_string;
        public DateTime? @c_datetime;
        public TimeSpan? @c_timespan;
        public Double? @c_double;
        public Decimal? @c_decimal;
    }

    public class TOutput
    {
        public Boolean? @c_bool;
    }
    ```
<#endif>
