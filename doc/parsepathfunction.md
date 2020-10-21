---
title: parse_path() - Azure Data Explorer
description: This article describes parse_path() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 10/23/2018
---
# parse_path()

Parses a file path `string` and returns a [`dynamic`](./scalar-data-types/dynamic.md) object that contains the following parts of the path:
* Scheme
* RootPath
* DirectoryPath
* DirectoryName
* FileName
* Extension
* AlternateDataStreamName

In addition to the simple paths with both types of slashes, the function supports paths with:
* Schemas. For example, "file://..."
* Shared paths. For example, "\\shareddrive\users..."
* Long paths. For example, "\\?\C:...""
* Alternate data streams. For example, "file1.exe:file2.exe"

## Syntax

`parse_path(`*path*`)`

## Arguments

* *path*: A string that represents a file path.

## Returns

An object of type `dynamic` that included the path components as listed above.

## Example

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
datatable(p:string) 
[
    @"C:\temp\file.txt",
    @"temp\file.txt",
    "file://C:/temp/file.txt:some.exe",
    @"\\shared\users\temp\file.txt.gz",
    "/usr/lib/temp/file.txt"
]
| extend path_parts = parse_path(p)

```

|p|path_parts
|---|---
|C:\temp\file.txt|{"Scheme":"","RootPath":"C:","DirectoryPath":"C:\\temp","DirectoryName":"temp","Filename":"file.txt","Extension":"txt","AlternateDataStreamName":""}
|temp\file.txt|{"Scheme":"","RootPath":"","DirectoryPath":"temp","DirectoryName":"temp","Filename":"file.txt","Extension":"txt","AlternateDataStreamName":""}
|file://C:/temp/file.txt:some.exe|{"Scheme":"file","RootPath":"C:","DirectoryPath":"C:/temp","DirectoryName":"temp","Filename":"file.txt","Extension":"txt","AlternateDataStreamName":"some.exe"}
|\\shared\users\temp\file.txt.gz|{"Scheme":"","RootPath":"","DirectoryPath":"\\\\shared\\users\\temp","DirectoryName":"temp","Filename":"file.txt.gz","Extension":"gz","AlternateDataStreamName":""}
|/usr/lib/temp/file.txt|{"Scheme":"","RootPath":"","DirectoryPath":"/usr/lib/temp","DirectoryName":"temp","Filename":"file.txt","Extension":"txt","AlternateDataStreamName":""}
