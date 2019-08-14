# strcat()

Concatenates between 1 and 64 arguments.

* In case if arguments are not of string type, they will be forcibly converted to string.

**Syntax**

`strcat(`*argument1*,*argument2* [, *argumentN*]`)`

**Arguments**

* *argument1* ... *argumentN* : expressions to be concatenated.

**Returns**

Arguments, concatenated to a single string.

**Examples**
  
   <!-- csl -->
```
print str = strcat("hello", " ", "world")
```

|str|
|---|
|hello world|
