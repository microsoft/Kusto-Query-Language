# count() (aggregation function)

Returns a count of the records per summarization group (or in total if summarization is done without grouping).

* Can be used only in context of aggregation inside [summarize](summarizeoperator.md)
* Use the [countif](countif-aggfunction.md) aggregation function
  to count only records for which some predicate returns `true`.

**Syntax**

summarize `count()`

**Returns**

Returns a count of the records per summarization group (or in total if summarization is done without grouping).
