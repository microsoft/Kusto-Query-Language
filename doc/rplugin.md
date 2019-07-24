# R plugin (Preview)

The R plugin runs a user-defined-function (UDF) using an R script. The R script gets tabular data as its input, and is expected to produce a tabular output.
The plugin's runtime is hosted in <#ifdef PAAS> a sandbox, an isolated and secure environment,<#endif> <#ifdef MICROSOFT>[sandboxes](../concepts/sandboxes.md),<#endif> running on the cluster's nodes.

### Syntax

<#ifdef PAAS>*T* `|` `evaluate` [`hint.distribution` `=` (`single` | `per_node`)] `r(`*output_schema*`,` *script* [`,` *script_parameters*]`)`<#endif>
<#ifdef MICROSOFT>*T* `|` `evaluate` [`hint.distribution` `=` (`single` | `per_node`)] `r(`*output_schema*`,` *script* [`,` *script_parameters*][`,` *external_artifacts*]`)`<#endif>

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
<#ifdef MICROSOFT>* *external_artifacts*: An optional (and **experimental**) `dynamic` literal which is a property bag of name/URL pairs of artifacts
    that are accessible from cloud storage and can be made available for the script to use at runtime.
    * Any URL that is referenced as part of this property bag is required to be included in the cluster's [Callout policy](../concepts/calloutpolicy.md).
    * The artifacts are made available for the script to consume from a local temporary directory, `D:/Temp`, and the names provided in the property bag are used as the local file names (see [example](#examples) below).
<#endif>

### Reserved R variables

The following variables are reserved for interaction between Kusto query language and the R code:

* `df`: The input tabular data (the values of `T` above), as an R DataFrame.
* `kargs`: The value of the *script_parameters* argument, as an R dictionary.
* `result`: An R DataFrame created by the R script, whose value becomes the tabular data that gets sent to
            any Kusto query operator that follows the plugin.

### Onboarding

<#ifdef MICROSOFT>* Prerequisites for enabling the plugin are listed [here](../concepts/sandboxes.md#prerequisites).<#endif>
* The plugin is disabled by default.
    * *Interested in enabling the plugin on your cluster?*
        <#ifdef MICROSOFT>* Open a [support ticket](https://aka.ms/kustosupport) in which you should specify
          you've read and acknowledged all the prerequisites, and have approval from the cluster's owner(s).<#endif>
		<#ifdef PAAS>* In the Azure portal, within your Azure Data Explorer cluster, select **New support request** in the left-hand menu.<#endif>
        * Disabling the plugin requires opening a support ticket as well.

### Notes and Limitations

* The R sandbox image is based on *R 3.4.4 for Windows*, and includes packages from [Anaconda's R Essentials bundle](https://docs.anaconda.com/anaconda/packages/r-language-pkg-docs/).
* The R sandbox limits accessing the network, therefore the R code can't dynamically install additional packages that are
  not included in the image.<#ifdef PAAS>Open a **New support request** in the Azure portal<#endif> <#ifdef MICROSOFT>Contact [Kusto Machine Learning DL](mailto:kustoml@microsoft.com)<#endif> if you need specific packages.
<#ifdef MICROSOFT>
* **[Ingestion from query](../management/data-ingestion/ingest-from-query.md) and [Update policies](../concepts/updatepolicy.md)**
    * It is possible to use the plugin in queries which are:
        1. Defined as part of an update policy, whose source table is ingested to using *non-streaming* ingestion.
        2. Run as part of a command which ingests from a query (e.g. `.set-or-append`).
    * In both the above cases, it's recommended to verify that the volume and frequency of the ingestion, as well as the complexity and
      resources utilization of the R logic are aligned with [sandbox limitations](../concepts/sandboxes.md#limitations), and the cluster's available resources.
      Failure to do so may result with [throttling errors](../concepts/sandboxes.md#errors).
    * It is *not* possible to use the plugin in a query which is defined as part of an update policy, whose source table is ingested to 
    using [*streaming* ingestion](../management/data-ingestion/streaming.md).<#endif>

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

<#ifdef MICROSOFT>
<!-- csl -->
```
print "This is an example for using 'external_artifacts'"
| evaluate r(
    typeof(File:string),
    'df <- as.data.frame(list(File=dir("D:/Temp", all.files = TRUE, recursive = TRUE, include.dirs = TRUE)));'
    'result <- df',
    external_artifacts = 
        dynamic({"this_is_my_first_file":"https://raw.githubusercontent.com/yonileibowitz/kusto.blog/master/resources/R/sample_script.r",
                 "this_is_a_script":"https://raw.githubusercontent.com/yonileibowitz/kusto.blog/master/resources/python/sample_script.py"})
)
```

| File                  |
|-----------------------|
| this_is_a_script      |
| this_is_my_first_file |
<#endif>


### Performance tips

* Reduce the plugin's input data set to the minimum amount required (columns/rows).
    * Use filters on the source data set, when possible, using Kusto's query language.
    * To perform a calculation on a subset of the source columns, project only those column before invoking the plugin.
* Use `hint.distribution = per_node` whenever the logic in your script is distributable.
    * You can also use the [partition operator](partitionoperator.md) for partitioning the input data set.
* Use Kusto's query language, whenever possible, to implement the logic of your R script.

    For example:

    <!-- csl -->
    ```    
    .show operations
    | where StartedOn > ago(7d) // Filtering out irrelevant records before invoking the plugin
    | project d_seconds = Duration / 1s // Projecting only a subset of the necessary columns
    | evaluate hint.distribution = per_node r( // Using per_node distribution, as the script's logic allows it
        typeof(*, 2d:double),
        'result <- df\n'
        'result$2d -< df$d_seconds\n' // Negative example: this logic should have been written using Kusto's query language
      )
    | summarize avg = avg(2d)
    ```

### Usage tips

* To avoid conflicts between Kusto string delimiters and R's ones, we recommend using single quote characters (`'`) for Kusto string 
  literals in Kusto queries, and double quote characters (`"`) for R string literals in R scripts.
* Use [externaldata operator](externaldata-operator.md) to obtain the content of
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

<#ifdef MICROSOFT>Please send feedback and questions about this plugin to [Kusto Machine Learning DL](mailto:kustoML@microsoft.com).<#endif>