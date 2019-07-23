---
title: User-Defined Functions - Azure Data Explorer | Microsoft Docs
description: This article describes User-Defined Functions in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 06/24/2019
---
# User-Defined Functions

Kusto supports user-defined functions, which are:
* part of the query itself (**ad-hoc functions**) or 
* stored in a persistent manner as part of the database metadata (**stored functions**).

A user-defined function has:
* A name
	* Must follow the [identifier naming rules](../schema-entities/entity-names.md#identifier-naming-rules).
	* Must be unique in the scope of the definition.
* A strongly-typed list of input parameters
	* May be scalar or tabular expressions.
	* Uniquely named with a type specification.
	* Scalar parameters may be provided with a default value, used implicitly when the function's caller does not provide a value for the parameter.
* A strongly-typed return value may be scalar or tabular.

A function's inputs and output determine how and where it can be used:

* **Scalar function**: 
	* A function with no inputs, or scalar inputs only, and producing a scalar output
	* Used wherever a scalar expression is allowed. 
	* May only use the row context in which it is defined, and can only refer to tables (and views) that are in the accessible schema.
* **Tabular function**: 
	* A function with no inputs, or at least one tabular input, and producing a tabular output.
	* Used wherever a tabular expression is allowed. 

> [!NOTE]
> All tabular parameters must appear before scalar parameters.

Example of scalar function:

```kusto
let Add7 = (arg0:long = 5) { arg0 + 7 };
range x from 1 to 10 step 1
| extend x_plus_7 = Add7(x), five_plus_seven = Add7()
```

Example of tabular function that takes no arguments:

```kusto
let tenNumbers = () { range x from 1 to 10 step 1};
tenNumbers
| extend x_plus_7 = x + 7
```

Example of a tabular function that uses a tabular input and a scalar input (all tabular parameters must appear before scalar parameters):

```kusto
let MyFilter = (T:(x:long), v:long) {
  T | where x >= v 
};
MyFilter((range x from 1 to 10 step 1), 9)
```

|x|
|---|
|9|
|10|

Example of a tabular function that uses a tabular input with no column specified. Any table can be passed to a function, and no table's column can be referenced inside the function.

```kusto
let MyDistinct = (T:(*)) {
  T | distinct * 
};
MyDistinct((range x from 1 to 3 step 1))
```

|x|
|---|
|1|
|2|
|3|

## Declaring user-defined functions

The declaration of a user-defined function provides:

* Function **name**
* Function **schema** (parameters it accepts, if any)
* Function **body**

> [!TIP]
> Lambda functions do not have a name and are bound to a name using a [let statement](../letstatement.md). Therefore, they can be regarded as user-defined stored functions.
> Example: Declaration for a lambda that accepts two arguments (a `string` called `s` and a `long` called`i`). It returns the product of the first (after converting it into a number) and the second. The lambda is bound to the name `f`:

```kusto
let f=(s:string, i:long) {
    tolong(s) * i
};
```

The function **body** includes:

* Exactly one expression, which provides the function's
   return value (scalar or tabular value).
* Any number (zero or more) of [let statements](../letstatement.md),
   whose scope is that of the function body. If specified,
   they must precede the expression defining the function's return value.
* Any number (zero or more) of [query parameters statements](../queryparametersstatement.md),
   which declare query parameters used by the function. If specified,
   they must precede the expression defining the function's return value.

> [!NOTE]
> Other kinds of [query statements](../statements.md) which are supported
> at the query "top level", are not supported inside a function body.

### Examples of user-definted functions


**Example: User-defined function that uses a let statement**

The following example binds the name `Test` to a user-defined
function (lambda) that makes use of three let
statements. The output is `70`:

```kusto
let Test1 = (id: int) {
  let Test2 = 10;
  let Test3 = 10 + Test2 + id;
  let Test4 = (arg: int) {
      let Test5 = 20;
      Test2 + Test3 + Test5 + arg
  };
  Test4(10)
};
range x from 1 to Test1(10) step 1
| count
```

**Example: User-defined function that defines a default value for a parameter**

The following example shows a function that accepts three arguments, the latter
two have a default value and do not have to be present at the call site.

```kusto
let f = (a:long, b:string = "b.default", c:long = 0) {
  strcat(a, "-", b, "-", c)
};
print f(12, c=7) // Returns "12-b.default-7"
```

## Invoking a user-defined function

A user-defined function that takes no arguments can be invoked by its
name, or by its name with an empty argument list in parentheses. For example:

```kusto
// Bind the identifier a to a user-defined function (lambda) that takes
// no arguments and returns a constant of type long:
let a=(){123};
// Invoke the function in two equivalent ways:
range x from 1 to 10 step 1
| extend y = x * a, z = x * a() 
```

```kusto
// Bind the identifier T to a user-defined function (lambda) that takes
// no arguments and returns a random two-by-two table:
let T=(){
  range x from 1 to 2 step 1
  | project x1 = rand(), x2 = rand()
};
// Invoke the function in two equivalent ways:
// (Note that the second invocation must be itself wrapped in
// an additional set of parentheses, as the union operator
// differentiates between "plain" names and expressions)
union T, (T())
```

A user-defined function that takes one or more scalar arguments can be invoked
by using the table name and a concrete argument list in parentheses:

```kusto
let f=(a:string, b:string) {
  strcat(a, " (la la la)", b)
};
print f("hello", "world")
```

A user-defined function that takes one or more table arguments (and any number
of scalar arguments) can be invoked using the table name and a concrete argument list
in parentheses:

```
let MyFilter = (T:(x:long), v:long) {
  T | where x >= v 
};
MyFilter((range x from 1 to 10 step 1), 9)
```

The operator `invoke` can also be used to invoke a user-defined function that
takes one or more table arguments and returns a table. This is useful when
the first concrete table argument to the function is the source of the `invoke`
operator:

```kusto
let append_to_column_a=(T:(a:string), what:string) {
    T | extend a=strcat(a, " ", what)
};
datatable (a:string) ["sad", "really", "sad"]
| invoke append_to_column_a(":-)")
```

## Default values

Functions may provide default values to some of their parameters. The following
rules apply:

* Default values may be provided for scalar parameters only.
* Default values are always literals (constants). They cannot be arbitrary
   calculations.
* Parameters with no default value always precede parameters
   that do have a default value.
* Callers must provide the value of all parameters with no default values
   arranged in the same order as the function declaration.
* Callers do not need to provide the value for parameters with default values,
   but may do so.
* Callers may provide arguments in an order that does not match the parameters
   order. If so, they must name their arguments.

The following example returns a table with two identical records; Note that
in the first invocation of `f`, the arguments are completely "scrambled", and
therefore each one is explicitly given a name:

```kusto
let f = (a:long, b:string = "b.default", c:long = 0) {
  strcat(a, "-", b, "-", c)
};
union
  (print x=f(c=7, a=12)), // "12-b.default-7"
  (print x=f(12, c=7))    // "12-b.default-7"
```

|x|
|---|
|12-b.default-7|
|12-b.default-7|

## View functions

User-defined functions that take no arguments and return a tabular expression
can be marked as views. Marking a user-defined function as a **view** means
that the function behaves like a table whenever wildcard table name resolution
is done. The following example shows two user-defined functions, `T_view` and
`T_notview`, and shows how only the first one is resolved by the wildcard
reference in the `union`:

```kusto
let T_view = view () { print x=1 };
let T_notview = () { print x=2 };
union T*
```

## Restrictions user-defined functions usage

The following restrictions apply:

1. User-defined functions can't pass into
   [toscalar()](../toscalarfunction.md) invocation
   information that depends on the row-context in which the
   function is called.

2. User-defined functions that return a tabular expression can't
   be invoked with an argument that varies with the row context.
   
3. A function taking at least one tabular input can't be invoked on a remote cluster.

4. A scalar function can't be invoked on a remote cluster.

The only place a user-defined function may be invoked
with an argument that varies with the row context is when the
user-defined function is composed of scalar functions only and
doesn't make use of `toscalar()`.


**Example of Restriction 1**

```kusto
// Supported:
// f is a scalar function that doesn't reference any tabular expression
let Table1 = datatable(xdate:datetime)[datetime(1970-01-01)];
let Table2 = datatable(Column:long)[1235];
let f = (hours:long) { now() + hours*1h };
Table2 | where Column != 123 | project d = f(10)

// Supported:
// f is a scalar function that references the tabular expression Table1,
// but is invoked with no reference to the current row context f(10):
let Table1 = datatable(xdate:datetime)[datetime(1970-01-01)];
let Table2 = datatable(Column:long)[1235];
let f = (hours:long) { toscalar(Table1 | summarize min(xdate) - hours*1h) };
Table2 | where Column != 123 | project d = f(10)

// Not supported:
// f is a scalar function that references the tabular expression Table1,
// and is invoked with a reference to the current row context f(Column):
let Table1 = datatable(xdate:datetime)[datetime(1970-01-01)];
let Table2 = datatable(Column:long)[1235];
let f = (hours:long) { toscalar(Table1 | summarize min(xdate) - hours*1h) };
Table2 | where Column != 123 | project d = f(Column)
```

**Example of Restriction 2**

```kusto
// Not supported:
// f is a tabular function that is invoked in a context
// that expects a scalar value.
let Table1 = datatable(xdate:datetime)[datetime(1970-01-01)];
let Table2 = datatable(Column:long)[1235];
let f = (hours:long) { range x from 1 to hours step 1 | summarize make_list(x) };
Table2 | where Column != 123 | project d = f(Column)
```

## Features unsupported by user-defined functions

1. **Function overloading**:
   Can't overload a function (for example, create multiple functions with the same name and different
   input schema).
2. **Default values**:
   The default value for a scalar parameter to a function must be
   a scalar literal (constant). Also, stored functions can't
   have a default value of type `dynamic`.