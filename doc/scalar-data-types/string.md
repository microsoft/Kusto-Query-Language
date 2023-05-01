---
title: The string data type - Azure Data Explorer
description: This article describes The string data type in Azure Data Explorer.
ms.reviewer: orspodek
ms.topic: reference
ms.date: 06/08/2022
---
# The string data type

The `string` data type represents a sequence of zero or more [Unicode](https://home.unicode.org/)
characters.

> [!NOTE]
>
> * Internally, strings are encoded in [UTF-8](https://en.wikipedia.org/wiki/UTF-8). Invalid (non-UTF8) characters are replaced with [U+FFFD](https://codepoints.net/U+FFFD) Unicode replacement characters at ingestion time.
> * Kusto has no data type that is equivalent to a single character. A single character is represented as a string of length 1.
> * When ingesting the `string` data type, if a single string value in a record exceeds 1MB (measured using UTF-8 encoding), the value is truncated and ingestion succeeds. If a single string value in a record, or the entire record, exceeds the allowed data limit of 64MB, ingestion fails.

## String literals

There are several ways to encode literals of the `string` data type in a query text:

* Enclose the string in double-quotes (`"`): `"This is a string literal. Single quote characters (') don't require escaping. Double quote characters (") are escaped by a backslash (\)."`
* Enclose the string in single-quotes (`'`): `'Another string literal. Single quote characters (') require escaping by a backslash (\). Double quote characters (") do not require escaping.'`

In the two representations above, the backslash (`\`) character indicates escaping.
The backslash is used to escape the enclosing quote characters, tab characters (`\t`),
newline characters (`\n`), and itself (`\\`).

> [!NOTE]
> The newline character (`\n`) and the return character (`\r`) can't be included
> as part of the string literal without being quoted. See also [multi-line string literals](#multi-line-string-literals).

## Verbatim string literals

Verbatim string literals are also supported. In this form, the backslash character (`\`) stands for itself, and not as an escape character. Prepending the `@` special character to string literals serves as a verbatim identifier.

* Enclose in double-quotes (`"`): `@"This is a verbatim string literal that ends with a backslash\. Double quote characters (") are escaped by a double quote (")."`
* Enclose in single-quotes (`'`): `@'This is a verbatim string literal that ends with a backslash\. Single quote characters (') are escaped by a single quote (').'`

> [!NOTE]
> The newline character (`\n`) and the return character (`\r`) can't be included
> as part of the string literal without being quoted. See also [multi-line string literals](#multi-line-string-literals).

## Splicing string literals

Two or more string literals are automatically joined to form a new string literal in the query if they have nothing between them, or they're separated only by whitespace and comments. <br>
For example, the following expressions all yield a string of length 13:

```kusto
print strlen("Hello"', '@"world!"); // Nothing between them

print strlen("Hello" ', ' @"world!"); // Separated by whitespace only

print strlen("Hello"
  // Comment
  ', '@"world!"); // Separated by whitespace and a comment
```

## Multi-line string literals

Multi-line string literals are string literals for which the newline (`\n`) and return (`\r`)
characters don't require escaping.

* Multi-line string literals always appear between two occurrences of the "triple-backtick chord" (`\``).

> [!NOTE]
> * Multi-line string literals do not support escaped characters. Similar to 
> [verbatim string literals](#verbatim-string-literals), multi-line string literals allow newline and return characters.
> * Multi-line string literals don't support obfuscation.

## Examples

```kusto
// Simple string notation
print s1 = 'some string', s2 = "some other string"

// Strings that include single or double-quotes can be defined as follows
print s1 = 'string with " (double quotes)',
          s2 = "string with ' (single quotes)"

// Strings with '\' can be prefixed with '@' (as in c#)
print myPath1 = @'C:\Folder\filename.txt'

// Escaping using '\' notation
print s = '\\n.*(>|\'|=|\")[a-zA-Z0-9/+]{86}=='

// Encode a C# program in a Kusto multi-line string
print program=```
  public class Program {
    public static void Main() {
      System.Console.WriteLine("Hello!");
    }
  }```
```

As can be seen, when a string is enclosed in double-quotes (`"`), the single-quote (`'`) character doesn't require escaping, and also the other way around. This method makes it easier to quote strings according to context.

## Obfuscated string literals

The system tracks queries and stores them for telemetry and analysis purposes.
For example, the query text might be made available to the cluster owner. If the
query text includes secret information, such as passwords, it might leak
information that should be kept private. To prevent such a leak from happening, the
query author may mark specific string literals as **obfuscated string literals**.
Such literals in the query text are automatically replaced by a number of
star (`*`) characters, so that they aren't available for later analysis.

> [!IMPORTANT]
> Mark all string literals that contain secret information, as obfuscated string literals.

An obfuscated string literal can be formed by taking a "regular" string literal,
and prepending an `h` or an `H` character in front of it. 

For example:

```kusto
h'hello'
h@'world'
h"hello"
```

> [!NOTE]
> In many cases, only a part of the string literal is secret. 
> In those cases, split the literal into a non-secret part and a secret
> part. Then, only mark the secret part as obfuscated.

For example:

```kusto
print x="https://contoso.blob.core.windows.net/container/blob.txt?"
  h'sv=2012-02-12&se=2013-04-13T0...'
```
