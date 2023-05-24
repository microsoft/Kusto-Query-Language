---
title:  infer_storage_schema plugin
description: Learn how to use the infer_storage_schema plugin to infer the schema of external data. 
ms.reviewer: alexans
ms.topic: reference
ms.date: 03/08/2023
---
# infer_storage_schema plugin

This plugin infers schema of external data, and returns it as CSL schema string. The string can be used when [creating external tables](../management/external-tables-azurestorage-azuredatalake.md). The plugin is invoked with the [`evaluate`](evaluateoperator.md) operator.

## Authentication and authorization

In the [properties of the request](#properties-of-the-request), you specify storage connection strings to access. Each storage connection string specifies the authorization method to use for access to the storage. Depending on the authorization method, the principal may need to be granted permissions on the external storage to perform the schema inference.

The following table lists the supported authentication methods and any required permissions by storage type.

|Authentication method|Azure Blob Storage / Data Lake Storage Gen2|Data Lake Storage Gen1|
|--|--|--|
|[Impersonation](../api/connection-strings/storage-authentication-methods.md#impersonation)|Storage Blob Data Reader|Reader|
|[Shared Access (SAS) token](../api/connection-strings/storage-authentication-methods.md#shared-access-sas-token)|List + Read|This authentication method isn't supported in Gen1.|
|[Azure AD access token](../api/connection-strings/storage-authentication-methods.md#azure-ad-access-token)||
|[Storage account access key](../api/connection-strings/storage-authentication-methods.md#storage-account-access-key)||This authentication method isn't supported in Gen1.|

## Syntax

`evaluate` `infer_storage_schema(` *Options* `)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *Options* | dynamic | &check; |A property bag specifying the [properties of the request](#properties-of-the-request).|

### Properties of the request

| Name | Type | Required | Description |
|--|--|--|--|
|*StorageContainers*| dynamic |&check;|An array of [storage connection strings](../api/connection-strings/storage-connection-strings.md) that represent prefix URI for stored data artifacts.|
|*DataFormat*|string|&check;|One of the supported [data formats](../../ingestion-supported-formats.md).|
|*FileExtension*|string||If specified, the function will only scan files ending with this file extension. Specifying the extension may speed up the process or eliminate data reading issues.|
|*FileNamePrefix*|string||If specified, the function will only scan files starting with this prefix. Specifying the prefix may speed up the process.|
|*Mode*|string||The schema inference strategy. A value of: `any`, `last`, `all`. The function infers the data schema from the first found file, from the last written file, or from all files respectively. The default value is `last`.|

## Returns

The `infer_storage_schema` plugin returns a single result table containing a single row/column holding CSL schema string.

> [!NOTE]
>
> * Storage container URI secret keys must have the permissions for *List* in addition to *Read*.
> * Schema inference strategy 'all' is a very "expensive" operation, as it implies reading from *all* artifacts found and merging their schema.
> * Some returned types may not be the actual ones as a result of wrong type guess (or, as a result of schema merge process). This is why you should review the result carefully before creating an external table.

## Example

```kusto
let options = dynamic({
  'StorageContainers': [
    h@'https://storageaccount.blob.core.windows.net/MobileEvents;secretKey'
  ],
  'FileExtension': '.parquet',
  'FileNamePrefix': 'part-',
  'DataFormat': 'parquet'
});
evaluate infer_storage_schema(options)
```

**Output**

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
