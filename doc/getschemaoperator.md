# getschema operator 

Produce a table that represents a tabular schema of the input.

<!-- csl -->
```
T | summarize MyCount=count() by Country | getschema 
```

**Syntax**

*T* `| ` `getschema`

**Example**

<!-- csl: https://help.kusto.windows.net:443/Samples -->
```
StormEvents
| top 10 by Timestamp
| getschema
```

|ColumnName|ColumnOrdinal|DataType|ColumnType|
|---|---|---|---|
|Timestamp|0|System.DateTime|datetime|
|Language|1|System.String|string|
|Page|2|System.String|string|
|Views|3|System.Int64|long
|BytesDelivered|4|System.Int64|long
