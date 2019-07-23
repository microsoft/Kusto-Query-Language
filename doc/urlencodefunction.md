# url_encode()

The function converts characters of the input URL into a format that can be transmitted over the Internet. 

Detailed information about URL encoding and decoding can be found [here](https://en.wikipedia.org/wiki/Percent-encoding).

**Syntax**

`url_encode(`*url*`)`

**Arguments**

* *url*: input URL (string).  

**Returns**

URL (string) converted into a format that can be transmitted over the Internet.

**Examples**

<!-- csl -->
```
let url = @'https://www.bing.com/';
print original = url, encoded = url_encode(url)
```

|original|encoded|
|---|---|
|https://www.bing.com/|https%3a%2f%2fwww.bing.com%2f|


 
