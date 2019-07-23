<#ifdef MICROSOFT># point()

Returns a dynamic array representation of a point.

**Syntax**

`point(`*latitude*`,`*longitude*`)`

**Arguments**

* *latitude*: Latitude value between -90 and 90.
* *longitude*: Longitude value between -180 and 180.

**Returns**

Returns a dynamic array containing the latitude and longitude values.
If the latitude value is outside [-90, 90] the function returns `null`.
If the longitude value is outside (-180, 180] the value will wrap around.

**Examples**

The following example returns `[1.0, 2.0]`:

<!-- csl -->
```
print point(1, 2)
```

The following example returns `[0.0, -90.0]`:

<!-- csl -->
```
print point(0, 270)
```

The following example returns `null`:

<!-- csl -->
```
print point(91, 0)
```
<#endif>