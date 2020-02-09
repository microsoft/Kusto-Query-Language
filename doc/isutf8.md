# isutf8()

Returns `true` if the argument is a valid utf8 string.

```
isutf8("some string") == true
```

**Syntax**

`isutf8(`[*value*]`)`

**Returns**

Indicates whether the argument is a valid utf8 string.

**Example**
```
T
| where isutf8(fieldName)
| count
```
