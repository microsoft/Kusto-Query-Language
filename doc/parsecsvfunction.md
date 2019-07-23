# parse_csv()

Splits a given string representing a single record of comma separated values and returns a string array with these values.

<!-- csl -->
```
parse_csv("aaa,bbb,ccc") == ["aaa","bbb","ccc"]
```

**Syntax**

`parse_csv(`*source*`)`

**Arguments**

* *source*: The source string representing a single record of comma separated values. Embedded line feeds, commas and quotes 
are supported. If a multiple record CSV text is provided, only the first record is taken

**Returns**

A string array that contains the split values.

**Examples**

<!-- csl: https://help.kusto.windows.net:443/Samples -->
```
print result=parse_csv('aa,"b,b,b",cc,"Escaping quotes: ""Title""","line1\nline2"')
```

|result|
|---|
|[<br>  "aa",<br>  "b,b,b",<br>  "cc",<br>  "Escaping quotes: \"Title\"",<br>  "line1\nline2"<br>]|

CSV payload with multiple records:
<!-- csl: https://help.kusto.windows.net:443/Samples -->
```
print result_multi_record=parse_csv('record1,a,b,c\nrecord2,x,y,z')
```

|result_multi_record|
|---|
|[<br>  "record1",<br>  "a",<br>  "b",<br>  "c"<br>]|
