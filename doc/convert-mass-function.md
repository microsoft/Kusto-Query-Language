---
title: convert_mass() - Azure Data Explorer
description: Learn how to use the convert_mass() function to convert a mass input value from one unit to another.
ms.reviewer: itsagui
ms.topic: reference
ms.date: 11/27/2022
---
# convert_mass

Convert a mass value from one unit to another.

## Syntax

`convert_mass(`*value*`,`*from*`,`*to*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *value* | real | &check; | The value to be converted. |
| *from* | string | &check; | The unit to convert from. For possible values, see [Conversion units](#conversion-units). |
| *to* | string | &check; | The unit to convert to. For possible values, see [Conversion units](#conversion-units). |

### Conversion units

* Centigram
* Decagram
* Decigram
* EarthMass
* Grain
* Gram
* Hectogram
* Kilogram
* Kilopound
* Kilotonne
* LongHundredweight
* LongTon
* Megapound
* Megatonne
* Microgram
* Milligram
* Nanogram
* Ounce
* Pound
* ShortHundredweight
* ShortTon
* Slug
* SolarMass
* Stone
* Tonne

## Returns

 Returns the input value converted from one mass unit to another.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUShKLS7NKVGwVUjOzytLLSqJz00sLtYw1DPSUVD3zszJTy9KzFUHsgPyS/NS1DUBemVMijUAAAA=" target="_blank">Run the query</a>

```kusto
print result = convert_mass(1.2, 'Kilogram', 'Pound')
```

**Output**

|result|
|---|
|2.64554714621853|
