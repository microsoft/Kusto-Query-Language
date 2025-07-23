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
    ALIAS DATABASE Name=identifierOrKeywordOrEscapedName '=' Expression=unnamedExpression;

letStatement:
      Function=letFunctionDeclaration
    | View=letViewDeclaration
    | Variable=letVariableDeclaration
    | Materialized=letMaterializeDeclaration
    | EntityGroup=letEntityGroupDeclaration
    ;

letVariableDeclaration:
    LET Name=identifierOrKeywordOrEscapedName '=' Expression=expression;

letFunctionDeclaration:
    LET Name=identifierOrKeywordOrEscapedName '=' '(' (ParameterList=letFunctionParameterList)? ')' Body=letFunctionBody;

letViewDeclaration:
    LET Name=identifierOrKeywordOrEscapedName '=' VIEW '(' (ParameterList=letViewParameterList)? ')' Body=letFunctionBody;

letViewParameterList:
    Parameters+=scalarParameter (',' Parameters+=scalarParameter)*;

letMaterializeDeclaration:
    LET Name=identifierOrKeywordOrEscapedName '=' MATERIALIZE '(' Expression=pipeExpression ')';

letEntityGroupDeclaration:
    LET Name=identifierOrKeywordOrEscapedName '=' entityGroupExpression;


letFunctionParameterList:
    TabularParameters+=tabularParameter (',' TabularParameters+=tabularParameter) (',' ScalarParameters+=scalarParameter)
    | ScalarParameters+= scalarParameter (',' ScalarParameters+=scalarParameter)
    ;

scalarParameter:
    Name=parameterName ':' Type=scalarType (Default=scalarParameterDefault)?;

scalarParameterDefault:
    '=' Value=literalExpression;

tabularParameter:
    Name=parameterName ':' (OpenSchema=tabularParameterOpenSchema | RowSchema=tabularParameterRowSchema);

tabularParameterOpenSchema:
    '(' '*' ')';

tabularParameterRowSchema:
    '(' Columns+=tabularParameterRowSchemaColumnDeclaration (',' Columns+=tabularParameterRowSchemaColumnDeclaration)* ')';

tabularParameterRowSchemaColumnDeclaration:
    Name=parameterName ':' Type=scalarType;

letFunctionBody:
    '{' (Statements+=letFunctionBodyStatement ';')* (Expression=expression)? (';')? '}';

letFunctionBodyStatement:
      Let=letStatement
	| DeclareQueryParameters=declareQueryParametersStatement
    ;


declarePatternStatement:
    DECLARE PATTERN Name=simpleNameReference (Definition=declarePatternDefinition)?;

declarePatternDefinition:
    '=' ParameterList=declarePatternParameterList (Path=declarePatternPathParameter)? '{' (Rules+=declarePatternRule)+ '}';

declarePatternParameterList:
    '(' Parameters+=declarePatternParameter (',' Parameters+=declarePatternParameter)* ')';

declarePatternParameter:
	Name=parameterName ':' Type=scalarType;

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
    Name=parameterName ':' Type=scalarType (Default=scalarParameterDefault)?;

queryStatement:
    Expression=expression;

// query expressions

expression:
    pipeExpression;

pipeExpression:
    Expression=beforePipeExpression (PipedOperators+=pipedOperator)*;

pipedOperator:
    '|' Operator=afterPipeOperator;

pipeSubExpression:
    Expression=afterPipeOperator (PipedOperators+=pipedOperator)*;

beforePipeExpression:
      beforeOrAfterPipeOperator 
    | printOperator 
    | macroExpandOperator 
    | rangeExpression 
    | scopedFunctionCallExpression
    | unnamedExpression 
    ;

afterPipeOperator:
      asOperator
    | assertSchemaOperator
    | consumeOperator
    | countOperator
    | distinctOperator
    | executeAndCacheOperator
    | extendOperator
    | facetByOperator
    | findOperator
    | forkOperator
    | getSchemaOperator
    | graphMarkComponentsOperator
    | graphMatchOperator
    | graphShortestPathsOperator
    | graphToTableOperator
    | invokeOperator
    | joinOperator
    | lookupOperator
    | makeGraphOperator
    | makeSeriesOperator
    | mvexpandOperator
    | mvapplyOperator
    | evaluateOperator
    | parseOperator
    | parseKvOperator
    | parseWhereOperator
    | partitionOperator
    | partitionByOperator
    | projectOperator
    | projectAwayOperator
    | projectRenameOperator
    | projectReorderOperator
    | projectKeepOperator
    | reduceByOperator
    | renderOperator
    | sampleOperator
    | sampleDistinctOperator
    | scanOperator
    | searchOperator
    | serializeOperator
    | sortOperator
    | summarizeOperator
    | takeOperator
    | topHittersOperator
    | topOperator
    | topNestedOperator
    | unionOperator
    | whereOperator
    ;

beforeOrAfterPipeOperator:
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
    AS (Parameters+=relaxedQueryOperatorParameter)* Name=identifierOrKeywordOrEscapedName;

assertSchemaOperator:
    ASSERTSCHEMA Schema=rowSchema;

consumeOperator:
    CONSUME (Parameters+=relaxedQueryOperatorParameter)*;

countOperator:
    COUNT (Parameters+=relaxedQueryOperatorParameter)*;

countOperatorAsClause:
    AS Name=identifierName;

distinctOperator:
    DISTINCT (Parameters+=relaxedQueryOperatorParameter)* 
        (Star=distinctOperatorStarTarget | ColumnList=distinctOperatorColumnListTarget)
    ;

distinctOperatorStarTarget:
    '*';

distinctOperatorColumnListTarget:
    Expressions+=namedExpression (',' Expressions+=namedExpression)*;


evaluateOperator:
    EVALUATE (Parameters+=relaxedQueryOperatorParameter)* PlugInCall=functionCallExpression (SchemaClause=evaluateOperatorSchemaClause)?;

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
    (Parameters+=relaxedQueryOperatorParameter)* (InClause=findOperatorInClause)? WHERE;

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
    ':' Type=extendedScalarType;

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
    Operator=forkPipeOperator (PipedOperators+=forkOperatorPipedOperator)*;

forkOperatorPipedOperator:
    '|' Operator=forkPipeOperator;

getSchemaOperator:
    GETSCHEMA;

graphMarkComponentsOperator:
    GRAPHMARKCOMPONENTS (Parametems+=relaxedQueryOperatorParameter)*;

graphMatchOperator:
    GRAPHMATCH
    (Parameters+=relaxedQueryOperatorParameter)*
    Patterns+=graphMatchPattern (',' Patterns+=graphMatchPattern)
    (WhereClause=graphMatchWhereClause)?
    (ProjectClause=graphMatchProjectClause)?
    ;

graphMatchPattern:
      Node=graphMatchPatternNode
    | UnnamedEdge=graphMatchPatternUnnamedEdge
    | NamedEdge=graphMatchPatternNamedEdge;

graphMatchPatternNode:
    '(' Name=identifierOrKeywordOrEscapedName ')';

graphMatchPatternUnnamedEdge:
    Direction=(DASHDASH_GREATERTHAN | LESSTHAN_DASHDASH | DASHDASH);

graphMatchPatternNamedEdge:
    OpenBracket=(DASH_OPENBRACKET | LESSTHAN_DASH_OPENBRACKET)
    Name=identifierOrKeywordOrEscapedName
    (Range=graphMatchPatternRange)?
    CloseBracket=(CLOSEBRACKET_DASH_GREATERTHAN | CLOSEBRACKET_DASH)
    ;

graphMatchPatternRange:
    ASTERISK LowerBound=invocationExpression DOTDOT UpperBound=invocationExpression;

graphMatchWhereClause:
    WHERE Expression=expression;

graphMatchProjectClause:
    PROJECT Expressions+=namedExpression (',' Expressions+=namedExpression)*;

graphToTableOperator:
    GRAPHTOTABLE Outputs+=graphToTableOutput (',' Outputs+=graphToTableOutput);

graphToTableOutput:
    Keyword=(NODES | EDGES) (AsClause=graphToTableAsClause)? (Parameters+=relaxedQueryOperatorParameter)*;

graphToTableAsClause:
    AS Name=identifierOrKeywordOrEscapedName;

graphShortestPathsOperator:
    GRAPHSHORTESTPATHS
    (Parameters+=relaxedQueryOperatorParameter)*
    Patterns+=graphMatchPattern (',' Patterns+=graphMatchPattern)
    (WhereClause=graphMatchWhereClause)?
    (ProjectClause=graphMatchProjectClause)?
    ;

invokeOperator:
    INVOKE FunctionCall=dotCompositeFunctionCallExpression;

joinOperator:
    JOIN (Parameters+=relaxedQueryOperatorParameter)* Table=unnamedExpression 
    (OnClause=joinOperatorOnClause | WhereClause=joinOperatorWhereClause)?;

joinOperatorOnClause:
    ON (Expressions+=unnamedExpression (',' Expressions+=unnamedExpression)*)?;

joinOperatorWhereClause:
    WHERE Predicate=unnamedExpression;

lookupOperator:
    LOOKUP (Parameters+=relaxedQueryOperatorParameter)* Table=unnamedExpression OnClause=joinOperatorOnClause;

macroExpandOperator:
    MACROEXPAND 
    (Parameters+=relaxedQueryOperatorParameter)*
    EntityGroup=macroExpandEntityGroup 
    AS ScopeName=identifierOrKeywordOrEscapedName
    '(' Statements+=statement (';' Statements+=statement)* (';')? ')';

macroExpandEntityGroup:
      EntityGroup=entityGroupExpression
    | Name=simpleNameReference
    | Entity=entityExpression
    ;

entityGroupExpression:
    ENTITYGROUP '[' Expressions+=unnamedExpression (',' Expressions+=unnamedExpression)* ']';

makeGraphOperator:
    MAKEGRAPH 
    (Parameters+=relaxedQueryOperatorParameter)*
    SourceColumn=simpleNameReference
    Direction=(DASHDASH_GREATERTHAN | DASHDASH)
    TargetColumn=simpleNameReference   
    (IdClause=makeGraphIdClause | TablesAndKeysClause=makeGraphTablesAndKeysClause)?
    (PartitionedByClause=makeGraphPartitionedByClause)?
    ;

makeGraphIdClause:
    WITH_NODE_ID '=' Name=identifierOrKeywordOrEscapedName;

makeGraphTablesAndKeysClause:
    WITH Table=invocationExpression ON Column=simpleNameReference;

makeGraphPartitionedByClause:
    PARTITIONEDBY Entity=entityPathOrElementExpression '(' SubQuery=contextualSubExpression ')';   

makeSeriesOperator:
    MAKESERIES 
    (Parameters+=relaxedQueryOperatorParameter)*
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
    (Parameters+=strictQueryOperatorParameter)*
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
    (Parameters+=strictQueryOperatorParameter)*
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
    Name=simpleNameReference (':' Type=scalarType)?;

parseOperatorPattern:
    (LeadingColumn=parseOperatorNameAndOptionalType)? (Segments+=parseOperatorPatternSegment)* (TrailingStar='*')?;

parseOperatorPatternSegment:
    ('*')? Text=stringLiteralExpression (Column=parseOperatorNameAndOptionalType)?;

parseWhereOperator:
    PARSEWHERE (KindClause=parseOperatorKindClause)? Expression=unnamedExpression WITH Pattern=parseOperatorPattern;

parseKvOperator:
    PARSEKV Expressions=unnamedExpression Keys=rowSchema (WithClause=parseKvWithClause)?;

parseKvWithClause:
    WITH '(' Properties+=queryOperatorProperty (',' Properties+=queryOperatorProperty)* ')';

partitionOperator:
    PARTITION 
    (Parameters+=relaxedQueryOperatorParameter)* 
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
    PARTITIONBY (Parameters+=relaxedQueryOperatorParameter)* Column=entityExpression (IdClause=partitionByOperatorIdClause)? '(' SubExpression=contextualSubExpression ')';

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
    REDUCE (Parameters+=strictQueryOperatorParameter)* BY ByExpression=namedExpression (WithClause=reduceByWithClause)?;

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
        | ANOMALYCHART
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
    | (Name=YMIN '=' NumberValue=numericLiteralExpression)
    | (Name=YMAX '=' NumberValue=numericLiteralExpression)
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
    SAMPLE_DISTINCT (Parameters+=strictQueryOperatorParameter)* Expression=namedExpression OF OfExpression=namedExpression;

sampleOperator:
    SAMPLE (Parameters+=strictQueryOperatorParameter)* Expression=namedExpression;

scanOperator:
    SCAN 
      (Parameters+=relaxedQueryOperatorParameter)*
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
      (Parameters+=relaxedQueryOperatorParameter)*
      (DataScope=dataScopeClause)? 
      (InClause=searchOperatorInClause)? 
      (Expression=unnamedExpression | Star=starExpression | StarAndExpression=searchOperatorStarAndExpression);
    
searchOperatorStarAndExpression:
    '*' AND Expression=unnamedExpression;

searchOperatorInClause:
    IN '(' Expressions+=findOperatorSource (',' Expressions+=findOperatorSource)* ')';


serializeOperator:
    SERIALIZE (Parameters+=strictQueryOperatorParameter)* Expressions+=namedExpression (',' Expressions+=namedExpression)*;


sortOperator:
    Keyword=(SORT | ORDER) (Parameters+=relaxedQueryOperatorParameter)* BY Expressions+=orderedExpression (',' Expressions+=orderedExpression)*;

orderedExpression:
    Expression=namedExpression Ordering=sortOrdering;

sortOrdering:
    OrderKind=(ASC | DESC)? (NULLS NullsKind=(FIRST | LAST))?;


summarizeOperator:
    SUMMARIZE (Parameters+=strictQueryOperatorParameter)* (Expressions+=namedExpression (',' Expressions+=namedExpression)*)? (ByClause=summarizeOperatorByClause)?;

summarizeOperatorByClause:
    BY Expressions+=namedExpression (',' Expressions+=namedExpression)* (BinClause=summarizeOperatorLegacyBinClause)?;

summarizeOperatorLegacyBinClause:
    BIN '=' Expression=numberLikeLiteralExpression;


takeOperator:
    Keyword=(LIMIT | TAKE) (Parameters+=strictQueryOperatorParameter)* Expression=namedExpression;

topOperator:
    TOP (Parameters+=strictQueryOperatorParameter)* Expression=namedExpression BY ByExpression=orderedExpression;

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
    UNION (Parameters+=relaxedQueryOperatorParameter)* Expressions+=unionOperatorExpression (',' Expressions+=unionOperatorExpression)*;

unionOperatorExpression:
      wildcardedEntityExpression
    | entityNameReference
    | parenthesizedExpression
    ;

whereOperator:
    Keyword=(FILTER | WHERE) (Parameters+=strictQueryOperatorParameter)* Predicate=namedExpression;


contextualSubExpression:
      pipeSubExpression 
    | contextualPipeExpression
    ;

contextualPipeExpression:
    Expression=contextualDataTableExpression (PipedOperators+=contextualPipeExpressionPipedOperator)*;

contextualPipeExpressionPipedOperator:
    '|' Operator=afterPipeOperator;

strictQueryOperatorParameter:
    NameToken=(
        BAGEXPANSION
        | BIN_LEGACY
        | CROSSCLUSTER__
        | CROSSDB__
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
        | ISFUZZY__
        | ID__
        | KIND
        | PACKEDCOLUMN__
        | SOURCECOLUMNINDEX__
        | WITH_ITEM_INDEX
        | WITH_MATCH_ID
        | WITH_STEP_NAME
        | WITHSOURCE
        | WITH_SOURCE
        | WITHNOSOURCE__
        )
    '=' (NameValue=identifierOrKeywordName | LiteralValue=literalExpression)
    ;

// allows any identifier
relaxedQueryOperatorParameter:
    NameToken=(
          IDENTIFIER
        | BAGEXPANSION
        | BIN_LEGACY
        | CROSSCLUSTER__
        | CROSSDB__
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
        | ISFUZZY__
        | ID__
        | KIND
        | PACKEDCOLUMN__
        | SOURCECOLUMNINDEX__
        | WITH_ITEM_INDEX
        | WITH_MATCH_ID
        | WITH_STEP_NAME
        | WITHSOURCE
        | WITH_SOURCE
        | WITHNOSOURCE__
        )
    '=' (NameValue=identifierOrKeywordName | LiteralValue=literalExpression)
    ;

queryOperatorProperty:
    Name=IDENTIFIER '=' (NameValue=identifierOrKeywordName | LiteralValue=literalExpression);


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
    Left=logicalAndExpression (Operations+=logicalOrOperation)*;

logicalOrOperation:
    OR Right=logicalAndExpression;

logicalAndExpression:
    Left=equalityExpression (Operations+=logicalAndOperation)*;

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
    Left=multiplicativeExpression (Operations+=additiveOperation)*;

additiveOperation:
    OperatorToken=('+' | '-') Right=multiplicativeExpression;

multiplicativeExpression:
    Left=stringOperatorExpression (Operations+=multiplicativeOperation)*;

multiplicativeOperation:
    OperatorToken=('*' | '/' | '%') Right=stringOperatorExpression;

stringOperatorExpression:
      stringBinaryOperatorExpression
    | stringStarOperatorExpression
    ;

stringBinaryOperatorExpression:
    Left=invocationExpression (Operation=stringBinaryOperation)?;

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
    Expression=functionCallOrPathRoot (Operations+=functionCallOrPathOperation)+;

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
    Call=functionCallExpression (Operations+=dotCompositeFunctionCallOperation)*;

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
    Expression=entityNameReference (Operators+=entityPathOrElementOperator)+;
    
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
    DATATABLE (Parameters+=relaxedQueryOperatorParameter)* Schema=rowSchema '[' (Values+=literalExpression)? (',' Values+=literalExpression)* (',')? ']';

rowSchema:
    '('  (Columns+=rowSchemaColumnDeclaration)? (',' Columns+=rowSchemaColumnDeclaration)* (',')? ')';

rowSchemaColumnDeclaration:
    Name=parameterName ':' Type=scalarType;

externalDataExpression:
    Keyword=(EXTERNALDATA | EXTERNAL_DATA) 
    (Parameters+=relaxedQueryOperatorParameter)* 
    Schema=rowSchema '[' ConnectionStrings+=stringLiteralExpression (',' ConnectionStrings+=stringLiteralExpression)* ']' 
    (WithClause=externalDataWithClause)?;

externalDataWithClause:
    WITH '(' (Properties+=externalDataWithClauseProperty (',' Properties+=externalDataWithClauseProperty)* (',')?)? ')';

externalDataWithClauseProperty:
      Name=parameterName '=' 
      ( StringValue=stringLiteralExpression 
      | TokenValue=(LONGLITERAL | REALLITERAL | BOOLEANLITERAL | DATETIMELITERAL | TYPELITERAL | GUIDLITERAL | RAWGUIDLITERAL)
      | NameValue=parameterName)
    ;

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

scalarType:
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

extendedScalarType:
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
    Token=IDENTIFIER;

keywordName:
    Token=
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
        | EDGES
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
        | NODES
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
    Token=
    (
          ACCUMULATE
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
      Identifier=identifierName
    | Keyword=keywordName
    ;

identifierOrKeywordOrEscapedName:
      Identifier=identifierName
    | Keyword=keywordName
    | Escaped=escapedName
    ;

identifierOrExtendedKeywordOrEscapedName:
      Identifier=identifierName
    | Keyword=keywordName
    | ExtendedKeyword=extendedKeywordName
    | Escaped=escapedName
    ;

identifierOrExtendedKeywordName:
      Identifier=identifierName
    | Keyword=keywordName
    | ExtendedKeyword=extendedKeywordName
    ;

wildcardedName:
    (Prefix=wildcardedNamePrefix)? '*' (Segments+=wildcardedNameSegment)*;

wildcardedNamePrefix:
      Identifier=IDENTIFIER 
    | Keyword=keywordName
    | ExtendedKeyword=extendedKeywordName
    ;

wildcardedNameSegment:
      Identifier=IDENTIFIER 
    | Keyword=keywordName
    | ExtendedKeyword=extendedKeywordName
    | Number=LONGLITERAL 
    | Star='*';


///////////////////////////////////////
// Literals

// all literals including with a sign prefix
literalExpression:
      Signed=signedLiteralExpression
    | Unsigned=unsignedLiteralExpression
    ;

// any literal without allowing for a sign prefix
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
numberLikeLiteralExpression:
      Long=longLiteralExpression
    | Int=intLiteralExpression
    | Real=realLiteralExpression
    | Decimal=decimalLiteralExpression
    | Signed=signedLiteralExpression
    | DateTime=dateTimeLiteralExpression
    | TimeSpan=timeSpanLiteralExpression
    ;

numericLiteralExpression:
      Long=longLiteralExpression
    | Int=intLiteralExpression
    | Real=realLiteralExpression
    | Decimal=decimalLiteralExpression
    | Signed=signedLiteralExpression
    ;

// a number with a sign prefix
signedLiteralExpression:
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

