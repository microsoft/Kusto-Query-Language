---
title:  Views
description: This article describes views in Azure Data Explorer.
ms.reviewer: zivc
ms.topic: reference
ms.date: 04/16/2023
---
# Views

**Views** are virtual tables based on the result-set of a Kusto Query Language query.
Just like a real table, a view contains rows and columns. Unlike a real table,
a view doesn't hold its own data storage.

## Define a view

Views are defined through [user-defined functions](../functions/user-defined-functions.md)
with the following requirements:

* The result of the function must be tabular (for example, it cannot be a scalar value).
* The function must take no arguments.

Views can be based on either [stored functions](./stored-functions.md) or defined as part of the query using a [let statement](../letstatement.md).

> [!NOTE]
> Views are not technically schema entities. However, all functions that comply
> with the constraints above are regarded as views.

### Example: Define and use a view

The following query defines and uses a view. The view
is used as-if a table called `T` was defined (there's no need to reference the
function `T` using the function call syntax `T()`):

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA8tJLVEIsdXQVKguKMrMK1GosDXUUai0Naq15uUKAQDE65hsHQAAAA==" target="_blank">Run the query</a>

```kusto
let T=() {print x=1, y=2};
T
```

**Returns**

x |y |
--|--|
1 | 2 |

## The view keyword

By default, operators that support a wildcard syntax to specify table names will not reference views, even if the view's name matches the wildcard. An example of this type of operator is the [union operator](../unionoperator.md). In this case, use the `view` keyword to have the view
included as well.

### Example: Use the view keyword

For example, the results of the following query include the `T1` view, but not `T2`:

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA8tJLVEIMbQty0wtV9DQrC4oyswrUfBLzE21VQoxVKq15uXKAakwskWXNAJLluZl5ucphGgBAJuXYhRHAAAA" target="_blank">Run the query</a>

```kusto
let T1=view (){print Name="T1"};
let T2=(){print Name="T2"};
union T*
```
