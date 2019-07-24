# Python plugin (Preview)

The Python plugin runs a user-defined-function (UDF) using a Python script. The Python script gets tabular data as its input, and is expected to produce a tabular output.
The plugin's runtime is hosted in <#ifdef PAAS> a sandbox, an isolated and secure Python environment,<#endif> <#ifdef MICROSOFT>[sandboxes](../concepts/sandboxes.md),<#endif> running on the cluster's nodes.

### Syntax

<#ifdef PAAS>*T* `|` `evaluate` [`hint.distribution` `=` (`single` | `per_node`)] `python(`*output_schema*`,` *script* [`,` *script_parameters*]`)`<#endif>
<#ifdef MICROSOFT>*T* `|` `evaluate` [`hint.distribution` `=` (`single` | `per_node`)] `python(`*output_schema*`,` *script* [`,` *script_parameters*][`,` *external_artifacts*]`)`<#endif>

### Arguments

* *output_schema*: A `type` literal that defines the output schema of the tabular data, returned by the Python code.
    * The format is: `typeof(`*ColumnName*`:` *ColumnType* [, ...]`)`, for example: `typeof(col1:string, col2:long)`.
    * For extending the input schema, use the following syntax: `typeof(*, col1:string, col2:long)`
* *script*: A `string` literal that is the valid Python script to be executed.
* *script_parameters*: An optional `dynamic` literal which is a property bag of name/value pairs to be passed to the
   Python script as the reserved `kargs` dictionary (see [Reserved Python variables](#reserved-python-variables)).
* *hint.distribution*: An optional hint for the plugin's execution to be distributed across multiple cluster nodes.
   Default: `single`.
    * `single`: A single instance of the script will run over the entire query data.
    * `per_node`: If the query before the Python block is distributed, then an instance of the script will run on each node over the data that it contains.
<#ifdef MICROSOFT>* *external_artifacts*: An optional (and **experimental**) `dynamic` literal which is a property bag of name/URL pairs of artifacts that are accessible from cloud storage and can be made available for the script to use at runtime.
    * Any URLs referenced in this property bag are required to be included in the cluster's [Callout policy](../concepts/calloutpolicy.md).
    * The artifacts are made available for the script to consume from a local temporary directory, `.\Temp`, and the names provided in the property bag are used as the local file names (see [example](#examples) below).
<#endif>

### Reserved Python variables

The following variables are reserved for interaction between Kusto query language and the Python code:

* `df`: The input tabular data (the values of `T` above), as a `pandas` DataFrame.
* `kargs`: The value of the *script_parameters* argument, as a Python dictionary.
* `result`: A `pandas` DataFrame created by the Python script whose value becomes the tabular data that gets sent to the Kusto query operator that follows the plugin.

### Onboarding

<#ifdef MICROSOFT>* Prerequisites for enabling the plugin are listed [here](../concepts/sandboxes.md#prerequisites).<#endif>
* The plugin is disabled by default.
    * *Interested in enabling the plugin on your cluster?*
        <#ifdef MICROSOFT>* Open a [support ticket](https://aka.ms/kustosupport) in which you should specify
          you've read and acknowledged all the prerequisites, and have approval from the cluster's owner(s).<#endif>
		<#ifdef PAAS>* In the Azure portal, within your Azure Data Explorer cluster, select **New support request** in the left-hand menu.<#endif>
        * Disabling the plugin requires opening a support ticket as well.

### Notes and Limitations

* The Python sandbox image is based on *Anaconda 5.2.0* distribution with *Python 3.6* engine.
  The list of its packages can be found [here](http://docs.anaconda.com/anaconda/packages/old-pkg-lists/5.2.0/py3.6_win-64/)
  (note that a small percentage of packages might be incompatible with the limitations enforced by the sandbox in which the plugin is run).
* The Python image also contain common ML packages: `tensorflow`, `keras`, `torch`, `hdbscan`, `xgboost` and other useful packages.
* The plugin imports *numpy* (as `np`) & *pandas* (as `pd`) by default.  You can import other modules as needed.
* The Python sandbox limits accessing the network, therefore, the Python code can't dynamically install additional Python packages that are
  not included in the image. <#ifdef PAAS>Open a **New support request** in the Azure portal<#endif> <#ifdef MICROSOFT>Contact [Kusto Machine Learning DL](mailto:kustoml@microsoft.com)<#endif> if you need specific packages.
<#ifdef MICROSOFT> 
* **[Ingestion from query](../management/data-ingestion/ingest-from-query.md) and [Update policies](../concepts/updatepolicy.md)**
    * It is possible to use the plugin in queries which are:
        1. Defined as part of an update policy, whose source table is ingested to using *non-streaming* ingestion.
        2. Run as part of a command which ingests from a query (e.g. `.set-or-append`).
    * In both the above cases, it's recommended to verify that the volume and frequency of the ingestion, as well as the complexity and
      resources utilization of the Python logic are aligned with [sandbox limitations](../concepts/sandboxes.md#limitations), and the cluster's available resources.
      Failure to do so may result with [throttling errors](../concepts/sandboxes.md#errors).
    * It is *not* possible to use the plugin in a query which is defined as part of an update policy, whose source table is ingested to 
    using [*streaming* ingestion](../management/data-ingestion/streaming.md).<#endif>
	

### Examples

<!-- csl -->
```
range x from 1 to 360 step 1
| evaluate python(
//
typeof(*, fx:double),               //  Output schema: append a new fx column to original table 
//
'result = df\n'                     //  The Python decorated script
'n = df.shape[0]\n'
'g = kargs["gain"]\n'
'f = kargs["cycles"]\n'
'result["fx"] = g * np.sin(df["x"]/n*2*np.pi*f)\n'
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
| evaluate python(
    typeof(File:string, Size:string),
    "import os\n"
    "result = pd.DataFrame(columns=['File','Size'])\n"
    "sizes = []\n"
    "path = '.\\\\Temp'\n"
    "files = os.listdir(path)\n"
    "result['File']=files\n"
    "for file in files:\n"
    "    sizes.append(os.path.getsize(path + '\\\\' + file))\n"
    "result['Size'] = sizes\n"
    "\n",
    external_artifacts = 
        dynamic({"this_is_my_first_file":"https://raw.githubusercontent.com/yonileibowitz/kusto.blog/master/resources/R/sample_script.r",
                 "this_is_a_script":"https://raw.githubusercontent.com/yonileibowitz/kusto.blog/master/resources/python/sample_script.py"})
)
```

| File                  | Size |
|-----------------------|------|
| this_is_a_script      | 120  |
| this_is_my_first_file | 105  |
<#endif>

### Performance tips

* Reduce the plugin's input data set to the minimum amount required (columns/rows).
    * Use filters on the source data set, when possible, with Kusto's query language.
    * To perform a calculation on a subset of the source columns, project only those column before invoking the plugin.
* Use `hint.distribution = per_node` whenever the logic in your script is distributable.
    * You can also use the [partition operator](partitionoperator.md) for partitioning the input data set.
* Use Kusto's query language, whenever possible, to implement the logic of your Python script.

    Example:

    <!-- csl -->
    ```    
    .show operations
    | where StartedOn > ago(7d) // Filtering out irrelevant records before invoking the plugin
    | project d_seconds = Duration / 1s // Projecting only a subset of the necessary columns
    | evaluate hint.distribution = per_node python( // Using per_node distribution, as the script's logic allows it
        typeof(*, _2d:double),
        'result = df\n'
        'result["_2d"] = 2 * df["d_seconds"]\n' // Negative example: this logic should have been written using Kusto's query language
      )
    | summarize avg = avg(_2d)
    ```

### Usage tips

* To generate multi-line strings containing the Python script in `Kusto.Explorer`, copy your Python script from your favorite
  Python editor (e.g. *Jupyter*, *Visual Studio Code*, *PyCharm* etc.), then either:
    * Press *F2* to open the **Edit in Python** window. Paste the script into this window. Select **OK**. The script will be
      decorated with quotes and new lines (so it's valid in Kusto) and automatically pasted into the query tab.
    * Paste the Python code directly into the query tab, select those lines and press *Ctrl+K*, *Ctrl+S* hot key to decorate them as
      above (to reverse it press *Ctrl+K*, *Ctrl+M* hot key). [Here](../tools/kusto-explorer-shortcuts.md#query-editor) is the full list of
      Query Editor shortcuts.
* To avoid conflicts between Kusto string delimiters and Python string literals, we recommend using single quote characters (`'`) for Kusto string 
  literals in Kusto queries, and double quote characters (`"`) for Python string literals in Python scripts.
* Use [externaldata operator](externaldata-operator.md) to obtain the content of a script that you've stored in an external location, such as Azure Blob storage or a public GitHub repository.
  
	**Example**

    <!-- csl -->
    ```    
    let script = 
        externaldata(script:string)
        [h'https://raw.githubusercontent.com/yonileibowitz/kusto.blog/master/resources/python/sample_script.py']
        with(format = raw);
    range x from 1 to 360 step 1
    | evaluate python(
        typeof(*, fx:double),
        toscalar(script), 
        pack('gain', 100, 'cycles', 4))
    | render linechart 
    ```

---

<#ifdef MICROSOFT>For questions or feedback about this plugin join the discussion at [Kusto Machine Learning DL](mailto:kustoML@microsoft.com).<#endif>