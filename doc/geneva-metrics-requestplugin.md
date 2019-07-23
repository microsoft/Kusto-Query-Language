<#ifdef MICROSOFT># geneva_metrics_request plugin

The `geneva_metrics_request` plugin sends a [KQL-M](https://genevamondocs.azurewebsites.net/metrics/advanced/kql/overview/overview.html) query to a Geneva metrics and returns as a Kusto Datatable which can be used for :
* Joining with kusto data 
* Visualizing in kusto explorer

`evaluate` `geneva_metrics_request` `(` *account_name* `,` *KQL-Mquery* [`,` *start_time* [`,` *end_time* [`,` *options*]]] `)`

**Arguments**

* *account_name*: A `string` literal indicating the account name for geneva metrics.

* *KQL-Mquery*: A `string` literal indicating the query that is to be executed to get metrics data.

* *start_time*: A `datetime` literal indicating the query start time. Optional, defaults to now(-1h)

* *end_time*: A `datetime` literal indicating the query end time. Optional, defaults to now().

* *options*: A constant value of type `dynamic` that holds a property bag specifying additional properties for the request, as explained later. Optional.

**Examples**

Using KQL-M query to fetch Average CPU for all nodes under account 'MetricTeamInternalMetrics' in azglobal-black datacenter for last 1 hour:

<!-- csl -->
```
let mdmData = evaluate geneva_metrics_request(
'MetricTeamInternalMetrics', 
@'metricNamespace("PlatformMetrics").metric("\\Processor(_Total)\\% Processor Time").dimensions("Datacenter", "RoleInstance") | where Datacenter == "azglobal-black" | project Average');
mdmData

```

Using KQL-M query to fetch top 5 nodes with most incoming traffic for account 'MetricTeamInternalMetrics' for last 1 day

<!-- csl -->
```
let startTime = now(-1d);
let mdmData = evaluate geneva_metrics_request(
'MetricTeamInternalMetrics', 
'metricNamespace("Metrics.Server").metric("RequestCount").dimensions("Datacenter", "RoleInstance") | top 5 by Max(Sum) asc | project Sum',
startTime);
mdmData
| project-away  TimestampUtc
| distinct RoleInstance
```

Using KQL-M query to fetch total request count per day under account 'MetricTeamInternalMetrics' in last 2 days

<!-- csl -->
```
let startTime = now(-2d);
let endTime = now(-1d);
let mdmData = evaluate geneva_metrics_request(
'MetricTeamInternalMetrics', 
'metricNamespace("Metrics.Server").metric("RequestCount").dimensions("Datacenter") | zoom TotalSum = Sum(Sum) by 1d',
startTime,
endTime);
mdmData
```

**Query data in Geneve Metrics INT environment**

For accounts in geneva metrics int environment, options parameter can be used to specify :

* *Environment*: A `string` literal geneva metrics environment. Supported values are INT|PROD
        -- INT for Geneva Metrics INT accounts
        -- PROD (default value) for Geneva Metrics prod account

Examples:
Query data for account 'MetricTeamInternalMetrics' in geneva metrics int environment.

<!-- csl -->
```
let startTime = now(-1h);
let endTime = now();
let options = dynamic({'Environment':'INT'});
let mdmData = evaluate geneva_metrics_request(
'MetricTeamInternalMetrics', 
'metricNamespace("Metrics.Server").metric("RequestCount").dimensions("Datacenter", "RoleInstance") | Where Datacenter == "EastUS2" | project Average',
startTime,
endTime,
options);
mdmData
```

**Support**

This plugin is in private preview. For any support questions please send a mail to 
[chgupt@microsoft.com](mailto:chgupt@microsoft.com).

<#endif>