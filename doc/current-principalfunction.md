# current_principal()

::: zone pivot="azuredataexplorer"

Returns the current principal name running the query.

**Syntax**

`current_principal()`

**Returns**

The current principal fully-qualified-name (FQN) as a `string`.  
The string is formed as:  
*PrinciplaType*`=`*PrincipalId*`;`*TenantId*

**Example**

<!-- csl: https://help.kusto.windows.net/Samples -->
```
print fqn=current_principal()
```

|fqn|
|---|
|aaduser=346e950e-4a62-42bf-96f5-4cf4eac3f11e;72f988bf-86f1-41af-91ab-2d7cd011db47|

::: zone-end

::: zone pivot="azuremonitor"

This isn't supported in Azure Monitor

::: zone-end
