---
title: Python plugin - Azure Data Explorer
description: Learn how to use the Python plugin to run user-defined functions using a Python script.
ms.reviewer: adieldar
ms.topic: reference
ms.date: 04/19/2023
zone_pivot_group_filename: data-explorer/zone-pivot-groups.json
zone_pivot_groups: kql-flavors
---
# Python plugin

::: zone pivot="azuredataexplorer"

The Python plugin runs a user-defined function (UDF) using a Python script. The Python script gets tabular data as its input, and produces tabular output.
The plugin's runtime is hosted in [sandboxes](../concepts/sandboxes.md), running on the cluster's nodes.

## Syntax

*T* `|` `evaluate` [`hint.distribution` `=` (`single` | `per_node`)] `python(`*output_schema*`,` *script* [`,` *script_parameters*][`,` *external_artifacts*][`,` *spill_to_disk*]`)`

## Parameters

|Name|Type|Required|Description|
|--|--|--|--|
|*output_schema*|string|&check;|A `type` literal that defines the output schema of the tabular data, returned by the Python code. The format is: `typeof(`*ColumnName*`:` *ColumnType*[, ...]`)`. For example, `typeof(col1:string, col2:long)`. To extend the input schema, use the following syntax: `typeof(*, col1:string, col2:long)`.|
|*script*|string|&check;|The valid Python script to execute. To generate multi-line strings, see [Usage tips](#usage-tips).|
|*script_parameters*|dynamic||A property bag of name value pairs to be passed to the Python script as the reserved `kargs` dictionary. For more information, see [Reserved Python variables](#reserved-python-variables).|
|`hint.distribution`|string||A hint for the plugin's execution to be distributed across multiple cluster nodes. The default value is `single`. `single` means a single instance of the script will run over the entire query data. `per_node` means that if the query before the Python block is distributed, an instance of the script will run on each node, on the data that it contains.|
|*external_artifacts*|dynamic||A property bag of name and URL pairs for artifacts that are accessible from cloud storage. See more in [Using external artifacts](#using-external-artifacts).|
|*spill_to_disk*|bool||Specifies an alternative method for serializing the input table to the Python sandbox. For serializing big tables set it to `true` to speed up the serialization and significantly reduce the sandbox memory consumption. Default is `false` as this parameter is experimental.|

## Reserved Python variables

The following variables are reserved for interaction between Kusto Query Language and the Python code.

* `df`: The input tabular data (the values of `T` above), as a `pandas` DataFrame.
* `kargs`: The value of the *script_parameters* argument, as a Python dictionary.
* `result`: A `pandas` DataFrame created by the Python script, whose value becomes the tabular data that gets sent to the Kusto query operator that follows the plugin.

## Enable the plugin

The plugin is disabled by default. Before you start, review the list of [prerequisites](../concepts/sandboxes.md#prerequisites-and-limitations).

To enable the plugin and select the version of the Python image, see [Enable language extensions on your cluster](../../language-extensions.md#enable-language-extensions-on-your-cluster).

## Python sandbox image

To change the version of the Python image, see [Change the Python language extensions image on your cluster](../../language-extensions.md#change-the-python-language-extensions-image-on-your-cluster).

To see the list of packages for the different Python images, see [Python package reference](../../python-package-reference.md).

> [!NOTE]
>
> * By default, the plugin imports *numpy* as **np** and *pandas* as **pd**. Optionally, you can import other modules as needed.
> * Some packages might be incompatible with the limitations enforced by the sandbox where the plugin is run.

## Use Ingestion from query and update policy

* Use the plugin in queries that are:
  * Defined as part of an [update policy](../management/updatepolicy.md), whose source table is ingested to using *non-streaming* ingestion.
  * Run as part of a command that [ingests from a query](../management/data-ingestion/ingest-from-query.md), such as `.set-or-append`.
    In both these cases, verify that the volume and frequency of the ingestion, and the complexity and
    resources used by the Python logic, align with [sandbox parameters](../concepts/sandboxes.md#sandbox-parameters) and the cluster's available resources. Failure to do so may result in [throttling errors](../concepts/sandboxes.md#errors).
* You can't use the plugin in a query that is defined as part of an update policy, whose source table is ingested using [streaming ingestion](../../ingest-data-streaming.md).

## Examples

~~~kusto
range x from 1 to 360 step 1
| evaluate python(
//
typeof(*, fx:double),               //  Output schema: append a new fx column to original table 
```
result = df
n = df.shape[0]
g = kargs["gain"]
f = kargs["cycles"]
result["fx"] = g * np.sin(df["x"]/n*2*np.pi*f)
```
, bag_pack('gain', 100, 'cycles', 4)    //  dictionary of parameters
)
| render linechart 
~~~

:::image type="content" source="images/plugin/sine-demo.png" alt-text="sine demo." border="false":::

~~~kusto
print "This is an example for using 'external_artifacts'"
| evaluate python(
    typeof(File:string, Size:string), ```if 1:
    import os
    result = pd.DataFrame(columns=['File','Size'])
    sizes = []
    path = '.\\\\Temp'
    files = os.listdir(path)
    result['File']=files
    for file in files:
        sizes.append(os.path.getsize(path + '\\\\' + file))
    result['Size'] = sizes
    ```,
    external_artifacts = 
        dynamic({"this_is_my_first_file":"https://kustoscriptsamples.blob.core.windows.net/samples/R/sample_script.r",
                 "this_is_a_script":"https://kustoscriptsamples.blob.core.windows.net/samples/python/sample_script.py"})
)
~~~

| File                  | Size |
|-----------------------|------|
| this_is_a_script      | 120  |
| this_is_my_first_file | 105  |

## Performance tips

* Reduce the plugin's input data set to the minimum amount required (columns/rows).
  * Use filters on the source data set, when possible, with Kusto's query language.
  * To do a calculation on a subset of the source columns, project only those columns before invoking the plugin.
* Use `hint.distribution = per_node` whenever the logic in your script is distributable.
  * You can also use the [partition operator](partitionoperator.md) for partitioning the input data set.
* Use Kusto's query language whenever possible, to implement the logic of your Python script.

## Usage tips

* To generate multi-line strings containing the Python script in `Kusto.Explorer`, copy your Python script from your favorite
  Python editor (*Jupyter*, *Visual Studio Code*, *PyCharm*, and so on).
  Now do one of:
  * Enclose the full script between lines containing three consecutive backticks, for example:  
      ` ``` `  
      ` python code`  
      ` ``` `
  * Press **F2** to open the *Edit in Python* window. Paste the script into this window. Select **OK**. The script will be
      decorated with quotes and new lines, so it's valid in Kusto, and automatically pasted into the query tab.
  * Paste the Python code directly into the query tab. Select those lines, and press **Ctrl+K**, **Ctrl+S** hot keys, to decorate them as
      above. To reverse, press **Ctrl+K**, **Ctrl+M** hot keys. See the full list of [Query Editor shortcuts](../tools/kusto-explorer-shortcuts.md#query-editor).
* To avoid conflicts between Kusto string delimiters and Python string literals, use:
  * Single quote characters (`'`) for Kusto string literals in Kusto queries
  * Double quote characters (`"`) for Python string literals in Python scripts
* Use the [`externaldata` operator](externaldata-operator.md) to obtain the content of a script that you've stored in an external location, such as Azure Blob storage.
  
### Example

```kusto
    let script = 
        externaldata(script:string)
        [h'https://kustoscriptsamples.blob.core.windows.net/samples/python/sample_script.py']
        with(format = raw);
    range x from 1 to 360 step 1
    | evaluate python(
        typeof(*, fx:double),
        toscalar(script), 
        bag_pack('gain', 100, 'cycles', 4))
    | render linechart 
 ```

## Using External Artifacts

External artifacts from cloud storage can be made available for the script and used at runtime.

The URLs referenced by the external artifacts property must be:

* Included in the cluster's [callout policy](../management/calloutpolicy.md).
* In a publicly available location, or provide the necessary credentials, as explained in [storage connection strings](../api/connection-strings/storage-connection-strings.md).

> [!NOTE]
> When authenticating external artifacts using Managed Identities, the `SandboxArtifacts` usage must be defined on the cluster level [managed identity policy](../management/managed-identity-policy.md).

The artifacts are made available for the script to consume from a local temporary directory, `.\Temp`. The names provided in the property bag are used as the local file names. See [Examples](#examples).

For information regarding referencing external packages, see [Install packages for the Python plugin](#install-packages-for-the-python-plugin).

## Install packages for the Python plugin

You may need to install package(s) yourself, for the following reasons:

* The package is private and is your own.
* The package is public but isn't included in the plugin's base image.

Install packages as follows:

### Prerequisites

  1. Create a blob container to host the packages, preferably in the same place as your cluster. For example, `https://artifactswestus.blob.core.windows.net/python`, assuming your cluster is in West US.
  1. Alter the cluster's [callout policy](../management/calloutpolicy.md) to allow access to that location.
        * This change requires [AllDatabasesAdmin](../management/access-control/role-based-access-control.md) permissions.

        * For example, to enable access to a blob located in `https://artifactswestus.blob.core.windows.net/python`, run the following command:

        ```kusto
        .alter-merge cluster policy callout @'[ { "CalloutType": "sandbox_artifacts", "CalloutUriRegex": "artifactswestus\\.blob\\.core\\.windows\\.net/python/","CanCall": true } ]'
        ```

### Install packages

1. For public packages in [PyPi](https://pypi.org/) or other channels,
download the package and its dependencies.

   * From a cmd window in your local Windows Python environment, run:

    ```python
    pip wheel [-w download-dir] package-name.
    ```

1. Create a zip file that contains the required package and its dependencies.

    * For private packages, zip the folder of the package and the folders of its dependencies.
    * For public packages, zip the files that were downloaded in the previous step.

> [!NOTE]
>
> * Make sure to download the package that is compatible to the Python engine and the platform of the sandbox runtime (currently 3.6.5 on Windows)
    > * Make sure to zip the `.whl` files themselves, and not their parent folder.
    > * You can skip `.whl` files for packages that already exist with the same version in the base sandbox image.

1. Upload the zipped file to a blob in the artifacts location (from step 1).

1. Call the `python` plugin.
    * Specify the `external_artifacts` parameter with a property bag of name and reference to the zip file (the blob's URL, including a SAS token).
    * In your inline python code, import `Zipackage` from `sandbox_utils` and call its `install()` method with the name of the zip file.

### Example

Install the [Faker](https://pypi.org/project/Faker/) package that generates fake data.

~~~kusto
range ID from 1 to 3 step 1 
| extend Name=''
| evaluate python(typeof(*), ```if 1:
    from sandbox_utils import Zipackage
    Zipackage.install("Faker.zip")
    from faker import Faker
    fake = Faker()
    result = df
    for i in range(df.shape[0]):
        result.loc[i, "Name"] = fake.name()
    ```,
    external_artifacts=bag_pack('faker.zip', 'https://artifacts.blob.core.windows.net/kusto/Faker.zip?*** REPLACE WITH YOUR SAS TOKEN ***'))
~~~

| ID | Name         |
|----|--------------|
|   1| Gary Tapia   |
|   2| Emma Evans   |
|   3| Ashley Bowen |

---

## See also

For more examples of UDF functions that use the Python plugin, see the [Functions library](../functions-library/functions-library.md).

::: zone-end

::: zone pivot="azuremonitor"

This capability isn't supported in Azure Monitor

::: zone-end
