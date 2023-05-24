---
title:  sign()
description: Learn how to use the sign() function to return the sign of the numeric expression.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/30/2023
---
# sign()

Returns the sign of the numeric expression.

## Syntax

`sign(`*number*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *number* | real | &check; | The number for which to return the sign.|

## Returns

The positive (+1), zero (0), or negative (-1) sign of the specified expression.

## Examples

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUSg2VLBVKM5Mz9PQNTHS1FEoNoLxDUA8YxjP0FDPSBMA4mpHrTMAAAA=" target="_blank">Run the query</a>

```kusto
print s1 = sign(-42), s2 = sign(0), s3 = sign(11.2)
```

**Output**

|s1|s2|s3|
|---|---|---|
|-1|0|1|
