# Set statement

::: zone pivot="azuredataexplorer"

The `set` statement is used to set a query option for the duration of the query.
Query options control how a query executes and returns results. They can be
Boolean flags (off by default), or have an integer value. A query may contain
zero, one, or more set statements. Set statements affect only the tabular expression
statements that trail them in the program order.

* Query options can also be enabled programmatically by setting them in the
  `ClientRequestProperties` object. See [here](../api/netfx/request-properties.md).
  
* Query options are not formally a part of the Kusto language, and may be
  modified without being considered as a breaking language change.

**Syntax**

`set` *OptionName* [`=` *OptionValue*]

**Example**

<!-- csl -->
```
set querytrace;
Events | take 100
```

::: zone-end

::: zone pivot="azuremonitor"

This isn't supported in Azure Monitor

::: zone-end
