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
        public static Diagnostic GetMissingCharacter(char ch)
        {
            return new Diagnostic($"Missing '{ch}'");
        }

        public static Diagnostic GetUnexpectedCharacter(string text)
        {
            return new Diagnostic($"Unexpected '{text}'");
        }

        public static Diagnostic GetMalformedToken(string term)
        {
            return new Diagnostic($"Malformed {term}");
        }

        public static Diagnostic GetMalformedLiteral()
        {
            return new Diagnostic("Malformed literal");
        }

        public static Diagnostic GetTokenExpected(params SyntaxKind[] kinds)
        {
            return GetTokenExpected((IReadOnlyList<SyntaxKind>)kinds);
        }

        public static Diagnostic GetTokenExpected(IReadOnlyList<SyntaxKind> kinds)
        {
            var list = kinds.Select(k => k.GetText()).ToList().Join(", ", " or ");
            return new Diagnostic($"Expected {list}");
        }

        public static Diagnostic GetTokenExpected(IReadOnlyList<string> texts)
        {
            var list = texts.Join(", ", " or ");
            return new Diagnostic($"Expected {list}");
        }

        public static Diagnostic GetTokenExpected(params string[] tokens)
        {
            return GetTokenExpected((IReadOnlyList<string>)tokens);
        }

        private static Diagnostic GetMissingElement(string term)
        {
            return new Diagnostic($"Missing {term}");
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

        public static Diagnostic GetParsePatternMustStartWithColumnNameOrStar()
        {
            return new Diagnostic("The pattern must start with a column name or *");
        }

        public static Diagnostic GetParsePatternNameDoesNotFollowStringLiteral()
        {
            return new Diagnostic("The column name must follow a string literal");
        }

        public static Diagnostic GetParsePatternStringLiteralMustFollowStar()
        {
            return new Diagnostic("A string literal must follow a *");
        }

        public static Diagnostic GetParsePatternUsingStarAfterStringColumnIsAmbiguous()
        {
            return new Diagnostic("Using * after parsing a string column is abmiguous.");
        }

        public static Diagnostic GetInvalidPatternPart()
        {
            return new Diagnostic("Invalid pattern part.");
        }

        public static Diagnostic GetIdentifierNameOnly()
        {
            return new Diagnostic("The name must be a single identifier only.");
        }

        public static Diagnostic GetOperatorNotDefined(string name, params TypeSymbol[] argumentTypes)
        {
            return GetOperatorNotDefined(name, (IReadOnlyList<TypeSymbol>)argumentTypes);
        }

        public static Diagnostic GetOperatorNotDefined(string name, IReadOnlyList<TypeSymbol> argumentTypes)
        {
            if (argumentTypes.Count == 1)
            {
                return new Diagnostic($"The operator '{name}' is not defined for the operand type {argumentTypes[0].Name}.");
            }
            else
            {
                var list = argumentTypes.Select(t => t.Name).ToList().Join(", ", " and ");
                return new Diagnostic($"The operator '{name}' is not defined for the operand types {list}.");
            }
        }

        public static Diagnostic GetTypeExpected(Symbol type)
        {
            return new Diagnostic($"A value of type '{type.Name}' expected.");
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
                return new Diagnostic($"A value of type {list} expected.");
            }
        }

        public static Diagnostic GetScalarTypeExpected()
        {
            return new Diagnostic($"Scalar value expected.");
        }

        public static Diagnostic GetColumnExpected()
        {
            return new Diagnostic("Column name expected.");
        }

        public static Diagnostic GetRenameAssignmentExpected()
        {
            return new Diagnostic("Column rename assignment expected.");
        }

        public static Diagnostic GetTableExpected()
        {
            return new Diagnostic("Table expected.");
        }

        public static Diagnostic GetTableOrScalarExpected()
        {
            return new Diagnostic("A table or scalar value expected.");
        }

        public static Diagnostic GetSingleColumnTableExpected()
        {
            return new Diagnostic("A table with only one column expected.");
        }

        public static Diagnostic GetDatabaseExpected()
        {
            return new Diagnostic("Database expected.");
        }

        public static Diagnostic GetClusterExpected()
        {
            return new Diagnostic("Cluster expected.");
        }

        public static Diagnostic GetTypeNotAllowed(Symbol type)
        {
            return new Diagnostic($"The value of type '{type.Name}' is not allowed in this context.");
        }

        public static Diagnostic GetFunctionRequiresArgumentList(string functionName)
        {
            return new Diagnostic($"The function '{functionName}' requires an argument list.");
        }

        public static Diagnostic GetArgumentCountExpected(int count)
        {
            if (count == 0)
            {
                return new Diagnostic($"No arguments expected.");
            }
            else if (count == 1)
            {
                return new Diagnostic($"1 argument expected.");
            }
            else
            {
                return new Diagnostic($"{count} arguments expected.");
            }
        }

        public static Diagnostic GetFunctionExpectsArgumentCountExact(string functionName, int count)
        {
            if (count == 0)
            {
                return new Diagnostic($"The function '{functionName}' expects no arguments.");
            }
            else if (count == 1)
            {
                return new Diagnostic($"The function '{functionName}' expects 1 argument.");
            }
            else
            {
                return new Diagnostic($"The function '{functionName}' expects {count} arguments.");
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
                return new Diagnostic($"The function '{functionName}' expects between {min} and {max} arguments.");
            }
        }

        public static Diagnostic GetFunctionHasIncorrectNumberOfArguments()
        {
            return new Diagnostic($"The function call has an incorrect number of arguments.");
        }

        public static Diagnostic GetScalarFunctionNotDefined(string name)
        {
            return new Diagnostic($"The scalar function '{name}' is not defined.");
        }

        public static Diagnostic GetAggregateFunctionNotDefined(string name)
        {
            return new Diagnostic($"The aggregate function '{name}' is not defined.");
        }

        public static Diagnostic GetPlugInFunctionNotDefined(string name)
        {
            return new Diagnostic($"The plug-in function '{name}' is not defined.");
        }

        public static Diagnostic GetFunctionNotDefinedWithMatchingParameters(string name, IReadOnlyList<Symbol> argumentTypes)
        {
            var types = string.Join(", ", argumentTypes.Select(p => p.Name));
            return new Diagnostic($"The function '{name}' is not compatible with arguments ({types})");
        }

        public static Diagnostic GetNameIsNotAFunction(string name)
        {
            return new Diagnostic($"The name '{name}' does not refer to a function.");
        }

        public static Diagnostic GetExpressionMustBeConstant()
        {
            return new Diagnostic("The expression must be a constant.");
        }

        public static Diagnostic GetExpressionMustBeConstantOrIdentifier()
        {
            return new Diagnostic("The expression must be a constant or identifier.");
        }

        public static Diagnostic GetExpressionMustBeLiteral()
        {
            return new Diagnostic($"The expression must be a literal.");
        }

        public static Diagnostic GetExpressionMustBeLiteralScalarValue()
        {
            return new Diagnostic($"The expression must be a literal scalar value.");
        }

        public static Diagnostic GetExpressionMustNotBeEmpty()
        {
            return new Diagnostic($"The expression value must not be empty.");
        }

        public static Diagnostic GetExpressionMustBeInteger()
        {
            return new Diagnostic("The expression value must be an integer.");
        }

        public static Diagnostic GetExpressionMustBeRealOrDecimal()
        {
            return new Diagnostic("The expression value must be an real or decimal number.");
        }

        public static Diagnostic GetExpressionMustBeIntegerOrDynamic()
        {
            return new Diagnostic("The expression value must be an integer or dynamic.");
        }


        public static Diagnostic GetExpressionMustBeNumeric()
        {
            return new Diagnostic("The expression value must be a number.");
        }

        public static Diagnostic GetExpressionMustBeSummable()
        {
            return new Diagnostic("The argument value must be a number, timespan or datetime.");
        }

        public static Diagnostic GetMultiValuedExpressionCannotBeAssignedToVariable()
        {
            return new Diagnostic("The multi-valued expression cannot be assigned to a variable.");
        }

        public static Diagnostic GetExpressionMustHaveValue<T>(IReadOnlyList<T> values)
        {
            if (values.Count == 1)
            {
                return new Diagnostic($"The expression must be the value: {values[0]}");
            }
            else
            {
                var list = values.Select(v => v.ToString()).ToList().Join(", ", " or ");
                return new Diagnostic($"The expression must be one of the values: {list}");
            }
        }

        public static Diagnostic GetExpressionMustHaveValue<T>(params T[] values)
        {
            return GetExpressionMustHaveValue((IReadOnlyList<T>)values);
        }

        public static Diagnostic GetExpressionMustHaveType(IReadOnlyList<Symbol> types)
        {
            if (types.Count == 1)
            {
                return new Diagnostic($"The expression must have the type {types[0].Name}.");
            }
            else
            {
                var list = types.Select(s => s.Name).ToList().Join(", ", " or ");
                return new Diagnostic($"The expression must have one of the types: {list}.");
            }
        }

        public static Diagnostic GetExpressionMustHaveType(params Symbol[] types)
        {
            return GetExpressionMustHaveType((IReadOnlyList<Symbol>)types);
        }

        public static Diagnostic GetNameDoesNotReferToAnyKnownItem(string name)
        {
            return new Diagnostic($"The name '{name}' does not refer to any known column, table, variable or function.");
        }

        public static Diagnostic GetNameDoesNotReferToAnyKnownFunction(string name)
        {
            return new Diagnostic($"The name '{name}' does not refer to any known function.");
        }

        public static Diagnostic GetAggregateNotAllowedInThisContext(string name)
        {
            return new Diagnostic($"The aggregate function '{name}' is not allowed in this context.");
        }

        public static Diagnostic GetColumnMustExistOnBothSidesOfJoin(string name)
        {
            return new Diagnostic($"The column '{name}' must exist on both sides of the join.");
        }

        public static Diagnostic GetNameRefersToMoreThanOneItem(string name)
        {
            return new Diagnostic($"The name '{name}' refers to more than one column or variable");
        }

        public static Diagnostic GetTheElementAccessOperatorIsNotAllowedInThisContext()
        {
            return new Diagnostic("The element access operator [] is not allowed in this context.");
        }

        public static Diagnostic GetTheExpressionHasNoName()
        {
            return new Diagnostic("A column name cannot be inferred for this expression.");
        }

        public static Diagnostic GetTheExpressionDoesNotHaveMultipleValues()
        {
            return new Diagnostic("The expression does not have multiple named values.");
        }

        public static Diagnostic GetTheNameDoesNotHaveCorrespondingExpression()
        {
            return new Diagnostic("The name does not have a corresponding expression.");
        }

        public static Diagnostic GetInvalidTypeName(string name)
        {
            return new Diagnostic($"The name '{name}' is not a valid type name.");
        }

        public static Diagnostic GetInvalidColumnDeclaration()
        {
            return new Diagnostic("The syntax is not a valid column declaration.");
        }

        public static Diagnostic GetDuplicateColumnDeclaration(string name)
        {
            return new Diagnostic($"A column with the name '{name}' is already declared.");
        }

        public static Diagnostic GetInvalidTypeExpression()
        {
            return new Diagnostic("The syntax is not a valid type expression.");
        }

        public static Diagnostic GetIncorrectNumberOfDataValues(int multiple)
        {
            return new Diagnostic($"Incorrect number of data values. The values should appear in multiples of {multiple}.");
        }

        public static Diagnostic GetQueryOperatorCannotBeFirst()
        {
            return new Diagnostic($"The operator cannot be the first operator in a query.");
        }

        public static Diagnostic GetQueryOperatorMustBeFirst()
        {
            return new Diagnostic("The operator must be the first operator in the query.");
        }

        public static Diagnostic GetQueryOperatorExpected()
        {
            return new Diagnostic("Query operator expected.");
        }

        public static Diagnostic GetTypeIsNotIntervalType(Symbol intervalType, Symbol rangeType)
        {
            return new Diagnostic($"The type '{intervalType.Name}' is not an appropriate interval type for '{rangeType.Name}'");
        }

        public static Diagnostic GetUnknownParameterName(string name)
        {
            return new Diagnostic($"The '{name}' is not a recognized parameter.");
        }

        public static Diagnostic GetParameterAlreadySpecified(string name)
        {
            return new Diagnostic($"The parameter '{name}' is already specified.");
        }

        public static Diagnostic GetNameDoesNotReferToTable(string name)
        {
            return new Diagnostic($"The name '{name}' does not refer to a table.");
        }

        public static Diagnostic GetInvalidJoinCondition()
        {
            return new Diagnostic("The join condition must be either the name of a column common to both tables or in the form $left.<column> == $right.<column>.");
        }
        public static Diagnostic GetInvalidJoinConditionOperand(string prefix)
        {
            return new Diagnostic($"The join condition operand must be: {prefix}.<column>");
        }

        public static Diagnostic GetTheExpressionRefersToMoreThanOneColumn()
        {
            return new Diagnostic("The expression refers to more than one column.");
        }

        public static Diagnostic GetPackMustBeLastItemInList()
        {
            return new Diagnostic("The pack(*) expression must be the last item in the list.");
        }

        public static Diagnostic GetValueCountMustEqualParameterCount()
        {
            return new Diagnostic("The number of values must equal the number of parameters.");
        }

        public static Diagnostic GetPathValueWithNoPathParameter()
        {
            return new Diagnostic("A path value can only be specified when a path name is part of the declaration.");
        }

        public static Diagnostic GetPathValueExpected()
        {
            return new Diagnostic("A path value is expected.");
        }

        public static Diagnostic GetNoPatternMatchesArguments()
        {
            return new Diagnostic("No pattern matches the specified arguments.");
        }

        public static Diagnostic GetDefaultValueExpected()
        {
            return new Diagnostic("Default value expected.");
        }

        public static Diagnostic GetTableHasNoColumns()
        {
            return new Diagnostic("The table has no columns");
        }

        public static Diagnostic GetStarExpressionNotAllowed()
        {
            return new Diagnostic("The * syntax is not allowed here.");
        }

        public static Diagnostic GetStarExpressionMustBeLastArgument()
        {
            return new Diagnostic("The * syntax must be the last argument.");
        }

        public static Diagnostic GetNamedArgumentsNotSupported()
        {
            return new Diagnostic("Named arguments are not supported for this function.");
        }

        public static Diagnostic GetCompoundNamedArgumentsNotSupported()
        {
            return new Diagnostic("Compound named arguments are not supported.");
        }

        public static Diagnostic GetUnnamedArgumentAfterOutofOrderNamedArgument()
        {
            return new Diagnostic("All arguments after an unordered named argument must be named.");
        }

        public static Diagnostic GetUnknownArgumentName()
        {
            return new Diagnostic($"The argument name does not refer to a declared parameter.");
        }

        public static Diagnostic GetMissingArgumentForParameter(string parameterName)
        {
            return new Diagnostic($"The argument for parameter '{parameterName}' is missing.");
        }

        public static Diagnostic GetIncompleteFragment()
        {
            return new Diagnostic("The incomplete fragment is unexpected.");
        }

        public static Diagnostic GetNoColumnsInScope()
        {
            return new Diagnostic("No columns are currently in scope.");
        }

        public static Diagnostic GetErrorInExpansion(string name, string errors)
        {
            return new Diagnostic($"Failure in expansion of '{name}': {errors}");
        }

        #region command diagnostics
        public static Diagnostic GetMissingCommand()
        {
            return new Diagnostic("Missing command.");
        }
        #endregion

    }
}