# string_size()

Returns the size, in bytes, of the input string.

**Syntax**

`string_size(`*source*`)`

**Arguments**

* *source*: The source string that will be measured for string size.

**Returns**

Returns the length, in bytes, of the input string.

**Examples**

<!-- csl -->
```
print size = string_size("hello")
```

|size|
|---|
|5|

<!-- csl -->
```
print size = string_size("â’¦â’°â’®â’¯â’ª")
```

|size|
|---|
|15|
