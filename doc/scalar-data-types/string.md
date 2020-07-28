---
title: The string data type - Azure Data Explorer
description: This article describes The string data type in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: rkarlin
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# The string data type

The `string` data type represents a Unicode string. Kusto strings are encoded in UTF-8 and by default are limited to 1MB.

## String literals

There are several ways to encode literals of the `string` data type.

* Enclose the string in double-quotes (`"`): `"This is a string literal. Single quote characters (') don't require escaping. Double quote characters (\") are escaped by a backslash (\\)"`
* Enclose the string in single-quotes (`'`): `'Another string literal. Single quote characters (\') require escaping by a backslash (\\). Double quote characters (") do not require escaping.'`

In the two representations above, the backslash (`\`) character indicates escaping.
It's used to escape the enclosing quote characters, tab characters (`\t`),
newline characters (`\n`), and itself (`\\`).

Verbatim string literals are also supported. In this form, the backslash character (`\`) stands for itself, and not as an escape character.

* Enclose in double-quotes (`"`): `@"This is a verbatim string literal that ends with a backslash\"`
* Enclose in single-quotes (`'`): `@'This is a verbatim string literal that ends with a backslash\'`

Two string literals in the query with nothing between them, or separated
only by whitespace and comments, automatically join to form a new string literal.
For example, the following expressions all yield length `13`.

```kusto
print strlen("Hello"', '@"world!"); // Nothing between them

print strlen("Hello" ', ' @"world!"); // Separated by whitespace only

print strlen("Hello"
  // Comment
  ', '@"world!"); // Separated by whitespace and a comment
```

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
