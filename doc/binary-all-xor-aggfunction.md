# binary_all_xor() (aggregation function)

Accumulates values using the binary `XOR` operation per summarization group (or in total, if summarization is done without grouping).

* Can be used only in context of aggregation inside [summarize](summarizeoperator.md)

**Syntax**

summarize `binary_all_xor(`*Expr*`)`

**Arguments**

* *Expr*: long number.

**Returns**

Returns a value that is aggregated using the binary `XOR` operation over records per summarization group (or in total, if summarization is done without grouping).

**Example**

Producing 'cafe-food' using binary `XOR` operations:

<!-- csl: https://help.kusto.windows.net/Samples -->
```
datatable(num:long)
[
  0x44404440,
  0x1E1E1E1E,
  0x90ABBA09,
  0x000B105A,
]
| summarize result = toupper(tohex(binary_all_xor(num)))
```

|result|
|---|
|CAFEF00D|
