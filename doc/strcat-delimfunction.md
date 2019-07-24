# strcat_delim()

Concatenates between 2 and 64 arguments, with delimiter, provided as first argument.

 * In case if arguments are not of string type, they will be forcibly converted to string.

**Syntax**

`strcat_delim(`*delimiter*,*argument1*,*argument2* [, *argumentN*]`)`

**Arguments**

* *delimiter*: string expression, which will be used as separator.
* *argument1* ... *argumentN* : expressions to be concatenated.

**Returns**

Arguments, concatenated to a single string with *delimiter*.

**Examples**

<!-- csl -->
```
print st = strcat_delim('-', 1, '2', 'A', 1s)

```

|st|
|---|
|1-2-A-00:00:01|
