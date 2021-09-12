namespace Kusto.Language.Syntax
{
    /// <summary>
    /// All the kinds of tokens that appear in Kusto 
    /// </summary>
    /// <remarks>
    /// Note for implementors: Are you adding a new value to this enum?
    /// Be sure to also add a new entry referencing it to the
    /// <see cref="SyntaxFacts"/> table.
    /// </remarks>
    public enum SyntaxKind : short
    {
        None = 0,

        // keywords
        AccessKeyword,
        AliasKeyword,
        AndKeyword,
        AsKeyword,
        AscKeyword,

        BetweenKeyword,
        BinKeyword,
        ByKeyword,

        ConsumeKeyword,
        ContainsKeyword,
        ContainsCsKeyword,
        Contains_CsKeyword,
        ContextualDataTableKeyword,
        CountKeyword,

        DatabaseKeyword,
        DataScopeKeyword,
        DataTableKeyword,
        DeclareKeyword,
        DefaultKeyword,
        DescKeyword,
        DistinctKeyword,

        EndsWithKeyword,
        EndsWithCsKeyword,
        EvaluateKeyword,
        ExecuteAndCacheKeyword,
        ExtendKeyword,
        ExternalDataKeyword,
        External_DataKeyword,

        FacetKeyword,
        FilterKeyword,
        FindKeyword,
        FirstKeyword,
        ForkKeyword,
        FromKeyword,

        GetSchemaKeyword,
        GrannyAscKeyword,
        GrannyDescKeyword,

        HasKeyword,
        HasAnyKeyword,
        HasAllKeyword,
        HasCsKeyword,
        HasPrefixKeyword,
        HasPrefixCsKeyword,
        HasSuffixKeyword,
        HasSuffixCsKeyword,
        HintDotConcurrencyKeyword,
        HintDotDistributionKeyword,
        HintDotMaterializedKeyword,
        HintDotNumPartitions,
        HintDotShuffleKeyKeyword,
        HintDotSpreadKeyword,
        HintDotRemoteKeyword,
        HintDotStrategyKeyword,
        HintDotProgressiveTopKeyword,

        IdKeyword,
        InKeyword,
        InCsKeyword,
        InvokeKeyword,

        JoinKeyword,

        LastKeyword,
        LetKeyword,
        LikeKeyword,
        LikeCsKeyword,
        LimitKeyword,
        LookupKeyword,

        MakeSeriesKeyword,
        MatchesRegexKeyword,
        MaterializeKeyword,
        MaterializedViewCombineKeyword,
        MvApplyKeyword,
        MvDashApplyKeyword,
        MvDashExpandKeyword,
        MvExpandKeyword,

        NotBetweenKeyword,
        NotContainsKeyword,
        NotContainsCsKeyword,
        NotBangContainsKeyword,
        NotBangContainsCsKeyword,
        NotEndsWithKeyword,
        NotEndsWithCsKeyword,
        NotHasKeyword,
        NotHasCsKeyword,
        NotHasPrefixKeyword,
        NotHasPrefixCsKeyword,
        NotHasSuffixKeyword,
        NotHasSuffixCsKeyword,
        NotInKeyword,
        NotInCsKeyword,
        NotLikeKeyword,
        NotLikeCsKeyword,
        NotStartsWithKeyword,
        NotStartsWithCsKeyword,
        NullKeyword,
        NullsKeyword,

        OfKeyword,
        OnKeyword,
        OptionalKeyword,
        OrKeyword,
        OrderKeyword,
        OthersKeyword,

        PackKeyword,
        ParseKeyword,
        ParseWhereKeyword,
        PartitionKeyword,
        PatternKeyword,
        PrintKeyword,
        ProjectKeyword,
        ProjectAwayKeyword,
        _ProjectAwayKeyword,
        ProjectKeepKeyword,
        ProjectRenameKeyword,
        ProjectReorderKeyword,
        ProjectSmartKeyword,

        QueryParametersKeyword,

        RangeKeyword,
        ReduceKeyword,
        RenderKeyword,
        RestrictKeyword,

        SampleKeyword,
        SampleDistinctKeyword,
        ScanKeyword,
        SearchKeyword,
        SerializeKeyword,
        SetKeyword,
        SortKeyword,
        StartsWithKeyword,
        StartsWithCsKeyword,
        StepKeyword,
        SummarizeKeyword,

        TakeKeyword,
        ToKeyword,
        TopKeyword,
        TopHittersKeyword,
        TopNestedKeyword,
        ToScalarKeyword,
        ToTableKeyword,
        TypeOfKeyword,

        UnionKeyword,
        UuidKeyword,

        ViewKeyword,

        WhereKeyword,
        WithKeyword,

        // scalar type keyword tokens
        BoolKeyword,
        BooleanKeyword,
        Int8Keyword,
        CharKeyword,
        UInt8Keyword,
        ByteKeyword,
        Int16Keyword,
        UInt16Keyword,
        IntKeyword,
        Int32Keyword,
        UIntKeyword,
        UInt32Keyword,
        LongKeyword,
        Int64Keyword,
        ULongKeyword,
        UInt64Keyword,
        SingleKeyword,
        FloatKeyword,
        RealKeyword,
        DecimalKeyword,
        DoubleKeyword,
        StringKeyword,
        TimeKeyword,
        TimespanKeyword,
        DateKeyword,
        DateTimeKeyword,
        GuidKeyword,
        UniqueIdKeyword,
        DynamicKeyword,

        // punctuation tokens
        OpenParenToken,
        CloseParenToken,
        OpenBracketToken,
        CloseBracketToken,
        OpenBraceToken,
        CloseBraceToken,
        BarToken,
        LessThanBarToken,
        PlusToken,
        MinusToken,
        AsteriskToken,
        SlashToken,
        PercentToken,
        DotToken,
        DotDotToken,
        BangToken,
        LessThanToken,
        LessThanOrEqualToken,
        GreaterThanToken,
        GreaterThanOrEqualToken,
        EqualToken,
        EqualEqualToken,
        BangEqualToken,
        LessThanGreaterThanToken,
        ColonToken,
        SemicolonToken,
        CommaToken,
        EqualTildeToken,
        BangTildeToken,
        AtToken,
        QuestionToken,
        FatArrowToken,

        // literal tokens
        BooleanLiteralToken,
        IntLiteralToken,
        LongLiteralToken,
        RealLiteralToken,
        DecimalLiteralToken,
        DateTimeLiteralToken,
        TimespanLiteralToken,
        GuidLiteralToken,
        StringLiteralToken,

        // other tokens
        IdentifierToken,
        DirectiveToken,
        EndOfTextToken,
        BadToken,

        // nodes
        List,
        SeparatedElement,
        ExpressionList,
        ExpressionCouple,
        RenameList,
        CustomNode,

        // literal expressions (kinds for LiteralExpression node)
        BooleanLiteralExpression,
        IntLiteralExpression,
        LongLiteralExpression,
        RealLiteralExpression,
        DecimalLiteralExpression,
        DateTimeLiteralExpression,
        TimespanLiteralExpression,
        GuidLiteralExpression,
        StringLiteralExpression,
        NullLiteralExpression,
        TokenLiteralExpression,  // any token (keyword/identifier/etc)

        // special literal expressions (each has own type)
        CompoundStringLiteralExpression,
        TypeOfLiteralExpression,

        // dynamic/json expressions
        DynamicExpression,
        JsonObjectExpression,
        JsonPair,
        JsonArrayExpression,

        // names
        TokenName,
        BracketedName,
        BracedName, // client parameters
        WildcardedName,
        BracketedWildcardedName,
        NameDeclaration,
        NameReference,

        ParenthesizedExpression,
        PathExpression,
        ElementExpression,
        SimpleNamedExpression,
        CompoundNamedExpression,
        FunctionCallExpression,
        ToScalarExpression,
        ToTableExpression,
        BracketedExpression,
        PipeExpression,
        NamedParameter,
        DataScopeExpression,
        DataTableExpression,
        ContextualDataTableExpression,
        ExternalDataExpression,
        ExternalDataWithClause,
        ExternalDataUriList,
        MaterializedViewCombineExpression,
        MaterializedViewCombineClause,

        // nullary?
        StarExpression,
        AtExpression,

        // unary operators
        UnaryPlusExpression,
        UnaryMinusExpression,

        // binary operators
        AddExpression,
        SubtractExpression,
        MultiplyExpression,
        DivideExpression,
        ModuloExpression,
        LessThanExpression,
        LessThanOrEqualExpression,
        GreaterThanExpression,
        GreaterThanOrEqualExpression,
        EqualExpression,
        NotEqualExpression,
        AndExpression,
        OrExpression,
        InExpression,
        InCsExpression,
        NotInExpression,
        NotInCsExpression,
        BetweenExpression,
        NotBetweenExpression,

        // string binary operators
        EqualTildeExpression, // equal - ignore case
        BangTildeExpression,  // not equal - ignore case
        HasExpression,
        HasCsExpression,
        NotHasExpression,
        NotHasCsExpression,
        HasPrefixExpression,
        HasPrefixCsExpression,
        NotHasPrefixExpression,
        NotHasPrefixCsExpression,
        HasSuffixExpression,
        HasSuffixCsExpression,
        NotHasSuffixExpression,
        NotHasSuffixCsExpression,
        LikeExpression,
        LikeCsExpression,
        NotLikeExpression,
        NotLikeCsExpression,
        ContainsExpression,
        ContainsCsExpression,
        NotContainsExpression,
        NotContainsCsExpression,
        StartsWithExpression,
        StartsWithCsExpression,
        NotStartsWithExpression,
        NotStartsWithCsExpression,
        EndsWithExpression,
        EndsWithCsExpression,
        NotEndsWithExpression,
        NotEndsWithCsExpression,
        MatchesRegexExpression,
        SearchExpression,
        HasAnyExpression,
        HasAllExpression,

        // common command-related expressions & clauses
        TypedColumnReference,
        PackExpression,
        NameAndTypeDeclaration,
        PrimitiveTypeExpression,
        SchemaTypeExpression,
        NameEqualsClause,
        DefaultExpressionClause,
        ToTypeOfClause,
        EvaluateSchemaClause,

        // query operators
        BadQueryOperator,

        AsOperator,

        ConsumeOperator,

        CountOperator,
        CountAsIdentifierClause,

        DistinctOperator,

        ExecuteAndCacheOperator,
        ExtendOperator,

        FacetOperator,
        FacetWithOperatorClause,
        FacetWithExpressionClause,

        FilterOperator,
        FindOperator,
        DataScopeClause,
        FindInClause,
        FindProjectClause,

        GetSchemaOperator,

        InvokeOperator,

        LookupOperator,
        JoinOperator,
        JoinOnClause,
        JoinWhereClause,

        SearchOperator,

        ForkOperator,
        ForkExpression,

        MakeSeriesOperator,
        MakeSeriesExpression,
        MakeSeriesOnClause,
        MakeSeriesInRangeClause,
        MakeSeriesFromClause,
        MakeSeriesToClause,
        MakeSeriesStepClause,
        MakeSeriesFromToStepClause,
        MakeSeriesByClause,

        MvApplyOperator,
        MvApplyExpression,
        MvApplyRowLimitClause,
        MvApplyContextIdClause,
        MvApplySubqueryExpression,

        MvExpandOperator,
        MvExpandExpression,
        MvExpandRowLimitClause,

        PartitionSubquery,
        PartitionQuery,
        PartitionScope,
        PartitionOperator,
        ParseOperator,
        ParseWhereOperator,

        EvaluateOperator,

        ProjectOperator,
        ProjectAwayOperator,
        ProjectKeepOperator,
        ProjectRenameOperator,
        ProjectReorderOperator,

        RangeOperator,

        ReduceByOperator,
        ReduceByWithClause,

        RenderOperator,
        RenderWithClause,
        NameReferenceList,

        SampleOperator,
        SampleDistinctOperator,

        ScanOperator,
        ScanOrderByClause,
        ScanPartitionByClause,
        ScanDeclareClause,
        ScanStep,
        ScanComputationClause,
        ScanAssignment,

        SerializeOperator,

        SortOperator,
        OrderedExpression,
        OrderingClause,
        OrderingNullsClause,

        SummarizeOperator,
        SummarizeByClause,

        TakeOperator,

        TopHittersOperator,
        TopHittersByClause,
        TopOperator,
        TopNestedOperator,
        TopNestedClause,
        TopNestedWithOthersClause,

        UnionOperator,

        PrintOperator,

        // statements
        AliasStatement,

        ExpressionStatement,

        FunctionDeclaration,
        FunctionParameters,
        FunctionParameter,
        DefaultValueDeclaration,
        FunctionBody,

        LetStatement,

        MaterializeExpression,

        RestrictStatement,

        SetOptionStatement,
        OptionValueClause,

        PatternStatement,
        PatternDeclaration,
        PatternPathParameter,
        PatternMatch,
        PatternPathValue,

        QueryParametersStatement,

        // commands
        CommandWithValueClause,
        CommandWithPropertyListClause,
        BadCommand,
        UnknownCommand,
        CustomCommand,

        // other
        QueryBlock,
        CommandBlock,
        DirectiveBlock,
        SkippedTokens,
        InputTextToken,
    }
}