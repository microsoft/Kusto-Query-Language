# reverse()

Function makes reverse of input string.

If input value is not of string type, function forcibly casts the value to string.

**Syntax**

`reverse(`*source*`)`

**Arguments**

* *source*: input value.  

**Returns**

The reverse order of a string value.

**Examples**

<!-- csl -->
```
print str = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
| extend rstr = reverse(str)
```

|str|rstr|
|---|---|
|ABCDEFGHIJKLMNOPQRSTUVWXYZ|ZYXWVUTSRQPONMLKJIHGFEDCBA|


<!-- csl -->
```
print ['int'] = 12345, ['double'] = 123.45, 
['datetime'] = datetime(2017-10-15 12:00), ['timespan'] = 3h
| project rint = reverse(['int']), rdouble = reverse(['double']), 
rdatetime = reverse(['datetime']), rtimespan = reverse(['timespan'])
```

|rint|rdouble|rdatetime|rtimespan|
|---|---|---|---|
|54321|54.321|Z0000000.00:00:21T51-01-7102|00:00:30|




 
