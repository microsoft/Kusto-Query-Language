# current_cluster_endpoint()

Returns the network endpoint (DNS name) of the current cluster being queried.

**Syntax**

`current_cluster_endpoint()`

**Returns**

The network endpoint (DNS name) of the current cluster being queried,
as a value of type `string`.

**Example**

<!-- csl -->
```
print strcat("This query executed on: ", current_cluster_endpoint())
```
