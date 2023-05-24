---
title:  Pattern statement
description: Learn how to use pattern statements to map string tuples to tabular expressions.
ms.reviewer: alexans
ms.topic: reference
ms.date: 05/01/2023
zone_pivot_group_filename: data-explorer/zone-pivot-groups.json
zone_pivot_groups: kql-flavors-all
---

# Pattern statement

::: zone pivot="azuredataexplorer, fabric"

A **pattern** is a construct that maps string tuples to tabular expressions. Each pattern must *declare* a pattern name and optionally *define* a pattern mapping. Patterns that define a mapping return a tabular expression when invoked. Any two statements must be separated by a semicolon.

*Empty patterns* are patterns that are declared but don't define a mapping. When invoked, they return error *SEM0036* along with the details of the missing pattern definitions in the HTTP header. Middle-tier applications that provide a Kusto Query Language (KQL) experience can use the returned details as part of their process to enrich KQL query results.
For more information, see [Working with middle-tier applications](#work-with-middle-tier-applications).

## Syntax

* Declare an empty pattern:

    `declare` `pattern` *PatternName* `;`

* Declare and define a pattern:

    `declare` `pattern` *PatternName* = `(`*ArgName* `:` *ArgType* [`,` ... ]`)` [`[` *PathName* `:` *PathArgType* `]`]

    `{`

    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;`(` *ArgValue1_1* [`,` *ArgValue2_1*`,` ... ] `)` [ `.[` *PathValue_1* `]` ] `=` `{` *expression1* `}` `;`

    &nbsp;&nbsp;&nbsp;&nbsp;[ `(` *ArgValue1_2* [`,` *ArgValue2_2*`,` ... ] `)` [ `.[` *PathValue_2* `]` ] `=` `{` *expression2* `}` `;` ... ]

    `}` `;`

* Invoke a pattern:

  * *PatternName* `(` *ArgValue1* [`,` *ArgValue2* ...] `).`*PathValue*
  * *PatternName* `(` *ArgValue1* [`,` *ArgValue2* ...] `).["`*PathValue*`"]`

## Parameters

| Name | Type | Required | Description |
| -- | -- | -- | -- |
| *PatternName* | string | &check; | The name of the pattern. |
| *ArgName* | string | &check; | The name of the argument. Patterns can have one or more arguments. |
| *ArgType* | string | &check; | The scalar data type of the *ArgName* argument. Possible values: `string` |
| *PathName* | string | | The name of the path argument. Patterns can have no path or one path. |
| *PathArgType* | string | | The type of the *PathArgType* argument. Possible values: `string` |
| *ArgValue* | string | &check; | The *ArgName* and optional *PathName* tuple values to be mapped to an *expression*.  |
| *PathValue* | string | | The value to map for *PathName*. |
| *expression* | string | &check; | A tabular or lambda expression that references a function returning tabular data. For example: `Logs | where Timestamp > ago(1h)` |

## Examples

In each of the following examples, a pattern is declared, defined, and then invoked.

### Define simple patterns

The following example defines a pattern that maps states to an expression that returns its capital city.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA3XPsQrCMBDG8b3QdzgytYsPoDiU4qqDiEjp8NkGLaaXkJ5IEd/dtFTUoUOGO+73h9S6MvCaHES0Z6rsncX3tKaE0eplJ77hS1p0AvlMZRw944goUYd9ptJFobb6QSfrb6oM8EkuXAnlcI3AhI3KzBncK3qt/t0R3TUExfKc3Jm+dQ2+NAejxqhDVXvBHN3UreUhPdrhTZ/7iUyJN/8CZSgGAQAA" target="_blank">Run the query</a>

```kusto
declare pattern country = (name:string)[state:string]
{
  ("USA").["New York"] = { print Capital = "Albany" };
  ("USA").["Washington"] = { print Capital = "Olympia" };
  ("Canada").["Alberta"] = { print Capital = "Edmonton" };
};
country("Canada").Alberta
```

**Output** 

|Capital|
|-------|
|Edmonton|

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA62RsQrCMBCGd6Hv8FOHtlDFtrgoHQQXB5+gdAhpLJWahCRCofruJpjBCi7SwJHckfv/fJeG0Z4oBkmMYYrjICVKxETKvqPEdIKfmp02quNtUmkqJPNZDQSLMVjArjgiWZSsq+hIDIlqVyoxQhHeMgy4KHFDBiOwhTZM2vMDUokro8Ybhm5bZmEKJ/EWGPDcf+mfmfWm2lr8re8lbM22N3HyaZLPAZFPILINVlOSYg6S4heJizu33+aueyT3mtTnbo6+8QXib67/+wEAAA==" target="_blank">Run the query</a>

The following example defines a pattern that defines some scoped application data.

```kusto
declare pattern App = (applicationId:string)[scope:string]  
{
    ('a1').['Data']    = { range x from 1 to 5 step 1 | project App = "App #1", Data    = x };
    ('a1').['Metrics'] = { range x from 1 to 5 step 1 | project App = "App #1", Metrics = rand() };
    ('a2').['Data']    = { range x from 1 to 5 step 1 | project App = "App #2", Data    = 10 - x };
    ('a3').['Metrics'] = { range x from 1 to 5 step 1 | project App = "App #3", Metrics = rand() };
};
union App('a2').Data, App('a1').Metrics
```

**Output**

|App|Data|Metrics|
|---|----|-------|
|App #2|9| |
|App #2|8| |
|App #2|7| |
|App #2|6| |
|App #2|5| |
|App #1| |0.53674122855537532|
|App #1| |0.78304713305654439|
|App #1| |0.20168860732346555|
|App #1| |0.13249123867679469|
|App #1| |0.19388305330563443|

### Normalization

There are syntax variations for invoking patterns. For example, the following union returns a single pattern expression since all the invocations are of the same pattern.

```kusto
declare pattern app = (applicationId:string)[eventType:string]
{
    ("ApplicationX").["StopEvents"] = { database("AppX").Events | where EventType == "StopEvent" };
    ("ApplicationX").["StartEvents"] = { database("AppX").Events | where EventType == "StartEvent" };
};
union
  app("ApplicationX").StartEvents,
  app('ApplicationX').StartEvents,
  app("ApplicationX").['StartEvents'],
  app("ApplicationX").["StartEvents"]
```

### No wildcards

There's no special treatment given to wildcards in a pattern. For example, the following query returns a single missing pattern invocation.

```kusto
declare pattern app = (applicationId:string)[eventType:string]
{
    ("ApplicationX").["StopEvents"] = { database("AppX").Events | where EventType == "StopEvent" };
    ("ApplicationX").["StartEvents"] = { database("AppX").Events | where EventType == "StartEvent" };
};
union app("ApplicationX").["*"]
| count
```

**Returns semantic error**
> One or more pattern references were not declared. Detected pattern references: ["app('ApplicationX').['*']"]

## Work with middle-tier applications

A middle-tier application provides its users with the ability to use KQL and wants to enhance the experience by enriching the query results with augmented data from its internal service.

To this end, the application provides users with a pattern statement that returns tabular data that their users can use in their queries. The pattern's arguments are the keys the application will use to retrieve the enrichment data. When the user runs the query, the application does not parse the query itself but instead plans to leverage the error returned by an empty pattern to retrieve the keys it requires. So it prepends the query with the empty pattern declaration, sends it to the cluster for processing, and then parses the returned HTTP header to retrieve the values of missing pattern arguments. The application uses these values to look up the enrichment data and builds a new declaration that defines the appropriate enrichment data mapping. Finally, the application prepends the new definition to the user's query, resends it for processing, and returns the result it receives to the user.

### Example

In the following example, a middle-tier application provides the ability to enrich queries with longitude/latitude locations. The application uses an internal service to map IP addresses to longitude/latitude locations, and provides a pattern called `map_ip_to_longlat` for this purpose. Let's suppose the application gets the following query from the user:

```kusto
map_ip_to_longlat("10.10.10.10")
```

The application does not parse this query and hence does not know which IP address (*10.10.10.10*) was passed to the pattern. So it prepends the user query with an empty `map_ip_to_longlat` pattern declaration and sends it for processing:

```kusto
declare pattern map_ip_to_longlat;
map_ip_to_longlat("10.10.10.10")
```

The application receives the following error in response.
> One or more pattern references were not declared. Detected pattern references: ["map_ip_to_longlat('10.10.10.10')"]

The application inspects the error, determines that the error indicates a missing pattern reference, and retrieves the missing IP address (*10.10.10.10*). It uses the IP address to look up the enrichment data in its internal service and builds a new pattern defining the mapping of the IP address to the corresponding longitude and latitude data. The new pattern is prepended to the user's query and run again. This time the query succeeds because the enrichment data is now declared in the query, and the result is sent to the user.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA2WNQQrDIBAA74J/WHJKoBW1lSQt/iB/kKVKCFgjureQv9dLLy3McZjx4RWxBMhIFEqCN2a3ZUe7i3taIxJY6NH7Emp9VCpbWgfODs4A+k5J8aUbmnhAbgLBgmRvo7hLM8/6Aksr2avSWshxMsrAydn55Ozv9VP8AIoi/+WcAAAA" target="_blank">Run the query</a>

```kusto
declare pattern map_ip_to_longlat = (address:string)
{
  ("10.10.10.10") = { print Lat=37.405992, Long=-122.078515 }
};
map_ip_to_longlat("10.10.10.10")
```

**Output**

|Lat|Long|
|---|---|
|37.405992|-122.078515|

::: zone-end

::: zone pivot="azuremonitor"

This capability isn't supported in Azure Monitor.

::: zone-end
