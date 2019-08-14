# print operator

Outputs single-row with one or more scalar expressions.

<!-- csl: https://help.kusto.windows.net:443/Samples -->
```
print x=1, s=strcat("Hello", ", ", "World!")
```

**Syntax**

`print` [*ColumnName* `=`] *ScalarExpression* [',' ...]

**Arguments**

* *ColumnName*: An option name to assign to the output's singular column.
* *ScalarExpression*: A scalar expression to evaluate.

**Returns**

A single-column, single-row, table whose single cell has the value of the evaluated *ScalarExpression*.

**Examples**

The `print` operator is useful as a quick way to evaluate one or more
scalar expressions and make a single-row table out of the resulting values.
For example:

<!-- csl: https://help.kusto.windows.net:443/Samples -->
```
print 0 + 1 + 2 + 3 + 4 + 5, x = "Wow!"
```

<!-- csl: https://help.kusto.windows.net:443/Samples -->
```
print banner=strcat("Hello", ", ", "World!")
```
