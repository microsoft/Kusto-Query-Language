---
title: parse-kv operator - Azure Data Explorer
description: Learn how to use the parse-kv operator to represent structured information extracted from a string expression in a key/value form.
ms.reviewer: alexans
ms.topic: reference
ms.date: 03/12/2023
---

# parse-kv operator

Extracts structured information from a string expression and represents the information in a key/value form.

The following extraction modes are supported:

* [**Specified delimeter**](#specified-delimeter): Extraction based on specified delimiters that dictate how keys/values and pairs are separated from each other.
* [**Non-specified delimeter**](#non-specified-delimiter): Extraction with no need to specify delimiters. Any non-alphanumeric character is considered a delimiter.
* [**Regex**](#regex): Extraction based on [RE2](re2.md) regular expression.

## Syntax

### Specified delimeter

*T* `|` `parse-kv` *Expression* `as` `(` *KeysList* `)` `with` `(` `pair_delimiter` `=` *PairDelimiter* `,` `kv_delimiter` `=` *KvDelimiter*  [`,` `quote` `=` *QuoteChars* ... [`,` `escape` `=` *EscapeChar* ...]] [`,` `greedy` `=` `true`] `)`

### Non-specified delimiter

*T* `|` `parse-kv` *Expression* `as` `(` *KeysList* `)` `with` `(` [`quote` `=` *QuoteChars* ... [`,` `escape` `=` *EscapeChar* ...]] `)`

### Regex

*T* `|` `parse-kv` *Expression* `as` `(` *KeysList* `)` `with` `(` `regex` `=` *RegexPattern*`)` `)`

## Parameters

|Name|Type|Required|Description|
|--|--|--|--|
|*Expression*|string|&check;|The expression from which to extract key values.|
|*KeysList*|string|&check;|A comma-separated list of key names and their value data types. The order of the keys doesn't have to match the order in which they appear in the text.|
|*PairDelimiter*|string||A delimiter that separates key value pairs from each other.|
|*KvDelimiter*|string||A delimiter that separates keys from values.|
|*QuoteChars*|string||A one- or two-character string literal representing opening and closing quotes that key name or the extracted value may be wrapped with. The parameter can be repeated to specify a separate set of opening/closing quotes.|
|*EscapeChar*|string||A one-character string literal describing a character that may be used for escaping special characters in a quoted value. The parameter can be repeated if multiple escape characters are used.|
|*RegexPattern*|string||A [RE2](re2.md) regular expression containing two capturing groups exactly. The first group represents the key name, and the second group represents the key value.|

## Returns

The original input tabular expression *T*, extended with columns per specified keys to extract.

> [!NOTE]
>
> * If a key doesn't appear in a record, the corresponding column value will either be `null` or an empty string, depending on the column type.
> * Only keys that are listed in the operator are extracted.
> * The first appearance of a key is extracted, and subsequent values are ignored.
> * When extracting keys and values, leading and trailing white spaces are ignored.

## Examples

### Extraction with well-defined delimiters

In the following example, keys and values are separated by well defined delimiters. These delimeters are comma and colon characters.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA02N0QqCMBiF73uKH29UmGClFAMfQKiuvI/h/tqfNmVbs6CHz1FCl+cczveNhrQD60wVNcqgkLXkRbnfFeVus2VwFK0ijfw0SMzzNYMGn45DoxAsGk8tAll4jAwO6LHnUOvLEK3eMApjMet8QIOwkHyPcyJ9nTGLqx9CXDS/PYWJnIJkFGTOEnu6k0NTxSxm0Pn/hsdpkJnhhq3LxCReAfEBrq2ju9UAAAA=" target="_blank">Run the query</a>

```kusto
print str="ThreadId:458745723, Machine:Node001, Text: The service is up, Level: Info"
| parse-kv str as (Text: string, ThreadId:long, Machine: string) with (pair_delimiter=',', kv_delimiter=':')
| project-away str
```

**Output**

|Text|	ThreadId|	Machine|
|--|--|--|
|The service is up| 458745723|	Node001

### Extraction with value quoting

Sometimes key names or values are wrapped in quotes, which allows the values themselves to contain delimiter characters. The following examples show how a `quote` argument is used for extracting such values.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA02Py6oCMQyG9z5F6KYKM9KOuin0LdwdROpM1OjYzmnjiODD24qoJIs/9y9DJM+QOFqZYmu1mutizQK6xN9wCbs7Y7K6WcHeUX+NaEUbvMeWKXhwuxAZOwECR8wLmS4obKMaXavia62MUma1lJMHDC4mrM9jOQsuwfRPfqfkxnSOscgKMpLJTeQPVeH56BeM6UPRb5x3bQY34iNMB0dx22FPF2LMz4Gs4Dz+ZmzO/F8Do5VCzgpWDKf8Tu1u7l7QnivUg0kbAQAA" target="_blank">Run the query</a>

```kusto
print str='src=10.1.1.123 dst=10.1.1.124 bytes=125 failure="connection aborted" "event time"=2021-01-01T10:00:54'
| parse-kv str as (['event time']:datetime, src:string, dst:string, bytes:long, failure:string) with (pair_delimiter=' ', kv_delimiter='=', quote='"')
| project-away str
```

**Output**

|event time|	src|	dst|	bytes|	failure|
|--|--|--|--|--|
|2021-01-01 10:00:54.0000000|	10.1.1.123|	10.1.1.124|	125|	connection aborted|

The following example uses different opening and closing quotes:

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA02PzU7DMAzH7zyFb0mkFiVlu0TKk0zTlLUGzLqkJF6nSTw8MQIV2Qf//fnzUigxVC5B1TIGZ5+d2PACU+VN7uD8YKzBDXt4jTTfCgY95pRwZMoJ4jkXxsmAxhXbQqYrmqAHO7jeioOz3lq/3xn19AVLLBX7yyqHIVbQB7XNqaOHKTJK3EGj8tJG6a0Tpk38EHmYs4hfqL+qgTvxO+glUjlNONOVGNuPoDq4rP8zoWU+b5kxKG2UEbiSP9pbfbzHh6z7BsDEnzAjAQAA" target="_blank">Run the query</a>

```kusto
print str='src=10.1.1.123 dst=10.1.1.124 bytes=125 failure=(connection aborted) (event time)=(2021-01-01 10:00:54)'
| parse-kv str as (['event time']:datetime, src:string, dst:string, bytes:long, failure:string) with (pair_delimiter=' ', kv_delimiter='=', quote='()')
| project-away str
```

**Output**

|event time|	src|	dst|	bytes|	failure|
|--|--|--|--|--|
|2021-01-01 10:00:54.0000000|	10.1.1.123|	10.1.1.124|	125|	connection aborted|

The values themselves may contain properly escaped quote characters, as the following example shows:

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA02PzW7CMAyA73sKr5eA1KKk0EukvAW3FU2h9WigpV1iQJX28NgIwZRI+fwj+8sUw5kgUXQqxcYZvTJyyjW0id7hBvYzYXKmrODHh/4S0WXUIUQcRkLoxsRTkEfVdbaf8ZOfDCgM6EpdmkLL3RpttbbVRn38weRjwuJ0ld3gEyy+lLSrnW09oWAObGS5HM6HXHRe/HCx/Sj8tHnWlnAL1MFi8iF+t9iHIRDy30DlcLr+zzjO/F7Y3amMEVPjJ+a6Vkuxi+MRGyr8zc9ieAfsfwfSJwEAAA==" target="_blank">Run the query</a>

```kusto
print str='src=10.1.1.123 dst=10.1.1.124 bytes=125 failure="the remote host sent \\"bye!\\"" time=2021-01-01T10:00:54'
| parse-kv str as (['time']:datetime, src:string, dst:string, bytes:long, failure:string) with (pair_delimiter=' ', kv_delimiter='=', quote='"', escape='\\')
| project-away str
```

**Output**

|time|	src|	dst|	bytes|	failure|
|--|--|--|--|--|
|2021-01-01 10:00:54.0000000|	10.1.1.123|	10.1.1.124|	125|	the remote host sent "bye!"|

### Extraction in greedy mode

There are cases when unquoted values may contain pair delimiters. In this case, use the `greedy` mode to indicate to the operator to scan until the next key appearance (or end of string) when looking for the value ending.

The following examples compare how the operator works with and without the `greedy` mode specified:

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA02NMQvCMBBGd3/FbWmhHbMI2Zwc3J3kqIc5Y5NwORoK/fE2KOLywfvg8bJwVCgqzkScyZ2Tj3BKBNmnSM5aC7bNxLq6C1W4JgnmsEFGKTSGpbmABbqmH3fg+Bg+9o+a/IUeKquHLiPL7U4vnllpj4MZICz/jzN9y0h60qQjVlxb6g1YjlF0sAAAAA==" target="_blank">Run the query</a>

```kusto
print str='name=John Doe phone=555 5555 city=New York'
| parse-kv str as (name:string, phone:string, city:string) with (pair_delimiter=' ', kv_delimiter='=')
| project-away str
```

**Output**

|name|	phone|	city|
|--|--|--|
|John|	555|	New

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA02NMQvCMBBGd3/FbWmhHbMI2Zwc3J0ktEdzxibhejYE/PEmKOLywfvg8RJTENiEjQp2RXOOLsApIiQXAxqtNeg2E0kxF8xwjezV4QXJ8oaj35sLdoOu6ccKFJbhY/+oyV/oIZM46JIlvs34oJUEaxzUAH7/f0x9FkacixF+Yt+aHO84yWizLa37BiIpW9a9AAAA" target="_blank">Run the query</a>

```kusto
print str='name=John Doe phone=555 5555 city=New York'
| parse-kv str as (name:string, phone:string, city:string) with (pair_delimiter=' ', kv_delimiter='=', greedy=true)
| project-away str
```

**Output**

|name|	phone|	city|
|--|--|--|
|John Doe|	555 5555|	New York|

### Extraction with no well-defined delimiters

In the following example, any non-alphanumeric character is considered a valid delimiter:

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUSguKbJVMjIwMtQ1AKEQQwMrAwMrYxOFaE8/N/9YhZCMotTEFM8UKxNTC3MTU3MjYx0F38TkjMy8VCu//JRUAwNDHYWQ1IoSK4XgksSiktQUJa4ahYLEouJU3ewykPEKicUKGhAVQF5mXroOwtCcfBAXZh5UXhNkQFF+VmpyiW5ieWIlSBgATRrnIq0AAAA=" target="_blank">Run the query</a>

```kusto
print str="2021-01-01T10:00:34 [INFO] ThreadId:458745723, Machine:Node001, Text: Started"
| parse-kv str as (Text: string, ThreadId:long, Machine: string)
| project-away str
```

**Output**

|Text|	ThreadId|	Machine|
|--|--|--|
|Started|	458745723|	Node001|

Values quoting and escaping is allowed in this mode as shown in the following example:

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA0WNTQuCQBRF9/2Kx2xGYYTxC2PAbeAi27jLFoM+cirUZkYt6MfnUBHczeVy7hm16i0Yq3MS8SgMuEsVcsG5iBM4FuXucIKq0yjbohVJus2SNItiBnvZdKpHUQ4tch4yqPBhBdCqQzCoZ9Ug1DUFZWAaKdm8YJTaYHCdnQ6kAe9DrE31Z/aX3AZXf//f3YdF2Q68+zRYzAklDNA0csScrhbf3evhgo0N5CKfDnoDfs645tsAAAA=" target="_blank">Run the query</a>

```kusto
print str="2021-01-01T10:00:34 [INFO] ThreadId:458745723, Machine:Node001, Text: 'The service \\' is up'"
| parse-kv str as (Text: string, ThreadId:long, Machine: string) with (quote="'", escape='\\')
| project-away str
```

**Output**

|Text|	ThreadId|	Machine|
|--|--|--|
|The service ' is up|	458745723|	Node001|

### Extraction using regex

When no delimiters define text structure well enough, regular expression-based extraction can be useful.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA32QQW6DMBBF9z3FyBtDEofWoES1hNJ7ECq59gBOGiBjE1qph6/ZVMmmi9m8ma95+iO5PoAPVL7xihE2SEgw0aeCLoTRqyzrBh96fcGtGS4ZoXWEJhws+lBmh658kXnBNhCz1ymyf7J3x9rekILz8ZezCmTRYPNhtDCN3IvC7qTQO5kLWbwaqW3+bPY5q/nTD4yaPIrzbVEG7SGp+J00r1Xkrm83sPA/oQf+8DtuoJ2cTWF2oYOEsMWv2AVLquMM9To9+pWKk1TvrF6ljKeLBA2n2IHQs/5eRH4Blp2dfEUBAAA=" target="_blank">Run the query</a>

```kusto
print str=@'["referer url: https://hostname.com/redirect?dest=/?h=1234", "request url: https://hostname.com/?h=1234", "advertiser id: 24fefbca-cf27-4d62-a623-249c2ad30c73"]'
| parse-kv str as (['referer url']:string, ['request url']:string, ['advertiser id']: guid) with (regex=@'"([\w ]+)\s*:\s*([^"]*)"')
| project-away str
```

**Output**

|referer url|	request url|	advertiser id|
|--|--|--|
|`https://hostname.com/redirect?dest=/?h=1234`|	`https://hostname.com/?h=1234`|	24fefbca-cf27-4d62-a623-249c2ad30c73|
