---
title: evaluate plugin operator - Azure Data Explorer | Microsoft Docs
description: This article describes evaluate plugin operator in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 11/11/2018
---
# evaluate plugin operator

Invokes a service-side query extension (plugin).

The `evaluate` operator is a tabular operator that provides the ability to
invoke query language extensions known as **plugins**. Plugins can be enabled
or disabled (unlike other language constructs which are always available),
and are not "bound" by the relational nature of the language (for example, they may
not have a predefined, statically-determined, output schema).

**Syntax**

[*T* `|`] `evaluate` *PluginName* `(` [*PluginArg1* [`,` *PluginArg2*]... `)`

Where:

* *T* is an optional tabular input to the plugin. (Some plugins don't take
  any input, and act as a tabular data source.)
* *PluginName* is the mandatory name of the plugin being invoked.
* *PluginArg1*, ... are the optional arguments to the plugin.

**Remarks**

Syntactically, `evaluate` behaves similarly
to the [invoke operator](./invokeoperator.md), which invokes tabular functions.

Plugins provided through the evaluate operator are not bound by the regular
rules of query execution or argument evaluation.

Plugins cannot be called cross-cluster; they are always evaluated "locally"
(on the same cluster to which the query was originally sent).

Specific plugins may have specific restrictions. For example, plugins whose output schema depends
on the data (e.g., the [bag_unpack plugin](./bag-unpackplugin.md)) cannot be used
when performing cross-cluster queries.