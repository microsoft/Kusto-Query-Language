---
title: azure_digital_twins_query_request plugin - Azure Data Explorer
description: This article describes the Azure Digital Twins query request plugin in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/23/2022
---
# azure_digital_twins_query_request plugin

The `azure_digital_twins_query_request` plugin runs an Azure Digital Twins query as part of a Kusto Query Language query. The plugin is invoked with the [`evaluate`](evaluateoperator.md) operator.

Using the plugin, you can query across data in both Azure Digital Twins and any data source accessible through the Kusto Query Language. For example, you can use the plugin to contextualize time series data in a Kusto query by joining it with knowledge graph data held in Azure Digital Twins.

## Syntax

`evaluate` `azure_digital_twins_query_request` `(` *AdtInstanceEndpoint* `,` *AdtQuery* `)`

## Arguments

* *AdtInstanceEndpoint*: A `string` literal indicating the Azure Digital Twins instance endpoint to be queried.

* *AdtQuery*: A `string` literal indicating the query that is to be run against the Azure Digital Twins endpoint. This query is written in a custom SQL-like query language for Azure Digital Twins, referred to as the **Azure Digital Twins query language**. For more information, see [**Query language for Azure Digital Twins**](/azure/digital-twins/concepts-query-language).

## Authentication and authorization

The azure_digital_twins_query_request plugin uses the Azure AD account of the user running the query to authenticate. To run a query, a user must at least be granted the **Azure Digital Twins Data Reader** role. Information on how to assign this role can be found in [**Security for Azure Digital Twins solutions**](/azure/digital-twins/concepts-security#authorization-azure-roles-for-azure-digital-twins).

## Examples

The following examples show how you can run various Azure Digital Twins queries, including queries that use additional Kusto expressions.

### Retrieval of all twins within an Azure Digital Twins instance

The following example returns all digital twins within an Azure Digital Twins instance.

```kusto
evaluate azure_digital_twins_query_request(
  'https://contoso.api.wcus.digitaltwins.azure.net',
  'SELECT T AS Twins FROM DIGITALTWINS T')
```

**Output**

:::image type="content" source="images/azure-digital-twins-query-request-plugin/adt-twins.png" alt-text="Screenshot of the twins present in the Azure Digital Twins instance.":::

### Projection of twin properties as columns along with additional Kusto expressions

The following example returns the result from the plugin as separate columns, and then performs additional operations using Kusto expressions.

```kusto
evaluate azure_digital_twins_query_request(
  'https://contoso.api.wcus.digitaltwins.azure.net',
  'SELECT T.Temperature, T.Humidity FROM DIGITALTWINS T WHERE IS_PRIMITIVE(T.Temperature) AND IS_PRIMITIVE(T.Humidity)')
| where Temperature > 20
| project TemperatureInC = Temperature, Humidity
```

**Output**

|TemperatureInC|Humidity|
|---|---|
|21|48|
|49|34|
|80|32|

### Joining the plugin results with another data source

The following example shows how to perform complex analysis, such as anomaly detection, through a `join` operation between the plugin results and a table containing historical data in a Kusto table, based on the ID column (`$dtid`).

```kusto
evaluate azure_digital_twins_query_request(
  'https://contoso.api.wcus.digitaltwins.azure.net',
  'SELECT T.$dtId AS tid, T.Temperature FROM DIGITALTWINS T WHERE IS_PRIMITIVE(T.$dtId) AND IS_PRIMITIVE(T.Temperature)')
| project tostring(tid), todouble(Temperature)
| join kind=inner (
    ADT_Data_History
) on $left.tid == $right.twinId
| make-series num=avg(value) on timestamp from min_t to max_t step dt by tid
| extend (anomalies, score , baseline) = 
          series_decompose_anomalies(num, 1.5, -1, 'linefit')
| render anomalychart with(anomalycolumns=anomalies, title= 'Test, anomalies')
```

ADT_Data_History is a table whose schema as follows:

|timestamp|twinId|modelId|name|value|relationshipTarget|relationshipId|
|---|---|---|---|---|---|---|
|2021-02-01 17:24|contosoRoom|dtmi:com:contoso:Room;1|Temperature|24|...|..|

**Output**

:::image type="content" source="images/azure-digital-twins-query-request-plugin/adt-anomaly.png" alt-text="Screenshot of the Anomaly chart of the test expression. Highlighted point is the anomaly.":::
