---
title: estimate_data_size() - Azure Data Explorer
description: Learn how to use the estimate_data_size() function to return an estimated data size in bytes of the selected columns of the tabular expression.
ms.reviewer: alexans
ms.topic: reference
ms.date: 03/09/2023
---
# estimate_data_size()

Returns an estimated data size in bytes of the selected columns of the tabular expression.

## Syntax

`estimate_data_size(`*columns*`)`

## Parameters

|Name|Type|Required|Description|
|--|--|--|--|
|*columns*|string|&check;|One or more comma-separated column references in the source tabular expression to use for data size estimation. To include all columns, use the wildcard (`*`) character.|

## Returns

The estimated data size in bytes of the record size. Estimation is based on data types and values lengths.

## Example

The following example calculates the total data size using `estimate_data_size()`.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA22NvQ7CMBCDd57CWxMYmvAbhr5F9ypSj1KpSarmkCLEw3OFtbdYlu3vFh8HQsFjSQEWnGANMtMsZuPqWrpqSnHQGDMcdh9QYYo9WlE0qOzxdL5cb+5uqm3ArzhRHPi5MuTfSsmvEPwyvgltYj814hVlHoNn6nrPvssSqr3Wf4pyB2t0kXUD68wXVvoNCMgAAAA=" target="_blank">Run the query</a>

```kusto
range x from 1 to 10 step 1                    // x (long) is 8 
| extend Text = '1234567890'                   // Text length is 10  
| summarize Total=sum(estimate_data_size(*))   // (8+10)x10 = 180
```

**Output**

|Total|
|---|
|180|
