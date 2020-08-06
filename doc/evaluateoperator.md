---
title: evaluate plugin operator - Azure Data Explorer | Microsoft Docs
description: This article describes evaluate plugin operator in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 10/30/2019
---
# evaluate operator plugins

Invokes a service-side query extension (plugin).

The `evaluate` operator is a tabular operator that provides the ability to
invoke query language extensions known as **plugins**. Plugins can be enabled
or disabled (unlike other language constructs, which are always available),
and aren't "bound" by the relational nature of the language (for example, they may
not have a predefined, statically determined, output schema).

> [!NOTE]
> * Syntactically, `evaluate` behaves similarly to the [invoke operator](./invokeoperator.md), which invokes tabular functions.
> * Plugins provided through the evaluate operator aren't bound by the regular rules of query execution or argument evaluation.
> * Specific plugins may have specific restrictions. For example, plugins whose output schema depends on the data (for example, [bag_unpack plugin](./bag-unpackplugin.md) and [pivot plugin](./pivotplugin.md)) can't be used when performing cross-cluster queries.

## Syntax 

[*T* `|`] `evaluate` [ *evaluateParameters* ] *PluginName* `(` [*PluginArg1* [`,` *PluginArg2*]... `)`

## Arguments

* *T* is an optional tabular input to the plugin. (Some plugins don't take
  any input, and act as a tabular data source.)
* *PluginName* is the mandatory name of the plugin being invoked.
* *PluginArg1*, ... are the optional arguments to the plugin.
* *evaluateParameters*: Zero or more (space-separated) parameters in the form of
  *Name* `=` *Value* that control the behavior of the evaluate operation and execution plan. Each plugin may decide differently how to handle each parameter. Refer to each plugin's documentation for specific behavior.  

## Parameters

The following parameters are supported: 

  |Name                |Values                           |Description                                |
  |--------------------|---------------------------------|-------------------------------------------|
  |`hint.distribution` |`single`, `per_node`, `per_shard`| [Distribution hints](#distribution-hints) |
  |`hint.pass_filters` |`true`, `false`| Allow `evaluate` operator to passthrough any matching filters before the plugin. Filter is considered as 'matched' if it refers to a column existing before the `evaluate` operator. Default: `false` |
  |`hint.pass_filters_column` |*column_name*| Allow plugin operator to passthrough filters referring to *column_name* before the plugin. Parameter can be used multiple times with different column names. |

## Distribution hints

Distribution hints specify how the plugin execution will be distributed across multiple cluster nodes. Each plugin may implement a different support for the distribution. The plugin's documentation specifies the distribution options supported by the plugin.

Possible values:

* `single`: A single instance of the plugin will run over the entire query data.
* `per_node`: If the query before the plugin call is distributed across nodes, then an instance of the plugin will run on each node over the data that it contains.
* `per_shard`: If the data before the plugin call is distributed across shards, then an instance of the plugin will run over each shard of the data.
