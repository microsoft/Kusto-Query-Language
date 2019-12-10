# extent_id()

::: zone pivot="azuredataexplorer"

Returns a unique identifier that identifies the data shard ("extent") that the current record resides in. 

Applying this function to calculated data which is not attached to a data shard returns an empty guid (all zeros).

**Syntax**

`extent_id()`

**Returns**

A value of type `guid` that identifies the current record's data shard,
or an empty guid (all zeros).

**Example**

The following example shows how to get a list of all the data shards
that have records from an hour ago with a specific value for the
column `ActivityId`. It demonstrates that some query operators (here,
the `where` operator, but this is also true for `extend` and `project`)
preserve the information about the data shard hosting the record.

<!-- csl -->
```
T
| where Timestamp > ago(1h)
| where ActivityId == 'dd0595d4-183e-494e-b88e-54c52fe90e5a'
| extend eid=extent_id()
| summarize by eid
```

::: zone-end

::: zone pivot="azuremonitor"

This isn't supported in Azure Monitor

::: zone-end
