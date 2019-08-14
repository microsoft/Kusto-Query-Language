# trim_end()

Removes trailing match of the specified regular expression.

**Syntax**

`trim_end(`*regex*`,` *text*`)`

**Arguments**

* *regex*: String or [regular expression](re2.md) to be trimmed from the end of *text*.  
* *text*: A string.

**Returns**

*text* after trimming matches of *regex* found in the end of *text*.

**Example**

Statement bellow trims *substring*  from the end of *string_to_trim*:

<!-- csl -->
```
let string_to_trim = @"bing.com";
let substring = ".com";
print string_to_trim = string_to_trim,trimmed_string = trim_end(substring,string_to_trim)
```

|string_to_trim|trimmed_string|
|--------------|--------------|
|bing.com      |bing          |

Next statement trims all non-word characters from the end of the string:

<!-- csl -->
```
print str = strcat("-  ","Te st",x,@"// $")
| extend trimmed_str = trim_end(@"[^\w]+",str)
```

|str          |trimmed_str|
|-------------|-----------|
|-  Te st1// $|-  Te st1  |
|-  Te st2// $|-  Te st2  |
|-  Te st3// $|-  Te st3  |
|-  Te st4// $|-  Te st4  |
|-  Te st5// $|-  Te st5  |
