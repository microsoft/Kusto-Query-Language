---
title: parse_command_line() - Azure Data Explorer
description: This article describes parse_command_line() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: slneimer
ms.service: data-explorer
ms.topic: reference
ms.date: 06/28/2020
---
# parse_command_line()

Parses a Unicode command-line string and returns a dynamic array of the command-line arguments.

## Syntax

`parse_command_line(`*command_line*,*parser_type*`)`

## Arguments

* *command_line*: Command line to parse.
* *parser_type*: The only value that is currently supported is `"windows"`, which parses the command line the same way as [CommandLineToArgvW](/windows/win32/api/shellapi/nf-shellapi-commandlinetoargvw).

## Returns

A dynamic array of the command-line arguments.

## Example

<!-- csl: https://help.kusto.windows.net:443/Samples -->
```kusto
print parse_command_line("echo \"hello world!\"", "windows")
```

|Result|
|---|
|["echo","hello world!"]|
