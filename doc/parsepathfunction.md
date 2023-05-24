---
title:  parse_path()
description: Learn how to use the parse_path() function to parse a file path.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/12/2023
---
# parse_path()

Parses a file path `string` and returns a [`dynamic`](./scalar-data-types/dynamic.md) object that contains the following parts of the path:

* Scheme
* RootPath
* DirectoryPath
* DirectoryName
* Filename
* Extension
* AlternateDataStreamName

In addition to the simple paths with both types of slashes, the function supports paths with:

* Schemas. For example, "file://..."
* Shared paths. For example, "\\shareddrive\users..."
* Long paths. For example, "\\?\C:...""
* Alternate data streams. For example, "file1.exe:file2.exe"

## Syntax

`parse_path(`*path*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *path* | string | &check; | The file path.|

## Returns

An object of type `dynamic` that included the path components as listed above.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA22NQQrDIBBF94J3EFcJFN0LhUKOUUMwOE0Ek4ozASk9fOMiLYHOrOY95n/vaN8xQpMMUg7r1ArO7pyJfW6yM5ZgSfYRIigqJC+H+YtlBUbrzujq9eENPhdQUOD3by3OLoO3G0LGc4uaXt9EvWHWMYznQMlZz9lbQCFYvUiO5iG5TCiu+5ERhoqa1H4A8WAkd+AAAAA=" target="_blank">Run the query</a>

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

**Output**

|p|path_parts
|---|---
|C:\temp\file.txt|{"Scheme":"","RootPath":"C:","DirectoryPath":"C:\\temp","DirectoryName":"temp","Filename":"file.txt","Extension":"txt","AlternateDataStreamName":""}
|temp\file.txt|{"Scheme":"","RootPath":"","DirectoryPath":"temp","DirectoryName":"temp","Filename":"file.txt","Extension":"txt","AlternateDataStreamName":""}
|file://C:/temp/file.txt:some.exe|{"Scheme":"file","RootPath":"C:","DirectoryPath":"C:/temp","DirectoryName":"temp","Filename":"file.txt","Extension":"txt","AlternateDataStreamName":"some.exe"}
|\\shared\users\temp\file.txt.gz|{"Scheme":"","RootPath":"","DirectoryPath":"\\\\shared\\users\\temp","DirectoryName":"temp","Filename":"file.txt.gz","Extension":"gz","AlternateDataStreamName":""}
|/usr/lib/temp/file.txt|{"Scheme":"","RootPath":"","DirectoryPath":"/usr/lib/temp","DirectoryName":"temp","Filename":"file.txt","Extension":"txt","AlternateDataStreamName":""}
