# Kusto Query Language Classification

The Kusto Query Language was created as part of the Kusto service, also known as [Azure Data Explorer](https://azure.microsoft.com/services/data-explorer/).

The contents of this documentation folder are an exact match for the implementation of the language by the Kusto service.

The language has been open-sourced, so that other data platforms and services can benefit and provide their users with a simple and productive language.
Whether these are developers, data scientists or data consumers who are familiar with the Kusto Query Language, they can all leverage their language
skills across the wide variety of products and services that have adopted it.

To achieve this goal, it is essential that the following conditions will be met:

* The semantics of the language are the same across all products and services, so that for a given data set the same query will return the same results.
* The supported flavor of the language is known and well defined.

Since the original full language will probably not be applicable to all product and services, the language has been categorized into flavors.
It is recommended that the implementer of the language will choose one of these flavors, so that users can easily understand what are the language constructs the product or service they use supports.

## Language flavors

The language is classified to three flavors:

* **Core:** The minimal language syntax that supports the majority of the data query scenarios.
* **Full:** The full language syntax.
* **Out of scope:** language construct that are not considered to be relevant to language implementors, either because they are legacy or because their semantic is not formal.

## Classification table

The following tables classifies the language syntax to the flavors outlined above.

### [Entities](./schema-entities/index.md)

|**Name**|**Flavor**|**Comment**|
|---|---|---|
|  [Entity names](./schema-entities/entity-names.md) | Core |  |
|  [Querying external entities](cross-cluster-or-database-queries.md) | Full |  |
|  [Tables](./schema-entities/tables.md) | Core |  |
|  [Columns](./schema-entities/columns.md) | Core |  |
|  [Stored functions](./schema-entities/stored-functions.md) | Full |  |
|  [External Tables](./schema-entities/externaltables.md) | Full |  |

### [Data types](./scalar-data-types/index.md)

|**Name**|**Flavor**|**Comment**|
|---|---|---|
|  [bool](./scalar-data-types/bool.md) | Core |  |
|  [datetime](./scalar-data-types/datetime.md) | Core |  |
|  [decimal](./scalar-data-types/decimal.md) | Core |  |
|  [dynamic](./scalar-data-types/dynamic.md) | Core |  |
|  [guid](./scalar-data-types/guid.md) | Core |  |
|  [int](./scalar-data-types/int.md) | Core |  |
|  [long](./scalar-data-types/long.md) | Core |  |
|  [real](./scalar-data-types/real.md) | Core |  |
|  [string](./scalar-data-types/string.md) | Core |  |
|  [timespan](./scalar-data-types/timespan.md) | Core |  |
|  [Null values](./scalar-data-types/null-values.md) | Core |  |

### [Functions](./functions/index.md)

|**Name**|**Flavor**|**Comment**|
|---|---|---|
|  [User-defined functions](./functions/user-defined-functions.md) | Core |  |

### [Query statements](statements.md)

|**Name**|**Flavor**|**Comment**|
|---|---|---|
|  [Alias statement](aliasstatement.md) | Full |  |
|  [Let statement](letstatement.md) | Core |  |
|  [Pattern statement](patternstatement.md) | Full |  |
|  [Query parameters statement](queryparametersstatement.md) | Core |  |
|  [Restrict statement](restrictstatement.md) | Full |  |
|  [Set statement](setstatement.md) | Full |  |
|  [Tabular expression statements](tabularexpressionstatements.md) | Core |  |
|  [Batches](batches.md) | Full |  |

### [Tabular operators](queries.md)

|**Name**|**Flavor**|**Comment**|
|---|---|---|
|  [as operator](asoperator.md) | Full |  |
|  [consume operator](consumeoperator.md) | Full |  |
|  [count operator](countoperator.md) | Core |  |
|  [datatable operator](datatableoperator.md) | Core |  |
|  [distinct operator](distinctoperator.md) | Core |  |
|  [evaluate operator](evaluateoperator.md) | Core | No semantics, followed by a syntax of a function call |
|  [extend operator](extendoperator.md) | Core |  |
|  [externaldata operator](externaldata-operator.md) | Full |  |
|  [facet operator](facetoperator.md) | Out of scope |  |
|  [find operator](findoperator.md) | Out of scope |  |
|  [fork operator](forkoperator.md) | Out of scope |  |
|  [getschema operator](getschemaoperator.md) | Core |  |
|  [invoke operator](invokeoperator.md) | Core |  |
|  [join operator](joinoperator.md) | Core | [Shuffle join and summarize](shufflequery.md) are out of scope. join flavors in core: inner, leftouter, rightouter, fullouter (requires specifying kind)
join flavors in full: innerunique (default), leftanti, right anti, left semi and right semi|
|  [limit operator](limitoperator.md) | Core |  |
|  [lookup operator](lookupoperator.md) | Full |  |
|  [make-series operator](make-seriesoperator.md) | Full |  |
|  [mv-apply operator](mv-applyoperator.md) | Full |  |
|  [mv-expand operator](mvexpandoperator.md) | Full |  |
|  [order operator](orderoperator.md) | Core |  |
|  [project operator](projectoperator.md) | Core |  |
|  [project-away operator](projectawayoperator.md) | Core |  |
|  [project-rename operator](projectrenameoperator.md) | Core |  |
|  [project-reorder operator](projectreord
eroperator.md) | Core |  |
|  [parse operator](parseoperator.md) | Core | Regex variant is in the "full" flavor |
|  [partition operator](partitionoperator.md) | Full |  |
|  [print operator](printoperator.md) | Core |  |
|  [range operator](rangeoperator.md) | Core |  |
|  [reduce operator](reduceoperator.md) | Out of scope |  |
|  [render operator](renderoperator.md) | Core |  |
|  [sample operator](sampleoperator.md) | Full |  |
|  [sample-distinct operator](sampledistinctoperator.md) | Full |  |
|  [search operator](searchoperator.md) | Out of scope |  |
|  [serialize operator](serializeoperator.md) | Full |  |
|  [sort operator](sortoperator.md) | Core |  |
|  [summarize operator](summarizeoperator.md) | Core | [Shuffle join and summarize](shufflequery.md) are out of scope |
|  [take operator](takeoperator.md) | Core |  |
|  [top operator](topoperator.md) | Core |  |
|  [top-nested operator](topnestedoperator.md) | Full |  |
|  [top-hitters operator](tophittersoperator.md) | Out of scope |  |
|  [union operator](unionoperator.md) | Core |  |
|  [where operator](whereoperator.md) | Core |  |

### Plugins ([evaluate operator](evaluateoperator.md))

|**Name**|**Flavor**|**Comment**|
|---|---|---|
|    [autocluster](autoclusterplugin.md) | Out of scope |  |
|    [bag_unpack](bag-unpackplugin.md) | Full |  |
|    [basket](basketplugin.md) | Out of scope  |  |
|    [dcount_intersect](dcount-intersect-plugin.md) | Full |  |
|    [narrow](narrowplugin.md) | Full |  |
|    [pivot](pivotplugin.md) | Full |  |
|    [preview](previewplugin.md) | Full |  |
|    [python](pythonplugin.md) | Full |  |
|    [R](rplugin.md) | Full |  |
|    [rolling_percentile](rolling-percentile-plugin.md) | Full |  |
|    [sql_request](sqlrequestplugin.md) | Full |  |
|    [sequence_detect](sequence-detect-plugin.md) | Out of scope |  |

### Special functions

|**Name**|**Flavor**|**Comment**|
|---|---|---|
|  [cluster()](clusterfunction.md) | Out of scope |  |
|  [database()](databasefunction.md) | Core |  |
|  [materialize()](materializefunction.md) | Out of scope |  |
|  [table()](tablefunction.md) | Full |  |
|  [toscalar()](toscalarfunction.md) | Core |  |

### Scalar operators

|**Name**|**Flavor**|**Comment**|
|---|---|---|
|  [Bitwise (binary) | Core |  | operators](binoperators.md) | Core |  |
|  [Datetime/timespan arithmetic](datetime-timespan-arithmetic.md) | Core |  |
|  [Logical (binary) | Core |  | operators](logicaloperators.md) | Core |  |
|  [Numerical operators](numoperators.md) | Core |  |
|  [String operators](datatypes-string-operators.md) | Core |  |
|  [between operator](betweenoperator.md) | Core |  |
|  [!between operator](notbetweenoperator.md) | Core |  |
|  [in/!in operators](inoperator.md) | Core |  | 

### [Scalar functions](scalarfunctions.md)

|**Name**|**Flavor**|**Comment**|
|---|---|---|
|  [abs()](abs-function.md) | Full |  |
|  [acos()](acosfunction.md) | Full |  |
|  [ago()](agofunction.md) | Core |  |
|  [array_concat()](arrayconcatfunction.md) | Full |  |
|  [array_iif()](arrayifffunction.md) | Full |  |
|  [array_length()](arraylengthfunction.md) | Full |  |
|  [array_slice()](arrayslicefunction.md) | Full |  |
|  [array_split()](arraysplitfunction.md) | Full |  |
|  [asin()](asinfunction.md) | Full |  |
|  [assert()](assert-function.md) | Full |  |
|  [atan()](atanfunction.md) | Full |  |
|  [atan2()](atan2function.md) | Full |  |
|  [base64_decode_toarray()](base64_decode_toarrayfunction.md) | Full |  |
|  [base64_decode_tostring()](base64_decode_tostringfunction.md) | Full |  |
|  [base64_encode_tostring()](base64_encode_tostringfunction.md) | Full |  |
|  [bag_keys()](bagkeysfunction.md) | Full |  |
|  [beta_cdf()](beta-cdffunction.md) | Full |  |
|  [beta_inv()](beta-invfunction.md) | Full |  |
|  [beta_pdf()](beta-pdffunction.md) | Full |  |
|  [bin()](binfunction.md) | Core |  |
|  [bin_at()](binatfunction.md) | Out of scope |  |
|  [bin_auto()](bin-autofunction.md) | Full |  |
|  [binary_and()](binary-andfunction.md) | Full |  |
|  [binary_not()](binary-notfunction.md) | Full |  |
|  [binary_or()](binary-orfunction.md) | Full |  |
|  [binary_shift_left()](binary-shift-leftfunction.md) | Full |  |
|  [binary_shift_right()](binary-shift-rightfunction.md) | Full |  |
|  [binary_xor()](binary-xorfunction.md) | Full |  |
|  [case()](casefunction.md) | Full |  |
|  [ceiling()](ceilingfunction.md) | Full |  |
|  [coalesce()](coalescefunction.md) | Full |  |
|  [column_ifexists()](columnifexists.md) | Full |  |
|  [cos()](cosfunction.md) | Full |  |
|  [cot()](cotfunction.md) | Full |  |
|  [countof()](countoffunction.md) | Full |  |
|  [current_cluster_endpoint()](current-cluster-endpoint-function.md) | Out of scope |  |
|  [current_cursor()](cursorcurrent.md) | Out of scope |  |
|  [current_database()](current-database-function.md) | Out of scope |  |
|  [current_principal()](current-principalfunction.md) | Out of scope |  |
|  [cursor_after()](cursorafterfunction.md) | Out of scope |  |
|  [cursor_before_or_at()](cursorbeforeoratfunction.md) | Out of scope |  |
|  [cursor_current()](cursorcurrent.md) | Out of scope |  |
|  [datetime_add()](datetime-addfunction.md) | Full |  |
|  [datetime_part()](datetime-partfunction.md) | Full |  |
|  [datetime_diff()](datetime-difffunction.md) | Full |  |
|  [dayofmonth()](dayofmonthfunction.md) | Full |  |
|  [dayofweek()](dayofweekfunction.md) | Full |  |
|  [dayofyear()](dayofyearfunction.md) | Full |  |
|  [dcount_hll()](dcount-hllfunction.md) | Full |  |
|  [degrees()](degreesfunction.md) | Out of scope |  |
|  [endofday()](endofdayfunction.md) | Full |  |
|  [endofmonth()](endofmonthfunction.md) | Full |  |
|  [endofweek()](endofweekfunction.md) | Full |  |
|  [endofyear()](endofyearfunction.md) | Full |  |
|  [estimate_data_size()](estimate-data-sizefunction.md) | Out of scope |  |
|  [exp()](exp-function.md) | Full |  |
|  [exp10()](exp10-function.md) | Full |  |
|  [exp2()](exp2-function.md) | Full |  |
|  [extent_id()](extentidfunction.md) | Out of scope |  |
|  [extent_tags()](extenttagsfunction.md) | Out of scope |  |
|  [extract()](extractfunction.md) | Core |  |
|  [extract_all()](extractallfunction.md) | Full |  |
|  [extractjson()](extractjsonfunction.md) | Full |  |
|  [floor()](floorfunction.md) | Full |  |
|  [format_datetime()](format-datetimefunction.md) | Full |  |
|  [format_timespan()](format-timespanfunction.md) | Full |  |
|  [gamma()](gammafunction.md) | Full |  |
|  [getmonth()](getmonthfunction.md) | Full |  |
|  [gettype()](gettypefunction.md) | Core |  |
|  [getyear()](getyearfunction.md) | Full |  |
|  [hash()](hashfunction.md) | Core |  |
|  [hash_sha256()](sha256hashfunction.md) | Full |  |
|  [hll_merge()](hllmergefunction.md) | Full |  |
|  [hourofday()](hourofdayfunction.md) | Full |  |
|  [iif()](iiffunction.md) | Core |  |
|  [indexof()](indexoffunction.md) | Core |  |
|  [indexof_regex()](indexofregexfunction.md) | Full |  |
|  [ingestion_time()](ingestiontimefunction.md) | Out of scope |  |
|  [isascii()](isascii.md) | Full |  |
|  [isempty()](isemptyfunction.md) | Core |  |
|  [isfinite()](isfinitefunction.md) | Core |  |
|  [isinf()](isinffunction.md) | Core |  |
|  [isnan()](isnanfunction.md) | Core |  |
|  [isnotempty(), notempty()](isnotemptyfunction.md) | Core |  |
|  [isnotnull(), notempty()](isnotnullfunction.md) | Core |  |
|  [isnull()](isnullfunction.md) | Core |  |
|  [isutf8()](isutf8.md) | Full |  |
|  [log()](log-function.md) | Core |  |
|  [log10()](log10-function.md) | Full |  |
|  [log2()](log2-function.md) | Full |  |
|  [loggamma()](loggammafunction.md) | Full |  |
|  [make_datetime()](make-datetimefunction.md) | Full |  |
|  [make_string()](makestringfunction.md) | Full |  |
|  [make_timespan()](make-timespanfunction.md) | Full |  |
|  [max_of()](max-offunction.md) | Full |  |
|  [min_of()](min-offunction.md) | Full |  |
|  [monthofyear()](monthofyearfunction.md) | Full |  |
|  [new_guid()](newguidfunction.md) | Full |  |
|  [not()](notfunction.md) | Core |  |
|  [now()](nowfunction.md) | Core |  |
|  [pack()](packfunction.md) | Full |  |
|  [pack_all()](packallfunction.md) | Full |  |
|  [pack_array()](packarrayfunction.md) | Full |  |
|  [pack_dictionary()](packdictionaryfunction.md) | Full |  |
|  [parse_csv()](parsecsvfunction.md) | Full |  |
|  [parse_ipv4()](parse-ipv4function.md) | Full |  |
|  [parse_json()](parsejsonfunction.md) | Core |  |
|  [parse_path()](parsepathfunction.md) | Full |  |
|  [parse_url()](parseurlfunction.md) | Full |  |
|  [parse_urlquery()](parseurlqueryfunction.md) | Full |  |
|  [parse_user_agent()](parse-useragentfunction.md) | Full |  |
|  [parse_version()](parse-versionfunction.md) | Full |  |
|  [parse_xml()](parse-xmlfunction.md) | Full |  |
|  [percentile_tdigest()](percentile-tdigestfunction.md) | Full |  |
|  [percentrank_tdigest()](percentrank-tdigestfunction.md) | Full |  |
|  [pi()](pifunction.md) | Full |  |
|  [pow()](powfunction.md) | Core |  |
|  [radians()](radiansfunction.md) | Full |  |
|  [rand()](randfunction.md) | Core |  |
|  [range()](rangefunction.md) | Full |  |
|  [rank_tdigest()](rank-tdigest.md) | Full |  |
|  [repeat()](repeatfunction.md) | Full |  |
|  [replace()](replacefunction.md) | Full |  |
|  [reverse()](reversefunction.md) | Full |  |
|  [round()](roundfunction.md) | Full |  |
|  [set_difference()](setdifferencefunction.md) | Full |  |
|  [set_intersect()](setintersectfunction.md) | Full |  |
|  [set_union()](setunionfunction.md) | Full |  |
|  [sign()](signfunction.md) | Full |  |
|  [sin()](sinfunction.md) | Full |  |
|  [split()](splitfunction.md) | Core |  |
|  [sqrt()](sqrtfunction.md) | Full |  |
|  [startofday()](startofdayfunction.md) | Full |  |
|  [startofmonth()](startofmonthfunction.md) | Full |  |
|  [startofweek()](startofweekfunction.md) | Full |  |
|  [startofyear()](startofyearfunction.md) | Full |  |
|  [strcat()](strcatfunction.md) | Core |  |
|  [strcat_array()](strcat-arrayfunction.md) | Full |  |
|  [strcat_delim()](strcat-delimfunction.md) | Full |  |
|  [strcmp()](strcmpfunction.md) | Full |  |
|  [string_size()](stringsizefunction.md) | Full |  |
|  [strlen()](strlenfunction.md) | Core |  |
|  [strrep()](strrepfunction.md) | Full |  |
|  [substring()](substringfunction.md) | Core |  |
|  [tan()](tanfunction.md) | Full |  |
|  [tdigest_merge()](tdigest-mergefunction.md) | Full |  |
|  [tobool()](toboolfunction.md) | Out of scope |  |
|  [todatetime()](todatetimefunction.md) | Out of scope |  |
|  [todecimal()](todecimalfunction.md) | Out of scope |  |
|  [todouble()](todoublefunction.md) | Out of scope |  |
|  [todynamic()](todynamicfunction.md) | Out of scope |  |
|  [toguid()](toguidfunction.md) | Out of scope |  |
|  [tohex()](tohexfunction.md) | Out of scope |  |
|  [toint()](tointfunction.md) | Out of scope |  |
|  [tolong()](tolongfunction.md) | Out of scope |  |
|  [tolower()](tolowerfunction.md) | Full |  |
|  [toreal()](todoublefunction.md) | Out of scope |  |
|  [tostring()](tostringfunction.md) | Out of scope |  |
|  [totimespan()](totimespanfunction.md) | Out of scope |  |
|  [toupper()](toupperfunction.md) | Full |  |
|  [to_utf8()](toutf8function.md) | Full |  |
|  [translate()](translatefunction.md) | Out of scope |  |
|  [treepath()](treepathfunction.md) | Full |  |
|  [trim()](trimfunction.md) | Full |  |
|  [trim_end()](trimendfunction.md) | Full |  |
|  [trim_start()](trimstartfunction.md) | Full |  |
|  [url_decode()](urldecodefunction.md) | Full |  |
|  [url_encode()](urlencodefunction.md) | Full |  |
|  [weekofyear()](weekofyearfunction.md) | Full |  |
|  [welch_test()](welch-testfunction.md) | Out of scope |  |
|  [zip()](zipfunction.md) | Full |  |

### Aggregation functions

|**Name**|**Flavor**|**Comment**|
|---|---|---|
|  [any()](any-aggfunction.md) | Core |  |
|  [anyif()](anyif-aggfunction.md) | Core |  |
|  [arg_max()](arg-max-aggfunction.md) | Core |  |
|  [arg_min()](arg-min-aggfunction.md) | Core |  |
|  [avg()](avg-aggfunction.md) | Core |  |
|  [avgif()](avgif-aggfunction.md) | Full |  |
|  [buildschema()](buildschema-aggfunction.md) | Full |  |
|  [count()](count-aggfunction.md) | Core |  |
|  [countif()](countif-aggfunction.md) | Full |  |
|  [dcount()](dcount-aggfunction.md) | Core |  |
|  [dcountif()](dcountif-aggfunction.md) | Full |  |
|  [hll()](hll-aggfunction.md) | Full |  |
|  [hll_merge()](hll-merge-aggfunction.md) | Full |  |
|  [make_bag()](make-bag-aggfunction.md) | Full |  |
|  [make_bag_if()](make-bag-if-aggfunction.md) | Full |  |
|  [make_list()](makelist-aggfunction.md) | Core |  |
|  [make_list_if()](makelistif-aggfunction.md) | Core |  |
|  [make_list_with_nulls()](make-list-with-nulls-aggfunction.md) | Core |  |
|  [make_set()](makeset-aggfunction.md) | Core |  |
|  [make_set_if()](makesetif-aggfunction.md) | Core |  |
|  [max()](max-aggfunction.md) | Core |  |
|  [maxif()](maxif-aggfunction.md) | Core |  |
|  [min()](min-aggfunction.md) | Core |  |
|  [minif()](minif-aggfunction.md) | Core |  |
|  [percentiles()](percentiles-aggfunction.md) | Core |  |
|  [stdev()](stdev-aggfunction.md) | Core |  |
|  [stdevif()](stdevif-aggfunction.md) | Full |  |
|  [stdevp()](stdevp-aggfunction.md) | Full |  |
|  [sum()](sum-aggfunction.md) | Core |  |
|  [sumif()](sumif-aggfunction.md) | Full |  |
|  [tdigest()](tdigest-aggfunction.md) | Full |  |
|  [tdigest_merge()](tdigest-merge-aggfunction.md) | Full |  |
|  [variance()](variance-aggfunction.md) | Core |  |
|  [varianceif()](varianceif-aggfunction.md) | Full |  |
|  [variancep()](variancep-aggfunction.md) | Full |  |

### [Window functions](windowsfunctions.md)

|**Name**|**Flavor**|**Comment**|
|---|---|---|
|  [next()](nextfunction.md) | Full |  |
|  [prev()](prevfunction.md) | Full |  |
|  [row_cumsum()](rowcumsumfunction.md) | Full |  |
|  [row_number()](rownumberfunction.md) | Full |  |

### Rolling window aggregations

|**Name**|**Flavor**|**Comment**|
|---|---|---|
|  [rolling_percentile()](rolling-percentile-plugin.md) | Full |  |

### Time Series Analysis

|**Name**|**Flavor**|**Comment**|
|---|---|---|
|  [time series analysis](machine-learning-and-tsa.md) | Full |  |
|  [make-series operator](make-seriesoperator.md) | Full |  |
|  [series_add()](series-addfunction.md) | Full |  |
|  [series_decompose()](series-decomposefunction.md) | Full |  |
|  [series_decompose_anomalies()](series-decompose-anomaliesfunction.md) | Full |  |
|  [series_decompose_forecast()](series-decompose-forecastfunction.md) | Full |  |
|  [series_divide()](series-dividefunction.md) | Full |  |
|  [series_equals()](series-equalsfunction.md) | Full |  |
|  [series_fill_backward()](series-fill-backwardfunction.md) | Full |  |
|  [series_fill_const()](series-fill-constfunction.md) | Full |  |
|  [series_fill_forward()](series-fill-forwardfunction.md) | Full |  |
|  [series_fill_linear()](series-fill-linearfunction.md) | Full |  |
|  [series_fir()](series-firfunction.md) | Full |  |
|  [series_fit_line()](series-fit-linefunction.md) | Full |  |
|  [series_fit_line_dynamic()](series-fit-line-dynamicfunction.md) | Full |  |
|  [series_fit_2lines()](series-fit-2linesfunction.md) | Full |  |
|  [series_fit_2lines_dynamic()](series-fit-2lines-dynamicfunction.md) | Full |  |
|  [series_greater()](series-greaterfunction.md) | Full |  |
|  [series_greater_equals()](series-greater-equalsfunction.md) | Full |  |
|  [series_iir()](series-iirfunction.md) | Full |  |
|  [series_less()](series-lessfunction.md) | Full |  |
|  [series_less_equals()](series-less-equalsfunction.md) | Full |  |
|  [series_multiply()](series-multiplyfunction.md) | Full |  |
|  [series_not_equals()](series-not-equalsfunction.md) | Full |  |
|  [series_outliers()](series-outliersfunction.md) | Full |  |
|  [series_pearson_correlation()](series-pearson-correlationfunction.md) | Full |  |
|  [series_periods_detect()](series-periods-detectfunction.md) | Full |  |
|  [series_periods_validate()](series-periods-validatefunction.md) | Full |  |
|  [series_seasonal()](series-seasonalfunction.md) | Full |  |
|  [series_stats()](series-statsfunction.md) | Full |  |
|  [series_stats_dynamic()](series-stats-dynamicfunction.md) | Full |  |
|  [series_subtract()](series-subtractfunction.md) | Full |  |

### [User Analytics](useranalytics.md)

|**Name**|**Flavor**|**Comment**|
|---|---|---|
|  [Activity Counts Metrics (total values, distinct values, new values)](activity-counts-metrics-plugin.md) | Out of scope |  |
|  [Activity Engagement (DAU, WAU, MAU)](activity-engagement-plugin.md) | Out of scope |  |
|  [Activity Metrics - New (retention, churn, new values)](new-activity-metrics-plugin.md) | Out of scope |  |
|  [Activity Metrics (retention, churn, new values)](activity-metrics-plugin.md) | Out of scope |  |
|  [Active Users Count](active-users-count-plugin.md) | Out of scope |  |
|  [Sliding Window Counts](sliding-window-counts-plugin.md) | Out of scope |  |
|  [Session Count](session-count-plugin.md) | Out of scope |  |
|  [Funnel Sequence](funnel-sequence-plugin.md) | Out of scope |  |
|  [Funnel Sequence Completion](funnel-sequence-completion-plugin.md) | Out of scope |  |
