// <#+
#if !T4
namespace Kusto.Language.Generators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
#endif

    public class CommandGenerator
    {
        private readonly StringBuilder _builder;
        private string _lineStartIndentation;
        private const string _indent = "    ";
        private bool _isLineStart = true;
        private static IReadOnlyDictionary<string, RuleInfo> _rulesMap;

        public CommandGenerator()
        {
            _builder = new StringBuilder();

            if (_rulesMap == null)
                _rulesMap = CreateRulesMap();
        }

        public static IReadOnlyList<CommandInfo> GetCommandInfos(Type commandGroupType)
        {
            return commandGroupType.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
                .Where(f => f.FieldType == typeof(CommandInfo))
                .Select(f => (CommandInfo)f.GetValue(null))
                .ToArray();
        }

        public string GenerateParser(string className, Type commandGroupType)
        {
            return GenerateParser(className, GetCommandInfos(commandGroupType));
        }

        public string GenerateParser(string className, IReadOnlyList<CommandInfo> commandInfos)
        {
            _builder.Clear();

            var initialCommandAndGrammars = ParseCommandGrammars(commandInfos);

            // remove unnecessary required grammar nodes to simplify partial parsing
            initialCommandAndGrammars = initialCommandAndGrammars.Select(cag =>
                new CommandAndGrammar(cag.Info, cag.Grammar.Apply(g => g is RequiredGrammar rg ? rg.Required : g), cag.GrammarName))
                .ToList();

            var adjustedCommandAndGrammars = initialCommandAndGrammars;
            var analysis = GrammarAnalysis.Create(adjustedCommandAndGrammars.Select(cag => cag.Grammar).ToArray());

            WriteHeader(className);

            WriteLine("using System;");
            WriteLine("using System.Linq;");
            WriteLine("using System.Collections.Generic;");
            WriteLine("using Kusto.Language.Symbols;");
            WriteLine("using Kusto.Language.Syntax;");
            WriteLine("using Kusto.Language.Editor;");
            WriteLine();
            WriteLine("namespace Kusto.Language.Parsing");
            WriteBraceNested(() =>
            {
                WriteLine("using static Parsers<LexicalToken>;");
                WriteLine("using static SyntaxParsers;");
                WriteLine("using Utils;");
                WriteLine("using System.Text;");

                WriteLine();
                WriteLine($"public class {className} : CommandGrammar");
                WriteBraceNested(() =>
                {
                    WriteLine($"public {className}(GlobalState globals) : base(globals)");
                    WriteLine("{");
                    WriteLine("}");

                    WriteLine();
                    WriteLine("internal override CommandParserInfo[] CreateCommandParsers(PredefinedRuleParsers rules)");
                    WriteBraceNested(() =>
                    {
                        var shapeMap = WriteCommonShapes(adjustedCommandAndGrammars);
                        var missingMap = WriteCommonMissings(adjustedCommandAndGrammars, shapeMap);
                        WriteCommandGrammarParsers(adjustedCommandAndGrammars, shapeMap, missingMap, analysis);

                        WriteLine("var commandParsers = new CommandParserInfo[]");
                        WriteNested("{", "};", () =>
                        {
                            for (int i = 0; i < adjustedCommandAndGrammars.Count; i++)
                            {
                                var commandAndGrammar = adjustedCommandAndGrammars[i];
                                Write($"new CommandParserInfo(\"{commandAndGrammar.GrammarName}\", {LegalName(commandAndGrammar.GrammarName)})");
                                if (i < adjustedCommandAndGrammars.Count - 1)
                                    WriteLine(",");
                                else
                                    WriteLine();
                            }
                        });

                        WriteLine();

                        WriteLine($"return commandParsers;");
                    });
                });
            });

            return _builder.ToString();
        }

        private void WriteHeader(string templateName)
        {
            WriteLine($"// WARNING: Do not modify this file");
            WriteLine($"//          This file is auto generated from the template file '{templateName}.tt'");
            WriteLine($"//          Instead modify the corresponding input info file in the Kusto.Language.Generator project.");
            WriteLine($"//          After making changes, use the right-click menu on the .tt file and select 'run custom tool'.");
            WriteLine();
        }

        public string GenerateSymbols(string className, Type commandGroupType)
        {
            return GenerateSymbols(className, GetCommandInfos(commandGroupType));
        }

        public string GenerateSymbols(string className, IReadOnlyList<CommandInfo> commandInfos)
        {
            _builder.Clear();

            WriteHeader(className);

            WriteLine("using System;");
            WriteLine("using System.Linq;");
            WriteLine("using System.Collections.Generic;");
            WriteLine("using Kusto.Language.Symbols;");
            WriteLine();
            WriteLine("namespace Kusto.Language");
            WriteBraceNested(() =>
            {
                WriteLine($"public static class {className}");
                WriteBraceNested(() =>
                {
                    var dups = BuildDuplicateSchemaMap(commandInfos);
                    if (dups.Count > 0)
                    {
                        foreach (var pair in dups)
                        {
                            WriteLine($"private static readonly string {pair.Value} = {GetDoubleQuotedStringLiteral(pair.Key)};");
                        }

                        WriteLine();
                    }

                    foreach (var info in commandInfos)
                    {
                        WriteLine($"public static readonly CommandSymbol {LegalName(info.Name)} =");
                        WriteNested(() =>
                        {
                            if (!dups.TryGetValue(info.Schema, out var schema))
                                schema = GetDoubleQuotedStringLiteral(info.Schema);

                            if (string.IsNullOrEmpty(info.Construction))
                            {
                                WriteLine($"new CommandSymbol({GetDoubleQuotedStringLiteral(info.Name)}, {schema});");
                            }
                            else
                            {
                                WriteLine($"new CommandSymbol(");
                                WriteNested(() =>
                                {
                                    WriteLine($"{GetDoubleQuotedStringLiteral(info.Name)},");
                                    Write($"{schema}");
                                    if (info.Construction != null)
                                    {
                                        WriteLine(",");
                                        Write($"{GetDoubleQuotedStringLiteral(info.Construction)}");
                                    }
                                    WriteLine(");");
                                });
                            }
                        });

                        WriteLine();
                    }

                    WriteLine("public static readonly IReadOnlyList<CommandSymbol> All = new CommandSymbol[]");
                    WriteNested("{", "};", () =>
                    {
                        for (int i = 0; i < commandInfos.Count; i++)
                        {
                            var info = commandInfos[i];
                            Write($"{LegalName(info.Name)}");
                            if (i < commandInfos.Count - 1)
                                WriteLine(",");
                            else
                                WriteLine();
                        }
                    });
                });

            });

            return _builder.ToString();
        }

        /// <summary>
        /// Builds a map between schema text and variable that will reference that schema
        /// so we don't have duplicate schemas in generated code.
        /// </summary>
        private Dictionary<string, string> BuildDuplicateSchemaMap(IReadOnlyList<CommandInfo> commandInfos)
        {
            var schemas = new HashSet<string>();
            var duplicateSchemas = new Dictionary<string, string>();

            foreach (var info in commandInfos)
            {
                if (schemas.Contains(info.Schema))
                {
                    if (!duplicateSchemas.ContainsKey(info.Schema))
                    {
                        duplicateSchemas.Add(info.Schema, "_schema" + duplicateSchemas.Count);
                    }
                }
                else
                {
                    schemas.Add(info.Schema);
                }
            }

            return duplicateSchemas;
        }

        private static string LegalName(string name)
        {
            if (!IsLegalName(name))
            {
                var builder = new StringBuilder();
                for (int i = 0; i < name.Length; i++)
                {
                    var c = name[i];
                    if ((i == 0 && !IsLegalNameFirstChar(c))
                        || (i > 0 && !IsLegalNameChar(c)))
                    {
                        // replaced illegal chars with _
                        builder.Append('_');
                    }
                    else
                    {
                        builder.Append(c);
                    }
                }

                return builder.ToString();
            }
            else
            {
                return name;
            }
        }

        private static bool IsLegalName(string name) =>
            IsLegalNameFirstChar(name[0])
            && name.All(IsLegalNameChar);

        private static bool IsLegalNameFirstChar(char c) =>
            char.IsLetter(c) || c == '_';

        private static bool IsLegalNameChar(char c) =>
            char.IsLetterOrDigit(c) || c == '_';

        [System.Diagnostics.DebuggerDisplay("{DebugText}")]
        private struct CommandAndGrammar
        {
            public readonly CommandInfo Info;
            public readonly Grammar Grammar;
            public readonly string GrammarName;

            public CommandAndGrammar(CommandInfo info, Grammar grammar, string grammarName)
            {
                this.Info = info;
                this.Grammar = grammar;
                this.GrammarName = grammarName;
            }

            private string DebugText =>
                $"{Info.Name}: {Grammar.ToString()}";
        }

        private List<CommandAndGrammar> ParseCommandGrammars(IReadOnlyList<CommandInfo> commandInfos)
        {
            return commandInfos.Select(info => new CommandAndGrammar(info, ParseCommandGrammar(info.Name, info.Grammar), info.Name))
                .Where(x => x.Grammar != null)
                .ToList();
        }

        private static Grammar ParseCommandGrammar(string commandName, string commandGrammar)
        {
            try
            {
                if (GrammarParser.TryParse(commandGrammar, out var contentGrammar, out var length))
                {
                    if (length != commandGrammar.Length)
                    {
                        throw new InvalidOperationException($"control command grammar {commandName} failed to parse fully at offset ({length}): {commandGrammar}");
                    }

                    return contentGrammar;
                }
                else
                {
                    throw new InvalidOperationException($"control command grammar {commandName} failed to parse");
                }
            }
            finally
            {
            }
        }

        /// <summary>
        /// Writes all common shapes and returns the grammar-to-shape map
        /// </summary>
        private IReadOnlyDictionary<Grammar, ShapeInfo> WriteCommonShapes(
            IReadOnlyList<CommandAndGrammar> commandAndGrammars)
        {
            var builder = new ShapeMapBuilder(_rulesMap);

            foreach (var cag in commandAndGrammars)
            {
                builder.Build(cag.Grammar);
            }

            var shapeMap = builder.ShapeMap;

            // make variables for common shapes and subsitutes the new shapes for the old shapes
            var newShapeMap = new Dictionary<Grammar, ShapeInfo>();
            var seenShapes = new HashSet<ShapeInfo>();
            var updatedShapes = new Dictionary<ShapeInfo, ShapeInfo>();

            foreach (var pair in shapeMap)
            {
                if (seenShapes.Contains(pair.Value))
                {
                    if (!updatedShapes.TryGetValue(pair.Value, out var newShape)
                        && !IsDefault(pair.Value))
                    {
                        var shapeCode = BuildShape(pair.Value);
                        var shapeName = "shape" + (updatedShapes.Count);
                        WriteLine($"var {shapeName} = {shapeCode};");
                        newShape = new ShapeInfo(shapeName);
                        updatedShapes[pair.Value] = newShape;
                    }
                }
                else
                {
                    seenShapes.Add(pair.Value);
                }
            }

            foreach (var pair in shapeMap)
            {
                if (updatedShapes.TryGetValue(pair.Value, out var updatedShape))
                {
                    newShapeMap.Add(pair.Key, updatedShape);
                }
                else
                {
                    newShapeMap.Add(pair.Key, pair.Value);
                }
            }

            if (updatedShapes.Count > 0)
            {
                WriteLine();
            }

            return newShapeMap;
        }

        private IReadOnlyDictionary<Grammar, MissingInfo> WriteCommonMissings(
            IReadOnlyList<CommandAndGrammar> commandAndGrammars,
            IReadOnlyDictionary<Grammar, ShapeInfo> shapeMap)
        {
            var builder = new MissingBuilder(_rulesMap, shapeMap);

            foreach (var cag in commandAndGrammars)
            {
                builder.Build(cag.Grammar);
            }

            var missingMap = builder.GetMissingMap();

            var updatedInfos = new Dictionary<MissingInfo, MissingInfo>();
            var seenMissing = new HashSet<MissingInfo>();

            foreach (var pair in missingMap)
            {
                if (seenMissing.Contains(pair.Value))
                {
                    // seen this one more than once
                    if (!updatedInfos.ContainsKey(pair.Value)
                        && !(pair.Value is FunctionMissingInfo))
                    {
                        var missingExpression = GetMissingFunction(pair.Value);
                        var missingName = "missing" + (updatedInfos.Count);
                        WriteLine($"Func<Source<LexicalToken>, int, SyntaxElement> {missingName} = {missingExpression};");
                        updatedInfos[pair.Value] = new FunctionMissingInfo(missingName);
                    }
                }
                else
                {
                    seenMissing.Add(pair.Value);
                }
            }

            // return a missing map with all the update missing info's 
            var updatedMissingMap = new Dictionary<Grammar, MissingInfo>();

            foreach (var pair in missingMap)
            {
                if (updatedInfos.TryGetValue(pair.Value, out var newValue))
                {
                    updatedMissingMap.Add(pair.Key, newValue);
                }
                else
                {
                    updatedMissingMap.Add(pair.Key, pair.Value);
                }
            }

            if (updatedInfos.Count > 0)
            {
                WriteLine();
            }

            return updatedMissingMap;
        }

        private void WriteCommandGrammarParsers(
            IReadOnlyList<CommandAndGrammar> commandAndGrammars,
            IReadOnlyDictionary<Grammar, ShapeInfo> shapeMap,
            IReadOnlyDictionary<Grammar, MissingInfo> missingMap,
            GrammarAnalysis analysis)
        {
            // look for common grammar sequences throughout all commands and pregenerate them up front.
            var commonGrammars = new Dictionary<Grammar, CommonGrammarInfo>(GrammarEquivalenceComparer.Instance);
            var seenGrammars = new HashSet<Grammar>(GrammarEquivalenceComparer.Instance);
            var commonWriter = new CommandGrammarWriter(this, _rulesMap, shapeMap, missingMap, commonGrammars, analysis);

            // pregenerate common grammar fragments
            foreach (var cag in commandAndGrammars)
            {
                Grammar.Walk(cag.Grammar,
                    fnVisitChildren: CheckForCommonGrammar
                    );
            }

            bool CheckForCommonGrammar(Grammar g)
            {
                // only look for common sequences (maybe more aggresive later)
                if (!(g is SequenceGrammar))
                    return true;

                if (seenGrammars.Contains(g))
                {
                    if (!commonGrammars.ContainsKey(g))
                    {
                        // we've already seen this grammar at least once, so lets write it out
                        var fragmentName = GetUniqueFragmentName(g);
                        Write($"var {fragmentName} = ");
                        WriteNested(() =>
                        {
                            var parserInfo = commonWriter.WriteFragment(g);
                            commonGrammars.Add(g, new CommonGrammarInfo(fragmentName, parserInfo));
                        });
                        WriteLine(";");
                    }

                    return false;
                }
                else
                {
                    seenGrammars.Add(g);
                    return true;
                }
            }

            string GetUniqueFragmentName(Grammar g)
            {
                return "fragment" + commonGrammars.Count;
            }

            if (commonGrammars.Count > 0)
                WriteLine();

            // write out all the command grammars
            foreach (var cag in commandAndGrammars)
            {
                WriteLine($"var {LegalName(cag.GrammarName)} = Command({GetDoubleQuotedStringLiteral(cag.Info.Name)}, ");
                WriteNested(() =>
                {
                    var writer = new CommandGrammarWriter(this, _rulesMap, shapeMap, missingMap, commonGrammars, analysis);
                    writer.WriteCommand(cag.Grammar);
                    WriteLine(");");
                    WriteLine();
                });
            }
        }

        internal class ElementInfo : IEquatable<ElementInfo>
        {
            /// <summary>
            /// The name of the element (NameInParent)
            /// </summary>
            public string Name { get; }

            /// <summary>
            /// Text of CompletionHint value
            /// </summary>
            public string Hint { get; }

            /// <summary>
            /// True if the element is optional
            /// </summary>
            public bool IsOptional { get; }

            public ElementInfo(string name, string hint, bool isOptional = false)
            {
                this.Name = name ?? "";
                this.Hint = hint ?? "Syntax";
                this.IsOptional = isOptional;
            }

            public ElementInfo(string hint, bool isOptional = false)
                : this(null, hint, isOptional)
            {
            }

            public static ElementInfo FromHint(string hint)
            {
                return new ElementInfo(hint);
            }

            public ElementInfo WithName(string name)
            {
                return new ElementInfo(name, this.Hint, this.IsOptional);
            }

            public ElementInfo WithHint(string hint)
            {
                return new ElementInfo(this.Name, hint, this.IsOptional);
            }

            public static readonly ElementInfo Default = new ElementInfo(null, null, false);

            public bool Equals(ElementInfo other)
            {
                return this.Name == other.Name
                    && this.Hint == other.Hint
                    && this.IsOptional == other.IsOptional;
            }

            public override bool Equals(object obj)
            {
                return obj is ElementInfo other
                    && Equals(other);
            }

            public override int GetHashCode()
            {
                return (this.Name?.GetHashCode() ?? 0)
                    + (this.Hint?.GetHashCode() ?? 0)
                    + (this.IsOptional ? 1 : 0);
            }
        }

        internal class ShapeInfo : IEquatable<ShapeInfo>
        {
            public string Name { get; }
            public IReadOnlyList<ElementInfo> Elements { get; }
            private readonly int _hashCode;

            private static readonly IReadOnlyList<ElementInfo> NoElements =
                new List<ElementInfo>().AsReadOnly();

            public ShapeInfo(string name)
            {
                this.Name = name;
                this.Elements = NoElements;
            }

            public ShapeInfo(IReadOnlyList<ElementInfo> elements)
            {
                this.Name = "";
                this.Elements = elements;

                foreach (var e in elements)
                {
                    _hashCode += e.GetHashCode();
                }
            }

            public bool Equals(ShapeInfo other)
            {
                if (this.Name != other.Name)
                    return false;

                if (this.Elements.Count != other.Elements.Count)
                    return false;

                for (int i = 0; i < this.Elements.Count; i++)
                {
                    if (!this.Elements[i].Equals(other.Elements[i]))
                        return false;
                }

                return true;
            }

            public override bool Equals(object obj)
            {
                return obj is ShapeInfo shape
                    && Equals(shape);
            }

            public override int GetHashCode()
            {
                return _hashCode;
            }
        }

        private static string GetDoubleQuotedStringLiteral(string text)
        {
            return $"\"{text}\"";
        }

        internal class ParserInfo
        {
            /// <summary>
            /// The expression that refers to the parser
            /// </summary>
            public string Expression { get; }

            /// <summary>
            /// True if this element is considered a term (default == false)
            /// </summary>
            public bool IsTerm { get; }

            /// <summary>
            /// Creates a <see cref="ParserInfo"/> instance.
            /// </summary>
            public ParserInfo(
                string expression,
                bool isTerm = false)
            {
                this.Expression = expression;
                this.IsTerm = isTerm;
            }

            /// <summary>
            /// Creates a <see cref="ParserInfo"/> instance.
            /// </summary>
            public ParserInfo(
                bool isTerm)
            {
                this.IsTerm = isTerm;
            }

            public static readonly ParserInfo Default = new ParserInfo(false);
        }

        internal class RuleInfo
        {
            public ParserInfo Parser { get; }
            public MissingInfo Missing { get; }
            public ElementInfo Element { get; }

            public RuleInfo(ParserInfo parser, MissingInfo missing, ElementInfo element)
            {
                this.Parser = parser;
                this.Missing = missing;
                this.Element = element;
            }
        }

        /// <summary>
        /// Writes out all common shapes
        /// </summary>
        private class ShapeMapBuilder : GrammarVisitor<ElementInfo>
        {
            private readonly IReadOnlyDictionary<string, RuleInfo> _rulesMap;
            private Dictionary<Grammar, ShapeInfo> _shapeMap;

            public ShapeMapBuilder(
                IReadOnlyDictionary<string, RuleInfo> rulesMap)
            {
                _rulesMap = rulesMap;
                _shapeMap = new Dictionary<Grammar, ShapeInfo>();
            }

            public IReadOnlyDictionary<Grammar, ShapeInfo> ShapeMap => _shapeMap;

            public void Build(Grammar root)
            {
                Visit(root);
            }

            private ElementInfo Visit(Grammar g)
            {
                var element = g.Accept(this);
                return element;
            }

            public override ElementInfo VisitAlternation(AlternationGrammar grammar)
            {
                var infos = grammar.Alternatives.Select(a => Visit(a)).ToList();
                return ElementInfo.FromHint(infos[0].Hint);
            }

            public override ElementInfo VisitHidden(HiddenGrammar grammar)
            {
                return Visit(grammar.Hidden);
            }

            public override ElementInfo VisitOneOrMore(OneOrMoreGrammar grammar)
            {
                var info = Visit(grammar.Repeated);
                return ElementInfo.FromHint(info.Hint);
            }

            public override ElementInfo VisitOptional(OptionalGrammar grammar)
            {
                var info = Visit(grammar.Optioned);
                return new ElementInfo(info.Hint, isOptional: true);
            }

            public override ElementInfo VisitRequired(RequiredGrammar grammar)
            {
                var info = Visit(grammar.Required);
                return ElementInfo.FromHint(info.Hint);
            }

            public override ElementInfo VisitRule(RuleGrammar grammar)
            {
                if (_rulesMap.TryGetValue(grammar.RuleName, out var info))
                {
                    return info.Element;
                }
                else
                {
                    return ElementInfo.Default;
                }
            }

            public override ElementInfo VisitSequence(SequenceGrammar grammar)
            {
                var infos = grammar.Steps.Select(s => Visit(s)).ToList();

                var shape = new ShapeInfo(infos);
                _shapeMap[grammar] = shape;

                return ElementInfo.FromHint(infos[0].Hint);
            }

            public override ElementInfo VisitTagged(TaggedGrammar grammar)
            {
                var info = Visit(grammar.Tagged);
                var namedInfo = info.WithName(grammar.Tag);

                // if tagged elements appear outside a sequence step they 
                // need their own custom node and shape.
                var shape = new ShapeInfo(new[] { namedInfo });
                _shapeMap[grammar] = shape;

                return namedInfo;
            }

            public override ElementInfo VisitToken(TokenGrammar grammar)
            {
                return ElementInfo.Default;
            }

            public override ElementInfo VisitZeroOrMore(ZeroOrMoreGrammar grammar)
            {
                var info = Visit(grammar.Repeated);
                return ElementInfo.FromHint(info.Hint);
            }
        }

        private static string BuildShape(ShapeInfo shape)
        {
            if (!string.IsNullOrEmpty(shape.Name))
            {
                return shape.Name;
            }
            else if (IsDefault(shape))
            {
                return "";
            }
            if (shape.Elements.Count == 1)
            {
                return BuildDescriptor(shape.Elements[0]);
            }
            else
            {
                return $"new [] {{{string.Join(", ", shape.Elements.Select(e => BuildDescriptor(e)))}}}";
            }
        }

        private static bool IsDefault(ShapeInfo shape)
        {
            return shape.Elements.All(e => IsDefaultElement(e));
        }


        private static bool IsDefaultElement(ElementInfo element)
        {
            return element == ElementInfo.Default
                || (string.IsNullOrEmpty(element.Name) && (string.IsNullOrEmpty(element.Hint) || element.Hint == "Syntax") && !element.IsOptional);
        }

        private static string BuildDescriptor(ElementInfo element)
        {
            var text = "CD(";

            if (!string.IsNullOrEmpty(element.Name))
            {
                text += $"{GetDoubleQuotedStringLiteral(element.Name)}";

                if (element.Hint != "Syntax")
                {
                    text += $", CompletionHint.{element.Hint}";
                }

                if (element.IsOptional)
                {
                    text += ", isOptional: true";
                }
            }
            else if (element.Hint != "Syntax")
            {
                text += $"CompletionHint.{element.Hint}";

                if (element.IsOptional)
                {
                    text += ", isOptional: true";
                }
            }
            else if (element.IsOptional)
            {
                text += "isOptional: true";
            }

            text += ")";

            return text;
        }

        internal abstract class MissingInfo : IEquatable<MissingInfo>
        {
            public abstract bool Equals(MissingInfo other);
        };

        /// <summary>
        /// A missing info with an expression that constructs its missing value
        /// </summary>
        internal class ExpressionMissingInfo : MissingInfo
        {
            public string Expression { get; }

            public ExpressionMissingInfo(string code)
            {
                this.Expression = code;
            }

            public override bool Equals(MissingInfo other)
            {
                return other is ExpressionMissingInfo tm
                    && tm.Expression == this.Expression;
            }

            public override bool Equals(object obj)
            {
                return obj is MissingInfo m && Equals(m);
            }

            public override int GetHashCode()
            {
                return this.Expression.GetHashCode();
            }
        }

        /// <summary>
        /// A missing info with a function that needs to be invoked to construct its value
        /// </summary>
        internal class FunctionMissingInfo : MissingInfo
        {
            public string Function { get; }

            public FunctionMissingInfo(string function)
            {
                this.Function = function;
            }

            public override bool Equals(MissingInfo other)
            {
                return other is FunctionMissingInfo fm
                    && fm.Function == this.Function;
            }

            public override bool Equals(object obj)
            {
                return obj is MissingInfo m && Equals(m);
            }

            public override int GetHashCode()
            {
                return this.Function.GetHashCode();
            }
        }

        internal class OneOrMoreMissingInfo : MissingInfo
        {
            public bool IsSeparated { get; }
            public MissingInfo ElementMissing { get; }

            public OneOrMoreMissingInfo(bool isSeparated, MissingInfo elementMissing) 
            {
                this.IsSeparated = isSeparated;
                this.ElementMissing = elementMissing; 
            }

            public override bool Equals(MissingInfo other)
            {
                return other is OneOrMoreMissingInfo oom
                    && this.IsSeparated == oom.IsSeparated
                    && this.ElementMissing.Equals(oom.ElementMissing);
            }

            public override bool Equals(object obj)
            {
                return obj is MissingInfo m && Equals(m);
            }

            public override int GetHashCode()
            {
                return this.ElementMissing.GetHashCode();
            }
        }

        internal class SequenceMissingInfo : MissingInfo
        {
            public ShapeInfo Shape { get; }
            public IReadOnlyList<MissingInfo> StepMissings { get; }
            private readonly int _hashCode;

            public SequenceMissingInfo(ShapeInfo shape, IReadOnlyList<MissingInfo> stepMissings) 
            {
                this.Shape = shape;
                this.StepMissings = stepMissings;

                foreach (var m in stepMissings)
                {
                    _hashCode += m.GetHashCode();
                }
            }

            public override bool Equals(MissingInfo other)
            {
                if (!(other is SequenceMissingInfo ms && ms.Shape == this.Shape && ms.StepMissings.Count == this.StepMissings.Count))
                    return false;

                for (int i = 0; i < this.StepMissings.Count; i++)
                {
                    if (!this.StepMissings[i].Equals(ms.StepMissings[i]))
                        return false;
                }

                return true;
            }

            public override int GetHashCode()
            {
                return _hashCode;
            }
        }

        private static string GetMissingFunction(MissingInfo missing)
        {
            if (missing is FunctionMissingInfo fm)
            {
                return fm.Function;
            }
            else
            {
                return $"(source, start) => {GetMissingExpression(missing)}";
            }
        }

        private static string GetMissingExpression(MissingInfo missing)
        {
            if (missing is ExpressionMissingInfo em)
            {
                return em.Expression;
            }
            else
            {
                var builder = new StringBuilder();
                BuildMissingExpression(builder, missing);
                return builder.ToString();
            }
        }

        private static void BuildMissingExpression(StringBuilder builder, MissingInfo missing)
        {
            switch (missing)
            {
                case ExpressionMissingInfo tm:
                    builder.Append(tm.Expression);
                    break;
                case FunctionMissingInfo fm:
                    builder.Append($"{fm.Function}(source, start)");
                    break;
                case OneOrMoreMissingInfo oom:
                    if (oom.IsSeparated)
                    {
                        builder.Append("new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(");
                        BuildMissingExpression(builder, oom.ElementMissing);
                        builder.Append("))");
                    }
                    else
                    {
                        builder.Append($"new SyntaxList<SyntaxElement>(");
                        BuildMissingExpression(builder, oom.ElementMissing);
                        builder.Append(")");
                    }
                    break;
                case SequenceMissingInfo sm:
                    builder.Append(("(SyntaxElement)new CustomNode("));

                    if (sm.Shape != null)
                    {
                        var shapeCode = BuildShape(sm.Shape);
                        if (!string.IsNullOrEmpty(shapeCode))
                        {
                            builder.Append($"{shapeCode}, ");
                        }
                    }

                    for (int i = 0; i < sm.StepMissings.Count; i++)
                    {
                        BuildMissingExpression(builder, sm.StepMissings[i]);
                        if (i < sm.StepMissings.Count - 1)
                            builder.Append(", ");
                    }
                    builder.Append(")");
                    break;
                default:
                    builder.Append("UNKNOWN_MISSING");
                    break;
            }
        }

        /// <summary>
        /// Builds missing info
        /// </summary>
        private class MissingBuilder : GrammarVisitor<MissingInfo>
        {
            private readonly IReadOnlyDictionary<string, RuleInfo> _rulesMap;
            private readonly IReadOnlyDictionary<Grammar, ShapeInfo> _shapeMap;
            private readonly HashSet<MissingInfo> _usedMissings;
            private readonly Dictionary<Grammar, MissingInfo> _missingMap;

            public MissingBuilder(
                IReadOnlyDictionary<string, RuleInfo> rulesMap,
                IReadOnlyDictionary<Grammar, ShapeInfo> shapeMap)
            {
                _rulesMap = rulesMap;
                _shapeMap = shapeMap;
                _usedMissings = new HashSet<MissingInfo>();
                _missingMap = new Dictionary<Grammar, MissingInfo>();
            }

            public void Build(Grammar root)
            {
                Visit(root);
            }

            public IReadOnlyDictionary<Grammar, MissingInfo> GetMissingMap()
            {
                // remove missings that are not used..
                return _missingMap
                    .Where(pair => _usedMissings.Contains(pair.Value))
                    .ToDictionary(
                        pair => pair.Key,
                        pair => pair.Value);
            }

            private MissingInfo Visit(Grammar grammar)
            {
                var missing = grammar.Accept(this);
                _missingMap[grammar] = missing;
                return missing;
            }

            public override MissingInfo VisitAlternation(AlternationGrammar grammar)
            {
                var missings = grammar.Alternatives.Select(a => Visit(a)).ToList();

                if (grammar.Alternatives.All(a => a is TokenGrammar))
                {
                    // these are all just tokens, so make a special missing that lists them all 
                    var tokenList = string.Join(", ", grammar.Alternatives.Select(t => GetDoubleQuotedStringLiteral(((TokenGrammar)t).TokenText)));
                    return new ExpressionMissingInfo($"CreateMissingToken({tokenList})");
                }
                else
                {
                    // use the first one
                    return missings[0];
                }
            }

            public override MissingInfo VisitHidden(HiddenGrammar grammar)
            {
                return Visit(grammar.Hidden);
            }

            public override MissingInfo VisitOneOrMore(OneOrMoreGrammar grammar)
            {
                var repeatedMissing = Visit(grammar.Repeated);
                return new OneOrMoreMissingInfo(grammar.Separator != null, repeatedMissing);
            }

            public override MissingInfo VisitOptional(OptionalGrammar grammar)
            {
                return Visit(grammar.Optioned);
            }

            public override MissingInfo VisitRequired(RequiredGrammar grammar)
            {
                // this is the place where missing values are actually used
                var missing = Visit(grammar.Required);

                if (RequiresMissingValueFunctionArgument(grammar))
                {
                    MarkAsUsed(missing);
                }

                return missing;
            }

            private static bool RequiresMissingValueFunctionArgument(RequiredGrammar g)
            {
                // token grammar and alternation of token grammars will use RequiredToken(xxx) API
                // which does not need a missing value to be specified,
                // all others translate to using Required(xxx, value) which does require a missing value function.
                return !(g.Required is TokenGrammar)
                    && !(g.Required is AlternationGrammar ag && ag.Alternatives.All(a => a is TokenGrammar))
                    && !(g.Required is ZeroOrMoreGrammar); // not actually required
            }

            private void MarkAsUsed(MissingInfo missing)
            {
                if (!_usedMissings.Contains(missing))
                {
                    _usedMissings.Add(missing);
                }
            }

            public override MissingInfo VisitRule(RuleGrammar grammar)
            {
                if (_rulesMap.TryGetValue(grammar.RuleName, out var info))
                {
                    return info.Missing;
                }
                else
                {
                    return new ExpressionMissingInfo("(SyntaxElement)null");
                }
            }

            public override MissingInfo VisitSequence(SequenceGrammar grammar)
            {
                var missings = grammar.Steps.Select(s => Visit(s)).ToList();

                _shapeMap.TryGetValue(grammar, out var shape);

                return new SequenceMissingInfo(shape, missings);
            }

            public override MissingInfo VisitTagged(TaggedGrammar grammar)
            {
                return Visit(grammar.Tagged);
            }

            public override MissingInfo VisitToken(TokenGrammar grammar)
            {
                return new ExpressionMissingInfo($"CreateMissingToken({GetDoubleQuotedStringLiteral(grammar.TokenText)})");
            }

            public override MissingInfo VisitZeroOrMore(ZeroOrMoreGrammar grammar)
            {
                var _ = Visit(grammar.Repeated);

                if (grammar.Separator == null)
                {
                    return new ExpressionMissingInfo("SyntaxList<SyntaxElement>.Empty()");
                }
                else
                {
                    return new ExpressionMissingInfo("SyntaxList<SeparatedElement<SyntaxElement>>.Empty()");
                }
            }
        }

        private class CommonGrammarInfo
        {
            public string Name { get; }
            public ParserInfo ParserInfo { get; }

            public CommonGrammarInfo(string name, ParserInfo info)
            {
                this.Name = name;
                this.ParserInfo = info;
            }
        }

        private class CommandGrammarWriter : GrammarVisitor<ParserInfo>
        {
            private readonly CommandGenerator _generator;
            private readonly IReadOnlyDictionary<string, RuleInfo> _rulesMap;
            private readonly IReadOnlyDictionary<Grammar, ShapeInfo> _shapeMap;
            private readonly IReadOnlyDictionary<Grammar, MissingInfo> _missingMap;
            private readonly IReadOnlyDictionary<Grammar, CommonGrammarInfo> _commonGrammarMap;
            private readonly GrammarAnalysis _analysis;

            public CommandGrammarWriter(
                CommandGenerator generator,
                IReadOnlyDictionary<string, RuleInfo> rulesMap,
                IReadOnlyDictionary<Grammar, ShapeInfo> shapeMap,
                IReadOnlyDictionary<Grammar, MissingInfo> missingMap,
                IReadOnlyDictionary<Grammar, CommonGrammarInfo> commonGrammarMap,
                GrammarAnalysis analysis)
            {
                _generator = generator;
                _rulesMap = rulesMap;
                _shapeMap = shapeMap;
                _missingMap = missingMap;
                _commonGrammarMap = commonGrammarMap;
                _analysis = analysis;
            }

            private Grammar _first;

            public void WriteCommand(Grammar commandRoot)
            {
                if (commandRoot is SequenceGrammar seq)
                {
                    _first = seq.Steps[0];
                }

                commandRoot.Accept(this);
            }

            public ParserInfo WriteFragment(Grammar fragmentRoot)
            {
                _first = null;
                return fragmentRoot.Accept(this);
            }

            private bool TryWriteCommonGrammar(Grammar grammar, out ParserInfo info)
            {
                if (_commonGrammarMap != null && _commonGrammarMap.TryGetValue(grammar, out var commonInfo))
                {
                    _generator.Write(commonInfo.Name);
                    info = commonInfo.ParserInfo;
                    return true;
                }

                info = null;
                return false;
            }

            private int depth;

            private ParserInfo Visit(Grammar grammar)
            {
                if (TryWriteCommonGrammar(grammar, out var info))
                {
                    return info;
                }
                else
                {
                    depth++;

                    if (depth > 500)
                        throw new InvalidOperationException("Depth exceededD");

                    var result = grammar.Accept(this);

                    depth--;
                    return result;
                }
            }

            public override ParserInfo VisitAlternation(AlternationGrammar grammar)
            {
                if (grammar.Alternatives.All(a => a is TokenGrammar))
                {
                    var tokens = grammar.Alternatives.OfType<TokenGrammar>().ToList();
                    return WriteTokens(tokens, isRequired: false);
                }
                else
                {
                    List<ParserInfo> infos = null;

                    //_generator.WriteLine("First(");
                    _generator.WriteLine("Best(");
                    _generator.WriteNested(() =>
                    {
                        infos = new List<ParserInfo>();

                        for (int i = 0; i < grammar.Alternatives.Count; i++)
                        {
                            var elem = grammar.Alternatives[i];
                            var info = WriteElementParser(elem);
                            infos.Add(info);

                            if (i < grammar.Alternatives.Count - 1 && grammar.Alternatives.Count > 1)
                            {
                                _generator.WriteLine(",");
                            }
                            else
                            {
                                _generator.Write(")");
                            }
                        }
                    });

                    return new ParserInfo(infos[0].IsTerm);
                }
            }

            public override ParserInfo VisitOneOrMore(OneOrMoreGrammar grammar)
            {
                ParserInfo elem = null;

                if (grammar.Separator is TokenGrammar tk && tk.TokenText == ",")
                {
                    _generator.WriteLine("OneOrMoreCommaList(");
                    _generator.WriteNested(() =>
                    {
                        elem = WriteElementParser(grammar.Repeated);

                        _missingMap.TryGetValue(grammar.Repeated, out var missing);
                        if (missing != null)
                        {
                            var missingFunction = GetMissingFunction(missing);
                            _generator.WriteLine(",");
                            _generator.Write($"fnMissingElement: {missingFunction}");
                        }

                        if (grammar.AllowTrailingSeparator)
                        {
                            _generator.WriteLine(",");
                            _generator.Write("allowTrailingComma: true");
                        }

                        _generator.Write(")");
                    });
                }
                else
                {
                    _generator.WriteLine("OneOrMoreList(");
                    _generator.WriteNested(() =>
                    {
                        elem = WriteElementParser(grammar.Repeated);

                        if (grammar.Separator != null)
                        {
                            _generator.WriteLine(",");
                            _generator.Write("separatorParser: ");
                            var sep = Visit(grammar.Separator);

                            // only supply missing element if there is a separator, as this is only used after a separator
                            // and we actually want this rule to fail for commands if there is no first element
                            _missingMap.TryGetValue(grammar.Repeated, out var missing);
                            if (missing != null)
                            {
                                var missingFunction = GetMissingFunction(missing);
                                _generator.WriteLine(",");
                                _generator.WriteLine($"fnMissingElement: {missingFunction}");
                            }
                        }

                        _generator.Write(")");
                    });
                }

                return ParserInfo.Default;
            }

            public override ParserInfo VisitOptional(OptionalGrammar grammar)
            {
                ParserInfo elem = null;

                if (grammar.Optioned is TokenGrammar
                    || grammar.Optioned is RuleGrammar)
                {
                    _generator.Write("Optional(");
                    elem = WriteElementParser(grammar.Optioned);
                    _generator.Write(")");
                }
                else
                {
                    _generator.WriteLine("Optional(");
                    _generator.WriteNested(() =>
                    {
                        elem = WriteElementParser(grammar.Optioned);
                        _generator.Write(")");
                    });
                }

                return ParserInfo.Default;
            }

            public override ParserInfo VisitRequired(RequiredGrammar grammar)
            {
                ParserInfo elem = null;

                if (grammar.Required is TokenGrammar tk)
                {
                    return WriteToken(tk, isRequired: true);
                }
                //else if (grammar.Required is OneOrMoreGrammar oom)
                //{
                //    return oom.Accept(this);
                //}
                else if (grammar.Required is ZeroOrMoreGrammar zom)
                {
                    return Visit(zom);
                }
                else if (grammar.Required is AlternationGrammar alt && alt.Alternatives.All(a => a is TokenGrammar))
                {
                    var tokens = alt.Alternatives.Cast<TokenGrammar>().ToList();
                    return WriteTokens(tokens, isRequired: true);
                }
                else
                {
                    _missingMap.TryGetValue(grammar.Required, out var missing);
                    var missingFunction = GetMissingFunction(missing);

                    if (grammar.Required is RuleGrammar rg)
                    {
                        _generator.Write("Required(");
                        elem = WriteElementParser(grammar.Required);
                        _generator.Write(", ");
                        _generator.Write($"{missingFunction})");
                    }
                    else
                    {
                        _generator.WriteLine("Required(");
                        _generator.WriteNested(() =>
                        {
                            elem = WriteElementParser(grammar.Required);
                            _generator.WriteLine(",");
                            _generator.Write($"{missingFunction})");
                        });
                    }
                }

                return ParserInfo.Default;
            }

            public override ParserInfo VisitRule(RuleGrammar grammar)
            {
                if (_rulesMap.TryGetValue(grammar.RuleName, out var info))
                {
                    // if this rule is a name-like rule, then return a parser that 
                    // ignores all alterative tokens that can appear at the same lexical position.
                    if (IsNameLike(grammar.RuleName))
                    {
                        var altTerms = _analysis.GetAlternativeTerms(grammar);
                        if (altTerms.Count > 1) // do not count this grammar
                        {
                            var altTokens = altTerms
                                .OfType<TokenGrammar>()
                                .Select(g => g.TokenText)
                                .Distinct()
                                .ToList();

                            if (altTokens.Count > 0)
                            {
                                var ignoreRules = GetIgnoreTokenRules(altTokens);
                                _generator.Write($"If({ignoreRules}, {info.Parser.Expression})");
                                return info.Parser;
                            }
                        }
                    }

                    _generator.Write(info.Parser.Expression);
                }

                return info.Parser;
            }

            private static string GetIgnoreTokenRules(IReadOnlyList<string> tokens)
            {
                if (tokens.Count == 1)
                {
                    return $"Not(Token({GetDoubleQuotedStringLiteral(tokens[0])}))";
                }
                else
                {
                    return $"Not(And(Token({string.Join(", ", tokens.Select(t => GetDoubleQuotedStringLiteral(t)))})))";
                }
            }

            public override ParserInfo VisitSequence(SequenceGrammar grammar)
            {
                List<ParserInfo> infos = null;
                ShapeInfo shape = null;

                if (grammar.Steps.Count == 1)
                {
                    return Visit(grammar.Steps[0]);
                }
                else if (grammar.Steps.Count < 10)
                {
                    infos = new List<ParserInfo>();

                    _generator.WriteLine("Custom(");
                    _generator.WriteNested(() =>
                    {
                        for (int i = 0; i < grammar.Steps.Count; i++)
                        {
                            var step = grammar.Steps[i];
                            var stepInfo = Visit(step);
                            infos.Add(stepInfo);

                            if (i < grammar.Steps.Count - 1)
                                _generator.WriteLine(",");
                        }

                        if (_shapeMap.TryGetValue(grammar, out shape))
                        {
                            var shapeCode = BuildShape(shape);
                            if (!string.IsNullOrEmpty(shapeCode))
                            {
                                _generator.WriteLine(",");
                                _generator.Write(shapeCode);
                            }
                        }

                        _generator.Write(")");
                    });
                }
                else
                {
                    infos = new List<ParserInfo>();

                    _generator.WriteLine("Custom(");
                    _generator.WriteNested(() =>
                    {
                        _generator.WriteLine("new Parser<LexicalToken>[] {");
                        _generator.WriteNested(() =>
                        {
                            for (int i = 0; i < grammar.Steps.Count; i++)
                            {
                                var step = grammar.Steps[i];
                                var stepInfo = Visit(step);
                                infos.Add(stepInfo);
                                if (i < grammar.Steps.Count - 1)
                                    _generator.WriteLine(",");
                            }

                            _generator.WriteLine("}");
                        });

                        if (_shapeMap.TryGetValue(grammar, out shape))
                        {
                            var shapeCode = BuildShape(shape);
                            if (!string.IsNullOrEmpty(shapeCode))
                            {
                                _generator.WriteLine(",");
                                _generator.Write(shapeCode);
                            }
                        }

                        _generator.Write(")");
                    });
                }

                return ParserInfo.Default;
            }

            public override ParserInfo VisitTagged(TaggedGrammar grammar)
            {
                var info = Visit(grammar.Tagged);
                return info;
            }

            public override ParserInfo VisitHidden(HiddenGrammar grammar)
            {
                var info = Visit(grammar.Hidden);
                _generator.Write(".Hide()");
                return info;
            }

            public override ParserInfo VisitToken(TokenGrammar grammar)
            {
                return WriteToken(grammar, isRequired: false);
            }

            private ParserInfo WriteToken(TokenGrammar grammar, bool isRequired)
            {
                var token = GetDoubleQuotedStringLiteral(grammar.TokenText);
                var ckind = GetCompletionKind(grammar);

                if (isRequired)
                {
                    if (ckind != null)
                    {
                        _generator.Write($"RequiredToken({token}, {ckind})");
                    }
                    else
                    {
                        _generator.Write($"RequiredToken({token})");
                    }
                }
                else
                {
                    if (ckind != null)
                    {
                        _generator.Write($"Token({token}, {ckind})");
                    }
                    else
                    {
                        _generator.Write($"Token({token})");
                    }
                }

                return new ParserInfo(isTerm: true);
            }

            private ParserInfo WriteTokens(IReadOnlyList<TokenGrammar> tokens, bool isRequired)
            {
                var tokenList = string.Join(", ", tokens.Select(t => GetDoubleQuotedStringLiteral(t.TokenText)));
                var missing = $"CreateMissingToken({tokenList})";

                if (isRequired)
                {
                    _generator.Write($"RequiredToken({string.Join(", ", tokens.Select(t => GetDoubleQuotedStringLiteral(t.TokenText)))})");

                    return new ParserInfo(
                        missing,
                        isTerm: true);
                }
                else
                {
                    _generator.Write($"Token({string.Join(", ", tokens.Select(t => GetDoubleQuotedStringLiteral(t.TokenText)))})");

                    return new ParserInfo(isTerm: true);
                }
            }

            private string GetCompletionKind(TokenGrammar token)
            {
                // the first term is always a command prefix
                if (token == _first)
                {
                    return "CompletionKind.CommandPrefix";
                }
                // otherwise its based on the text
                {
                    return null;
                }
            }

            public override ParserInfo VisitZeroOrMore(ZeroOrMoreGrammar grammar)
            {
                ParserInfo elem = null;

                if (grammar.Separator is TokenGrammar tk && tk.TokenText == ",")
                {
                    _generator.WriteLine("ZeroOrMoreCommaList(");
                    _generator.WriteNested(() =>
                    {
                        elem = WriteElementParser(grammar.Repeated);

                        if (_missingMap.TryGetValue(grammar.Repeated, out var missing))
                        {
                            _generator.WriteLine(",");
                            var missingFunction = GetMissingFunction(missing);
                            _generator.WriteLine($"fnMissingElement: {missingFunction}");
                        }

                        if (grammar.AllowTrailingSeparator)
                        {
                            _generator.WriteLine(",");
                            _generator.Write("allowTrailingComma: true");
                        }

                        _generator.Write(")");
                    });
                }
                else
                {
                    _generator.WriteLine("ZeroOrMoreList(");
                    _generator.WriteNested(() =>
                    {
                        elem = WriteElementParser(grammar.Repeated);

                        if (grammar.Separator != null)
                        {
                            _generator.WriteLine(",");
                            _generator.Write("separatorParser: ");
                            var sep = Visit(grammar.Separator);

                            // only supply missing element if separators are a thing, as missing elements only apply
                            // to elements after a separator
                            if (_missingMap.TryGetValue(grammar.Repeated, out var missing))
                            {
                                _generator.WriteLine(",");
                                var missingFunction = GetMissingFunction(missing);
                                _generator.WriteLine($"fnMissingElement: {missingFunction}");
                            }
                        }

                        if (grammar.AllowTrailingSeparator)
                        {
                            _generator.WriteLine(",");
                            _generator.WriteLine("allowTrailingSeparator: true");
                        }

                        _generator.Write(")");
                    });
                }

                return ParserInfo.Default;
            }

            private ParserInfo WriteElementParser(Grammar grammar)
            {
                if (TryWriteCommonGrammar(grammar, out var commonParserInfo))
                {
                    return commonParserInfo;
                }
                else if (grammar is TaggedGrammar tg)
                {
                    ParserInfo info = null;

                    _generator.WriteLine("Custom(");
                    _generator.WriteNested(() =>
                    {
                        info = Visit(grammar);

                        if (_shapeMap.TryGetValue(tg, out var shape))
                        {
                            var shapeCode = BuildShape(shape);
                            if (!string.IsNullOrEmpty(shapeCode))
                            {
                                _generator.WriteLine(",");
                                _generator.Write($"{shapeCode})");
                            }
                            else
                            {
                                _generator.Write(")");
                            }
                        }
                        else
                        {
                            _generator.Write(")");
                        }
                    });

                    return info;
                }
                else
                {
                    return Visit(grammar);
                }
            }
        }

        private static IReadOnlyDictionary<string, RuleInfo> CreateRulesMap()
        {
            // values
            var KustoValueInfo =
                new RuleInfo(
                    new ParserInfo("rules.Value"),
                    new FunctionMissingInfo("rules.MissingValue"),
                    new ElementInfo(hint: "Literal"));

            var KustoStringLiteralInfo =
                new RuleInfo(
                    new ParserInfo("rules.StringLiteral"),
                    new FunctionMissingInfo("rules.MissingStringLiteral"),
                    new ElementInfo(hint: "Literal"));

            var KustoBracketedStringLiteralInfo =
                new RuleInfo(
                    new ParserInfo("rules.BracketedStringLiteral"),
                    new FunctionMissingInfo("rules.MissingStringLiteral"),
                    new ElementInfo(hint: "Literal"));

            var KustoGuidLiteralInfo =
                new RuleInfo(
                    new ParserInfo("rules.AnyGuidLiteralOrString"),
                    new FunctionMissingInfo("rules.MissingValue"),
                    new ElementInfo(hint: "Literal"));

            var KustoTypeInfo =
                new RuleInfo(
                    new ParserInfo("rules.Type"),
                    new FunctionMissingInfo("rules.MissingType"),
                    new ElementInfo(hint: "Syntax"));

            // name declarations

            var KustoNameDeclarationInfo =
                new RuleInfo(
                    new ParserInfo("rules.NameDeclaration"),
                    new FunctionMissingInfo("rules.MissingNameDeclaration"),
                    new ElementInfo(hint: "None"));

            var KustoQualifiedNameDeclarationInfo =
                new RuleInfo(
                    new ParserInfo("rules.QualifiedNameDeclaration"),
                    new FunctionMissingInfo("rules.MissingNameDeclaration"),
                    new ElementInfo(hint: "None"));

            var KustoWildcardedNameDeclarationInfo =
                new RuleInfo(
                    new ParserInfo("rules.WildcardedNameDeclaration"),
                    new FunctionMissingInfo("rules.MissingNameDeclaration"),
                    new ElementInfo(hint: "None"));

            var KustoQualifiedWildcardedNameDeclarationInfo =
                new RuleInfo(
                    new ParserInfo("rules.QualifiedWildcardedNameDeclaration"),
                    new FunctionMissingInfo("rules.MissingNameDeclaration"),
                    new ElementInfo(hint: "None"));


            // name references

            var KustoColumnNameInfo =
                new RuleInfo(
                    new ParserInfo("rules.ColumnNameReference"),
                    new FunctionMissingInfo("rules.MissingNameReference"),
                    new ElementInfo(hint: "Column"));

            var KustoTableNameInfo =
                new RuleInfo(
                    new ParserInfo("rules.TableNameReference"),
                    new FunctionMissingInfo("rules.MissingNameReference"),
                    new ElementInfo(hint: "Table"));

            var KustoExternalTableNameInfo =
                new RuleInfo(
                    new ParserInfo("rules.ExternalTableNameReference"),
                    new FunctionMissingInfo("rules.MissingNameReference"),
                    new ElementInfo(hint: "ExternalTable"));

            var KustoMaterializedViewNameInfo =
                new RuleInfo(
                    new ParserInfo("rules.MaterializedViewNameReference"),
                    new FunctionMissingInfo("rules.MissingNameReference"),
                    new ElementInfo(hint: "MaterializedView"));

            var KustoFunctionNameInfo =
                new RuleInfo(
                    new ParserInfo("rules.FunctionNameReference"),
                    new FunctionMissingInfo("rules.MissingNameReference"),
                    new ElementInfo(hint: "Function"));

            var EntityGroupsNameInfo =
                new RuleInfo(
                    new ParserInfo("rules.EntityGroupNameReference"),
                    new FunctionMissingInfo("rules.MissingNameReference"),
                    new ElementInfo(hint: "EntityGroup"));

            var KustoDatabaseNameInfo =
                new RuleInfo(
                    new ParserInfo("rules.DatabaseNameReference"),
                    new FunctionMissingInfo("rules.MissingNameReference"),
                    new ElementInfo(hint: "Database"));

            var KustoClusterNameInfo =
                new RuleInfo(
                    new ParserInfo("rules.ClusterNameReference"),
                    new FunctionMissingInfo("rules.MissingNameReference"),
                    new ElementInfo(hint: "Cluster"));

            var KustoGraphModelNameInfo =
                new RuleInfo(
                    new ParserInfo("rules.GraphModelNameReference"),
                    new FunctionMissingInfo("rules.MissingNameReference"),
                    new ElementInfo(hint: "GraphModel"));

            var KustoGraphModelSnapshotNameInfo =
                new RuleInfo(
                    new ParserInfo("rules.GraphModelSnapshotNameReference"),
                    new FunctionMissingInfo("rules.MissingNameReference"),
                    new ElementInfo(hint: "GraphModel"));

            // dotted name references

            var KustoDatabaseTableNameInfo =
                new RuleInfo(
                    new ParserInfo("rules.DatabaseTableNameReference"),
                    new FunctionMissingInfo("rules.MissingNameReference"),
                    new ElementInfo(hint: "Table"));

            var KustoDatabaseTableColumnNameInfo =
                new RuleInfo(
                    new ParserInfo("rules.DatabaseTableColumnNameReference"),
                    new FunctionMissingInfo("rules.MissingNameReference"),
                    new ElementInfo(hint: "Column"));

            var KustoTableColumnNameInfo =
                new RuleInfo(
                    new ParserInfo("rules.TableColumnNameReference"),
                    new FunctionMissingInfo("rules.MissingNameReference"),
                    new ElementInfo(hint: "Column"));

            var KustoDatabaseExternalTableNameInfo =
                new RuleInfo(
                    new ParserInfo("rules.DatabaseExternalTableNameReference"),
                    new FunctionMissingInfo("rules.MissingNameReference"),
                    new ElementInfo(hint: "ExternalTable"));

            var KustoDatabaseMaterializedViewNameInfo =
                new RuleInfo(
                    new ParserInfo("rules.DatabaseMaterializedViewNameReference"),
                    new FunctionMissingInfo("rules.MissingNameReference"),
                    new ElementInfo(hint: "MaterializedView"));

            var KustoDatabaseFunctionNameInfo =
                new RuleInfo(
                    new ParserInfo("rules.DatabaseFunctionNameReference"),
                    new FunctionMissingInfo("rules.MissingNameReference"),
                    new ElementInfo(hint: "Function"));

            var KustoDatabaseEntityGroupNameInfo =
                new RuleInfo(
                    new ParserInfo("rules.DatabaseEntityGroupNameReference"),
                    new FunctionMissingInfo("rules.MissingNameReference"),
                    new ElementInfo(hint: "EntityGroup"));

            // other syntax
            var KustoFunctionDeclarationInfo =
                new RuleInfo(
                    new ParserInfo("rules.FunctionDeclaration"),
                    new FunctionMissingInfo("rules.MissingFunctionDeclaration"),
                    new ElementInfo(hint: "Syntax"));

            var KustoFunctionBody =
                new RuleInfo(
                    new ParserInfo("rules.FunctionBody"),
                    new FunctionMissingInfo("rules.MissingFunctionBody"),
                    new ElementInfo(hint: "Syntax"));

            var KustoQueryInputInfo =
                new RuleInfo(
                    new ParserInfo("rules.QueryInput"),
                    new FunctionMissingInfo("rules.MissingExpression"),
                    new ElementInfo(hint: "NonScalar"));

            var KustoScriptInputInfo =
                new RuleInfo(
                    new ParserInfo("rules.ScriptInput"),
                    new FunctionMissingInfo("rules.MissingStatement"),
                    new ElementInfo(hint: "NonScalar"));

            var KustoInputText =
                new RuleInfo(
                    new ParserInfo("rules.InputText"),
                    new FunctionMissingInfo("rules.MissingInputText"),
                    new ElementInfo(hint: "None"));

            var KustoBracketedInputText =
                new RuleInfo(
                    new ParserInfo("rules.BracketedInputText"),
                    new FunctionMissingInfo("rules.MissingInputText"),
                    new ElementInfo(hint: "None"));

            return new Dictionary<string, RuleInfo>()
                {
                    { "value", KustoValueInfo },
                    { "timespan", KustoValueInfo },
                    { "datetime", KustoValueInfo },
                    { "string", KustoStringLiteralInfo },
                    { "bracketed_string", KustoBracketedStringLiteralInfo },
                    { "bool", KustoValueInfo },
                    { "long", KustoValueInfo },
                    { "int", KustoValueInfo },
                    { "decimal", KustoValueInfo },
                    { "real", KustoValueInfo },
                    { "type", KustoTypeInfo },
                    { "guid", KustoGuidLiteralInfo },

                    { "name", KustoNameDeclarationInfo },
                    { "qualified_name", KustoQualifiedNameDeclarationInfo },
                    { "wildcarded_name", KustoWildcardedNameDeclarationInfo },
                    { "qualified_wildcarded_name", KustoQualifiedWildcardedNameDeclarationInfo },

                    { "column", KustoColumnNameInfo },
                    { "table", KustoTableNameInfo },
                    { "externaltable", KustoExternalTableNameInfo },
                    { "materializedview", KustoMaterializedViewNameInfo },
                    { "function", KustoFunctionNameInfo },
                    { "entitygroup", EntityGroupsNameInfo },
                    { "database", KustoDatabaseNameInfo },
                    { "cluster", KustoClusterNameInfo },
                    { "graph_model", KustoGraphModelNameInfo },
                    { "graph_model_snapshot", KustoGraphModelSnapshotNameInfo },

                    { "table_column", KustoTableColumnNameInfo },
                    { "database_table", KustoDatabaseTableNameInfo },
                    { "database_table_column", KustoDatabaseTableColumnNameInfo },
                    { "database_externaltable", KustoDatabaseExternalTableNameInfo },
                    { "database_materializedview", KustoDatabaseMaterializedViewNameInfo },
                    { "database_function", KustoDatabaseFunctionNameInfo },
                    { "database_entitygroup", KustoDatabaseEntityGroupNameInfo },

                    { "function_declaration", KustoFunctionDeclarationInfo },
                    { "function_body", KustoFunctionBody },
                    { "input_query", KustoQueryInputInfo },
                    { "input_script", KustoScriptInputInfo },
                    { "input_data", KustoInputText },
                    { "bracketed_input_data", KustoBracketedInputText }
                };
        }

        private static bool IsNameLike(string ruleName)
        {
            switch (ruleName)
            {
                case "name":
                case "wildcarded_name":
                case "qualified_wildcarded_name":
                case "column":
                case "table":
                case "externaltable":
                case "materializedview":
                case "function":
                case "entitygroup":
                case "database":
                case "cluster":
                case "graph_model":
                case "graph_model_snapshot":
                case "database_table":
                case "database_table_column":
                case "database_externaltable":
                case "database_materializedview":
                case "database_function":
                case "database_entitygroup":
                case "table_column":
                    return true;
                default:
                    return false;
            }
        }

        private void Write(string text)
        {
            if (_isLineStart)
            {
                _builder.Append(_lineStartIndentation);
                _isLineStart = false;
            }

            _builder.Append(text);
        }

        private void WriteLine(string text = null)
        {
            if (text != null)
                Write(text);
            _builder.AppendLine();
            _isLineStart = true;
        }

        private void WriteNested(Action action)
        {
            var oldLineStartIndentation = _lineStartIndentation;
            _lineStartIndentation = _lineStartIndentation + _indent;
            action();
            _lineStartIndentation = oldLineStartIndentation;
        }

        private void WriteNested(string open, string close, Action action)
        {
            if (!_isLineStart)
                WriteLine();
            WriteLine(open);
            WriteNested(action);
            WriteLine(close);
        }

        private void WriteBraceNested(Action action)
        {
            WriteNested("{", "}", action);
        }
    }

#if !T4
}
#endif
// #>