# url_encode_component()

The function converts characters of the input URL into a format that can be transmitted over the Internet. 

Detailed information about URL encoding and decoding can be found [here](https://en.wikipedia.org/wiki/Percent-encoding).
Differs from [url_encode](./urlencodefunction.md) by encoding spaces as '20%' and not as '+'.

**Syntax**

`url_encode_component(`*url*`)`

**Arguments**

* *url*: input URL (string).  

**Returns**

URL (string) converted into a format that can be transmitted over the Internet.

**Examples**

<!-- csl -->
```
let url = @'https://www.bing.com/hello word/';
print original = url, encoded = url_encode_component(url)
```

|original|encoded|
|---|---|
|https://www.bing.com/hello word/|https%3a%2f%2fwww.bing.com%2fhello%20word|


 
