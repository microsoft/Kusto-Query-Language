---
title:  indexof() 
description: Learn how to use the indexof() function to report the zero-based index position of the input string.
ms.reviewer: alexans
ms.topic: reference
ms.date: 12/28/2022
---
# indexof()

Reports the zero-based index of the first occurrence of a specified string within the input string.

For more information, see [`indexof_regex()`](indexofregexfunction.md).

## Syntax

`indexof(`*string*`,`*match*`[,`*start*`[,`*length*`[,`*occurrence*`]]])`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
|*string*| string | &check; | The source string to search.|  
|*match*| string | &check; | The string for which to search.|
|*start*| int | | The search start position. A negative value will offset the starting search position from the end of the *string* by this many steps: `abs(`*start*`)`. |
|*length*| int | | The number of character positions to examine. A value of -1 means unlimited length.|
|*occurrence*| int | | The number of the occurrence. The default is 1.|

> [!NOTE]
> If *string* or *match* isn't of type `string`, the function forcibly casts their value to `string`.

## Returns

The zero-based index position of *match*.

* Returns -1 if *match* isn't found in *string*.
* Returns `null` if:
  * *start* is less than 0.
  * *occurrence* is less than 0.
  * *length* is less than -1.

## Examples

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA42STW6DMBCF9znFyJsWyVEKhCRd9DDGjIlVYqOxadLbd4LdqpUoioXFxu97b35Gsi5uwHa3Et7Aug5v3jwL1eoOTS+k4L8ogM9uB4P379MIxk+u47f8jVOEEJnRb0DeKdX/FFnKfbGEIQyoSJ+xA1KuR8isepVVzayk5AyKYgBD/pIkUEpo53B+DKBMRIIK9FlRkBA8mGkYvoNo5Z4itJgSZfP9inmdC/lrnky81hMROo3gTXbIyGYFuW2KucUOexXtByZoep7lh1/ysqr3zeEom9TUdFger1yapRBhVKQuyIkCXJHutZG27fDJ1YbInY4+Dy6AmGkCFI9DNCLbHVfSVnJbFo+uxGkB9MODBLyPq3yRUBVrmzZvR6a+Pkjd8q0zNZ6Rr6VucUpgAzgflzbyC/vZ0L4mAwAA" target="_blank">Run the query</a>

```kusto
print
 idx1 = indexof("abcdefg","cde")    // lookup found in input string
 , idx2 = indexof("abcdefg","cde",1,4) // lookup found in researched range 
 , idx3 = indexof("abcdefg","cde",1,2) // search starts from index 1, but stops after 2 chars, so full lookup can't be found
 , idx4 = indexof("abcdefg","cde",3,4) // search starts after occurrence of lookup
 , idx5 = indexof("abcdefg","cde",-5)  // negative start index
 , idx6 = indexof(1234567,5,1,4)       // two first parameters were forcibly casted to strings "12345" and "5"
 , idx7 = indexof("abcdefg","cde",2,-1)  // lookup found in input string
 , idx8 = indexof("abcdefgabcdefg", "cde", 1, 10, 2)   // lookup found in input range
 , idx9 = indexof("abcdefgabcdefg", "cde", 1, -1, 3)   // the third occurrence of lookup is not in researched range
```

**Output**

|idx1|idx2|idx3|idx4|idx5|idx6|idx7|idx8|idx9|
|----|----|----|----|----|----|----|----|----|
|2   |2   |-1  |-1  | 2  |4   |2   |9   |-1  |
