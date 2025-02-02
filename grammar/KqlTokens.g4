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
ATSYMBOL: '@';
BAR: '|';
CLOSE_BRACE: '}';
CLOSE_BRACKET: ']';
CLOSE_PAREN: ')';
COMMA: ',';
COLON: ':';
DASH: '-';
DOT: '.';
DOTDOT: '..';
EQUAL: '=';
EQUALEQUAL: '==';
EXCLAIMATIONPOINT: '!';
EXCLAIMATIONPOINT_EQUAL: '!=';
GREATERTHAN: '>';
GREATERTHAN_EQUAL: '>=';
LESSTHAN: '<';
LESSTHAN_EQUAL: '<=';
LESSTHAN_GREATERTHAN: '<>';
OPEN_BRACE: '{';
OPEN_BRACKET: '[';
OPEN_PAREN: '(';
PLUS: '+';
QUESTIONMARK: '?';
SEMICOLON: ';';
SLASH: '/';
FATARROW: '=>';

// keyword tokens
CHART3D_: '3Dchart';
ACCESS: 'access';
ACCUMULATE: 'accumulate';
ADMINS: 'admins';
AGGREGATIONS: 'aggregations';
ALIAS: 'alias';
ALL: 'all';
AND: 'and';
ANOMALYCHART: 'anomalychart';
ANOMALYCOLUMNS: 'anomalycolumns';
AREACHART: 'areachart';
AS: 'as';
ASC: 'asc';
AXES: 'axes';
BARCHART: 'barchart';
BASE: 'base';
BETWEEN: 'between';
BIN: 'bin';
BY: 'by';
CARD: 'card';
CLUSTER: 'cluster';
COLUMNCHART: 'columnchart';
CONSUME: 'consume';
CONTAINS: 'contains';
CONTEXTUAL_DATATABLE: '__contextual_datatable';
COUNT: 'count';
DATABASE: 'database';
DATASCOPE: 'datascope';
DATATABLE: 'datatable';
DECLARE: 'declare';
DEFAULT: 'default';
DELTA: 'delta';
DESC: 'desc';
DISTINCT: 'distinct';
ENTITYGROUP: 'entity_group';
EVALUATE: 'evaluate';
EXECUTE: 'execute';
EXECUTE_AND_CACHE: '__executeAndCache';
EXTEND: 'extend';
EXTERNALDATA: 'externaldata';
EXTERNAL_DATA: 'external_data';
FACET: 'facet';
FILTER: 'filter';
FIND: 'find';
FIRST: 'first';
FLAGS: 'flags';
FOLDER: 'folder';
FORK: 'fork';
FROM: 'from';
FUNCTION: 'function';
GETSCHEMA: 'getschema';
GRANNYASC: 'granny-asc';
GRANNYDESC: 'granny-desc';
GROUPS: 'groups';
HAS: 'has';
HAS_ALL: 'has_all';
HAS_ANY: 'has_any';
HIDDEN_: 'hidden';
HOT: 'hot';
HOTCACHE: 'hotcache';
HOTDATA: 'hotdata';
HOTINDEX: 'hotindex';
ID: 'id';
IN: 'in';
IN_CI: 'in~';
INTO: 'into';
INVOKE: 'invoke';
JOIN: 'join';
KIND: 'kind';
LADDERCHART: 'ladderchart';
LAST: 'last';
LEGEND: 'legend';
LET: 'let';
LIMIT: 'limit';
LINEAR: 'linear';
LINECHART: 'linechart';
LIST: 'list';
LOOKUP: 'lookup';
LOG: 'log';
MACROEXPAND: 'macro-expand';
MAKE_SERIES: 'make-series';
MAP: 'map';
MATERIALIZE: 'materialize';
MATERIALIZED_VIEW: 'materialized-view';
MATERIALIZED_VIEW_COMBINE: 'materialized-view-combine';
MV_APPLY: 'mv-apply';
MV_EXPAND: 'mv-expand';
MVAPPLY: 'mvapply';
MVEXPAND: 'mvexpand';
NONE: 'none';
NOOPTIMIZATION: 'nooptimization';
NOT_BETWEEN: '!between';
NOT_IN: '!in';
NOT_IN_CI: '!in~';
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
PARSEWHERE: 'parse-where';
PARTITION: 'partition';
PARTITIONBY: '__partitionby';
PATTERN: 'pattern';
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
STACKED: 'stacked';
STACKED100: 'stacked100';
STACKEDAREACHART: 'stackedareachart';
STEP: 'step';
STORED_QUERY_RESULTS: 'stored-query-results';
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
TYPE: 'type';
TYPEOF: 'typeof';
UNION: 'union';
UNSTACKED: 'unstacked';
UUID: 'uuid';
VIEW: 'view';
VISIBLE: 'visible';
WHERE: 'where';
WITH: 'with';
WITHSOURCE: 'withsource';
WITH_SOURCE: 'with_source';

XAXIS: 'xaxis';
XCOLUMN: 'xcolumn';
XMIN: 'xmin';
XMAX: 'xmax';
XTITLE: 'xtitle';
YAXIS: 'yaxis';
YCOLUMNS: 'ycolumns';
YMIN: 'ymin';
YMAX: 'ymax';
YSPLIT: 'ysplit';
YTITLE: 'ytitle';
BOOL: 'bool';
BOOLEAN: 'boolean';
INT8: 'int8';
CHAR: 'char';
UINT8: 'uint8';
BYTE: 'byte';
INT16: 'int16';
UINT16: 'uint16';
INT: 'int';
INT32: 'int32';
UINT: 'uint';
UINT32: 'uint32';
LONG: 'long';
INT64: 'int64';
ULONG: 'ulong';
UINT64: 'uint64';
FLOAT: 'float';
REAL: 'real';
DOUBLE: 'double';
STRING: 'string';
TIME: 'time';
TIMESPAN: 'timespan';
DATE: 'date';
DATETIME: 'datetime';
GUID: 'guid';
UNIQUEID: 'uniqueid';
DYNAMIC: 'dynamic';
DECIMAL: 'decimal';

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

UUIDLITERAL:
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

SP:
    (WHITESPACE)+ -> channel(HIDDEN);

WHITESPACE:
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
;

LineComment:
    '//' ~('\n' | '\r' | '\u2028' | '\u2029')* -> channel(HIDDEN);
