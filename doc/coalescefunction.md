---
title:  coalesce()
description: Learn how to use the coalesce() function to evaluate a list of expressions to return the first non-null expression.
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/27/2022
---
# coalesce()

Evaluates a list of expressions and returns the first non-null (or non-empty for string) expression.

## Syntax

`coalesce(`*arg*`,`*arg_2*`,[`*arg_3*`,...])`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| arg | scalar | &check; | The expression to be evaluated.|

> [!NOTE]
>
> * All arguments must be of the same type.
> * Maximum of 64 arguments is supported.

## Returns

The value of the first *arg* whose value isn't null (or not-empty for string expressions).

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/SampleLogs?query=H4sIAAAAAAAAAysoyswrUShKLS7NKbFNzk/MSS1OTtUoyc/Jz0vXUMrLL1FIVMgrzU1KLVLS1FGAiZsYgXjGxpoA2oxMXz8AAAA=" target="_blank">Run the query</a>

```kusto
print result=coalesce(tolong("not a number"), tolong("42"), 33)
```

**Output**

|result|
|---|
|42|
