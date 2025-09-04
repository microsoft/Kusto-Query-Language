using System;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Parsing
{
    using Utils;

    /// <summary>
    /// API's for parsing as much of a parser's grammar that matches the input.
    /// </summary>
    public static class PartialParser
    {
        /// <summary>
        /// Parses the input into either the expected output for the parser,
        /// or one or more outputs that correspond to the recognized parts the grammar.
        /// </summary>
        public static int ParsePartial<TInput>(
            this Parser<TInput> parser, Source<TInput> input, int inputStart, List<object> output, int outputStart, Action<Parser<TInput>, List<object>> onFailure = null)
        {
            return Partial<TInput>.Parse(parser, input, inputStart, output, outputStart, onFailure);
        }

        /// <summary>
        /// Parses the input into the output of the partially parsed parser that consumes the most input.
        /// </summary>
        public static int ParsePartialBest<TInput>(
            IReadOnlyList<Parser<TInput>> parsers, Source<TInput> input, int inputStart, List<object> output, int outputStart, List<Parser<TInput>> bestParsers, Action<Parser<TInput>, List<object>> onFailure = null)
        {
            return Partial<TInput>.ParseBest(parsers, input, inputStart, output, outputStart, bestParsers, onFailure);
        }

        /// <summary>
        /// Returns the number of input item that would be consumed by <see cref="ParsePartial"/>
        /// </summary>
        public static int ScanPartial<TInput>(
            this Parser<TInput> parser, Source<TInput> input, int inputStart)
        {
            return Partial<TInput>.Scan(parser, input, inputStart);
        }

        /// <summary>
        /// Returns the number of input items that would be consumed by <see cref="ParsePartialBest"/>.
        /// </summary>
        public static int ScanPartialBest<TInput>(
            IReadOnlyList<Parser<TInput>> parsers, Source<TInput> input, int inputStart)
        {
            return Partial<TInput>.ScanBest(parsers, input, inputStart);
        }

        private static class Partial<TInput>
        {
            public static int Scan(Parser<TInput> parser, Source<TInput> input, int inputStart)
            {
                var pathFinder = new PathFinder();
                var path = pathFinder.FindPath(parser, input, inputStart);
                return path != null && path != Path.Empty ? path.InputLength : -1;
            }

            public static int Parse(Parser<TInput> parser, Source<TInput> input, int inputStart, List<object> output, int outputStart, Action<Parser<TInput>, List<object>> onFailure)
            {
                var pathFinder = new PathFinder();
                var path = pathFinder.FindPath(parser, input, inputStart);
                return ParsePath(path, input, inputStart, output, outputStart, onFailure);
            }

            private class MemoizedInfo
            {
                public Dictionary<BestPathKey, BestPathInfo> BestPaths = 
                    new Dictionary<BestPathKey, BestPathInfo>();
            }

            private struct BestPathKey : IEquatable<BestPathKey>
            {
                public IReadOnlyList<Parser<TInput>> Parsers;
                public int InputStart;

                public BestPathKey(
                    IReadOnlyList<Parser<TInput>> parsers,
                    int inputStart)
                {
                    this.Parsers = parsers;
                    this.InputStart = inputStart;
                    _hc = 0;
                    ComputeHashCode();
                }

                private int _hc;
                private void ComputeHashCode()
                {
                    int hc = this.InputStart;
                    foreach (var parser in this.Parsers)
                    {
                        hc += parser.GetHashCode();
                    }
                    _hc = hc;
                }

                public bool Equals(BestPathKey other)
                {
                    if (other.InputStart != this.InputStart)
                        return false;
                    if (other.Parsers.Count != this.Parsers.Count)
                        return false;
                    for (int i = 0; i < this.Parsers.Count; i++)
                    {
                        if (other.Parsers[i] != this.Parsers[i])
                            return false;
                    }
                    return true;
                }

                public override bool Equals(object obj) =>
                    obj is BestPathKey other
                    && Equals(other);

                public override int GetHashCode() => _hc;
            }

            private class BestPathInfo
            {
                public Path BestPath;
                public List<Parser<TInput>> BestParsers;

                public BestPathInfo(
                    Path bestPath,
                    List<Parser<TInput>> bestParsers)
                {
                    this.BestPath = bestPath;
                    this.BestParsers = bestParsers;
                }
            }

            private static BestPathInfo GetBestPath(
                IReadOnlyList<Parser<TInput>> parsers,
                Source<TInput> source,
                int inputStart)
            {
                var cached = source.Cache.GetOrCreate<MemoizedInfo>();
                var key = new BestPathKey(parsers, inputStart);
                if (cached.BestPaths.TryGetValue(key, out var info))
                    return info;
                var pathFinder = new PathFinder();
                var bestParsers = new List<Parser<TInput>>();
                var path = pathFinder.FindBestPath(parsers, source, inputStart, bestParsers);
                info = new BestPathInfo(path, bestParsers);
                cached.BestPaths[key] = info;
                return info;
            }

            public static int ScanBest(IReadOnlyList<Parser<TInput>> parsers, Source<TInput> input, int inputStart)
            {
                var info = GetBestPath(parsers, input, inputStart);
                return info.BestPath != null && info.BestPath != Path.Empty 
                    ? info.BestPath.InputLength 
                    : -1;
            }

            public static int ParseBest(IReadOnlyList<Parser<TInput>> parsers, Source<TInput> input, int inputStart, List<object> output, int outputStart, List<Parser<TInput>> bestParsers, Action<Parser<TInput>, List<object>> onFailure)
            {
                var best = GetBestPath(parsers, input, inputStart);
                if (bestParsers != null)
                    bestParsers.AddRange(best.BestParsers);
                return ParsePath(best.BestPath, input, inputStart, output, outputStart, onFailure);
            }

            private static int ParsePath(Path path, Source<TInput> input, int inputStart, List<object> output, int outputStart, Action<Parser<TInput>, List<object>> onFailure)
            {
                if (path == null || path == Path.Empty)
                    return -1;

                var inputPosition = inputStart;
                var steps = GetSteps(path);

                foreach (var step in steps)
                {
                    if (step.IsError)
                    {
                        onFailure?.Invoke(step.Parser, output);
                    }
                    else
                    {
                        var len = step.Parser.Parse(input, inputPosition, output, output.Count);
                        inputPosition += len;
                    }
                }

                return inputPosition - inputStart;
            }

            private static IReadOnlyList<Path> GetSteps(Path path)
            {
                var steps = new Path[path.Length];
                while (path != Path.Empty)
                {
                    steps[path.Length - 1] = path;
                    path = path.Previous;
                }
                return steps;
            }

            /// <summary>
            /// A pool of path lists.
            /// </summary>
            private static readonly ObjectPool<List<Path>> _pathListPool =
                new ObjectPool<List<Path>>(() => new List<Path>(), list => list.Clear(), 20);

            /// <summary>
            /// A pool of output lists.
            /// </summary>
            private static readonly ObjectPool<List<ScanOutput>> _outputListPool =
                new ObjectPool<List<ScanOutput>>(() => new List<ScanOutput>(), list => list.Clear(), 20);

            /// <summary>
            /// True if the output path has succesfully scanned something relative to the input path.
            /// </summary>
            private static bool IsSuccess(Path input, Path output) =>
                output != input && !output.IsError;

            /// <summary>
            /// True if the output path has made some progress, either successfully or partially scanned.
            /// </summary>
            private static bool HasProgress(Path input, Path output) =>
                output != input;

            /// <summary>
            /// Replace one or more repeated instances of inner parser at end of path with outer parser,
            /// consuming the same amount out input.
            /// </summary>
            private static Path AdjustPath(Parser<TInput> outer, Parser<TInput> inner, Path initial, Path result, int maxRepeats = 1)
            {
                if (!result.IsError)
                {
                    int n = 0;
                    Path path = result;

                    while (path.Parser == inner)
                    {
                        path = path.Previous;
                        n++;
                    }

                    if (path == initial && n > 0 && n <= maxRepeats)
                    {
                        return initial.Add(outer, result.InputLength - initial.InputLength);
                    }
                }

                return result;
            }

            /// <summary>
            /// Replace sequences of inner parsers at end of path with outer parser,
            /// consuming the same amount out input.
            /// </summary>
            private static Path AdjustPath(Parser<TInput> outer, IReadOnlyList<Parser<TInput>> inners, Path initial, Path result)
            {
                if (result.Length == initial.Length + inners.Count
                    && !result.IsError)
                {
                    // check that all inners are represented in sequence
                    var path = result;
                    for (int i = inners.Count - 1; i >= 0; i--, path = path.Previous)
                    {
                        if (path.Parser != inners[i])
                            return result;
                    }

                    return initial.Add(outer, result.InputLength - initial.InputLength);
                }

                return result;
            }

            /// <summary>
            /// Replace one or more repeated instances of inner parser at end of each path with outer parser,
            /// consuming the same amount out input.
            /// </summary>
            private static ScanOutput AdjustPaths(ScanOutput output, Parser<TInput> outer, Parser<TInput> inner, int maxRepeats = 1)
            {
                if (output.HasMultiplePaths)
                {
                    var outputPaths = _pathListPool.AllocateFromPool();
                    try
                    {
                        foreach (var path in output.Paths)
                        {
                            outputPaths.Add(AdjustPath(outer, inner, output.Input.Path, path, maxRepeats));
                        }

                        return output.WithPaths(outputPaths);
                    }
                    finally
                    {
                        _pathListPool.ReturnToPool(outputPaths);
                    }
                }
                else
                {
                    return output.WithPath(AdjustPath(outer, inner, output.Input.Path, output.Path, maxRepeats));
                }
            }

            /// <summary>
            /// Gets the single best path from the collection of paths.
            /// </summary>
            private static Path GetBestPath(IReadOnlyList<Path> paths, Path input)
            {
                var maxInput = paths.Max(p => p.InputLength);
                var bestPaths = paths.Where(p => p.InputLength == maxInput).ToList();

                if (bestPaths.Count > 1)
                {
                    var firstSuccess = bestPaths.FirstOrDefault(p => IsSuccess(input, p));
                    if (firstSuccess != null)
                        return firstSuccess;

                    var withoutError = bestPaths.FirstOrDefault(p => !p.IsError);
                    if (withoutError != null)
                        return withoutError;

                    // all paths are errors... combine them
                    var combinedErrors = new BestParser<TInput>(bestPaths.Select(p => p.Parser).ToReadOnly());
                    var combinedPath = bestPaths[0].Previous.AddError(combinedErrors);
                    return combinedPath;
                }
                else
                {
                    return bestPaths[0];
                }
            }


            /// <summary>
            /// A parser visitor that finds paths that are legal full or partial scan of an input.
            /// </summary>
            private class PathFinder : ParserVisitor<TInput, ScanInput, ScanOutput>
            {
                public PathFinder()
                {
                }

                /// <summary>
                /// Finds the sequence of least granular parsers in the grammar that consume the most input.
                /// </summary>
                public Path FindPath(Parser<TInput> parser, Source<TInput> source, int inputStart)
                {
                    return FindBestPath(new[] { parser }, source, inputStart);
                }

                public Path FindBestPath(
                    IReadOnlyList<Parser<TInput>> parsers,
                    Source<TInput> source,
                    int inputStart,
                    List<Parser<TInput>> bestParsers = null)
                {
                    var input = new ScanInput(source, inputStart, Path.Empty);

                    var results = parsers.SelectMany(parser =>
                    {
                        var output = parser.Accept(this, input);
                        return output.Paths.Select(path => (parser, path));
                    }).ToList();

                    var maxInput = results.Max(r => r.path.InputLength);
                    var bestResults = results.Where(r => r.path.InputLength == maxInput).ToList();

                    if (bestParsers != null)
                    {
                        bestParsers.AddRange(bestResults.Select(r => r.parser).Distinct());
                    }

                    var bestPaths = bestResults.Select(r => r.path).ToList();
                    var bestPath = GetBestPath(bestPaths, Path.Empty);

#if false
                    var nonPartialResults = bestResults.Where(r => IsSuccess(Path.Empty, r.path)).ToList();
                    var firstNonPartial = nonPartialResults.FirstOrDefault();
                    if (firstNonPartial != default)
                    {
                        return firstNonPartial.path;
                    }

                    // if all are otherwise equal, return the first one
                    var bestPath = bestResults[0].path;

#endif
                    return bestPath;
                }

                public override ScanOutput VisitMatch(MatchParser<TInput> parser, ScanInput input)
                {
                    return ScanParser(parser, parser, input);
                }

                public override ScanOutput VisitMatch<TOutput>(MatchParser<TInput, TOutput> parser, ScanInput input)
                {
                    return ScanParser(parser, parser, input);
                }

                public override ScanOutput VisitConvert<TOutput>(ConvertParser<TInput, TOutput> parser, ScanInput input)
                {
                    return ScanParser(parser, parser.Pattern, input);
                }

                public override ScanOutput VisitMap<TOutput>(MapParser<TInput, TOutput> parser, ScanInput input)
                {
                    return ScanParser(parser, parser, input);
                }

                public override ScanOutput VisitFails(FailsParser<TInput> parser, ScanInput input)
                {
                    return ScanParser(parser, parser, input);
                }

                public override ScanOutput VisitNot(NotParser<TInput> parser, ScanInput input)
                {
                    return ScanParser(parser, parser, input);
                }

                /// <summary>
                /// Just simple scan of parser.
                /// </summary>
                private ScanOutput ScanParser(Parser<TInput> parser, Parser<TInput> scanned, ScanInput input)
                {
                    var len = scanned.Scan(input.Source, input.InputStart + input.Path.InputLength);
                    if (len > 0)
                    {
                        return new ScanOutput(input, input.Path.Add(parser, len));
                    }
                    return input;
                }

                public override ScanOutput VisitForward<TOutput>(ForwardParser<TInput, TOutput> parser, ScanInput input)
                {
                    // don't partial parse a forward parser, since it implies cyclic behavior
                    var deferred = parser.DeferredParser();
                    return ScanParser(parser, deferred, input);
                }

                public override ScanOutput VisitIf<TOutput>(IfParser<TInput, TOutput> parser, ScanInput input)
                {
                    return ScanIf(parser, parser.Test, parser.Parser, input);
                }

                public override ScanOutput VisitIf(IfParser<TInput> parser, ScanInput input)
                {
                    return ScanIf(parser, parser.Test, parser.Parser, input);
                }

                private ScanOutput ScanIf(Parser<TInput> outer, Parser<TInput> test, Parser<TInput> inner, ScanInput input)
                {
                    var len = test.Scan(input.Source, input.InputStart + input.Path.InputLength);
                    if (len > 0)
                    {
                        var result = inner.Accept(this, input);
                        return AdjustPaths(result, outer, inner);
                    }
                    return input;
                }

                public override ScanOutput VisitLimit<TOutput>(LimitParser<TInput, TOutput> parser, ScanInput input)
                {
                    var len = parser.Limiter.Scan(input.Source, input.InputStart + input.Path.InputLength);
                    if (len >= 0)
                    {
                        var limitSource = new LimitSource<TInput>(input.Source, input.InputStart + len);
                        var result = parser.Limited.Accept(this, input.WithSource(limitSource));
                        return AdjustPaths(result, parser, parser.Limited);
                    }
                    return input;
                }

                public override ScanOutput VisitSequence(SequenceParser<TInput> parser, ScanInput input)
                {
                    return ScanSequence(parser, parser.Parsers, input);
                }

                public override ScanOutput VisitRule<TOutput>(RuleParser<TInput, TOutput> parser, ScanInput input)
                {
                    return ScanSequence(parser, parser.Parsers, input);
                }

                public override ScanOutput VisitProduce<TOutput>(ProduceParser<TInput, TOutput> parser, ScanInput input)
                {
                    return AdjustPaths(parser.Parser.Accept(this, input), parser, parser.Parser);
                }

                private ScanOutput ScanSequence(Parser<TInput> outer, IReadOnlyList<Parser<TInput>> stepParsers, ScanInput input)
                {
                    var paths = _pathListPool.AllocateFromPool();
                    try
                    {
                        ScanSequenceSteps(stepParsers, 0, input, input.Path, paths);

                        var best = GetBestPath(paths, input.Path);
                        best = AdjustPath(outer, stepParsers, input.Path, best);

                        return new ScanOutput(input, best);
                    }
                    finally
                    {
                        _pathListPool.ReturnToPool(paths);
                    }
                }

                /// <summary>
                /// Scans the sequence steps from stepIndex forward to the end of the sequence,
                /// placing all the resulting paths in the paths collection.
                /// </summary>
                private void ScanSequenceSteps(IReadOnlyList<Parser<TInput>> stepParsers, int stepIndex, ScanInput initialInput, Path path, List<Path> resultPaths)
                {
                    var stepParser = stepParsers[stepIndex];

                    // scan this step
                    var stepOutput = stepParser.Accept(this, initialInput.WithPath(path));

                    if (stepOutput.HasMultiplePaths)
                    {
                        foreach (var outputPath in stepOutput.Paths)
                        {
                            if (stepIndex < stepParsers.Count - 1 && IsSuccess(path, outputPath))
                            {
                                ScanSequenceSteps(stepParsers, stepIndex + 1, initialInput, outputPath, resultPaths);
                            }
                            else
                            {
                                var resultPath = AddSequenceError(stepParser, initialInput.Path, stepOutput.Input.Path, outputPath);
                                resultPaths.Add(resultPath);
                            }
                        }
                    }
                    else if (stepIndex < stepParsers.Count - 1 && IsSuccess(path, stepOutput.Path))
                    {
                        ScanSequenceSteps(stepParsers, stepIndex + 1, initialInput, stepOutput.Path, resultPaths);
                    }
                    else
                    {
                        var resultPath = AddSequenceError(stepParser, initialInput.Path, stepOutput.Input.Path, stepOutput.Path);
                        resultPaths.Add(resultPath);
                    }
                }

                /// <summary>
                /// Adds an error to the final path when parsing a sequence.
                /// </summary>
                private static Path AddSequenceError(Parser<TInput> parser, Path initialPath, Path previousStepPath, Path finalPath)
                {
                    // if the path is the same as the initial path, return it without any error (it will be condered to have failed scanning).
                    // if the made no progress over the last step (previous) then it should have an error since this is a partial parsing of the sequence.
                    // don't add an error on top an already existing error that might exist.
                    if (finalPath != initialPath && finalPath == previousStepPath && !finalPath.IsError)
                        return finalPath.AddError(parser);
                    return finalPath;
                }

                public override ScanOutput VisitOptional<TOutput>(OptionalParser<TInput, TOutput> parser, ScanInput input)
                {
                    return ScanOptional(parser, parser.Parser, input);
                }

                public override ScanOutput VisitRequired<TOutput>(RequiredParser<TInput, TOutput> parser, ScanInput input)
                {
                    return ScanOptional(parser, parser.Parser, input);
                }

                private ScanOutput ScanOptional(Parser<TInput> outer, Parser<TInput> inner, ScanInput input)
                {
                    var result = AdjustPaths(inner.Accept(this, input), outer, inner);

                    // add path where option consumes nothing
                    if (!result.HadSuccess)
                        result = result.AddPath(input.Path.Add(outer, 0)); 

                    return result;
                }

                public override ScanOutput VisitOneOrMore(OneOrMoreParser<TInput> parser, ScanInput input)
                {
                    return ScanRepeat(parser, parser.Parser, 1, Int32.MaxValue, input);
                }

                public override ScanOutput VisitZeroOrMore(ZeroOrMoreParser<TInput> parser, ScanInput input)
                {
                    return ScanRepeat(parser, parser.Parser, 0, Int32.MaxValue, input);
                }

                private ScanOutput ScanRepeat(
                    Parser<TInput> outer,
                    Parser<TInput> inner,
                    int min,
                    int max,
                    ScanInput input)
                {
                    var (result, maxSuccesses, maxSuccessInputLength) = ScanRepeat(inner, min, max, input, input);

                    result = AdjustPaths(result, outer, inner, max);

                    // if we had more than min successful scans, but the result has no fully successful paths
                    // add a path that represents the maximal successful scan of the outer parser.
                    if (maxSuccesses >= min && !result.HadSuccess)
                    {
                        result = result.AddPath(input.Path.Add(outer, maxSuccessInputLength - input.Path.InputLength));
                    }

                    return result;
                }

                private (ScanOutput result, int maxSuccesses, int maxSuccessInputLength) ScanRepeat(
                    Parser<TInput> inner,
                    int min,
                    int max,
                    ScanInput input,
                    ScanOutput previous)
                {
                    var previousOutputs = _outputListPool.AllocateFromPool();
                    var newOutputs = _outputListPool.AllocateFromPool();
                    var resultPaths = _pathListPool.AllocateFromPool();

                    try
                    {
                        previousOutputs.Add(previous);

                        int maxSuccessInputLength = input.Path.InputLength;

                        int i = 0;
                        int maxSuccesses = 0;
                        bool errorOnFirst = previous.Input.Path != input.Path;

                        for (; i <= max && previousOutputs.Count > 0; i++)
                        {
                            newOutputs.Clear();
                            foreach (var prevOutput in previousOutputs)
                            {
                                if (prevOutput.HasMultiplePaths)
                                {
                                    foreach (var path in prevOutput.Paths)
                                    {
                                        if (i == 0 || (i < max && IsSuccess(prevOutput.Input.Path, path)))
                                        {
                                            var output = inner.Accept(this, input.WithPath(path));
                                            newOutputs.Add(output);

                                            if (output.HadSuccess)
                                            {
                                                maxSuccesses = i + 1;
                                                maxSuccessInputLength = Math.Max(maxSuccessInputLength, output.MaxSuccessInputLength);
                                            }
                                        }
                                        else
                                        {
                                            // repeats less than minimum are a logically a sequence
                                            var resultPath = i <= min
                                                ? AddSequenceError(inner, input.Path, prevOutput.Input.Path, path)
                                                : path;
                                            resultPaths.Add(resultPath);
                                        }
                                    }
                                }
                                else if (i == 0 || (i < max && IsSuccess(prevOutput.Input.Path, prevOutput.Path)))
                                {
                                    var output = inner.Accept(this, input.WithPath(prevOutput.Path));
                                    newOutputs.Add(output);

                                    if (output.HadSuccess)
                                    {
                                        maxSuccesses = i + 1;
                                        maxSuccessInputLength = Math.Max(maxSuccessInputLength, output.MaxSuccessInputLength);
                                    }
                                }
                                else
                                {
                                    // repeats less than minimum are a logically a sequence
                                    var resultPath = i <= min
                                        ? AddSequenceError(inner, input.Path, prevOutput.Input.Path, prevOutput.Path)
                                        : prevOutput.Path;
                                    resultPaths.Add(resultPath);
                                }
                            }

                            // swap previous and new outputs and try again...
                            var tmp = previousOutputs;
                            previousOutputs = newOutputs;
                            newOutputs = tmp;
                        }

                        var result = new ScanOutput(input, resultPaths);
                        return (result, maxSuccesses, maxSuccessInputLength);
                    }
                    finally
                    {
                        _outputListPool.ReturnToPool(previousOutputs);
                        _outputListPool.ReturnToPool(newOutputs);
                        _pathListPool.ReturnToPool(resultPaths);
                    }
                }

                public override ScanOutput VisitBest<TOutput>(BestParser<TInput, TOutput> parser, ScanInput input)
                {
                    return ScanBest(parser, parser.Parsers, input);
                }

                public override ScanOutput VisitBest(BestParser<TInput> parser, ScanInput input)
                {
                    return ScanBest(parser, parser.Parsers, input);
                }

                private ScanOutput ScanBest(Parser<TInput> outer, IReadOnlyList<Parser<TInput>> alternates, ScanInput input)
                {
                    var candidates = _pathListPool.AllocateFromPool();
                    try
                    {
                        foreach (var alt in alternates)
                        {
                            var output = AdjustPaths(alt.Accept(this, input), outer, alt);
                            output.GetPaths(candidates);
                        }

                        var best = GetBestPath(candidates, input.Path);

                        return new ScanOutput(input, best);
                    }
                    finally
                    {
                        _pathListPool.ReturnToPool(candidates);
                    }
                }

                public override ScanOutput VisitFirst<TOutput>(FirstParser<TInput, TOutput> parser, ScanInput input)
                {
                    return ScanFirst(parser, parser.Parsers, input);
                }

                public override ScanOutput VisitFirst(FirstParser<TInput> parser, ScanInput input)
                {
                    return ScanFirst(parser, parser.Parsers, input);
                }

                public ScanOutput ScanFirst(Parser<TInput> outer, IReadOnlyList<Parser<TInput>> alternates, ScanInput input)
                {
                    var candidates = _pathListPool.AllocateFromPool();
                    try
                    {
                        foreach (var alt in alternates)
                        {
                            var accepted = alt.Accept(this, input);
                            var output = AdjustPaths(accepted, outer, alt);
                            output.GetPaths(candidates);
                        }

                        // first no-error path is obviously first
                        var first = candidates.FirstOrDefault(c => IsSuccess(input.Path, c));

                        // if only error paths, then take best one?
                        if (first == null)
                            first = GetBestPath(candidates, input.Path);

                        return new ScanOutput(input, first);
                    }
                    finally
                    {
                        _pathListPool.ReturnToPool(candidates);
                    }
                }

                public override ScanOutput VisitApply<TLeft, TOutput>(ApplyParser<TInput, TLeft, TOutput> outer, ScanInput input)
                {
                    var leftOutput = outer.LeftParser.Accept(this, input);

                    if (!leftOutput.HadSuccess)
                        return leftOutput;

                    var min = outer.ApplyKind == ApplyKind.One ? 1 : 0;
                    var max = outer.ApplyKind == ApplyKind.One || outer.ApplyKind == ApplyKind.ZeroOrOne ? 1 : Int32.MaxValue;

                    var (result, maxSuccesses, maxSuccessInputLength) = ScanRepeat(outer.RightParser, min, max, input, leftOutput);

                    result = AdjustApplyPaths(result, outer, outer.LeftParser, outer.RightParser, max);

                    // if we had more than min successful scans (putting us in the or-more territory)
                    // we found no paths that were successfully scanned
                    // add a path that represents the maximal successful scan of the outer parser.
                    if (maxSuccesses >= min && !result.HadSuccess)
                    {
                        result = result.AddPath(input.Path.Add(outer, maxSuccessInputLength - input.Path.InputLength));
                    }

                    return result;
                }

                private static ScanOutput AdjustApplyPaths(ScanOutput output, Parser<TInput> outer, Parser<TInput> left, Parser<TInput> right, int maxRight)
                {
                    if (output.HasMultiplePaths)
                    {
                        var outputPaths = _pathListPool.AllocateFromPool();
                        try
                        {
                            foreach (var path in output.Paths)
                            {
                                if (IsSuccess(output.Input.Path, path))
                                {
                                    outputPaths.Add(AdjustApplyPath(outer, left, right, output.Input.Path, path, maxRight));
                                }
                            }

                            return output.WithPaths(outputPaths);
                        }
                        finally
                        {
                            _pathListPool.ReturnToPool(outputPaths);
                        }
                    }
                    else
                    {
                        return output.WithPath(AdjustApplyPath(outer, left, right, output.Input.Path, output.Path, maxRight));
                    }
                }

                private static Path AdjustApplyPath(Parser<TInput> outer, Parser<TInput> left, Parser<TInput> right, Path initial, Path result, int maxRight)
                {
                    if (!result.IsError)
                    {
                        int nRight = 0;
                        Path path = result;

                        while (path.Parser == right)
                        {
                            path = path.Previous;
                            nRight++;
                        }

                        if (path.Parser == left
                            && path.Previous == initial
                            && nRight <= maxRight)
                        {
                            return initial.Add(outer, result.InputLength - initial.InputLength);
                        }
                    }

                    return result;
                }
            }

            #region Path, ScanInput, ScanOutput

            /// <summary>
            /// A sequence of <see cref="Parser{TInput}"/> that represent a full or partial scan of the input.
            /// </summary>
            public class Path
            {
                /// <summary>
                /// The previous path and all the previous steps.
                /// </summary>
                public readonly Path Previous;

                /// <summary>
                /// The last parser used.
                /// </summary>
                public readonly Parser<TInput> Parser;

                /// <summary>
                /// The total number of nodes in this path.
                /// </summary>
                public readonly int Length;

                /// <summary>
                /// The total input length of the path.
                /// </summary>
                public readonly int InputLength;

                /// <summary>
                /// True if this step is an error.
                /// </summary>
                public readonly bool IsError;

                /// <summary>
                /// An empty path
                /// </summary>
                public static readonly Path Empty =
                    new Path(null, null, 0, 0, false);

                private Path(
                    Path previous,
                    Parser<TInput> parser,
                    int length,
                    int inputLength,
                    bool isError)
                {
                    this.Previous = previous;
                    this.Parser = parser;
                    this.Length = length;
                    this.InputLength = inputLength;
                    this.IsError = isError;
                }

                public Path Add(Parser<TInput> parser, int inputLength)
                {
                    return new Path(
                        this,
                        parser,
                        this.Length + 1,
                        this.InputLength + inputLength,
                        false
                        );
                }

                public Path AddError(Parser<TInput> parser)
                {
                    return new Path(
                        this,
                        parser,
                        this.Length + 1,
                        this.InputLength,
                        true
                       );
                }
            }

            /// <summary>
            /// The input for <see cref="PathFinder"/> visits.
            /// </summary>
            public struct ScanInput
            {
                /// <summary>
                /// The input source in use.
                /// </summary>
                public readonly Source<TInput> Source;

                /// <summary>
                /// The input start position within the source
                /// </summary>
                public readonly int InputStart;

                /// <summary>
                /// The path before the parser is scanned
                /// </summary>
                public readonly Path Path;

                public ScanInput(
                    Source<TInput> source,
                    int inputStart,
                    Path path)
                {
                    this.Source = source;
                    this.InputStart = inputStart;
                    this.Path = path;
                }

                public ScanInput WithSource(Source<TInput> source, int inputStart = -1)
                {
                    return new ScanInput(source, inputStart >= 0 ? inputStart : this.InputStart, this.Path);
                }

                public ScanInput WithPath(Path path)
                {
                    return new ScanInput(this.Source, this.InputStart, path);
                }

                public static implicit operator ScanOutput(ScanInput input) =>
                    new ScanOutput(input, input.Path);
            }

            /// <summary>
            /// The output for <see cref="PathFinder"/> visits.
            /// </summary>
            public struct ScanOutput
            {
                public ScanInput Input { get; }

                private Path _path;

                public Path Path
                {
                    get
                    {
                        if (_paths != null && _paths.Count == 1)
                        {
                            return _paths[0];
                        }

                        return _path;
                    }
                }

                private IReadOnlyList<Path> _paths;

                public IReadOnlyList<Path> Paths
                {
                    get
                    {
                        if (_paths == null)
                        {
                            _paths = SafeList<Path>.Empty.Append(this.Path);
                        }

                        return _paths;
                    }
                }

                public ScanOutput(ScanInput input, Path path)
                {
                    this.Input = input;
                    _path = path;
                    _paths = null;
                }

                public ScanOutput(ScanInput input, IEnumerable<Path> paths)
                {
                    this.Input = input;
                    _paths = paths.ToReadOnly();
                    _path = null;
                }

                /// <summary>
                /// True if the output has multiple resulting paths.
                /// </summary>
                public bool HasMultiplePaths =>
                    _paths != null && _paths.Count > 1;

                /// <summary>
                /// True if any path made progress relative to the input path.
                /// </summary>
                public bool MadeProgress
                {
                    get
                    {
                        if (this.HasMultiplePaths)
                        {
                            var me = this;
                            return this.Paths.Any(p => HasProgress(me.Input.Path, p));
                        }
                        else
                        {
                            return HasProgress(this.Input.Path, this.Path);
                        }
                    }
                }

                /// <summary>
                /// True if any path had a success scan relative to input path.
                /// </summary>
                public bool HadSuccess
                {
                    get
                    {
                        if (this.HasMultiplePaths)
                        {
                            var me = this;
                            return this.Paths.Any(p => IsSuccess(me.Input.Path, p));
                        }
                        else
                        {
                            return IsSuccess(this.Input.Path, this.Path);
                        }
                    }
                }

                /// <summary>
                /// The maximum input length of successfully scanned paths.
                /// </summary>
                public int MaxSuccessInputLength
                {
                    get
                    {
                        if (this.HasMultiplePaths)
                        {
                            var me = this;
                            return this.Paths.Where(p => IsSuccess(me.Input.Path, p)).Max(p => p.InputLength);
                        }
                        else if (IsSuccess(this.Input.Path, this.Path))
                        {
                            return this.Path.InputLength;
                        }
                        else
                        {
                            return this.Input.Path.Length;
                        }
                    }
                }

                /// <summary>
                /// Returns a new <see cref="ScanOutput"/> with the Input modified to the specified input.
                /// </summary>
                public ScanOutput WithInput(ScanInput input)
                {
                    if (HasMultiplePaths)
                    {
                        return new ScanOutput(input, this.Paths);
                    }
                    else
                    {
                        return new ScanOutput(input, this.Path);
                    }
                }

                /// <summary>
                /// Return a new <see cref="ScanOutput"/> with the path modified to the specified path.
                /// </summary>
                public ScanOutput WithPath(Path path)
                {
                    return new ScanOutput(this.Input, path);
                }

                /// <summary>
                /// Returns a new <see cref="ScanOutput"/> with the paths modified to the specified paths.
                /// </summary>
                public ScanOutput WithPaths(IEnumerable<Path> paths)
                {
                    return new ScanOutput(this.Input, paths);
                }

                /// <summary>
                /// Returns a new <see cref="ScanOutput"/> with the paths modified to be the same paths as from the other output.
                /// </summary>
                public ScanOutput WithPaths(ScanOutput other)
                {
                    if (other.HasMultiplePaths)
                    {
                        return new ScanOutput(this.Input, other.Paths);
                    }
                    else
                    {
                        return new ScanOutput(this.Input, other.Path);
                    }
                }

                /// <summary>
                /// Returns a new <see cref="ScanOutput"/> with paths modified to include the specified path.
                /// </summary>
                public ScanOutput AddPath(Path path)
                {
                    return WithPaths(this.Paths.Append(path).Distinct());
                }

                /// <summary>
                /// Returns a new <see cref="ScanOutput"/> with paths modified to include the specified paths.
                /// </summary>
                public ScanOutput AddPaths(IEnumerable<Path> paths)
                {
                    return WithPaths(this.Paths.Concat(paths).Distinct());
                }

                /// <summary>
                /// Returns a new <see cref="ScanOutput"/> with paths modified to include the paths from the other output.
                /// </summary>
                public ScanOutput AddPaths(ScanOutput other)
                {
                    if (other.HasMultiplePaths)
                    {
                        return AddPaths(other.Paths);
                    }
                    else
                    {
                        return AddPath(other.Path);
                    }
                }

                /// <summary>
                /// Appends the paths to the list
                /// </summary>
                public void GetPaths(List<Path> paths)
                {
                    if (HasMultiplePaths)
                    {
                        paths.AddRange(this.Paths);
                    }
                    else
                    {
                        paths.Add(this.Path);
                    }
                }
            }

            #endregion
        }
    }
}
