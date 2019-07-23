# min_of()

Returns the minimum value of several evaluated numeric expressions.

<!-- csl -->
```
min_of(10, 1, -3, 17) == -3
```

**Syntax**

`min_of` `(`*expr_1*`,` *expr_2* ...`)`

**Arguments**

* *expr_i*: A scalar expression, to be evaluated.

- All arguments must be of the same type.
- Maximum of 64 arguments is supported.

**Returns**

The minimum value of all argument expressions.

**Example**

<!-- csl: https://help.kusto.windows.net/Samples  -->
```
print result=min_of(10, 1, -3, 17) 
```

|result|
|---|
|-3|
