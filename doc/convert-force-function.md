---
title: convert_force() - Azure Data Explorer
description: Learn how to use the convert_force() function to convert a force input value from one unit to another.
ms.reviewer: itsagui
ms.topic: reference
ms.date: 11/27/2022
---
# convert_force

Convert a force value from one unit to another.

## Syntax

`convert_force(`*value*`,`*from*`,`*to*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *value* | real | &check; | The value to be converted. |
| *from* | string | &check; | The unit to convert from. For possible values, see [Conversion units](#conversion-units). |
| *to* | string | &check; | The unit to convert to. For possible values, see [Conversion units](#conversion-units). |

### Conversion units

* Decanewton
* Dyn
* KilogramForce
* Kilonewton
* KiloPond
* KilopoundForce
* Meganewton
* Micronewton
* Millinewton
* Newton
* OunceForce
* Poundal
* PoundForce
* ShortTonForce
* TonneForce

## Returns

 Returns the input value converted from one force unit to another. Invalid units return `null`.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUShKLS7NKVGwVUjOzytLLSqJT8svSk7VMNQz0lFQ90stL8nPUweyXFKTE/MgPE0AhSGK6TkAAAA=" target="_blank">Run the query</a>

```kusto
print result = convert_force(1.2, 'Newton', 'Decanewton')
```

**Output**

|result|
|---|
|0.12|
