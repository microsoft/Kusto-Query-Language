// Generated from c:/Kusto1/dev/Src/Grammar/Kql.g4 by ANTLR 4.13.1
import org.antlr.v4.runtime.tree.ParseTreeListener;

/**
 * This interface defines a complete listener for a parse tree produced by
 * {@link KqlParser}.
 */
public interface KqlListener extends ParseTreeListener {
	/**
	 * Enter a parse tree produced by {@link KqlParser#top}.
	 * @param ctx the parse tree
	 */
	void enterTop(KqlParser.TopContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#top}.
	 * @param ctx the parse tree
	 */
	void exitTop(KqlParser.TopContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#query}.
	 * @param ctx the parse tree
	 */
	void enterQuery(KqlParser.QueryContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#query}.
	 * @param ctx the parse tree
	 */
	void exitQuery(KqlParser.QueryContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#statement}.
	 * @param ctx the parse tree
	 */
	void enterStatement(KqlParser.StatementContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#statement}.
	 * @param ctx the parse tree
	 */
	void exitStatement(KqlParser.StatementContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#aliasDatabaseStatement}.
	 * @param ctx the parse tree
	 */
	void enterAliasDatabaseStatement(KqlParser.AliasDatabaseStatementContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#aliasDatabaseStatement}.
	 * @param ctx the parse tree
	 */
	void exitAliasDatabaseStatement(KqlParser.AliasDatabaseStatementContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#letStatement}.
	 * @param ctx the parse tree
	 */
	void enterLetStatement(KqlParser.LetStatementContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#letStatement}.
	 * @param ctx the parse tree
	 */
	void exitLetStatement(KqlParser.LetStatementContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#letVariableDeclaration}.
	 * @param ctx the parse tree
	 */
	void enterLetVariableDeclaration(KqlParser.LetVariableDeclarationContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#letVariableDeclaration}.
	 * @param ctx the parse tree
	 */
	void exitLetVariableDeclaration(KqlParser.LetVariableDeclarationContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#letFunctionDeclaration}.
	 * @param ctx the parse tree
	 */
	void enterLetFunctionDeclaration(KqlParser.LetFunctionDeclarationContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#letFunctionDeclaration}.
	 * @param ctx the parse tree
	 */
	void exitLetFunctionDeclaration(KqlParser.LetFunctionDeclarationContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#letViewDeclaration}.
	 * @param ctx the parse tree
	 */
	void enterLetViewDeclaration(KqlParser.LetViewDeclarationContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#letViewDeclaration}.
	 * @param ctx the parse tree
	 */
	void exitLetViewDeclaration(KqlParser.LetViewDeclarationContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#letViewParameterList}.
	 * @param ctx the parse tree
	 */
	void enterLetViewParameterList(KqlParser.LetViewParameterListContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#letViewParameterList}.
	 * @param ctx the parse tree
	 */
	void exitLetViewParameterList(KqlParser.LetViewParameterListContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#letMaterializeDeclaration}.
	 * @param ctx the parse tree
	 */
	void enterLetMaterializeDeclaration(KqlParser.LetMaterializeDeclarationContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#letMaterializeDeclaration}.
	 * @param ctx the parse tree
	 */
	void exitLetMaterializeDeclaration(KqlParser.LetMaterializeDeclarationContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#letEntityGroupDeclaration}.
	 * @param ctx the parse tree
	 */
	void enterLetEntityGroupDeclaration(KqlParser.LetEntityGroupDeclarationContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#letEntityGroupDeclaration}.
	 * @param ctx the parse tree
	 */
	void exitLetEntityGroupDeclaration(KqlParser.LetEntityGroupDeclarationContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#letFunctionParameterList}.
	 * @param ctx the parse tree
	 */
	void enterLetFunctionParameterList(KqlParser.LetFunctionParameterListContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#letFunctionParameterList}.
	 * @param ctx the parse tree
	 */
	void exitLetFunctionParameterList(KqlParser.LetFunctionParameterListContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#scalarParameter}.
	 * @param ctx the parse tree
	 */
	void enterScalarParameter(KqlParser.ScalarParameterContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#scalarParameter}.
	 * @param ctx the parse tree
	 */
	void exitScalarParameter(KqlParser.ScalarParameterContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#scalarParameterDefault}.
	 * @param ctx the parse tree
	 */
	void enterScalarParameterDefault(KqlParser.ScalarParameterDefaultContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#scalarParameterDefault}.
	 * @param ctx the parse tree
	 */
	void exitScalarParameterDefault(KqlParser.ScalarParameterDefaultContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#tabularParameter}.
	 * @param ctx the parse tree
	 */
	void enterTabularParameter(KqlParser.TabularParameterContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#tabularParameter}.
	 * @param ctx the parse tree
	 */
	void exitTabularParameter(KqlParser.TabularParameterContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#tabularParameterOpenSchema}.
	 * @param ctx the parse tree
	 */
	void enterTabularParameterOpenSchema(KqlParser.TabularParameterOpenSchemaContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#tabularParameterOpenSchema}.
	 * @param ctx the parse tree
	 */
	void exitTabularParameterOpenSchema(KqlParser.TabularParameterOpenSchemaContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#tabularParameterRowSchema}.
	 * @param ctx the parse tree
	 */
	void enterTabularParameterRowSchema(KqlParser.TabularParameterRowSchemaContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#tabularParameterRowSchema}.
	 * @param ctx the parse tree
	 */
	void exitTabularParameterRowSchema(KqlParser.TabularParameterRowSchemaContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#tabularParameterRowSchemaColumnDeclaration}.
	 * @param ctx the parse tree
	 */
	void enterTabularParameterRowSchemaColumnDeclaration(KqlParser.TabularParameterRowSchemaColumnDeclarationContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#tabularParameterRowSchemaColumnDeclaration}.
	 * @param ctx the parse tree
	 */
	void exitTabularParameterRowSchemaColumnDeclaration(KqlParser.TabularParameterRowSchemaColumnDeclarationContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#letFunctionBody}.
	 * @param ctx the parse tree
	 */
	void enterLetFunctionBody(KqlParser.LetFunctionBodyContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#letFunctionBody}.
	 * @param ctx the parse tree
	 */
	void exitLetFunctionBody(KqlParser.LetFunctionBodyContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#letFunctionBodyStatement}.
	 * @param ctx the parse tree
	 */
	void enterLetFunctionBodyStatement(KqlParser.LetFunctionBodyStatementContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#letFunctionBodyStatement}.
	 * @param ctx the parse tree
	 */
	void exitLetFunctionBodyStatement(KqlParser.LetFunctionBodyStatementContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#declarePatternStatement}.
	 * @param ctx the parse tree
	 */
	void enterDeclarePatternStatement(KqlParser.DeclarePatternStatementContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#declarePatternStatement}.
	 * @param ctx the parse tree
	 */
	void exitDeclarePatternStatement(KqlParser.DeclarePatternStatementContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#declarePatternDefinition}.
	 * @param ctx the parse tree
	 */
	void enterDeclarePatternDefinition(KqlParser.DeclarePatternDefinitionContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#declarePatternDefinition}.
	 * @param ctx the parse tree
	 */
	void exitDeclarePatternDefinition(KqlParser.DeclarePatternDefinitionContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#declarePatternParameterList}.
	 * @param ctx the parse tree
	 */
	void enterDeclarePatternParameterList(KqlParser.DeclarePatternParameterListContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#declarePatternParameterList}.
	 * @param ctx the parse tree
	 */
	void exitDeclarePatternParameterList(KqlParser.DeclarePatternParameterListContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#declarePatternParameter}.
	 * @param ctx the parse tree
	 */
	void enterDeclarePatternParameter(KqlParser.DeclarePatternParameterContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#declarePatternParameter}.
	 * @param ctx the parse tree
	 */
	void exitDeclarePatternParameter(KqlParser.DeclarePatternParameterContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#declarePatternPathParameter}.
	 * @param ctx the parse tree
	 */
	void enterDeclarePatternPathParameter(KqlParser.DeclarePatternPathParameterContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#declarePatternPathParameter}.
	 * @param ctx the parse tree
	 */
	void exitDeclarePatternPathParameter(KqlParser.DeclarePatternPathParameterContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#declarePatternRule}.
	 * @param ctx the parse tree
	 */
	void enterDeclarePatternRule(KqlParser.DeclarePatternRuleContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#declarePatternRule}.
	 * @param ctx the parse tree
	 */
	void exitDeclarePatternRule(KqlParser.DeclarePatternRuleContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#declarePatternRuleArgumentList}.
	 * @param ctx the parse tree
	 */
	void enterDeclarePatternRuleArgumentList(KqlParser.DeclarePatternRuleArgumentListContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#declarePatternRuleArgumentList}.
	 * @param ctx the parse tree
	 */
	void exitDeclarePatternRuleArgumentList(KqlParser.DeclarePatternRuleArgumentListContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#declarePatternRulePathArgument}.
	 * @param ctx the parse tree
	 */
	void enterDeclarePatternRulePathArgument(KqlParser.DeclarePatternRulePathArgumentContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#declarePatternRulePathArgument}.
	 * @param ctx the parse tree
	 */
	void exitDeclarePatternRulePathArgument(KqlParser.DeclarePatternRulePathArgumentContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#declarePatternRuleArgument}.
	 * @param ctx the parse tree
	 */
	void enterDeclarePatternRuleArgument(KqlParser.DeclarePatternRuleArgumentContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#declarePatternRuleArgument}.
	 * @param ctx the parse tree
	 */
	void exitDeclarePatternRuleArgument(KqlParser.DeclarePatternRuleArgumentContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#declarePatternBody}.
	 * @param ctx the parse tree
	 */
	void enterDeclarePatternBody(KqlParser.DeclarePatternBodyContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#declarePatternBody}.
	 * @param ctx the parse tree
	 */
	void exitDeclarePatternBody(KqlParser.DeclarePatternBodyContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#restrictAccessStatement}.
	 * @param ctx the parse tree
	 */
	void enterRestrictAccessStatement(KqlParser.RestrictAccessStatementContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#restrictAccessStatement}.
	 * @param ctx the parse tree
	 */
	void exitRestrictAccessStatement(KqlParser.RestrictAccessStatementContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#restrictAccessStatementEntity}.
	 * @param ctx the parse tree
	 */
	void enterRestrictAccessStatementEntity(KqlParser.RestrictAccessStatementEntityContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#restrictAccessStatementEntity}.
	 * @param ctx the parse tree
	 */
	void exitRestrictAccessStatementEntity(KqlParser.RestrictAccessStatementEntityContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#setStatement}.
	 * @param ctx the parse tree
	 */
	void enterSetStatement(KqlParser.SetStatementContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#setStatement}.
	 * @param ctx the parse tree
	 */
	void exitSetStatement(KqlParser.SetStatementContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#setStatementOptionValue}.
	 * @param ctx the parse tree
	 */
	void enterSetStatementOptionValue(KqlParser.SetStatementOptionValueContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#setStatementOptionValue}.
	 * @param ctx the parse tree
	 */
	void exitSetStatementOptionValue(KqlParser.SetStatementOptionValueContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#declareQueryParametersStatement}.
	 * @param ctx the parse tree
	 */
	void enterDeclareQueryParametersStatement(KqlParser.DeclareQueryParametersStatementContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#declareQueryParametersStatement}.
	 * @param ctx the parse tree
	 */
	void exitDeclareQueryParametersStatement(KqlParser.DeclareQueryParametersStatementContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#declareQueryParametersStatementParameter}.
	 * @param ctx the parse tree
	 */
	void enterDeclareQueryParametersStatementParameter(KqlParser.DeclareQueryParametersStatementParameterContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#declareQueryParametersStatementParameter}.
	 * @param ctx the parse tree
	 */
	void exitDeclareQueryParametersStatementParameter(KqlParser.DeclareQueryParametersStatementParameterContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#queryStatement}.
	 * @param ctx the parse tree
	 */
	void enterQueryStatement(KqlParser.QueryStatementContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#queryStatement}.
	 * @param ctx the parse tree
	 */
	void exitQueryStatement(KqlParser.QueryStatementContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#expression}.
	 * @param ctx the parse tree
	 */
	void enterExpression(KqlParser.ExpressionContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#expression}.
	 * @param ctx the parse tree
	 */
	void exitExpression(KqlParser.ExpressionContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#pipeExpression}.
	 * @param ctx the parse tree
	 */
	void enterPipeExpression(KqlParser.PipeExpressionContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#pipeExpression}.
	 * @param ctx the parse tree
	 */
	void exitPipeExpression(KqlParser.PipeExpressionContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#pipedOperator}.
	 * @param ctx the parse tree
	 */
	void enterPipedOperator(KqlParser.PipedOperatorContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#pipedOperator}.
	 * @param ctx the parse tree
	 */
	void exitPipedOperator(KqlParser.PipedOperatorContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#pipeSubExpression}.
	 * @param ctx the parse tree
	 */
	void enterPipeSubExpression(KqlParser.PipeSubExpressionContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#pipeSubExpression}.
	 * @param ctx the parse tree
	 */
	void exitPipeSubExpression(KqlParser.PipeSubExpressionContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#beforePipeExpression}.
	 * @param ctx the parse tree
	 */
	void enterBeforePipeExpression(KqlParser.BeforePipeExpressionContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#beforePipeExpression}.
	 * @param ctx the parse tree
	 */
	void exitBeforePipeExpression(KqlParser.BeforePipeExpressionContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#afterPipeOperator}.
	 * @param ctx the parse tree
	 */
	void enterAfterPipeOperator(KqlParser.AfterPipeOperatorContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#afterPipeOperator}.
	 * @param ctx the parse tree
	 */
	void exitAfterPipeOperator(KqlParser.AfterPipeOperatorContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#beforeOrAfterPipeOperator}.
	 * @param ctx the parse tree
	 */
	void enterBeforeOrAfterPipeOperator(KqlParser.BeforeOrAfterPipeOperatorContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#beforeOrAfterPipeOperator}.
	 * @param ctx the parse tree
	 */
	void exitBeforeOrAfterPipeOperator(KqlParser.BeforeOrAfterPipeOperatorContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#forkPipeOperator}.
	 * @param ctx the parse tree
	 */
	void enterForkPipeOperator(KqlParser.ForkPipeOperatorContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#forkPipeOperator}.
	 * @param ctx the parse tree
	 */
	void exitForkPipeOperator(KqlParser.ForkPipeOperatorContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#asOperator}.
	 * @param ctx the parse tree
	 */
	void enterAsOperator(KqlParser.AsOperatorContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#asOperator}.
	 * @param ctx the parse tree
	 */
	void exitAsOperator(KqlParser.AsOperatorContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#assertSchemaOperator}.
	 * @param ctx the parse tree
	 */
	void enterAssertSchemaOperator(KqlParser.AssertSchemaOperatorContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#assertSchemaOperator}.
	 * @param ctx the parse tree
	 */
	void exitAssertSchemaOperator(KqlParser.AssertSchemaOperatorContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#consumeOperator}.
	 * @param ctx the parse tree
	 */
	void enterConsumeOperator(KqlParser.ConsumeOperatorContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#consumeOperator}.
	 * @param ctx the parse tree
	 */
	void exitConsumeOperator(KqlParser.ConsumeOperatorContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#countOperator}.
	 * @param ctx the parse tree
	 */
	void enterCountOperator(KqlParser.CountOperatorContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#countOperator}.
	 * @param ctx the parse tree
	 */
	void exitCountOperator(KqlParser.CountOperatorContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#countOperatorAsClause}.
	 * @param ctx the parse tree
	 */
	void enterCountOperatorAsClause(KqlParser.CountOperatorAsClauseContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#countOperatorAsClause}.
	 * @param ctx the parse tree
	 */
	void exitCountOperatorAsClause(KqlParser.CountOperatorAsClauseContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#distinctOperator}.
	 * @param ctx the parse tree
	 */
	void enterDistinctOperator(KqlParser.DistinctOperatorContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#distinctOperator}.
	 * @param ctx the parse tree
	 */
	void exitDistinctOperator(KqlParser.DistinctOperatorContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#distinctOperatorStarTarget}.
	 * @param ctx the parse tree
	 */
	void enterDistinctOperatorStarTarget(KqlParser.DistinctOperatorStarTargetContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#distinctOperatorStarTarget}.
	 * @param ctx the parse tree
	 */
	void exitDistinctOperatorStarTarget(KqlParser.DistinctOperatorStarTargetContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#distinctOperatorColumnListTarget}.
	 * @param ctx the parse tree
	 */
	void enterDistinctOperatorColumnListTarget(KqlParser.DistinctOperatorColumnListTargetContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#distinctOperatorColumnListTarget}.
	 * @param ctx the parse tree
	 */
	void exitDistinctOperatorColumnListTarget(KqlParser.DistinctOperatorColumnListTargetContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#evaluateOperator}.
	 * @param ctx the parse tree
	 */
	void enterEvaluateOperator(KqlParser.EvaluateOperatorContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#evaluateOperator}.
	 * @param ctx the parse tree
	 */
	void exitEvaluateOperator(KqlParser.EvaluateOperatorContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#evaluateOperatorSchemaClause}.
	 * @param ctx the parse tree
	 */
	void enterEvaluateOperatorSchemaClause(KqlParser.EvaluateOperatorSchemaClauseContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#evaluateOperatorSchemaClause}.
	 * @param ctx the parse tree
	 */
	void exitEvaluateOperatorSchemaClause(KqlParser.EvaluateOperatorSchemaClauseContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#extendOperator}.
	 * @param ctx the parse tree
	 */
	void enterExtendOperator(KqlParser.ExtendOperatorContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#extendOperator}.
	 * @param ctx the parse tree
	 */
	void exitExtendOperator(KqlParser.ExtendOperatorContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#executeAndCacheOperator}.
	 * @param ctx the parse tree
	 */
	void enterExecuteAndCacheOperator(KqlParser.ExecuteAndCacheOperatorContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#executeAndCacheOperator}.
	 * @param ctx the parse tree
	 */
	void exitExecuteAndCacheOperator(KqlParser.ExecuteAndCacheOperatorContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#facetByOperator}.
	 * @param ctx the parse tree
	 */
	void enterFacetByOperator(KqlParser.FacetByOperatorContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#facetByOperator}.
	 * @param ctx the parse tree
	 */
	void exitFacetByOperator(KqlParser.FacetByOperatorContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#facetByOperatorWithOperatorClause}.
	 * @param ctx the parse tree
	 */
	void enterFacetByOperatorWithOperatorClause(KqlParser.FacetByOperatorWithOperatorClauseContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#facetByOperatorWithOperatorClause}.
	 * @param ctx the parse tree
	 */
	void exitFacetByOperatorWithOperatorClause(KqlParser.FacetByOperatorWithOperatorClauseContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#facetByOperatorWithExpressionClause}.
	 * @param ctx the parse tree
	 */
	void enterFacetByOperatorWithExpressionClause(KqlParser.FacetByOperatorWithExpressionClauseContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#facetByOperatorWithExpressionClause}.
	 * @param ctx the parse tree
	 */
	void exitFacetByOperatorWithExpressionClause(KqlParser.FacetByOperatorWithExpressionClauseContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#findOperator}.
	 * @param ctx the parse tree
	 */
	void enterFindOperator(KqlParser.FindOperatorContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#findOperator}.
	 * @param ctx the parse tree
	 */
	void exitFindOperator(KqlParser.FindOperatorContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#findOperatorParametersWhereClause}.
	 * @param ctx the parse tree
	 */
	void enterFindOperatorParametersWhereClause(KqlParser.FindOperatorParametersWhereClauseContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#findOperatorParametersWhereClause}.
	 * @param ctx the parse tree
	 */
	void exitFindOperatorParametersWhereClause(KqlParser.FindOperatorParametersWhereClauseContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#findOperatorInClause}.
	 * @param ctx the parse tree
	 */
	void enterFindOperatorInClause(KqlParser.FindOperatorInClauseContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#findOperatorInClause}.
	 * @param ctx the parse tree
	 */
	void exitFindOperatorInClause(KqlParser.FindOperatorInClauseContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#findOperatorProjectClause}.
	 * @param ctx the parse tree
	 */
	void enterFindOperatorProjectClause(KqlParser.FindOperatorProjectClauseContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#findOperatorProjectClause}.
	 * @param ctx the parse tree
	 */
	void exitFindOperatorProjectClause(KqlParser.FindOperatorProjectClauseContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#findOperatorProjectExpression}.
	 * @param ctx the parse tree
	 */
	void enterFindOperatorProjectExpression(KqlParser.FindOperatorProjectExpressionContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#findOperatorProjectExpression}.
	 * @param ctx the parse tree
	 */
	void exitFindOperatorProjectExpression(KqlParser.FindOperatorProjectExpressionContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#findOperatorColumnExpression}.
	 * @param ctx the parse tree
	 */
	void enterFindOperatorColumnExpression(KqlParser.FindOperatorColumnExpressionContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#findOperatorColumnExpression}.
	 * @param ctx the parse tree
	 */
	void exitFindOperatorColumnExpression(KqlParser.FindOperatorColumnExpressionContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#findOperatorOptionalColumnType}.
	 * @param ctx the parse tree
	 */
	void enterFindOperatorOptionalColumnType(KqlParser.FindOperatorOptionalColumnTypeContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#findOperatorOptionalColumnType}.
	 * @param ctx the parse tree
	 */
	void exitFindOperatorOptionalColumnType(KqlParser.FindOperatorOptionalColumnTypeContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#findOperatorPackExpression}.
	 * @param ctx the parse tree
	 */
	void enterFindOperatorPackExpression(KqlParser.FindOperatorPackExpressionContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#findOperatorPackExpression}.
	 * @param ctx the parse tree
	 */
	void exitFindOperatorPackExpression(KqlParser.FindOperatorPackExpressionContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#findOperatorProjectSmartClause}.
	 * @param ctx the parse tree
	 */
	void enterFindOperatorProjectSmartClause(KqlParser.FindOperatorProjectSmartClauseContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#findOperatorProjectSmartClause}.
	 * @param ctx the parse tree
	 */
	void exitFindOperatorProjectSmartClause(KqlParser.FindOperatorProjectSmartClauseContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#findOperatorProjectAwayClause}.
	 * @param ctx the parse tree
	 */
	void enterFindOperatorProjectAwayClause(KqlParser.FindOperatorProjectAwayClauseContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#findOperatorProjectAwayClause}.
	 * @param ctx the parse tree
	 */
	void exitFindOperatorProjectAwayClause(KqlParser.FindOperatorProjectAwayClauseContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#findOperatorProjectAwayStar}.
	 * @param ctx the parse tree
	 */
	void enterFindOperatorProjectAwayStar(KqlParser.FindOperatorProjectAwayStarContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#findOperatorProjectAwayStar}.
	 * @param ctx the parse tree
	 */
	void exitFindOperatorProjectAwayStar(KqlParser.FindOperatorProjectAwayStarContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#findOperatorProjectAwayColumnList}.
	 * @param ctx the parse tree
	 */
	void enterFindOperatorProjectAwayColumnList(KqlParser.FindOperatorProjectAwayColumnListContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#findOperatorProjectAwayColumnList}.
	 * @param ctx the parse tree
	 */
	void exitFindOperatorProjectAwayColumnList(KqlParser.FindOperatorProjectAwayColumnListContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#findOperatorSource}.
	 * @param ctx the parse tree
	 */
	void enterFindOperatorSource(KqlParser.FindOperatorSourceContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#findOperatorSource}.
	 * @param ctx the parse tree
	 */
	void exitFindOperatorSource(KqlParser.FindOperatorSourceContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#findOperatorSourceEntityExpression}.
	 * @param ctx the parse tree
	 */
	void enterFindOperatorSourceEntityExpression(KqlParser.FindOperatorSourceEntityExpressionContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#findOperatorSourceEntityExpression}.
	 * @param ctx the parse tree
	 */
	void exitFindOperatorSourceEntityExpression(KqlParser.FindOperatorSourceEntityExpressionContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#forkOperator}.
	 * @param ctx the parse tree
	 */
	void enterForkOperator(KqlParser.ForkOperatorContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#forkOperator}.
	 * @param ctx the parse tree
	 */
	void exitForkOperator(KqlParser.ForkOperatorContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#forkOperatorFork}.
	 * @param ctx the parse tree
	 */
	void enterForkOperatorFork(KqlParser.ForkOperatorForkContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#forkOperatorFork}.
	 * @param ctx the parse tree
	 */
	void exitForkOperatorFork(KqlParser.ForkOperatorForkContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#forkOperatorExpressionName}.
	 * @param ctx the parse tree
	 */
	void enterForkOperatorExpressionName(KqlParser.ForkOperatorExpressionNameContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#forkOperatorExpressionName}.
	 * @param ctx the parse tree
	 */
	void exitForkOperatorExpressionName(KqlParser.ForkOperatorExpressionNameContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#forkOperatorExpression}.
	 * @param ctx the parse tree
	 */
	void enterForkOperatorExpression(KqlParser.ForkOperatorExpressionContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#forkOperatorExpression}.
	 * @param ctx the parse tree
	 */
	void exitForkOperatorExpression(KqlParser.ForkOperatorExpressionContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#forkOperatorPipedOperator}.
	 * @param ctx the parse tree
	 */
	void enterForkOperatorPipedOperator(KqlParser.ForkOperatorPipedOperatorContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#forkOperatorPipedOperator}.
	 * @param ctx the parse tree
	 */
	void exitForkOperatorPipedOperator(KqlParser.ForkOperatorPipedOperatorContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#getSchemaOperator}.
	 * @param ctx the parse tree
	 */
	void enterGetSchemaOperator(KqlParser.GetSchemaOperatorContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#getSchemaOperator}.
	 * @param ctx the parse tree
	 */
	void exitGetSchemaOperator(KqlParser.GetSchemaOperatorContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#graphMarkComponentsOperator}.
	 * @param ctx the parse tree
	 */
	void enterGraphMarkComponentsOperator(KqlParser.GraphMarkComponentsOperatorContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#graphMarkComponentsOperator}.
	 * @param ctx the parse tree
	 */
	void exitGraphMarkComponentsOperator(KqlParser.GraphMarkComponentsOperatorContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#graphMatchOperator}.
	 * @param ctx the parse tree
	 */
	void enterGraphMatchOperator(KqlParser.GraphMatchOperatorContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#graphMatchOperator}.
	 * @param ctx the parse tree
	 */
	void exitGraphMatchOperator(KqlParser.GraphMatchOperatorContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#graphMatchPattern}.
	 * @param ctx the parse tree
	 */
	void enterGraphMatchPattern(KqlParser.GraphMatchPatternContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#graphMatchPattern}.
	 * @param ctx the parse tree
	 */
	void exitGraphMatchPattern(KqlParser.GraphMatchPatternContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#graphMatchPatternNode}.
	 * @param ctx the parse tree
	 */
	void enterGraphMatchPatternNode(KqlParser.GraphMatchPatternNodeContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#graphMatchPatternNode}.
	 * @param ctx the parse tree
	 */
	void exitGraphMatchPatternNode(KqlParser.GraphMatchPatternNodeContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#graphMatchPatternUnnamedEdge}.
	 * @param ctx the parse tree
	 */
	void enterGraphMatchPatternUnnamedEdge(KqlParser.GraphMatchPatternUnnamedEdgeContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#graphMatchPatternUnnamedEdge}.
	 * @param ctx the parse tree
	 */
	void exitGraphMatchPatternUnnamedEdge(KqlParser.GraphMatchPatternUnnamedEdgeContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#graphMatchPatternNamedEdge}.
	 * @param ctx the parse tree
	 */
	void enterGraphMatchPatternNamedEdge(KqlParser.GraphMatchPatternNamedEdgeContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#graphMatchPatternNamedEdge}.
	 * @param ctx the parse tree
	 */
	void exitGraphMatchPatternNamedEdge(KqlParser.GraphMatchPatternNamedEdgeContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#graphMatchPatternRange}.
	 * @param ctx the parse tree
	 */
	void enterGraphMatchPatternRange(KqlParser.GraphMatchPatternRangeContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#graphMatchPatternRange}.
	 * @param ctx the parse tree
	 */
	void exitGraphMatchPatternRange(KqlParser.GraphMatchPatternRangeContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#graphMatchWhereClause}.
	 * @param ctx the parse tree
	 */
	void enterGraphMatchWhereClause(KqlParser.GraphMatchWhereClauseContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#graphMatchWhereClause}.
	 * @param ctx the parse tree
	 */
	void exitGraphMatchWhereClause(KqlParser.GraphMatchWhereClauseContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#graphMatchProjectClause}.
	 * @param ctx the parse tree
	 */
	void enterGraphMatchProjectClause(KqlParser.GraphMatchProjectClauseContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#graphMatchProjectClause}.
	 * @param ctx the parse tree
	 */
	void exitGraphMatchProjectClause(KqlParser.GraphMatchProjectClauseContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#graphToTableOperator}.
	 * @param ctx the parse tree
	 */
	void enterGraphToTableOperator(KqlParser.GraphToTableOperatorContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#graphToTableOperator}.
	 * @param ctx the parse tree
	 */
	void exitGraphToTableOperator(KqlParser.GraphToTableOperatorContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#graphToTableOutput}.
	 * @param ctx the parse tree
	 */
	void enterGraphToTableOutput(KqlParser.GraphToTableOutputContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#graphToTableOutput}.
	 * @param ctx the parse tree
	 */
	void exitGraphToTableOutput(KqlParser.GraphToTableOutputContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#graphToTableAsClause}.
	 * @param ctx the parse tree
	 */
	void enterGraphToTableAsClause(KqlParser.GraphToTableAsClauseContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#graphToTableAsClause}.
	 * @param ctx the parse tree
	 */
	void exitGraphToTableAsClause(KqlParser.GraphToTableAsClauseContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#graphShortestPathsOperator}.
	 * @param ctx the parse tree
	 */
	void enterGraphShortestPathsOperator(KqlParser.GraphShortestPathsOperatorContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#graphShortestPathsOperator}.
	 * @param ctx the parse tree
	 */
	void exitGraphShortestPathsOperator(KqlParser.GraphShortestPathsOperatorContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#invokeOperator}.
	 * @param ctx the parse tree
	 */
	void enterInvokeOperator(KqlParser.InvokeOperatorContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#invokeOperator}.
	 * @param ctx the parse tree
	 */
	void exitInvokeOperator(KqlParser.InvokeOperatorContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#joinOperator}.
	 * @param ctx the parse tree
	 */
	void enterJoinOperator(KqlParser.JoinOperatorContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#joinOperator}.
	 * @param ctx the parse tree
	 */
	void exitJoinOperator(KqlParser.JoinOperatorContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#joinOperatorOnClause}.
	 * @param ctx the parse tree
	 */
	void enterJoinOperatorOnClause(KqlParser.JoinOperatorOnClauseContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#joinOperatorOnClause}.
	 * @param ctx the parse tree
	 */
	void exitJoinOperatorOnClause(KqlParser.JoinOperatorOnClauseContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#joinOperatorWhereClause}.
	 * @param ctx the parse tree
	 */
	void enterJoinOperatorWhereClause(KqlParser.JoinOperatorWhereClauseContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#joinOperatorWhereClause}.
	 * @param ctx the parse tree
	 */
	void exitJoinOperatorWhereClause(KqlParser.JoinOperatorWhereClauseContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#lookupOperator}.
	 * @param ctx the parse tree
	 */
	void enterLookupOperator(KqlParser.LookupOperatorContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#lookupOperator}.
	 * @param ctx the parse tree
	 */
	void exitLookupOperator(KqlParser.LookupOperatorContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#macroExpandOperator}.
	 * @param ctx the parse tree
	 */
	void enterMacroExpandOperator(KqlParser.MacroExpandOperatorContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#macroExpandOperator}.
	 * @param ctx the parse tree
	 */
	void exitMacroExpandOperator(KqlParser.MacroExpandOperatorContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#macroExpandEntityGroup}.
	 * @param ctx the parse tree
	 */
	void enterMacroExpandEntityGroup(KqlParser.MacroExpandEntityGroupContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#macroExpandEntityGroup}.
	 * @param ctx the parse tree
	 */
	void exitMacroExpandEntityGroup(KqlParser.MacroExpandEntityGroupContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#entityGroupExpression}.
	 * @param ctx the parse tree
	 */
	void enterEntityGroupExpression(KqlParser.EntityGroupExpressionContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#entityGroupExpression}.
	 * @param ctx the parse tree
	 */
	void exitEntityGroupExpression(KqlParser.EntityGroupExpressionContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#makeGraphOperator}.
	 * @param ctx the parse tree
	 */
	void enterMakeGraphOperator(KqlParser.MakeGraphOperatorContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#makeGraphOperator}.
	 * @param ctx the parse tree
	 */
	void exitMakeGraphOperator(KqlParser.MakeGraphOperatorContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#makeGraphIdClause}.
	 * @param ctx the parse tree
	 */
	void enterMakeGraphIdClause(KqlParser.MakeGraphIdClauseContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#makeGraphIdClause}.
	 * @param ctx the parse tree
	 */
	void exitMakeGraphIdClause(KqlParser.MakeGraphIdClauseContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#makeGraphTablesAndKeysClause}.
	 * @param ctx the parse tree
	 */
	void enterMakeGraphTablesAndKeysClause(KqlParser.MakeGraphTablesAndKeysClauseContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#makeGraphTablesAndKeysClause}.
	 * @param ctx the parse tree
	 */
	void exitMakeGraphTablesAndKeysClause(KqlParser.MakeGraphTablesAndKeysClauseContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#makeGraphPartitionedByClause}.
	 * @param ctx the parse tree
	 */
	void enterMakeGraphPartitionedByClause(KqlParser.MakeGraphPartitionedByClauseContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#makeGraphPartitionedByClause}.
	 * @param ctx the parse tree
	 */
	void exitMakeGraphPartitionedByClause(KqlParser.MakeGraphPartitionedByClauseContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#makeSeriesOperator}.
	 * @param ctx the parse tree
	 */
	void enterMakeSeriesOperator(KqlParser.MakeSeriesOperatorContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#makeSeriesOperator}.
	 * @param ctx the parse tree
	 */
	void exitMakeSeriesOperator(KqlParser.MakeSeriesOperatorContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#makeSeriesOperatorOnClause}.
	 * @param ctx the parse tree
	 */
	void enterMakeSeriesOperatorOnClause(KqlParser.MakeSeriesOperatorOnClauseContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#makeSeriesOperatorOnClause}.
	 * @param ctx the parse tree
	 */
	void exitMakeSeriesOperatorOnClause(KqlParser.MakeSeriesOperatorOnClauseContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#makeSeriesOperatorAggregation}.
	 * @param ctx the parse tree
	 */
	void enterMakeSeriesOperatorAggregation(KqlParser.MakeSeriesOperatorAggregationContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#makeSeriesOperatorAggregation}.
	 * @param ctx the parse tree
	 */
	void exitMakeSeriesOperatorAggregation(KqlParser.MakeSeriesOperatorAggregationContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#makeSeriesOperatorExpressionDefaultClause}.
	 * @param ctx the parse tree
	 */
	void enterMakeSeriesOperatorExpressionDefaultClause(KqlParser.MakeSeriesOperatorExpressionDefaultClauseContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#makeSeriesOperatorExpressionDefaultClause}.
	 * @param ctx the parse tree
	 */
	void exitMakeSeriesOperatorExpressionDefaultClause(KqlParser.MakeSeriesOperatorExpressionDefaultClauseContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#makeSeriesOperatorInRangeClause}.
	 * @param ctx the parse tree
	 */
	void enterMakeSeriesOperatorInRangeClause(KqlParser.MakeSeriesOperatorInRangeClauseContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#makeSeriesOperatorInRangeClause}.
	 * @param ctx the parse tree
	 */
	void exitMakeSeriesOperatorInRangeClause(KqlParser.MakeSeriesOperatorInRangeClauseContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#makeSeriesOperatorFromToStepClause}.
	 * @param ctx the parse tree
	 */
	void enterMakeSeriesOperatorFromToStepClause(KqlParser.MakeSeriesOperatorFromToStepClauseContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#makeSeriesOperatorFromToStepClause}.
	 * @param ctx the parse tree
	 */
	void exitMakeSeriesOperatorFromToStepClause(KqlParser.MakeSeriesOperatorFromToStepClauseContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#makeSeriesOperatorByClause}.
	 * @param ctx the parse tree
	 */
	void enterMakeSeriesOperatorByClause(KqlParser.MakeSeriesOperatorByClauseContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#makeSeriesOperatorByClause}.
	 * @param ctx the parse tree
	 */
	void exitMakeSeriesOperatorByClause(KqlParser.MakeSeriesOperatorByClauseContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#mvapplyOperator}.
	 * @param ctx the parse tree
	 */
	void enterMvapplyOperator(KqlParser.MvapplyOperatorContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#mvapplyOperator}.
	 * @param ctx the parse tree
	 */
	void exitMvapplyOperator(KqlParser.MvapplyOperatorContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#mvapplyOperatorLimitClause}.
	 * @param ctx the parse tree
	 */
	void enterMvapplyOperatorLimitClause(KqlParser.MvapplyOperatorLimitClauseContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#mvapplyOperatorLimitClause}.
	 * @param ctx the parse tree
	 */
	void exitMvapplyOperatorLimitClause(KqlParser.MvapplyOperatorLimitClauseContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#mvapplyOperatorIdClause}.
	 * @param ctx the parse tree
	 */
	void enterMvapplyOperatorIdClause(KqlParser.MvapplyOperatorIdClauseContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#mvapplyOperatorIdClause}.
	 * @param ctx the parse tree
	 */
	void exitMvapplyOperatorIdClause(KqlParser.MvapplyOperatorIdClauseContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#mvapplyOperatorExpression}.
	 * @param ctx the parse tree
	 */
	void enterMvapplyOperatorExpression(KqlParser.MvapplyOperatorExpressionContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#mvapplyOperatorExpression}.
	 * @param ctx the parse tree
	 */
	void exitMvapplyOperatorExpression(KqlParser.MvapplyOperatorExpressionContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#mvapplyOperatorExpressionToClause}.
	 * @param ctx the parse tree
	 */
	void enterMvapplyOperatorExpressionToClause(KqlParser.MvapplyOperatorExpressionToClauseContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#mvapplyOperatorExpressionToClause}.
	 * @param ctx the parse tree
	 */
	void exitMvapplyOperatorExpressionToClause(KqlParser.MvapplyOperatorExpressionToClauseContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#mvexpandOperator}.
	 * @param ctx the parse tree
	 */
	void enterMvexpandOperator(KqlParser.MvexpandOperatorContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#mvexpandOperator}.
	 * @param ctx the parse tree
	 */
	void exitMvexpandOperator(KqlParser.MvexpandOperatorContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#mvexpandOperatorExpression}.
	 * @param ctx the parse tree
	 */
	void enterMvexpandOperatorExpression(KqlParser.MvexpandOperatorExpressionContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#mvexpandOperatorExpression}.
	 * @param ctx the parse tree
	 */
	void exitMvexpandOperatorExpression(KqlParser.MvexpandOperatorExpressionContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#parseOperator}.
	 * @param ctx the parse tree
	 */
	void enterParseOperator(KqlParser.ParseOperatorContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#parseOperator}.
	 * @param ctx the parse tree
	 */
	void exitParseOperator(KqlParser.ParseOperatorContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#parseOperatorKindClause}.
	 * @param ctx the parse tree
	 */
	void enterParseOperatorKindClause(KqlParser.ParseOperatorKindClauseContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#parseOperatorKindClause}.
	 * @param ctx the parse tree
	 */
	void exitParseOperatorKindClause(KqlParser.ParseOperatorKindClauseContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#parseOperatorFlagsClause}.
	 * @param ctx the parse tree
	 */
	void enterParseOperatorFlagsClause(KqlParser.ParseOperatorFlagsClauseContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#parseOperatorFlagsClause}.
	 * @param ctx the parse tree
	 */
	void exitParseOperatorFlagsClause(KqlParser.ParseOperatorFlagsClauseContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#parseOperatorNameAndOptionalType}.
	 * @param ctx the parse tree
	 */
	void enterParseOperatorNameAndOptionalType(KqlParser.ParseOperatorNameAndOptionalTypeContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#parseOperatorNameAndOptionalType}.
	 * @param ctx the parse tree
	 */
	void exitParseOperatorNameAndOptionalType(KqlParser.ParseOperatorNameAndOptionalTypeContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#parseOperatorPattern}.
	 * @param ctx the parse tree
	 */
	void enterParseOperatorPattern(KqlParser.ParseOperatorPatternContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#parseOperatorPattern}.
	 * @param ctx the parse tree
	 */
	void exitParseOperatorPattern(KqlParser.ParseOperatorPatternContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#parseOperatorPatternSegment}.
	 * @param ctx the parse tree
	 */
	void enterParseOperatorPatternSegment(KqlParser.ParseOperatorPatternSegmentContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#parseOperatorPatternSegment}.
	 * @param ctx the parse tree
	 */
	void exitParseOperatorPatternSegment(KqlParser.ParseOperatorPatternSegmentContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#parseWhereOperator}.
	 * @param ctx the parse tree
	 */
	void enterParseWhereOperator(KqlParser.ParseWhereOperatorContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#parseWhereOperator}.
	 * @param ctx the parse tree
	 */
	void exitParseWhereOperator(KqlParser.ParseWhereOperatorContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#parseKvOperator}.
	 * @param ctx the parse tree
	 */
	void enterParseKvOperator(KqlParser.ParseKvOperatorContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#parseKvOperator}.
	 * @param ctx the parse tree
	 */
	void exitParseKvOperator(KqlParser.ParseKvOperatorContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#parseKvWithClause}.
	 * @param ctx the parse tree
	 */
	void enterParseKvWithClause(KqlParser.ParseKvWithClauseContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#parseKvWithClause}.
	 * @param ctx the parse tree
	 */
	void exitParseKvWithClause(KqlParser.ParseKvWithClauseContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#partitionOperator}.
	 * @param ctx the parse tree
	 */
	void enterPartitionOperator(KqlParser.PartitionOperatorContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#partitionOperator}.
	 * @param ctx the parse tree
	 */
	void exitPartitionOperator(KqlParser.PartitionOperatorContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#partitionOperatorInClause}.
	 * @param ctx the parse tree
	 */
	void enterPartitionOperatorInClause(KqlParser.PartitionOperatorInClauseContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#partitionOperatorInClause}.
	 * @param ctx the parse tree
	 */
	void exitPartitionOperatorInClause(KqlParser.PartitionOperatorInClauseContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#partitionOperatorSubExpressionBody}.
	 * @param ctx the parse tree
	 */
	void enterPartitionOperatorSubExpressionBody(KqlParser.PartitionOperatorSubExpressionBodyContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#partitionOperatorSubExpressionBody}.
	 * @param ctx the parse tree
	 */
	void exitPartitionOperatorSubExpressionBody(KqlParser.PartitionOperatorSubExpressionBodyContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#partitionOperatorFullExpressionBody}.
	 * @param ctx the parse tree
	 */
	void enterPartitionOperatorFullExpressionBody(KqlParser.PartitionOperatorFullExpressionBodyContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#partitionOperatorFullExpressionBody}.
	 * @param ctx the parse tree
	 */
	void exitPartitionOperatorFullExpressionBody(KqlParser.PartitionOperatorFullExpressionBodyContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#partitionByOperator}.
	 * @param ctx the parse tree
	 */
	void enterPartitionByOperator(KqlParser.PartitionByOperatorContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#partitionByOperator}.
	 * @param ctx the parse tree
	 */
	void exitPartitionByOperator(KqlParser.PartitionByOperatorContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#partitionByOperatorIdClause}.
	 * @param ctx the parse tree
	 */
	void enterPartitionByOperatorIdClause(KqlParser.PartitionByOperatorIdClauseContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#partitionByOperatorIdClause}.
	 * @param ctx the parse tree
	 */
	void exitPartitionByOperatorIdClause(KqlParser.PartitionByOperatorIdClauseContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#printOperator}.
	 * @param ctx the parse tree
	 */
	void enterPrintOperator(KqlParser.PrintOperatorContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#printOperator}.
	 * @param ctx the parse tree
	 */
	void exitPrintOperator(KqlParser.PrintOperatorContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#projectAwayOperator}.
	 * @param ctx the parse tree
	 */
	void enterProjectAwayOperator(KqlParser.ProjectAwayOperatorContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#projectAwayOperator}.
	 * @param ctx the parse tree
	 */
	void exitProjectAwayOperator(KqlParser.ProjectAwayOperatorContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#projectKeepOperator}.
	 * @param ctx the parse tree
	 */
	void enterProjectKeepOperator(KqlParser.ProjectKeepOperatorContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#projectKeepOperator}.
	 * @param ctx the parse tree
	 */
	void exitProjectKeepOperator(KqlParser.ProjectKeepOperatorContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#projectOperator}.
	 * @param ctx the parse tree
	 */
	void enterProjectOperator(KqlParser.ProjectOperatorContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#projectOperator}.
	 * @param ctx the parse tree
	 */
	void exitProjectOperator(KqlParser.ProjectOperatorContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#projectRenameOperator}.
	 * @param ctx the parse tree
	 */
	void enterProjectRenameOperator(KqlParser.ProjectRenameOperatorContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#projectRenameOperator}.
	 * @param ctx the parse tree
	 */
	void exitProjectRenameOperator(KqlParser.ProjectRenameOperatorContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#projectReorderOperator}.
	 * @param ctx the parse tree
	 */
	void enterProjectReorderOperator(KqlParser.ProjectReorderOperatorContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#projectReorderOperator}.
	 * @param ctx the parse tree
	 */
	void exitProjectReorderOperator(KqlParser.ProjectReorderOperatorContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#projectReorderExpression}.
	 * @param ctx the parse tree
	 */
	void enterProjectReorderExpression(KqlParser.ProjectReorderExpressionContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#projectReorderExpression}.
	 * @param ctx the parse tree
	 */
	void exitProjectReorderExpression(KqlParser.ProjectReorderExpressionContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#reduceByOperator}.
	 * @param ctx the parse tree
	 */
	void enterReduceByOperator(KqlParser.ReduceByOperatorContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#reduceByOperator}.
	 * @param ctx the parse tree
	 */
	void exitReduceByOperator(KqlParser.ReduceByOperatorContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#reduceByWithClause}.
	 * @param ctx the parse tree
	 */
	void enterReduceByWithClause(KqlParser.ReduceByWithClauseContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#reduceByWithClause}.
	 * @param ctx the parse tree
	 */
	void exitReduceByWithClause(KqlParser.ReduceByWithClauseContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#renderOperator}.
	 * @param ctx the parse tree
	 */
	void enterRenderOperator(KqlParser.RenderOperatorContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#renderOperator}.
	 * @param ctx the parse tree
	 */
	void exitRenderOperator(KqlParser.RenderOperatorContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#renderOperatorWithClause}.
	 * @param ctx the parse tree
	 */
	void enterRenderOperatorWithClause(KqlParser.RenderOperatorWithClauseContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#renderOperatorWithClause}.
	 * @param ctx the parse tree
	 */
	void exitRenderOperatorWithClause(KqlParser.RenderOperatorWithClauseContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#renderOperatorLegacyPropertyList}.
	 * @param ctx the parse tree
	 */
	void enterRenderOperatorLegacyPropertyList(KqlParser.RenderOperatorLegacyPropertyListContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#renderOperatorLegacyPropertyList}.
	 * @param ctx the parse tree
	 */
	void exitRenderOperatorLegacyPropertyList(KqlParser.RenderOperatorLegacyPropertyListContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#renderOperatorProperty}.
	 * @param ctx the parse tree
	 */
	void enterRenderOperatorProperty(KqlParser.RenderOperatorPropertyContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#renderOperatorProperty}.
	 * @param ctx the parse tree
	 */
	void exitRenderOperatorProperty(KqlParser.RenderOperatorPropertyContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#renderPropertyNameList}.
	 * @param ctx the parse tree
	 */
	void enterRenderPropertyNameList(KqlParser.RenderPropertyNameListContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#renderPropertyNameList}.
	 * @param ctx the parse tree
	 */
	void exitRenderPropertyNameList(KqlParser.RenderPropertyNameListContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#renderOperatorLegacyProperty}.
	 * @param ctx the parse tree
	 */
	void enterRenderOperatorLegacyProperty(KqlParser.RenderOperatorLegacyPropertyContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#renderOperatorLegacyProperty}.
	 * @param ctx the parse tree
	 */
	void exitRenderOperatorLegacyProperty(KqlParser.RenderOperatorLegacyPropertyContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#sampleDistinctOperator}.
	 * @param ctx the parse tree
	 */
	void enterSampleDistinctOperator(KqlParser.SampleDistinctOperatorContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#sampleDistinctOperator}.
	 * @param ctx the parse tree
	 */
	void exitSampleDistinctOperator(KqlParser.SampleDistinctOperatorContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#sampleOperator}.
	 * @param ctx the parse tree
	 */
	void enterSampleOperator(KqlParser.SampleOperatorContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#sampleOperator}.
	 * @param ctx the parse tree
	 */
	void exitSampleOperator(KqlParser.SampleOperatorContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#scanOperator}.
	 * @param ctx the parse tree
	 */
	void enterScanOperator(KqlParser.ScanOperatorContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#scanOperator}.
	 * @param ctx the parse tree
	 */
	void exitScanOperator(KqlParser.ScanOperatorContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#scanOperatorOrderByClause}.
	 * @param ctx the parse tree
	 */
	void enterScanOperatorOrderByClause(KqlParser.ScanOperatorOrderByClauseContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#scanOperatorOrderByClause}.
	 * @param ctx the parse tree
	 */
	void exitScanOperatorOrderByClause(KqlParser.ScanOperatorOrderByClauseContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#scanOperatorPartitionByClause}.
	 * @param ctx the parse tree
	 */
	void enterScanOperatorPartitionByClause(KqlParser.ScanOperatorPartitionByClauseContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#scanOperatorPartitionByClause}.
	 * @param ctx the parse tree
	 */
	void exitScanOperatorPartitionByClause(KqlParser.ScanOperatorPartitionByClauseContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#scanOperatorDeclareClause}.
	 * @param ctx the parse tree
	 */
	void enterScanOperatorDeclareClause(KqlParser.ScanOperatorDeclareClauseContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#scanOperatorDeclareClause}.
	 * @param ctx the parse tree
	 */
	void exitScanOperatorDeclareClause(KqlParser.ScanOperatorDeclareClauseContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#scanOperatorStep}.
	 * @param ctx the parse tree
	 */
	void enterScanOperatorStep(KqlParser.ScanOperatorStepContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#scanOperatorStep}.
	 * @param ctx the parse tree
	 */
	void exitScanOperatorStep(KqlParser.ScanOperatorStepContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#scanOperatorStepOutputClause}.
	 * @param ctx the parse tree
	 */
	void enterScanOperatorStepOutputClause(KqlParser.ScanOperatorStepOutputClauseContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#scanOperatorStepOutputClause}.
	 * @param ctx the parse tree
	 */
	void exitScanOperatorStepOutputClause(KqlParser.ScanOperatorStepOutputClauseContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#scanOperatorBody}.
	 * @param ctx the parse tree
	 */
	void enterScanOperatorBody(KqlParser.ScanOperatorBodyContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#scanOperatorBody}.
	 * @param ctx the parse tree
	 */
	void exitScanOperatorBody(KqlParser.ScanOperatorBodyContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#scanOperatorAssignment}.
	 * @param ctx the parse tree
	 */
	void enterScanOperatorAssignment(KqlParser.ScanOperatorAssignmentContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#scanOperatorAssignment}.
	 * @param ctx the parse tree
	 */
	void exitScanOperatorAssignment(KqlParser.ScanOperatorAssignmentContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#searchOperator}.
	 * @param ctx the parse tree
	 */
	void enterSearchOperator(KqlParser.SearchOperatorContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#searchOperator}.
	 * @param ctx the parse tree
	 */
	void exitSearchOperator(KqlParser.SearchOperatorContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#searchOperatorStarAndExpression}.
	 * @param ctx the parse tree
	 */
	void enterSearchOperatorStarAndExpression(KqlParser.SearchOperatorStarAndExpressionContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#searchOperatorStarAndExpression}.
	 * @param ctx the parse tree
	 */
	void exitSearchOperatorStarAndExpression(KqlParser.SearchOperatorStarAndExpressionContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#searchOperatorInClause}.
	 * @param ctx the parse tree
	 */
	void enterSearchOperatorInClause(KqlParser.SearchOperatorInClauseContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#searchOperatorInClause}.
	 * @param ctx the parse tree
	 */
	void exitSearchOperatorInClause(KqlParser.SearchOperatorInClauseContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#serializeOperator}.
	 * @param ctx the parse tree
	 */
	void enterSerializeOperator(KqlParser.SerializeOperatorContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#serializeOperator}.
	 * @param ctx the parse tree
	 */
	void exitSerializeOperator(KqlParser.SerializeOperatorContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#sortOperator}.
	 * @param ctx the parse tree
	 */
	void enterSortOperator(KqlParser.SortOperatorContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#sortOperator}.
	 * @param ctx the parse tree
	 */
	void exitSortOperator(KqlParser.SortOperatorContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#orderedExpression}.
	 * @param ctx the parse tree
	 */
	void enterOrderedExpression(KqlParser.OrderedExpressionContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#orderedExpression}.
	 * @param ctx the parse tree
	 */
	void exitOrderedExpression(KqlParser.OrderedExpressionContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#sortOrdering}.
	 * @param ctx the parse tree
	 */
	void enterSortOrdering(KqlParser.SortOrderingContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#sortOrdering}.
	 * @param ctx the parse tree
	 */
	void exitSortOrdering(KqlParser.SortOrderingContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#summarizeOperator}.
	 * @param ctx the parse tree
	 */
	void enterSummarizeOperator(KqlParser.SummarizeOperatorContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#summarizeOperator}.
	 * @param ctx the parse tree
	 */
	void exitSummarizeOperator(KqlParser.SummarizeOperatorContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#summarizeOperatorByClause}.
	 * @param ctx the parse tree
	 */
	void enterSummarizeOperatorByClause(KqlParser.SummarizeOperatorByClauseContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#summarizeOperatorByClause}.
	 * @param ctx the parse tree
	 */
	void exitSummarizeOperatorByClause(KqlParser.SummarizeOperatorByClauseContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#summarizeOperatorLegacyBinClause}.
	 * @param ctx the parse tree
	 */
	void enterSummarizeOperatorLegacyBinClause(KqlParser.SummarizeOperatorLegacyBinClauseContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#summarizeOperatorLegacyBinClause}.
	 * @param ctx the parse tree
	 */
	void exitSummarizeOperatorLegacyBinClause(KqlParser.SummarizeOperatorLegacyBinClauseContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#takeOperator}.
	 * @param ctx the parse tree
	 */
	void enterTakeOperator(KqlParser.TakeOperatorContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#takeOperator}.
	 * @param ctx the parse tree
	 */
	void exitTakeOperator(KqlParser.TakeOperatorContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#topOperator}.
	 * @param ctx the parse tree
	 */
	void enterTopOperator(KqlParser.TopOperatorContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#topOperator}.
	 * @param ctx the parse tree
	 */
	void exitTopOperator(KqlParser.TopOperatorContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#topHittersOperator}.
	 * @param ctx the parse tree
	 */
	void enterTopHittersOperator(KqlParser.TopHittersOperatorContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#topHittersOperator}.
	 * @param ctx the parse tree
	 */
	void exitTopHittersOperator(KqlParser.TopHittersOperatorContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#topHittersOperatorByClause}.
	 * @param ctx the parse tree
	 */
	void enterTopHittersOperatorByClause(KqlParser.TopHittersOperatorByClauseContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#topHittersOperatorByClause}.
	 * @param ctx the parse tree
	 */
	void exitTopHittersOperatorByClause(KqlParser.TopHittersOperatorByClauseContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#topNestedOperator}.
	 * @param ctx the parse tree
	 */
	void enterTopNestedOperator(KqlParser.TopNestedOperatorContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#topNestedOperator}.
	 * @param ctx the parse tree
	 */
	void exitTopNestedOperator(KqlParser.TopNestedOperatorContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#topNestedOperatorPart}.
	 * @param ctx the parse tree
	 */
	void enterTopNestedOperatorPart(KqlParser.TopNestedOperatorPartContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#topNestedOperatorPart}.
	 * @param ctx the parse tree
	 */
	void exitTopNestedOperatorPart(KqlParser.TopNestedOperatorPartContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#topNestedOperatorWithOthersClause}.
	 * @param ctx the parse tree
	 */
	void enterTopNestedOperatorWithOthersClause(KqlParser.TopNestedOperatorWithOthersClauseContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#topNestedOperatorWithOthersClause}.
	 * @param ctx the parse tree
	 */
	void exitTopNestedOperatorWithOthersClause(KqlParser.TopNestedOperatorWithOthersClauseContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#unionOperator}.
	 * @param ctx the parse tree
	 */
	void enterUnionOperator(KqlParser.UnionOperatorContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#unionOperator}.
	 * @param ctx the parse tree
	 */
	void exitUnionOperator(KqlParser.UnionOperatorContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#unionOperatorExpression}.
	 * @param ctx the parse tree
	 */
	void enterUnionOperatorExpression(KqlParser.UnionOperatorExpressionContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#unionOperatorExpression}.
	 * @param ctx the parse tree
	 */
	void exitUnionOperatorExpression(KqlParser.UnionOperatorExpressionContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#whereOperator}.
	 * @param ctx the parse tree
	 */
	void enterWhereOperator(KqlParser.WhereOperatorContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#whereOperator}.
	 * @param ctx the parse tree
	 */
	void exitWhereOperator(KqlParser.WhereOperatorContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#contextualSubExpression}.
	 * @param ctx the parse tree
	 */
	void enterContextualSubExpression(KqlParser.ContextualSubExpressionContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#contextualSubExpression}.
	 * @param ctx the parse tree
	 */
	void exitContextualSubExpression(KqlParser.ContextualSubExpressionContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#contextualPipeExpression}.
	 * @param ctx the parse tree
	 */
	void enterContextualPipeExpression(KqlParser.ContextualPipeExpressionContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#contextualPipeExpression}.
	 * @param ctx the parse tree
	 */
	void exitContextualPipeExpression(KqlParser.ContextualPipeExpressionContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#contextualPipeExpressionPipedOperator}.
	 * @param ctx the parse tree
	 */
	void enterContextualPipeExpressionPipedOperator(KqlParser.ContextualPipeExpressionPipedOperatorContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#contextualPipeExpressionPipedOperator}.
	 * @param ctx the parse tree
	 */
	void exitContextualPipeExpressionPipedOperator(KqlParser.ContextualPipeExpressionPipedOperatorContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#strictQueryOperatorParameter}.
	 * @param ctx the parse tree
	 */
	void enterStrictQueryOperatorParameter(KqlParser.StrictQueryOperatorParameterContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#strictQueryOperatorParameter}.
	 * @param ctx the parse tree
	 */
	void exitStrictQueryOperatorParameter(KqlParser.StrictQueryOperatorParameterContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#relaxedQueryOperatorParameter}.
	 * @param ctx the parse tree
	 */
	void enterRelaxedQueryOperatorParameter(KqlParser.RelaxedQueryOperatorParameterContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#relaxedQueryOperatorParameter}.
	 * @param ctx the parse tree
	 */
	void exitRelaxedQueryOperatorParameter(KqlParser.RelaxedQueryOperatorParameterContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#queryOperatorProperty}.
	 * @param ctx the parse tree
	 */
	void enterQueryOperatorProperty(KqlParser.QueryOperatorPropertyContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#queryOperatorProperty}.
	 * @param ctx the parse tree
	 */
	void exitQueryOperatorProperty(KqlParser.QueryOperatorPropertyContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#namedExpression}.
	 * @param ctx the parse tree
	 */
	void enterNamedExpression(KqlParser.NamedExpressionContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#namedExpression}.
	 * @param ctx the parse tree
	 */
	void exitNamedExpression(KqlParser.NamedExpressionContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#namedExpressionNameClause}.
	 * @param ctx the parse tree
	 */
	void enterNamedExpressionNameClause(KqlParser.NamedExpressionNameClauseContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#namedExpressionNameClause}.
	 * @param ctx the parse tree
	 */
	void exitNamedExpressionNameClause(KqlParser.NamedExpressionNameClauseContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#namedExpressionNameList}.
	 * @param ctx the parse tree
	 */
	void enterNamedExpressionNameList(KqlParser.NamedExpressionNameListContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#namedExpressionNameList}.
	 * @param ctx the parse tree
	 */
	void exitNamedExpressionNameList(KqlParser.NamedExpressionNameListContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#scopedFunctionCallExpression}.
	 * @param ctx the parse tree
	 */
	void enterScopedFunctionCallExpression(KqlParser.ScopedFunctionCallExpressionContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#scopedFunctionCallExpression}.
	 * @param ctx the parse tree
	 */
	void exitScopedFunctionCallExpression(KqlParser.ScopedFunctionCallExpressionContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#unnamedExpression}.
	 * @param ctx the parse tree
	 */
	void enterUnnamedExpression(KqlParser.UnnamedExpressionContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#unnamedExpression}.
	 * @param ctx the parse tree
	 */
	void exitUnnamedExpression(KqlParser.UnnamedExpressionContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#logicalOrExpression}.
	 * @param ctx the parse tree
	 */
	void enterLogicalOrExpression(KqlParser.LogicalOrExpressionContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#logicalOrExpression}.
	 * @param ctx the parse tree
	 */
	void exitLogicalOrExpression(KqlParser.LogicalOrExpressionContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#logicalOrOperation}.
	 * @param ctx the parse tree
	 */
	void enterLogicalOrOperation(KqlParser.LogicalOrOperationContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#logicalOrOperation}.
	 * @param ctx the parse tree
	 */
	void exitLogicalOrOperation(KqlParser.LogicalOrOperationContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#logicalAndExpression}.
	 * @param ctx the parse tree
	 */
	void enterLogicalAndExpression(KqlParser.LogicalAndExpressionContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#logicalAndExpression}.
	 * @param ctx the parse tree
	 */
	void exitLogicalAndExpression(KqlParser.LogicalAndExpressionContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#logicalAndOperation}.
	 * @param ctx the parse tree
	 */
	void enterLogicalAndOperation(KqlParser.LogicalAndOperationContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#logicalAndOperation}.
	 * @param ctx the parse tree
	 */
	void exitLogicalAndOperation(KqlParser.LogicalAndOperationContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#equalityExpression}.
	 * @param ctx the parse tree
	 */
	void enterEqualityExpression(KqlParser.EqualityExpressionContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#equalityExpression}.
	 * @param ctx the parse tree
	 */
	void exitEqualityExpression(KqlParser.EqualityExpressionContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#equalsEqualityExpression}.
	 * @param ctx the parse tree
	 */
	void enterEqualsEqualityExpression(KqlParser.EqualsEqualityExpressionContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#equalsEqualityExpression}.
	 * @param ctx the parse tree
	 */
	void exitEqualsEqualityExpression(KqlParser.EqualsEqualityExpressionContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#listEqualityExpression}.
	 * @param ctx the parse tree
	 */
	void enterListEqualityExpression(KqlParser.ListEqualityExpressionContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#listEqualityExpression}.
	 * @param ctx the parse tree
	 */
	void exitListEqualityExpression(KqlParser.ListEqualityExpressionContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#betweenEqualityExpression}.
	 * @param ctx the parse tree
	 */
	void enterBetweenEqualityExpression(KqlParser.BetweenEqualityExpressionContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#betweenEqualityExpression}.
	 * @param ctx the parse tree
	 */
	void exitBetweenEqualityExpression(KqlParser.BetweenEqualityExpressionContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#starEqualityExpression}.
	 * @param ctx the parse tree
	 */
	void enterStarEqualityExpression(KqlParser.StarEqualityExpressionContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#starEqualityExpression}.
	 * @param ctx the parse tree
	 */
	void exitStarEqualityExpression(KqlParser.StarEqualityExpressionContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#relationalExpression}.
	 * @param ctx the parse tree
	 */
	void enterRelationalExpression(KqlParser.RelationalExpressionContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#relationalExpression}.
	 * @param ctx the parse tree
	 */
	void exitRelationalExpression(KqlParser.RelationalExpressionContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#additiveExpression}.
	 * @param ctx the parse tree
	 */
	void enterAdditiveExpression(KqlParser.AdditiveExpressionContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#additiveExpression}.
	 * @param ctx the parse tree
	 */
	void exitAdditiveExpression(KqlParser.AdditiveExpressionContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#additiveOperation}.
	 * @param ctx the parse tree
	 */
	void enterAdditiveOperation(KqlParser.AdditiveOperationContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#additiveOperation}.
	 * @param ctx the parse tree
	 */
	void exitAdditiveOperation(KqlParser.AdditiveOperationContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#multiplicativeExpression}.
	 * @param ctx the parse tree
	 */
	void enterMultiplicativeExpression(KqlParser.MultiplicativeExpressionContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#multiplicativeExpression}.
	 * @param ctx the parse tree
	 */
	void exitMultiplicativeExpression(KqlParser.MultiplicativeExpressionContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#multiplicativeOperation}.
	 * @param ctx the parse tree
	 */
	void enterMultiplicativeOperation(KqlParser.MultiplicativeOperationContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#multiplicativeOperation}.
	 * @param ctx the parse tree
	 */
	void exitMultiplicativeOperation(KqlParser.MultiplicativeOperationContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#stringOperatorExpression}.
	 * @param ctx the parse tree
	 */
	void enterStringOperatorExpression(KqlParser.StringOperatorExpressionContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#stringOperatorExpression}.
	 * @param ctx the parse tree
	 */
	void exitStringOperatorExpression(KqlParser.StringOperatorExpressionContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#stringBinaryOperatorExpression}.
	 * @param ctx the parse tree
	 */
	void enterStringBinaryOperatorExpression(KqlParser.StringBinaryOperatorExpressionContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#stringBinaryOperatorExpression}.
	 * @param ctx the parse tree
	 */
	void exitStringBinaryOperatorExpression(KqlParser.StringBinaryOperatorExpressionContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#stringBinaryOperation}.
	 * @param ctx the parse tree
	 */
	void enterStringBinaryOperation(KqlParser.StringBinaryOperationContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#stringBinaryOperation}.
	 * @param ctx the parse tree
	 */
	void exitStringBinaryOperation(KqlParser.StringBinaryOperationContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#stringBinaryOperator}.
	 * @param ctx the parse tree
	 */
	void enterStringBinaryOperator(KqlParser.StringBinaryOperatorContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#stringBinaryOperator}.
	 * @param ctx the parse tree
	 */
	void exitStringBinaryOperator(KqlParser.StringBinaryOperatorContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#stringStarOperatorExpression}.
	 * @param ctx the parse tree
	 */
	void enterStringStarOperatorExpression(KqlParser.StringStarOperatorExpressionContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#stringStarOperatorExpression}.
	 * @param ctx the parse tree
	 */
	void exitStringStarOperatorExpression(KqlParser.StringStarOperatorExpressionContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#invocationExpression}.
	 * @param ctx the parse tree
	 */
	void enterInvocationExpression(KqlParser.InvocationExpressionContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#invocationExpression}.
	 * @param ctx the parse tree
	 */
	void exitInvocationExpression(KqlParser.InvocationExpressionContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#functionCallOrPathExpression}.
	 * @param ctx the parse tree
	 */
	void enterFunctionCallOrPathExpression(KqlParser.FunctionCallOrPathExpressionContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#functionCallOrPathExpression}.
	 * @param ctx the parse tree
	 */
	void exitFunctionCallOrPathExpression(KqlParser.FunctionCallOrPathExpressionContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#functionCallOrPathRoot}.
	 * @param ctx the parse tree
	 */
	void enterFunctionCallOrPathRoot(KqlParser.FunctionCallOrPathRootContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#functionCallOrPathRoot}.
	 * @param ctx the parse tree
	 */
	void exitFunctionCallOrPathRoot(KqlParser.FunctionCallOrPathRootContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#functionCallOrPathPathExpression}.
	 * @param ctx the parse tree
	 */
	void enterFunctionCallOrPathPathExpression(KqlParser.FunctionCallOrPathPathExpressionContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#functionCallOrPathPathExpression}.
	 * @param ctx the parse tree
	 */
	void exitFunctionCallOrPathPathExpression(KqlParser.FunctionCallOrPathPathExpressionContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#functionCallOrPathOperation}.
	 * @param ctx the parse tree
	 */
	void enterFunctionCallOrPathOperation(KqlParser.FunctionCallOrPathOperationContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#functionCallOrPathOperation}.
	 * @param ctx the parse tree
	 */
	void exitFunctionCallOrPathOperation(KqlParser.FunctionCallOrPathOperationContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#functionalCallOrPathPathOperation}.
	 * @param ctx the parse tree
	 */
	void enterFunctionalCallOrPathPathOperation(KqlParser.FunctionalCallOrPathPathOperationContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#functionalCallOrPathPathOperation}.
	 * @param ctx the parse tree
	 */
	void exitFunctionalCallOrPathPathOperation(KqlParser.FunctionalCallOrPathPathOperationContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#functionCallOrPathElementOperation}.
	 * @param ctx the parse tree
	 */
	void enterFunctionCallOrPathElementOperation(KqlParser.FunctionCallOrPathElementOperationContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#functionCallOrPathElementOperation}.
	 * @param ctx the parse tree
	 */
	void exitFunctionCallOrPathElementOperation(KqlParser.FunctionCallOrPathElementOperationContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#legacyFunctionCallOrPathElementOperation}.
	 * @param ctx the parse tree
	 */
	void enterLegacyFunctionCallOrPathElementOperation(KqlParser.LegacyFunctionCallOrPathElementOperationContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#legacyFunctionCallOrPathElementOperation}.
	 * @param ctx the parse tree
	 */
	void exitLegacyFunctionCallOrPathElementOperation(KqlParser.LegacyFunctionCallOrPathElementOperationContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#toScalarExpression}.
	 * @param ctx the parse tree
	 */
	void enterToScalarExpression(KqlParser.ToScalarExpressionContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#toScalarExpression}.
	 * @param ctx the parse tree
	 */
	void exitToScalarExpression(KqlParser.ToScalarExpressionContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#toTableExpression}.
	 * @param ctx the parse tree
	 */
	void enterToTableExpression(KqlParser.ToTableExpressionContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#toTableExpression}.
	 * @param ctx the parse tree
	 */
	void exitToTableExpression(KqlParser.ToTableExpressionContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#noOptimizationParameter}.
	 * @param ctx the parse tree
	 */
	void enterNoOptimizationParameter(KqlParser.NoOptimizationParameterContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#noOptimizationParameter}.
	 * @param ctx the parse tree
	 */
	void exitNoOptimizationParameter(KqlParser.NoOptimizationParameterContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#dotCompositeFunctionCallExpression}.
	 * @param ctx the parse tree
	 */
	void enterDotCompositeFunctionCallExpression(KqlParser.DotCompositeFunctionCallExpressionContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#dotCompositeFunctionCallExpression}.
	 * @param ctx the parse tree
	 */
	void exitDotCompositeFunctionCallExpression(KqlParser.DotCompositeFunctionCallExpressionContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#dotCompositeFunctionCallOperation}.
	 * @param ctx the parse tree
	 */
	void enterDotCompositeFunctionCallOperation(KqlParser.DotCompositeFunctionCallOperationContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#dotCompositeFunctionCallOperation}.
	 * @param ctx the parse tree
	 */
	void exitDotCompositeFunctionCallOperation(KqlParser.DotCompositeFunctionCallOperationContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#functionCallExpression}.
	 * @param ctx the parse tree
	 */
	void enterFunctionCallExpression(KqlParser.FunctionCallExpressionContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#functionCallExpression}.
	 * @param ctx the parse tree
	 */
	void exitFunctionCallExpression(KqlParser.FunctionCallExpressionContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#namedFunctionCallExpression}.
	 * @param ctx the parse tree
	 */
	void enterNamedFunctionCallExpression(KqlParser.NamedFunctionCallExpressionContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#namedFunctionCallExpression}.
	 * @param ctx the parse tree
	 */
	void exitNamedFunctionCallExpression(KqlParser.NamedFunctionCallExpressionContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#argumentExpression}.
	 * @param ctx the parse tree
	 */
	void enterArgumentExpression(KqlParser.ArgumentExpressionContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#argumentExpression}.
	 * @param ctx the parse tree
	 */
	void exitArgumentExpression(KqlParser.ArgumentExpressionContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#countExpression}.
	 * @param ctx the parse tree
	 */
	void enterCountExpression(KqlParser.CountExpressionContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#countExpression}.
	 * @param ctx the parse tree
	 */
	void exitCountExpression(KqlParser.CountExpressionContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#starExpression}.
	 * @param ctx the parse tree
	 */
	void enterStarExpression(KqlParser.StarExpressionContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#starExpression}.
	 * @param ctx the parse tree
	 */
	void exitStarExpression(KqlParser.StarExpressionContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#primaryExpression}.
	 * @param ctx the parse tree
	 */
	void enterPrimaryExpression(KqlParser.PrimaryExpressionContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#primaryExpression}.
	 * @param ctx the parse tree
	 */
	void exitPrimaryExpression(KqlParser.PrimaryExpressionContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#nameReferenceWithDataScope}.
	 * @param ctx the parse tree
	 */
	void enterNameReferenceWithDataScope(KqlParser.NameReferenceWithDataScopeContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#nameReferenceWithDataScope}.
	 * @param ctx the parse tree
	 */
	void exitNameReferenceWithDataScope(KqlParser.NameReferenceWithDataScopeContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#dataScopeClause}.
	 * @param ctx the parse tree
	 */
	void enterDataScopeClause(KqlParser.DataScopeClauseContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#dataScopeClause}.
	 * @param ctx the parse tree
	 */
	void exitDataScopeClause(KqlParser.DataScopeClauseContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#parenthesizedExpression}.
	 * @param ctx the parse tree
	 */
	void enterParenthesizedExpression(KqlParser.ParenthesizedExpressionContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#parenthesizedExpression}.
	 * @param ctx the parse tree
	 */
	void exitParenthesizedExpression(KqlParser.ParenthesizedExpressionContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#rangeExpression}.
	 * @param ctx the parse tree
	 */
	void enterRangeExpression(KqlParser.RangeExpressionContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#rangeExpression}.
	 * @param ctx the parse tree
	 */
	void exitRangeExpression(KqlParser.RangeExpressionContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#entityExpression}.
	 * @param ctx the parse tree
	 */
	void enterEntityExpression(KqlParser.EntityExpressionContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#entityExpression}.
	 * @param ctx the parse tree
	 */
	void exitEntityExpression(KqlParser.EntityExpressionContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#entityPathOrElementExpression}.
	 * @param ctx the parse tree
	 */
	void enterEntityPathOrElementExpression(KqlParser.EntityPathOrElementExpressionContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#entityPathOrElementExpression}.
	 * @param ctx the parse tree
	 */
	void exitEntityPathOrElementExpression(KqlParser.EntityPathOrElementExpressionContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#entityPathOrElementOperator}.
	 * @param ctx the parse tree
	 */
	void enterEntityPathOrElementOperator(KqlParser.EntityPathOrElementOperatorContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#entityPathOrElementOperator}.
	 * @param ctx the parse tree
	 */
	void exitEntityPathOrElementOperator(KqlParser.EntityPathOrElementOperatorContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#entityPathOperator}.
	 * @param ctx the parse tree
	 */
	void enterEntityPathOperator(KqlParser.EntityPathOperatorContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#entityPathOperator}.
	 * @param ctx the parse tree
	 */
	void exitEntityPathOperator(KqlParser.EntityPathOperatorContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#entityElementOperator}.
	 * @param ctx the parse tree
	 */
	void enterEntityElementOperator(KqlParser.EntityElementOperatorContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#entityElementOperator}.
	 * @param ctx the parse tree
	 */
	void exitEntityElementOperator(KqlParser.EntityElementOperatorContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#legacyEntityPathElementOperator}.
	 * @param ctx the parse tree
	 */
	void enterLegacyEntityPathElementOperator(KqlParser.LegacyEntityPathElementOperatorContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#legacyEntityPathElementOperator}.
	 * @param ctx the parse tree
	 */
	void exitLegacyEntityPathElementOperator(KqlParser.LegacyEntityPathElementOperatorContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#entityName}.
	 * @param ctx the parse tree
	 */
	void enterEntityName(KqlParser.EntityNameContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#entityName}.
	 * @param ctx the parse tree
	 */
	void exitEntityName(KqlParser.EntityNameContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#entityNameReference}.
	 * @param ctx the parse tree
	 */
	void enterEntityNameReference(KqlParser.EntityNameReferenceContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#entityNameReference}.
	 * @param ctx the parse tree
	 */
	void exitEntityNameReference(KqlParser.EntityNameReferenceContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#atSignName}.
	 * @param ctx the parse tree
	 */
	void enterAtSignName(KqlParser.AtSignNameContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#atSignName}.
	 * @param ctx the parse tree
	 */
	void exitAtSignName(KqlParser.AtSignNameContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#extendedPathName}.
	 * @param ctx the parse tree
	 */
	void enterExtendedPathName(KqlParser.ExtendedPathNameContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#extendedPathName}.
	 * @param ctx the parse tree
	 */
	void exitExtendedPathName(KqlParser.ExtendedPathNameContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#wildcardedEntityExpression}.
	 * @param ctx the parse tree
	 */
	void enterWildcardedEntityExpression(KqlParser.WildcardedEntityExpressionContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#wildcardedEntityExpression}.
	 * @param ctx the parse tree
	 */
	void exitWildcardedEntityExpression(KqlParser.WildcardedEntityExpressionContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#wildcardedPathExpression}.
	 * @param ctx the parse tree
	 */
	void enterWildcardedPathExpression(KqlParser.WildcardedPathExpressionContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#wildcardedPathExpression}.
	 * @param ctx the parse tree
	 */
	void exitWildcardedPathExpression(KqlParser.WildcardedPathExpressionContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#wildcardedPathName}.
	 * @param ctx the parse tree
	 */
	void enterWildcardedPathName(KqlParser.WildcardedPathNameContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#wildcardedPathName}.
	 * @param ctx the parse tree
	 */
	void exitWildcardedPathName(KqlParser.WildcardedPathNameContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#contextualDataTableExpression}.
	 * @param ctx the parse tree
	 */
	void enterContextualDataTableExpression(KqlParser.ContextualDataTableExpressionContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#contextualDataTableExpression}.
	 * @param ctx the parse tree
	 */
	void exitContextualDataTableExpression(KqlParser.ContextualDataTableExpressionContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#dataTableExpression}.
	 * @param ctx the parse tree
	 */
	void enterDataTableExpression(KqlParser.DataTableExpressionContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#dataTableExpression}.
	 * @param ctx the parse tree
	 */
	void exitDataTableExpression(KqlParser.DataTableExpressionContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#rowSchema}.
	 * @param ctx the parse tree
	 */
	void enterRowSchema(KqlParser.RowSchemaContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#rowSchema}.
	 * @param ctx the parse tree
	 */
	void exitRowSchema(KqlParser.RowSchemaContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#rowSchemaColumnDeclaration}.
	 * @param ctx the parse tree
	 */
	void enterRowSchemaColumnDeclaration(KqlParser.RowSchemaColumnDeclarationContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#rowSchemaColumnDeclaration}.
	 * @param ctx the parse tree
	 */
	void exitRowSchemaColumnDeclaration(KqlParser.RowSchemaColumnDeclarationContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#externalDataExpression}.
	 * @param ctx the parse tree
	 */
	void enterExternalDataExpression(KqlParser.ExternalDataExpressionContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#externalDataExpression}.
	 * @param ctx the parse tree
	 */
	void exitExternalDataExpression(KqlParser.ExternalDataExpressionContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#externalDataWithClause}.
	 * @param ctx the parse tree
	 */
	void enterExternalDataWithClause(KqlParser.ExternalDataWithClauseContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#externalDataWithClause}.
	 * @param ctx the parse tree
	 */
	void exitExternalDataWithClause(KqlParser.ExternalDataWithClauseContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#externalDataWithClauseProperty}.
	 * @param ctx the parse tree
	 */
	void enterExternalDataWithClauseProperty(KqlParser.ExternalDataWithClausePropertyContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#externalDataWithClauseProperty}.
	 * @param ctx the parse tree
	 */
	void exitExternalDataWithClauseProperty(KqlParser.ExternalDataWithClausePropertyContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#materializedViewCombineExpression}.
	 * @param ctx the parse tree
	 */
	void enterMaterializedViewCombineExpression(KqlParser.MaterializedViewCombineExpressionContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#materializedViewCombineExpression}.
	 * @param ctx the parse tree
	 */
	void exitMaterializedViewCombineExpression(KqlParser.MaterializedViewCombineExpressionContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#materializeViewCombineBaseClause}.
	 * @param ctx the parse tree
	 */
	void enterMaterializeViewCombineBaseClause(KqlParser.MaterializeViewCombineBaseClauseContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#materializeViewCombineBaseClause}.
	 * @param ctx the parse tree
	 */
	void exitMaterializeViewCombineBaseClause(KqlParser.MaterializeViewCombineBaseClauseContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#materializedViewCombineDeltaClause}.
	 * @param ctx the parse tree
	 */
	void enterMaterializedViewCombineDeltaClause(KqlParser.MaterializedViewCombineDeltaClauseContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#materializedViewCombineDeltaClause}.
	 * @param ctx the parse tree
	 */
	void exitMaterializedViewCombineDeltaClause(KqlParser.MaterializedViewCombineDeltaClauseContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#materializedViewCombineAggregationsClause}.
	 * @param ctx the parse tree
	 */
	void enterMaterializedViewCombineAggregationsClause(KqlParser.MaterializedViewCombineAggregationsClauseContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#materializedViewCombineAggregationsClause}.
	 * @param ctx the parse tree
	 */
	void exitMaterializedViewCombineAggregationsClause(KqlParser.MaterializedViewCombineAggregationsClauseContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#scalarType}.
	 * @param ctx the parse tree
	 */
	void enterScalarType(KqlParser.ScalarTypeContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#scalarType}.
	 * @param ctx the parse tree
	 */
	void exitScalarType(KqlParser.ScalarTypeContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#extendedScalarType}.
	 * @param ctx the parse tree
	 */
	void enterExtendedScalarType(KqlParser.ExtendedScalarTypeContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#extendedScalarType}.
	 * @param ctx the parse tree
	 */
	void exitExtendedScalarType(KqlParser.ExtendedScalarTypeContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#parameterName}.
	 * @param ctx the parse tree
	 */
	void enterParameterName(KqlParser.ParameterNameContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#parameterName}.
	 * @param ctx the parse tree
	 */
	void exitParameterName(KqlParser.ParameterNameContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#simpleNameReference}.
	 * @param ctx the parse tree
	 */
	void enterSimpleNameReference(KqlParser.SimpleNameReferenceContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#simpleNameReference}.
	 * @param ctx the parse tree
	 */
	void exitSimpleNameReference(KqlParser.SimpleNameReferenceContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#extendedNameReference}.
	 * @param ctx the parse tree
	 */
	void enterExtendedNameReference(KqlParser.ExtendedNameReferenceContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#extendedNameReference}.
	 * @param ctx the parse tree
	 */
	void exitExtendedNameReference(KqlParser.ExtendedNameReferenceContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#wildcardedNameReference}.
	 * @param ctx the parse tree
	 */
	void enterWildcardedNameReference(KqlParser.WildcardedNameReferenceContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#wildcardedNameReference}.
	 * @param ctx the parse tree
	 */
	void exitWildcardedNameReference(KqlParser.WildcardedNameReferenceContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#simpleOrWildcardedNameReference}.
	 * @param ctx the parse tree
	 */
	void enterSimpleOrWildcardedNameReference(KqlParser.SimpleOrWildcardedNameReferenceContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#simpleOrWildcardedNameReference}.
	 * @param ctx the parse tree
	 */
	void exitSimpleOrWildcardedNameReference(KqlParser.SimpleOrWildcardedNameReferenceContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#identifierName}.
	 * @param ctx the parse tree
	 */
	void enterIdentifierName(KqlParser.IdentifierNameContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#identifierName}.
	 * @param ctx the parse tree
	 */
	void exitIdentifierName(KqlParser.IdentifierNameContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#keywordName}.
	 * @param ctx the parse tree
	 */
	void enterKeywordName(KqlParser.KeywordNameContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#keywordName}.
	 * @param ctx the parse tree
	 */
	void exitKeywordName(KqlParser.KeywordNameContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#extendedKeywordName}.
	 * @param ctx the parse tree
	 */
	void enterExtendedKeywordName(KqlParser.ExtendedKeywordNameContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#extendedKeywordName}.
	 * @param ctx the parse tree
	 */
	void exitExtendedKeywordName(KqlParser.ExtendedKeywordNameContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#escapedName}.
	 * @param ctx the parse tree
	 */
	void enterEscapedName(KqlParser.EscapedNameContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#escapedName}.
	 * @param ctx the parse tree
	 */
	void exitEscapedName(KqlParser.EscapedNameContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#identifierOrKeywordName}.
	 * @param ctx the parse tree
	 */
	void enterIdentifierOrKeywordName(KqlParser.IdentifierOrKeywordNameContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#identifierOrKeywordName}.
	 * @param ctx the parse tree
	 */
	void exitIdentifierOrKeywordName(KqlParser.IdentifierOrKeywordNameContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#identifierOrKeywordOrEscapedName}.
	 * @param ctx the parse tree
	 */
	void enterIdentifierOrKeywordOrEscapedName(KqlParser.IdentifierOrKeywordOrEscapedNameContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#identifierOrKeywordOrEscapedName}.
	 * @param ctx the parse tree
	 */
	void exitIdentifierOrKeywordOrEscapedName(KqlParser.IdentifierOrKeywordOrEscapedNameContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#identifierOrExtendedKeywordOrEscapedName}.
	 * @param ctx the parse tree
	 */
	void enterIdentifierOrExtendedKeywordOrEscapedName(KqlParser.IdentifierOrExtendedKeywordOrEscapedNameContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#identifierOrExtendedKeywordOrEscapedName}.
	 * @param ctx the parse tree
	 */
	void exitIdentifierOrExtendedKeywordOrEscapedName(KqlParser.IdentifierOrExtendedKeywordOrEscapedNameContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#identifierOrExtendedKeywordName}.
	 * @param ctx the parse tree
	 */
	void enterIdentifierOrExtendedKeywordName(KqlParser.IdentifierOrExtendedKeywordNameContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#identifierOrExtendedKeywordName}.
	 * @param ctx the parse tree
	 */
	void exitIdentifierOrExtendedKeywordName(KqlParser.IdentifierOrExtendedKeywordNameContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#wildcardedName}.
	 * @param ctx the parse tree
	 */
	void enterWildcardedName(KqlParser.WildcardedNameContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#wildcardedName}.
	 * @param ctx the parse tree
	 */
	void exitWildcardedName(KqlParser.WildcardedNameContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#wildcardedNamePrefix}.
	 * @param ctx the parse tree
	 */
	void enterWildcardedNamePrefix(KqlParser.WildcardedNamePrefixContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#wildcardedNamePrefix}.
	 * @param ctx the parse tree
	 */
	void exitWildcardedNamePrefix(KqlParser.WildcardedNamePrefixContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#wildcardedNameSegment}.
	 * @param ctx the parse tree
	 */
	void enterWildcardedNameSegment(KqlParser.WildcardedNameSegmentContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#wildcardedNameSegment}.
	 * @param ctx the parse tree
	 */
	void exitWildcardedNameSegment(KqlParser.WildcardedNameSegmentContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#literalExpression}.
	 * @param ctx the parse tree
	 */
	void enterLiteralExpression(KqlParser.LiteralExpressionContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#literalExpression}.
	 * @param ctx the parse tree
	 */
	void exitLiteralExpression(KqlParser.LiteralExpressionContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#unsignedLiteralExpression}.
	 * @param ctx the parse tree
	 */
	void enterUnsignedLiteralExpression(KqlParser.UnsignedLiteralExpressionContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#unsignedLiteralExpression}.
	 * @param ctx the parse tree
	 */
	void exitUnsignedLiteralExpression(KqlParser.UnsignedLiteralExpressionContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#numberLikeLiteralExpression}.
	 * @param ctx the parse tree
	 */
	void enterNumberLikeLiteralExpression(KqlParser.NumberLikeLiteralExpressionContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#numberLikeLiteralExpression}.
	 * @param ctx the parse tree
	 */
	void exitNumberLikeLiteralExpression(KqlParser.NumberLikeLiteralExpressionContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#numericLiteralExpression}.
	 * @param ctx the parse tree
	 */
	void enterNumericLiteralExpression(KqlParser.NumericLiteralExpressionContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#numericLiteralExpression}.
	 * @param ctx the parse tree
	 */
	void exitNumericLiteralExpression(KqlParser.NumericLiteralExpressionContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#signedLiteralExpression}.
	 * @param ctx the parse tree
	 */
	void enterSignedLiteralExpression(KqlParser.SignedLiteralExpressionContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#signedLiteralExpression}.
	 * @param ctx the parse tree
	 */
	void exitSignedLiteralExpression(KqlParser.SignedLiteralExpressionContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#longLiteralExpression}.
	 * @param ctx the parse tree
	 */
	void enterLongLiteralExpression(KqlParser.LongLiteralExpressionContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#longLiteralExpression}.
	 * @param ctx the parse tree
	 */
	void exitLongLiteralExpression(KqlParser.LongLiteralExpressionContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#intLiteralExpression}.
	 * @param ctx the parse tree
	 */
	void enterIntLiteralExpression(KqlParser.IntLiteralExpressionContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#intLiteralExpression}.
	 * @param ctx the parse tree
	 */
	void exitIntLiteralExpression(KqlParser.IntLiteralExpressionContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#realLiteralExpression}.
	 * @param ctx the parse tree
	 */
	void enterRealLiteralExpression(KqlParser.RealLiteralExpressionContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#realLiteralExpression}.
	 * @param ctx the parse tree
	 */
	void exitRealLiteralExpression(KqlParser.RealLiteralExpressionContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#decimalLiteralExpression}.
	 * @param ctx the parse tree
	 */
	void enterDecimalLiteralExpression(KqlParser.DecimalLiteralExpressionContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#decimalLiteralExpression}.
	 * @param ctx the parse tree
	 */
	void exitDecimalLiteralExpression(KqlParser.DecimalLiteralExpressionContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#dateTimeLiteralExpression}.
	 * @param ctx the parse tree
	 */
	void enterDateTimeLiteralExpression(KqlParser.DateTimeLiteralExpressionContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#dateTimeLiteralExpression}.
	 * @param ctx the parse tree
	 */
	void exitDateTimeLiteralExpression(KqlParser.DateTimeLiteralExpressionContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#timeSpanLiteralExpression}.
	 * @param ctx the parse tree
	 */
	void enterTimeSpanLiteralExpression(KqlParser.TimeSpanLiteralExpressionContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#timeSpanLiteralExpression}.
	 * @param ctx the parse tree
	 */
	void exitTimeSpanLiteralExpression(KqlParser.TimeSpanLiteralExpressionContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#booleanLiteralExpression}.
	 * @param ctx the parse tree
	 */
	void enterBooleanLiteralExpression(KqlParser.BooleanLiteralExpressionContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#booleanLiteralExpression}.
	 * @param ctx the parse tree
	 */
	void exitBooleanLiteralExpression(KqlParser.BooleanLiteralExpressionContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#guidLiteralExpression}.
	 * @param ctx the parse tree
	 */
	void enterGuidLiteralExpression(KqlParser.GuidLiteralExpressionContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#guidLiteralExpression}.
	 * @param ctx the parse tree
	 */
	void exitGuidLiteralExpression(KqlParser.GuidLiteralExpressionContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#typeLiteralExpression}.
	 * @param ctx the parse tree
	 */
	void enterTypeLiteralExpression(KqlParser.TypeLiteralExpressionContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#typeLiteralExpression}.
	 * @param ctx the parse tree
	 */
	void exitTypeLiteralExpression(KqlParser.TypeLiteralExpressionContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#signedLongLiteralExpression}.
	 * @param ctx the parse tree
	 */
	void enterSignedLongLiteralExpression(KqlParser.SignedLongLiteralExpressionContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#signedLongLiteralExpression}.
	 * @param ctx the parse tree
	 */
	void exitSignedLongLiteralExpression(KqlParser.SignedLongLiteralExpressionContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#signedRealLiteralExpression}.
	 * @param ctx the parse tree
	 */
	void enterSignedRealLiteralExpression(KqlParser.SignedRealLiteralExpressionContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#signedRealLiteralExpression}.
	 * @param ctx the parse tree
	 */
	void exitSignedRealLiteralExpression(KqlParser.SignedRealLiteralExpressionContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#stringLiteralExpression}.
	 * @param ctx the parse tree
	 */
	void enterStringLiteralExpression(KqlParser.StringLiteralExpressionContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#stringLiteralExpression}.
	 * @param ctx the parse tree
	 */
	void exitStringLiteralExpression(KqlParser.StringLiteralExpressionContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#dynamicLiteralExpression}.
	 * @param ctx the parse tree
	 */
	void enterDynamicLiteralExpression(KqlParser.DynamicLiteralExpressionContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#dynamicLiteralExpression}.
	 * @param ctx the parse tree
	 */
	void exitDynamicLiteralExpression(KqlParser.DynamicLiteralExpressionContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#jsonValue}.
	 * @param ctx the parse tree
	 */
	void enterJsonValue(KqlParser.JsonValueContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#jsonValue}.
	 * @param ctx the parse tree
	 */
	void exitJsonValue(KqlParser.JsonValueContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#jsonObject}.
	 * @param ctx the parse tree
	 */
	void enterJsonObject(KqlParser.JsonObjectContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#jsonObject}.
	 * @param ctx the parse tree
	 */
	void exitJsonObject(KqlParser.JsonObjectContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#jsonPair}.
	 * @param ctx the parse tree
	 */
	void enterJsonPair(KqlParser.JsonPairContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#jsonPair}.
	 * @param ctx the parse tree
	 */
	void exitJsonPair(KqlParser.JsonPairContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#jsonArray}.
	 * @param ctx the parse tree
	 */
	void enterJsonArray(KqlParser.JsonArrayContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#jsonArray}.
	 * @param ctx the parse tree
	 */
	void exitJsonArray(KqlParser.JsonArrayContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#jsonBoolean}.
	 * @param ctx the parse tree
	 */
	void enterJsonBoolean(KqlParser.JsonBooleanContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#jsonBoolean}.
	 * @param ctx the parse tree
	 */
	void exitJsonBoolean(KqlParser.JsonBooleanContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#jsonDateTime}.
	 * @param ctx the parse tree
	 */
	void enterJsonDateTime(KqlParser.JsonDateTimeContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#jsonDateTime}.
	 * @param ctx the parse tree
	 */
	void exitJsonDateTime(KqlParser.JsonDateTimeContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#jsonGuid}.
	 * @param ctx the parse tree
	 */
	void enterJsonGuid(KqlParser.JsonGuidContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#jsonGuid}.
	 * @param ctx the parse tree
	 */
	void exitJsonGuid(KqlParser.JsonGuidContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#jsonNull}.
	 * @param ctx the parse tree
	 */
	void enterJsonNull(KqlParser.JsonNullContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#jsonNull}.
	 * @param ctx the parse tree
	 */
	void exitJsonNull(KqlParser.JsonNullContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#jsonString}.
	 * @param ctx the parse tree
	 */
	void enterJsonString(KqlParser.JsonStringContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#jsonString}.
	 * @param ctx the parse tree
	 */
	void exitJsonString(KqlParser.JsonStringContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#jsonTimeSpan}.
	 * @param ctx the parse tree
	 */
	void enterJsonTimeSpan(KqlParser.JsonTimeSpanContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#jsonTimeSpan}.
	 * @param ctx the parse tree
	 */
	void exitJsonTimeSpan(KqlParser.JsonTimeSpanContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#jsonLong}.
	 * @param ctx the parse tree
	 */
	void enterJsonLong(KqlParser.JsonLongContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#jsonLong}.
	 * @param ctx the parse tree
	 */
	void exitJsonLong(KqlParser.JsonLongContext ctx);
	/**
	 * Enter a parse tree produced by {@link KqlParser#jsonReal}.
	 * @param ctx the parse tree
	 */
	void enterJsonReal(KqlParser.JsonRealContext ctx);
	/**
	 * Exit a parse tree produced by {@link KqlParser#jsonReal}.
	 * @param ctx the parse tree
	 */
	void exitJsonReal(KqlParser.JsonRealContext ctx);
}