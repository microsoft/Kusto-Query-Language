---
title:  has_ipv4()
description: Learn how to use the has_ipv4() function to check if a specified IPv4 address appears in the text.
ms.reviewer: alexans
ms.topic: reference
ms.date: 12/18/2022
---
# has_ipv4()

Returns a value indicating whether a specified IPv4 address appears in a text.

IP address entrances in a text must be properly delimited with non-alphanumeric characters. For example, properly delimited IP addresses are:

* "These requests came from: 192.168.1.1, 10.1.1.115 and 10.1.1.201"
* "05:04:54 127.0.0.1 GET /favicon.ico 404"

## Syntax

`has_ipv4(`*source* `,` *ip_address* `)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *source* | string | &check; | The text to search.|
| *ip_address* | string | &check; | The value containing the IP address for which to search.|

## Returns

`true` if the *ip_address* is a valid IPv4 address, and it was found in *source*. Otherwise, the function returns `false`.

> [!TIP]
>
> * To search for many IPv4 addresses at once, use [has_any_ipv4()](has-any-ipv4-function.md) function.
> * To search for IPv4 addresses prefix, use [has_ipv4_prefix()](has-ipv4-prefix-function.md) function.

## Examples

### Properly formatted IP address

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUShKLS7NKbHNSCyOzywoM9FQNzC1MjCxMjVRMDQy1zMAQkMFd9cQBf20xLLM5Pw8PSChYGJgoq6joA5Xoa4JAIl8hqBNAAAA" target="_blank">Run the query</a>

```kusto
print result=has_ipv4('05:04:54 127.0.0.1 GET /favicon.ico 404', '127.0.0.1')
```

**Output**

|result|
|--|
|true|

### Invalid IP address

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUShKLS7NKbHNSCyOzywoM9FQNzC1MjCxMjVRMDQy1zMAQiNTMwV31xAF/bTEsszk/Dw9IKFgYmCirqOgjqRGXRMAk108LFEAAAA=" target="_blank">Run the query</a>

```kusto
print result=has_ipv4('05:04:54 127.0.0.256 GET /favicon.ico 404', '127.0.0.256')
```

**Output**

|result|
|--|
|false|

### Improperly delimited IP

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUShKLS7NKbHNSCyOzywoM9FQNzC1MjCxMjUxNDLXMwBCQwV31xAF/bTEsszk/Dw9IKFgYmCirqOgDlehrgkAPqxiHkwAAAA=" target="_blank">Run the query</a>

```kusto
print result=has_ipv4('05:04:54127.0.0.1 GET /favicon.ico 404', '127.0.0.1')
```

**Output**

|result|
|--|
|false|
