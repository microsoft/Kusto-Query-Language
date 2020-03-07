# hash_many()

Returns a combined hash value of multiple values.

**Syntax**

`hash_many(`*s1* `,` *s2* [`,` *s3* ...]`)`

**Arguments**

* *s1*, *s2*, ..., *sN*: input values that will be hashed together.

**Returns**

The combined hash value of the given scalars.

**Examples**

<!-- csl: https://help.kusto.windows.net/Samples -->
```
print value1 = "Hello", value2 = "World"
| extend combined = hash_many(value1, value2)
```

|value1|value2|combined|
|---|---|---|
|Hello|World|-1440138333540407281|
