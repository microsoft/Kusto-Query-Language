---
title: current_principal_details() - Azure Data Explorer
description: This article describes current_principal_details() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 11/08/2019
---
# current_principal_details()

Returns details of the principal running the query.

## Syntax

`current_principal_details()`

## Returns

The details of the current principal as a `dynamic`.

## Example

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
print d=current_principal_details()
```

|d|
|---|
|{<br>  "UserPrincipalName": "user@fabrikam.com",<br>  "IdentityProvider": "https://sts.windows.net",<br>  "Authority": "72f988bf-86f1-41af-91ab-2d7cd011db47",<br>  "Mfa": "True",<br>  "Type": "AadUser",<br>  "DisplayName": "James Smith (upn: user@fabrikam.com)",<br>  "ObjectId": "346e950e-4a62-42bf-96f5-4cf4eac3f11e",<br>  "FQN": null,<br>  "Notes": null<br>}|
