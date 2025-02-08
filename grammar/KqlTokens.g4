lexer grammar KqlTokens;

fragment EscapeSequence:
    '\\' (
          '\''
        | '"'
        | '\\'
        | 'a'
        | 'b'
        | 'f'
        | 'n'
        | 'r'
        | 't'
        | 'u'
        | 'U'
        | 'x'
        | 'v'
        | ('0'..'3') ('0'..'7') ('0'..'7')
        | ('0'..'7') ('0'..'7')
        | ('0'..'7')
    )
;

// punctuation tokens
ASTERISK: '*';
ATSIGN: '@';
BAR: '|';
CLOSEBRACE: '}';
CLOSEBRACKET: ']';
CLOSEBRACKET_DASH: ']-';
CLOSEBRACKET_DASH_GREATERTHAN: ']->';
CLOSEPAREN: ')';
COMMA: ',';
COLON: ':';
DASH: '-';
DASHDASH: '--';
DASHDASH_GREATERTHAN: '-->';
DASH_OPENBRACKET: '-[';
DOT: '.';
DOTDOT: '..';
EQUAL: '=';
EQUALEQUAL: '==';
EQUALTILDE: '=~';
EXCLAIMATIONPOINT_EQUAL: '!=';
EXCLAIMATIONPOINT_TILDE: '!~';
GREATERTHAN: '>';
GREATERTHAN_EQUAL: '>=';
LESSTHAN: '<';
LESSTHAN_DASHDASH: '<--';
LESSTHAN_DASH_OPENBRACKET: '<-[';
LESSTHAN_EQUAL: '<=';
LESSTHAN_GREATERTHAN: '<>';
OPENBRACE: '{';
OPENBRACKET: '[';
OPENPAREN: '(';
PERCENTSIGN: '%';
PLUS: '+';
SEMICOLON: ';';
SLASH: '/';
EQUAL_GREATERTHAN: '=>';

// keywords and contextual keywords
CHART3D_: '3Dchart';
ACCESS: 'access';
ACCUMULATE: 'accumulate';
AGGREGATIONS: 'aggregations';
ALIAS: 'alias';
ALL: 'all';
AND: 'and';
ANOMALYCHART: 'anomalychart';
ANOMALYCOLUMNS: 'anomalycolumns';
AREACHART: 'areachart';
AS: 'as';
ASC: 'asc';
ASSERTSCHEMA: 'assert-schema';
AXES: 'axes';
BAGEXPANSION: 'bagexpansion';
BARCHART: 'barchart';
BASE: 'base';
BETWEEN: 'between';
BIN: 'bin';
BIN_LEGACY: 'bin_legacy';
BY: 'by';
CARD: 'card';
CLUSTER: 'cluster';
COLUMNCHART: 'columnchart';
CONSUME: 'consume';
CONTAINS: 'contains';
CONTAINSCS: 'containscs';
CONTAINS_CS: 'contains_cs';
CONTEXTUAL_DATATABLE: '__contextual_datatable';
COUNT: 'count';
CROSSCLUSTER__: '__crossCluster';
CROSSDB__: '__crossDB';
DATABASE: 'database';
DATASCOPE: 'datascope';
DATATABLE: 'datatable';
DECLARE: 'declare';
DECODEBLOCKS: 'decodeblocks';
DEFAULT: 'default';
DELTA: 'delta';
DESC: 'desc';
DISTINCT: 'distinct';
EDGES: 'edges';
ENDSWITH: 'endswith';
ENDSWITH_CS: 'endswith_cs';
ENTITYGROUP: 'entity_group';
EVALUATE: 'evaluate';
EXECUTE: 'execute';
EXECUTE_AND_CACHE: '__executeAndCache';
EXPANDOUTPUT: 'expandoutput';
EXTEND: 'extend';
EXTERNALDATA: 'externaldata';
EXTERNAL_DATA: 'external_data';
FACET: 'facet';
FILTER: 'filter';
FIND: 'find';
FIRST: 'first';
FLAGS: 'flags';
FORK: 'fork';
FROM: 'from';
GETSCHEMA: 'getschema';
GRANNYASC: 'granny-asc';
GRANNYDESC: 'granny-desc';
GRAPHMARKCOMPONENTS: 'graph-mark-components';
GRAPHMATCH: 'graph-match';
GRAPHMERGE: 'graph-merge';
GRAPHSHORTESTPATHS: 'graph-shortest-paths';
GRAPHTOTABLE: 'graph-to-table';
HAS: 'has';
HAS_ALL: 'has_all';
HAS_ANY: 'has_any';
HAS_CS: 'has_cs';
HASPREFIX: 'hasprefix';
HASPREFIX_CS: 'hasprefix_cs';
HASSUFFIX: 'hassuffix';
HASSUFFIX_CS: 'hassuffix_cs';
HIDDEN_: 'hidden';
HINT_CONCURRENCY: 'hint.concurrency';
HINT_DISTRIBUTION: 'hint.distribution';
HINT_MATERIALIZED: 'hint.materialized';
HINT_NUM_PARTITIONS: 'hint.num_partitions';
HINT_PASS_FILTERS: 'hint.pass_filters';
HINT_PASS_FILTERS_COLUMN: 'hint.pass_filters_column';
HINT_PROGRESSIVE_TOP: 'hint.progressive_top';
HINT_REMOTE: 'hint.remote';
HINT_SUFFLEKEY: 'hint.shufflekey';
HINT_SPREAD: 'hint.spread';
HINT_STRATEGY: 'hint.strategy';
HOT: 'hot';
HOTCACHE: 'hotcache';
HOTDATA: 'hotdata';
HOTINDEX: 'hotindex';
ID: 'id';
ID__: '__id';
IN: 'in';
IN_CI: 'in~';
INTO: 'into';
INVOKE: 'invoke';
ISFUZZY: 'isfuzzy';
ISFUZZY__: '__isFuzzy';
JOIN: 'join';
KIND: 'kind';
LADDERCHART: 'ladderchart';
LAST: 'last';
LEGEND: 'legend';
LET: 'let';
LIKE: 'like';
LIKECS: 'likecs';
LIMIT: 'limit';
LINEAR: 'linear';
LINECHART: 'linechart';
LIST: 'list';
LOOKUP: 'lookup';
LOG: 'log';
MACROEXPAND: 'macro-expand';
MAKEGRAPH: 'make-graph';
MAKESERIES: 'make-series';
MAP: 'map';
MATCHES_REGEX: 'matches regex';
MATERIALIZE: 'materialize';
MATERIALIZED_VIEW_COMBINE: 'materialized-view-combine';
MV_APPLY: 'mv-apply';
MV_EXPAND: 'mv-expand';
MVAPPLY: 'mvapply';
MVEXPAND: 'mvexpand';
NODES: 'nodes';
NONE: 'none';
NOOPTIMIZATION: 'nooptimization';
NOT_BETWEEN: '!between';
NOT_CONTAINS: '!contains';
NOT_CONTAINS_CS: '!contains_cs';
NOT_ENDSWITH_CS: '!endswith_cs';
NOT_ENDSWITH: '!endswith';
NOT_HAS: '!has';
NOT_HAS_CS: '!has_cs';
NOT_HASPREFIX: '!hasprefix';
NOT_HASPREFIX_CS: '!hasprefix_cs';
NOT_HASSUFFIX: '!hassuffix';
NOT_HASSUFFIX_CS: '!hassuffix_cs';
NOT_IN: '!in';
NOT_IN_CI: '!in~';
NOT_STARTSWITH: '!startswith';
NOT_STARTSWITH_CS: '!startswith_cs';
NOTCONTAINS: 'notcontains';
NOTCONTAINSCS: 'notcontainscs';
NOTLIKE: 'notlike';
NOTLIKECS: 'notlikecs';
NULL: 'null';
NULLS: 'nulls';
OF: 'of';
ON: 'on';
OPTIONAL: 'optional';
OR: 'or';
ORDER: 'order';
OTHERS: 'others';
OUTPUT: 'output';
PACK: 'pack';
PANELS: 'panels';
PARSE: 'parse';
PARSEKV: 'parse-kv';
PARSEWHERE: 'parse-where';
PARTITION: 'partition';
PARTITIONBY: '__partitionby';
PARTITIONEDBY: 'partitioned-by';
PATTERN: 'pattern';
PACKEDCOLUMN__: '__packedColumn';
PIECHART: 'piechart';
PIVOTCHART: 'pivotchart';
PLUGIN: 'plugin';
PRINT: 'print';
PROJECT: 'project';
PROJECTAWAY: 'project-away';
PROJECTAWAY_: '__projectAway';
PROJECTKEEP: 'project-keep';
PROJECTRENAME: 'project-rename';
PROJECTREORDER: 'project-reorder';
PROJECTSMART: 'project-smart';
QUERYPARAMETERS: 'query_parameters';
RANGE: 'range';
REDUCE: 'reduce';
REGEX: 'regex';
RELAXED: 'relaxed';
RENDER: 'render';
REPLACE: 'replace';
RESTRICT: 'restrict';
SAMPLE: 'sample';
SAMPLE_DISTINCT: 'sample-distinct';
SCAN: 'scan';
SCATTERCHART: 'scatterchart';
SEARCH: 'search';
SERIALIZE: 'serialize';
SERIES: 'series';
SET: 'set';
SIMPLE: 'simple';
SORT: 'sort';
SOURCECOLUMNINDEX__: '__sourceColumnIndex';
STACKED: 'stacked';
STACKED100: 'stacked100';
STACKEDAREACHART: 'stackedareachart';
STARTSWITH: 'startswith';
STARTSWITH_CS: 'startswith_cs';
STEP: 'step';
SUMMARIZE: 'summarize';
TABLE: 'table';
TAKE: 'take';
THRESHOLD: 'threshold';
TIMECHART: 'timechart';
TIMELINE: 'timeline';
TIMEPIVOT: 'timepivot';
TITLE: 'title';
TO: 'to';
TOP: 'top';
TOP_HITTERS: 'top-hitters';
TOP_NESTED: 'top-nested';
TOSCALAR: 'toscalar';
TOTABLE: 'totable';
TREEMAP: 'treemap';
TYPEOF: 'typeof';
UNION: 'union';
UNSTACKED: 'unstacked';
UUID: 'uuid';
VIEW: 'view';
VISIBLE: 'visible';
WHERE: 'where';
WITH: 'with';
WITHNOSOURCE__: '__noWithSource';
WITHSOURCE: 'withsource';
WITH_ITEM_INDEX: 'with_itemindex';
WITH_MATCH_ID: 'with_match_id';
WITH_NODE_ID: 'with_node_id';
WITH_SOURCE: 'with_source';
WITH_STEP_NAME: 'with_step_name';
XAXIS: 'xaxis';
XCOLUMN: 'xcolumn';
XMAX: 'xmax';
XMIN: 'xmin';
XTITLE: 'xtitle';
YAXIS: 'yaxis';
YCOLUMNS: 'ycolumns';
YMAX: 'ymax';
YMIN: 'ymin';
YSPLIT: 'ysplit';
YTITLE: 'ytitle';

// types
BOOL: 'bool';
BOOLEAN: 'boolean';
DATE: 'date';
DATETIME: 'datetime';
DECIMAL: 'decimal';
DOUBLE: 'double';
DYNAMIC: 'dynamic';
FLOAT: 'float';
GUID: 'guid';
INT: 'int';
INT8: 'int8';
INT16: 'int16';
INT32: 'int32';
INT64: 'int64';
LONG: 'long';
STRING: 'string';
REAL: 'real';
TIME: 'time';
TIMESPAN: 'timespan';
UINT: 'uint';
UINT8: 'uint8';
UINT16: 'uint16';
UINT32: 'uint32';
UINT64: 'uint64';
ULONG: 'ulong';
UNIQUEID: 'uniqueid';

fragment LparenGooRparen:
    '(' (~')')* ')';

LONGLITERAL:
      IntegerNumber
    | LONG LparenGooRparen
    | INT64 LparenGooRparen
;

INTLITERAL:
      INT LparenGooRparen
    | INT32 LparenGooRparen
;

fragment NonHexIntegerNumber:
    ('0'..'9')+;

fragment IntegerNumber:
      NonHexIntegerNumber
    | HexPrefix HexDigit+
;

fragment SignedIntegerNumber:
    PlusOrMinus NonHexIntegerNumber;

fragment HexPrefix:
      '0x'
    | '0X'
;

fragment HexDigit:
    ('0'..'9' | 'a'..'f' | 'A'..'F');

REALLITERAL:
      NonIntegerNumber
    | REAL LparenGooRparen
    | DOUBLE LparenGooRparen
;

DECIMALLITERAL:
    DECIMAL LparenGooRparen;

fragment PlusOrMinus:
    ('+' | '-');

fragment Exponent:
    ('e' | 'E') PlusOrMinus? ('0'..'9')+;

fragment NonIntegerNumber:
      ('0'..'9')+ '.' ('0'..'9')* Exponent?
    | ('0'..'9')+ Exponent
;

fragment MultiLineStringQuote: '```';
fragment AlternateMultiLineStringQuote: '~~~';

STRINGLITERAL:
      ('h' | 'H')? '"' (EscapeSequence | ~('\\' | '"' | '\r' | '\n'))* '"'
    | ('h' | 'H')? '\'' (EscapeSequence | ~('\'' | '\\' | '\r' | '\n'))* '\''
    | ('h' | 'H')? '@"' ('""' | ~('"' | '\r' | '\n'))* '"'
    | ('h' | 'H')? '@\'' ('\'\'' | ~('\'' | '\r' | '\n'))* '\''
    | ('h' | 'H')? MultiLineStringQuote (.)*? MultiLineStringQuote
    | ('h' | 'H')? AlternateMultiLineStringQuote (.)*? AlternateMultiLineStringQuote
;

BOOLEANLITERAL:
      'true'
    | 'false'
    | 'TRUE'
    | 'FALSE'
    | 'True'
    | 'False'
    | BOOL LparenGooRparen
;

DATETIMELITERAL:
    DATETIME LparenGooRparen;

fragment TimespanNumber:
    ('0'..'9')+ ('.' ('0'..'9')+)?;

TIMESPANLITERAL:
      TimespanNumber 'm' ((('in') ('ute')?)? | 'inutes')
    | TimespanNumber 's' ((('ec') ('ond')?)? | 'econds')
    | TimespanNumber 'd' (('ay') ('s')?)?
    | TimespanNumber 'h' (('our') ('s')?)?
    | TimespanNumber 'hr' ('s')?
    | TimespanNumber 'ms'
    | TimespanNumber 'milli' ('s' ((('ec') ('ond')?)? | 'econds'))?
    | TimespanNumber 'micro' ('s' ((('ec') ('ond')?)? | 'econds'))?
    | TimespanNumber 'nano' ('s' ((('ec') ('ond')?)? | 'econds'))?
    | TimespanNumber 'tick' ('s')?
    | TIME LparenGooRparen
    | TIMESPAN LparenGooRparen
;

TYPELITERAL:
    TYPEOF LparenGooRparen;

RAWGUIDLITERAL:
    EightHexDigits '-' FourHexDigits '-' FourHexDigits '-' FourHexDigits '-' TwelveHexDigits;

GUIDLITERAL:
      GUID LparenGooRparen
    | UUID LparenGooRparen
    | UNIQUEID LparenGooRparen
    ;

fragment FourHexDigits:
    HexDigit HexDigit HexDigit HexDigit;

fragment EightHexDigits:
    FourHexDigits FourHexDigits;

fragment TwelveHexDigits:
    EightHexDigits FourHexDigits;

IDENTIFIER:
      ('$' | '_' | 'a'..'z' | 'A'..'Z') ('_' | 'a'..'z' | 'A'..'Z' | '0'..'9')*
    | ('0'..'9')+ ('_' | 'a'..'z' | 'A'..'Z') ('_' | 'a'..'z' | 'A'..'Z' | '0'..'9')*
    ;

WHITESPACE:
    (
          '\t'
        | ' '
        | '\r'
        | '\n'
        | '\f'
        | '\u00a0'
        | '\u1680'
        | '\u180e'
        | '\u2000'
        | '\u2001'
        | '\u2002'
        | '\u2003'
        | '\u2004'
        | '\u2005'
        | '\u2006'
        | '\u2007'
        | '\u2008'
        | '\u2009'
        | '\u200a'
        | '\u200b'
        | '\u202f'
        | '\u205f'
        | '\u3000'
        | '\ufeff'
    )+ 
    -> channel(HIDDEN);

COMMENT:
    '//' ~('\n' | '\r' | '\u2028' | '\u2029')* -> channel(HIDDEN);
