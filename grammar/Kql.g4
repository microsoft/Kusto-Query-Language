grammar Kql;

import KqlTokens;

top:
    query;

query:
    Statements+=statement (';' Statements+=statement)* (';')? EOF;

statement:
      AliasDatabase=aliasDatabaseStatement
    | DeclarePattern=declarePatternStatement
    | DeclareQueryParameters=declareQueryParametersStatement
    | Let=letStatement
    | Query=queryStatement
    | RestrictAccess=restrictAccessStatement
    | Set=setStatement
;

aliasDatabaseStatement:
    ALIAS DATABASE Name=simpleNameReference '=' Expression=unnamedExpression;

letStatement:
      Function=letFunctionDeclaration
    | View=letViewDeclaration
    | Variable=letVariableDeclaration
    | Materialized=letMaterializeDeclaration
    ;

letVariableDeclaration:
    LET Name=simpleNameReference '=' Expression=expression;

letFunctionDeclaration:
    LET Name=simpleNameReference '=' '(' (ParameterList=letFunctionParameterList)? ')' Body=letFunctionBody;

letViewDeclaration:
    LET Name=simpleNameReference '=' VIEW '(' (ParameterList=letViewParameterList)? ')' Body=letFunctionBody;

letViewParameterList:
    Parameters+=scalarParameter (',' Parameters+=scalarParameter)*;

letMaterializeDeclaration:
    LET Name=simpleNameReference '=' MATERIALIZE '(' Expression=pipeExpression ')';

letFunctionParameterList:
    TabularParameters+=tabularParameter (',' TabularParameters+=tabularParameter) (',' ScalarParameters+=scalarParameter)
    | ScalarParameters+= scalarParameter (',' ScalarParameters+=scalarParameter)
    ;

scalarParameter:
    Name=parameterName ':' Type=parameterType (Default=scalarParameterDefault)?;

scalarParameterDefault:
    '=' Value=literalExpression;

tabularParameter:
    Name=parameterName ':' (OpenSchema=tabularParameterOpenSchema | RowSchema=tabularParameterRowSchema);

tabularParameterOpenSchema:
    '(' '*' ')';

tabularParameterRowSchema:
    '(' Columns+=tabularParameterRowSchemaColumnDeclaration (',' Columns+=tabularParameterRowSchemaColumnDeclaration)* ')';

tabularParameterRowSchemaColumnDeclaration:
    Name=parameterName ':' Type=parameterType;

letFunctionBody:
    '{' (Statements+=letFunctionBodyStatement ';')* (Expression=expression)? (';')? '}';

letFunctionBodyStatement:
      Let=letStatement
	| DeclareQueryParameters=declareQueryParametersStatement
    ;


declarePatternStatement:
    DECLARE PATTERN Name=simpleNameReference (Definition=declarePatternDefinition)?;

declarePatternDefinition:
    '=' ParameterList=declarePatternParameterList (Path=declarePatternPathParameter)? '{' (declarePatternRule)+ '}';

declarePatternParameterList:
    '(' Parameters+=declarePatternParameter (',' Parameters+=declarePatternParameter)* ')';

declarePatternParameter:
	Name=parameterName ':' Type=parameterType;

declarePatternPathParameter:
    '[' Parameter=declarePatternParameter ']';

declarePatternRule:
    ArgumentList=declarePatternRuleArgumentList (PathArgument=declarePatternRulePathArgument)? '=' Body=declarePatternBody (TrailingSemicolon=';')?;

declarePatternRuleArgumentList:
    '(' Arguments+=declarePatternRuleArgument (',' Arguments+=declarePatternRuleArgument)* ')';

declarePatternRulePathArgument:
    '.' '[' Expression=declarePatternRuleArgument ']';

declarePatternRuleArgument:
    stringLiteralExpression;

declarePatternBody:
    '{' (Statements+=letFunctionBodyStatement ';')* Expression=expression '}';

restrictAccessStatement:
    RESTRICT ACCESS TO '(' Entities+=restrictAccessStatementEntity (',' Entities+=restrictAccessStatementEntity)* ')';

restrictAccessStatementEntity:
      SimpleName=simpleNameReference
    | WildcardedEntity=wildcardedEntityExpression
    ;

setStatement:
    SET Name=identifierOrKeywordName ('=' Value=setStatementOptionValue)?;

setStatementOptionValue:
      Name=identifierOrKeywordName
    | Literal=literalExpression
    ;

declareQueryParametersStatement:
     DECLARE QUERYPARAMETERS '(' Parameters+=declareQueryParametersStatementParameter (',' Parameters+=declareQueryParametersStatementParameter)* ')';

declareQueryParametersStatementParameter:
    Name=parameterName ':' Type=parameterType (Default=scalarParameterDefault)?;

queryStatement:
    Expression=expression;

// query expressions

expression:
    pipeExpression;

pipeExpression:
    Expression=beforePipeExpression (pipedOperator)*;

pipedOperator:
    '|' Operator=afterPipeOperator;

pipeSubExpression:
    Expression=afterPipeOperator (pipedOperator)*;

afterPipeOperator:
    queryOperator;

beforePipeExpression:
      unnamedExpression 
    | rangeExpression 
    | printOperator 
    | queryOperatorPipedOrUnpiped 
    | entityGroupExpression 
    | macroExpandOperator 
    | scopedFunctionCallExpression
    ;

queryOperator:
      consumeOperator
    | countOperator
    | executeAndCacheOperator
    | extendOperator
    | facetByOperator
    | whereOperator
    | getSchemaOperator
    | findOperator
    | searchOperator
    | forkOperator
    | partitionOperator
    | partitionByOperator
    | joinOperator
    | makeSeriesOperator
    | mvexpandOperator
    | mvapplyOperator
    | evaluateOperator
    | parseOperator
    | parseWhereOperator
    | projectOperator
    | sampleOperator
    | sampleDistinctOperator
    | projectAwayOperator
    | projectRenameOperator
    | projectReorderOperator
    | projectKeepOperator
    | reduceByOperator
    | summarizeOperator
    | distinctOperator
    | takeOperator
    | sortOperator
    | topHittersOperator
    | topOperator
    | topNestedOperator
    | unionOperator
    | renderOperator
    | asOperator
    | serializeOperator
    | invokeOperator
    | lookupOperator
    | scanOperator
    ;

queryOperatorPipedOrUnpiped:
      findOperator
    | searchOperator
    | unionOperator
    | evaluateOperator
    ;

forkPipeOperator:
      countOperator
    | extendOperator
    | whereOperator
    | parseOperator
    | parseWhereOperator
    | takeOperator
    | topNestedOperator
    | projectOperator
    | projectAwayOperator
    | projectRenameOperator
    | projectReorderOperator
    | projectKeepOperator
    | summarizeOperator
    | distinctOperator
    | topHittersOperator
    | topOperator
    | sortOperator
    | mvexpandOperator
    | reduceByOperator
    | sampleOperator
    | sampleDistinctOperator
    | asOperator
    | invokeOperator
    | executeAndCacheOperator
    | scanOperator
    ;

asOperator:
    AS (Parameters=queryOperatorParameters)? Name=identifierOrKeywordOrEscapedName;

consumeOperator:
    CONSUME (Parameters=queryOperatorParameters)?;

countOperator:
    COUNT (AsClause=countOperatorAsClause)?;

countOperatorAsClause:
    AS Name=identifierName;

distinctOperator:
    DISTINCT (Parameters=queryOperatorParameters)? 
        (Star=distinctOperatorStarTarget | ColumnList=distinctOperatorColumnListTarget)
    ;

distinctOperatorStarTarget:
    '*';

distinctOperatorColumnListTarget:
    Expressions+=unnamedExpression (',' Expressions+=unnamedExpression)*;


evaluateOperator:
    EVALUATE (Parameters=queryOperatorParameters)? PlugInCall=functionCallExpression (SchemaClause=evaluateOperatorSchemaClause)?;

evaluateOperatorSchemaClause:
    ':' Schema=rowSchema;


extendOperator:
    EXTEND Expressions+=namedExpression (',' Expressions+=namedExpression)*;

executeAndCacheOperator:
    EXECUTE_AND_CACHE;

facetByOperator:
    FACET BY Entities+=entityExpression (',' Entities+=entityExpression)*
    (WithOperatorClause=facetByOperatorWithOperatorClause | WithExpressionClause=facetByOperatorWithExpressionClause)?
    ;

facetByOperatorWithOperatorClause:
    WITH Operator=forkPipeOperator;

facetByOperatorWithExpressionClause:
    WITH '(' Expression=forkOperatorExpression ')';

findOperator:
    FIND 
        (DataScopeClause=dataScopeClause)? 
        (ParameterWhereClause=findOperatorParametersWhereClause)? 
        Expression=unnamedExpression 
        (ProjectClause=findOperatorProjectClause | ProjectSmartClause=findOperatorProjectSmartClause)?
        (ProjectAwayClause=findOperatorProjectAwayClause)?;

findOperatorParametersWhereClause:
    (Parameters=queryOperatorParameters)? (InClause=findOperatorInClause)? WHERE;

findOperatorInClause:
    IN '(' Expressions+=findOperatorSource (',' Expressions+=findOperatorSource)* ')';

findOperatorProjectClause:
    PROJECT Expressions+=findOperatorProjectExpression (',' Expressions+=findOperatorProjectExpression)*;

findOperatorProjectExpression:
    Column=findOperatorColumnExpression
    | Pack=findOperatorPackExpression
    ;

findOperatorColumnExpression:
    Name=parameterName (OptionalType=findOperatorOptionalColumnType)?;

findOperatorOptionalColumnType:
    ':' Type=extendedParameterType;

findOperatorPackExpression:
    PACK '(' '*' ')';

findOperatorProjectSmartClause:
    PROJECTSMART;

findOperatorProjectAwayClause:
    PROJECTAWAY_ 
    (Star=findOperatorProjectAwayStar | ColumnList=findOperatorProjectAwayColumnList)
    ;

findOperatorProjectAwayStar:
    '*';

findOperatorProjectAwayColumnList:
    Columns+=findOperatorColumnExpression (',' Columns+=findOperatorColumnExpression)*;

findOperatorSource:
      Entity=findOperatorSourceEntityExpression
    | WildcardedEntity=wildcardedEntityExpression
    ;

findOperatorSourceEntityExpression:
    Entity=entityNameReference ('|' AsOperators+=asOperator)*;

forkOperator:
    FORK (forkOperatorFork)+;

forkOperatorFork:
    (Name=forkOperatorExpressionName)? '(' Expression=forkOperatorExpression ')';

forkOperatorExpressionName:
    Name=identifierOrKeywordOrEscapedName '=';

forkOperatorExpression:
    Operator=forkPipeOperator (forkOperatorPipedOperator)*;

forkOperatorPipedOperator:
    '|' Operator=forkPipeOperator;

getSchemaOperator:
    GETSCHEMA;

invokeOperator:
    INVOKE FunctionCall=dotCompositeFunctionCallExpression;

joinOperator:
    JOIN (Parameters=queryOperatorParameters)? Table=unnamedExpression 
    (OnClause=joinOperatorOnClause | WhereClause=joinOperatorWhereClause)?;

joinOperatorOnClause:
    ON (Expressions+=unnamedExpression (',' Expressions+=unnamedExpression)*)?;

joinOperatorWhereClause:
    WHERE Predicate=unnamedExpression;

lookupOperator:
    LOOKUP (Parameters=queryOperatorParameters)? Table=unnamedExpression OnClause=joinOperatorOnClause;


macroExpandOperator:
    MACROEXPAND (Parameters=queryOperatorParameters)? 
    EntityGroup=macroExpandEntityGroup AS Name=identifierOrKeywordOrEscapedName
    '('Statements+=statement (';' Statements+=statement)* (';')? ')';

macroExpandEntityGroup:
      entityGroupExpression
    | simpleNameReference
    | entityExpression
    ;

entityGroupExpression:
    ENTITYGROUP '[' Expressions+=unnamedExpression (',' Expressions+=unnamedExpression)* ']';

makeSeriesOperator:
    MAKE_SERIES 
    (Parameters=queryOperatorParameters)?
	Aggregations+=makeSeriesOperatorAggregation (',' Aggregations+=makeSeriesOperatorAggregation)*
    OnClause=makeSeriesOperatorOnClause
    (InRangeClause=makeSeriesOperatorInRangeClause | FromToStepClause=makeSeriesOperatorFromToStepClause)
    (ByClause=makeSeriesOperatorByClause)?;

makeSeriesOperatorOnClause:
    ON Expression=namedExpression;

makeSeriesOperatorAggregation:
    Expression=namedExpression (Default=makeSeriesOperatorExpressionDefaultClause)?;

makeSeriesOperatorExpressionDefaultClause:
    DEFAULT '=' Value=namedExpression;

makeSeriesOperatorInRangeClause:
    IN RANGE '(' FromExpression=namedExpression ToComma=',' ToExpression=namedExpression StepComma=',' StepExpression=namedExpression ')';

makeSeriesOperatorFromToStepClause:
    (FROM FromExpression=namedExpression)? (TO ToExpression=namedExpression)? STEP StepExpression=namedExpression;

makeSeriesOperatorByClause:
    BY Expressions+=namedExpression (',' Expressions+=namedExpression)*;


mvapplyOperator:
    Keyword=(MVAPPLY | MV_APPLY) 
    (Parameters=queryOperatorParameters)?
    Expressions+=mvapplyOperatorExpression (',' Expressions+=mvapplyOperatorExpression)* 
    (LimitClause=mvapplyOperatorLimitClause)? 
    (IdClause=mvapplyOperatorIdClause)? 
    ON '(' OnExpression=contextualSubExpression ')';

mvapplyOperatorLimitClause:
    LIMIT LimitValue=LONGLITERAL;

mvapplyOperatorIdClause:
    ID IdValue=GUIDLITERAL;

mvapplyOperatorExpression:
    Expression=namedExpression (ToClause=mvapplyOperatorExpressionToClause)?;

mvapplyOperatorExpressionToClause:
    TO Type=TYPELITERAL;


mvexpandOperator:
    Keyword=(MVEXPAND | MV_EXPAND) 
    (Parameters=queryOperatorParameters)? 
    Expressions+=mvexpandOperatorExpression (',' Expressions+=mvexpandOperatorExpression)* 
    (LimitClause=mvapplyOperatorLimitClause)?;

mvexpandOperatorExpression:
    Expression=namedExpression (ToClause=mvapplyOperatorExpressionToClause)?;

parseOperator:
    PARSE (KindClause=parseOperatorKindClause)? Expression=unnamedExpression WITH Pattern=parseOperatorPattern;

parseOperatorKindClause:
    KIND '=' Kind=(SIMPLE | REGEX | RELAXED) (FlagsClause=parseOperatorFlagsClause)?;

parseOperatorFlagsClause:
    FLAGS '=' Flags=IDENTIFIER;

parseOperatorNameAndOptionalType:
    Name=simpleNameReference (':' Type=parameterType)?;

parseOperatorPattern:
    (LeadingColumn=parseOperatorNameAndOptionalType)? (parseOperatorPatternSegment)* (TrailingStar='*')?;

parseOperatorPatternSegment:
    ('*')? Text=stringLiteralExpression (Column=parseOperatorNameAndOptionalType)?;

parseWhereOperator:
    PARSEWHERE (KindClause=parseOperatorKindClause)? Expression=unnamedExpression WITH Pattern=parseOperatorPattern;


partitionOperator:
    PARTITION 
    (Parameters=queryOperatorParameters)? 
    BY ByExpression=entityExpression 
    (InClause=partitionOperatorInClause)? 
    (SubExpressionBody=partitionOperatorSubExpressionBody | FullExpressionBody=partitionOperatorFullExpressionBody);

partitionOperatorInClause:
    IN (FunctionCall=functionCallExpression | Literal=dynamicLiteralExpression);

partitionOperatorSubExpressionBody:
    '(' SubExpression=pipeSubExpression ')';

partitionOperatorFullExpressionBody:
    '{' Expression=pipeExpression '}';


partitionByOperator:
    PARTITIONBY (Parameters=queryOperatorParameters)? Column=entityExpression (IdClause=partitionByOperatorIdClause)? '(' SubExpression=contextualSubExpression ')';

partitionByOperatorIdClause:
    ID IdValue=GUIDLITERAL;

printOperator:
    PRINT Expressions+=namedExpression (',' Expressions+=namedExpression)*;

projectAwayOperator:
    PROJECTAWAY (Columns+=simpleOrWildcardedNameReference (',' Columns+=simpleOrWildcardedNameReference)*)?;

projectKeepOperator:
    PROJECTKEEP Columns+=simpleOrWildcardedNameReference (',' Columns+=simpleOrWildcardedNameReference)*;

projectOperator:
    PROJECT (Expressions+=namedExpression (',' Expressions+=namedExpression)*)?;

projectRenameOperator:
    PROJECTRENAME (Expressions+=namedExpression (',' Expressions+=namedExpression)*)?;

projectReorderOperator:
    PROJECTREORDER (Expressions+=projectReorderExpression (',' Expressions+=projectReorderExpression)*)?;

projectReorderExpression:
    Expression=simpleOrWildcardedNameReference (Order=(ASC | DESC | GRANNYASC | GRANNYDESC))?;


reduceByOperator:
    REDUCE (Parameters=queryOperatorParameters)? BY ByExpression=namedExpression (WithClause=reduceByWithClause)?;

reduceByWithClause:
    WITH Expressions+=namedExpression (',' Expressions+=namedExpression)*;


renderOperator:
    RENDER 
    CharType=(
        TABLE
        | LIST
        | BARCHART
        | PIECHART
        | LADDERCHART
        | TIMECHART
        | LINECHART
        | ANOMALYCOLUMNS
        | PIVOTCHART
        | AREACHART
        | STACKEDAREACHART
        | SCATTERCHART
        | TIMEPIVOT
        | COLUMNCHART
        | TIMELINE
        | CHART3D_
        | CARD
        | TREEMAP
        | IDENTIFIER)
    (WithClause=renderOperatorWithClause | LegacyPropertyList=renderOperatorLegacyPropertyList)?;

renderOperatorWithClause:
    WITH '(' (Properties+=renderOperatorProperty (',' Properties+=renderOperatorProperty)*)? ')';

renderOperatorLegacyPropertyList:
    (Properties+=renderOperatorLegacyProperty)+;

renderOperatorProperty:
      (Name=TITLE '=' ExpressionValue=functionCallOrPathExpression)
    | (Name=XCOLUMN '=' NameValue=simpleNameReference)
    | (Name=SERIES '=' NameListValue=renderPropertyNameList)
    | (Name=YCOLUMNS '=' NameListValue=renderPropertyNameList)
    | (Name=ANOMALYCOLUMNS '=' NameListValue=renderPropertyNameList)
    | (Name=KIND '=' TokenValue=(DEFAULT | UNSTACKED | STACKED | STACKED100 | MAP))
    | (Name=XTITLE '=' ExpressionValue=functionCallOrPathExpression)
    | (Name=YTITLE '=' ExpressionValue=functionCallOrPathExpression)
    | (Name=XAXIS '=' TokenValue=(LINEAR | LOG))
    | (Name=YAXIS '=' TokenValue=(LINEAR | LOG))
    | (Name=LEGEND '=' TokenValue=(VISIBLE | HIDDEN_))
    | (Name=YSPLIT '=' TokenValue=(NONE | AXES | PANELS))
    | (Name=ACCUMULATE '=' BoolValue=BOOLEANLITERAL)
    | (Name=YMIN '=' NumberValue=numberLiteralExpression)
    | (Name=YMAX '=' NumberValue=numberLiteralExpression)
    | (Name=XMIN '=' LiteralValue=literalExpression)
    | (Name=XMAX '=' LiteralValue=literalExpression)
    ;

renderPropertyNameList:
    Names+=extendedNameReference (',' Names+=extendedNameReference)*;    

renderOperatorLegacyProperty:
      (Name=TITLE '=' StringValue=stringLiteralExpression)
    | (Name=KIND '=' TokenValue=(DEFAULT | UNSTACKED | STACKED | STACKED100 | MAP))
    | (Name=WITH StringValue=stringLiteralExpression)
    | (Name=BY NameListValue=renderPropertyNameList)
    | (Name=ACCUMULATE '=' BoolValue=BOOLEANLITERAL)
    ;


sampleDistinctOperator:
    SAMPLE_DISTINCT (Parameters=queryOperatorParameters)? Expression=namedExpression OF OfExpression=namedExpression;

sampleOperator:
    SAMPLE (Parameters=queryOperatorParameters)? Expression=namedExpression;

scanOperator:
    SCAN 
      (Parameters=queryOperatorParameters)?
      (OrderByClause=scanOperatorOrderByClause)?
      (PartitionByClause=scanOperatorPartitionByClause)? 
      (DeclareClause=scanOperatorDeclareClause)? 
      WITH '(' (Steps+=scanOperatorStep)+ ')'
    ;

scanOperatorOrderByClause:
    ORDER BY Expressions+=orderedExpression (',' Expressions+=orderedExpression);

scanOperatorPartitionByClause:
    PARTITION BY Expressions+=unnamedExpression (',' Expressions+=unnamedExpression)*;

scanOperatorDeclareClause:
    DECLARE '(' Parameters+=scalarParameter (',' Parameters+=scalarParameter)* ')';

scanOperatorStep:
    STEP Name=parameterName (OPTIONAL)? (OutputClause=scanOperatorStepOutputClause)? ':' Expression=unnamedExpression (Body=scanOperatorBody)? ';';

scanOperatorStepOutputClause:
    OUTPUT '=' OutputKind=(ALL | LAST | NONE);

scanOperatorBody:
    '=>' Assignments+=scanOperatorAssignment (',' Assignments+=scanOperatorAssignment)*;

scanOperatorAssignment:
    Name=parameterName '=' Expression=unnamedExpression;


searchOperator:
    SEARCH 
      (Parameters=queryOperatorParameters)? 
      (DataScope=dataScopeClause)? 
      (InClause=searchOperatorInClause)? 
      (Expression=unnamedExpression | Star=starExpression | StarAndExpression=searchOperatorStarAndExpression);
    
searchOperatorStarAndExpression:
    '*' AND Expression=unnamedExpression;

searchOperatorInClause:
    IN '(' Expressions+=findOperatorSource (',' Expressions+=findOperatorSource)* ')';


serializeOperator:
    SERIALIZE (Parameters=queryOperatorParameters)? Expressions+=namedExpression (',' Expressions+=namedExpression)*;


sortOperator:
    Keyword=(SORT | ORDER) (Parameters=queryOperatorParameters)? BY Expressions+=orderedExpression (',' Expressions+=orderedExpression)*;

orderedExpression:
    Expression=namedExpression Ordering=sortOrdering;

sortOrdering:
    OrderKind=(ASC | DESC)? (NULLS NullsKind=(FIRST | LAST))?;


summarizeOperator:
    SUMMARIZE (Parameters=queryOperatorParameters)? (Expressions+=namedExpression (',' Expressions+=namedExpression)*)? (ByClause=summarizeOperatorByClause)?;

summarizeOperatorByClause:
    BY Expressions+=namedExpression (',' Expressions+=namedExpression) (BinClause=summarizeOperatorLegacyBinClause)?;

summarizeOperatorLegacyBinClause:
    BIN '=' Expression=numericLiteralExpression;


takeOperator:
    Keyword=(LIMIT | TAKE) (Parameters=queryOperatorParameters)? Expression=namedExpression;

topOperator:
    TOP (Parameters=queryOperatorParameters)? Expression=namedExpression BY ByExpression=orderedExpression;

topHittersOperator:
    TOP_HITTERS Expression=namedExpression OF OfExpression=namedExpression (ByClause=topHittersOperatorByClause)?;

topHittersOperatorByClause:
    BY ByExpression=orderedExpression;

topNestedOperator:
    Segments+=topNestedOperatorPart (',' Segments+=topNestedOperatorPart)*;

topNestedOperatorPart:
    TOP_NESTED (Expression=namedExpression)? OF OfExpression=namedExpression (WithOthers=topNestedOperatorWithOthersClause)? BY ByExpression=orderedExpression;

topNestedOperatorWithOthersClause:
    WITH OTHERS '=' Expression=namedExpression;


unionOperator:
    UNION (Parameters=queryOperatorParameters)? Expressions+=unionOperatorExpression (',' Expressions+=unionOperatorExpression)*;

unionOperatorExpression:
      wildcardedEntityExpression
    | entityNameReference
    | parenthesizedExpression
    ;

whereOperator:
    Keyword=(FILTER | WHERE) (Parameters=queryOperatorParameters)? Predicate=namedExpression;


contextualSubExpression:
      pipeSubExpression 
    | contextualPipeExpression
    ;

contextualPipeExpression:
    Expression=contextualDataTableExpression (contextualPipeExpressionPipedOperator)*;

contextualPipeExpressionPipedOperator:
    '|' Operator=afterPipeOperator;


queryOperatorParameters:
    (queryOperatorParameter)+;

queryOperatorParameter:
    NameToken=(
        BAGEXPANSION
        | BIN_LEGACY
        | DECODEBLOCKS
        | EXPANDOUTPUT
        | HINT_CONCURRENCY
        | HINT_DISTRIBUTION
        | HINT_MATERIALIZED
        | HINT_NUM_PARTITIONS
        | HINT_PASS_FILTERS
        | HINT_PASS_FILTERS_COLUMN
        | HINT_PROGRESSIVE_TOP
        | HINT_REMOTE
        | HINT_SUFFLEKEY
        | HINT_SPREAD
        | HINT_STRATEGY
        | ISFUZZY
        | KIND
        | WITH_ITEM_INDEX
        | WITH_MATCH_ID
        | WITH_STEP_NAME
        | WITHSOURCE
        | WITH_SOURCE
        | CROSSCLUSTER__
        | CROSSDB__
        | ID__
        | ISFUZZY__
        | WITHNOSOURCE__
        | PACKEDCOLUMN__
        | SOURCECOLUMNINDEX__
        )
    '=' Value=queryOperatorParameterValue;

queryOperatorParameterValue:
      identifierOrKeywordName
    | literalExpression
    ;

// Non-query expressions

namedExpression:
    (Name=namedExpressionNameClause)? Expression=unnamedExpression;

namedExpressionNameClause:
    (Name=identifierOrExtendedKeywordOrEscapedName | NameList=namedExpressionNameList) '=';    

namedExpressionNameList:
    '(' Names+=identifierOrExtendedKeywordOrEscapedName (',' Names+=identifierOrExtendedKeywordOrEscapedName)* ')';
 
scopedFunctionCallExpression:
    Scope=simpleNameReference '.' FunctionCall=functionCallExpression;

unnamedExpression:
    logicalOrExpression;

logicalOrExpression:
    Left=logicalAndExpression (logicalOrOperation)*;

logicalOrOperation:
    OR Right=logicalAndExpression;

logicalAndExpression:
    Left=equalityExpression (logicalAndOperation)*;

logicalAndOperation:
    AND Right=equalityExpression;

equalityExpression:
      relationalExpression
    | equalsEqualityExpression
    | listEqualityExpression
    | betweenEqualityExpression
    | starEqualityExpression
    ;

equalsEqualityExpression:
    Left=relationalExpression OperatorToken=('==' | '<>' | '!=') Right=relationalExpression;

listEqualityExpression:
    Left=relationalExpression OperatorToken=(IN | NOT_IN | IN_CI | NOT_IN_CI | HAS_ANY | HAS_ALL) '(' Expressions+=invocationExpression (',' Expressions+=invocationExpression)* ')';

betweenEqualityExpression:
    Left=relationalExpression OperatorToken=(BETWEEN | NOT_BETWEEN) '(' StartExpression=invocationExpression '..' EndExpression=invocationExpression ')';

starEqualityExpression:
    '*' '==' Expression=relationalExpression;

relationalExpression:
    Left=additiveExpression (OperatorToken=('<' | '>' | '<=' | '>=') Right=additiveExpression)?;

additiveExpression:
    Left=multiplicativeExpression (additiveOperation)*;

additiveOperation:
    OperatorToken=('+' | '-') Right=multiplicativeExpression;

multiplicativeExpression:
    Left=stringOperatorExpression (multiplicativeOperation)*;

multiplicativeOperation:
    OperatorToken=('*' | '/' | '%') Right=stringOperatorExpression;

stringOperatorExpression:
      stringBinaryOperatorExpression
    | stringStarOperatorExpression
    ;

stringBinaryOperatorExpression:
    Left=invocationExpression (stringBinaryOperation)*;

stringBinaryOperation:
    (Operator=stringBinaryOperator | HasOperator=':') Right=invocationExpression;

stringBinaryOperator:
    OperatorToken=
    (
          '=~'
        | '!~'
        | HAS
        | NOT_HAS
        | HAS_CS
        | NOT_HAS_CS
        | HASPREFIX
        | NOT_HASPREFIX
        | HASPREFIX_CS
        | NOT_HASPREFIX_CS
        | HASSUFFIX
        | NOT_HASSUFFIX
        | HASSUFFIX_CS
        | NOT_HASSUFFIX_CS
        | LIKE
        | NOTLIKE
        | LIKECS
        | NOTLIKECS
        | CONTAINS
        | NOTCONTAINS
        | CONTAINSCS
        | NOTCONTAINSCS
        | NOT_CONTAINS
        | CONTAINS_CS
        | NOT_CONTAINS_CS
        | STARTSWITH
        | NOT_STARTSWITH
        | STARTSWITH_CS
        | NOT_STARTSWITH_CS
        | ENDSWITH
        | NOT_ENDSWITH
        | ENDSWITH_CS
        | NOT_ENDSWITH_CS
        | MATCHES_REGEX
    );

stringStarOperatorExpression:
    LeftStarToken='*' Operator=stringBinaryOperator Right=invocationExpression;

invocationExpression:
    (OperatorToken=('+' | '-'))? Expression=functionCallOrPathExpression;

functionCallOrPathExpression:
      functionCallOrPathRoot
    | functionCallOrPathPathExpression
    | toTableExpression
    ;

functionCallOrPathRoot:
    dotCompositeFunctionCallExpression 
    | primaryExpression 
    | toScalarExpression
    ;

functionCallOrPathPathExpression:
    Expression=functionCallOrPathRoot (functionCallOrPathOperation)+;

functionCallOrPathOperation:
    functionalCallOrPathPathOperation 
    | functionCallOrPathElementOperation 
    | legacyFunctionCallOrPathElementOperation
    ;

functionalCallOrPathPathOperation:
    '.' Name=identifierOrKeywordOrEscapedName;

functionCallOrPathElementOperation:
    '[' Element=unnamedExpression ']';

legacyFunctionCallOrPathElementOperation:
    '.' '[' Element=unnamedExpression ']';

toScalarExpression:
    TOSCALAR (noOptimizationParameter)? '(' Expression=pipeExpression ')';

toTableExpression:
    TOTABLE (noOptimizationParameter)? '(' Expression=pipeExpression ')';

noOptimizationParameter:
    KIND '=' NOOPTIMIZATION;

dotCompositeFunctionCallExpression:
    Call=functionCallExpression (dotCompositeFunctionCallOperation)*;

dotCompositeFunctionCallOperation:
    '.' Call=functionCallExpression;

functionCallExpression:
      namedFunctionCallExpression
    | countExpression
    ;

namedFunctionCallExpression:
    Name=simpleNameReference '(' (Arguments+=argumentExpression (',' Arguments+=argumentExpression)*)? ')';

argumentExpression:
      namedExpression
    | starExpression
    ;

countExpression:
    COUNT '(' (Expression=namedExpression)? ')';


starExpression:
    '*';

primaryExpression:
      unsignedLiteralExpression
    | nameReferenceWithDataScope
    | dataTableExpression
    | externalDataExpression
    | contextualDataTableExpression
    | materializedViewCombineExpression
    | parenthesizedExpression
    ;

nameReferenceWithDataScope:
	Name=simpleNameReference (Scope=dataScopeClause)?;

dataScopeClause:
    DATASCOPE '=' KindToken=(HOTCACHE | ALL);

parenthesizedExpression:
    '(' Expression=expression ')';

rangeExpression:
    RANGE Expression=simpleNameReference FROM FromExpression=unnamedExpression TO ToExpression=unnamedExpression STEP StepExpression=unnamedExpression;


// entity expressions
entityExpression:
    entityNameReference
    | entityPathOrElementExpression
    ;

entityPathOrElementExpression:
    Expression=entityNameReference (entityPathOrElementOperator)+;
    
entityPathOrElementOperator:
    Path=entityPathOperator
    | Element=entityElementOperator 
    | PathElement=legacyEntityPathElementOperator
    ;

entityPathOperator:
     '.' Name=entityName;

entityElementOperator:
    '[' Expression=unnamedExpression ']';

legacyEntityPathElementOperator:
    '.' '[' Expression=unnamedExpression ']';

entityName:
    ATSIGN=atSignName 
    | Name=identifierOrExtendedKeywordOrEscapedName 
    | ExtendedName=extendedPathName;

entityNameReference:
    Name=entityName
    ;

atSignName:
    NameToken='@';

extendedPathName:
    NameToken=(KIND | WITHSOURCE | WITH_SOURCE);

wildcardedEntityExpression:
      wildcardedNameReference
    | dotCompositeFunctionCallExpression
    | wildcardedPathExpression 
    ;

wildcardedPathExpression:
    Expression=dotCompositeFunctionCallExpression '.' Name=wildcardedPathName;

wildcardedPathName:
    wildcardedName 
    | entityName
    ;



contextualDataTableExpression:
    CONTEXTUAL_DATATABLE Id=GUIDLITERAL Schema=rowSchema;

dataTableExpression:
    DATATABLE (Parameters=queryOperatorParameters)? Schema=rowSchema '[' (Values+=literalExpression)? (',' Values+=literalExpression)* (',')? ']';

rowSchema:
    '('  (Columns+=rowSchemaColumnDeclaration)? (',' Columns+=rowSchemaColumnDeclaration)* (',')? ')';

rowSchemaColumnDeclaration:
    Name=parameterName ':' Type=parameterType;

externalDataExpression:
    Keyword=(EXTERNALDATA | EXTERNAL_DATA) (Parameters=queryOperatorParameters)? Schema=rowSchema '[' ConnectionStrings+=stringLiteralExpression (',' ConnectionStrings+=stringLiteralExpression)* ']' (Properties=withPropertiesClause)?;


materializedViewCombineExpression:
    MATERIALIZED_VIEW_COMBINE '(' Name=stringLiteralExpression ')'
    BaseClause=materializeViewCombineBaseClause
    DeltaClause=materializedViewCombineDeltaClause
    AggregationsClause=materializedViewCombineAggregationsClause;

materializeViewCombineBaseClause:
    BASE '(' Expression=expression ')';

materializedViewCombineDeltaClause:
    DELTA '(' Expression=expression ')';

materializedViewCombineAggregationsClause:
    AGGREGATIONS '(' Operator=summarizeOperator ')';

withPropertiesClause:
    WITH '(' (Properties+=withPropertiesProperty (',' Properties+=withPropertiesProperty)* (',')?)? ')';

withPropertiesProperty:
      Name=parameterName '=' 
      ( StringValue=stringLiteralExpression 
      | TokenValue=(LONGLITERAL | REALLITERAL | BOOLEANLITERAL | DATETIMELITERAL | TYPELITERAL | UUIDLITERAL)
      | NameValue=parameterName)
    ;

parameterType:
    Token=(
        BOOL
        | BOOLEAN
        | DATE
        | DATETIME
        | DECIMAL
        | DOUBLE
        | DYNAMIC
        | GUID
        | INT
        | INT64
        | INT8
        | LONG
        | REAL
        | STRING
        | TIME
        | TIMESPAN
        | UNIQUEID
    );

extendedParameterType:
    Token=(
        BOOL
        | BOOLEAN
        | DATE
        | DATETIME
        | DECIMAL
        | DOUBLE
        | DYNAMIC
        | FLOAT
        | GUID
        | INT
        | INT16
        | INT32
        | INT64
        | INT8
        | LONG
        | REAL
        | STRING
        | TIME
        | TIMESPAN
        | UINT
        | UINT16
        | UINT32
        | UINT64
        | UINT8
        | ULONG
        | UNIQUEID
    );

parameterName:
    Name=identifierOrExtendedKeywordOrEscapedName;

///////////////////////////////////////
// name reference expressions

simpleNameReference:
    Name=identifierOrKeywordOrEscapedName;

extendedNameReference:
    Name=identifierOrExtendedKeywordOrEscapedName;

wildcardedNameReference:
    Name=wildcardedName;

simpleOrWildcardedNameReference:
      SimpleName=simpleNameReference
    | WildcardedName=wildcardedNameReference
    ;

///////////////////////////////////////
// names

identifierName:
    NameToken=IDENTIFIER;

keywordName:
    NameToken=
    (
        ACCESS
        | AGGREGATIONS
        | ALIAS
        | ALL
        | AXES
        | BASE
        | BIN
        | BOOL
        | CLUSTER
        | DATABASE
        | DECLARE
        | DEFAULT
        | DELTA
        | EVALUATE
        | EXECUTE
        | FACET
        | FORK
        | FROM
        | GUID
        | HIDDEN_
        | HOT
        | HOTDATA
        | HOTINDEX
        | ID
        | INTO
        | LEGEND
        | LET
        | LINEAR
        | LOG
        | LOOKUP
        | LIST
        | MAP
        | NONE
        | NULL
        | NULLS
        | ON
        | OPTIONAL
        | OUTPUT
        | PACK
        | PARTITION
        | PARTITIONBY
        | PATTERN
        | PLUGIN
        | QUERYPARAMETERS
        | RANGE
        | REDUCE
        | REPLACE
        | RENDER
        | RESTRICT
        | SERIES
        | STACKED
        | STACKED100
        | STEP
        | THRESHOLD
        | TYPEOF
        | UNSTACKED
        | UUID
        | VIEW
        | VISIBLE
        | WITH
        | XAXIS
        | XCOLUMN
        | XMAX
        | XMIN
        | XTITLE
        | YAXIS
        | YCOLUMNS
        | YMAX
        | YMIN
        | YTITLE
        | YSPLIT
    );

extendedKeywordName:
    NameToken=
    (
        // same as keywordName
        ACCESS
        | AGGREGATIONS
        | ALIAS
        | ALL
        | AXES
        | BASE
        | BIN
        | BOOL
        | CLUSTER
        | DATABASE
        | DECLARE
        | DEFAULT
        | DELTA
        | EVALUATE
        | EXECUTE
        | FACET
        | FORK
        | FROM
        | GUID
        | HIDDEN_
        | HOT
        | HOTDATA
        | HOTINDEX
        | ID
        | INTO
        | LEGEND
        | LET
        | LINEAR
        | LOG
        | LOOKUP
        | LIST
        | MAP
        | NONE
        | NULL
        | NULLS
        | ON
        | OPTIONAL
        | OUTPUT
        | PACK
        | PARTITION
        | PARTITIONBY
        | PATTERN
        | PLUGIN
        | QUERYPARAMETERS
        | RANGE
        | REDUCE
        | REPLACE
        | RENDER
        | RESTRICT
        | SERIES
        | STACKED
        | STACKED100
        | STEP
        | THRESHOLD
        | TYPEOF
        | UNSTACKED
        | UUID
        | VIEW
        | VISIBLE
        | WITH
        | XAXIS
        | XCOLUMN
        | XMAX
        | XMIN
        | XTITLE
        | YAXIS
        | YCOLUMNS
        | YMAX
        | YMIN
        | YTITLE
        | YSPLIT

        // additional keywords
        | ACCUMULATE
        | AS
        | BY
        | CONTAINS
        | CONSUME
        | COUNT
        | DATATABLE
        | DISTINCT
        | EXTEND
        | EXTERNALDATA
        | FIND
        | FILTER
        | HAS
        | IN
        | INVOKE
        | LIMIT
        | MATERIALIZE
        | OF
        | PARSE
        | PRINT
        | SAMPLE
        | SAMPLE_DISTINCT
        | SCAN
        | SEARCH
        | SERIALIZE
        | SET
        | SORT
        | SUMMARIZE
        | TAKE
        | TITLE
        | TO
        | TOP
        | TOSCALAR
        | TOTABLE
        | TOP_NESTED
        | TOP_HITTERS
        | WHERE
    );

escapedName:
    '[' StringLiteral=stringLiteralExpression ']';

identifierOrKeywordName:
      IdentifierName=identifierName
    | KeywordName=keywordName
    ;

identifierOrKeywordOrEscapedName:
      IdentifierName=identifierName
    | KeywordName=keywordName
    | EscapedName=escapedName
    ;

identifierOrExtendedKeywordOrEscapedName:
      IdentifierName=identifierName
    | KeywordName=extendedKeywordName
    | EscapedName=escapedName
    ;

identifierOrExtendedKeywordName:
      IdentifierName=identifierName
    | KeywordName=extendedKeywordName
    ;

wildcardedName:
    (Prefix=wildcardedNamePrefix)? '*' (wildcardedNameSegment)*;

wildcardedNamePrefix:
    Identifier=IDENTIFIER 
    | Keyword=extendedNameKeywords;

wildcardedNameSegment:
    Identifier=IDENTIFIER 
    | Keyword=extendedNameKeywords 
    | Number=LONGLITERAL 
    | Star='*';

nameKeywords:
    NameToken=
    (
        ACCESS
        | AGGREGATIONS
        | ALIAS
        | ALL
        | AXES
        | BASE
        | BIN
        | BOOL
        | CLUSTER
        | DATABASE
        | DECLARE
        | DEFAULT
        | DELTA
        | EVALUATE
        | EXECUTE
        | FACET
        | FORK
        | FROM
        | GUID
        | HIDDEN_
        | HOT
        | HOTDATA
        | HOTINDEX
        | ID
        | INTO
        | LEGEND
        | LET
        | LINEAR
        | LOG
        | LOOKUP
        | LIST
        | MAP
        | NONE
        | NULL
        | NULLS
        | ON
        | OPTIONAL
        | OUTPUT
        | PACK
        | PARTITION
        | PARTITIONBY
        | PATTERN
        | PLUGIN
        | QUERYPARAMETERS
        | RANGE
        | REDUCE
        | REPLACE
        | RENDER
        | RESTRICT
        | SERIES
        | STACKED
        | STACKED100
        | STEP
        | THRESHOLD
        | TYPEOF
        | UNSTACKED
        | UUID
        | VIEW
        | VISIBLE
        | WITH
        | XAXIS
        | XCOLUMN
        | XMAX
        | XMIN
        | XTITLE
        | YAXIS
        | YCOLUMNS
        | YMAX
        | YMIN
        | YTITLE
        | YSPLIT
    );

extendedNameKeywords:
      nameKeywords
    | ACCUMULATE
    | AS
    | BY
    | CONTAINS
    | CONSUME
    | COUNT
    | DATATABLE
    | DISTINCT
    | EXTEND
    | EXTERNALDATA
    | FIND
    | FILTER
    | HAS
    | IN
    | INVOKE
    | LIMIT
    | MATERIALIZE
    | OF
    | PARSE
    | PRINT
    | SAMPLE
    | SAMPLE_DISTINCT
    | SCAN
    | SEARCH
    | SERIALIZE
    | SET
    | SORT
    | SUMMARIZE
    | TAKE
    | TITLE
    | TO
    | TOP
    | TOSCALAR
    | TOTABLE
    | TOP_NESTED
    | TOP_HITTERS
    | WHERE
    ;

///////////////////////////////////////
// Literals

literalExpression:
      SignedNumber=signedNumberLiteralExpression
    | UnsignedNumber=unsignedLiteralExpression
    ;

// any literal that does not have a sign
unsignedLiteralExpression:
      Long=longLiteralExpression
    | Int=intLiteralExpression
    | Real=realLiteralExpression
    | Decimal=decimalLiteralExpression
    | DateTime=dateTimeLiteralExpression
    | TimeSpan=timeSpanLiteralExpression
    | Boolean=booleanLiteralExpression
    | Guid=guidLiteralExpression
    | Type=typeLiteralExpression
    | String=stringLiteralExpression
    | Dynamic=dynamicLiteralExpression
    ;

// any number like literal (numbers, date and time) 
numericLiteralExpression:
      Long=longLiteralExpression
    | Int=intLiteralExpression
    | Real=realLiteralExpression
    | Decimal=decimalLiteralExpression
    | DateTime=dateTimeLiteralExpression
    | TimeSpan=timeSpanLiteralExpression
    | Signed=signedNumberLiteralExpression
    ;

numberLiteralExpression:
      Long=longLiteralExpression
    | Int=intLiteralExpression
    | Real=realLiteralExpression
    | Decimal=decimalLiteralExpression
    | Signed=signedNumberLiteralExpression
    ;

// a number with a sign prefix
signedNumberLiteralExpression:
    Long=signedLongLiteralExpression
    | Real=signedRealLiteralExpression
    ;

longLiteralExpression:
    Token=LONGLITERAL;

intLiteralExpression:
    Token=INTLITERAL;

realLiteralExpression:
    Token=REALLITERAL;

decimalLiteralExpression:
    Token=DECIMALLITERAL;

dateTimeLiteralExpression:
    Token=DATETIMELITERAL;

timeSpanLiteralExpression:
    Token=TIMESPANLITERAL;

booleanLiteralExpression:
    Token=BOOLEANLITERAL;

guidLiteralExpression:
    Token=GUIDLITERAL;

typeLiteralExpression:
    Token=TYPELITERAL;

signedLongLiteralExpression:
    SignToken=('+' | '-') LiteralToken=LONGLITERAL;

signedRealLiteralExpression:
    SignToken=('+' | '-') LiteralToken=REALLITERAL;

stringLiteralExpression:
    Tokens+=STRINGLITERAL (Tokens+=STRINGLITERAL)*;

dynamicLiteralExpression:
    DYNAMIC '(' Value=jsonValue ')';

jsonValue:
      Array=jsonArray
    | Boolean=jsonBoolean
    | DateTime=jsonDateTime
    | Guid=jsonGuid
    | Long=jsonLong
    | Null=jsonNull
    | Object=jsonObject
    | Real=jsonReal
    | String=jsonString
    | Timespan=jsonTimeSpan
    | Dynamic=dynamicLiteralExpression
    ;

jsonObject:
     '{' (Pairs+=jsonPair (',' Pairs+=jsonPair)*)? '}';

jsonPair:
    Name=STRINGLITERAL ':' Value=jsonValue;

jsonArray:
     '[' (Values+=jsonValue (',' Values+=jsonValue)*)? ']';

jsonBoolean:
    Token=BOOLEANLITERAL;

jsonDateTime:
    Token=DATETIMELITERAL;

jsonGuid:
    Token=GUIDLITERAL;

jsonNull:
    Token=NULL;

jsonString:
     Tokens+=STRINGLITERAL (Tokens+=STRINGLITERAL)*;

jsonTimeSpan:
    Token=TIMESPANLITERAL;

jsonLong:
    (SignToken='-')? LiteralToken=LONGLITERAL;

jsonReal:
    (SignToken='-')? LiteralToken=REALLITERAL;

