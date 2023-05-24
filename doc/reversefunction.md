---
title:  reverse()
description: Learn how to use the reverse() function to reverse the order of the input string.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/17/2023
---
# reverse()

Function reverses the order of the input string.
If the input value isn't of type `string`, then the function forcibly casts the value to type `string`.

## Syntax

`reverse(`*value*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *value* | string | &check; | input value.|  

## Returns

The reverse order of a string value.

## Examples

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUSguKVKwVVBydHJ2cXVz9/D08vbx9fMPCAwKDgkNC4+IjFLiqlFIrShJzUtRKIIoLkotSy0qTtUA8jQB/i1rL0UAAAA=" target="_blank">Run the query</a>

```kusto
print str = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
| extend rstr = reverse(str)
```

**Output**

|str|rstr|
|---|---|
|ABCDEFGHIJKLMNOPQRSTUVWXYZ|ZYXWVUTSRQPONMLKJIHGFEDCBA|

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUYhWB5LqsQq2CoZGxiamOkCBlPzSpJxUmJgeSJALKJpYklqSmQsRh3E0jAwMzXUNDXQNTYFqrQwMNEEGgGSKCxLzwEqNM7hqFAqK8rNSk0sUwFbaKhSllqUWFadqQC0HaiqCWIoiB3MHUJqrCGYjqgq4m0BGwKxFUYJwiyYA300QcvEAAAA=" target="_blank">Run the query</a>

```kusto
print ['int'] = 12345, ['double'] = 123.45, 
['datetime'] = datetime(2017-10-15 12:00), ['timespan'] = 3h
| project rint = reverse(['int']), rdouble = reverse(['double']), 
rdatetime = reverse(['datetime']), rtimespan = reverse(['timespan'])
```

**Output**

|rint|rdouble|rdatetime|rtimespan|
|---|---|---|---|
|54321|54.321|Z0000000.00:00:21T51-01-7102|00:00:30|
