---
title:  Add a comment in KQL
description: Learn how to comment single and multi-lines in Kusto Query Language.
ms.reviewer: andresilva
ms.topic: reference
ms.date: 05/30/2023
---
# Add a comment in KQL

Indicates user-provided text. Comments can be inserted on a separate line, nested at the end or within a KQL query or command. The engine does not evaluate the comment. 

## Syntax

```kusto
// text of comment
```

## Remarks

Use the two slashes (//) for single and multi-line comments. The following table lists the keyboard shortcuts that you can use to comment or uncomment text.

| Hot Key  | Description  |
| ------------ | ------------ |
| `Ctrl`+`K`, `Ctrl`+`C`  | Comment current line or selected lines.  |
| `Ctrl`+`K`, `Ctrl`+`U`  | Uncomment current line or selected lines.  |

## Example

This example returns a count of events in the New York state:

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA02OMQ6DMBAEe16x4gN+ASVpIhGJFBElIYuwAj7pfECTxyfGKWhHO6N1Di1t1QCbiEHWYJAR3BgswmfccEcn+ka03ohRZTn43USXOk+tf84sTqT4YJ+oaZWkqkLZ1A90t/Zawjlc/GzUo6McRF/xv8/l5Ph4cn69494XVG+MV7IAAAA=" target="_blank">Run the query</a>

```kusto
// Return the count of events in the New York state from the StormEvents table
StormEvents
| where State == "NEW YORK" // Filter the records where the State is "NEW YORK"
| count
```