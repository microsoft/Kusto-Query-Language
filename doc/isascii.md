# isascii()

Returns `true` if the argument is a valid ascii string.
    
<!-- csl -->
```
isascii("some string") == true
```

**Syntax**

`isascii(`[*value*]`)`

**Returns**

Indicates whether the argument is a valid ascii string.

**Example**

<!-- csl -->
```
T
| where isascii(fieldName)
| count
```
