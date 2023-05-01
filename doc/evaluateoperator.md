---
title: evaluate plugin operator - Azure Data Explorer
description: Learn how to use the evaluate plugin operator to invoke plugins.
ms.reviewer: alexans
ms.topic: reference
ms.date: 03/09/2023
---
# evaluate plugin operator

Invokes a service-side query extension (plugin).

The `evaluate` operator is a tabular operator that allows you to invoke query language extensions known as **plugins**. Unlike other language constructs, plugins can be enabled or disabled. Plugins aren't "bound" by the relational nature of the language. In other words, they may not have a predefined, statically determined, output schema.

> [!NOTE]
>
> * Syntactically, `evaluate` behaves similarly to the [invoke operator](./invokeoperator.md), which invokes tabular functions.
> * Plugins provided through the evaluate operator aren't bound by the regular rules of query execution or argument evaluation.
> * Specific plugins may have specific restrictions. For example, plugins whose output schema depends on the data. For example, [bag_unpack plugin](./bag-unpackplugin.md) and [pivot plugin](./pivotplugin.md) can't be used when performing cross-cluster queries.

## Syntax

[*T* `|`] `evaluate` [ *evaluateParameters* ] *PluginName* `(`[ *PluginArgs* ]`)`

## Parameters

|Name|Type|Required|Description|
|--|--|--|--|
| *T* | string | | A tabular input to the plugin. Some plugins don't take any input and act as a tabular data source.|
| *evaluateParameters* | string | | Zero or more space-separated [evaluate parameters](#evaluate-parameters) in the form of *Name* `=` *Value* that control the behavior of the evaluate operation and execution plan. Each plugin may decide differently how to handle each parameter. Refer to each plugin's documentation for specific behavior.|
| *PluginName* | string | &check; | The mandatory name of the plugin being invoked. |
| *PluginArgs* | string | | Zero or more comma-separated arguments to provide to the plugin.|

### Evaluate parameters

The following parameters are supported:

  |Name                |Values                           |Description                                |
  |--------------------|---------------------------------|-------------------------------------------|
  |`hint.distribution` |`single`, `per_node`, `per_shard`| [Distribution hints](#distribution-hints) |
  |`hint.pass_filters` |`true`, `false`| Allow `evaluate` operator to passthrough any matching filters before the plugin. Filter is considered as 'matched' if it refers to a column existing before the `evaluate` operator. Default: `false` |
  |`hint.pass_filters_column` |*column_name*| Allow plugin operator to passthrough filters referring to *column_name* before the plugin. Parameter can be used multiple times with different column names. |

## Plugins

The following plugins are supported:

* [autocluster plugin](autoclusterplugin.md)
* [azure-digital-twins-query-request plugin](azure-digital-twins-query-request-plugin.md)
* [bag-unpack plugin](bag-unpackplugin.md)
* [basket plugin](basketplugin.md)
* [cosmosdb-sql-request plugin](cosmosdb-plugin.md)
* [dcount-intersect plugin](dcount-intersect-plugin.md)
* [diffpatterns plugin](diffpatternsplugin.md)
* [diffpatterns-text plugin](diffpatterns-textplugin.md)
* [infer-storage-schema plugin](inferstorageschemaplugin.md)
* [ipv4-lookup plugin](ipv4-lookup-plugin.md)
* [mysql-request-plugin](mysqlrequest-plugin.md)
* [narrow plugin](narrowplugin.md)
* [pivot plugin](pivotplugin.md)
* [preview plugin](previewplugin.md)
* [R plugin](rplugin.md)
* [rolling-percentile plugin](rolling-percentile-plugin.md)
* [rows-near plugin](rows-near-plugin.md)
* [schema-merge plugin](schemamergeplugin.md)
* [sql-request plugin](sqlrequestplugin.md)
* [sequence-detect plugin](sequence-detect-plugin.md)

## Distribution hints

Distribution hints specify how the plugin execution will be distributed across multiple cluster nodes. Each plugin may implement a different support for the distribution. The plugin's documentation specifies the distribution options supported by the plugin.

Possible values:

* `single`: A single instance of the plugin will run over the entire query data.
* `per_node`: If the query before the plugin call is distributed across nodes, then an instance of the plugin will run on each node over the data that it contains.
* `per_shard`: If the data before the plugin call is distributed across shards, then an instance of the plugin will run over each shard of the data.
