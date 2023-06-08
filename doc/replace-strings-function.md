---
title: replace_strings() - Azure Data Explorer
description: Learn how to use the replace_strings() function to replace multiple strings matches with multiple replacement strings.
ms.reviewer: alexans
ms.topic: reference
ms.date: 05/21/2023
---
# replace_strings()

Replaces all strings matches with specified strings.

To replace an individual string, see [replace_string()](replace-string-function.md).

## Syntax

`replace_strings(`*text*`,` *lookups*`,` *rewrites*`)`

## Parameters

|Name|Type|Required|Description|
|--|--|--|--|
|*text*|string|&check;|The source string.|
|*lookups*|dynamic|&check;|The array that includes lookup strings. Array element that isn't a string is ignored.|
|*rewrites*|dynamic|&check;|The array that includes rewrites. Array element that isn't a string is ignored (no replacement made).|

## Returns

Returns *text* after replacing all matches of *lookups* with evaluations of *rewrites*. Matches don't overlap.

## Examples

### Simple replacement

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA22NwQrCMBBE74X+w9BLWij0C3rwrghei0hIlxBqNiFJQcGPd4sFPbinYZj3NibHBSfKWVsamwO8ts6gJGcWGM0oa2JoiQWyDBLnYJu6eoEehXjGeS0meMKIRPGuDd2y0GxzW1fYb/f332Z+svbOtJMSs+qhxKquXY9hwDGEZY3YNf+YbSxMdJYCC7ZRl893T1x+mO4NCQvYwOEAAAA=" target="_blank">Run the query</a>

```kusto
print Message="A magic trick can turn a cat into a dog"
| extend Outcome = replace_strings(
        Message,
        dynamic(['cat', 'dog']), // Lookup strings
        dynamic(['dog', 'pigeon']) // Replacements
        )
```

|Message|Outcome|
|---|---|
|A magic trick can turn a cat into a dog|A magic trick can turn a dog into a pigeon|

### Replacement with an empty string

Replacement with an empty string removes the matching string.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA22OywrCMBBF94X+w6WbtBDoF3ThXhHcisiQDCHUPEhSUPDjTbFgF87qMsw5c2OyvuDEOZPhqTvAkbEKJVk1Q5FHWZIH1VhQL0ONOpiubd7gZ2GvcV6KCo4xIXF8kOJ7rrQ3uW8bbLP55W+jX56cVf1VrA+EhNjpxW2QGEccQ5iXiM33D9Y2U4xMaTVUbKUu3xqOfdkxwwehFI6X6gAAAA==" target="_blank">Run the query</a>

```kusto
print Message="A magic trick can turn a cat into a dog"
| extend Outcome = replace_strings(
        Message,
        dynamic(['turn', ' into a dog']), // Lookup strings
        dynamic(['disappear', '']) // Replacements
        )
```

|Message|Outcome|
|---|---|
|A magic trick can turn a cat into a dog|A magic trick can disappear a cat|

### Replacement order

The order of match elements matters: the earlier match takes the precedence.
Note the difference between Outcome1 and Outcome2: `This` vs `Thwas`.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA8vPTTVSsFUoSi3ISUxOjS8uKcrMSy/W4OVSgALf1OLixPRUHYRISmVeYm5mska0emaxuo6CekgGkI7V1FHQ11fwyc/PLi1QgBqDTU95IrImkJ4giN25qXklSDo0AS8KRWGaAAAA" target="_blank">Run the query</a>

```kusto
 print Message="This is an example of using replace_strings()"
| extend Outcome1 = replace_strings(
        Message,
        dynamic(['This', 'is']), // Lookup strings
        dynamic(['This', 'was']) // Replacements
        ),
        Outcome2 = replace_strings(
        Message,
        dynamic(['is', 'This']), // Lookup strings
        dynamic(['was', 'This']) // Replacements
        )
```

|Message|Outcome1|Outcome2|
|---|---|---|
|This is an example of using replace_strings()|This was an example of using replace_strings()|Thwas was an example of using replace_strings()|

### Nonstring replacement

Replace elements that aren't strings aren't replaced and the original string is kept. The match is still considered being valid, and other possible replacements aren't performed on the matched string. In the following example, 'This' isn't replaced with the numeric `12345`, and it remains in the output unaffected by possible match with 'is'.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA22NywrCMBBF94X+w6WbNlAovpb9A0UQdyIS2jEGmwdNggp+vCkGFHQYZnG5Z44dpfbYkHNcUFvsL9IhLtegO1d2IJgzgpNaYCQ78I5OzkdGuIoVefaMNU+6xzb4zihC+1PLM6RJlvqT9A/NleyqQzmJyxplvEdWo2mwNuYaLNKbP8xsvliuInLjEzMhu7dakfZfAHsBMAz0MeQAAAA=" target="_blank">Run the query</a>

```kusto
 print Message="This is an example of using replace_strings()"
| extend Outcome = replace_strings(
        Message,
        dynamic(['This', 'is']), // Lookup strings
        dynamic([12345, 'was']) // Replacements
        )
```

|Message|Outcome|
|---|---|
|This is an example of using replace_strings()|This was an example of using replace_strings()|

## See also

* For a replacement of a single string, see [replace_string()](replace-string-function.md).
* For a replacement based on regular expression, see [replace_regex()](replace-regex-function.md).
* For replacing a set of characters, see [translate()](translatefunction.md).

