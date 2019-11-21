# hash_combine()

Combines hash values of two or more hashes.

**Syntax**

`hash_combine(`*h1* `,` *h2* [`,` *h3* ...]`)`

**Arguments**

* *h1*: Long value representing the first hash value.
* *h2*: Long value representing the second hash value.
* *hN*: Long value representing Nth hash value.

**Returns**

The combined hash value of the given scalars.

**Examples**

<!-- csl: https://help.kusto.windows.net:443/Samples -->
```
print value1 = "Hello", value2 = "World"
| extend h1 = hash(value1), h2=hash(value2)
| extend combined = hash_combine(h1, h2)
```

|value1|value2|h1|h2|combined|
|---|---|---|---|---|
|Hello|World|753694413698530628|1846988464401551951|-1440138333540407281|
