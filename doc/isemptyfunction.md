# isempty()

Returns `true` if the argument is an empty string or is null.
    
<!-- csl -->
```
isempty("") == true
```

**Syntax**

`isempty(`[*value*]`)`

**Returns**

Indicates whether the argument is an empty string or isnull.

|x|isempty(x)
|---|---
| "" | true
|"x" | false
|parsejson("")|true
|parsejson("[]")|false
|parsejson("{}")|false

**Example**

<!-- csl -->
```
T
| where isempty(fieldName)
| count
```
