---
title: Samples - Azure Data Explorer
description: This article describes Samples in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# Samples

Below are a few common query needs and how the Kusto query language can be used
to meet them.

## Display a column chart

Project two or more columns and use them as the x and y axis of a chart.

<!-- csl: https://help.kusto.windows.net/Samples  -->
```kusto 
StormEvents
| where isnotempty(EndLocation) 
| summarize event_count=count() by EndLocation
| top 10 by event_count
| render columnchart
```

* The first column forms the x-axis. It can be numeric, datetime, or string. 
* Use `where`, `summarize`, and `top` to limit the volume of data that you display.
* Sort the results to define the order of the x-axis.

:::image type="content" source="images/samples/060.png" alt-text="Screenshot of a column chart. The y-axis ranges from 0 to around 50. Ten colored columns depict the respective values of 10 locations.":::

## Get sessions from start and stop events

Suppose you have a log of events. Some events mark the start or end of an extended activity or session. 

|Name|City|SessionId|Timestamp|
|---|---|---|---|
|Start|London|2817330|2015-12-09T10:12:02.32|
|Game|London|2817330|2015-12-09T10:12:52.45|
|Start|Manchester|4267667|2015-12-09T10:14:02.23|
|Stop|London|2817330|2015-12-09T10:23:43.18|
|Cancel|Manchester|4267667|2015-12-09T10:27:26.29|
|Stop|Manchester|4267667|2015-12-09T10:28:31.72|

Every event has a SessionId. The problem is to match up the start and stop events with the same ID.

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

1. Use [`let`](./letstatement.md) to name a projection of the table that is pared down as far as possible before going into the join.
1. Use [`Project`](./projectoperator.md) to change the names of the timestamps so that both the start and stop times can appear in the result. 
   It also selects the other columns to see in the result. 
1. Use [`join`](./joinoperator.md) to match up the start and stop entries for the same activity, creating a row for each activity. 
1. Finally, `project` again adds a column to show the duration of the activity.


|City|SessionId|StartTime|StopTime|Duration|
|---|---|---|---|---|
|London|2817330|2015-12-09T10:12:02.32|2015-12-09T10:23:43.18|00:11:40.46|
|Manchester|4267667|2015-12-09T10:14:02.23|2015-12-09T10:28:31.72|00:14:29.49|

### Get sessions, without session ID

Suppose that the start and stop events don't conveniently have a session ID that we can match with. But we do have an IP address of the client where the session took place. Assuming each client address only conducts one session at a time, we can match each start event to the next stop event from the same IP address.

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

The join will match every start time with all the stop times from the same client IP address. 
1. Remove matches with earlier stop times.
1. Group by start time and IP to get a group for each session. 
1. Supply a `bin` function for the StartTime parameter. If you don't, Kusto will automatically use 1-hour bins that will match some start times with the wrong stop times.

`arg_min` picks out the row with the smallest duration in each group, and the `*` parameter passes through all the other columns. 
The argument prefixes "min_" to each column name. 

:::image type="content" source="images/samples/040.png" alt-text="A table listing the results, with columns for the start time, client I P, duration, city, and earliest stop for each client-start time combination."::: 

Add code to count the durations in conveniently sized bins. 
In this example, because of a preference for a bar chart, divide by `1s` to convert the timespans to numbers. 

```
    // Count the frequency of each duration:
    | summarize count() by duration=bin(min_duration/1s, 10) 
      // Cut off the long tail:
    | where duration < 300
      // Display in a bar chart:
    | sort by duration asc | render barchart 
```

:::image type="content" source="images/samples/050.png" alt-text="Column chart depicting the number of sessions with durations in specified ranges. Over 400 sessions lasted 10 seconds. Less than 100 were 290 seconds.":::

### Real example

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

Suppose you have a table of activities with their start and end times. Show a chart over time that displays how many activities are concurrently running at any time.

Here's a sample input, called `X`.

|SessionId | StartTime | StopTime |
|---|---|---|
| a | 10:01:03 | 10:10:08 |
| b | 10:01:29 | 10:03:10 |
| c | 10:03:02 | 10:05:20 |

For a chart in 1-minute bins, create something that, at each 1m interval, there's a count for each running activity.

Here's an intermediate result.

```kusto
X | extend samples = range(bin(StartTime, 1m), StopTime, 1m)
```

`range` generates an array of values at the specified intervals.

|SessionId | StartTime | StopTime  | samples|
|---|---|---|---|
| a | 10:01:33 | 10:06:31 | [10:01:00,10:02:00,...10:06:00]|
| b | 10:02:29 | 10:03:45 | [10:02:00,10:03:00]|
| c | 10:03:12 | 10:04:30 | [10:03:00,10:04:00]|

Instead of keeping those arrays, expand them by using [mv-expand](./mvexpandoperator.md).

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

Now group these by sample time, counting the occurrences of each activity.

```kusto
X
| mv-expand samples = range(bin(StartTime, 1m), StopTime , 1m)
| summarize count(SessionId) by bin(todatetime(samples),1m)
```

* Use todatetime() because [mv-expand](./mvexpandoperator.md) yields a column of dynamic type.
* Use bin() because, for numeric values and dates, summarize always applies a bin function with a default interval if you don't supply one. 


| count_SessionId | samples|
|---|---|
| 1 | 10:01:00|
| 2 | 10:02:00|
| 3 | 10:03:00|
| 2 | 10:04:00|
| 1 | 10:05:00|
| 1 | 10:06:00|

The results can be rendered as a bar chart or time chart.

## Introduce null bins into summarize

When the `summarize` operator is applied over a group key that consists of a
`datetime` column, "bin" those values to fixed-width bins.

```kusto
let StartTime=ago(12h);
let StopTime=now()
T
| where Timestamp > StartTime and Timestamp <= StopTime 
| where ...
| summarize Count=count() by bin(Timestamp, 5m)
```

The above example produces a table with a single row per group of rows in `T`
that fall into each bin of five minutes. What it doesn't do is add "null bins" --
rows for time bin values between `StartTime` and `StopTime` for which there's
no corresponding row in `T`. 

It's desirable to "pad" the table with those bins. Here's one way to do it.

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

Here's a step-by-step explanation of the above query: 

1. The `union` operator lets you add additional rows to a table. Those
   rows are produced by the `union` expression.
1. The `range` operator produces a table having a single row/column.
   The table is not used for anything other than for `mv-expand` to work on.
1. The `mv-expand` operator over the `range` function creates as many
   rows as there are 5-minute bins between `StartTime` and `EndTime`.
1. Use a `Count` of `0`.
1. The `summarize` operator groups together bins from the original
   (left, or outer) argument to `union`. The operator also bins from the inner argument to it
   (the null bin rows). This process ensures that the output has one row per bin,
   whose value is either zero or the original count.  

## Get more out of your data in Kusto with Machine Learning 

There are many interesting use cases that leverage machine learning algorithms and derive interesting insights out of telemetry data. 
Often, these algorithms require a very structured dataset as their input. The raw log data will usually not match the required structure and size. 

Start by looking for anomalies in the error rate of a specific Bing Inferences service. The logs table has 65B records. 
The simple query below filters 250K errors, and creates a time series data of errors count that uses the anomaly detection function 
[series_decompose_anomalies](series-decompose-anomaliesfunction.md). The anomalies are detected by the Kusto service, and are highlighted as red dots on the time series chart.

```kusto
Logs
| where Timestamp >= datetime(2015-08-22) and Timestamp < datetime(2015-08-23) 
| where Level == "e" and Service == "Inferences.UnusualEvents_Main" 
| summarize count() by bin(Timestamp, 5min)
| render anomalychart 
```

The service identified few time buckets with suspicious error rate. Use Kusto to zoom into this time frame, and run a query that 
aggregates on the ‘Message' column. Try to find the top errors. 

The relevant parts of the entire stack trace of the message are trimmed out to better fit onto the page. 

You can see the successful identification of the top eight errors. However, there follows a long series of errors, since the error message 
was created by a format string that contained changing data. 

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

This is where the `reduce` operator helps. 
The operator identified 63 different errors that originated by the same trace instrumentation point in the code, 
and helps focus on additional meaningful error traces in that time window.

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
|  5112|Unexpected Inference System error..System.NullReferenceException: Object reference not set to an instance of an object..
|  174|InferenceHostService call failed..System.ServiceModel.CommunicationException: There was an error writing to the pipe:...
|  63|Inference System error..Microsoft.Bing.Platform.Inferences.\*: Write \* to write to the Object BOSS.\*: SocialGraph.BOSS.Reques...
|  10|ExecuteAlgorithmMethod for method 'RunCycleFromInterimData' has failed...
|  10|Inference System error..Microsoft.Bing.Platform.Inferences.Service.Managers.UserInterimDataManagerException:...
|  3|InferenceHostService call failed..System.ServiceModel.\*: The object, System.ServiceModel.Channels.\*+\*, for \*\* is the \*...   at Syst...

Now you have a good view into the top errors that contributed to the detected anomalies.

To understand the impact of these errors across the sample system: 
* The 'Logs' table contains additional dimensional data such as 'Component', 'Cluster', and so on.
* The new 'autocluster' plugin can help derive that insight with a simple query. 
* In the example below, you can clearly see that each of the top four errors is specific to a component. Also, while the top three errors are specific to DB4 cluster, 
the fourth one happens across all clusters.

```kusto
Logs
| where Timestamp >= datetime(2015-08-22 05:00) and Timestamp < datetime(2015-08-22 06:00)
| where Level == "e" and Service == "Inferences.UnusualEvents_Main"
| evaluate autocluster()
```

|Count |Percent (%)|Component|Cluster|Message
|---|---|---|---|---
|7125|26.64|InferenceHostService|DB4|ExecuteAlgorithmMethod for method ....
|7125|26.64|Unknown Component|DB4|InferenceHostService call failed....
|7124|26.64|InferenceAlgorithmExecutor|DB4|Unexpected Inference System error...
|5112|19.11|InferenceAlgorithmExecutor|*|Unexpected Inference System error... 

## Map values from one set to another

A common use case is static mapping of values, which can help in make results more presentable.
For example, consider the next table. 
`DeviceModel` specifies a model of the device, which is not a very convenient form of referencing the device name.  


|DeviceModel |Count 
|---|---
|iPhone5,1 |32 
|iPhone3,2 |432 
|iPhone7,2 |55 
|iPhone5,2 |66 

  
The following is a better representation.  

|FriendlyName |Count 
|---|---
|iPhone 5 |32 
|iPhone 4 |432 
|iPhone 6 |55 
|iPhone5 |66 

The two approaches below demonstrate how the representation can be achieved.  

### Mapping using dynamic dictionary

The approach shows mapping with a dynamic dictionary and dynamic accessors.

<!-- csl: https://help.kusto.windows.net:443/Samples -->
```kusto
// Data set definition
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

### Map using static table

The approach shows mapping with a persistent table and join operator.
 
Create the mapping table, just once.

```kusto
.create table Devices (DeviceModel: string, FriendlyName: string) 

.ingest inline into table Devices 
    ["iPhone5,1","iPhone 5"]["iPhone3,2","iPhone 4"]["iPhone7,2","iPhone 6"]["iPhone5,2","iPhone5"]
```

Content of Devices now.

|DeviceModel |FriendlyName 
|---|---
|iPhone5,1 |iPhone 5 
|iPhone3,2 |iPhone 4 
|iPhone7,2 |iPhone 6 
|iPhone5,2 |iPhone5 

Use the same trick for creating a test table source.

```kusto
.create table Source (DeviceModel: string, Count: int)

.ingest inline into table Source ["iPhone5,1",32]["iPhone3,2",432]["iPhone7,2",55]["iPhone5,2",66]
```

Join and project.

```kusto
Devices  
| join (Source) on DeviceModel  
| project FriendlyName, Count
```

Result:

|FriendlyName |Count 
|---|---
|iPhone 5 |32 
|iPhone 4 |432 
|iPhone 6 |55 
|iPhone5 |66 


## Create and use query-time dimension tables

You will often want to join the results of a query with some ad-hoc dimension
table that is not stored in the database. It's possible to define an expression
whose result is a table scoped to a single query. 
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

Here's a slightly more complex example.

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
* an `ID` column that identifies the entity with which each row is associated, such as a User ID or a Node ID
* a `timestamp` column that provides the time reference for the row
* other columns

A query that returns the latest two records for each value of the `ID` column, where "latest" is defined as "having the highest value of `timestamp`" can be made with the [top-nested operator](topnestedoperator.md).

For example:

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

Notes 
Numbering below refers to numbers in the code sample, far right.

1. The `datatable` is a way to produce some test data for demonstration
   purposes. Normally, you'd use real data here.
1. This line essentially means "return all distinct values of `id`".
1. This line then returns, for the top two records that maximize:
     * the `timestamp` column
     * the columns of the previous level (here, just `id`)
     * the column specified at this level (here, `timestamp`)
1. This line adds the values of the `bla` column for each of the records
   returned by the previous level. If the table has other columns of interest,
   you can repeat this line for every such column.
1. This final line uses the [project-away operator](projectawayoperator.md)
   to remove the "extra" columns introduced by `top-nested`.

## Extend a table with some percent-of-total calculation

A tabular expression that includes a numeric column, is more useful to the user when it is accompanied, alongside, with its value as a percentage of the total. 
For example, assume that there is a query that produces the following table:

|SomeSeries|SomeInt|
|----------|-------|
|Foo       |    100|
|Bar       |    200|

If you want to display this table as:

|SomeSeries|SomeInt|Pct |
|----------|-------|----|
|Foo       |    100|33.3|
|Bar       |    200|66.6|

Then you need to calculate the total (sum) of the `SomeInt` column,
and then divide each value of this column by the total. 
For arbitrary results use the [as operator](asoperator.md).

```kusto
// The following table literal represents a long calculation
// that ends up with an anonymous tabular value:
datatable (SomeInt:int, SomeSeries:string) [
  100, "Foo",
  200, "Bar",
]
// We now give this calculation a name ("X"):
| as X
// Having this name we can refer to it in the sub-expression
// "X | summarize sum(SomeInt)":
| extend Pct = 100 * bin(todouble(SomeInt) / toscalar(X | summarize sum(SomeInt)), 0.001)
```

## Perform aggregations over a sliding window

The following example shows how to summarize columns using a sliding window.
Use the table below, which contains prices of fruits by timestamps. 
Calculate the min, max, and sum costs of each fruit per day, using a sliding window of seven days. Each record in the result set aggregates the previous seven days, and the result contains a record per day in the analysis period.  

The fruits table:

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

The sliding window aggregation query.
An explanation follows the query results:

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

Query details:

The query "stretches" (duplicates) each record in the input table throughout the seven days after its actual appearance. 
Each record actually appears seven times.
As a result, the daily aggregation includes all records of the previous seven days.

Step-by-step explanation 
Numbering below refers to numbers in the code sample, far right:
1. Bin each record to one day (relative to _start). 
2. Determine the end of the range per record - _bin + 7d, unless this is out of the _(start, end)_ range, in which case it is adjusted. 
3. For each record, create an array of seven days (timestamps), starting at the current record's day. 
4. mv-expand the array, thus duplicating each record to seven records, 1 day apart from each other. 
5. Perform the aggregation function for each day. Due to #4, this actually summarizes the _past_ seven days. 
6. The data for the first seven days is incomplete. There's no 7d lookback period for the first seven days. The first seven days are excluded from the final result. In the example, they only participate in the aggregation for 2018-10-01.

## Find preceding event
The next example demonstrates how to find a preceding event between 2 data sets.  

*Purpose:*: There are two data sets, A and B. For each record in B find its preceding event in A (that is, the arg_max record in A that is still “older” than B). 
Below is the expected output for the following sample data sets. 

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

There are two different approaches suggested for this problem. You should test both on your specific data set, to find the one most suitable for you.

> [!NOTE] 
> Each method may run differently on different data sets.

### Suggestion #1

This suggestion serializes both data sets by ID and timestamp, then groups all B events with all their preceding A events, and picks the `arg_max` out of all the As in the group.

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

### Suggestion #2

This suggestion requires a max-lookback-period (how much “older” the record in A may be, when compared to B. The method then joins the two data sets on ID and this lookback period. 
The join produces all possible candidates, all A records which are older than B and within the lookback period, and then the closest one to B is filtered by arg_min(TimestampB – TimestampA). The shorter the lookback period is, the better the query results will be.

In the example below, the lookback period is set to 1m, and the record with ID 'z' does not have a corresponding 'A' event, since its 'A' is older by 2m.

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

|Id|B_Timestamp|A_Timestamp|EventB|EventA|
|---|---|---|---|---|
|x|2019-01-01 00:00:03.0000000|2019-01-01 00:00:01.0000000|B|Ax2|
|x|2019-01-01 00:00:04.0000000|2019-01-01 00:00:01.0000000|B|Ax2|
|y|2019-01-01 00:00:04.0000000|2019-01-01 00:00:02.0000000|B|Ay1|
|z|2019-01-01 00:02:00.0000000||B||
