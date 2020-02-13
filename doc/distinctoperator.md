# distinct operator

Produces a table with the distinct combination of the provided columns of the input table. 

<!-- csl -->
```
T | distinct Column1, Column2
```

Produces a table with the distinct combination of all columns in the input table.

<!-- csl -->
```
T | distinct *
```

**Example**

Shows the distinct combination of fruit and price.

<!-- csl -->
```
Table | distinct fruit, price
```

![alt text](./Images/aggregations/distinct.PNG "distinct")

**Notes**

* Unlike `summarize by ...`, the `distinct` operator supports providing an asterisk (`*`) as the group key, making it easier to use for wide tables.
* If the group by keys are of high cardinalities, using `summarize by ...` with the [shuffle strategy](shufflequery.md) could be useful.
