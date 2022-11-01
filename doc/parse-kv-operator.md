---
title: parse-kv operator - Azure Data Explorer
description: This article describes the parse-kv operator in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 07/10/2022
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

*T* `|` `parse-kv` *Expression* `as` `(` *KeysList* `)` `with` `(` `regex` `=` *RegexPattern*) `)`

## Arguments

* **Expression**: The string expression to extract key values from.
* **KeysList**: A comma-separated list of key names and their value data types. The order of the keys doesn't have to match the order in which they appear in the text.
* **PairDelimiter**: A string literal representing a delimiter that separates key value pairs from each other.
* **KvDelimiter**: A string literal representing a delimiter that separates keys from values.
* **QuoteChars**: A one- or two-character string literal representing opening and closing quotes that key name or the extracted value may be wrapped with. The parameter can be repeated to specify a separate set of opening/closing quotes.
* **EscapeChar**: A one-character string literal describing a character that may be used for escaping special characters in a quoted value. The parameter can be repeated if multiple escape characters are used.
* **RegexPattern**: A [RE2](re2.md) regular expression containing two capturing groups exactly. The first group represents the key name, the second group represents the key value.

## Returns

The original input tabular expression *T*, extended with columns per specified keys to extract.

> [!NOTE]
> * If a key doesn't appear in a record, the corresponding column value will either be `null` or an empty string, depending on the column type.
> * Only keys that are listed in the operator are extracted.
> * The first appearance of a key is extracted, and subsequent values are ignored.
> * When extracting keys and values, leading and trailing white spaces are ignored.

## Examples

### Extraction with well-defined delimiters

In the following example, keys and values are separated by well defined delimiters. These delimeters are comma and colon characters. :

```kusto
print str="ThreadId:458745723, Machine:Node001, Text: The service is up, Level: Info"
| parse-kv str as (Text: string, ThreadId:long, Machine: string) with (pair_delimiter=',', kv_delimiter=':')
| project-away str
```

|Text|	ThreadId|	Machine|
|--|--|--|
|The service is up| 458745723|	Node001

### Extraction with value quoting

Sometimes key names or values are wrapped in quotes, which allows the values themselves to contain delimiter characters. The following examples show how a `quote` argument is used for extracting such values.

```kusto
print str='src=10.1.1.123 dst=10.1.1.124 bytes=125 failure="connection aborted" "event time"=2021-01-01T10:00:54'
| parse-kv str as (['event time']:datetime, src:string, dst:string, bytes:long, failure:string) with (pair_delimiter=' ', kv_delimiter='=', quote='"')
| project-away str
```

|event time|	src|	dst|	bytes|	failure|
|--|--|--|--|--|
|2021-01-01 10:00:54.0000000|	10.1.1.123|	10.1.1.124|	125|	connection aborted|

The following example uses different opening and closing quotes:

```kusto
print str='src=10.1.1.123 dst=10.1.1.124 bytes=125 failure=(connection aborted) (event time)=(2021-01-01 10:00:54)'
| parse-kv str as (['event time']:datetime, src:string, dst:string, bytes:long, failure:string) with (pair_delimiter=' ', kv_delimiter='=', quote='()')
| project-away str
```

|event time|	src|	dst|	bytes|	failure|
|--|--|--|--|--|
|2021-01-01 10:00:54.0000000|	10.1.1.123|	10.1.1.124|	125|	connection aborted|

The values themselves may contain properly escaped quote characters, as the following example shows:

```kusto
print str='src=10.1.1.123 dst=10.1.1.124 bytes=125 failure="the remote host sent \\"bye!\\"" time=2021-01-01T10:00:54'
| parse-kv str as (['time']:datetime, src:string, dst:string, bytes:long, failure:string) with (pair_delimiter=' ', kv_delimiter='=', quote='"', escape='\\')
| project-away str
```

|time|	src|	dst|	bytes|	failure|
|--|--|--|--|--|
|2021-01-01 10:00:54.0000000|	10.1.1.123|	10.1.1.124|	125|	the remote host sent "bye!"|

### Extraction in greedy mode

There are cases when unquoted values may contain pair delimiters. In this case, use the `greedy` mode to indicate to the operator to scan until the next key appearance (or end of string) when looking for the value ending.

The following examples compare how the operator works with and without the `greedy` mode specified:

```kusto
print str='name=John Doe phone=555 5555 city=New York'
| parse-kv str as (name:string, phone:string, city:string) with (pair_delimiter=' ', kv_delimiter='=')
| project-away str
```

|name|	phone|	city|
|--|--|--|
|John|	555|	New


```kusto
print str='name=John Doe phone=555 5555 city=New York'
| parse-kv str as (name:string, phone:string, city:string) with (pair_delimiter=' ', kv_delimiter='=', greedy=true)
| project-away str
```

|name|	phone|	city|
|--|--|--|
|John Doe|	555 5555|	New York|

### Extraction with no well-defined delimiters

In the following example, any non-alphanumeric character is considered a valid delimiter:

```kusto
print str="2021-01-01T10:00:34 [INFO] ThreadId:458745723, Machine:Node001, Text: Started"
| parse-kv str as (Text: string, ThreadId:long, Machine: string)
| project-away str
```

|Text|	ThreadId|	Machine|
|--|--|--|
|Started|	458745723|	Node001|

Values quoting and escaping is allowed in this mode as shown in the following example:

```kusto
print str="2021-01-01T10:00:34 [INFO] ThreadId:458745723, Machine:Node001, Text: 'The service \\' is up'"
| parse-kv str as (Text: string, ThreadId:long, Machine: string) with (quote="'", escape='\\')
| project-away str
```

|Text|	ThreadId|	Machine|
|--|--|--|
|The service ' is up|	458745723|	Node001|

### Extraction using regex

When no delimiters define text structure well enough, regular expression-based extraction can be useful.

```kusto
print str=@'["referer url: https://hostname.com/redirect?dest=/?h=1234", "request url: https://hostname.com/?h=1234", "advertiser id: 24fefbca-cf27-4d62-a623-249c2ad30c73"]'
| parse-kv str as (['referer url']:string, ['request url']:string, ['advertiser id']: guid) with (regex=@'"([\w ]+)\s*:\s*([^"]*)"')
| project-away str
```

|referer url|	request url|	advertiser id|
|--|--|--|
|`https://hostname.com/redirect?dest=/?h=1234`|	`https://hostname.com/?h=1234`|	24fefbca-cf27-4d62-a623-249c2ad30c73|
