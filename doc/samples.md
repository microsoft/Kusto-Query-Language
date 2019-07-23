---
title: Samples - Azure Data Explorer | Microsoft Docs
description: This article describes Samples in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 03/28/2019
---
# Samples

Below are a few common query needs and how the Kusto query language can be used
to meet them.

## Display a column chart

Project two or more columns and use them as the x and y axis of a chart:

```kusto 
StormEvents
| where isnotempty(EndLocation) 
| summarize event_count=count() by EndLocation
| top 10 by event_count
| render columnchart
```

* The first column forms the x-axis. It can be numeric, datetime, or string. 
* Use `where`, `summarize` and `top` to limit the volume of data that you display.
* Sort the results so as to define the order of the x-axis.

![alt text](./Images/samples/060.png "060")

<a name="activities"></a>
## Get sessions from start and stop events

Let's suppose we have a log of events, in which some events mark the start or end of an extended activity or session. 

|Name|City|SessionId|Timestamp|
|---|---|---|---|
|Start|London|2817330|2015-12-09T10:12:02.32|
|Game|London|2817330|2015-12-09T10:12:52.45|
|Start|Manchester|4267667|2015-12-09T10:14:02.23|
|Stop|London|2817330|2015-12-09T10:23:43.18|
|Cancel|Manchester|4267667|2015-12-09T10:27:26.29|
|Stop|Manchester|4267667|2015-12-09T10:28:31.72|

Every event has an SessionId, so the problem is to match up the start and stop events with the same id.

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

Use [`let`](./letstatement.md) to name a projection of the table that is pared down as far as possible before going into the join.
[`Project`](./projectoperator.md) is used to change the names of the timestamps so that both the start and stop times can appear in the result. It also selects the other columns we want to see in the result. [`join`](./joinoperator.md)  matches up the start and stop entries for the same activity, creating a  row for each activity. Finally, `project` again adds a column to show the duration of the activity.


|City|SessionId|StartTime|StopTime|Duration|
|---|---|---|---|---|
|London|2817330|2015-12-09T10:12:02.32|2015-12-09T10:23:43.18|00:11:40.46|
|Manchester|4267667|2015-12-09T10:14:02.23|2015-12-09T10:28:31.72|00:14:29.49|

### Get sessions, without session id

Now let's suppose that the start and stop events don't conveniently have a session id that we can match on. But we do have an IP address of the client where the session took place. Assuming each client address only conducts one session at a time, we can match each start event to the next stop event from the same IP address.

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

The join will match every start time with all the stop times from the same client IP address. So we first remove matches with earlier stop times.

Then we group by start time and ip to get a group for each session. We must supply a `bin` function for the StartTime parameter: if we don't, Kusto will automatically use 1-hour bins, which will match some start times with the wrong stop times.

`arg_min` picks out the row with the smallest duration in each group, and the `*` parameter passes through all the other columns, though it prefixes "min_" to each column name. 


![alt text](./images/samples/040.png "040") 

Then we can add some code to count the durations in conveniently-sized bins. We've a slight preference for a bar chart, so we divide by `1s` to convert the timespans to numbers. 


      // Count the frequency of each duration:
    | summarize count() by duration=bin(min_duration/1s, 10) 
      // Cut off the long tail:
    | where duration < 300
      // Display in a bar chart:
    | sort by duration asc | render barchart 


![alt text](./images/samples/050.png "050") 


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

<a name="concurrent-activities"><a/>
## Chart concurrent sessions over time

Suppose we have a table of activities with their start and end times.  We'd like to see a chart over time that shows how many are running concurrently at any time.

Here's an example input, which we'll call `X`:

|SessionId | StartTime | StopTime |
|---|---|---|
| a | 10:01:03 | 10:10:08 |
| b | 10:01:29 | 10:03:10 |
| c | 10:03:02 | 10:05:20 |

We want a chart in 1-minute bins, so we want to create something that, at each 1m interval, we can count for each running activity.

Here's an intermediate result:

```kusto
X | extend samples = range(bin(StartTime, 1m), Stop, 1m)
```

`range` generates an array of values at the specified intervals:

|SessionId | StartTime | StopTime  | samples|
|---|---|---|---|
| a | 10:03:33 | 10:06:31 | [10:01:00,10:02:00,...10:06:00]|
| b | 10:02:29 | 10:03:45 |          [10:02:00,10:03:00]|
| c | 10:03:12 | 10:04:30 |                   [10:03:00,10:04:00]|

But instead of keeping those arrays, we'll expand them using [mv-expand](./mvexpandoperator.md):

```kusto
X | mv-expand samples = range(bin(StartTime, 1m), StopTime , 1m)
```

|SessionId | StartTime | StopTime  | samples|
|---|---|---|---|
| a | 10:03:33 | 10:06:31 | 10:01:00|
| a | 10:03:33 | 10:06:31 | 10:02:00|
| b | 10:02:29 | 10:03:45 | 10:02:00|
| a | 10:03:33 | 10:06:31 | 10:03:00|
| b | 10:02:29 | 10:03:45 | 10:03:00|
| c | 10:03:12 | 10:04:30 | 10:03:00|
| a | 10:03:33 | 10:06:31 | 10:04:00|
| c | 10:03:12 | 10:04:30 | 10:04:00|
|...||||

We can now group these by sample time, counting the occurrences of each activity:

```kusto
X
| mv-expand samples = range(bin(StartTime, 1m), StopTime , 1m)
| summarize count(SessionId) by bin(todatetime(samples),1m)
```

* We need todatetime() because [mv-expand](./mvexpandoperator.md) yields a column of dynamic type.
* We need bin() because, for numeric values and dates, summarize always applies a bin function with a default interval if you don't supply one. 


| count_SessionId | samples|
|---|---|
| 1 | 10:01:00|
| 2 | 10:02:00|
| 3 | 10:03:00|
| 2 | 10:04:00|
| 1 | 10:05:00|

This can be rendered as a bar chart or time chart.

## Introduce null bins into summarize

When the `summarize` operator is applied over a group key that consists of a
`datetime` column, one normally "bins" those values to fixed-width bins.
For example:

```kusto
let StartTime=ago(12h);
let StopTime=now()
T
| where Timestamp > StartTime and Timestamp <= StopTime 
| where ...
| summarize Count=count() by bin(Timestamp, 5m)
```

This operation produces a table with a single row per group of rows in `T`
that fall into each bin of five minutes. What it doesn't do is add "null bins" --
rows for time bin values between `StartTime` and `StopTime` for which there's
no corresponding row in `T`. 

Often, it is desired to "pad" the table with those bins. Here's one way to do
it:

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

Here's a step-by-step explanation of the query above: 

1. Using the `union` operator allows us to add additional rows to a table. Those
   rows are produced by the expression to `union`.
2. Using the `range` operator to produce a table having a single row/column.
   The table is not used for anything other than for `mv-expand` to work on.
3. Using the `mv-expand` operator over the `range` function to create as many
   rows as there are 5-minute bins between `StartTime` and `EndTime`.
4. All with a `Count` of `0`.
5. Last, we use the `summarize` operator to group-together bins from the original
   (left, or outer) argument to `union` and bins from the inner argument to it
   (namely, the null bin rows). This ensure that the output has one row per bin,
   whose value is either zero or the original count.  

## Get more out of your data in Kusto using Machine Learning 

There are many interesting use cases for leveraging machine learning algorithms and derive interesting insights out of telemetry data. While often these algorithms require a very structured dataset as their input, the raw log data will usually not match the required structure and size. 

Our journey starts with looking for anomalies in the error rate of a specific Bing Inferences service. The Logs table has 65B records, and the simple query below filters 250K errors, and creates a time series data of errors count that utilizes anomaly detection function[series_decompose_anomalies](series-decompose-anomaliesfunction.md). The anomalies detected by the Kusto service, and are highlighted as red dots on the time series chart.

```kusto
Logs
| where Timestamp >= datetime(2015-08-22) and Timestamp < datetime(2015-08-23) 
| where Level == "e" and Service == "Inferences.UnusualEvents_Main" 
| summarize count() by bin(Timestamp, 5min)
| render anomalychart 
```

The service identified few time buckets with suspicious error rate. I'm using Kusto to zoom into this time frame, running a query that aggregates on the ‘Message' column trying to look for the top errors. I've trimmed the relevant parts out of the entire stack trace of the message to better fit into the page. You can see that I had nice success with the top eight errors, but then reached a long tail of errors since the error message was created by a format string that contained changing data. 

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

This is where the new `reduce` operator comes to help. The `reduce` operator identified 63 different errors as originated by the same trace instrumentation point in the code, and helped me focus on additional meaningful error trace in that time window.

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
|  63|Inference System error..Microsoft.Bing.Platform.Inferences.*.*.*: * write * * * to write to the Object * BOSS *.  * * *: SocialGraph.BOSS.Reques...
|  10|ExecuteAlgorithmMethod for method 'RunCycleFromInterimData' has failed...
|  10|Inference System error..Microsoft.Bing.Platform.Inferences.Service.Managers.UserInterimDataManagerException:...
|  3|InferenceHostService call failed..System.ServiceModel.*: The * object, System.ServiceModel.Channels.*+*, * * * for * * * is * the * *...   at Syst...

Now that I have a good view into the top errors that contributed to the detected anomalies, I want to understand the impact of these errors across my system. The 'Logs' table contains additional dimensional data such as 'Component', 'Cluster', etc... The new 'autocluster' plugin can help me derive that insight with a simple query. In this example below, I can clearly see that each of the top four errors are specific to a component, and while the top three errors are specific to DB4 cluster, the fourth one happens across all clusters.

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

## Mapping values from one set to another

A common use-case is using static mapping of values that can help in adopting results into more presentable way.  
For example, consider having next table. DeviceModel  specifies a model of the device, which is not a very convenient form of referencing to the device name.  


|DeviceModel |Count 
|---|---
|iPhone5,1 |32 
|iPhone3,2 |432 
|iPhone7,2 |55 
|iPhone5,2 |66 

  
A better representation may be:  

|FriendlyName |Count 
|---|---
|iPhone 5 |32 
|iPhone 4 |432 
|iPhone 6 |55 
|iPhone5 |66 

The two approaches below demonstrate how this can be achieved.  

### Mapping using dynamic dictionary

The approach below shows how the mapping can be achieved using a dynamic dictionary and dynamic accessors.

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



### Mapping using static table

The approach below shows how the mapping can be achieved using a persistent table and join operator.
 
Create the mapping table (just once):

```kusto
.create table Devices (DeviceModel: string, FriendlyName: string) 

.ingest inline into table Devices 
    ["iPhone5,1","iPhone 5"]["iPhone3,2","iPhone 4"]["iPhone7,2","iPhone 6"]["iPhone5,2","iPhone5"]
```

Content of Devices now:

|DeviceModel |FriendlyName 
|---|---
|iPhone5,1 |iPhone 5 
|iPhone3,2 |iPhone 4 
|iPhone7,2 |iPhone 6 
|iPhone5,2 |iPhone5 


Same trick for creating test table Source:

```kusto
.create table Source (DeviceModel: string, Count: int)

.ingest inline into table Source ["iPhone5,1",32]["iPhone3,2",432]["iPhone7,2",55]["iPhone5,2",66]
```


Join and project:

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


## Creating and using query-time dimension tables

In many cases one wants to join the results of a query with some ad-hoc dimension
table that is not stored in the database. It is possible to define an expression
whose result is a table scoped to a single query by doing something like this:

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

## Retrieving the latest (by timestamp) records per identity

Suppose you have a table that includes an `id` column (identifying the entity
with which each row is associated, such as a User Id or a Node Id) and a `timestamp`
column (providing the time reference for the row), as well as other columns. Your goal
is to write a query that returns the latest 2 records for each value of the `id`
column, where "latest" is defined as "having the highest value of `timestamp`".

This can be done using the [top-nested operator](topnestedoperator.md).
First we provide the query, and then we'll explain it:

```kusto
datatable(id:string, timestamp:datetime, bla:string)           // (1)
  [
  "Barak",  datetime(2015-01-01), "1",
  "Barak",  datetime(2016-01-01), "2",
  "Barak",  datetime(2017-01-20), "3",
  "Donald", datetime(2017-01-20), "4",
  "Donald", datetime(2017-01-18), "5",
  "Donald", datetime(2017-01-19), "6"
  ]
| top-nested   of id        by dummy0=max(1),                  // (2)
  top-nested 2 of timestamp by dummy1=max(timestamp),  // (3)
  top-nested   of bla       by dummy2=max(1)                   // (4)
| project-away dummy0, dummy1, dummy2                          // (5)
```

Notes
1. The `datatable` is just a way to produce some test data for demonstration
   purposes. In reality, of course, you'd have the data here.
2. This line essentially means "return all distinct values of `id`".
3. This line then returns, for the top 2 records that maximize the `timestamp`
   column, the columns of the previous level (here, just `id`) and the column
   specified at this level (here, `timestamp`).
4. This line adds the values of the `bla` column for each of the records
   returned by the previous level. If the table has other columns of interest,
   one would repeat this line for every such column.
5. Finally, we use the [project-away operator](projectawayoperator.md)
   to remove the "extra" columns introduced by `top-nested`.

## Extending a table with some percent-of-total calculation

Often, when one has a tabular expression that includes a numeric column, it is
desireable to present that column to the user alongside its value as a percentage
of the total. For example, assume that there is some query whose value is the
following table:

|SomeSeries|SomeInt|
|----------|-------|
|Foo       |    100|
|Bar       |    200|

And you want to display this table as:

|SomeSeries|SomeInt|Pct |
|----------|-------|----|
|Foo       |    100|33.3|
|Bar       |    200|66.6|

To do so, one needs to calculate the total (sum) of the `SomeInt` column,
and then divide each value of this column by the total. It is possible to do
so for arbitrary results by giving these results a name using the
[as operator](asoperator.md):

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

## Performing aggregations over a sliding window
The following example shows how to summarize columns using a sliding window. Lets take, for example, the table below, which contains prices of fruits by timestamps. Suppose we would like to calculate the min, max and sum cost of each fruit per day, using a sliding window of 7 days. In other words, each record in the result set aggregates the past 7 days, and the result contains a record per day in the analysis period.  

Fruits table: 

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

Sliding window aggreation query (explanation is provided below query results): 

```
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

The query "stretches" (duplicates) each record in the input table throughout 7 days post its actual appearance, such that each record actually appears 7 times. As a result, when performing the aggregation per each day, the aggregation includes all records of previous 7 days.

Step-by-step explanation (numbers refer to the numbers in query inline comments):
1. Bin each record to 1d (relative to _start). 
2. Determine the end of the range per record - _bin + 7d, unless this is out of the _(start, end)_ range, in which case it is adjusted. 
3. For each record, create an array of 7 days (timestamps), starting at current record's day. 
4. mv-expand the array, thus duplicating each record to 7 records, 1 day apart from each other. 
5. Perform the aggregation function for each day. Due to #4, this actually summarizes the _past_ 7 days. 
6. Finally, since the data for the first 7d is incomplete (there's no 7d lookback period for the first 7 days), we exclude the first 7 days from the final result (they only participate in the aggregation for the 2018-10-01). 