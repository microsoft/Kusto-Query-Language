# bitset_count_ones()

Returns the number of set bits in the binary representation of a number.

<!-- csl -->
```
bitset_count_ones(42)
```

**Syntax**

`bitset_count_ones(`*num1*``)`

**Arguments**

* *num1*: long or integer number.

**Returns**

Returns the number of set bits in the binary representation of a number.

**Example**

<!-- csl: https://help.kusto.windows.net/Samples -->
```
// 42 = 32+8+2 : b'00101010' == 3 bits set
print ones = bitset_count_ones(42) 
```

|ones|
|---|
|3|
