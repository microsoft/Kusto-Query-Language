# todynamic(), toobject()

Interprets a `string` as a [JSON value](https://json.org/) and returns the value as [`dynamic`](./scalar-data-types/dynamic.md). 

It is superior to using [extractjson() function](./extractjsonfunction.md)
when you need to extract more than one element of a JSON compound object.

Aliases to [parse_json()](./parsejsonfunction.md) function.

**Syntax**

`todynamic(`*json*`)`
`toobject(`*json*`)`

**Arguments**

* *json*: A JSON document.

**Returns**

An object of type `dynamic` specified by *json*.

*Note*: Prefer using [dynamic()](./scalar-data-types/dynamic.md) when possible.
