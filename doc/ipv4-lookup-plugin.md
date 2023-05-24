---
title:  ipv4_lookup plugin
description: Learn how to use the ipv4_lookup plugin to look up an IPv4 value in a lookup table.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/01/2023
---
# ipv4_lookup plugin

The `ipv4_lookup` plugin looks up an IPv4 value in a lookup table and returns rows with matched values. The plugin is invoked with the [`evaluate`](evaluateoperator.md) operator.

## Syntax

*T* `|` `evaluate` `ipv4_lookup(` *LookupTable* `,` *SourceIPv4Key* `,` *IPv4LookupKey* [`,` *ExtraKey1* [.. `,` *ExtraKeyN* [`,` *return_unmatched* ]]] `)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *T* | string | &check; | The tabular input whose column *SourceIPv4Key* will be used for IPv4 matching.|
| *LookupTable* | string | &check; | Table or tabular expression with IPv4 lookup data, whose column *LookupKey* will be used for IPv4 matching. IPv4 values can be masked using [IP-prefix notation](#ip-prefix-notation).|
| *SourceIPv4Key* | string | &check; | The column of *T* with IPv4 string to be looked up in *LookupTable*. IPv4 values can be masked using [IP-prefix notation](#ip-prefix-notation).|
| *IPv4LookupKey* | string | &check; | The column of *LookupTable* with IPv4 string that is matched against each *SourceIPv4Key* value.|
| *ExtraKey1* .. *ExtraKeyN* | string | | Additional column references that are used for lookup matches. Similar to `join` operation: records with equal values will be considered matching. Column name references must exist both is source table `T` and `LookupTable`.|
| *return_unmatched* | bool | | A boolean flag that defines if the result should include all or only matching rows (default: `false` - only matching rows returned).|

[!INCLUDE [ip-prefix-notation](../../includes/ip-prefix-notation.md)]

## Returns

The `ipv4_lookup` plugin returns a result of join (lookup) based on IPv4 key. The schema of the table is the union of the source table and the lookup table, similar to the result of the [`lookup` operator](lookupoperator.md).

If the *return_unmatched* argument is set to `true`, the resulting table will include both matched and unmatched rows (filled with nulls).

If the *return_unmatched* argument is set to `false`, or omitted (the default value of `false` is used), the resulting table will have as many records as matching results. This variant of lookup has better performance compared to `return_unmatched=true` execution.

> [!NOTE]
>
> * This plugin covers the scenario of IPv4-based join, assuming a small lookup table size (100K-200K rows), with the input table optionally having a larger size.
> * The performance of the plugin will depend on the sizes of the lookup and data source tables, the number of columns, and number of matching records.

## Examples

### IPv4 lookup - matching rows only

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA12STU/DMAyG7/0V1i6AVJIlW8cY4sCXEOzABOKE0BTaMKK1SZWkm5D48dhtgY1D3ybOY79WYs7hbgGlc+umhqjeSj3DwPJaRZVwDgvlo1ElFLiHd++qGXzEWIcZ515t2crEj+atCdrnzkZtI8tdxQkOOga+0s7U8tjUmzGvVIjat2e7cZaHTVLq+GMK561X28mh1XHr/HoWojd2lQKZGIs2y9wVug9D+he2qtI7dGOj/1ya4Hb5v4Md+ih5SQAGQgg2mTIhp2zIxckgHVw8kQSj8He/IFG1soOU6IwhRuApxm+eSRrvao2LR9o9NqFNJFbKEZNZht8YM+T4X+mnWxJsRNXO6y5lPGGjCZPDIcsEH0kEHq5Icq1sm3Q1J8Gng7tQKluE3gpzmJiOfnz2O7u9RHm2JuoC5mhYuArTXs/6Rwh7D2Dqvfs5+C0t5EEKADgh+6UIomsRrAOgg7qboENxKpmYZFiAiRNE2gpr67aWekD/5Av0RpWNihpoQJbdbB7285FiMIV+MI6+AVer8Qy+AgAA" target="_blank">Run the query</a>

```kusto
// IP lookup table: IP_Data
// Partial data from: https://raw.githubusercontent.com/datasets/geoip2-ipv4/master/data/geoip2-ipv4.csv
let IP_Data = datatable(network:string, continent_code:string ,continent_name:string, country_iso_code:string, country_name:string)
[
  "111.68.128.0/17","AS","Asia","JP","Japan",
  "5.8.0.0/19","EU","Europe","RU","Russia",
  "223.255.254.0/24","AS","Asia","SG","Singapore",
  "46.36.200.51/32","OC","Oceania","CK","Cook Islands",
  "2.20.183.0/24","EU","Europe","GB","United Kingdom",
];
let IPs = datatable(ip:string)
[
  '2.20.183.12',   // United Kingdom
  '5.8.1.2',       // Russia
  '192.165.12.17', // Unknown
];
IPs
| evaluate ipv4_lookup(IP_Data, ip, network)
```

**Output**

|ip|network|continent_code|continent_name|country_iso_code|country_name|
|---|---|---|---|---|---|
|2.20.183.12|2.20.183.0/24|EU|Europe|GB|United Kingdom|
|5.8.1.2|5.8.0.0/19|EU|Europe|RU|Russia|

### IPv4 lookup - return both matching and non-matching rows

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA11SwW7bMAy9+yuIXNoCnhQpcZqm6GHrhqLrYUGLnobCUG0tEWJLhkQlGLCPH2U7RVIDpq3H9/gIipzD4xoa53axA1TvjV4RUH5XqDLOYa08GtVATWf44127ggRvEbuw4tyrA9sY3Mb3GLSvnEVtkVWu5UkQNAa+0c508ovp9nPeqoDa97lTnFVhnzUaj8Zw1/v13VxajQfnd6uA3thNnjyMJZeycrUeUTiBrWpP4WjR/y1NcJ/5Q+KUfZX9zoCeiRCCLZZMyCWbcnE9ySdfX1IIRtHn5zoF1Sk7yQd+wYiYqDeU+fGaQvSu0/TznE7PMfTSgS3ljMmioHdOGjn/VP7lIQXqR3XO66NovmCzBZPTKSsEn0mi/LpPodLK9rL7pxToGuExNMrW4cOOVEwsZ0ev8/4evlF4tQZ1DU9kWruWhG+343WEs6sw3Tiq46QuPooLeZETQKtxXmygpQEJNlBgoA0zGdLiRjKxKKgIE9dE6qvsrDvY1Al1kf0DvVdNVKghLUw57OvluC85gTmMi5KD1xi9LaNtFVZb6uUO0Ed99R9LR6CF6wIAAA==" target="_blank">Run the query</a>

```kusto
// IP lookup table: IP_Data
// Partial data from: 
// https://raw.githubusercontent.com/datasets/geoip2-ipv4/master/data/geoip2-ipv4.csv
let IP_Data = datatable(network:string,continent_code:string ,continent_name:string ,country_iso_code:string ,country_name:string )
[
    "111.68.128.0/17","AS","Asia","JP","Japan",
    "5.8.0.0/19","EU","Europe","RU","Russia",
    "223.255.254.0/24","AS","Asia","SG","Singapore",
    "46.36.200.51/32","OC","Oceania","CK","Cook Islands",
    "2.20.183.0/24","EU","Europe","GB","United Kingdom",
];
let IPs = datatable(ip:string)
[
    '2.20.183.12',   // United Kingdom
    '5.8.1.2',       // Russia
    '192.165.12.17', // Unknown
];
IPs
| evaluate ipv4_lookup(IP_Data, ip, network, return_unmatched = true)
```

**Output**

|ip|network|continent_code|continent_name|country_iso_code|country_name|
|---|---|---|---|---|---|
|2.20.183.12|2.20.183.0/24|EU|Europe|GB|United Kingdom|
|5.8.1.2|5.8.0.0/19|EU|Europe|RU|Russia|
|192.165.12.17||||||

### IPv4 lookup - using source in external_data()

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA12RTU/DMAyG7/0VuXWTSkrL1yjajQvigkCcJhRlrbVFS+MqcbpN4sfjfiAmcsvjR8lr2wKJlzf1rEmLtYATgXfaqobvCwd0RH+oAnnjdtkO0OkWlGkqi3yv0ZFx4EjV2MCv9UcHeaaCcXTkz8oE/KdP/ELOTFDaoTu3GIPqPJ7O1RbRDjxoAmsNwcB704AfS8tE8Nmke6IuVHnu9VHuDO3jNgbwQyQOJGts86GxABRy7sZ05ZXp+tu81YH7HmuXXNahT7+eEjvOKPB8BoP01sLCdHPaZbIZP09LWV7LYnUjizLNGOS5+HSctBGvrDXYTtqdXMlCToqYtPcYgtFTuXgsZXF/x4/I4oElLn8coQGXcBAOkXwL6LWNPAcxZFQW8RC7xbzDjGEm5sVlwgNF71R0raZ6z1HWgnyE5Q/Q4oI/9gEAAA==" target="_blank">Run the query</a>

```kusto
let IP_Data = external_data(network:string,geoname_id:long,continent_code:string,continent_name:string ,country_iso_code:string,country_name:string,is_anonymous_proxy:bool,is_satellite_provider:bool)
    ['https://raw.githubusercontent.com/datasets/geoip2-ipv4/master/data/geoip2-ipv4.csv'];
let IPs = datatable(ip:string)
[
    '2.20.183.12',   // United Kingdom
    '5.8.1.2',       // Russia
    '192.165.12.17', // Sweden
];
IPs
| evaluate ipv4_lookup(IP_Data, ip, network, return_unmatched = true)
```

**Output**

|ip|network|geoname_id|continent_code|continent_name|country_iso_code|country_name|is_anonymous_proxy|is_satellite_provider|
|---|---|---|---|---|---|---|---|---|
|2.20.183.12|2.20.183.0/24|2635167|EU|Europe|GB|United Kingdom|0|0|
|5.8.1.2|5.8.0.0/19|2017370|EU|Europe|RU|Russia|0|0|
|192.165.12.17|192.165.8.0/21|2661886|EU|Europe|SE|Sweden|0|0|

### IPv4 lookup - using extra columns for matching

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA22SS0/DMBCE7/kVe0srBYeEVynigkAIcUEgTghFbrJqLRxvZG9aKvHjWSdF5ZVDFH+eSXYmtshw91Bda9ZwCfjO6J22VSPriUPekH+bB/bGLbMlktMtVqaZW5J1TY6NQ8dVTQ1+qfY0incUBPeO/bYygX7JR/5NnJlQaUdu21Ifqs7T+3a+ILKRB81orWGMfG0a9MPWNAG5XtIVcxfmee71Ri0Nr/pFH9DHkWQgVVObx2ABOeSSxnTlgenWx3mrg+Qe9r5zVYd1+nqR2KGjIP1EBeuFxYnpvqaFfxNH/G/iafIyDJuWqjxUxexIFWWaRXDTe+pQntPbK7nnOTw7SdrAvdgaakfbiZqpQo0W+Gl7fB5tj30IRo/y4rxUxemJfEQVZ1G0lw+vEPnTBht0YAKkTzcpHEAgMAwbcinDAqHVXK+wSaQJaSH5AFxr28uPgFhSZYne+m6yO0SZwAx2J+d3N39LmX4Cz/sMXoACAAA=" target="_blank">Run the query</a>

```kusto
let IP_Data = external_data(network:string,geoname_id:long,continent_code:string,continent_name:string ,country_iso_code:string,country_name:string,is_anonymous_proxy:bool,is_satellite_provider:bool)
    ['https://raw.githubusercontent.com/datasets/geoip2-ipv4/master/data/geoip2-ipv4.csv'];
let IPs = datatable(ip:string, continent_name:string, country_iso_code:string)
[
    '2.20.183.12',   'Europe', 'GB', // United Kingdom
    '5.8.1.2',       'Europe', 'RU', // Russia
    '192.165.12.17', 'Europe', '',   // Sweden is 'SE' - so it won't be matched
];
IPs
| evaluate ipv4_lookup(IP_Data, ip, network, continent_name, country_iso_code)
```

**Output**

|ip|continent_name|country_iso_code|network|geoname_id|continent_code|country_name|is_anonymous_proxy|is_satellite_provider|
|---|---|---|---|---|---|---|---|---|
|2.20.183.12|Europe|GB|2.20.183.0/24|2635167|EU|United Kingdom|0|0|
|5.8.1.2|Europe|RU|5.8.0.0/19|2017370|EU|Russia|0|0|
