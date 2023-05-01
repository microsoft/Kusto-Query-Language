---
title: activity_engagement plugin - Azure Data Explorer
description: Learn how to use the activity_engagement plugin to calculate activity engagement ratios.
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/20/2022
---
# activity_engagement plugin

Calculates activity engagement ratio based on ID column over a sliding timeline window.

The activity_engagement plugin can be used for calculating DAU/WAU/MAU (daily/weekly/monthly activities).

## Syntax

*T* `| evaluate` `activity_engagement(`*IdColumn*`,` *TimelineColumn*`,` [*Start*`,` *End*`,`] *InnerActivityWindow*`,` *OuterActivityWindow* [`,` *dim1*`,` *dim2*`,` ...]`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *T* | string | &check; | The tabular input used to calculate engagement. |
| *IdCoumn* | string | &check; | The name of the column with ID values that represent user activity. |
| *TimelineColumn* | string | &check; | The name of the column that represents timeline. |
| *Start* | datetime |  | The analysis start period. |
| *End* | datetime |  | The analysis end period. |
| *InnerActivityWindow* | timespan | &check; | The inner-scope analysis window period. |
| *OuterActivityWindow* | timespan | &check; | The outer-scope analysis window period. |
| *dim1*, *dim2*, ... | dynamic |  | An array of the dimensions columns that slice the activity metrics calculation. |

## Returns

Returns a table that has a distinct count of ID values inside an inner-scope window, inside an outer-scope window, and the activity ratio for each inner-scope window period for each existing dimensions combination.

Output table schema is:

|TimelineColumn|dcount_activities_inner|dcount_activities_outer|activity_ratio|dim1|..|dim_n|
|---|---|---|---|--|--|--|--|--|--|
|type: as of *TimelineColumn*|long|long|double|..|..|..|

## Examples

### DAU/WAU calculation

The following example calculates DAU/WAU (Daily Active Users / Weekly Active Users ratio) over a randomly generated data.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA21RQWrDMBC8B/KHvUVKbGy1hByKD6GBviDkUIoR1sZVsS0jr0MCfXxXiigp1NgCrWdmZ3aLAt5wQK8JwevBuB6MJg3uDPOEHnRD9mLJ4rRcdEhQT6Q9QRVQSLZH8VSqXV4qfuVLwuBg/kM8RwR3aRFqo29w9twuKZK782AiHEGZ5eIb8EqhFMTIdW5ohYi8PJFkoYx8AHoGhhBCbtRDuQ5Jpio2FknIrLfl2ssM/tw3quQzV1xXEoJEf8nxOrImWFPdhYJVuo3oziJQJXS2twRMDc9yURTwqrtm7sJMD/tjcdof2RZZFz1ddDeHP2myN07d6hZ7HEi8r6xZfWRxOlkKmcW5sCH+dve4o3df2FCCHfRcn/Rc/QrGXmv2ExN4ZvMiwyaaT9b7AZnMt5byAQAA" target="_blank">Run the query</a>

```kusto
// Generate random data of user activities
let _start = datetime(2017-01-01);
let _end = datetime(2017-01-31);
range _day from _start to _end  step 1d
| extend d = tolong((_day - _start)/1d)
| extend r = rand()+1
| extend _users=range(tolong(d*50*r), tolong(d*50*r+100*r-1), 1) 
| mv-expand id=_users to typeof(long) take 1000000
// Calculate DAU/WAU ratio
| evaluate activity_engagement(['id'], _day, _start, _end, 1d, 7d)
| project _day, Dau_Wau=activity_ratio*100 
| render timechart 
```

:::image type="content" source="images/activity-engagement-plugin/activity-engagement-dau-wau.png" border="false" alt-text="Graph displaying the ratio of daily active users to weekly active users as specified in the query.":::

### DAU/MAU calculation

The following example calculates DAU/WAU (Daily Active Users / Weekly Active Users ratio) over a randomly generated data.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA2VRwWrDMAy9F/oPutVuExKvlB1GDmWFnXrsaYxgYjXzSOLgKKWFffxk14yOhcRg5b2n96SigDcc0GtC8HowrgejSYM7wzyhB92QvViyOC0XHRLUE2lPUAUUku1RPJXqOS8Vv/IlYXAw/xG7fBsR3KVFqI2+wdlzu6RI7s6DiXAEZZaLb8ArhVIQI9e5oRUi8vJEkoUy8gHoGRhCCLlRD+U6JJmq2FgkIbPelWsvM/hz36iSz1xxXUkIEv0lx+vImmBNdRcKVuk2ojuLQJXQ2d4SMDU8y0VRwKvumrkLMz3sT8Vxf2JbZF30dNHdHP6kyd44datb7HEg8b6yZvWRxelkKWQW58KG+NuW97yjd1/YUMId9Fwf9Vz9KsZmazYUI3im8ybDKppPFvwB0tS5hPMBAAA=" target="_blank">Run the query</a>

```kusto
// Generate random data of user activities
let _start = datetime(2017-01-01);
let _end = datetime(2017-05-31);
range _day from _start to _end  step 1d
| extend d = tolong((_day - _start)/1d)
| extend r = rand()+1
| extend _users=range(tolong(d*50*r), tolong(d*50*r+100*r-1), 1) 
| mv-expand id=_users to typeof(long) take 1000000
// Calculate DAU/MAU ratio
| evaluate activity_engagement(['id'], _day, _start, _end, 1d, 30d)
| project _day, Dau_Mau=activity_ratio*100 
| render timechart 
```

:::image type="content" source="images/activity-engagement-plugin/activity-engagement-dau-mau.png" border="false" alt-text="Graph displaying the ratio of daily active users to monthly active users as specified in the query.":::

### DAU/MAU calculation with additional dimensions

The following example calculates DAU/WAU (Daily Active Users / Weekly Active Users ratio) over a randomly generated data with additional dimension (`mod3`).

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA2VRwWrDMAy9D/oPojBqtwmJF8oOI4eywk499jRGMLGaeSRxcJTSwj5+shtGx0JisPL03tNTlsEb9ug1IXjdG9eB0aTBnWAa0YOuyZ4tWRwXDy0SVCNpT1AGFJLtUDzl6jnNFb/yZcZgb/4jtmkREazSIFRGX+HkWW5mJHfrg5FwAGUWD9+AFwqlQEaudX0jROxL5yaZKSPvgJ6BYQghN+quXIVJxjIKi5nIrLf52ssE/tw3KuczVVxXEgJFd07xMjAnWFPeiIJVug7oTiK0SmhtZwm4NTx3up0zBTsaydeaxDJcy2XCRPAIBfvOMnjVbT21Ifz97pgddkf2T9ZFkrNup/BnXsGV42l0gx32JN5X1qw+khhjMqeRxADZOX9FzkcQjPEM3n1hTTN6r6fqoKfylzdKrtn/rSXO7ZmK1x/2V38y+Q8r5V3jKAIAAA==" target="_blank">Run the query</a>

```kusto
// Generate random data of user activities
let _start = datetime(2017-01-01);
let _end = datetime(2017-05-31);
range _day from _start to _end  step 1d
| extend d = tolong((_day - _start)/1d)
| extend r = rand()+1
| extend _users=range(tolong(d*50*r), tolong(d*50*r+100*r-1), 1) 
| mv-expand id=_users to typeof(long) take 1000000
| extend mod3 = strcat("mod3=", id % 3)
// Calculate DAU/MAU ratio
| evaluate activity_engagement(['id'], _day, _start, _end, 1d, 30d, mod3)
| project _day, Dau_Mau=activity_ratio*100, mod3 
| render timechart 
```

:::image type="content" source="images/activity-engagement-plugin/activity-engagement-dau-mau-mod3.png" border="false" alt-text="Graph displaying the ratio of daily active users to monthly active users with modulo 3 as specified in the query.":::
