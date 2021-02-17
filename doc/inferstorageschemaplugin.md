---
title: infer_storage_schema plugin - Azure Data Explorer
description: This article describes infer_storage_schema plugin in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 03/24/2020
---
# infer_storage_schema plugin

This plug-in infers schema of external data, and returns it as CSL schema string. The string can be used when [creating external tables](../management/external-tables-azurestorage-azuredatalake.md#create-or-alter-external-table).

```kusto
let options = dynamic({
  'StorageContainers': [
    h@'https://storageaccount.blob.core.windows.net/container1;secretKey'
  ],
  'DataFormat': 'parquet',
  'FileExtension': '.parquet'
});
evaluate infer_storage_schema(options)
```

## Syntax

`evaluate` `infer_storage_schema(` *Options* `)`

## Arguments

A single *Options* argument is a constant value of type `dynamic` that holds
a property bag specifying properties of the request:

|Name                    |Required|Description|
|------------------------|--------|-----------|
|`StorageContainers`|Yes|List of [storage connection strings](../api/connection-strings/storage.md) that represent prefix URI for stored data artifacts|
|`DataFormat`|Yes|One of supported [data formats](../../ingestion-supported-formats.md).|
|`FileExtension`|No|Only scan files ending with this file extension. It's not required, but specifying it may speed up the process (or eliminate data reading issues)|
|`FileNamePrefix`|No|Only scan files starting with this prefix. It's not required, but specifying it may speed up the process|
|`Mode`|No|Schema inference strategy, one of: `any`, `last`, `all`. Infer data schema from any (first found) file, from last written file, or from all files respectively. The default value is `last`.|

## Returns

The `infer_storage_schema` plugin returns a single result table containing a single row/column holding CSL schema string.

> [!NOTE]
> * Storage container URI secret keys must have the permissions for *List* in addition to *Read*.
> * Schema inference strategy 'all' is a very "expensive" operation, as it implies reading from *all* artifacts found and merging their schema.
> * Some returned types may not be the actual ones as a result of wrong type guess (or, as a result of schema merge process). This is why you should review the result carefully before creating an external table.

## Example

```kusto
let options = dynamic({
  'StorageContainers': [
    h@'https://storageaccount.blob.core.windows.net/MovileEvents;secretKey'
  ],
  'FileExtension': '.parquet',
  'FileNamePrefix': 'part-',
  'DataFormat': 'parquet'
});
evaluate infer_storage_schema(options)
```

*Result*

|CslSchema|
|---|
|app_id:string, user_id:long, event_time:datetime, country:string, city:string, device_type:string, device_vendor:string, ad_network:string, campaign:string, site_id:string, event_type:string, event_name:string, organic:string, days_from_install:int, revenue:real|

Use the returned schema in external table definition:

```kusto
.create external table MobileEvents(
    app_id:string, user_id:long, event_time:datetime, country:string, city:string, device_type:string, device_vendor:string, ad_network:string, campaign:string, site_id:string, event_type:string, event_name:string, organic:string, days_from_install:int, revenue:real
)
kind=blob
partition by (dt:datetime = bin(event_time, 1d), app:string = app_id)
pathformat = ('app=' app '/dt=' datetime_pattern('yyyyMMdd', dt))
dataformat = parquet
(
    h@'https://storageaccount.blob.core.windows.net/MovileEvents;secretKey'
)
```