---
title:  Kusto Query Language (KQL) overview
description: Learn about how to use Kusto Query Language to explore data.
ms.reviewer: alexans
ms.topic: reference
ms.custom: build-2023, build-2023-dataai
ms.date: 03/16/2023
adobe-target: true
---
# Kusto Query Language (KQL) overview

Kusto Query Language is a powerful tool to explore your data and discover patterns, identify anomalies and outliers, create statistical modeling, and more. The query uses schema entities that are organized in a hierarchy similar to SQLs: databases, tables, and columns.

## What is a Kusto query?

A Kusto query is a read-only request to process data and return results. The request is stated in plain text, using a data-flow model that is easy to read, author, and automate. Kusto queries are made of one or more query statements.

## What is a query statement?

There are three kinds of user [query statements](statements.md):

* A [tabular expression statement](tabularexpressionstatements.md)
* A [let statement](letstatement.md)
* A [set statement](setstatement.md)

All query statements are separated by a `;` (semicolon), and only affect the query at hand.

>[!NOTE]
> For information about application query statements, see [Application query statements](statements.md#application-query-statements).

The most common kind of query statement is a tabular expression **statement**, which means both its input and output consist of tables or tabular datasets. Tabular statements contain zero or more **operators**, each of which starts with a tabular input and returns a tabular output. Operators are sequenced by a `|` (pipe). Data flows, or is piped, from one operator to the next. The data is filtered or manipulated at each step and then fed into the following step.

It's like a funnel, where you start out with an entire data table. Each time the data passes through another operator, it's filtered, rearranged, or summarized. Because the piping of information from one operator to another is sequential, the query operator order is important, and can affect both results and performance. At the end of the funnel, you're left with a refined output.

Let's look at an example query.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSspVuCqUSjPSC1KVQguSSwqCcnMTVVISi0pT03NU9BISSxJLQGKaBgZGJjrGhrqGhhqKujpKaCJG4HENZENKklVsLVVUHLz8Q/ydHFUUgDZkpxfmlcCAIItD6l6AAAA" target="_blank">Run the query</a>

```kusto
StormEvents 
| where StartTime between (datetime(2007-11-01) .. datetime(2007-12-01))
| where State == "FLORIDA"  
| count 
```

|Count|
|-----|
|   28|

> [!NOTE]
> KQL is case-sensitive for everything – table names, table column names, operators, functions, and so on.

This query has a single tabular expression statement. The statement begins with a reference to a table called *StormEvents* and contains several operators, [`where`](whereoperator.md) and [`count`](countoperator.md), each separated by a pipe. The data rows for the source table are filtered by the value of the *StartTime* column and then filtered by the value of the *State* column. In the last line, the query returns a table with a single column and a single row containing the count of the remaining rows.

To try out some more Kusto queries, see [Tutorial: Write Kusto queries](tutorial.md).

## Management commands

In contrast to Kusto queries, [Management commands](../management/index.md) are requests to Kusto to process or modify data or metadata. For example, the following management command creates a new Kusto table with two columns, `Level` and `Text`:

```kusto
.create table Logs (Level:string, Text:string)
```

Management commands have their own syntax, which isn't part of the Kusto Query Language syntax, although the two share many concepts. In particular, management commands are distinguished from queries by having the first character in the text of the command be the dot (`.`) character (which can't start a query).
This distinction prevents many kinds of security attacks, simply because it prevents embedding management commands inside queries.

Not all management commands modify data or metadata. The large class of commands that start with `.show`, are used to display metadata or data. For example, the `.show tables` command returns a list of all tables in the current database.

For more information on management commands, see [Management commands overview](../management/index.md).

## Next steps

* [Tutorial: Learn common operators](tutorials/learn-common-operators.md)
* [Tutorial: Use aggregation functions](tutorials/use-aggregation-functions.md)
* [KQL quick reference](../../kql-quick-reference.md)
* [SQL to Kusto cheat sheet](sqlcheatsheet.md)
* [Query best practices](best-practices.md)
* [Query data with T-SQL](/azure/data-explorer/t-sql)
