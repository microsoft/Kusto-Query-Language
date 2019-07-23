---
title: parse_user_agent() - Azure Data Explorer | Microsoft Docs
description: This article describes parse_user_agent() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 10/23/2018
---
# parse_user_agent()

Interprets a user-agent string, which identifies the user's browser and provides certain system details to servers hosting the websites the user visits. The result is returned as [`dynamic`](./scalar-data-types/dynamic.md). 

**Syntax**

`parse_user_agent(`*user-agent-string*, *look-for*`)`

**Arguments**

* *user-agent-string*: An expression of type `string`, representing a user-agent string.

* *look-for*: An expression of type `string` or `dynamic`, representing what the function should be looking for in the user-agent string (parsing target). 
The possible options: "browser", "os", "device". If only a single parsing target is required it can be passed a `string` parameter.
If two or three are required they can be passed as a `dynamic array`.

**Returns**

An object of type `dynamic` that contains the information about the requested parsing targets.

Browser: Family, MajorVersion, MinorVersion, Patch                 

OperatingSystem: Family, MajorVersion, MinorVersion, Patch, PatchMinor             

Device: Family, Brand, Model
 
**Example**

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