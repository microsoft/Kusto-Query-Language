---
title: pattern statement - Azure Data Explorer
description: This article describes pattern statement in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
zone_pivot_group_filename: data-explorer/zone-pivot-groups.json
zone_pivot_groups: kql-flavors
---
# pattern statement

::: zone pivot="azuredataexplorer"

A **pattern** is a named view-like construct that maps predefined
string tuples to parameterless function bodies. Patterns are unique
in two aspects:

* Patterns are "invoked" by using a syntax that resembles scoped table references.
* Patterns have a controlled, close-ended, set of argument values that
  can be mapped, and the mapping process is done by Kusto. If a pattern is declared but not defined, Kusto identifies and flags all invocations to the pattern as errors. This identification makes it possible to "resolve" these patterns by a middle-tier application.

## Pattern declaration

The pattern statement is used to declare or define a pattern.
For example, a pattern statement that declares `app`
to be a pattern.

```kusto
declare pattern app;
```

This statement tells Kusto that `app` is a pattern, but doesn't
tell Kusto how to resolve the pattern. As a result, any attempt to
invoke this pattern in the query will result in a specific error, and will 
list all such invocations. For example:

```kusto
declare pattern app;
app("ApplicationX").StartEvents
| join kind=inner app("ApplicationX").StopEvents on CorrelationId
| count
```

This query will generate an error from Kusto, indicating that the next
pattern invocations can't be resolved: `app("ApplicationX")["StartEvents"]`
and `app("ApplicationX")["StopEvents"]`.

## Syntax of pattern declaration

`declare` `pattern` *PatternName*

## Pattern definition

The pattern statement can also be used to define a pattern. In a pattern
definition, all possible invocations of the pattern are explicitly laid
out, and the corresponding tabular expression given. When Kusto then executes
the query, it replaces each pattern invocation with the corresponding pattern body. For example:

```kusto
declare pattern app = (applicationId:string)[eventType:string]
{
    ("ApplicationX").["StopEvents"] = { database("AppX").Events | where EventType == "StopEvent" };
    ("ApplicationX").["StartEvents"] = { database("AppX").Events | where EventType == "StartEvents" } ;
};
app("ApplicationX").StartEvents
| join kind=inner app("ApplicationX").StopEvents on CorrelationId
| count
```

The expression that is provided for each pattern that is matched, is either a table name or a reference to a [let statement](letstatement.md).

## Syntax of pattern definition

`declare` `pattern` *PatternName* = `(`*ArgName* `:` *ArgType* [`,` ... ]`)` [`[` *PathName* `:` *PathArgType* `]`]
`{`
&nbsp;&nbsp;&nbsp;&nbsp; `(` *ArgValue1* [`,` *ArgValue2* ... ] `)` [ `.[` *PathValue `]` ] `=` `{`  *expression*  `};`
&nbsp;&nbsp;&nbsp;&nbsp; [
&nbsp;&nbsp;&nbsp;&nbsp; `(` *ArgValue1_2* [`,` *ArgValue2_2* ... ] `)` [ `.[` *PathValue_2* `]` ] `=` `{`  *expression_2*  `};`
&nbsp;&nbsp;&nbsp;&nbsp; ...
&nbsp;&nbsp;&nbsp;&nbsp; ]
`}`

* *PatternName*: Name of the pattern keyword. Syntax that defines keyword only is allowed: for detecting all pattern references with a specified keyword.
* *ArgName*: Name of the pattern argument. Patterns allow one or more argument names.
* *ArgType*: Type of the pattern argument (currently only `string` is allowed)
* *PathName*: Name of the path argument. Patterns allow zero or one path name.
* *PathType*: Type of the path argument (currently only `string` is allowed)
* *ArgValue1*, *ArgValue2*, ... - values of the pattern arguments (currently only `string` literals are allowed)
* *PathValue* - value of the pattern path (currently only `string` literals are allowed)
* *expression*: The *expression* - a tabular expression (for example, `Logs | where Timestamp > ago(1h)`),
  or a lambda expression that references a function.

## Pattern invocation

The pattern invocation syntax is similar to the scoped table reference syntax.

* *PatternName* `(` *ArgValue1* [`,` *ArgValue2* ...] `).`*PathValue*
* *PatternName* `(` *ArgValue1* [`,` *ArgValue2* ...] `).["`*PathValue*`"]`

## Notes

**Scenario**

The pattern statement is designed for middle-tier applications that accept user queries and then send these queries to Kusto. Such applications often prefix those user queries with a logical schema model. The model is a set of [let statements](letstatement.md), possibly suffixed by a [restrict statement](restrictstatement.md).

Some applications need a syntax that they can provide users. The syntax is used to reference entities that are defined in the logical schema that the applications construct. However, sometimes entities aren't known ahead of time, or the number of potential entities is too large to be pre-defined in the logical schema.

Pattern solves this scenario in the following way. The middle-tier application sends
the query to Kusto with all patterns declared, but not defined. Kusto then parses the
query. If there are one or more pattern invocations, Kusto returns an error to
the middle-tier application with all such invocations explicitly listed. The middle-tier application can then resolve each of these references, and rerun the query. This time, prefixing it with the fully elaborated pattern definition.

**Normalizations**

Kusto automatically normalizes the pattern. For example, the following are all
invocations of the same pattern, and a single one is reported back.

```kusto
declare pattern app;
union
  app("ApplicationX").StartEvent,
  app('ApplicationX').StartEvent,
  app("ApplicationX").['StartEvent'],
  app("ApplicationX").["StartEvent"]
```

This also means that you can't define them together, since they're considered
to be the same.

**Wildcards**

Kusto doesn't treat wildcards in a pattern in any special way. For example,
in the following query.

```kusto
declare pattern app;
union app("ApplicationX").*
| count
```

Kusto will report a single missing pattern invocation: `app("ApplicationX").["*"]`.

## Examples

Queries over more than a single pattern invocation.

```kusto
declare pattern A
{
    // ...
};
union (A('a1').Text), (A('a2').Text)
```

|App|SomeText|
|---|---|
|App #1|This is a free text: 1|
|App #1|This is a free text: 2|
|App #1|This is a free text: 3|
|App #1|This is a free text: 4|
|App #1|This is a free text: 5|
|App #2|This is a free text: 9|
|App #2|This is a free text: 8|
|App #2|This is a free text: 7|
|App #2|This is a free text: 6|
|App #2|This is a free text: 5|

```kusto
declare pattern App;
union (App('a1').Text), (App('a2').Text)
```

**Semantic error**:

> SEM0036: One or more pattern references weren't declared. Detected pattern references: ["App('a1').['Text']","App('a2').['Text']"].

```kusto
declare pattern App;
declare pattern App = (applicationId:string)[scope:string]  
{
    ('a1').['Data']    = { range x from 1 to 5 step 1 | project App = "App #1", Data    = x };
    ('a1').['Metrics'] = { range x from 1 to 5 step 1 | project App = "App #1", Metrics = rand() };
    ('a2').['Data']    = { range x from 1 to 5 step 1 | project App = "App #2", Data    = 10 - x };
    ('a3').['Metrics'] = { range x from 1 to 5 step 1 | project App = "App #3", Metrics = rand() };
};
union (App('a2').Metrics), (App('a3').Metrics) 
```

**Semantic error returned**:

> SEM0036: One or more pattern references weren't declared. Detected pattern references: ["App('a2').['Metrics']","App('a3').['Metrics']"].

::: zone-end

::: zone pivot="azuremonitor"

This capability isn't supported in Azure Monitor

::: zone-end
