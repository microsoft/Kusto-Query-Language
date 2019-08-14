# base64_encode_tostring()

Encodes a string as base64 string

**Syntax**

`base64_encode_tostring(`*String*`)`

**Arguments**

* *String*: Input string to be encoded as base64 string.

**Returns**

Returns the string encoded as base64 string.

* For decoding base64 strings to a UTF-8 string see [base64_decode_tostring()](base64_decode_tostringfunction.md)
* For decoding base64 strings to an array of long values see [base64_decode_toarray()](base64_decode_toarrayfunction.md)


**Example**

<!-- csl: https://help.kusto.windows.net:443/Samples -->
```
print Quine=base64_encode_tostring("Kusto")
```

|Quine   |
|--------|
|S3VzdG8=|

