---
title: Scalar Functions - Azure Data Explorer | Microsoft Docs
description: This article describes Scalar Functions in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 05/27/2020
---
# Scalar function types

## Binary functions

|Function Name     |Description                                          |
|-------------------------|--------------------------------------------------------|
|[binary_and()](binary-andfunction.md)|Returns a result of the bitwise and operation between two values.|
|[binary_not()](binary-notfunction.md)|Returns a bitwise negation of the input value.|
|[binary_or()](binary-orfunction.md)|Returns a result of the bitwise or operation of the two values.|
|[binary_shift_left()](binary-shift-leftfunction.md)|Returns binary shift left operation on a pair of numbers: a << n.|
|[binary_shift_right()](binary-shift-rightfunction.md)|Returns binary shift right operation on a pair of numbers: a >> n.|
|[binary_xor()](binary-xorfunction.md)|Returns a result of the bitwise xor operation of the two values.|
|[bitset_count_ones()](bitset-count-onesfunction.md)|Returns the number of set bits in the binary representation of a number.|

## Conversion functions

|Function Name     |Description                                          |
|-------------------------|--------------------------------------------------------|
|[tobool()](toboolfunction.md)|Converts input to boolean (signed 8-bit) representation.|
|[todatetime()](todatetimefunction.md)|Converts input to datetime scalar.|
|[todouble()/toreal()](todoublefunction.md)|Converts the input to a value of type real. (todouble() and toreal() are synonyms.)|
|[tostring()](tostringfunction.md)|Converts input to a string representation.|
|[totimespan()](totimespanfunction.md)|Converts input to timespan scalar.|

## DateTime/timespan functions

|Function Name     |Description                                          |
|-------------------------|--------------------------------------------------------|
|[ago()](agofunction.md)|Subtracts the given timespan from the current UTC clock time.|
|[datetime_add()](datetime-addfunction.md)|Calculates a new datetime from a specified datepart multiplied by a specified amount, added to a specified datetime.|
|[datetime_part()](datetime-partfunction.md)|Extracts the requested date part as an integer value.|
|[datetime_diff()](datetime-difffunction.md)|Returns the end of the year containing the date, shifted by an offset, if provided.|
|[dayofmonth()](dayofmonthfunction.md)|Returns the integer number representing the day number of the given month.|
|[dayofweek()](dayofweekfunction.md)|Returns the integer number of days since the preceding Sunday, as a timespan.|
|[dayofyear()](dayofyearfunction.md)|Returns the integer number represents the day number of the given year.|
|[endofday()](endofdayfunction.md)|Returns the end of the day containing the date, shifted by an offset, if provided.|
|[endofmonth()](endofmonthfunction.md)|Returns the end of the month containing the date, shifted by an offset, if provided.|
|[endofweek()](endofweekfunction.md)|Returns the end of the week containing the date, shifted by an offset, if provided.|
|[endofyear()](endofyearfunction.md)|Returns the end of the year containing the date, shifted by an offset, if provided.|
|[format_datetime()](format-datetimefunction.md)|Formats a datetime parameter based on the format pattern parameter.|
|[format_timespan()](format-timespanfunction.md)|Formats a format-timespan parameter based on the format pattern parameter.|
|[getmonth()](getmonthfunction.md)|Get the month number (1-12) from a datetime.|
|[getyear()](getyearfunction.md)|Returns the year part of the datetime argument.|
|[hourofday()](hourofdayfunction.md)|Returns the integer number representing the hour number of the given date.|
|[make_datetime()](make-datetimefunction.md)|Creates a datetime scalar value from the specified date and time.|
|[make_timespan()](make-timespanfunction.md)|Creates a timespan scalar value from the specified time period.|
|[monthofyear()](monthofyearfunction.md)|Returns the integer number represents the month number of the given year.|
|[now()](nowfunction.md)|Returns the current UTC clock time, optionally offset by a given timespan.|
|[startofday()](startofdayfunction.md)|Returns the start of the day containing the date, shifted by an offset, if provided.|
|[startofmonth()](startofmonthfunction.md)|Returns the start of the month containing the date, shifted by an offset, if provided.|
|[startofweek()](startofweekfunction.md)|Returns the start of the week containing the date, shifted by an offset, if provided.|
|[startofyear()](startofyearfunction.md)|Returns the start of the year containing the date, shifted by an offset, if provided.|
|[todatetime()](todatetimefunction.md)|Converts input to datetime scalar.|
|[totimespan()](totimespanfunction.md)|Converts input to timespan scalar.|
|[unixtime_microseconds_todatetime()](unixtime-microseconds-todatetimefunction.md)|Converts unix-epoch microseconds to UTC datetime.|
|[unixtime_milliseconds_todatetime()](unixtime-milliseconds-todatetimefunction.md)|Converts unix-epoch milliseconds to UTC datetime.|
|[unixtime_nanoseconds_todatetime()](unixtime-nanoseconds-todatetimefunction.md)|Converts unix-epoch nanoseconds to UTC datetime.|
|[unixtime_seconds_todatetime()](unixtime-seconds-todatetimefunction.md)|Converts unix-epoch seconds to UTC datetime.|
|[weekofyear()](weekofyearfunction.md)|Returns an integer representing the week number.|


## Dynamic/array functions

|Function Name     |Description                                          |
|-------------------------|--------------------------------------------------------|
|[array_concat()](arrayconcatfunction.md)|Concatenates a number of dynamic arrays to a single array.|
|[array_iif()](arrayifffunction.md)|Applies element-wise iif function on arrays.|
|[array_index_of()](arrayindexoffunction.md)|Searches the array for the specified item, and returns its position.|
|[array_length()](arraylengthfunction.md)|Calculates the number of elements in a dynamic array.|
|[array_reverse()](array-reverse-function.md)|Reverses the order of the elements in a dynamic array.|
|[array_rotate_left()](array_rotate_leftfunction.md)|Rotates values inside a dynamic array to the left.|
|[array_rotate_right()](array_rotate_rightfunction.md)|Rotates values inside a dynamic array to the right.|
|[array_shift_left()](array_shift_leftfunction.md)|Shifts values inside a dynamic array to the left.|
|[array_shift_right()](array_shift_rightfunction.md)|Shifts values inside a dynamic array to the right.|
|[array_slice()](arrayslicefunction.md)|Extracts a slice of a dynamic array.|
|[array_sort_asc()](arraysortascfunction.md)|Sorts a collection of arrays in ascending order.|
|[array_sort_desc()](arraysortdescfunction.md)|Sorts a collection of arrays in descending order.|
|[array_split()](arraysplitfunction.md)|Builds an array of arrays split from the input array.|
|[bag_keys()](bagkeysfunction.md)|Enumerates all the root keys in a dynamic property-bag object.|
|[bag_merge()](bag-merge-function.md)|Merges dynamic property-bags into a dynamic property-bag with all properties merged.|
|[bag_remove_keys()](bag-remove-keys-function.md)|Removes keys and associated values from a dynamic property-bag.|
|[pack()](packfunction.md)|Creates a dynamic object (property bag) from a list of names and values.|
|[pack_all()](packallfunction.md)|Creates a dynamic object (property bag) from all the columns of the tabular expression.|
|[pack_array()](packarrayfunction.md)|Packs all input values into a dynamic array.|
|[repeat()](repeatfunction.md)|Generates a dynamic array holding a series of equal values.|
|[set_difference()](setdifferencefunction.md)|Returns an array of the set of all distinct values that are in the first array but aren't in other arrays.|
|[set_has_element()](sethaselementfunction.md)|Determines whether the specified array contains the specified element.|
|[set_intersect()](setintersectfunction.md)|Returns an array of the set of all distinct values that are in all arrays.|
|[set_union()](setunionfunction.md)|Returns an array of the set of all distinct values that are in any of provided arrays.|
|[treepath()](treepathfunction.md)|Enumerates all the path expressions that identify leaves in a dynamic object.|
|[zip()](zipfunction.md)|The zip function accepts any number of dynamic arrays. Returns an array whose elements are each an array with the elements of the input arrays of the same index.|

## Window scalar functions

|Function Name     |Description                                          |
|-------------------------|--------------------------------------------------------|
|[next()](nextfunction.md)|For the serialized row set, returns a value of a specified column from the later row according to the offset.|
|[prev()](prevfunction.md)|For the serialized row set, returns a value of a specified column from the earlier row according to the offset.|
|[row_cumsum()](rowcumsumfunction.md)|Calculates the cumulative sum of a column.|
|[row_number()](rownumberfunction.md)|Returns a row's number in the serialized row set - consecutive numbers starting from a given index or from 1 by default.|
|[row_rank()](rowrankfunction.md)|Returns a row's rank in the serialized row set.|

## Flow control functions

|Function Name            |Description                                             |
|-------------------------|--------------------------------------------------------|
|[toscalar()](toscalarfunction.md)|Returns a scalar constant value of the evaluated expression.|

## Mathematical functions

|Function Name     |Description                                          |
|-------------------------|--------------------------------------------------------|
|[abs()](abs-function.md)|Calculates the absolute value of the input.|
|[acos()](acosfunction.md)|Returns the angle whose cosine is the specified number (the inverse operation of cos()).|
|[asin()](asinfunction.md)|Returns the angle whose sine is the specified number (the inverse operation of sin()).|
|[atan()](atanfunction.md)|Returns the angle whose tangent is the specified number (the inverse operation of tan()).|
|[atan2()](atan2function.md)|Calculates the angle, in radians, between the positive x-axis and the ray from the origin to the point (y, x).|
|[beta_cdf()](beta-cdffunction.md)|Returns the standard cumulative beta distribution function.|
|[beta_inv()](beta-invfunction.md)|Returns the inverse of the beta cumulative probability beta density function.|
|[beta_pdf()](beta-pdffunction.md)|Returns the probability density beta function.|
|[cos()](cosfunction.md)|Returns the cosine function.|
|[cot()](cotfunction.md)|Calculates the trigonometric cotangent of the specified angle, in radians.|
|[degrees()](degreesfunction.md)|Converts angle value in radians into value in degrees, using formula degrees = (180 / PI) * angle-in-radians.|
|[exp()](exp-function.md)|The base-e exponential function of x, which is e raised to the power x: e^x.|
|[exp10()](exp10-function.md)|The base-10 exponential function of x, which is 10 raised to the power x: 10^x.|
|[exp2()](exp2-function.md)|The base-2 exponential function of x, which is 2 raised to the power x: 2^x.|
|[gamma()](gammafunction.md)|Computes gamma function.|
|[isfinite()](isfinitefunction.md)|Returns whether input is a finite value (isn't infinite or NaN).|
|[isinf()](isinffunction.md)|Returns whether input is an infinite (positive or negative) value.|
|[isnan()](isnanfunction.md)|Returns whether input is Not-a-Number (NaN) value.|
|[log()](log-function.md)|Returns the natural logarithm function.|
|[log10()](log10-function.md)|Returns the common (base-10) logarithm function.|
|[log2()](log2-function.md)|Returns the base-2 logarithm function.|
|[loggamma()](loggammafunction.md)|Computes log of absolute value of the gamma function.|
|[not()](notfunction.md)|Reverses the value of its bool argument.|
|[pi()](pifunction.md)|Returns the constant value of Pi (π).|
|[pow()](powfunction.md)|Returns a result of raising to power.|
|[radians()](radiansfunction.md)|Converts angle value in degrees into value in radians, using formula radians = (PI / 180) * angle-in-degrees.|
|[rand()](randfunction.md)|Returns a random number.|
|[range()](rangefunction.md)|Generates a dynamic array holding a series of equally spaced values.|
|[round()](roundfunction.md)|Returns the rounded source to the specified precision.|
|[sign()](signfunction.md)|Sign of a numeric expression.|
|[sin()](sinfunction.md)|Returns the sine function.|
|[sqrt()](sqrtfunction.md)|Returns the square root function.|
|[tan()](tanfunction.md)|Returns the tangent function.|
|[welch_test()](welch-testfunction.md)|Computes the p-value of the [Welch-test function](https://en.wikipedia.org/wiki/Welch%27s_t-test).|

## Metadata functions

|Function Name     |Description                                          |
|-------------------------|--------------------------------------------------------|
|[column_ifexists()](columnifexists.md)|Takes a column name as a string and a default value. Returns a reference to the column if it exists, otherwise - returns the default value.|
|[current_cluster_endpoint()](current-cluster-endpoint-function.md)|Returns the current cluster running the query.|
|[current_database()](current-database-function.md)|Returns the name of the database in scope.|
|[current_principal()](current-principalfunction.md)|Returns the current principal running this query.|
|[current_principal_details()](current-principal-detailsfunction.md)|Returns details of the principal running the query.|
|[current_principal_is_member_of()](current-principal-ismemberoffunction.md)|Checks group membership or principal identity of the current principal running the query.|
|[cursor_after()](cursorafterfunction.md)|Used to access to the records that were ingested after the previous value of the cursor.|
|[estimate_data_size()](estimate-data-sizefunction.md)|Returns an estimated data size of the selected columns of the tabular expression.|
|[extent_id()](extentidfunction.md)|Returns a unique identifier that identifies the data shard ("extent") that the current record resides in.|
|[extent_tags()](extenttagsfunction.md)|Returns a dynamic array with the tags of the data shard ("extent") that the current record resides in.|
|[ingestion_time()](ingestiontimefunction.md)|Retrieves the record's $IngestionTime hidden datetime column, or null.|

## Rounding functions

|Function Name     |Description                                          |
|-------------------------|--------------------------------------------------------|
|[bin()](binfunction.md)|Rounds values down to an integer multiple of a given bin size.|
|[bin_at()](binatfunction.md)|Rounds values down to a fixed-size "bin", with control over the bin's starting point. (See also bin function.)|
|[ceiling()](ceilingfunction.md)|Calculates the smallest integer greater than, or equal to, the specified numeric expression.|
|[floor()](floorfunction.md)|Rounds values down to an integer multiple of a given bin size.|

## Conditional functions

|Function Name     |Description                                          |
|-------------------------|--------------------------------------------------------|
|[case()](casefunction.md)|Evaluates a list of predicates and returns the first result expression whose predicate is satisfied.|
|[coalesce()](coalescefunction.md)|Evaluates a list of expressions and returns the first non-null (or non-empty for string) expression.|
|[iif()/iff()](iiffunction.md)|Evaluates the first argument (the predicate), and returns the value of either the second or third arguments, depending on whether the predicate evaluated to true (second) or false (third).|
|[max_of()](max-offunction.md)|Returns the maximum value of several evaluated numeric expressions.|
|[min_of()](min-offunction.md)|Returns the minimum value of several evaluated numeric expressions.|

## Series element-wise functions

|Function Name     |Description                                          |
|-------------------------|--------------------------------------------------------|
|[series_add()](series-addfunction.md)|Calculates the element-wise addition of two numeric series inputs.|
|[series_divide()](series-dividefunction.md)|Calculates the element-wise division of two numeric series inputs.|
|[series_equals()](series-equalsfunction.md)|Calculates the element-wise equals (`==`) logic operation of two numeric series inputs.|
|[series_greater()](series-greaterfunction.md)|Calculates the element-wise greater (`>`) logic operation of two numeric series inputs.|
|[series_greater_equals()](series-greater-equalsfunction.md)|Calculates the element-wise greater or equals (`>=`) logic operation of two numeric series inputs.|
|[series_less()](series-lessfunction.md)|Calculates the element-wise less (`<`) logic operation of two numeric series inputs.|
|[series_less_equals()](series-less-equalsfunction.md)|Calculates the element-wise less or equal (`<=`) logic operation of two numeric series inputs.|
|[series_multiply()](series-multiplyfunction.md)|Calculates the element-wise multiplication of two numeric series inputs.|
|[series_not_equals()](series-not-equalsfunction.md)|Calculates the element-wise not equals (`!=`) logic operation of two numeric series inputs.|
|[series_subtract()](series-subtractfunction.md)|Calculates the element-wise subtraction of two numeric series inputs.|

## Series processing functions

|Function Name     |Description                                          |
|-------------------------|--------------------------------------------------------|
|[series_decompose()](series-decomposefunction.md)|Does a decomposition of the series into components.|
|[series_decompose_anomalies()](series-decompose-anomaliesfunction.md)|Finds anomalies in a series based on series decomposition.|
|[series_decompose_forecast()](series-decompose-forecastfunction.md)|Forecast based on series decomposition.|
|[series_fill_backward()](series-fill-backwardfunction.md)|Performs backward fill interpolation of missing values in a series.|
|[series_fill_const()](series-fill-constfunction.md)|Replaces missing values in a series with a specified constant value.|
|[series_fill_forward()](series-fill-forwardfunction.md)|Performs forward fill interpolation of missing values in a series.|
|[series_fill_linear()](series-fill-linearfunction.md)|Performs linear interpolation of missing values in a series.|
|[series_fft()](series-fft-function.md)|Applies the Fast Fourier Transform (FFT) on a series.|
|[series_fir()](series-firfunction.md)|Applies a Finite Impulse Response filter on a series.|
|[series_fit_2lines()](series-fit-2linesfunction.md)|Applies two segments linear regression on a series, returning multiple columns.|
|[series_fit_2lines_dynamic()](series-fit-2lines-dynamicfunction.md)|Applies two segments linear regression on a series, returning dynamic object.|
|[series_fit_line()](series-fit-linefunction.md)|Applies linear regression on a series, returning multiple columns.|
|[series_fit_line_dynamic()](series-fit-line-dynamicfunction.md)|Applies linear regression on a series, returning dynamic object.|
|[series_fit_poly()](series-fit-poly-function.md)|Applies polynomial regression on a series, returning multiple columns.|
|[series_ifft()](series-ifft-function.md)|Applies the Inverse Fast Fourier Transform (IFFT) on a series.|
|[series_iir()](series-iirfunction.md)|Applies an Infinite Impulse Response filter on a series.|
|[series_outliers()](series-outliersfunction.md)|Scores anomaly points in a series.|
|[series_pearson_correlation()](series-pearson-correlationfunction.md)|Calculates the Pearson correlation coefficient of two series.|
|[series_periods_detect()](series-periods-detectfunction.md)|Finds the most significant periods that exist in a time series.|
|[series_periods_validate()](series-periods-validatefunction.md)|Checks whether a time series contains periodic patterns of given lengths.|
|[series_seasonal()](series-seasonalfunction.md)|Finds the seasonal component of the series.|
|[series_stats()](series-statsfunction.md)|Returns statistics for a series in multiple columns.|
|[series_stats_dynamic()](series-stats-dynamicfunction.md)|Returns statistics for a series in dynamic object.|

## String functions

|Function Name     |Description                                          |
|-------------------------|--------------------------------------------------------|
|[base64_encode_tostring()](base64_encode_tostringfunction.md)|Encodes a string as base64 string.|
|[base64_decode_tostring()](base64_decode_tostringfunction.md)|Decodes a base64 string to a UTF-8 string.|
|[base64_decode_toarray()](base64_decode_toarrayfunction.md)|Decodes a base64 string to an array of long values.|
|[countof()](countoffunction.md)|Counts occurrences of a substring in a string. Plain string matches may overlap; regex matches don't.|
|[extract()](extractfunction.md)|Get a match for a regular expression from a text string.|
|[extract_all()](extractallfunction.md)|Get all matches for a regular expression from a text string.|
|[extractjson()](extractjsonfunction.md)|Get a specified element out of a JSON text using a path expression.|
|[indexof()](indexoffunction.md)|Function reports the zero-based index of the first occurrence of a specified string within input string.|
|[isempty()](isemptyfunction.md)|Returns true if the argument is an empty string or is null.|
|[isnotempty()](isnotemptyfunction.md)|Returns true if the argument isn't an empty string or a null.|
|[isnotnull()](isnotnullfunction.md)|Returns true if the argument is not null.|
|[isnull()](isnullfunction.md)|Evaluates its sole argument and returns a bool value indicating if the argument evaluates to a null value.|
|[parse_command_line()](parse-command-line.md)|Parses a Unicode command line string and returns an array of the command line arguments.|
|[parse_csv()](parsecsvfunction.md)|Splits a given string representing comma-separated values and returns a string array with these values.|
|[parse_ipv4()](parse-ipv4function.md)|Converts input to long (signed 64-bit) number representation.|
|[parse_ipv4_mask()](parse-ipv4-maskfunction.md)|Converts input string and IP-prefix mask to long (signed 64-bit) number representation.|
|[parse_ipv6()](parse-ipv6function.md)|Converts IPv6 or IPv4 string to a canonical IPv6 string representation.|
|[parse_ipv6_mask()](parse-ipv6-maskfunction.md)|Converts IPv6 or IPv4 string and netmask to a canonical IPv6 string representation.|
|[parse_json()](parsejsonfunction.md)|Interprets a string as a JSON value) and returns the value as dynamic.|
|[parse_url()](parseurlfunction.md)|Parses an absolute URL string and returns a dynamic object contains all parts of the URL.|
|[parse_urlquery()](parseurlqueryfunction.md)|Parses a url query string and returns a dynamic object contains the Query parameters.|
|[parse_version()](parse-versionfunction.md)|Converts input string representation of version to a comparable decimal number.|
|[replace()](replacefunction.md)|Replace all regex matches with another string.|
|[reverse()](reversefunction.md)|Function makes reverse of input string.|
|[split()](splitfunction.md)|Splits a given string according to a given delimiter and returns a string array with the contained substrings.|
|[strcat()](strcatfunction.md)|Concatenates between 1 and 64 arguments.|
|[strcat_delim()](strcat-delimfunction.md)|Concatenates between 2 and 64 arguments, with delimiter, provided as first argument.|
|[strcmp()](strcmpfunction.md)|Compares two strings.|
|[strlen()](strlenfunction.md)|Returns the length, in characters, of the input string.|
|[strrep()](strrepfunction.md)|Repeats given string provided number of times (default - 1).|
|[substring()](substringfunction.md)|Extracts a substring from a source string starting from some index to the end of the string.|
|[toupper()](toupperfunction.md)|Converts a string to upper case.|
|[translate()](translatefunction.md)|Replaces a set of characters ('searchList') with another set of characters ('replacementList') in a given a string.|
|[trim()](trimfunction.md)|Removes all leading and trailing matches of the specified regular expression.|
|[trim_end()](trimendfunction.md)|Removes trailing match of the specified regular expression.|
|[trim_start()](trimstartfunction.md)|Removes leading match of the specified regular expression.|
|[url_decode()](urldecodefunction.md)|The function converts encoded URL into a regular URL representation.|
|[url_encode()](urlencodefunction.md)|The function converts characters of the input URL into a format that can be transmitted over the Internet.|

## IPv4/IPv6 functions

|Function Name     |Description                                          |
|-------------------------|--------------------------------------------------------|
|[ipv4_compare()](ipv4-comparefunction.md)|Compares two IPv4 strings.|
|[ipv4_is_in_range()](ipv4-is-in-range-function.md)|Checks if IPv4 string address is in IPv4-prefix notation range.|
|[ipv4_is_match()](ipv4-is-matchfunction.md)|Matches two IPv4 strings.|
|[ipv4_is_private()](ipv4-is-privatefunction.md)|Checks if IPv4 string address belongs to a set of private network IPs.|
|[ipv4_netmask_suffix](ipv4-netmask-suffix-function.md)|Returns the value of the IPv4 netmask suffix from IPv4 string address.|
|[parse_ipv4()](parse-ipv4function.md)|Converts input string to long (signed 64-bit) number representation.|
|[parse_ipv4_mask()](parse-ipv4-maskfunction.md)|Converts input string and IP-prefix mask to long (signed 64-bit) number representation.|
|[ipv6_compare()](ipv6-comparefunction.md)|Compares two IPv4 or IPv6 strings.|
|[ipv6_is_match()](ipv6-is-matchfunction.md)|Matches two IPv4 or IPv6 strings.|
|[parse_ipv6()](parse-ipv6function.md)|Converts IPv6 or IPv4 string to a canonical IPv6 string representation.|
|[parse_ipv6_mask()](parse-ipv6-maskfunction.md)|Converts IPv6 or IPv4 string and netmask to a canonical IPv6 string representation.|
|[format_ipv4()](format-ipv4-function.md)|Parses input with a netmask and returns string representing IPv4 address.|
|[format_ipv4_mask()](format-ipv4-mask-function.md)|Parses input with a netmask and returns string representing IPv4 address as CIDR notation.|

## Type functions

|Function Name     |Description                                          |
|-------------------------|--------------------------------------------------------|
|[gettype()](gettypefunction.md)|Returns the runtime type of its single argument.|

## Scalar aggregation functions

|Function Name     |Description                                          |
|-------------------------|--------------------------------------------------------|
|[dcount_hll()](dcount-hllfunction.md)|Calculates the dcount from hll results (which was generated by hll or hll-merge).|
|[hll_merge()](hllmergefunction.md)|Merges hll results (scalar version of the aggregate version hll-merge()).|
|[percentile_tdigest()](percentile-tdigestfunction.md)|Calculates the percentile result from tdigest results (which was generated by tdigest or merge-tdigests).|
|[percentrank_tdigest()](percentrank-tdigestfunction.md)|Calculates the percentage ranking of a value in a dataset.|
|[rank_tdigest()](rank-tdigest.md)|Calculates relative rank of a value in a set.|
|[tdigest_merge()](tdigest-mergefunction.md)|Merges tdigest results (scalar version of the aggregate version tdigest-merge()).|

## Geospatial functions

|Function Name|Description|
|--------------------------------------------------------------------------|--------------------------------------------------------|
|[geo_distance_2points()](geo-distance-2points-function.md)|Calculates the shortest distance between two geospatial coordinates on Earth.|
|[geo_distance_point_to_line()](geo-distance-point-to-line-function.md)|Calculates the shortest distance between a coordinate and a line on Earth.|
|[geo_point_in_circle()](geo-point-in-circle-function.md)|Calculates whether the geospatial coordinates are inside a circle on Earth.|
|[geo_point_in_polygon()](geo-point-in-polygon-function.md)|Calculates whether the geospatial coordinates are inside a polygon or a multipolygon on Earth.|
|[geo_point_to_geohash()](geo-point-to-geohash-function.md)|Calculates the Geohash string value for a geographic location.|
|[geo_geohash_to_central_point()](geo-geohash-to-central-point-function.md)|Calculates the geospatial coordinates that represent the center of a Geohash rectangular area.|
|[geo_point_to_s2cell()](geo-point-to-s2cell-function.md)|Calculates the S2 cell token string value for a geographic location.|
|[geo_s2cell_to_central_point()](geo-s2cell-to-central-point-function.md)|Calculates the geospatial coordinates that represent the center of an S2 cell.|
|[geo_polygon_to_s2cells()](geo-polygon-to-s2cells-function.md)|Calculates S2 cell tokens that cover a polygon or multipolygon on Earth. Useful geospatial join tool.|
|[geo_line_densify()](geo-line-densify-function.md)|Converts planar line edges to geodesics by adding intermediate points.|
|[geo_polygon_densify()](geo-polygon-densify-function.md)|Converts polygon or multipolygon planar edges to geodesics by adding intermediate points.|

## Hash functions

|Function Name|Description|
|--------------------------------------------------------------------------|--------------------------------------------------------|
|[hash()](hashfunction.md)|Returns a hash value for the input value.|
|[hash_combine()](hash_combinefunction.md)|Combines two or more hash values.|
|[hash_many()](hash_manyfunction.md)|Returns a combined hash value of multiple values.|
|[hash_md5()](md5hashfunction.md)|Returns a MD5 hash value for the input value.|
|[hash_sha256()](sha256hashfunction.md)|Returns a SHA256 hash value for the input value.|
