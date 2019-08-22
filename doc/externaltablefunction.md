# external_table()

References an external table by name.

<!--- csl --->
```
external_table('StormEvent')
```

**Syntax**

`external_table` `(` *TableName* [`,` *MappingName* ] `)`

**Arguments**

* *TableName*: The name of the external table being queried.
  Must be a string literal referencing an external table of kind
  `blob` or `adl`. <!-- TODO: Document data formats supported -->

* *MappingName*: An optional name of the mapping object that maps the
  fields in the actual (external) data shards to the columns output
  by this function.

**Notes**

See [external tables](schema-entities/externaltables.md) for more information
on external tables.

See also [commands for managing external tables](../management/externaltables.md).

The `external_table` function has similar restrictions
as the [table](tablefunction.md) function.
