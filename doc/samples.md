---
title: Samples for queries in Azure Data Explorer and Azure Monitor
description: This article describes common queries and examples that use the Kusto Query Language for Azure Data Explorer and Azure Monitor.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 10/08/2020
ms.localizationpriority: high 
zone_pivot_group_filename: data-explorer/zone-pivot-groups.json
zone_pivot_groups: kql-flavors
---

# Samples for queries for Azure Data Explorer and Azure Monitor

::: zone pivot="azuredataexplorer"

This article identifies common query needs in Azure Data Explorer and how you can use the Kusto Query Language to meet them.

## Display a column chart

To project two or more columns, and then use the columns as the x-axis and y-axis of a chart:

<!-- csl: https://help.kusto.windows.net/Samples  -->
```kusto 
StormEvents
| where isnotempty(EndLocation) 
| summarize event_count=count() by EndLocation
| top 10 by event_count
| render columnchart
```

* The first column forms the x-axis. It can be numeric, date-time, or string. 
* Use `where`, `summarize`, and `top` to limit the volume of data you display.
* Sort the results to define the order of the x-axis.

:::image type="content" source="images/samples/color-bar-chart.png" alt-text="Screenshot of a column chart, with ten colored columns that depict the respective values of 10 locations.":::

## Get sessions from start and stop events

In a log of events, some events mark the start or end of an extended activity or session. 

|Name|City|SessionId|Timestamp|
|---|---|---|---|
|Start|London|2817330|2015-12-09T10:12:02.32|
|Game|London|2817330|2015-12-09T10:12:52.45|
|Start|Manchester|4267667|2015-12-09T10:14:02.23|
|Stop|London|2817330|2015-12-09T10:23:43.18|
|Cancel|Manchester|4267667|2015-12-09T10:27:26.29|
|Stop|Manchester|4267667|2015-12-09T10:28:31.72|

Every event has a session ID (`SessionId`). The challenge is to match start and stop events with a session ID.

Example:

```kusto
let Events = MyLogTable | where ... ;

Events
| where Name == "Start"
| project Name, City, SessionId, StartTime=timestamp
| join (Events 
        | where Name="Stop"
        | project StopTime=timestamp, SessionId) 
    on SessionId
| project City, SessionId, StartTime, StopTime, Duration = StopTime - StartTime
```

To match start and stop events with a session ID:

1. Use [let](./letstatement.md) to name a projection of the table that's pared down as far as possible before starting the join.
1. Use [project](./projectoperator.md) to change the names of the timestamps so that both the start time and the stop time appear in the results. `project` also selects the other columns to view in the results. 
1. Use [join](./joinoperator.md) to match the start and stop entries for the same activity. A row is created for each activity. 
1. Use `project` again to add a column to show the duration of the activity.

Here's the output:

|City|SessionId|StartTime|StopTime|Duration|
|---|---|---|---|---|
|London|2817330|2015-12-09T10:12:02.32|2015-12-09T10:23:43.18|00:11:40.46|
|Manchester|4267667|2015-12-09T10:14:02.23|2015-12-09T10:28:31.72|00:14:29.49|

## Get sessions without using a session ID

Suppose that the start and stop events don't conveniently have a session ID that we can match with. But, we do have the IP address of the client in which the session took place. Assuming each client address conducts only one session at a time, we can match each start event to the next stop event from the same IP address:

Example:

```kusto
Events 
| where Name == "Start" 
| project City, ClientIp, StartTime = timestamp
| join  kind=inner
    (Events
    | where Name == "Stop" 
    | project StopTime = timestamp, ClientIp)
    on ClientIp
| extend duration = StopTime - StartTime 
    // Remove matches with earlier stops:
| where  duration > 0  
    // Pick out the earliest stop for each start and client:
| summarize arg_min(duration, *) by bin(StartTime,1s), ClientIp
```

The `join` matches every start time with all the stop times from the same client IP address. The sample code:

- Removes matches with earlier stop times.
- Groups by start time and IP address to get a group for each session. 
- Supplies a `bin` function for the `StartTime` parameter. If you don't do this step, Kusto automatically uses one-hour bins that match some start times with the wrong stop times.

`arg_min` finds the row with the smallest duration in each group, and the `*` parameter passes through all the other columns. 

The argument prefixes `min_` to each column name. 

:::image type="content" source="images/samples/start-stop-ip-address-table.png" alt-text="Screenshot of a table that lists the results, with columns for the start time, client IP, duration, city, and earliest stop for each client/start time combination."::: 

Add code to count the durations in conveniently sized bins. In this example, because of a preference for a bar chart, divide by `1s` to convert the timespans to numbers:

```
    // Count the frequency of each duration:
    | summarize count() by duration=bin(min_duration/1s, 10) 
      // Cut off the long tail:
    | where duration < 300
      // Display in a bar chart:
    | sort by duration asc | render barchart 
```

:::image type="content" source="images/samples/number-of-sessions-bar-chart.png" alt-text="Screenshot of a column chart that depicts the number of sessions, with durations in specified ranges.":::

### Full example

```kusto
Logs  
| filter ActivityId == "ActivityId with Blablabla" 
| summarize max(Timestamp), min(Timestamp)  
| extend Duration = max_Timestamp - min_Timestamp 
 
wabitrace  
| filter Timestamp >= datetime(2015-01-12 11:00:00Z)  
| filter Timestamp < datetime(2015-01-12 13:00:00Z)  
| filter EventText like "NotifyHadoopApplicationJobPerformanceCounters"  	 
| extend Tenant = extract("tenantName=([^,]+),", 1, EventText) 
| extend Environment = extract("environmentName=([^,]+),", 1, EventText)  
| extend UnitOfWorkId = extract("unitOfWorkId=([^,]+),", 1, EventText)  
| extend TotalLaunchedMaps = extract("totalLaunchedMaps=([^,]+),", 1, EventText, typeof(real))  
| extend MapsSeconds = extract("mapsMilliseconds=([^,]+),", 1, EventText, typeof(real)) / 1000 
| extend TotalMapsSeconds = MapsSeconds  / TotalLaunchedMaps 
| filter Tenant == 'DevDiv' and Environment == 'RollupDev2'  
| filter TotalLaunchedMaps > 0 
| summarize sum(TotalMapsSeconds) by UnitOfWorkId  
| extend JobMapsSeconds = sum_TotalMapsSeconds * 1 
| project UnitOfWorkId, JobMapsSeconds 
| join ( 
wabitrace  
| filter Timestamp >= datetime(2015-01-12 11:00:00Z)  
| filter Timestamp < datetime(2015-01-12 13:00:00Z)  
| filter EventText like "NotifyHadoopApplicationJobPerformanceCounters"  
| extend Tenant = extract("tenantName=([^,]+),", 1, EventText) 
| extend Environment = extract("environmentName=([^,]+),", 1, EventText)  
| extend UnitOfWorkId = extract("unitOfWorkId=([^,]+),", 1, EventText)   
| extend TotalLaunchedReducers = extract("totalLaunchedReducers=([^,]+),", 1, EventText, typeof(real)) 
| extend ReducesSeconds = extract("reducesMilliseconds=([^,]+)", 1, EventText, typeof(real)) / 1000 
| extend TotalReducesSeconds = ReducesSeconds / TotalLaunchedReducers 
| filter Tenant == 'DevDiv' and Environment == 'RollupDev2'  
| filter TotalLaunchedReducers > 0 
| summarize sum(TotalReducesSeconds) by UnitOfWorkId  
| extend JobReducesSeconds = sum_TotalReducesSeconds * 1 
| project UnitOfWorkId, JobReducesSeconds ) 
on UnitOfWorkId 
| join ( 
wabitrace  
| filter Timestamp >= datetime(2015-01-12 11:00:00Z)  
| filter Timestamp < datetime(2015-01-12 13:00:00Z)  
| filter EventText like "NotifyHadoopApplicationJobPerformanceCounters"  
| extend Tenant = extract("tenantName=([^,]+),", 1, EventText) 
| extend Environment = extract("environmentName=([^,]+),", 1, EventText)  
| extend JobName = extract("jobName=([^,]+),", 1, EventText)  
| extend StepName = extract("stepName=([^,]+),", 1, EventText)  
| extend UnitOfWorkId = extract("unitOfWorkId=([^,]+),", 1, EventText)  
| extend LaunchTime = extract("launchTime=([^,]+),", 1, EventText, typeof(datetime))  
| extend FinishTime = extract("finishTime=([^,]+),", 1, EventText, typeof(datetime)) 
| extend TotalLaunchedMaps = extract("totalLaunchedMaps=([^,]+),", 1, EventText, typeof(real))  
| extend TotalLaunchedReducers = extract("totalLaunchedReducers=([^,]+),", 1, EventText, typeof(real)) 
| extend MapsSeconds = extract("mapsMilliseconds=([^,]+),", 1, EventText, typeof(real)) / 1000 
| extend ReducesSeconds = extract("reducesMilliseconds=([^,]+)", 1, EventText, typeof(real)) / 1000 
| extend TotalMapsSeconds = MapsSeconds  / TotalLaunchedMaps  
| extend TotalReducesSeconds = (ReducesSeconds / TotalLaunchedReducers / ReducesSeconds) * ReducesSeconds  
| extend CalculatedDuration = (TotalMapsSeconds + TotalReducesSeconds) * time(1s) 
| filter Tenant == 'DevDiv' and Environment == 'RollupDev2') 
on UnitOfWorkId 
| extend MapsFactor = TotalMapsSeconds / JobMapsSeconds 
| extend ReducesFactor = TotalReducesSeconds / JobReducesSeconds 
| extend CurrentLoad = 1536 + (768 * TotalLaunchedMaps) + (1536 * TotalLaunchedMaps) 
| extend NormalizedLoad = 1536 + (768 * TotalLaunchedMaps * MapsFactor) + (1536 * TotalLaunchedMaps * ReducesFactor) 
| summarize sum(CurrentLoad), sum(NormalizedLoad) by  JobName  
| extend SaveFactor = sum_NormalizedLoad / sum_CurrentLoad 
```

## Chart concurrent sessions over time

Suppose you have a table of activities and their start and end times. You can show a chart that displays how many activities run concurrently over time.

Here's a sample input, called `X`:

|SessionId | StartTime | StopTime |
|---|---|---|
| a | 10:01:03 | 10:10:08 |
| b | 10:01:29 | 10:03:10 |
| c | 10:03:02 | 10:05:20 |

For a chart in one-minute bins, you want to count each running activity at each one-minute interval.

Here's an intermediate result:

```kusto
X | extend samples = range(bin(StartTime, 1m), StopTime, 1m)
```

`range` generates an array of values at the specified intervals:

|SessionId | StartTime | StopTime  | samples|
|---|---|---|---|
| a | 10:01:33 | 10:06:31 | [10:01:00,10:02:00,...10:06:00]|
| b | 10:02:29 | 10:03:45 | [10:02:00,10:03:00]|
| c | 10:03:12 | 10:04:30 | [10:03:00,10:04:00]|

Instead of keeping those arrays, expand them by using [mv-expand](./mvexpandoperator.md):

```kusto
X | mv-expand samples = range(bin(StartTime, 1m), StopTime , 1m)
```

|SessionId | StartTime | StopTime  | samples|
|---|---|---|---|
| a | 10:01:33 | 10:06:31 | 10:01:00|
| a | 10:01:33 | 10:06:31 | 10:02:00|
| a | 10:01:33 | 10:06:31 | 10:03:00|
| a | 10:01:33 | 10:06:31 | 10:04:00|
| a | 10:01:33 | 10:06:31 | 10:05:00|
| a | 10:01:33 | 10:06:31 | 10:06:00|
| b | 10:02:29 | 10:03:45 | 10:02:00|
| b | 10:02:29 | 10:03:45 | 10:03:00|
| c | 10:03:12 | 10:04:30 | 10:03:00|
| c | 10:03:12 | 10:04:30 | 10:04:00|

Now, group the results by sample time and count the occurrences of each activity:

```kusto
X
| mv-expand samples = range(bin(StartTime, 1m), StopTime , 1m)
| summarize count_SessionId = count() by bin(todatetime(samples),1m)
```

* Use `todatetime()` because [mv-expand](./mvexpandoperator.md) results in a column of dynamic type.
* Use `bin()` because, for numeric values and dates, if you don't supply an interval, `summarize` always applies a `bin()` function by using a default interval. 

Here's the output:

| count_SessionId | samples|
|---|---|
| 1 | 10:01:00|
| 2 | 10:02:00|
| 3 | 10:03:00|
| 2 | 10:04:00|
| 1 | 10:05:00|
| 1 | 10:06:00|

You can use a bar chart or timechart to render the results.

## Introduce null bins into *summarize*

When the `summarize` operator is applied over a group key that consists of a date-time column, bin those values to fixed-width bins:

```kusto
let StartTime=ago(12h);
let StopTime=now()
T
| where Timestamp > StartTime and Timestamp <= StopTime 
| where ...
| summarize Count=count() by bin(Timestamp, 5m)
```

This example produces a table that has a single row per group of rows in `T` that fall into each bin of five minutes.

What the code doesn't do is add "null bins"—rows for time bin values between `StartTime` and `StopTime` for which there's no corresponding row in `T`. It's a good idea to "pad" the table with those bins. Here's one way to do it:

```kusto
let StartTime=ago(12h);
let StopTime=now()
T
| where Timestamp > StartTime and Timestamp <= StopTime 
| summarize Count=count() by bin(Timestamp, 5m)
| where ...
| union ( // 1
  range x from 1 to 1 step 1 // 2
  | mv-expand Timestamp=range(StartTime, StopTime, 5m) to typeof(datetime) // 3
  | extend Count=0 // 4
  )
| summarize Count=sum(Count) by bin(Timestamp, 5m) // 5 
```

Here's a step-by-step explanation of the preceding query: 

1. Use the `union` operator to add more rows to a table. Those rows are produced by the `union` expression.
1. The `range` operator produces a table that has a single row and column. The table isn't used for anything other than for `mv-expand` to work on.
1. The `mv-expand` operator over the `range` function creates as many rows as there are five-minute bins between `StartTime` and `EndTime`.
1. Use a `Count` of `0`.
1. The `summarize` operator groups together bins from the original (left, or outer) argument to `union`. The operator also bins from the inner argument to it (the null bin rows). This process ensures that the output has one row per bin whose value is either zero or the original count.

## Get more from your data by using Kusto with machine learning 

Many interesting use cases use machine learning algorithms and derive interesting insights from telemetry data. Often, these algorithms require a strictly structured dataset as their input. The raw log data usually doesn't match the required structure and size. 

Start by looking for anomalies in the error rate of a specific Bing inferences service. The logs table has 65 billion records. The following basic query filters 250,000 errors, and then creates a time series of error count that uses the anomaly detection function [series_decompose_anomalies](series-decompose-anomaliesfunction.md). The anomalies are detected by the Kusto service and are highlighted as red dots on the time series chart.

```kusto
Logs
| where Timestamp >= datetime(2015-08-22) and Timestamp < datetime(2015-08-23) 
| where Level == "e" and Service == "Inferences.UnusualEvents_Main" 
| summarize count() by bin(Timestamp, 5min)
| render anomalychart 
```

The service identified few time buckets that had suspicious error rates. Use Kusto to zoom into this timeframe. Then, run a query that aggregates on the `Message` column. Try to find the top errors. 

The relevant parts of the entire stack trace of the message are trimmed out, so the results fit better on the page. 

You can see successful identification of the top eight errors. However, next is a long series of errors, because the error message was created by using a format string that contained changing data:

```kusto
Logs
| where Timestamp >= datetime(2015-08-22 05:00) and Timestamp < datetime(2015-08-22 06:00)
| where Level == "e" and Service == "Inferences.UnusualEvents_Main"
| summarize count() by Message 
| top 10 by count_ 
| project count_, Message 
```

|count_|Message
|---|---
|7125|ExecuteAlgorithmMethod for method 'RunCycleFromInterimData' has failed...
|7125|InferenceHostService call failed..System.NullReferenceException: Object reference not set to an instance of an object...
|7124|Unexpected Inference System error..System.NullReferenceException: Object reference not set to an instance of an object... 
|5112|Unexpected Inference System error..System.NullReferenceException: Object reference not set to an instance of an object..
|174|InferenceHostService call failed..System.ServiceModel.CommunicationException: There was an error writing to the pipe:...
|10|ExecuteAlgorithmMethod for method 'RunCycleFromInterimData' has failed...
|10|Inference System error..Microsoft.Bing.Platform.Inferences.Service.Managers.UserInterimDataManagerException:...
|3|InferenceHostService call failed..System.ServiceModel.CommunicationObjectFaultedException:...
|1|Inference System error... SocialGraph.BOSS.OperationResponse...AIS TraceId:8292FC561AC64BED8FA243808FE74EFD...
|1|Inference System error... SocialGraph.BOSS.OperationResponse...AIS TraceId: 5F79F7587FF943EC9B641E02E701AFBF...

At this point, using the `reduce` operator helps. The operator identified 63 different errors that originated at the same trace instrumentation point in the code. `reduce` helps focus on additional meaningful error traces in that time window.

```kusto
Logs
| where Timestamp >= datetime(2015-08-22 05:00) and Timestamp < datetime(2015-08-22 06:00)
| where Level == "e" and Service == "Inferences.UnusualEvents_Main"
| reduce by Message with threshold=0.35
| project Count, Pattern
```

|Count|Pattern
|---|---
|7125|ExecuteAlgorithmMethod for method 'RunCycleFromInterimData' has failed...
|  7125|InferenceHostService call failed..System.NullReferenceException: Object reference not set to an instance of an object...
|  7124|Unexpected Inference System error..System.NullReferenceException: Object reference not set to an instance of an object...
|  5112|Unexpected Inference System error..System.NullReferenceException: Object reference not set to an instance of an object...
|  174|InferenceHostService call failed..System.ServiceModel.CommunicationException: There was an error writing to the pipe:...
|  63|Inference System error..Microsoft.Bing.Platform.Inferences.\*: Write \* to write to the Object BOSS.\*: SocialGraph.BOSS.Reques...
|  10|ExecuteAlgorithmMethod for method 'RunCycleFromInterimData' has failed...
|  10|Inference System error..Microsoft.Bing.Platform.Inferences.Service.Managers.UserInterimDataManagerException:...
|  3|InferenceHostService call failed..System.ServiceModel.\*: The object, System.ServiceModel.Channels.\*+\*, for \*\* is the \*... at Syst...

Now, you have a good view into the top errors that contributed to the detected anomalies.

To understand the effect of these errors across the sample system, consider that: 
* The `Logs` table contains additional dimensional data, like `Component` and `Cluster`.
* The new autocluster plugin can help derive component and cluster insight with a simple query. 

In the following example, you can clearly see that each of the top four errors is specific to a component. Also, although the top three errors are specific to the DB4 cluster, the fourth error happens across all clusters.

```kusto
Logs
| where Timestamp >= datetime(2015-08-22 05:00) and Timestamp < datetime(2015-08-22 06:00)
| where Level == "e" and Service == "Inferences.UnusualEvents_Main"
| evaluate autocluster()
```

|Count |Percentage (%)|Component|Cluster|Message
|---|---|---|---|---
|7125|26.64|InferenceHostService|DB4|ExecuteAlgorithmMethod for method...
|7125|26.64|Unknown Component|DB4|InferenceHostService call failed...
|7124|26.64|InferenceAlgorithmExecutor|DB4|Unexpected Inference System error...
|5112|19.11|InferenceAlgorithmExecutor|*|Unexpected Inference System error...

## Map values from one set to another

A common query use case is static mapping of values. Static mapping can help make results more presentable.

For example, in the next table, `DeviceModel` specifies a device model. Using the device model isn't a convenient form of referencing the device name.  

|DeviceModel |Count 
|---|---
|iPhone5,1 |32 
|iPhone3,2 |432 
|iPhone7,2 |55 
|iPhone5,2 |66 

 Using a friendly name is more convenient:  

|FriendlyName |Count 
|---|---
|iPhone 5 |32 
|iPhone 4 |432 
|iPhone 6 |55 
|iPhone5 |66 

The next two examples demonstrate how to change from using a device model to a friendly name to identify a device.  

### Map by using a dynamic dictionary

You can achieve mapping by using a dynamic dictionary and dynamic accessors. For example:

<!-- csl: https://help.kusto.windows.net:443/Samples -->
```kusto
// Dataset definition
let Source = datatable(DeviceModel:string, Count:long)
[
  'iPhone5,1', 32,
  'iPhone3,2', 432,
  'iPhone7,2', 55,
  'iPhone5,2', 66,
];
// Query start here
let phone_mapping = dynamic(
  {
    "iPhone5,1" : "iPhone 5",
    "iPhone3,2" : "iPhone 4",
    "iPhone7,2" : "iPhone 6",
    "iPhone5,2" : "iPhone5"
  });
Source 
| project FriendlyName = phone_mapping[DeviceModel], Count
```

|FriendlyName|Count|
|---|---|
|iPhone 5|32|
|iPhone 4|432|
|iPhone 6|55|
|iPhone5|66|

### Map by using a static table

You also can achieve mapping by using a persistent table and a `join` operator.
 
1. Create the mapping table only once:

    ```kusto
    .create table Devices (DeviceModel: string, FriendlyName: string) 

    .ingest inline into table Devices 
        ["iPhone5,1","iPhone 5"]["iPhone3,2","iPhone 4"]["iPhone7,2","iPhone 6"]["iPhone5,2","iPhone5"]
    ```

1. Create a table of the device contents:

    |DeviceModel |FriendlyName 
    |---|---
    |iPhone5,1 |iPhone 5 
    |iPhone3,2 |iPhone 4 
    |iPhone7,2 |iPhone 6 
    |iPhone5,2 |iPhone5 

1. Create a test table source:

    ```kusto
    .create table Source (DeviceModel: string, Count: int)

    .ingest inline into table Source ["iPhone5,1",32]["iPhone3,2",432]["iPhone7,2",55]["iPhone5,2",66]
    ```

1. Join the tables and run the project:

   ```kusto
   Devices  
   | join (Source) on DeviceModel  
   | project FriendlyName, Count
   ```

Here's the output:

|FriendlyName |Count 
|---|---
|iPhone 5 |32 
|iPhone 4 |432 
|iPhone 6 |55 
|iPhone5 |66 


## Create and use query-time dimension tables

Often, you'll want to join the results of a query with an ad-hoc dimension table that isn't stored in the database. You can define an expression whose result is a table scoped to a single query. 

For example:

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
// Create a query-time dimension table using datatable
let DimTable = datatable(EventType:string, Code:string)
  [
    "Heavy Rain", "HR",
    "Tornado",    "T"
  ]
;
DimTable
| join StormEvents on EventType
| summarize count() by Code
```

Here's a slightly more complex example:

```kusto
// Create a query-time dimension table using datatable
let TeamFoundationJobResult = datatable(Result:int, ResultString:string)
  [
    -1, 'None', 0, 'Succeeded', 1, 'PartiallySucceeded', 2, 'Failed',
    3, 'Stopped', 4, 'Killed', 5, 'Blocked', 6, 'ExtensionNotFound',
    7, 'Inactive', 8, 'Disabled', 9, 'JobInitializationError'
  ]
;
JobHistory
  | where PreciseTimeStamp > ago(1h)
  | where Service  != "AX"
  | where Plugin has "Analytics"
  | sort by PreciseTimeStamp desc
  | join kind=leftouter TeamFoundationJobResult on Result
  | extend ExecutionTimeSpan = totimespan(ExecutionTime)
  | project JobName, StartTime, ExecutionTimeSpan, ResultString, ResultMessage
```

## Retrieve the latest records (by timestamp) per identity

Suppose you have a table that includes:
* An `ID` column that identifies the entity with which each row is associated, such as a user ID or a node ID
* A `timestamp` column that provides the time reference for the row
* Other columns

You can use the [top-nested operator](topnestedoperator.md) to make a query that returns the latest two records for each value of the `ID` column, where _latest_ is defined as _having the highest value of `timestamp`_:

```kusto
datatable(id:string, timestamp:datetime, bla:string)           // #1
  [
  "Barak",  datetime(2015-01-01), "1",
  "Barak",  datetime(2016-01-01), "2",
  "Barak",  datetime(2017-01-20), "3",
  "Donald", datetime(2017-01-20), "4",
  "Donald", datetime(2017-01-18), "5",
  "Donald", datetime(2017-01-19), "6"
  ]
| top-nested   of id        by dummy0=max(1),                  // #2
  top-nested 2 of timestamp by dummy1=max(timestamp),          // #3
  top-nested   of bla       by dummy2=max(1)                   // #4
| project-away dummy0, dummy1, dummy2                          // #5
```


Here's a step-by-step explanation of the preceding query (the numbering refers to the numbers in the code comments):

1. The `datatable` is a way to produce some test data for demonstration purposes. Normally, you'd use real data here.
1. This line essentially means _return all distinct values of `id`_. 
1. This line then returns, for the top two records that maximize:
     * The `timestamp` column
     * The columns of the preceding level (here, just `id`)
     * The column specified at this level (here, `timestamp`)
1. This line adds the values of the `bla` column for each of the records returned by the preceding level. If the table has other columns you're interested in, you can repeat this line for each of those columns.
1. The final line uses the [project-away operator](projectawayoperator.md) to remove the "extra" columns that are introduced by `top-nested`.

## Extend a table by a percentage of the total calculation

A tabular expression that includes a numeric column is more useful to the user when it's accompanied by its value as a percentage of the total.

For example, assume that a query produces the following table:

|SomeSeries|SomeInt|
|----------|-------|
|Apple       |    100|
|Banana       |    200|

You want to show the table like this:

|SomeSeries|SomeInt|Pct |
|----------|-------|----|
|Apple       |    100|33.3|
|Banana       |    200|66.6|

To change the way the table appears, calculate the total (sum) of the `SomeInt` column, and then divide each value of this column by the total. For arbitrary results, use the [as operator](asoperator.md).

For example:

```kusto
// The following table literally represents a long calculation
// that ends up with an anonymous tabular value:
datatable (SomeInt:int, SomeSeries:string) [
  100, "Apple",
  200, "Banana",
]
// We now give this calculation a name ("X"):
| as X
// Having this name we can refer to it in the sub-expression
// "X | summarize sum(SomeInt)":
| extend Pct = 100 * bin(todouble(SomeInt) / toscalar(X | summarize sum(SomeInt)), 0.001)
```

## Perform aggregations over a sliding window

The following example shows how to summarize columns by using a sliding window. For the query, use the following table, which contains prices of fruits by timestamps.

Calculate the minimum, maximum, and sum costs of each fruit per day by using a sliding window of seven days. Each record in the result set aggregates the preceding seven days, and the results contain a record per day in the analysis period.

Fruit table:

|Timestamp|Fruit|Price|
|---|---|---|
|2018-09-24 21:00:00.0000000|Bananas|3|
|2018-09-25 20:00:00.0000000|Apples|9|
|2018-09-26 03:00:00.0000000|Bananas|4|
|2018-09-27 10:00:00.0000000|Plums|8|
|2018-09-28 07:00:00.0000000|Bananas|6|
|2018-09-29 21:00:00.0000000|Bananas|8|
|2018-09-30 01:00:00.0000000|Plums|2|
|2018-10-01 05:00:00.0000000|Bananas|0|
|2018-10-02 02:00:00.0000000|Bananas|0|
|2018-10-03 13:00:00.0000000|Plums|4|
|2018-10-04 14:00:00.0000000|Apples|8|
|2018-10-05 05:00:00.0000000|Bananas|2|
|2018-10-06 08:00:00.0000000|Plums|8|
|2018-10-07 12:00:00.0000000|Bananas|0|

Here's the sliding window aggregation query. See the explanation after the query result.

```kusto
let _start = datetime(2018-09-24);
let _end = _start + 13d; 
Fruits 
| extend _bin = bin_at(Timestamp, 1d, _start) // #1 
| extend _endRange = iif(_bin + 7d > _end, _end, 
                            iff( _bin + 7d - 1d < _start, _start,
                                iff( _bin + 7d - 1d < _bin, _bin,  _bin + 7d - 1d)))  // #2
| extend _range = range(_bin, _endRange, 1d) // #3
| mv-expand _range to typeof(datetime) limit 1000000 // #4
| summarize min(Price), max(Price), sum(Price) by Timestamp=bin_at(_range, 1d, _start) ,  Fruit // #5
| where Timestamp >= _start + 7d; // #6

```

Here's the output:

|Timestamp|Fruit|min_Price|max_Price|sum_Price|
|---|---|---|---|---|
|2018-10-01 00:00:00.0000000|Apples|9|9|9|
|2018-10-01 00:00:00.0000000|Bananas|0|8|18|
|2018-10-01 00:00:00.0000000|Plums|2|8|10|
|2018-10-02 00:00:00.0000000|Bananas|0|8|18|
|2018-10-02 00:00:00.0000000|Plums|2|8|10|
|2018-10-03 00:00:00.0000000|Plums|2|8|14|
|2018-10-03 00:00:00.0000000|Bananas|0|8|14|
|2018-10-04 00:00:00.0000000|Bananas|0|8|14|
|2018-10-04 00:00:00.0000000|Plums|2|4|6|
|2018-10-04 00:00:00.0000000|Apples|8|8|8|
|2018-10-05 00:00:00.0000000|Bananas|0|8|10|
|2018-10-05 00:00:00.0000000|Plums|2|4|6|
|2018-10-05 00:00:00.0000000|Apples|8|8|8|
|2018-10-06 00:00:00.0000000|Plums|2|8|14|
|2018-10-06 00:00:00.0000000|Bananas|0|2|2|
|2018-10-06 00:00:00.0000000|Apples|8|8|8|
|2018-10-07 00:00:00.0000000|Bananas|0|2|2|
|2018-10-07 00:00:00.0000000|Plums|4|8|12|
|2018-10-07 00:00:00.0000000|Apples|8|8|8|

The query "stretches" (duplicates) each record in the input table throughout the seven days after its actual appearance. Each record actually appears seven times. As a result, the daily aggregation includes all records of the preceding seven days.


Here's a step-by-step explanation of the preceding query: 

1. Bin each record to one day (relative to `_start`). 
1. Determine the end of the range per record: `_bin + 7d`, unless the value is out of the range of `_start` and `_end`, in which case, it's adjusted. 
1. For each record, create an array of seven days (timestamps), starting at the current record's day. 
1. `mv-expand` the array, thus duplicating each record to seven records, one day apart from each other. 
1. Perform the aggregation function for each day. Due to #4, this step actually summarizes the _past_ seven days. 
1. The data for the first seven days is incomplete because there's no seven-day lookback period for the first seven days. The first seven days are excluded from the final result. In the example, they participate only in the aggregation for 2018-10-01.

## Find the preceding event

The next example demonstrates how to find a preceding event between two datasets.  

You have two datasets, A and B. For each record in dataset B, find its preceding event in dataset A (that is, the `arg_max` record in A that is still _older_ than B).

Here are the sample datasets: 

```kusto
let A = datatable(Timestamp:datetime, ID:string, EventA:string)
[
    datetime(2019-01-01 00:00:00), "x", "Ax1",
    datetime(2019-01-01 00:00:01), "x", "Ax2",
    datetime(2019-01-01 00:00:02), "y", "Ay1",
    datetime(2019-01-01 00:00:05), "y", "Ay2",
    datetime(2019-01-01 00:00:00), "z", "Az1"
];
let B = datatable(Timestamp:datetime, ID:string, EventB:string)
[
    datetime(2019-01-01 00:00:03), "x", "B",
    datetime(2019-01-01 00:00:04), "x", "B",
    datetime(2019-01-01 00:00:04), "y", "B",
    datetime(2019-01-01 00:02:00), "z", "B"
];
A; B
```

|Timestamp|ID|EventB|
|---|---|---|
|2019-01-01 00:00:00.0000000|x|Ax1|
|2019-01-01 00:00:00.0000000|z|Az1|
|2019-01-01 00:00:01.0000000|x|Ax2|
|2019-01-01 00:00:02.0000000|y|Ay1|
|2019-01-01 00:00:05.0000000|y|Ay2|

</br>

|Timestamp|ID|EventA|
|---|---|---|
|2019-01-01 00:00:03.0000000|x|B|
|2019-01-01 00:00:04.0000000|x|B|
|2019-01-01 00:00:04.0000000|y|B|
|2019-01-01 00:02:00.0000000|z|B|

Expected output: 

|ID|Timestamp|EventB|A_Timestamp|EventA|
|---|---|---|---|---|
|x|2019-01-01 00:00:03.0000000|B|2019-01-01 00:00:01.0000000|Ax2|
|x|2019-01-01 00:00:04.0000000|B|2019-01-01 00:00:01.0000000|Ax2|
|y|2019-01-01 00:00:04.0000000|B|2019-01-01 00:00:02.0000000|Ay1|
|z|2019-01-01 00:02:00.0000000|B|2019-01-01 00:00:00.0000000|Az1|

We recommend two different approaches for this problem. You can test both on your specific dataset to find the one that is most suitable for your scenario.

> [!NOTE] 
> Each approach might run differently on different datasets.

### Approach 1

This approach serializes both datasets by ID and timestamp. Then, it groups all events in dataset B with all their preceding events in dataset A. Finally, it picks the `arg_max` out of all the events in dataset A in the group.

```kusto
A
| extend A_Timestamp = Timestamp, Kind="A"
| union (B | extend B_Timestamp = Timestamp, Kind="B")
| order by ID, Timestamp asc 
| extend t = iff(Kind == "A" and (prev(Kind) != "A" or prev(Id) != ID), 1, 0)
| extend t = row_cumsum(t)
| summarize Timestamp=make_list(Timestamp), EventB=make_list(EventB), arg_max(A_Timestamp, EventA) by t, ID
| mv-expand Timestamp to typeof(datetime), EventB to typeof(string)
| where isnotempty(EventB)
| project-away t
```

### Approach 2

This approach to solving the problem requires a maximum lookback period. The approach looks at how much _older_ the record in dataset A might be compared to dataset B. The method then joins the two datasets based on ID and this lookback period.

The `join` produces all possible candidates, all dataset A records that are older than records in dataset B and within the lookback period. Then, the closest one to dataset B is filtered by `arg_min (TimestampB - TimestampA)`. The shorter the lookback period is, the better the query results will be.

In the following example, the lookback period is set to `1m`. The record with ID `z` doesn't have a corresponding `A` event because its `A` event is older by two minutes.

```kusto 
let _maxLookbackPeriod = 1m;  
let _internalWindowBin = _maxLookbackPeriod / 2;
let B_events = B 
    | extend ID = new_guid()
    | extend _time = bin(Timestamp, _internalWindowBin)
    | extend _range = range(_time - _internalWindowBin, _time + _maxLookbackPeriod, _internalWindowBin) 
    | mv-expand _range to typeof(datetime) 
    | extend B_Timestamp = Timestamp, _range;
let A_events = A 
    | extend _time = bin(Timestamp, _internalWindowBin)
    | extend _range = range(_time - _internalWindowBin, _time + _maxLookbackPeriod, _internalWindowBin) 
    | mv-expand _range to typeof(datetime) 
    | extend A_Timestamp = Timestamp, _range;
B_events
    | join kind=leftouter (
        A_events
) on ID, _range
| where isnull(A_Timestamp) or (A_Timestamp <= B_Timestamp and B_Timestamp <= A_Timestamp + _maxLookbackPeriod)
| extend diff = coalesce(B_Timestamp - A_Timestamp, _maxLookbackPeriod*2)
| summarize arg_min(diff, *) by ID
| project ID, B_Timestamp, A_Timestamp, EventB, EventA
```

|ID|B_Timestamp|A_Timestamp|EventB|EventA|
|---|---|---|---|---|
|x|2019-01-01 00:00:03.0000000|2019-01-01 00:00:01.0000000|B|Ax2|
|x|2019-01-01 00:00:04.0000000|2019-01-01 00:00:01.0000000|B|Ax2|
|y|2019-01-01 00:00:04.0000000|2019-01-01 00:00:02.0000000|B|Ay1|
|z|2019-01-01 00:02:00.0000000||B||


## Next steps

- Walk through a [a tutorial on the Kusto Query Language](tutorial.md?pivots=azuredataexplorer).

::: zone-end

::: zone pivot="azuremonitor"

This article identifies common query needs in Azure Monitor and how you can use the Kusto Query Language to meet them.

## String operations

The following sections give examples of how to work with strings when using the Kusto Query Language.

### Strings and how to escape them

String values are wrapped with either single or double quotes. Add the backslash (\\) to the left of a character to escape the character: `\t` for tab, `\n` for newline, and `\"` for the single quote character.

```kusto
print "this is a 'string' literal in double \" quotes"
```

```kusto
print 'this is a "string" literal in single \' quotes'
```

To prevent "\\" from acting as an escape character, add "\@" as a prefix to the string:

```kusto
print @"C:\backslash\not\escaped\with @ prefix"
```


### String comparisons

Operator       |Description                         |Case-sensitive|Example (yields `true`)
---------------|------------------------------------|--------------|-----------------------
`==`           |Equals                              |Yes           |`"aBc" == "aBc"`
`!=`           |Not equals                          |Yes           |`"abc" != "ABC"`
`=~`           |Equals                              |No            |`"abc" =~ "ABC"`
`!~`           |Not equals                          |No            |`"aBc" !~ "xyz"`
`has`          |Right-side value is a whole term in left-side value |No|`"North America" has "america"`
`!has`         |Right-side value isn't a full term in left-side value       |No            |`"North America" !has "amer"` 
`has_cs`       |Right-side value is a whole term in left-side value |Yes|`"North America" has_cs "America"`
`!has_cs`      |Right-side value isn't a full term in left-side value       |Yes            |`"North America" !has_cs "amer"` 
`hasprefix`    |Right-side value is a term prefix in left-side value         |No            |`"North America" hasprefix "ame"`
`!hasprefix`   |Right-side value isn't a term prefix in left-side value     |No            |`"North America" !hasprefix "mer"` 
`hasprefix_cs`    |Right-side value is a term prefix in left-side value         |Yes            |`"North America" hasprefix_cs "Ame"`
`!hasprefix_cs`   |Right-side value isn't a term prefix in left-side value     |Yes            |`"North America" !hasprefix_cs "CA"` 
`hassuffix`    |Right-side value is a term suffix in left-side value         |No            |`"North America" hassuffix "ica"`
`!hassuffix`   |Right-side value isn't a term suffix in left-side value     |No            |`"North America" !hassuffix "americ"`
`hassuffix_cs`    |Right-side value is a term suffix in left-side value         |Yes            |`"North America" hassuffix_cs "ica"`
`!hassuffix_cs`   |Right-side value isn't a term suffix in left-side value     |Yes            |`"North America" !hassuffix_cs "icA"`
`contains`     |Right-side value occurs as a subsequence of left-side value  |No            |`"FabriKam" contains "BRik"`
`!contains`    |Right-side value doesn't occur in left-side value           |No            |`"Fabrikam" !contains "xyz"`
`contains_cs`   |Right-side value occurs as a subsequence of left-side value  |Yes           |`"FabriKam" contains_cs "Kam"`
`!contains_cs`  |Right-side value doesn't occur in left-side value           |Yes           |`"Fabrikam" !contains_cs "Kam"`
`startswith`   |Right-side value is an initial subsequence of left-side value|No            |`"Fabrikam" startswith "fab"`
`!startswith`  |Right-side value isn't an initial subsequence of left-side value|No        |`"Fabrikam" !startswith "kam"`
`startswith_cs`   |Right-side value is an initial subsequence of left-side value|Yes            |`"Fabrikam" startswith_cs "Fab"`
`!startswith_cs`  |Right-side value isn't an initial subsequence of left-side value|Yes        |`"Fabrikam" !startswith_cs "fab"`
`endswith`     |Right-side value is a closing subsequence of left-side value|No             |`"Fabrikam" endswith "Kam"`
`!endswith`    |Right-side value isn't a closing subsequence of left-side value|No         |`"Fabrikam" !endswith "brik"`
`endswith_cs`     |Right-side value is a closing subsequence of left-side value|Yes             |`"Fabrikam" endswith "Kam"`
`!endswith_cs`    |Right-side value isn't a closing subsequence of left-side value|Yes         |`"Fabrikam" !endswith "brik"`
`matches regex`|Left-side value contains a match for right-side value|Yes           |`"Fabrikam" matches regex "b.*k"`
`in`           |Equals to one of the elements       |Yes           |`"abc" in ("123", "345", "abc")`
`!in`          |Not equals to any of the elements   |Yes           |`"bca" !in ("123", "345", "abc")`


### *countof*

Counts occurrences of a substring within a string. Can match plain strings or use a regular expression (regex). Plain string matches might overlap, but regex matches don't overlap.

```
countof(text, search [, kind])
```

- `text`: The input string 
- `search`: Plain string or regex to match inside text
- `kind`: _normal_ | _regex_ (default: normal).

Returns the number of times that the search string can be matched in the container. Plain string matches might overlap, but regex matches don't overlap.

#### Plain string matches

```kusto
print countof("The cat sat on the mat", "at");  //result: 3
print countof("aaa", "a");  //result: 3
print countof("aaaa", "aa");  //result: 3 (not 2!)
print countof("ababa", "ab", "normal");  //result: 2
print countof("ababa", "aba");  //result: 2
```

#### Regex matches

```kusto
print countof("The cat sat on the mat", @"\b.at\b", "regex");  //result: 3
print countof("ababa", "aba", "regex");  //result: 1
print countof("abcabc", "a.c", "regex");  // result: 2
```


### *extract*

Gets a match for a regular expression from a specific string. Optionally, can convert the extracted substring to the specified type.

```kusto
extract(regex, captureGroup, text [, typeLiteral])
```

- `regex`: A regular expression.
- `captureGroup`: A positive integer constant that indicates the capture group to extract. Use 0 for the entire match, 1 for the value matched by the first parenthesis \(\) in the regular expression, and 2 or more for subsequent parentheses.
- `text` - The string to search.
- `typeLiteral` - An optional type literal (for example, `typeof(long)`). If provided, the extracted substring is converted to this type.

Returns the substring matched against the indicated capture group `captureGroup`, optionally converted to `typeLiteral`. If there's no match or the type conversion fails, returns null.

The following example extracts the last octet of `ComputerIP` from a heartbeat record:

```kusto
Heartbeat
| where ComputerIP != "" 
| take 1
| project ComputerIP, last_octet=extract("([0-9]*$)", 1, ComputerIP) 
```

The following example extracts the last octet, casts it to a *real* type (number), and then calculates the next IP value:

```kusto
Heartbeat
| where ComputerIP != "" 
| take 1
| extend last_octet=extract("([0-9]*$)", 1, ComputerIP, typeof(real)) 
| extend next_ip=(last_octet+1)%255
| project ComputerIP, last_octet, next_ip
```

In the next example, the string `Trace` is searched for a definition of `Duration`. The match is cast to `real` and multiplied by a time constant (1 s), which then casts `Duration` to type `timespan`.

```kusto
let Trace="A=12, B=34, Duration=567, ...";
print Duration = extract("Duration=([0-9.]+)", 1, Trace, typeof(real));  //result: 567
print Duration_seconds =  extract("Duration=([0-9.]+)", 1, Trace, typeof(real)) * time(1s);  //result: 00:09:27
```


### *isempty*, *isnotempty*, *notempty*

- `isempty` returns `true` if the argument is an empty string or null (see `isnull`).
- `isnotempty` returns `true` if the argument isn't an empty string or null (see `isnotnull`). Alias: `notempty`.


```kusto
isempty(value)
isnotempty(value)
```

#### Example

```kusto
print isempty("");  // result: true

print isempty("0");  // result: false

print isempty(0);  // result: false

print isempty(5);  // result: false

Heartbeat | where isnotempty(ComputerIP) | take 1  // return 1 Heartbeat record in which ComputerIP isn't empty
```


### *parseurl*

Splits a URL into its parts, like protocol, host, and port, and then returns a dictionary object that contains the parts as strings.

```
parseurl(urlstring)
```

#### Example

```kusto
print parseurl("http://user:pass@contoso.com/icecream/buy.aspx?a=1&b=2#tag")
```

Here's the output:

```
{
	"Scheme" : "http",
	"Host" : "contoso.com",
	"Port" : "80",
	"Path" : "/icecream/buy.aspx",
	"Username" : "user",
	"Password" : "pass",
	"Query Parameters" : {"a":"1","b":"2"},
	"Fragment" : "tag"
}
```

### *replace*

Replaces all regex matches with another string. 

```
replace(regex, rewrite, input_text)
```

- `regex`: The regular expression to match by. It can contain capture groups in parentheses \(\).
- `rewrite`: The replacement regex for any match made by matching a regex. Use \0 to refer to the whole match, \1 for the first capture group, \2, and so on, for subsequent capture groups.
- `input_text`: The input string to search in.

Returns the text after replacing all matches of regex with evaluations of rewrite. Matches don't overlap.

#### Example

```kusto
SecurityEvent
| take 1
| project Activity 
| extend replaced = replace(@"(\d+) -", @"Activity ID \1: ", Activity) 
```

Here's the output:

Activity                                        |Replaced
------------------------------------------------|----------------------------------------------------------
4663 - An attempt was made to access an object  |Activity ID 4663: An attempt was made to access an object.


### *split*

Splits a specific string according to a specified delimiter, and then returns an array of the resulting substrings.

```
split(source, delimiter [, requestedIndex])
```

- `source`: The string to be split according to the specified delimiter.
- `delimiter`: The delimiter that will be used to split the source string.
- `requestedIndex`: An optional zero-based index. If provided, the returned string array holds only that item (if it exists).


#### Example

```kusto
print split("aaa_bbb_ccc", "_");    // result: ["aaa","bbb","ccc"]
print split("aa_bb", "_");          // result: ["aa","bb"]
print split("aaa_bbb_ccc", "_", 1);	// result: ["bbb"]
print split("", "_");              	// result: [""]
print split("a__b", "_");           // result: ["a","","b"]
print split("aabbcc", "bb");        // result: ["aa","cc"]
```

### *strcat*

Concatenates string arguments (supports 1-16 arguments).

```
strcat("string1", "string2", "string3")
```

#### Example

```kusto
print strcat("hello", " ", "world")	// result: "hello world"
```


### *strlen*

Returns the length of a string.

```
strlen("text_to_evaluate")
```

#### Example

```kusto
print strlen("hello")	// result: 5
```


### *substring*

Extracts a substring from a specific source string, starting at the specified index. Optionally, you can specify the length of the requested substring.

```
substring(source, startingIndex [, length])
```

- `source`: The source string that the substring is taken from.
- `startingIndex`: The zero-based starting character position of the requested substring.
- `length`: An optional parameter that you can use to specify the requested length of the returned substring.

#### Example

```kusto
print substring("abcdefg", 1, 2);	// result: "bc"
print substring("123456", 1);		// result: "23456"
print substring("123456", 2, 2);	// result: "34"
print substring("ABCD", 0, 2);	// result: "AB"
```


### *tolower*, *toupper*

Converts a specific string to all lowercase or all uppercase.

```
tolower("value")
toupper("value")
```

#### Example

```kusto
print tolower("HELLO");	// result: "hello"
print toupper("hello");	// result: "HELLO"
```

## Date and time operations

The following sections give examples of how to work with date and time values when using the Kusto Query Language.

### Date-time basics

The Kusto Query Language has two main data types associated with dates and times: `datetime` and `timespan`. All dates are expressed in UTC. Although multiple date-time formats are supported, the ISO-8601 format is preferred. 

Timespans are expressed as a decimal followed by a time unit:

|Shorthand   | Time unit    |
|:---|:---|
|d           | day          |
|h           | hour         |
|m           | minute       |
|s           | second       |
|ms          | millisecond  |
|microsecond | microsecond  |
|tick        | nanosecond   |

You can create date-time values by casting a string using the `todatetime` operator. For example, to review the VM heartbeats sent in a specific timeframe, use the `between` operator to specify a time range:

```kusto
Heartbeat
| where TimeGenerated between(datetime("2018-06-30 22:46:42") .. datetime("2018-07-01 00:57:27"))
```

Another common scenario is comparing a date-time value to the present. For example, to see all heartbeats over the last two minutes, you can use the `now` operator together with a timespan that represents two minutes:

```kusto
Heartbeat
| where TimeGenerated > now() - 2m
```

A shortcut is also available for this function:

```kusto
Heartbeat
| where TimeGenerated > now(-2m)
```

The shortest and most readable method is using the `ago` operator:

```kusto
Heartbeat
| where TimeGenerated > ago(2m)
```

Suppose that instead of knowing the start and end times, you know the start time and the duration. You can rewrite the query:

```kusto
let startDatetime = todatetime("2018-06-30 20:12:42.9");
let duration = totimespan(25m);
Heartbeat
| where TimeGenerated between(startDatetime .. (startDatetime+duration) )
| extend timeFromStart = TimeGenerated - startDatetime
```

### Convert time units

You might want to express a date-time or timespan value in a time unit other than the default. For example, if you're reviewing error events from the past 30 minutes and need a calculated column that shows how long ago the event happened, you can use this query:

```kusto
Event
| where TimeGenerated > ago(30m)
| where EventLevelName == "Error"
| extend timeAgo = now() - TimeGenerated 
```

The `timeAgo` column holds values like `00:09:31.5118992`, which are formatted as hh:mm:ss.fffffff. If you want to format these values to the `number` of minutes since the start time, divide that value by `1m`:

```kusto
Event
| where TimeGenerated > ago(30m)
| where EventLevelName == "Error"
| extend timeAgo = now() - TimeGenerated
| extend timeAgoMinutes = timeAgo/1m 
```

### Aggregations and bucketing by time intervals

Another common scenario is the need to obtain statistics for a specific time period in a specific time unit. For this scenario, you can use a `bin` operator as part of a `summarize` clause.

Use the following query to get the number of events that occurred every five minutes during the past half-hour:

```kusto
Event
| where TimeGenerated > ago(30m)
| summarize events_count=count() by bin(TimeGenerated, 5m) 
```

This query produces the following table:  

|TimeGenerated(UTC)|events_count|
|--|--|
|2018-08-01T09:30:00.000|54|
|2018-08-01T09:35:00.000|41|
|2018-08-01T09:40:00.000|42|
|2018-08-01T09:45:00.000|41|
|2018-08-01T09:50:00.000|41|
|2018-08-01T09:55:00.000|16|

Another way to create buckets of results is to use functions like `startofday`:

```kusto
Event
| where TimeGenerated > ago(4d)
| summarize events_count=count() by startofday(TimeGenerated) 
```

Here's the output:

|timestamp|count_|
|--|--|
|2018-07-28T00:00:00.000|7,136|
|2018-07-29T00:00:00.000|12,315|
|2018-07-30T00:00:00.000|16,847|
|2018-07-31T00:00:00.000|12,616|
|2018-08-01T00:00:00.000|5,416|


### Time zones

Because all date-time values are expressed in UTC, it's often useful to convert these values into the local time zone. For example, use this calculation to convert UTC to PST times:

```kusto
Event
| extend localTimestamp = TimeGenerated - 8h
```

## Aggregations

The following sections give examples of how to aggregate the results of a query when using the Kusto Query Language.

### *count*

Count the number of rows in the result set after any filters are applied. The following example returns the total number of rows in the `Perf` table from the last 30 minutes. The results are returned in a column named `count_` unless you assign a specific name to the column:


```kusto
Perf
| where TimeGenerated > ago(30m) 
| summarize count()
```

```kusto
Perf
| where TimeGenerated > ago(30m) 
| summarize num_of_records=count() 
```

A timechart visualization might be useful to see a trend over time:

```kusto
Perf 
| where TimeGenerated > ago(30m) 
| summarize count() by bin(TimeGenerated, 5m)
| render timechart
```

The output from this example shows the `Perf` record count trend line in five-minute intervals:


:::image type="content" source="images/samples/perf-count-line-chart.png" alt-text="Screenshot of a line chart that shows the Perf record count trend line in five-minute intervals.":::

### *dcount*, *dcountif*

Use `dcount` and `dcountif` to count distinct values in a specific column. The following query evaluates how many distinct computers sent heartbeats in the last hour:

```kusto
Heartbeat 
| where TimeGenerated > ago(1h) 
| summarize dcount(Computer)
```

To count only the Linux computers that sent heartbeats, use `dcountif`:

```kusto
Heartbeat 
| where TimeGenerated > ago(1h) 
| summarize dcountif(Computer, OSType=="Linux")
```

### Evaluate subgroups

To perform a count or other aggregations on subgroups in your data, use the `by` keyword. For example, to count the number of distinct Linux computers that sent heartbeats in each country or region, use this query:

```kusto
Heartbeat 
| where TimeGenerated > ago(1h) 
| summarize distinct_computers=dcountif(Computer, OSType=="Linux") by RemoteIPCountry
```

|RemoteIPCountry  | distinct_computers  |
------------------|---------------------|
|United States 	  | 19          		|
|Canada        	  | 3       	  		|
|Ireland   	      | 0		       		|
|United Kingdom	  | 0		       		|
|Netherlands	  | 2  					|


To analyze even smaller subgroups of your data, add column names to the `by` section. For example, you might want to count the distinct computers from each country or region per type of operating system (`OSType`):

```kusto
Heartbeat 
| where TimeGenerated > ago(1h) 
| summarize distinct_computers=dcountif(Computer, OSType=="Linux") by RemoteIPCountry, OSType
```


### Percentile

To find the median value, use the `percentile` function with a value to specify the percentile:

```kusto
Perf
| where TimeGenerated > ago(30m) 
| where CounterName == "% Processor Time" and InstanceName == "_Total" 
| summarize percentiles(CounterValue, 50) by Computer
```

You also can specify different percentiles to get an aggregated result for each:

```kusto
Perf
| where TimeGenerated > ago(30m) 
| where CounterName == "% Processor Time" and InstanceName == "_Total" 
| summarize percentiles(CounterValue, 25, 50, 75, 90) by Computer
```

The results might show that some computer CPUs have similar median values. However, although some computers are steady around the median, others have reported much lower and higher CPU values. The high and low values mean that the computers have experienced spikes.

### Variance

To directly evaluate the variance of a value, use the standard deviation and variance methods:

```kusto
Perf
| where TimeGenerated > ago(30m) 
| where CounterName == "% Processor Time" and InstanceName == "_Total" 
| summarize stdev(CounterValue), variance(CounterValue) by Computer
```

A good way to analyze the stability of CPU usage is to combine `stdev` with the median calculation:

```kusto
Perf
| where TimeGenerated > ago(130m) 
| where CounterName == "% Processor Time" and InstanceName == "_Total" 
| summarize stdev(CounterValue), percentiles(CounterValue, 50) by Computer
```

### Generate lists and sets

You can use `makelist` to pivot data by the order of values in a specific column. For example, you might want to explore the most common order events that take place on your computers. You can essentially pivot the data by the order of `EventID` values on each computer: 

```kusto
Event
| where TimeGenerated > ago(12h)
| order by TimeGenerated desc
| summarize makelist(EventID) by Computer
```

Here's the output:

|Computer|list_EventID|
|---|---|
| computer1 | [704,701,1501,1500,1085,704,704,701] |
| computer2 | [326,105,302,301,300,102] |
| ... | ... |

`makelist` generates a list in the order that data was passed into it. To sort events from oldest to newest, use `asc` in the `order` statement instead of `desc`. 

You might find it useful to create a list only of distinct values. This list is called a _set_, and you can generate it by using the `makeset` command:

```kusto
Event
| where TimeGenerated > ago(12h)
| order by TimeGenerated desc
| summarize makeset(EventID) by Computer
```

Here's the output:

|Computer|list_EventID|
|---|---|
| computer1 | [704,701,1501,1500,1085] |
| computer2 | [326,105,302,301,300,102] |
| ... | ... |

Like `makelist`, `makeset` also works with ordered data. The `makeset` command generates arrays based on the order of the rows that are passed into it.

### Expand lists

The inverse operation of `makelist` or `makeset` is `mv-expand`. The `mv-expand` command expands a list of values to separate rows. It can expand across any number of dynamic columns, including JSON and array columns. For example, you can check the `Heartbeat` table for solutions that sent data from computers that sent a heartbeat in the past hour:

```kusto
Heartbeat
| where TimeGenerated > ago(1h)
| project Computer, Solutions
```

Here's the output:

| Computer | Solutions | 
|--------------|----------------------|
| computer1 | "security", "updates", "changeTracking" |
| computer2 | "security", "updates" |
| computer3 | "antiMalware", "changeTracking" |
| ... | ... |

Use `mv-expand` to show each value in a separate row instead of in a comma-separated list:

```kusto
Heartbeat
| where TimeGenerated > ago(1h)
| project Computer, split(Solutions, ",")
| mv-expand Solutions
```

Here's the output:

| Computer | Solutions | 
|--------------|----------------------|
| computer1 | "security" |
| computer1 | "updates" |
| computer1 | "changeTracking" |
| computer2 | "security" |
| computer2 | "updates" |
| computer3 | "antiMalware" |
| computer3 | "changeTracking" |
| ... | ... |


You can use `makelist` to group items together. In the output, you can see the list of computers per solution:

```kusto
Heartbeat
| where TimeGenerated > ago(1h)
| project Computer, split(Solutions, ",")
| mv-expand Solutions
| summarize makelist(Computer) by tostring(Solutions) 
```

Here's the output:

|Solutions | list_Computer |
|--------------|----------------------|
| "security" | ["computer1", "computer2"] |
| "updates" | ["computer1", "computer2"] |
| "changeTracking" | ["computer1", "computer3"] |
| "antiMalware" | ["computer3"] |
| ... | ... |

### Missing bins

A useful application of `mv-expand` is filling in default values for missing bins. For example, suppose you're looking for the uptime of a specific computer by exploring its heartbeat. You also want to see the source of the heartbeat, which is in the `Category` column. Normally, we would use a basic `summarize` statement:

```kusto
Heartbeat
| where TimeGenerated > ago(12h)
| summarize count() by Category, bin(TimeGenerated, 1h)
```

Here's the output:

| Category | TimeGenerated | count_ |
|--------------|----------------------|--------|
| Direct Agent | 2017-06-06T17:00:00Z | 15 |
| Direct Agent | 2017-06-06T18:00:00Z | 60 |
| Direct Agent | 2017-06-06T20:00:00Z | 55 |
| Direct Agent | 2017-06-06T21:00:00Z | 57 |
| Direct Agent | 2017-06-06T22:00:00Z | 60 |
| ... | ... | ... |

In the output, the bucket that's associated with "2017-06-06T19:00:00Z" is missing because there isn't any heartbeat data for that hour. Use the `make-series` function to assign a default value to empty buckets. A row is generated for each category. The output includes two extra array columns, one for values and one for matching time buckets:

```kusto
Heartbeat
| make-series count() default=0 on TimeGenerated in range(ago(1d), now(), 1h) by Category 
```

Here's the output:

| Category | count_ | TimeGenerated |
|---|---|---|
| Direct Agent | [15,60,0,55,60,57,60,...] | ["2017-06-06T17:00:00.0000000Z","2017-06-06T18:00:00.0000000Z","2017-06-06T19:00:00.0000000Z","2017-06-06T20:00:00.0000000Z","2017-06-06T21:00:00.0000000Z",...] |
| ... | ... | ... |

The third element of the *count_* array is 0, as expected. The _TimeGenerated_ array has a matching time stamp of "2017-06-06T19:00:00.0000000Z". But, this array format is difficult to read. Use `mv-expand` to expand the arrays and produce the same format output as generated by `summarize`:

```kusto
Heartbeat
| make-series count() default=0 on TimeGenerated in range(ago(1d), now(), 1h) by Category 
| mv-expand TimeGenerated, count_
| project Category, TimeGenerated, count_
```

Here's the output:

| Category | TimeGenerated | count_ |
|--------------|----------------------|--------|
| Direct Agent | 2017-06-06T17:00:00Z | 15 |
| Direct Agent | 2017-06-06T18:00:00Z | 60 |
| Direct Agent | 2017-06-06T19:00:00Z | 0 |
| Direct Agent | 2017-06-06T20:00:00Z | 55 |
| Direct Agent | 2017-06-06T21:00:00Z | 57 |
| Direct Agent | 2017-06-06T22:00:00Z | 60 |
| ... | ... | ... |



### Narrow results to a set of elements: *let*, *makeset*, *toscalar*, *in*

A common scenario is to select the names of specific entities based on a set of criteria, and then filter a different dataset down to that set of entities. For example, you might find computers that are known to have missing updates and identify IP addresses that these computers called out to.

Here's an example:

```kusto
let ComputersNeedingUpdate = toscalar(
    Update
    | summarize makeset(Computer)
    | project set_Computer
);
WindowsFirewall
| where Computer in (ComputersNeedingUpdate)
```

## Joins

You can use joins to analyze data from multiple tables in the same query. A join merges the rows of two datasets by matching values of specified columns.

Here's an example:

```kusto
SecurityEvent 
| where EventID == 4624		// sign-in events
| project Computer, Account, TargetLogonId, LogonTime=TimeGenerated
| join kind= inner (
    SecurityEvent 
    | where EventID == 4634		// sign-out events
    | project TargetLogonId, LogoffTime=TimeGenerated
) on TargetLogonId
| extend Duration = LogoffTime-LogonTime
| project-away TargetLogonId1 
| top 10 by Duration desc
```

In the example, the first dataset filters for all sign-in events. That dataset is joined with a second dataset that filters for all sign-out events. The projected columns are `Computer`, `Account`, `TargetLogonId`, and `TimeGenerated`. The datasets are correlated by a shared column, `TargetLogonId`. The output is a single record per correlation that has both the sign-in and sign-out time.

If both datasets have columns that have the same name, the columns of the right-side dataset are given an index number. In this example, the results would show `TargetLogonId` with values from the left-side table and `TargetLogonId1` with values from the right-side table. In this case, the second `TargetLogonId1` column was removed by using the `project-away` operator.

> [!NOTE]
> To improve performance, keep only the relevant columns of the joined datasets by using the `project` operator.


Use the following syntax to join two datasets in which the joined key has a different name between the two tables:

```
Table1
| join ( Table2 ) 
on $left.key1 == $right.key2
```

### Lookup tables

A common use of joins is to use `datatable` for static value mapping. Using `datatable` can help make results more presentable. For example, you can enrich security event data with the event name for each event ID:

```kusto
let DimTable = datatable(EventID:int, eventName:string)
  [
    4625, "Account activity",
    4688, "Process action",
    4634, "Account activity",
    4658, "The handle to an object was closed",
    4656, "A handle to an object was requested",
    4690, "An attempt was made to duplicate a handle to an object",
    4663, "An attempt was made to access an object",
    5061, "Cryptographic operation",
    5058, "Key file operation"
  ];
SecurityEvent
| join kind = inner
 DimTable on EventID
| summarize count() by eventName
```

Here's the output:

| eventName | count_ |
|:---|:---|
| The handle to an object was closed | 290,995 |
| A handle to an object was requested | 154,157 |
| An attempt was made to duplicate a handle to an object | 144,305 |
| An attempt was made to access an object | 123,669 |
| Cryptographic operation | 153,495 |
| Key file operation | 153,496 |

## JSON and data structures

Nested objects are objects that contain other objects in an array or in a map of key-value pairs. The objects are represented as JSON strings. This section describes how you can use JSON to retrieve data and analyze nested objects.

### Work with JSON strings

Use `extractjson` to access a specific JSON element in a known path. This function requires a path expression that uses the following conventions:

- Use _$_ to refer to the root folder.
- Use the bracket or dot notation to refer to indexes and elements as illustrated in the following examples.


Use brackets for indexes and dots to separate elements:

```kusto
let hosts_report='{"hosts": [{"location":"North_DC", "status":"running", "rate":5},{"location":"South_DC", "status":"stopped", "rate":3}]}';
print hosts_report
| extend status = extractjson("$.hosts[0].status", hosts_report)
```

This example is similar, but it uses only the brackets notation:

```kusto
let hosts_report='{"hosts": [{"location":"North_DC", "status":"running", "rate":5},{"location":"South_DC", "status":"stopped", "rate":3}]}';
print hosts_report 
| extend status = extractjson("$['hosts'][0]['status']", hosts_report)
```

For only one element, you can use only the dot notation:

```kusto
let hosts_report=dynamic({"location":"North_DC", "status":"running", "rate":5});
print hosts_report 
| extend status = hosts_report.status
```


### *parsejson*

It's easiest to access multiple elements in your JSON structure as a dynamic object. Use `parsejson` to cast text data to a dynamic object. After you convert the JSON to a dynamic type, you can use additional functions to analyze the data.

```kusto
let hosts_object = parsejson('{"hosts": [{"location":"North_DC", "status":"running", "rate":5},{"location":"South_DC", "status":"stopped", "rate":3}]}');
print hosts_object 
| extend status0=hosts_object.hosts[0].status, rate1=hosts_object.hosts[1].rate
```

### *arraylength*

Use `arraylength` to count the number of elements in an array:

```kusto
let hosts_object = parsejson('{"hosts": [{"location":"North_DC", "status":"running", "rate":5},{"location":"South_DC", "status":"stopped", "rate":3}]}');
print hosts_object 
| extend hosts_num=arraylength(hosts_object.hosts)
```

### *mv-expand*

Use `mv-expand` to break the properties of an object into separate rows:

```kusto
let hosts_object = parsejson('{"hosts": [{"location":"North_DC", "status":"running", "rate":5},{"location":"South_DC", "status":"stopped", "rate":3}]}');
print hosts_object 
| mv-expand hosts_object.hosts[0]
```

:::image type="content" source="images/samples/mvexpand-rows.png" alt-text="Screenshot shows hosts_0 with values for location, status, and rate.":::

### *buildschema*

Use `buildschema` to get the schema that admits all values of an object:

```kusto
let hosts_object = parsejson('{"hosts": [{"location":"North_DC", "status":"running", "rate":5},{"location":"South_DC", "status":"stopped", "rate":3}]}');
print hosts_object 
| summarize buildschema(hosts_object)
```

The result is a schema in JSON format:

```json
{
    "hosts":
    {
        "indexer":
        {
            "location": "string",
            "rate": "int",
            "status": "string"
        }
    }
}
```

The schema describes the names of the object fields and their matching data types. 

Nested objects might have different schemas, as in the following example:

```kusto
let hosts_object = parsejson('{"hosts": [{"location":"North_DC", "status":"running", "rate":5},{"status":"stopped", "rate":"3", "range":100}]}');
print hosts_object 
| summarize buildschema(hosts_object)
```

## Charts

The following sections give examples of how to work with charts when using the Kusto Query Language.

### Chart the results

Begin by reviewing the number of computers per operating system during the past hour:

```kusto
Heartbeat
| where TimeGenerated > ago(1h)
| summarize count(Computer) by OSType  
```

By default, the results display as a table:

:::image type="content" source="images/samples/query-results-table.png" alt-text="Screenshot that shows query results in a table.":::

For a more useful view, select **Chart**, and then select the **Pie** option to visualize the results:

:::image type="content" source="images/samples/query-results-pie-chart.png" alt-text="Screenshot that shows query results in a pie chart.":::

### Timecharts

Show the average and the 50th and 95th percentiles of processor time in bins of one hour. 

The following query generates multiple series. In the results, you can choose which series to show in the timechart.

```kusto
Perf
| where TimeGenerated > ago(1d) 
| where CounterName == "% Processor Time" 
| summarize avg(CounterValue), percentiles(CounterValue, 50, 95)  by bin(TimeGenerated, 1h)
```

Select the **Line** chart display option:

:::image type="content" source="images/samples/multiple-series-line-chart.png" alt-text="Screenshot that shows a multiple-series line chart.":::

#### Reference line

A reference line can help you easily identify whether the metric exceeded a specific threshold. To add a line to a chart, extend the dataset by adding a constant column:

```kusto
Perf
| where TimeGenerated > ago(1d) 
| where CounterName == "% Processor Time" 
| summarize avg(CounterValue), percentiles(CounterValue, 50, 95)  by bin(TimeGenerated, 1h)
| extend Threshold = 20
```

:::image type="content" source="images/samples/multiple-series-threshold-line-chart.png" alt-text="Screenshot that shows a multiple-series line chart with a threshold reference line.":::


### Multiple dimensions

Multiple expressions in the `by` clause of `summarize` create multiple rows in the results. One row is created for each combination of values.

```kusto
SecurityEvent
| where TimeGenerated > ago(1d)
| summarize count() by tostring(EventID), AccountType, bin(TimeGenerated, 1h)
```

When you view the results as a chart, the chart uses the first column from the `by` clause. The following example shows a stacked column chart that's created by using the `EventID` value. Dimensions must be of the `string` type. In this example, the `EventID` value is cast to `string`:

:::image type="content" source="images/samples/select-column-chart-type-eventid.png" alt-text="Screenshot that shows a bar chart based on the EventID column.":::

You can switch between columns by selecting the drop-down arrow for the column name:

:::image type="content" source="images/samples/select-column-chart-type-accounttype.png" alt-text="Screenshot that shows a bar chart based on AccountType column, with the column selector visible.":::

## Smart analytics

This section includes examples that use smart analytics functions in Azure Log Analytics to analyze user activity. You can use these examples to analyze your own applications that are monitored by Azure Application Insights, or use the concepts in these queries for similar analysis on other data. 

### Cohorts analytics

Cohort analysis tracks the activity of specific groups of users, known as _cohorts_. Cohort analytics attempts to measure how appealing a service is by measuring the rate of returning users. Users are grouped by the time they first used the service. When analyzing cohorts, we expect to find a decrease in activity over the first tracked periods. Each cohort is titled by the week its members were observed for the first time.

The following example analyzes the number of activities users completed during five weeks after their first use of the service:

```kusto
let startDate = startofweek(bin(datetime(2017-01-20T00:00:00Z), 1d));
let week = range Cohort from startDate to datetime(2017-03-01T00:00:00Z) step 7d;
// For each user, we find the first and last timestamp of activity
let FirstAndLastUserActivity = (end:datetime) 
{
    customEvents
    | where customDimensions["sourceapp"]=="ai-loganalyticsui-prod"
    // Check 30 days back to see first time activity.
    | where timestamp > startDate - 30d
    | where timestamp < end      
    | summarize min=min(timestamp), max=max(timestamp) by user_AuthenticatedId
};
let DistinctUsers = (cohortPeriod:datetime, evaluatePeriod:datetime) {
    toscalar (
    FirstAndLastUserActivity(evaluatePeriod)
    // Find members of the cohort: only users that were observed in this period for the first time.
    | where min >= cohortPeriod and min < cohortPeriod + 7d  
    // Pick only the members that were active during the evaluated period or after.
    | where max > evaluatePeriod - 7d
    | summarize dcount(user_AuthenticatedId)) 
};
week 
| where Cohort == startDate
// Finally, calculate the desired metric for each cohort. In this sample, we calculate distinct users but you can change
// this to any other metric that would measure the engagement of the cohort members.
| extend 
    r0 = DistinctUsers(startDate, startDate+7d),
    r1 = DistinctUsers(startDate, startDate+14d),
    r2 = DistinctUsers(startDate, startDate+21d),
    r3 = DistinctUsers(startDate, startDate+28d),
    r4 = DistinctUsers(startDate, startDate+35d)
| union (week | where Cohort == startDate + 7d 
| extend 
    r0 = DistinctUsers(startDate+7d, startDate+14d),
    r1 = DistinctUsers(startDate+7d, startDate+21d),
    r2 = DistinctUsers(startDate+7d, startDate+28d),
    r3 = DistinctUsers(startDate+7d, startDate+35d) )
| union (week | where Cohort == startDate + 14d 
| extend 
    r0 = DistinctUsers(startDate+14d, startDate+21d),
    r1 = DistinctUsers(startDate+14d, startDate+28d),
    r2 = DistinctUsers(startDate+14d, startDate+35d) )
| union (week | where Cohort == startDate + 21d 
| extend 
    r0 = DistinctUsers(startDate+21d, startDate+28d),
    r1 = DistinctUsers(startDate+21d, startDate+35d) ) 
| union (week | where Cohort == startDate + 28d 
| extend 
    r0 = DistinctUsers (startDate+28d, startDate+35d) )
// Calculate the retention percentage for each cohort by weeks
| project Cohort, r0, r1, r2, r3, r4,
          p0 = r0/r0*100,
          p1 = todouble(r1)/todouble (r0)*100,
          p2 = todouble(r2)/todouble(r0)*100,
          p3 = todouble(r3)/todouble(r0)*100,
          p4 = todouble(r4)/todouble(r0)*100 
| sort by Cohort asc
```

Here's the output:

:::image type="content" source="images/samples/cohorts-table.png" alt-text="Screenshot that shows a table of cohorts based on activity.":::

### Rolling monthly active users and user stickiness

The following example uses time-series analysis with the [series_fir](/azure/kusto/query/series-firfunction) function. You can use the `series_fir` function for sliding window computations. The sample application being monitored is an online store that tracks users' activity through custom events. The query tracks two types of user activities: `AddToCart` and `Checkout`. It defines an active user as a user who completed a checkout at least once on a specific day.

```kusto
let endtime = endofday(datetime(2017-03-01T00:00:00Z));
let window = 60d;
let starttime = endtime-window;
let interval = 1d;
let user_bins_to_analyze = 28;
// Create an array of filters coefficients for series_fir(). A list of '1' in our case will produce a simple sum.
let moving_sum_filter = toscalar(range x from 1 to user_bins_to_analyze step 1 | extend v=1 | summarize makelist(v)); 
// Level of engagement. Users will be counted as engaged if they completed at least this number of activities.
let min_activity = 1;
customEvents
| where timestamp > starttime  
| where customDimensions["sourceapp"] == "ai-loganalyticsui-prod"
// We want to analyze users who actually checked out in our website.
| where (name == "Checkout") and user_AuthenticatedId <> ""
// Create a series of activities per user.
| make-series UserClicks=count() default=0 on timestamp 
	in range(starttime, endtime-1s, interval) by user_AuthenticatedId
// Create a new column that contains a sliding sum. 
// Passing 'false' as the last parameter to series_fir() prevents normalization of the calculation by the size of the window.
// For each time bin in the *RollingUserClicks* column, the value is the aggregation of the user activities in the 
// 28 days that preceded the bin. For example, if a user was active once on 2016-12-31 and then inactive throughout 
// January, then the value will be 1 between 2016-12-31 -> 2017-01-28 and then 0s. 
| extend RollingUserClicks=series_fir(UserClicks, moving_sum_filter, false)
// Use the zip() operator to pack the timestamp with the user activities per time bin.
| project User_AuthenticatedId=user_AuthenticatedId , RollingUserClicksByDay=zip(timestamp, RollingUserClicks)
// Transpose the table and create a separate row for each combination of user and time bin (1 day).
| mv-expand RollingUserClicksByDay
| extend Timestamp=todatetime(RollingUserClicksByDay[0])
// Mark the users that qualify according to min_activity.
| extend RollingActiveUsersByDay=iff(toint(RollingUserClicksByDay[1]) >= min_activity, 1, 0)
// And finally, count the number of users per time bin.
| summarize sum(RollingActiveUsersByDay) by Timestamp
// First 28 days contain partial data, so we filter them out.
| where Timestamp > starttime + 28d
// Render as timechart.
| render timechart
```

Here's the output:

:::image type="content" source="images/samples/rolling-monthly-active-users-chart.png" alt-text="Screenshot of a chart that shows rolling active users by day over a month.":::

The following example turns the preceding query into a reusable function. The example then uses the query to calculate rolling user stickiness. An active user in this query is defined as a user who completed a checkout at least once on a specific day.

```kusto
let rollingDcount = (sliding_window_size: int, event_name:string)
{
    let endtime = endofday(datetime(2017-03-01T00:00:00Z));
    let window = 90d;
    let starttime = endtime-window;
    let interval = 1d;
    let moving_sum_filter = toscalar(range x from 1 to sliding_window_size step 1 | extend v=1| summarize makelist(v));    
    let min_activity = 1;
    customEvents
    | where timestamp > starttime
    | where customDimensions["sourceapp"]=="ai-loganalyticsui-prod"
    | where (name == event_name)
    | where user_AuthenticatedId <> ""
    | make-series UserClicks=count() default=0 on timestamp 
		in range(starttime, endtime-1s, interval) by user_AuthenticatedId
    | extend RollingUserClicks=fir(UserClicks, moving_sum_filter, false)
    | project User_AuthenticatedId=user_AuthenticatedId , RollingUserClicksByDay=zip(timestamp, RollingUserClicks)
    | mv-expand RollingUserClicksByDay
    | extend Timestamp=todatetime(RollingUserClicksByDay[0])
    | extend RollingActiveUsersByDay=iff(toint(RollingUserClicksByDay[1]) >= min_activity, 1, 0)
    | summarize sum(RollingActiveUsersByDay) by Timestamp
    | where Timestamp > starttime + 28d
};
// Use the moving_sum_filter with bin size of 28 to count MAU.
rollingDcount(28, "Checkout")
| join
(
    // Use the moving_sum_filter with bin size of 1 to count DAU.
    rollingDcount(1, "Checkout")
)
on Timestamp
| project sum_RollingActiveUsersByDay1 *1.0 / sum_RollingActiveUsersByDay, Timestamp
| render timechart
```

Here's the output:

:::image type="content" source="images/samples/user-stickiness-chart.png" alt-text="Screenshot of a chart that shows user stickiness over time.":::

### Regression analysis

This example demonstrates how to create an automated detector for service disruptions based exclusively on an application's trace logs. The detector seeks abnormal, sudden increases in the relative amount of error and warning traces in the application.

Two techniques are used to evaluate the service status based on trace logs data:

- Use [make-series](/azure/kusto/query/make-seriesoperator) to convert semi-structured textual trace logs into a metric that represents the ratio between positive and negative trace lines.
- Use [series_fit_2lines](/azure/kusto/query/series-fit-2linesfunction) and [series_fit_line](/azure/kusto/query/series-fit-linefunction) for advanced step-jump detection by using time-series analysis with a two-line linear regression.

```kusto
let startDate = startofday(datetime("2017-02-01"));
let endDate = startofday(datetime("2017-02-07"));
let minRsquare = 0.8;  // Tune the sensitivity of the detection sensor. Values close to 1 indicate very low sensitivity.
// Count all Good (Verbose + Info) and Bad (Error + Fatal + Warning) traces, per day.
traces
| where timestamp > startDate and timestamp < endDate
| summarize 
    Verbose = countif(severityLevel == 0),
    Info = countif(severityLevel == 1), 
    Warning = countif(severityLevel == 2),
    Error = countif(severityLevel == 3),
    Fatal = countif(severityLevel == 4) by bin(timestamp, 1d)
| extend Bad = (Error + Fatal + Warning), Good = (Verbose + Info)
// Determine the ratio of bad traces, from the total.
| extend Ratio = (todouble(Bad) / todouble(Good + Bad))*10000
| project timestamp , Ratio
// Create a time series.
| make-series RatioSeries=any(Ratio) default=0 on timestamp in range(startDate , endDate -1d, 1d) by 'TraceSeverity' 
// Apply a 2-line regression to the time series.
| extend (RSquare2, SplitIdx, Variance2,RVariance2,LineFit2)=series_fit_2lines(RatioSeries)
// Find out if our 2-line is trending up or down.
| extend (Slope,Interception,RSquare,Variance,RVariance,LineFit)=series_fit_line(LineFit2)
// Check whether the line fit reaches the threshold, and if the spike represents an increase (rather than a decrease).
| project PatternMatch = iff(RSquare2 > minRsquare and Slope>0, "Spike detected", "No Match")
```

## Next steps

- Walk through a [tutorial on the Kusto Query Language](tutorial.md?pivots=azuremonitor).


::: zone-end

