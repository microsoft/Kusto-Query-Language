# ingestion_time()

Retrieves the record's `$IngestionTime` hidden `datetime` column, or null.
The `$IngestionTime` column is automatically defined when the table's

::: zone pivot="azuredataexplorer"

[IngestionTime policy](../management/ingestiontimepolicy.md) is set (enabled).

::: zone-end

::: zone pivot="azuremonitor"

IngestionTime policy is set (enabled).

::: zone-end

If the table doesn't have this policy defined, a null value is returned.

This function must be used in the context of an actual table
to return the relevant data. For example, it will return null for all records
if it's invoked following a `summarize` operator.

**Syntax**

 `ingestion_time()`

**Returns**

A `datetime` value specifying the approximate time of ingestion into a table.

**Example**

<!-- csl -->
```
T 
| extend ingestionTime = ingestion_time() | top 10 by ingestionTime
```
