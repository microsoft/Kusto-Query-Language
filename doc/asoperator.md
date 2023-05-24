---
title:  as operator
description: Learn how to use the as operator to bind a name to the operator's input tabular expression.
ms.reviewer: alexans
ms.topic: reference
ms.date: 03/14/2023
---
# as operator

Binds a name to the operator's input tabular expression. This allows the query to reference the value of the tabular expression multiple times without breaking the query and binding a name through the [let statement](letstatement.md).

To optimize multiple uses of the `as` operator within a single query, see [Named expressions](../../named-expressions.md).

## Syntax

*T* `|` `as` [`hint.materialized` `=` `true`] *Name*

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
|*T*| string | &check; | The tabular expression to rename.|
| *Name*| string| &check; | The temporary name for the tabular expression.|
| *`hint.materialized`*| bool |  | If set to `true`, the value of the tabular expression will be as if it was wrapped by a [materialize()](./materializefunction.md) function call.|

> [!NOTE]
>
> * The name given by `as` will be used in the `withsource=` column of [union](./unionoperator.md), the `source_` column of [find](./findoperator.md), and the `$table` column of [search](./searchoperator.md).
> * The tabular expression named using the operator in a [join](./joinoperator.md)'s outer tabular input (`$left`) can also be used in the join's tabular inner input (`$right`).

## Examples

In the following two examples the union's generated TableName column will consist of 'T1' and 'T2'.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/?query=H4sIAAAAAAAAAytKzEtPVahQSCvKz1UwVCjJVzA0UCguSS0AcrhqFBKLFULAjNK8zPw8hfLMkozi/NKi5FTbkMSknFS/xNxUBY0iPGZAjDDSBAAgKK6faAAAAA==" target="_blank">Run the query</a>

```kusto
range x from 1 to 10 step 1 
| as T1 
| union withsource=TableName (range x from 1 to 10 step 1 | as T2)
```

Alternatively, you can write the same example as follows:

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/?query=H4sIAAAAAAAAAyvNy8zPUyjPLMkozi8tSk61DUlMykn1S8xNVdAoSsxLT1WoUEgrys9VMFQoyVcwNFAoLkktAHJqFBKLFUIMNXWIUWakCQB5tG07ZwAAAA==" target="_blank">Run the query</a>

```kusto
union withsource=TableName (range x from 1 to 10 step 1 | as T1), (range x from 1 to 10 step 1 | as T2)
```

In the following example, the 'left side' of the join will be:
`MyLogTable` filtered by `type == "Event"` and `Name == "Start"`
and the 'right side' of the join will be:
`MyLogTable` filtered by `type == "Event"` and `Name == "Stop"`

```kusto
MyLogTable  
| where type == "Event"
| as T
| where Name == "Start"
| join (
    T
    | where Name == "Stop"
) on ActivityId
```
