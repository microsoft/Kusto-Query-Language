# weekofyear()

Returns an integer which represents the week number. The week number is calculated from the first week of a year, which is the one that includes the first Wednesday.
<!-- 
23-Jan-2020: According to Omayer Gharra, the following should be added to this doc when the new function is created (the current WeekOfYear function does not comply with the ISO standard):

`Week_Of_Year` returns the First Week of a year (according to the ISO 8601 standard), which is the one that includes the first Thursday (https://en.wikipedia.org/wiki/ISO_8601#Week_dates)
-->

```
weekofyear(datetime("2015-12-14"))
```

**Syntax**

`weekofyear(`*a_date*`)`

**Arguments**

* `a_date`: A `datetime`.

**Returns**

`week number` - The week number that contains the given date.
