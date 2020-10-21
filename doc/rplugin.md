---
title: R plugin (Preview) - Azure Data Explorer
description: This article describes R plugin (Preview) in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 04/01/2020
zone_pivot_group_filename: data-explorer/zone-pivot-groups.json
zone_pivot_groups: kql-flavors
---
# R plugin (Preview)

::: zone pivot="azuredataexplorer"

The R plugin runs a user-defined-function (UDF) using an R script. 
The script gets tabular data as its input, and produces tabular output.
The plugin's runtime is hosted in a [sandbox](../concepts/sandboxes.md) on the cluster's nodes. The sandbox provides an isolated and secure environment.

## Syntax

*T* `|` `evaluate` [`hint.distribution` `=` (`single` | `per_node`)] `r(`*output_schema*`,` *script* [`,` *script_parameters*]`)`

## Arguments

* *output_schema*: A `type` literal that defines the output schema of the tabular data, returned by the R code.
    * The format is: `typeof(`*ColumnName*`:` *ColumnType*[, ...]`)`, for example: `typeof(col1:string, col2:long)`.
    * To extend the input schema, use the following syntax: `typeof(*, col1:string, col2:long)`.
* *script*: A `string` literal that is the valid R script to be executed.
* *script_parameters*: An optional `dynamic` literal that is a property bag of name and value pairs to be passed to the R script as the reserved `kargs` dictionary. For more information, see [Reserved R variables](#reserved-r-variables).
* *hint.distribution*: An optional hint for the plugin's execution to be distributed across multiple cluster nodes.
   Default: `single`.
    * `single`: A single instance of the script will run over the entire query data.
    * `per_node`: If the query before the R block is distributed, an instance of the script will run on each node over the data that it contains.

## Reserved R variables

The following variables are reserved for interaction between Kusto Query Language and the R code:

* `df`: The input tabular data (the values of `T` above), as an R DataFrame.
* `kargs`: The value of the *script_parameters* argument, as an R dictionary.
* `result`: An R DataFrame created by the R script. The value becomes the tabular data that gets sent to any Kusto query operator that follows the plugin.

## Enable the plugin

* The plugin is disabled by default.
* Enable or disable the plugin in the Azure portal in the **Configuration** tab of your cluster. For more information see [Manage language extensions in your Azure Data Explorer cluster (Preview)](../../language-extensions.md)

## Notes and limitations

* The R sandbox image is based on *R 3.4.4 for Windows*, and includes packages from [Anaconda's R Essentials bundle](https://docs.anaconda.com/anaconda/packages/r-language-pkg-docs/).
* The R sandbox limits access to the network. The R code can't dynamically install additional packages that aren't included in the image. If you need specific packages, open a **New support request** in the Azure portal.

## Examples

```kusto
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

:::image type="content" source="images/plugin/sine-demo.png" alt-text="Sine demo" border="false":::

## Performance tips

* Reduce the plugin's input data set to the minimum amount required (columns/rows).
    * Use filters on the source data set using the Kusto Query Language, when possible.
    * To make a calculation on a subset of the source columns, project only those columns before invoking the plugin.
* Use `hint.distribution = per_node` whenever the logic in your script is distributable.
    * You can also use the [partition operator](partitionoperator.md) for partitioning the input data set.
* Whenever possible, use the Kusto Query Language to implement the logic of your R script.

    For example:

    ```kusto    
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

## Usage tips

* To avoid conflicts between Kusto string delimiters and R string delimiters:  
    * Use single quote characters (`'`) for Kusto string literals in Kusto queries.
    * Use double quote characters (`"`) for R string literals in R scripts.
* Use the [external data operator](externaldata-operator.md) to obtain the content of
  a script that you've stored in an external location, such as Azure blob storage or a public GitHub repository.
  
  For example:

    ```kusto
    let script = 
        externaldata(script:string)
        [h'https://kustoscriptsamples.blob.core.windows.net/samples/R/sample_script.r']
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

This capability isn't supported in Azure Monitor

::: zone-end

