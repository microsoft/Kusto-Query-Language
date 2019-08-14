# bag_keys()

Enumerates all the root keys in a dynamic property-bag object.

`bag_keys(`*dynamic object*`)`

**Returns**

An array of keys, order is undetermined.

**Examples**

|Expression|Evaluates to|
|---|---|
|`bag_keys(dynamic({'a':'b', 'c':123}))` | `['a','c']`|
|`bag_keys(dynamic({'a':'b', 'c':{'d':123}})) `|`['a','c']`|
|`bag_keys(dynamic({'a':'b', 'c':[{'d':123}]})) `|`['a','c']`|
|`bag_keys(dynamic(null))`|`null`|
|`bag_keys(dynamic({}))`|`[]`|
|`bag_keys(dynamic('a'))`|`null`|
|`bag_keys(dynamic([]))  `|`null`|
