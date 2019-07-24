# atan2()

Calculates the angle, in radians, between the positive x-axis and the ray from the origin to the point (y, x).

**Syntax**

`atan2(`*y*`,`*x*`)`

**Arguments**

* *x*: X coordinate (a real number).
* *y*: Y coordinate (a real number).

**Returns**

* The angle, in radians, between the positive x-axis and the ray from the origin to the point (y, x).

**Examples**

<!-- csl -->
```
print atan2_0 = atan2(1,1) // Pi / 4 radians (45 degrees)
| extend atan2_1 = atan2(0,-1) // Pi radians (180 degrees)
| extend atan2_2 = atan2(-1,0) // - Pi / 2 radians (-90 degrees)
```

|atan2_0|atan2_1|atan2_2|
|---|---|---|
|0.785398163397448|3.14159265358979|-1.5707963267949|
