# current_principal_is_member_of()

::: zone pivot="azuredataexplorer"

Checks group membership or principal identity of the current principal running the query.

```
print current_principal_is_member_of(
    'aaduser=user1@fabrikam.com', 
    'aadgroup=group1@fabrikam.com',
    'aadapp=66ad1332-3a94-4a69-9fa2-17732f093664;72f988bf-86f1-41af-91ab-2d7cd011db47'
    )
```

**Syntax**

`current_principal_is_member_of`(`*list of string literals*`)

**Arguments**

* *list of expressions* - a comma separated list of string literals, where each literal is a principal fully-qualified-name (FQN) string formed as:  
*PrinciplaType*`=`*PrincipalId*`;`*TenantId*

| PrincipalType   | FQN Prefix  |
|-----------------|-------------|
| AAD User        | `aaduser=`  |
| AAD Group       | `aadgroup=` |
| AAD Application | `aadapp=`   |

**Returns**

The function returns:
* `true`: if the current principal running the query was successfully matched for at least one input argument.
* `false`: if the current principal running the query was not member of any `aadgroup=` FQN arguments and was not equal to any of the `aaduser=` or `aadapp=` FQN arguments.
* `(null)`: if the current principal running the query was not member of any `aadgroup=` FQN arguments and was not equal to any of the `aaduser=` or `aadapp=` FQN arguments, and at least one FQN argument was not successfully resolved (was not presed in AAD). 

> [!NOTE]
> Because the function returns a tri-state value (`true`, `false`,  and `null`), it's important to rely only on positive return values to confirm successful membership. In other words, the following expressions are NOT the same:
> 
> * `where current_principal_is_member_of('non-existing-group')`
> * `where current_principal_is_member_of('non-existing-group') != false` 


**Example**

<!-- csl: https://help.kusto.windows.net/Samples -->
```
print result=current_principal_is_member_of(
    'aaduser=user1@fabrikam.com', 
    'aadgroup=group1@fabrikam.com',
    'aadapp=66ad1332-3a94-4a69-9fa2-17732f093664;72f988bf-86f1-41af-91ab-2d7cd011db47'
    )
```

| result |
|--------|
| (null) |

Using dynamic array instead of multple arguments:

<!-- csl: https://help.kusto.windows.net/Samples -->
```
print result=current_principal_is_member_of(
    dynamic([
    'aaduser=user1@fabrikam.com', 
    'aadgroup=group1@fabrikam.com',
    'aadapp=66ad1332-3a94-4a69-9fa2-17732f093664;72f988bf-86f1-41af-91ab-2d7cd011db47'
    ]))
```

| result |
|--------|
| (null) |

::: zone-end

::: zone pivot="azuremonitor"

This isn't supported in Azure Monitor

::: zone-end
