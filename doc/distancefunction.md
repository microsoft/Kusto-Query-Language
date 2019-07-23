<#ifdef MICROSOFT># distance()

Returns the distance between two points in meters.

**Syntax**

`distance(`*point1*`, `*point2*`)`

**Arguments**

* *point1*: starting point.
* *point2*: ending point.

**Returns**

Returns the great-circle distance between two points in meters.
The function has an accuracy of 0.5%.

**Examples**

The following example returns `9113818.71527951`:

<!-- csl -->
```
print distance(point(32.083770, 34.785037), point(40.730211, -74.000559))
```
<#endif>