---
title: R plugin (Preview) - Azure Data Explorer
description: Learn how to use the R plugin (Preview) to run a user-defined function using an R script.
ms.reviewer: adieldar
ms.topic: reference
ms.date: 03/12/2023
zone_pivot_group_filename: data-explorer/zone-pivot-groups.json
zone_pivot_groups: kql-flavors
---
# R plugin (Preview)

::: zone pivot="azuredataexplorer"

The R plugin runs a user-defined function (UDF) using an R script.

The script gets tabular data as its input, and produces tabular output.
The plugin's runtime is hosted in a [sandbox](../concepts/sandboxes.md) on the cluster's nodes. The sandbox provides an isolated and secure environment.

## Syntax

*T* `|` `evaluate` [`hint.distribution` `=` (`single` | `per_node`)] `r(`*output_schema*`,` *script* [`,` *script_parameters*] [`,` *external_artifacts*]`)`

## Parameters

|Name|Type|Required|Description|
|--|--|--|--|
|*output_schema*|string|&check;|A `type` literal that defines the output schema of the tabular data, returned by the R code. The format is: `typeof(`*ColumnName*`:` *ColumnType*[, ...]`)`. For example: `typeof(col1:string, col2:long)`. To extend the input schema, use the following syntax: `typeof(*, col1:string, col2:long)`.|
|*script*|string|&check;|The valid R script to be executed.|
|*script_parameters*|dynamic||A property bag of name and value pairs to be passed to the R script as the reserved `kargs` dictionary. For more information, see [Reserved R variables](#reserved-r-variables).|
|`hint.distribution`|string||Hint for the plugin's execution to be distributed across multiple cluster nodes. The default value is `single`. `single` means that a single instance of the script will run over the entire query data. `per_node` means that if the query before the R block is distributed, an instance of the script will run on each node over the data that it contains.|
|*external_artifacts*|dynamic||A property bag of name and URL pairs for artifacts that are accessible from cloud storage. They can be made available for the script to use at runtime. URLs referenced in this property bag are required to be included in the cluster's [callout policy](../management/calloutpolicy.md) and in a publicly available location, or contain the necessary credentials, as explained in [storage connection strings](../api/connection-strings/storage-connection-strings.md). The artifacts are made available for the script to consume from a local temporary directory, `.\Temp`. The names provided in the property bag are used as the local file names. See [Example](#examples). For more information, see [Install packages for the R plugin](#install-packages-for-the-r-plugin).|

## Reserved R variables

The following variables are reserved for interaction between Kusto Query Language and the R code:

* `df`: The input tabular data (the values of `T` above), as an R DataFrame.
* `kargs`: The value of the *script_parameters* argument, as an R dictionary.
* `result`: An R DataFrame created by the R script. The value becomes the tabular data that gets sent to any Kusto query operator that follows the plugin.

## Enable the plugin

* The plugin is disabled by default.
* Enable or disable the plugin in the Azure portal in the **Configuration** tab of your cluster. For more information, see [Manage language extensions in your Azure Data Explorer cluster (Preview)](../../language-extensions.md)

## R sandbox image

* The R sandbox image is based on *R 3.4.4 for Windows*, and includes packages from [Anaconda's R Essentials bundle](https://docs.anaconda.com/anaconda/packages/r-language-pkg-docs/).

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
, bag_pack('gain', 100, 'cycles', 4)    //  dictionary of parameters
)
| render linechart 
```

:::image type="content" source="images/plugin/sine-demo.png" alt-text="Sine demo." border="false":::

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
        bag_pack('gain', 100, 'cycles', 4))
    | render linechart 
    ```

## Install packages for the R plugin

Follow these step by step instructions to install package(s) that aren't included in the plugin's base image.

### Prerequisites

  1. Create a blob container to host the packages, preferably in the same place as your cluster. For example, `https://artifactswestus.blob.core.windows.net/r`, assuming your cluster is in West US.
  1. Alter the cluster's [callout policy](../management/calloutpolicy.md) to allow access to that location.
        * This change requires [AllDatabasesAdmin](../management/access-control/role-based-access-control.md) permissions.

        * For example, to enable access to a blob located in `https://artifactswestus.blob.core.windows.net/r`, run the following command:

        ```kusto
        .alter-merge cluster policy callout @'[ { "CalloutType": "sandbox_artifacts", "CalloutUriRegex": "artifactswestus\\.blob\\.core\\.windows\\.net/r/","CanCall": true } ]'
        ```

### Install packages

The example snips below assume local R machine on Windows environment.

1. Verify you're using the appropriate R version – current R Sandbox version is 3.4.4:

    ```
    > R.Version()["version.string"]

    $version.string
    [1] "R version 3.4.4 (2018-03-15)"
    ```

    If needed you can download it from [here](https://cran.r-project.org/bin/windows/base/old/3.4.4/).

1. Launch the x64 RGui

1. Create a new empty folder to be populated with all the relevant packages you would like to install. In this example we install the [brglm2 package](https://cran.r-project.org/web/packages/brglm2/index.html), so creating "C:\brglm2".

1. Add the newly created folder path to lib paths:

    ```
    > .libPaths("C://brglm2")
    ```

1. Verify that the new folder is now the first path in .libPaths():

    ```
    > .libPaths()
    
    [1] "C:/brglm2"    "C:/Program Files/R/R-3.4.4/library"
    
    ```

1. Once this setup is done, any package that we install shall be added to this new folder. Let's install the requested package and its dependencies:

    ```
    > install.packages("brglm2")
    ```

    In case the question "Do you want to install from sources the packages which need compilation?" pops up, answer "Y".

1. Verify that new folders were added to "C:\brglm2":

    :::image type="content" source="images/plugin/sample-directory.png" alt-text="Screenshot of library directory content.":::

1. Select all items in that folder and zip them to e.g. libs.zip (do not zip the parent folder). You should get an archive structure like this:

    libs.zip:

    * brglm2 (folder)
    * enrichwith (folder)
    * numDeriv (folder)

1. Upload libs.zip to the blob container that was set above

1. Call the `r` plugin.
    * Specify the `external_artifacts` parameter with a property bag of name and reference to the zip file (the blob's URL, including a SAS token).
    * In your inline r code, import `zipfile` from `sandboxutils` and call its `install()` method with the name of the zip file.

### Example

Install the [brglm2 package](https://cran.r-project.org/web/packages/brglm2/index.html):

~~~kusto
print x=1
| evaluate r(typeof(*, ver:string),
    'library(sandboxutils)\n'
    'zipfile.install("brglm2.zip")\n'
    'library("brglm2")\n'
    'result <- df\n'
    'result$ver <-packageVersion("brglm2")\n'
    ,external_artifacts=bag_pack(brglm2.zip', 'https://artifactswestus.blob.core.windows.net/r/libs.zip?*** REPLACE WITH YOUR SAS TOKEN ***'))
~~~

| x | ver     |
|---|---------|
|  1| 1.8.2   |

Make sure that the archive's name (first value in pack pair) has the *.zip suffix to prevent collisions when unzipping folders whose name is identical to the archive name.

---

::: zone-end

::: zone pivot="azuremonitor"

This capability isn't supported in Azure Monitor

::: zone-end
