---
title:  current_principal_details()
description: Learn how to use the current_principal_details() function to return the details of the principal running the query. 
ms.reviewer: alexans
ms.topic: reference
ms.date: 04/16/2023
---
# current_principal_details()

Returns details of the principal running the query.

## Syntax

`current_principal_details()`

## Returns

The details of the current principal as a `dynamic`.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUUixTS4tKkrNK4kvAPKTMwsSc+JTUksSM3OKNTQBdsrI5yMAAAA=" target="_blank">Run the query</a>

```kusto
print d=current_principal_details()
```

**Example output**

|d|
|---|
|{<br>  "UserPrincipalName": "user@fabrikam.com",<br>  "IdentityProvider": "https://sts.windows.net",<br>  "Authority": "72f988bf-86f1-41af-91ab-2d7cd011db47",<br>  "Mfa": "True",<br>  "Type": "AadUser",<br>  "DisplayName": "James Smith (upn: user@fabrikam.com)",<br>  "ObjectId": "346e950e-4a62-42bf-96f5-4cf4eac3f11e",<br>  "FQN": null,<br>  "Notes": null<br>}|
