# evaluate plugin operator

Invokes a service-side query extension (plugin).

The `evaluate` operator is a tabular operator that provides the ability to
invoke query language extensions known as **plugins**. Plugins can be enabled
or disabled (unlike other language constructs which are always available),
and are not "bound" by the relational nature of the language (for example, they may
not have a predefined, statically-determined, output schema).

**Syntax** 

[*T* `|`] `evaluate` [ *evaluateParameters* ] *PluginName* `(` [*PluginArg1* [`,` *PluginArg2*]... `)`

Where:

* *T* is an optional tabular input to the plugin. (Some plugins don't take
  any input, and act as a tabular data source.)
* *PluginName* is the mandatory name of the plugin being invoked.
* *PluginArg1*, ... are the optional arguments to the plugin.
* *evaluateParameters*: Zero or more (space-separated) parameters in the form of
  *Name* `=` *Value* that control the behavior of the evaluate operation and execution plan. The following parameters are supported: 

  |Name                |Values                           |Description                                |
  |--------------------|---------------------------------|-------------------------------------------|
  |`hint.distribution` |`single`, `per_node`, `per_shard`| [Distribution hints](#distribution-hints) |

**Notes**

* Syntactically, `evaluate` behaves similarly
to the [invoke operator](./invokeoperator.md), which invokes tabular functions.
* Plugins provided through the evaluate operator aren't bound by the regular rules of query execution or argument evaluation.
Specific plugins may have specific restrictions. For example, plugins whose output schema depends on the data (for example, [bag_unpack plugin](./bag-unpackplugin.md)) can't be used
when performing cross-cluster queries.

## Distribution hints

Distribution hints specify how the plugin execution will be distributed across multiple cluster nodes. Each plugin may implement a different support for the distribution. The plugin's documentation specifies the distribution options supported by the plugin.

Possible values:

* `single`: A single instance of the plugin will run over the entire query data.
* `per_node`: If the query before the plugin call is distributed across nodes, then an instance of the plugin will run on each node over the data that it contains.
* `per_shard`: If the data before the plugin call is distributed across shards, then an instance of the plugin will run over each shard of the data.
