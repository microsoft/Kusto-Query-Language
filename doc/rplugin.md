# R plugin (Preview)

::: zone pivot="azuredataexplorer"

The R plugin runs a user-defined-function (UDF) using an R script. The R script gets tabular data as its input, and is expected to produce tabular output.
The plugin's runtime is hosted in a [sandbox](../concepts/sandboxes.md), an isolated and secure environment running on the cluster's nodes.

### Syntax


### Arguments

* *output_schema*: A `type` literal that defines the output schema of the tabular data, returned by the R code.
    * The format is: `typeof(`*ColumnName*`:` *ColumnType* [, ...]`)`, for example: `typeof(col1:string, col2:long)`.
    * To extend the input schema, use the following syntax: `typeof(*, col1:string, col2:long)`.
* *script*: A `string` literal that is the valid R script to be executed.
* *script_parameters*: An optional `dynamic` literal which is a property bag of name/value pairs to be passed to the
   R script as the reserved `kargs` dictionary (see [Reserved R variables](#reserved-r-variables)).
* *hint.distribution*: An optional hint for the plugin's execution to be distributed across multiple cluster nodes.
   Default: `single`.
    * `single`: A single instance of the script will run over the entire query data.
    * `per_node`: If the query before the R block is distributed, an instance of the script will run on each node over the data that it contains.

### Reserved R variables

The following variables are reserved for interaction between Kusto Query Language and the R code:

* `df`: The input tabular data (the values of `T` above), as an R DataFrame.
* `kargs`: The value of the *script_parameters* argument, as an R dictionary.
* `result`: An R DataFrame created by the R script, whose value becomes the tabular data that gets sent to
            any Kusto query operator that follows the plugin.

### Onboarding

* The plugin is disabled by default.
    * *Interested in enabling the plugin on your cluster?*
        * Disabling the plugin requires opening a support ticket as well.

### Notes and Limitations

* The R sandbox image is based on *R 3.4.4 for Windows*, and includes packages from [Anaconda's R Essentials bundle](https://docs.anaconda.com/anaconda/packages/r-language-pkg-docs/).
* The R sandbox limits accessing the network, therefore the R code can't dynamically install additional packages that are

### Examples

<!-- csl -->
```
range x from 1 to 360 step 1
| evaluate r(
//
typeof(*, fx:double),               //  Output schema: append a new fx column to original table 
//
'result <- df\n'                    //  The R decorated script
'n <- nrow(df)\n'
'g <- kargs$gain\n'
'f <- kargs$cycles\n'
'result$fx <- g * sin(df$x / n * 2 * pi * f)'
//
, pack('gain', 100, 'cycles', 4)    //  dictionary of parameters
)
| render linechart 
```
![alt text](./images/samples/sine-demo.png "sine-demo")



### Performance tips

* Reduce the plugin's input data set to the minimum amount required (columns/rows).
    * Use filters on the source data set, when possible, using the Kusto Query Language.
    * To perform a calculation on a subset of the source columns, project only those column before invoking the plugin.
* Use `hint.distribution = per_node` whenever the logic in your script is distributable.
    * You can also use the [partition operator](partitionoperator.md) for partitioning the input data set.
* Whenever possible, use the Kusto Query Language to implement the logic of your R script.

    For example:

    <!-- csl -->
    ```    
    .show operations
    | where StartedOn > ago(1d) // Filtering out irrelevant records before invoking the plugin
    | project d_seconds = Duration / 1s // Projecting only a subset of the necessary columns
    | evaluate hint.distribution = per_node r( // Using per_node distribution, as the script's logic allows it
        typeof(*, d2:double),
        'result <- df\n'
        'result$d2 <- df$d_seconds\n' // Negative example: this logic should have been written using Kusto's query language
      )
    | summarize avg = avg(d2)
    ```

### Usage tips

* To avoid conflicts between Kusto string delimiters and R's ones, we recommend using single quote characters (`'`) for Kusto string 
  literals in Kusto queries, and double quote characters (`"`) for R string literals in R scripts.
* Use the [externaldata operator](externaldata-operator.md) to obtain the content of
  a script that you've stored in an external location, such as Azure blob storage, a public GitHub repository, etc.
  
  For example:

    <!-- csl -->
    ```    
    let script = 
        externaldata(script:string)
        [h'https://raw.githubusercontent.com/yonileibowitz/kusto.blog/master/resources/R/sample_script.r']
        with(format = raw);
    range x from 1 to 360 step 1
    | evaluate r(
        typeof(*, fx:double),
        toscalar(script), 
        pack('gain', 100, 'cycles', 4))
    | render linechart 
    ```

---

::: zone-end

::: zone pivot="azuremonitor"

This isn't supported in Azure Monitor

::: zone-end

