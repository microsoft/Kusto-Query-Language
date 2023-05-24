---
title:  min_of()
description: Learn how to use the min_of() function to return the minimum value of all argument expressions.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/08/2023
---
# min_of()

Returns the minimum value of several evaluated scalar expressions.

## Syntax

`min_of` `(`*arg*`,` *arg_2*`,` [ *arg_3*, ... ]`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *arg*, *arg_2*, ... | scalar | &check; | A comma separated list of 2-64 scalar expressions to compare. The function returns the minimum value among these expressions.|

* All arguments must be of the same type.
* Maximum of 64 arguments is supported.
* Non-null values take precedence to null values.

## Returns

The minimum value of all argument expressions.

## Examples

Find the maximum value in an array:

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUShKLS7NKbHNzcyLz0/TMDTQUTDUUdA1BlLmmgCoI/wyIgAAAA==" target="_blank">Run the query</a>

```kusto
print result=min_of(10, 1, -3, 17) 
```

**Output**

|result|
|---|
|-3|

Find the minimum value in a data-table. Non-null values take precedence over null values:

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA0tJLAHCpJxUBQ1HK4XMvBIdBScwrckVzaUABKY6CkY6YJahgY6CIYQJlNfIK83J0dRRMIZK6iAJoquBM7liuWoUCorys1KTSxRyM/Pi89M0HIE2agIA226WNIcAAAA=" target="_blank">Run the query</a>

```kusto
datatable (A: int, B: int)
[
    5, 2,
    10, 1,
    int(null), 3,
    1, int(null),
    int(null), int(null)
]
| project min_of(A, B)
```

**Output**

|result|
|---|
|2|
|1|
|3|
|1|
|(null)|
