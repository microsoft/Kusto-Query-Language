using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language
{
    using Symbols;
    using Syntax;
    using System.Text;
    using Utils;

    public static class DiagnosticFacts
    {
        public static Diagnostic GetMissingText(string text)
        {
            return new Diagnostic("KS001", $"Missing '{text}'");
        }

        public static Diagnostic GetUnexpectedCharacter(string text)
        {
            return new Diagnostic("KS002", $"Unexpected: '{text}'");
        }

        public static Diagnostic GetMalformedToken(string term)
        {
            return new Diagnostic("KS003", $"Malformed {term}");
        }

        public static Diagnostic GetMalformedLiteral()
        {
            return new Diagnostic("KS004", "Malformed literal");
        }

        public static Diagnostic GetTokenExpected(params SyntaxKind[] kinds)
        {
            return GetTokenExpected((IReadOnlyList<SyntaxKind>)kinds);
        }

        public static Diagnostic GetTokenExpected(IReadOnlyList<SyntaxKind> kinds)
        {
            return GetTokenExpected(kinds.Select(k => k.GetText()));
        }

        public static Diagnostic GetTokenExpected(IEnumerable<string> texts)
        {
            var list = texts.Select(t => $"'{t}'").ToArray().Join(", ", " or ");
            return new Diagnostic("KS005", $"Expected: {list}");
        }

        public static Diagnostic GetTokenExpected(params string[] tokens)
        {
            return GetTokenExpected((IReadOnlyList<string>)tokens);
        }

        private static Diagnostic GetMissingElement(string term)
        {
            return new Diagnostic("KS006", $"Missing {term}");
        }

        public static Diagnostic GetMissingName()
        {
            return GetMissingElement("name");
        }

        public static Diagnostic GetMissingValue()
        {
            return GetMissingElement("value");
        }

        public static Diagnostic GetMissingExpression()
        {
            return GetMissingElement("expression");
        }

        public static Diagnostic GetMissingNumber()
        {
            return GetMissingElement("number");
        }

        public static Diagnostic GetMissingString()
        {
            return GetMissingElement("string");
        }

        public static Diagnostic GetMissingBoolean()
        {
            return GetMissingElement("boolean");
        }

        public static Diagnostic GetMissingTypeOfLiteral()
        {
            return GetMissingElement("typeof");
        }

        public static Diagnostic GetMissingFunctionCall()
        {
            return GetMissingElement("function call");
        }

        public static Diagnostic GetMissingFunctionDeclaration()
        {
            return GetMissingElement("function declaration");
        }

        public static Diagnostic GetMissingTypeName()
        {
            return GetMissingElement("type name");
        }

        public static Diagnostic GetMissingParameter()
        {
            return GetMissingElement("parameter");
        }

        public static Diagnostic GetMissingFirstOrLast()
        {
            return GetMissingElement("first or last");
        }

        public static Diagnostic GetMissingJsonValue()
        {
            return GetMissingElement("json value");
        }

        public static Diagnostic GetMissingJoinOnClause()
        {
            return GetMissingElement("join on condition clause");
        }

        public static Diagnostic GetMissingJsonPair()
        {
            return GetMissingElement("json key:value pair");
        }

        public static Diagnostic GetMissingStatement()
        {
            return GetMissingElement("statement");
        }

        public static Diagnostic GetMissingPatternMatch()
        {
            return GetMissingElement("pattern match clause");
        }

        public static Diagnostic GetMissingClause()
        {
            return GetMissingElement("clause");
        }

        public static Diagnostic GetMissingClause(string clauseName)
        {
            return GetMissingElement($"{clauseName} clause");
        }

        public static Diagnostic GetMissingSchemaDeclaration()
        {
            return GetMissingElement("schema declaration");
        }

        public static Diagnostic GetParsePatternMustStartWithColumnNameOrStar()
        {
            return new Diagnostic("KS100", "The pattern must start with a column name or *");
        }

        public static Diagnostic GetParsePatternNameDoesNotFollowStringLiteral()
        {
            return new Diagnostic("KS101", "The column name must follow a string literal");
        }

        public static Diagnostic GetParsePatternStringLiteralMustFollowStar()
        {
            return new Diagnostic("KS102", "A string literal must follow a *");
        }

        public static Diagnostic GetParsePatternUsingStarAfterStringColumnIsAmbiguous()
        {
            return new Diagnostic("KS103", "Using * after parsing a string column is ambiguous.");
        }

        public static Diagnostic GetInvalidPatternPart()
        {
            return new Diagnostic("KS104", "Invalid pattern part.");
        }

        public static Diagnostic GetIdentifierNameOnly()
        {
            return new Diagnostic("KS105", "The name must be a single identifier only.");
        }

        public static Diagnostic GetOperatorNotDefined(string name, params TypeSymbol[] argumentTypes)
        {
            return GetOperatorNotDefined(name, (IReadOnlyList<TypeSymbol>)argumentTypes);
        }

        public static Diagnostic GetOperatorNotDefined(string name, IReadOnlyList<TypeSymbol> argumentTypes)
        {
            if (argumentTypes.Count == 1)
            {
                return new Diagnostic("KS106", $"The operator '{name}' is not defined for the operand type {argumentTypes[0].Name}.");
            }
            else
            {
                var list = argumentTypes.Select(t => t.Name).ToList().Join(", ", " and ");
                return new Diagnostic("KS106", $"The operator '{name}' is not defined for the operand types {list}.");
            }
        }

        public static Diagnostic GetTypeExpected(Symbol type)
        {
            return new Diagnostic("KS107", $"A value of type '{type.Name}' expected.");
        }

        public static Diagnostic GetTypeExpected(IReadOnlyList<TypeSymbol> types)
        {
            if (types.Count == 0)
            {
                return GetTypeExpected(types[0]);
            }
            else
            {
                var list = types.Select(t => "'" + t.Name + "'").ToList().Join(", ", " or ");
                return new Diagnostic("KS107", $"A value of type {list} expected.");
            }
        }

        public static Diagnostic GetScalarTypeExpected()
        {
            return new Diagnostic("KS108", $"Scalar value expected.");
        }

        public static Diagnostic GetColumnExpected()
        {
            return new Diagnostic("KS109", "Column name expected.");
        }

        public static Diagnostic GetRenameAssignmentExpected()
        {
            return new Diagnostic("KS110", "Column rename assignment expected.");
        }

        public static Diagnostic GetTableExpected()
        {
            return new Diagnostic("KS111", "Table expected.");
        }

        public static Diagnostic GetTableOrScalarExpected()
        {
            return new Diagnostic("KS112", "A table or scalar value expected.");
        }

        public static Diagnostic GetSingleColumnTableExpected()
        {
            return new Diagnostic("KS113", "A table with only one column expected.");
        }

        public static Diagnostic GetDatabaseExpected()
        {
            return new Diagnostic("KS114", "Database expected.");
        }

        public static Diagnostic GetClusterExpected()
        {
            return new Diagnostic("KS115", "Cluster expected.");
        }

        public static Diagnostic GetTypeNotAllowed(Symbol type)
        {
            return new Diagnostic("KS116", $"The value of type '{type.Name}' is not allowed in this context.");
        }

        public static Diagnostic GetFunctionRequiresArgumentList(string functionName)
        {
            return new Diagnostic("KS117", $"The function '{functionName}' requires an argument list.");
        }

        public static Diagnostic GetArgumentCountExpected(int count)
        {
            if (count == 0)
            {
                return new Diagnostic("KS118", $"No arguments expected.");
            }
            else if (count == 1)
            {
                return new Diagnostic("KS118", $"1 argument expected.");
            }
            else
            {
                return new Diagnostic("KS118", $"{count} arguments expected.");
            }
        }

        public static Diagnostic GetFunctionExpectsArgumentCountExact(string functionName, int count)
        {
            if (count == 0)
            {
                return new Diagnostic("KS119", $"The function '{functionName}' expects no arguments.");
            }
            else if (count == 1)
            {
                return new Diagnostic("KS119", $"The function '{functionName}' expects 1 argument.");
            }
            else
            {
                return new Diagnostic("KS119", $"The function '{functionName}' expects {count} arguments.");
            }
        }

        public static Diagnostic GetFunctionExpectsArgumentCountRange(string functionName, int min, int max)
        {
            if (min == max)
            {
                return GetFunctionExpectsArgumentCountExact(functionName, min);
            }
            else
            {
                return new Diagnostic("KS120", $"The function '{functionName}' expects between {min} and {max} arguments.");
            }
        }

        public static Diagnostic GetFunctionHasIncorrectNumberOfArguments()
        {
            return new Diagnostic("KS121", $"The function call has an incorrect number of arguments.");
        }

        public static Diagnostic GetScalarFunctionNotDefined(string name)
        {
            return new Diagnostic("KS122", $"The scalar function '{name}' is not defined.");
        }

        public static Diagnostic GetAggregateFunctionNotDefined(string name)
        {
            return new Diagnostic("KS123", $"The aggregate function '{name}' is not defined.");
        }

        public static Diagnostic GetPlugInFunctionNotDefined(string name)
        {
            return new Diagnostic("KS124", $"The plug-in function '{name}' is not defined.");
        }

        public static Diagnostic GetPlugInFunctionIsNotEnabled(string name)
        {
            return new Diagnostic("KS125", $"The plug-in function '{name}' is not enabled.");
        }

        public static Diagnostic GetPluginNotAllowedInThisContext(string name)
        {
            return new Diagnostic("KS126", $"The plug-in function '{name}' is not allowed in this context.");
        }

        public static Diagnostic GetFunctionNotDefinedWithMatchingParameters(string name, IReadOnlyList<Symbol> argumentTypes)
        {
            var types = string.Join(", ", argumentTypes.Select(p => p.Name));
            return new Diagnostic("KS127", $"The function '{name}' is not compatible with arguments ({types})");
        }

        public static Diagnostic GetNameIsNotAFunction(string name)
        {
            return new Diagnostic("KS128", $"The name '{name}' does not refer to a function.");
        }

        public static Diagnostic GetExpressionMustBeConstant()
        {
            return new Diagnostic("KS129", "The expression must be a constant.");
        }

        public static Diagnostic GetExpressionMustBeConstantOrIdentifier()
        {
            return new Diagnostic("KS130", "The expression must be a constant or identifier.");
        }

        public static Diagnostic GetExpressionMustBeLiteral()
        {
            return new Diagnostic("KS131", $"The expression must be a literal.");
        }

        public static Diagnostic GetExpressionMustBeLiteralScalarValue()
        {
            return new Diagnostic("KS132", $"The expression must be a literal scalar value.");
        }

        public static Diagnostic GetExpressionMustNotBeEmpty()
        {
            return new Diagnostic("KS133", $"The expression value must not be empty.");
        }

        public static Diagnostic GetExpressionMustBeInteger()
        {
            return new Diagnostic("KS134", "The expression value must be an integer.");
        }

        public static Diagnostic GetExpressionMustBeRealOrDecimal()
        {
            return new Diagnostic("KS135", "The expression value must be an real or decimal number.");
        }

        public static Diagnostic GetExpressionMustBeIntegerOrDynamic()
        {
            return new Diagnostic("KS136", "The expression value must be an integer or dynamic.");
        }


        public static Diagnostic GetExpressionMustBeNumeric()
        {
            return new Diagnostic("KS137", "The expression value must be a number.");
        }

        public static Diagnostic GetExpressionMustBeSummable()
        {
            return new Diagnostic("KS138", "The argument value must be summable: a number, timespan or datetime.");
        }

        public static Diagnostic GetMultiValuedExpressionCannotBeAssignedToVariable()
        {
            return new Diagnostic("KS139", "The multi-valued expression cannot be assigned to a variable.");
        }

        public static Diagnostic GetExpressionMustHaveValue<T>(IReadOnlyList<T> values)
        {
            if (values.Count == 1)
            {
                return new Diagnostic("KS140", $"The expression must be the value: {values[0]}");
            }
            else
            {
                var list = values.Select(v => v.ToString()).ToList().Join(", ", " or ");
                return new Diagnostic("KS140", $"The expression must be one of the values: {list}");
            }
        }

        public static Diagnostic GetExpressionMustHaveValue<T>(params T[] values)
        {
            return GetExpressionMustHaveValue((IReadOnlyList<T>)values);
        }

        public static Diagnostic GetExpressionMustHaveType<S>(IReadOnlyList<S> types)
            where S : Symbol
        {
            if (types.Count == 1)
            {
                return new Diagnostic("KS141", $"The expression must have the type {types[0].Name}.");
            }
            else
            {
                var list = types.Select(s => s.Name).ToList().Join(", ", " or ");
                return new Diagnostic("KS141", $"The expression must have one of the types: {list}.");
            }
        }

        public static Diagnostic GetExpressionMustHaveType<S>(params S[] types)
            where S : Symbol
        {
            return GetExpressionMustHaveType((IReadOnlyList<S>)types);
        }

        public static Diagnostic GetNameDoesNotReferToAnyKnownItem(string name)
        {
            return new Diagnostic("KS142", $"The name '{name}' does not refer to any known column, table, variable or function.");
        }

        public static Diagnostic GetFunctionNotDefined(string name)
        {
            return new Diagnostic("KS143", $"The function '{name}' is not defined.");
        }

        public static Diagnostic GetAggregateNotAllowedInThisContext(string name)
        {
            return new Diagnostic("KS144", $"The aggregate function '{name}' is not allowed in this context.");
        }

        public static Diagnostic GetColumnMustExistOnBothSidesOfJoin(string name)
        {
            return new Diagnostic("KS145", $"The column '{name}' must exist on both sides of the join.");
        }

        public static Diagnostic GetNameRefersToMoreThanOneItem(string name)
        {
            return new Diagnostic("KS146", $"The name '{name}' refers to more than one column or variable");
        }

        public static Diagnostic GetTheElementAccessOperatorIsNotAllowedInThisContext()
        {
            return new Diagnostic("KS147", "The element access operator [] is not allowed in this context.");
        }

        public static Diagnostic GetTheExpressionHasNoName()
        {
            return new Diagnostic("KS148", "A column name cannot be inferred for this expression.");
        }

        public static Diagnostic GetTheExpressionDoesNotHaveMultipleValues()
        {
            return new Diagnostic("KS149", "The expression does not have multiple named values.");
        }

        public static Diagnostic GetTheNameDoesNotHaveCorrespondingExpression()
        {
            return new Diagnostic("KS150", "The name does not have a corresponding expression.");
        }

        public static Diagnostic GetInvalidTypeName(string name)
        {
            return new Diagnostic("KS160", $"The name '{name}' is not a valid type name.");
        }

        public static Diagnostic GetInvalidColumnDeclaration()
        {
            return new Diagnostic("KS170", "The syntax is not a valid column declaration.");
        }

        public static Diagnostic GetDuplicateColumnDeclaration(string name)
        {
            return new Diagnostic("KS171", $"A column with the name '{name}' is already declared.");
        }

        public static Diagnostic GetInvalidTypeExpression()
        {
            return new Diagnostic("KS172", "The syntax is not a valid type expression.");
        }

        public static Diagnostic GetIncorrectNumberOfDataValues(int multiple)
        {
            return new Diagnostic("KS173", $"Incorrect number of data values. The values should appear in multiples of {multiple}.");
        }

        public static Diagnostic GetQueryOperatorCannotBeFirst()
        {
            return new Diagnostic("KS174", $"The operator cannot be the first operator in a query.");
        }

        public static Diagnostic GetQueryOperatorMustBeFirst()
        {
            return new Diagnostic("KS175", "The operator must be the first operator in the query.");
        }

        public static Diagnostic GetQueryOperatorExpected()
        {
            return new Diagnostic("KS176", "Query operator expected.");
        }

        public static Diagnostic GetQueryOperatorNotAllowedInContext(string name)
        {
            return new Diagnostic("KS177", $"The query operator '{name}' is not allowed in the current context.");
        }

        public static Diagnostic GetTypeIsNotIntervalType(Symbol intervalType, Symbol rangeType)
        {
            return new Diagnostic("KS178", $"The type '{intervalType.Name}' is not an appropriate interval type for '{rangeType.Name}'");
        }

        public static Diagnostic GetUnknownQueryOperatorParameterName(string name)
        {
            return new Diagnostic("KS179", $"The name '{name}' is not a recognized parameter for this operator.").WithSeverity(DiagnosticSeverity.Warning);
        }

        public static Diagnostic GetParameterAlreadySpecified(string name)
        {
            return new Diagnostic("KS180", $"The parameter '{name}' is already specified.");
        }

        public static Diagnostic GetNameDoesNotReferToTable(string name)
        {
            return new Diagnostic("KS181", $"The name '{name}' does not refer to a table.");
        }

        public static Diagnostic GetInvalidJoinCondition()
        {
            return new Diagnostic("KS182", "The join condition must be either the name of a column common to both tables or in the form $left.<column> == $right.<column>.");
        }
        public static Diagnostic GetInvalidJoinConditionOperand(string prefix)
        {
            return new Diagnostic("KS183", $"The join condition operand must be: {prefix}.<column>");
        }

        public static Diagnostic GetTheExpressionRefersToMoreThanOneColumn()
        {
            return new Diagnostic("KS184", "The expression refers to more than one column.");
        }

        public static Diagnostic GetPackMustBeLastItemInList()
        {
            return new Diagnostic("KS185", "The pack(*) expression must be the last item in the list.");
        }

        public static Diagnostic GetValueCountMustEqualParameterCount()
        {
            return new Diagnostic("KS185", "The number of values must equal the number of parameters.");
        }

        public static Diagnostic GetPathValueWithNoPathParameter()
        {
            return new Diagnostic("KS186", "A path value can only be specified when a path name is part of the declaration.");
        }

        public static Diagnostic GetPathValueExpected()
        {
            return new Diagnostic("KS187", "A path value is expected.");
        }

        public static Diagnostic GetNoPatternMatchesArguments()
        {
            return new Diagnostic("KS188", "No pattern matches the specified arguments.");
        }

        public static Diagnostic GetDefaultValueExpected()
        {
            return new Diagnostic("KS189", "Default value expected.");
        }

        public static Diagnostic GetTableHasNoColumns()
        {
            return new Diagnostic("KS190", "The table has no columns");
        }

        public static Diagnostic GetStarExpressionNotAllowed()
        {
            return new Diagnostic("KS191", "The * syntax is not allowed here.");
        }

        public static Diagnostic GetStarExpressionMustBeLastArgument()
        {
            return new Diagnostic("KS192", "The * syntax must be the last argument.");
        }

        public static Diagnostic GetNamedArgumentsNotSupported()
        {
            return new Diagnostic("KS193", "Named arguments are not supported for this function.");
        }

        public static Diagnostic GetCompoundNamedArgumentsNotSupported()
        {
            return new Diagnostic("KS194", "Compound named arguments are not supported.");
        }

        public static Diagnostic GetUnnamedArgumentAfterOutofOrderNamedArgument()
        {
            return new Diagnostic("KS195", "All arguments after an unordered named argument must be named.");
        }

        public static Diagnostic GetUnknownArgumentName()
        {
            return new Diagnostic("KS196", $"The argument name does not refer to a declared parameter.");
        }

        public static Diagnostic GetMissingArgumentForParameter(string parameterName)
        {
            return new Diagnostic("KS197", $"The argument for parameter '{parameterName}' is missing.");
        }

        public static Diagnostic GetIncompleteFragment()
        {
            return new Diagnostic("KS198", "The incomplete fragment is unexpected.");
        }

        public static Diagnostic GetNoColumnsInScope()
        {
            return new Diagnostic("KS199", "No columns are currently in scope.");
        }

        public static Diagnostic GetErrorInExpansion(string name, string errors)
        {
            return new Diagnostic("KS200", $"Failure in expansion of '{name}': {errors}");
        }

        public static Diagnostic GetVariableAlreadyDeclared(string name)
        {
            return new Diagnostic("KS201", $"A variable with the name '{name}' has already been declared.");
        }

        public static Diagnostic GetMaterializedViewNameMustBeStringLiteral()
        {
            return new Diagnostic("KS202", $"Materialized view name must be a string literal");
        }

        public static Diagnostic AnalysisFailure(string analyzerName, string message)
        {
            return new Diagnostic("KS203", $"Failure in analysis '{analyzerName}': {message}");
        }

        public static Diagnostic GetNameDoesNotReferToAnyKnownTable(string name)
        {
            return new Diagnostic("KS204", $"The name '{name}' does not refer to any known table, tabular variable or function.");
        }

        public static Diagnostic GetFuzzyUnionOperandNotDefined(string name)
        {
            return new Diagnostic("KS205",
                $"The fuzzy union operand '{name}' does not refer to any known table, tabular variable or function.")
                .WithSeverity(DiagnosticSeverity.Warning);
        }

        public static Diagnostic GetExpressionMustBeOrderable()
        {
            return new Diagnostic("KS206", "The argument value must be orderable: a number, timespan, datetime, string or boolean.");
        }

        public static Diagnostic GetNameDoesNotReferToAnyKnownCluster(string name)
        {
            return new Diagnostic("KS207", $"The name '{name}' does not refer to any known cluster.");
        }

        public static Diagnostic GetNameDoesNotReferToAnyKnownDatabase(string name)
        {
            return new Diagnostic("KS208", $"The name '{name}' does not refer to any known database.");
        }

        public static Diagnostic GetNameDoesNotReferToAnyKnownExternalTable(string name)
        {
            return new Diagnostic("KS209", $"The name '{name}' does not refer to any known external table.");
        }

        public static Diagnostic GetNameDoesNotReferToAnyKnownMaterializedView(string name)
        {
            return new Diagnostic("KS210", $"The name '{name}' does not refer to any known materialized view.");
        }

        public static Diagnostic GetNameDoesNotReferToAnyKnownFunction(string name)
        {
            return new Diagnostic("KS211", $"The name '{name}' does not refer to any known function.");
        }

        public static Diagnostic GetBestEffortUnionOperandNotDefined(string name)
        {
            return new Diagnostic("KS212",
                            $"The best_effort union operand '{name}' does not refer to any known table, tabular variable or function.")
                            .WithSeverity(DiagnosticSeverity.Warning);
        }

        #region command diagnostics
        public static Diagnostic GetMissingCommand()
        {
            return new Diagnostic("KS300", "Missing command.");
        }
        #endregion
    }
}