using System;
using System.Linq;
using System.Collections.Generic;

namespace Kusto.Language.Parsing
{
    public static class PartialParser
    {
        /// <summary>
        /// Parses the input into one or more output values that either
        /// are the expected output or the parts of the expected output that succeeded parsing.
        /// </summary>
        public static int ParsePartial<TInput>(
            this Parser<TInput> parser, Source<TInput> input, int inputStart, List<object> output, int outputStart)
        {
            return Partial<TInput>.Parse(parser, input, inputStart, output, outputStart);
        }

        /// <summary>
        /// Returns the number of input item that would be consumed by <see cref="ParsePartial"/>
        /// </summary>
        public static int PartialScan<TInput>(
            this Parser<TInput> parser, Source<TInput> input, int inputStart)
        {
            return Partial<TInput>.Scan(parser, input, inputStart);
        }

        private static class Partial<TInput>
        {
            public static int Scan(Parser<TInput> parser, Source<TInput> input, int inputStart)
            {
                var pathFinder = new PathFinder(input, inputStart);
                var path = pathFinder.FindPath(parser);
                return path.StepCount > 0 ? path.InputLength : -1;
            }

            public static int Parse(Parser<TInput> parser, Source<TInput> input, int inputStart, List<object> output, int outputStart)
            {
                var pathFinder = new PathFinder(input, inputStart);
                var path = pathFinder.FindPath(parser);

                if (path.StepCount == 0)
                    return -1;

                var inputPosition = inputStart;
                var steps = path.GetSteps();
                foreach (var step in steps)
                {
                    for (int i = 0; i < step.Repeat; i++)
                    {
                        var len = step.Parser.Parse(input, inputPosition, output, output.Count);
                        inputPosition += len;
                    }
                }

                return inputPosition - inputStart;
            }

            public class Path
            {
                /// <summary>
                /// The last step in the path.
                /// </summary>
                public readonly Step Step;

                /// <summary>
                /// The total number of steps in this path.
                /// </summary>
                public readonly int StepCount;

                /// <summary>
                /// The previous path and all the previous steps.
                /// </summary>
                public readonly Path Previous;

                /// <summary>
                /// The total input length of the path.
                /// </summary>
                public readonly int InputLength;

                /// <summary>
                /// An alternate path possible due to optional grammar
                /// </summary>
                public readonly Path Alternate;

                private Path(
                    Step step,
                    int stepCount,
                    Path previous,
                    int inputLength,
                    Path alternate)
                {
                    this.Step = step;
                    this.StepCount = stepCount;
                    this.Previous = previous;
                    this.InputLength = inputLength;
                    this.Alternate = alternate;
                }

                public static Path Empty =
                    new Path(null, 0, null, 0, null);

                public Path AddStep(Parser<TInput> parser, int inputLength, int repeat = 1)
                {
                    var newStep = new Step(parser, inputLength, repeat);
                    return new Path(newStep, this.StepCount + 1, this != Empty ? this : null, this.InputLength + inputLength, null);
                }

                public Path WithAlternate(Path alternate)
                {
                    if (this.Alternate != alternate)
                        return new Path(this.Step, this.StepCount, this.Previous, this.InputLength, alternate);
                    return this;
                }

                public List<Step> GetSteps()
                {
                    var steps = new List<Step>();
                    GetSteps(steps);
                    return steps;
                }

                public void GetSteps(List<Step> steps)
                {
                    this.Previous?.GetSteps(steps);
                    if (this.Step != null)
                        steps.Add(Step);
                }
            }

            public class Step
            {
                /// <summary>
                /// The parser used for the step.
                /// </summary>
                public readonly Parser<TInput> Parser;

                /// <summary>
                /// The total number of input items consumed.
                /// </summary>
                public readonly int InputLength;

                /// <summary>
                /// The number of times repeated.
                /// </summary>
                public readonly int Repeat;

                public Step(Parser<TInput> parser, int inputLength, int repeat)
                {
                    this.Parser = parser;
                    this.InputLength = inputLength;
                    this.Repeat = repeat;
                }
            }

            private class PathFinder : ParserVisitor<TInput>
            {
                private readonly int _inputStart;
                private Source<TInput> _source;
                private Path _currentPath;

                public PathFinder(Source<TInput> source, int inputStart)
                {
                    _source = source;
                    _inputStart = inputStart;
                    _currentPath = Path.Empty;
                }

                /// <summary>
                /// Finds the sequence of least granular parsers in the grammar that consume the most input.
                /// </summary>
                public Path FindPath(Parser<TInput> parser)
                {
                    parser.Accept(this);

                    if (_currentPath.Alternate != null 
                        && _currentPath.Alternate.InputLength > _currentPath.InputLength)
                    {
                        return _currentPath.Alternate;
                    }

                    return _currentPath;
                }

                private int Scan(Parser<TInput> parser, Path path)
                {
                    return parser.Scan(_source, _inputStart + path.InputLength);
                }

                public override void VisitSequence(SequenceParser<TInput> parser)
                {
                    var original = _currentPath;
                    var allNormal = AddSequence(parser.Parsers);
                    if (allNormal)
                        AdjustNormal(original, _currentPath, parser);
                }

                public override void VisitRule<TOutput>(RuleParser<TInput, TOutput> parser)
                {
                    var original = _currentPath;
                    var allNormal = AddSequence(parser.Parsers);
                    if (allNormal)
                        AdjustNormal(original, _currentPath, parser);
                }

                private bool AddSequence(IReadOnlyList<Parser<TInput>> parsers)
                {
                    var originalPath = _currentPath;
                    bool allNormal = true;

                    for (int i = 0; i < parsers.Count; i++)
                    {
                        var previous = _currentPath;
                        var stepParser = parsers[i];
                        allNormal &= AddStep(stepParser, includeAlternate: i > 0);

                        // if we made no progress on this step then abandon
                        if (_currentPath.StepCount == previous.StepCount)
                            return false;
                    }

                    return allNormal;
                }

                private bool AddStep(Parser<TInput> stepParser, bool includeAlternate)
                {
                    var originalPath = _currentPath;

                    // advance to next step
                    stepParser.Accept(this);
                    var primaryPath = _currentPath;

                    // if previous path had an alternate path, try that too and pick which is better
                    if (originalPath.Alternate != null)
                    {
                        _currentPath = originalPath.Alternate;
                        stepParser.Accept(this);

                        if (_currentPath.InputLength > primaryPath.InputLength)
                        {
                            // alternate path + step consumed more input
                            return false; // not normal
                        }
                    }

                    // primary path + step consumed more input (or no alternate)
                    _currentPath = primaryPath;
                    return IsNormalPath(originalPath, _currentPath, stepParser);
                }

                /// <summary>
                /// Determines current path ends with a step that is just the expected parser...
                /// </summary>
                private static bool IsNormalPath(Path originalPath, Path currentPath, Parser<TInput> stepParser)
                {
                    // only one step and its is the expected parser
                    return currentPath.StepCount == originalPath.StepCount + 1
                        && currentPath.Step.Parser == stepParser;
                }

                private void AdjustNormal(Path original, Path result, Parser<TInput> parser)
                {
                    _currentPath = original.AddStep(parser, result.InputLength - original.InputLength).WithAlternate(result.Alternate);
                }

                public override void VisitApply<TLeft, TOutput>(ApplyParser<TInput, TLeft, TOutput> parser)
                {
                    var originalPath = _currentPath;
                    parser.LeftParser.Accept(this);
                    var isNormal = IsNormalPath(originalPath, _currentPath, parser.LeftParser);

                    if (_currentPath.StepCount > originalPath.StepCount)
                    {
                        isNormal &= AddApplyRight(parser.ApplyKind, parser.RightParser);
                    }

                    if (isNormal)
                    {
                        AdjustNormal(originalPath, _currentPath, parser);
                    }
                }

                private bool AddApplyRight(ApplyKind kind, Parser<TInput> parser)
                {
                    switch (kind)
                    {
                        case ApplyKind.One:
                            return AddMinToMax(parser, 1, 1, true);
                        case ApplyKind.ZeroOrOne:
                            return AddMinToMax(parser, 0, 1, true);
                        case ApplyKind.ZeroOrMore:
                            return AddMinToMax(parser, 0, int.MaxValue, true);
                        default:
                            throw new NotImplementedException();
                    }
                }

                public override void VisitBest<TOutput>(BestParser<TInput, TOutput> parser)
                {
                    var original = _currentPath;
                    var isNormal = AddBestPath(parser.Parsers);
                    if (isNormal)
                        AdjustNormal(original, _currentPath, parser);
                }

                public override void VisitBest(BestParser<TInput> parser)
                {
                    var original = _currentPath;
                    var isNormal = AddBestPath(parser.Parsers);
                    if (isNormal)
                        AdjustNormal(original, _currentPath, parser);
                }

                private bool AddBestPath(IReadOnlyList<Parser<TInput>> parsers)
                {
                    var originalPath = _currentPath;
                    Path bestNormalPath = null;
                    Path bestPartialPath = null;

                    foreach (var parser in parsers)
                    {
                        _currentPath = originalPath;
                        parser.Accept(this);

                        var currentIsNormal = IsNormalPath(originalPath, _currentPath, parser);
                        if (currentIsNormal 
                            && (bestNormalPath == null
                                || _currentPath.InputLength > bestNormalPath.InputLength))
                        {
                            bestNormalPath = _currentPath;
                            if (_currentPath.Alternate != null
                                && (bestPartialPath == null 
                                    || _currentPath.Alternate.InputLength > bestPartialPath.InputLength))
                            {
                                bestPartialPath = _currentPath.Alternate;
                            }
                        }
                        else if (bestPartialPath ==  null
                            || _currentPath.InputLength > bestPartialPath.InputLength)
                        {
                            bestPartialPath = _currentPath;
                        }
                    }

                    if (bestNormalPath != null)
                    {
                        _currentPath = bestNormalPath;
                        return true;
                    }
                    else if (bestPartialPath != null)
                    {
                        _currentPath = bestPartialPath;
                        return false;
                    }
                    else
                    {
                        // this should never happen unless there are no parsers
                        _currentPath = originalPath;
                        return true;
                    }
                }

                public override void VisitFirst<TOutput>(FirstParser<TInput, TOutput> parser)
                {
                    var original = _currentPath;
                    var isNormal = AddFirstPath(parser.Parsers);
                    if (isNormal)
                        AdjustNormal(original, _currentPath, parser);
                }

                public override void VisitFirst(FirstParser<TInput> parser)
                {
                    var original = _currentPath;
                    var isNormal = AddFirstPath(parser.Parsers);
                    if (isNormal)
                        AdjustNormal(original, _currentPath, parser);
                }

                private bool AddFirstPath(IReadOnlyList<Parser<TInput>> parsers)
                {
                    var originalPath = _currentPath;
                    Path bestPartialPath = null;

                    // return either the first normal path or the best partial path
                    foreach (var parser in parsers)
                    {
                        _currentPath = originalPath;
                        parser.Accept(this);

                        var currentIsNormal = IsNormalPath(originalPath, _currentPath, parser);
                        if (currentIsNormal)
                        {
                            // first normal parse that succeeds
                            return true;
                        }
                        else if (bestPartialPath == null
                            || _currentPath.InputLength > bestPartialPath.InputLength)
                        {
                            bestPartialPath = _currentPath;
                        }
                    }

                    if (bestPartialPath != null)
                    {
                        _currentPath = bestPartialPath;
                        return false;
                    }
                    else
                    {
                        _currentPath = originalPath;
                        return true;
                    }
                }

                public override void VisitProduce<TOutput>(ProduceParser<TInput, TOutput> parser)
                {
                    var original = _currentPath;
                    parser.Parser.Accept(this);
                    if (IsNormalPath(original, _currentPath, parser.Parser))
                        AdjustNormal(original, _currentPath, parser);
                }

                public override void VisitConvert<TOutput>(ConvertParser<TInput, TOutput> parser)
                {
                    var originalPath = _currentPath;
                    var len = Scan(parser.Pattern, originalPath);
                    if (len > 0)
                    {
                        _currentPath = _currentPath.AddStep(parser, len);
                    }
                }

                public override void VisitFails(FailsParser<TInput> parser)
                {
                    var len = Scan(parser, _currentPath);
                    if (len > 0)
                    {
                        _currentPath = _currentPath.AddStep(parser, len);
                    }
                }

                public override void VisitForward<TOutput>(ForwardParser<TInput, TOutput> parser)
                {
                    var original = _currentPath;
                    var deferredParser = parser.DeferredParser();
                    deferredParser.Accept(this);
                    if (IsNormalPath(original, _currentPath, deferredParser))
                        AdjustNormal(original, _currentPath, parser);
                }

                public override void VisitIf<TOutput>(IfParser<TInput, TOutput> parser)
                {
                    if (Scan(parser.Test, _currentPath) > 0)
                    {
                        var original = _currentPath;
                        parser.Parser.Accept(this);
                        if (IsNormalPath(original, _currentPath, parser.Parser))
                            AdjustNormal(original, _currentPath, parser);
                    }
                }

                public override void VisitIf(IfParser<TInput> parser)
                {
                    if (Scan(parser.Test, _currentPath) > 0)
                    {
                        var original = _currentPath;
                        parser.Parser.Accept(this);
                        if (IsNormalPath(original, _currentPath, parser.Parser))
                            AdjustNormal(original, _currentPath, parser);
                    }
                }

                public override void VisitLimit<TOutput>(LimitParser<TInput, TOutput> parser)
                {
                    // Needs limit too?
                    var len = Scan(parser.Limiter, _currentPath);
                    if (len >= 0)
                    {
                        var originalSource = _source;
                        _source = new LimitSource<TInput>(originalSource, _inputStart + len);

                        var originalPath = _currentPath;
                        parser.Limited.Accept(this);

                        if (IsNormalPath(originalPath, _currentPath, parser.Limited))
                            AdjustNormal(originalPath, _currentPath, parser);

                        _source = originalSource;
                    }
                }

                public override void VisitMap<TOutput>(MapParser<TInput, TOutput> parser)
                {
                    var len = Scan(parser, _currentPath);
                    if (len > 0)
                    {
                        _currentPath = _currentPath.AddStep(parser, len);
                    }
                }

                public override void VisitMatch(MatchParser<TInput> parser)
                {
                    var len = Scan(parser, _currentPath);
                    if (len > 0)
                    {
                        _currentPath = _currentPath.AddStep(parser, len);
                    }
                }

                public override void VisitMatch<TOutput>(MatchParser<TInput, TOutput> parser)
                {
                    var len = Scan(parser, _currentPath);
                    if (len > 0)
                    {
                        _currentPath = _currentPath.AddStep(parser, len);
                    }
                }

                public override void VisitNot(NotParser<TInput> parser)
                {
                    var len = Scan(parser, _currentPath);
                    if (len > 0)
                    {
                        _currentPath = _currentPath.AddStep(parser, len);
                    }
                }

                public override void VisitOneOrMore(OneOrMoreParser<TInput> parser)
                {
                    var original = _currentPath;
                    var allNormal = AddMinToMax(parser.Parser, 1, int.MaxValue, false);
                    if (allNormal)
                        AdjustNormal(original, _currentPath, parser);
                }

                public override void VisitZeroOrMore(ZeroOrMoreParser<TInput> parser)
                {
                    var original = _currentPath;
                    var allNormal = AddMinToMax(parser.Parser, 0, int.MaxValue, false);
                    if (allNormal)
                        AdjustNormal(original, _currentPath, parser);
                }

                public override void VisitOptional<TOutput>(OptionalParser<TInput, TOutput> parser)
                {
                    var original = _currentPath;
                    var allNormal = AddMinToMax(parser.Parser, 0, 1, false);
                    if (allNormal)
                        AdjustNormal(original, _currentPath, parser);
                }

                public override void VisitRequired<TOutput>(RequiredParser<TInput, TOutput> parser)
                {
                    var original = _currentPath;
                    var allNormal = AddMinToMax(parser.Parser, 0, 1, false);
                    if (allNormal)
                        AdjustNormal(original, _currentPath, parser);
                }

                private bool AddMinToMax(
                    Parser<TInput> parser,
                    int min,
                    int max,
                    bool includeAlternate)
                {
                    var originalPath = _currentPath;

                    bool isNormal = true;
                    int normalCount = 0;
                    int normalInputLength = originalPath.InputLength;
                    var normalPath = originalPath;

                    for (int n = 0; n < max; n++)
                    {
                        isNormal = AddStep(parser, includeAlternate);
                        includeAlternate = true;
                        if (isNormal)
                        {
                            // succesfully parsed once normally (not partial)
                            normalInputLength = _currentPath.InputLength;
                            // recompute normal path to include all the normal parsing successes as one step
                            normalCount = n + 1;
                            normalPath = originalPath.AddStep(parser, normalInputLength - originalPath.InputLength, normalCount);
                            _currentPath = normalPath;
                            continue;
                        }
                        else if (n < min)
                        {
                            // we encountered a partial parsing before the min was reached
                            // return what we have as not normal
                            return false;
                        }
                        break;
                    }

                    if (normalCount == 0 && min == 0)
                    {
                        // one fake instance (repeat == 0) so caller be able to adjust path
                        normalPath = originalPath.AddStep(parser, 0, 1);
                    }

                    // if current path (may include partial) is longer than normal path, add it as an alternative
                    if (_currentPath.InputLength > normalPath.InputLength)
                    {
                        normalPath = normalPath.WithAlternate(_currentPath);
                    }

                    _currentPath = normalPath;

                    // either returning the normal path or the normal path + alternative
                    return true;
                }
            }
        }
    }
}
