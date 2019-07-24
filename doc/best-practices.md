# Query best practices 

## General

There are several "Dos and Don'ts" you can follow to make you query run faster.

### Do

-	Use time filters first. Kusto is highly optimized to utilize time filters.
-	Prefer `has` keyword over `contains` when looking for full tokens. `has` is more performant as it doesn't have to look-up for substrings.
-	Prefer looking in specific column rather than using `*` (full text search across all columns)
-   If you find that most of your queries deal with extracting fields from [dynamic objects](./scalar-data-types/dynamic.md) across millions of rows, consider
materializing this column at ingestion time. This way - you will pay only once for column extraction.  

### Don't

-   Try new queries without `limit [small number]` or `count` at the end.
    Running unbound queries over unknown data set may yield GBs of results to be returned to the client, resulting in slow response and cluster being busy.
-   If you find that you're applying conversions (JSON, string, etc) over 1 billion records - reshape your query to reduce amount of data fed into the conversion
-   Don't use `tolower(Col) == "lowercasestring"` to do case insensitive comparisons. Kusto has an operator for that. Please use `Col =~ "lowercasestring"` instead.
-   Don't filter on a calculated column, if you can filter on a table column. In other words: Don't do this `T | extend _value = <expression> | where predicate(_value)`, instead do: `T | where predicate(<expression>)`

## summarize operator

-	When the group by keys of the summarize operator are with high cardinality (best practice: above 1 million) then it is recommended to use the [hint.strategy=shuffle](./shufflesummarize.md).

## join operator

-   When using [join operator](./joinoperator.md) - choose the table with less rows to be the first one (left-most). 
-   When using [join operator](./joinoperator.md) data across clusters - run the query on the "right" side of the join (where most of the data is located).
-   When left side is small (up to 100,000 records) and right side is big then it is recommended to use the [hint.strategy=broadcast](./broadcastjoin.md).
-   When both sides of the join are too big and the join key is with high cardinality, then it is recommended to use the [hint.strategy=shuffle](./shufflejoin.md).
    
## parse operator and extract() function

-	[parse operator](./parseoperator.md) (simple mode) is useful when the valued in the target column contain strings which all share the same format or pattern.
For example, for a column whose values look like  `"Time = <time>, ResourceId = <resourceId>, Duration = <duration>, ...."`, when we are interested in extracting the values of each field, using `parse` operator would be better than using several `extract()` statements.
-	[extract() function](./extractfunction.md) is useful when the parsed strings do not all follow the same format or pattern.
In such cases, it is possible to extract the required values by using a regex.

## materialize() function

-	When using the [materialize() function](./materializefunction.md), try to push all possible operators that will reduce the materialized data set and still keeps the semantics of the query. e.g: filters, project only required columns, etc.
    in this example:

    <!--csl-->
    ```
    let materializedData = materialize(Table
    | where Timestamp > ago(1d));
    union (materializedData
    | where Text !has "somestring"
    | summarize dcount(Resource1)), (materializedData
    | where Text !has "somestring"
    | summarize dcount(Resource2))
    ```

-	The filter on Text is mutual and can be pushed to the materialize expression.
    In addition, the query needs only these columns `Timestamp`, `Text`, `Resource1` and `Resource2` columns so it is recommended to project these columns inside the materialized expression.
    
    <!--csl-->
    ```
    let materializedData = materialize(Table
    | where Timestamp > ago(1d)
    | where Text !has "somestring"
    | project Timestamp, Resource1, Resource2, Text);
    union (materializedData
    | summarize dcount(Resource1)), (materializedData
    | summarize dcount(Resource2))
    ```
    
-	if the filters are not identical like in this query:  

    <!--csl-->
    ```
    let materializedData = materialize(Table
    | where Timestamp > ago(1d));
    union (materializedData
    | where Text has "String1"
    | summarize dcount(Resource1)), (materializedData
    | where Text has "String2"
    | summarize dcount(Resource2))
    ```
-	It might be worthy (when the combined filter reduces the materialized result drastically) to combine both filters on the materialized result by a logical `or` expression as in this query below but we should keep the filters in each union leg to preserve the semantics of the query:
     
    <!--csl-->
    ```
    let materializedData = materialize(Table
    | where Timestamp > ago(1d)
    | where Text has "String1" or Text has "String2"
    | project Timestamp, Resource1, Resource2, Text);
    union (materializedData
    | where Text has "String1"
    | summarize dcount(Resource1)), (materializedData
    | where Text has "String2"
    | summarize dcount(Resource2))
    ```