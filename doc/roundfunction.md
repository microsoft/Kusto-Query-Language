# round()

Returns the rounded source to the specified precision.

**Syntax**

`round(`*source* [`,` *Precision*]`)`

**Arguments**

* *source*: The source scalar the round is calculated on.
* *Precision*: Number of digits the source will be rounded to.(default value is 0)

**Returns**

The rounded source to the specified precision.

Round is different than [`bin()`](binfunction.md)/[`floor()`](floorfunction.md) in
that the first rounds a number to a specific number of digits while the last rounds value to an integer multiple 
of a given bin size (round(2.15, 1) returns 2.2 while bin(2.15, 1) returns 2).
 

**Examples**

<!-- csl -->
```
round(2.15, 1)                   // 2.2
round(2.15) (which is the same as round(2.15, 0))                   // 2
round(-50.55, -2)                   // -100
round(21.5, -1)                   // 20
```
