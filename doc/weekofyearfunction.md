# week_of_year()

Returns an integer which represents the week number. The week number is calculated from the first week of a year, which is the one that includes the first Thursday, according to [ISO 8601](https://en.wikipedia.org/wiki/ISO_8601#Week_dates).

<!-- csl -->
```
week_of_year(datetime("2015-12-14"))
```

**Syntax**

`week_of_year(`*a_date*`)`

**Arguments**

* `a_date`: A `datetime`.

**Returns**

`week number` - The week number that contains the given date.

**Examples**

|Input                                    |Output|
|-----------------------------------------|------|
|`week_of_year(datetime(2020-12-31))`     |`53`  |
|`week_of_year(datetime(2020-06-15))`     |`25`  |
|`week_of_year(datetime(1970-01-01))`     |`1`   |
|`week_of_year(datetime(2000-01-01))`     |`52`  |

> [!NOTE]
> `weekofyear()` is an obsolete variant of this function. `weekofyear()` was not ISO 8601 compliant; the first week of a year was defined as the week with the year's first Wednesday in it.
The current version of this function, `week_of_year()`, is ISO 8601 compliant; the first week of a year is defined as the week with the year's first Thursday in it.
