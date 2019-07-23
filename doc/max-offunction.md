# max_of()

Returns the maximum value of several evaluated numeric expressions.

<!-- csl -->
```
max_of(10, 1, -3, 17) == 17
```

**Syntax**

`max_of` `(`*expr_1*`,` *expr_2* ...`)`

**Arguments**

* *expr_i*: A scalar expression, to be evaluated.

- All arguments must be of the same type.
- Maximum of 64 arguments is supported.

**Returns**

The maximum value of all argument expressions.

**Example**

<!-- csl: https://help.kusto.windows.net/Samples  -->
```
print result = max_of(10, 1, -3, 17) 
```

|result|
|---|
|17|
