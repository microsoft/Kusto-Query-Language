# strlen()

Returns the length, in characters, of the input string.

**Syntax**

`strlen(`*source*`)`

**Arguments**

* *source*: The source string that will be measured for string length.

**Returns**

Returns the length, in characters, of the input string.

**Notes**

Each Unicode character in the string is equal to `1`, including surrogates.
(e.g: Chinese characters will be counted once despite the fact that it requires more than one value in UTF-8 encoding).


**Examples**

<!-- csl -->
```
print length = strlen("hello")
```

|length|
|---|
|5|

<!-- csl -->
```
print length = strlen("â’¦â’°â’®â’¯â’ª")
```

|length|
|---|
|5|
