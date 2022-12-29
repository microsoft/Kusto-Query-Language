---
title: current_principal() - Azure Data Explorer
description: Learn how to use the current_principal() function to return the name of the principal running the query.
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/23/2022
zone_pivot_group_filename: data-explorer/zone-pivot-groups.json
zone_pivot_groups: kql-flavors
---
# current_principal()

::: zone pivot="azuredataexplorer"

Returns the current principal name that runs the query.

## Syntax

`current_principal()`

## Returns

The current principal fully qualified name (FQN) as a `string`.  
The string format is:  
*PrinciplaType*`=`*PrincipalId*`;`*TenantId*

## Example

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
print fqn=current_principal()
```

**Output**

|fqn|
|---|
|aaduser=346e950e-4a62-42bf-96f5-4cf4eac3f11e;72f988bf-86f1-41af-91ab-2d7cd011db47|

::: zone-end

::: zone pivot="azuremonitor"

This capability isn't supported in Azure Monitor

::: zone-end
