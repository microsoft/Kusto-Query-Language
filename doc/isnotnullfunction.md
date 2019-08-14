# isnotnull()

Returns `true` if the argument is not null.

**Syntax**

`isnotnull(`[*value*]`)`

`notnull(`[*value*]`)` - alias for `isnotnull`

**Example**

<!-- csl -->
```
T | where isnotnull(PossiblyNull) | count
```

Notice that there are other ways of achieving this effect:

<!-- csl -->
```
T | summarize count(PossiblyNull)
```
