---
title:  parse_user_agent()
description: Learn how to use the parse_user_agent() to return a dynamic object that contains information about the user-agent.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/08/2023
---
# parse_user_agent()

Interprets a user-agent string, which identifies the user's browser and provides certain system details to servers hosting the websites the user visits. The result is returned as [`dynamic`](./scalar-data-types/dynamic.md).

## Syntax

`parse_user_agent(`*user-agent-string*, *look-for*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *user-agent-string*| string | &check; | The user-agent string to parse.|
| *look-for*| string or dynamic | &check; | The value to search for in *user-agent-string*. The possible options are "browser", "os", or "device". If only a single parsing target is required, it can be passed a `string` parameter. If two or three targets are required, they can be passed as a `dynamic` array.|

## Returns

An object of type `dynamic` that contains the information about the requested parsing targets.

Browser: Family, MajorVersion, MinorVersion, Patch

OperatingSystem: Family, MajorVersion, MinorVersion, Patch, PatchMinor

Device: Family, Brand, Model

> [!WARNING]
> The function implementation is built on regex checks of the input string against a huge number of predefined patterns. Therefore the expected time and CPU consumption is high.
When the function is used in a query, make sure it runs in a distributed manner on multiple machines.
If queries with this function are frequently used, you may want to pre-create the results via [update policy](../management/updatepolicy.md), but you need to take into account that using this function inside the update policy will increase the ingestion latency.
 
## Examples

### Look-for parameter as string

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAz3OsQrCMBSF4d2nuGRKoSZWySDFoZNK7aKWjiWxFwkNSUgqFvHhjQ5uZ/jhOz5oO8EjYpB3TGsHpHEvbYzkgq2AdtoO7hlLaEtAu2wvGVTeG+xQ1XriYlOwLdD6cG1OORg9IuzxNrpUDU5hdTzzNROsIIs34DyhHWBOhpchYv9V+x9L/wdyICokEAPJPkPKisOcAAAA" target="_blank">Run the query</a>

```kusto
print useragent = "Mozilla/5.0 (Windows; U; en-US) AppleWebKit/531.9 (KHTML, like Gecko) AdobeAIR/2.5.1"
| extend x = parse_user_agent(useragent, "browser") 
```

Expected result is a dynamic object:

{
  "Browser": {
    "Family": "AdobeAIR",
    "MajorVersion": "2",
    "MinorVersion": "5",
    "Patch": "1"
  }
}

### Look-for parameter as dynamic array

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAz2PQUvDQBCF7/6KYU8JJNlsUkUpHiQFlTa1EKWHImWSTMqQ7W7YpNqKP97Vgzx4DA/e8L3BsZngNJLDA/nrHkRpv1hrlNdJCkF1OdaM5qWSd0k2h7c5VOSYxptU5omCte0Z17cqzqVKE688g42zHWuS5fNiE2f+SWFNx4eTw4mtkcVqUcTKd0N4GAZNW6qXPMmZyiFYPr2Wqwg09wSP1PQ2hAo7dCxn4uob6DyRaeHsKQd0I+1/ufd/4MH/hAjai8EjN8FO1M5++lxEwo7eWvrghsR7GP4AbEGfNfcAAAA=" target="_blank">Run the query</a>

```kusto
print useragent = "Mozilla/5.0 (SymbianOS/9.2; U; Series60/3.1 NokiaN81-3/10.0.032 Profile/MIDP-2.0 Configuration/CLDC-1.1 ) AppleWebKit/413 (KHTML, like Gecko) Safari/4"
| extend x = parse_user_agent(useragent, dynamic(["browser","os","device"])) 
```

Expected result is a dynamic object:

{
  "Browser": {
    "Family": "Nokia OSS Browser",
    "MajorVersion": "3",
    "MinorVersion": "1",
    "Patch": ""
  },
  "OperatingSystem": {
    "Family": "Symbian OS",
    "MajorVersion": "9",
    "MinorVersion": "2",
    "Patch": "",
    "PatchMinor": ""
  },
  "Device": {
    "Family": "Nokia N81",
    "Brand": "Nokia",
    "Model": "N81-3"
  }
}
