using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Kusto.Language.Parsing
{
    using Utils;

    /// <summary>
    /// Parser combinators, APIs to combine/construt parsers out of other parsers.
    /// </summary>
    public static class Parsers<TInput>
    {
        /// <summary>
        /// A parser that consumes all the input items successfully consumed by the specified parsers.
        /// Fails if any parser fails.
        /// </summary>
        public static Parser<TInput> And(params Parser<TInput>[] parsers) =>
            new SequenceParser<TInput>(parsers);

        /// <summary>
        /// A parser that always consumes one input item, but produces nothing.
        /// </summary>
        public static readonly Parser<TInput> Any =
            Match(t => true).WithTag("<any>");

        /// <summary>
        /// A parser that yields the result of the left-hand applied to the right-hand parser.
        /// </summary>
        public static Parser<TInput, TOutput> Apply<TLeft, TOutput>(Parser<TInput, TLeft> leftParser, Func<LeftValue<TLeft>, RightParser<TInput, TOutput>> fnRightParser) =>
            new ApplyParser<TInput, TLeft, TOutput>(ApplyKind.One, leftParser, fnRightParser(default(LeftValue<TLeft>)));

        /// <summary>
        /// A parser that yields the result of the left-hand parser or the result of applying that value to the right-hand parser.
        /// </summary>
        public static Parser<TInput, TOutput> ApplyOptional<TOutput>(Parser<TInput, TOutput> leftParser, Func<LeftValue<TOutput>, RightParser<TInput, TOutput>> fnRightParser) =>
            new ApplyParser<TInput, TOutput, TOutput>(ApplyKind.ZeroOrOne, leftParser, fnRightParser(default(LeftValue<TOutput>)));

        /// <summary>
        /// A left associative parser that yields the result of the left-hand parser, or the result of applying that value (and subsequent results) to the right-hand parser zero or more times.
        /// </summary>
        public static Parser<TInput, TOutput> ApplyZeroOrMore<TOutput>(Parser<TInput, TOutput> leftParser, Func<LeftValue<TOutput>, RightParser<TInput, TOutput>> fnRightParser) =>
            new ApplyParser<TInput, TOutput, TOutput>(ApplyKind.ZeroOrMore, leftParser, fnRightParser(default(LeftValue<TOutput>)));

        /// <summary>
        /// A parser that yields the result of the parser that consumed the most input items.
        /// </summary>
        public static Parser<TInput> Best(params Parser<TInput>[] parsers) =>
            new BestParser<TInput>(parsers);

        /// <summary>
        /// A parser that yields the result of the parser that consumed the most input items.
        /// </summary>
        public static Parser<TInput, TOutput> Best<TOutput>(params Parser<TInput, TOutput>[] parsers) =>
            new BestParser<TInput, TOutput>(parsers);

        /// <summary>
        /// A parser that yields the result of the parser that produced the best output item.
        /// </summary>
        public static Parser<TInput, TOutput> Best<TOutput>(Parser<TInput, TOutput>[] parsers, Func<TOutput, TOutput, bool> fnBetter) =>
            new BestParser<TInput, TOutput>(parsers, fnBetter);

        /// <summary>
        /// A parser that yields the result of the parser that consumed the most input items.
        /// </summary>
        public static RightParser<TInput, TOutput> Best<TOutput>(params RightParser<TInput, TOutput>[] parsers) =>
            new RightParser<TInput, TOutput>(new BestParser<TInput, TOutput>(parsers.Select(p => p.Parser).ToArray()));

        /// <summary>
        /// A parser that converts all the successfully scanned input items into a single output item.
        /// </summary>
        public static Parser<TInput, TOutput> Convert<TOutput>(Parser<TInput> pattern, SourceProducer<TInput, TOutput> producer) =>
            new ConvertParser<TInput, TOutput>(pattern, producer);

        /// <summary>
        /// A parser that converts all the successfully scanned input items into a single output item.
        /// </summary>
        public static Parser<TInput, TOutput> Convert<TOutput>(Parser<TInput> pattern, Func<IReadOnlyList<TInput>, TOutput> producer) =>
            new ConvertParser<TInput, TOutput>(pattern, producer);

        /// <summary>
        /// A parser that converts all the successfully scanned input characters into a single output item.
        /// </summary>
        public static Parser<char, TOutput> Convert<TOutput>(Parser<char> pattern, Func<string, TOutput> producer) =>
            Parsers<char>.Convert(pattern, (Source<char> source, int start, int length) =>
            {
                // check for TextSource to do it the easy way
                if (source is TextSource ts)
                {
                    return producer(ts.PeekText(start, length));
                }
                else
                {
                    // otherwise, do it the hard way
                    var builder = new System.Text.StringBuilder();

                    for (int i = 0; i < length; i++)
                    {
                        builder.Append(source.Peek(start + i));
                    }

                    return producer(builder.ToString());
                }
            });

        /// <summary>
        /// A parser that converts all the successfully scanned input items into a single output item.
        /// </summary>
        public static Parser<TInput, TOutput> Convert<TOutput>(Parser<TInput> pattern, Func<TInput, TOutput> producer) =>
            new ConvertParser<TInput, TOutput>(pattern, producer);

        /// <summary>
        /// A parser that converts all the successfully scanned input items into a single output item.
        /// </summary>
        public static Parser<TInput, TOutput> Convert<TOutput>(Parser<TInput> pattern, TOutput value) =>
            Convert(pattern, (TInput t) => value);

        /// <summary>
        /// A parser that produces the count of the number of successfully scanned and consumed input items.
        /// </summary>
        public static Parser<TInput, int> Count(Parser<TInput> scanner) =>
            Convert(scanner, (source, start, length) => length);

        /// <summary>
        /// A parser that succeeds (without consuming input) if the specified parser scan fails. Does not produce output.
        /// </summary>
        public static Parser<TInput> Fails(Parser<TInput> parser) =>
            new FailsParser<TInput>(parser);

        /// <summary>
        /// A parser that yields the result of the first parser to succeed.
        /// </summary>
        public static Parser<TInput, TOutput> First<TOutput>(params Parser<TInput, TOutput>[] parsers) =>
            new FirstParser<TInput, TOutput>(parsers);

        /// <summary>
        /// A parser that yields the result of the first parser to succeed.
        /// </summary>
        public static Parser<TInput> First(params Parser<TInput>[] parsers) =>
            new FirstParser<TInput>(parsers);

        /// <summary>
        /// A parser that yields the result of the first parser to succeed.
        /// </summary>
        public static RightParser<TInput, TOutput> First<TOutput>(params RightParser<TInput, TOutput>[] parsers) =>
            new RightParser<TInput, TOutput>(new FirstParser<TInput, TOutput>(parsers.Select(p => p.Parser).ToArray()));

        /// <summary>
        /// A parser that forwards to a deferred parser.
        /// This parser is typically used to resolve cycles in grammar.
        /// </summary>
        public static Parser<TInput, TOutput> Forward<TOutput>(Func<Parser<TInput, TOutput>> deferredParser) =>
            new ForwardParser<TInput, TOutput>(deferredParser);

        /// <summary>
        /// A parser that forwards to a deferred parser.
        /// This parser is typically used to resolve cycles in grammar.
        /// </summary>
        public static Parser<TInput, TOutput> Forward<TOutput>(Parser<TInput, TOutput> parser) =>
            new ForwardParser<TInput, TOutput>(() => parser);

        /// <summary>
        /// A parser that produces the result of the specified parser only if a scan of the test parser succeeds.
        /// </summary>
        public static Parser<TInput, TOutput> If<TOutput>(Parser<TInput> test, Parser<TInput, TOutput> parser) =>
            new IfParser<TInput, TOutput>(test, parser);

        /// <summary>
        /// A parser that produces the result of the specified parser only if a scan of the test parser succeeds.
        /// </summary>
        public static RightParser<TInput, TOutput> If<TOutput>(Parser<TInput> test, RightParser<TInput, TOutput> parser) =>
            new RightParser<TInput, TOutput>(new IfParser<TInput, TOutput>(test, parser.Parser));

        /// <summary>
        /// A parser that produces the result of the specified parser only if a scan of the test parser succeeds.
        /// </summary>
        public static Parser<TInput> If(Parser<TInput> test, Parser<TInput> parser) =>
            new IfParser<TInput>(test, parser);

        /// <summary>
        /// A parser that constrains the amount of input that can be accessed by another parser.
        /// </summary>
        public static Parser<TInput, TOutput> Limit<TOutput>(Parser<TInput> limiter, Parser<TInput, TOutput> limited) =>
            new LimitParser<TInput, TOutput>(limiter, limited);

        /// <summary>
        /// Creates a parser that parses a list of elements.
        /// </summary>
        /// <param name="elementParser">The parser for each element.</param>
        /// <param name="oneOrMore">If true, the generated parser expects at least one element to exist.</param>
        /// <param name="producer">A function that converts the series of elements into the produced value.</param>
        public static Parser<TInput, TProducer> List<TElement, TProducer>(
            Parser<TInput, TElement> elementParser,
            bool oneOrMore,
            Func<IReadOnlyList<TElement>, TProducer> producer)
        {
            return List(
                elementParser,
                fnMissingElement: null,
                oneOrMore: oneOrMore,
                producer: producer);
        }

        /// <summary>
        /// Creates a parser that parses a list of elements.
        /// </summary>
        /// <param name="elementParser">The parser for each element.</param>
        /// <param name="fnMissingElement">An optional function that constructs a new element to be used when an expected element is missing.</param>
        /// <param name="oneOrMore">If true, the generated parser expects at least one element to exist.</param>
        /// <param name="producer">A function that converts the series of elements into the produced value.</param>
        public static Parser<TInput, TProducer> List<TElement, TProducer>(
            Parser<TInput, TElement> elementParser,
            Func<Source<TInput>, int, TElement> fnMissingElement,
            bool oneOrMore,
            Func<IReadOnlyList<TElement>, TProducer> producer)
        {
            if (oneOrMore)
            {
                if (fnMissingElement != null)
                {
                    var requiredElement = Required(elementParser, fnMissingElement);

                    return Produce(
                        Sequence(
                            requiredElement,
                            ZeroOrMore(elementParser)),
                        producer);
                }
                else
                {
                    return OneOrMore(elementParser, producer);
                }
            }
            else
            {
                return ZeroOrMore(elementParser, producer);
            }
        }

        /// <summary>
        /// Creates a parser that parses a list of elements and separators.
        /// </summary>
        /// <param name="elementParser">The parser for each element.</param>
        /// <param name="separatorParser">The parser for each separator.</param>
        /// <param name="fnMissingElement">An optional function that constructs a new element to be used when the element is missing (between two separators).</param>
        /// <param name="fnMissingSeparator">An optional function that constructs a new separator instance to be used when the separator is missing (between two elements).</param>
        /// <param name="endOfList">An optional parser that quickly determines if there are no more elements.</param>
        /// <param name="oneOrMore">If true, the generated parser expects at least one element to exist.</param>
        /// <param name="allowTrailingSeparator">If true, it is legal for a final separator to occur without a following element.</param>
        /// <param name="producer">A function that converts the series of elements and separators into the produced value.</param>
        public static Parser<TInput, TProducer> OList<TElement, TSeparator, TProducer>(
            Parser<TInput, TElement> elementParser,
            Parser<TInput, TSeparator> separatorParser,
            Func<Source<TInput>, int, TElement> fnMissingElement,
            Func<Source<TInput>, int, TSeparator> fnMissingSeparator,
            Parser<TInput> endOfList,
            bool oneOrMore,
            bool allowTrailingSeparator,
            Func<IReadOnlyList<object>, TProducer> producer)
        {
            return OList(
                elementParser,
                separatorParser,
                elementParser,
                fnMissingElement,
                fnMissingSeparator,
                fnMissingElement,
                endOfList,
                oneOrMore,
                allowTrailingSeparator,
                producer);
        }

        /// <summary>
        /// Creates a parser that parses a list of elements and separators.
        /// </summary>
        /// <param name="primaryElementParser">The parser for the primary element.</param>
        /// <param name="separatorParser">The parser for each separator.</param>
        /// <param name="secondaryElementParser">The parser for any element after the first separator.</param>
        /// <param name="fnMissingPrimaryElement">An optional function that constructs a new element to be used when the primary element is missing (between two separators).</param>
        /// <param name="fnMissingSecondaryElement">An optional function that constructs a new element to be used when the secondary element is missing (between two separators).</param>
        /// <param name="fnMissingSeparator">An optional function that constructs a new separator instance to be used when the separator is missing (between two elements).</param>
        /// <param name="endOfList">An optional parser that quickly determines if there are not more elements.</param>
        /// <param name="oneOrMore">If true, the generated parser expects at least one element to exist.</param>
        /// <param name="allowTrailingSeparator">If true, it is legal for a final separator to occur without a following element.</param>
        /// <param name="producer">A function that converts the series of elements and separators into the produced value.</param>
        public static Parser<TInput, TProducer> OList<TElement, TSeparator, TProducer>(
            Parser<TInput, TElement> primaryElementParser,
            Parser<TInput, TSeparator> separatorParser,
            Parser<TInput, TElement> secondaryElementParser,
            Func<Source<TInput>, int, TElement> fnMissingPrimaryElement, // optional
            Func<Source<TInput>, int, TSeparator> fnMissingSeparator, // optional
            Func<Source<TInput>, int, TElement> fnMissingSecondaryElement, // optional
            Parser<TInput> endOfList, // optional
            bool oneOrMore,
            bool allowTrailingSeparator,
            Func<IReadOnlyList<object>, TProducer> producer)
        {
            Ensure.ArgumentNotNull(primaryElementParser, nameof(primaryElementParser));
            Ensure.ArgumentNotNull(separatorParser, nameof(separatorParser));

            if (secondaryElementParser == null)
                secondaryElementParser = primaryElementParser;

            var requiredPrimaryElementParser = fnMissingPrimaryElement != null ? Required(primaryElementParser, fnMissingPrimaryElement) : primaryElementParser;
            var requiredSecondaryElementParser = fnMissingSecondaryElement != null ? Required(secondaryElementParser, fnMissingSecondaryElement) : secondaryElementParser;
            var requiredSeparatorParser = fnMissingSeparator != null ? Required(separatorParser, fnMissingSeparator) : separatorParser;
            Func<TProducer> emptyList = () => producer(new object[] { });

            if (oneOrMore)
            {
                if (allowTrailingSeparator)
                {
                    var secondaryParser = Sequence(separatorParser, secondaryElementParser);

                    if (fnMissingSeparator != null && endOfList != null)
                    {
                        secondaryParser = First(
                            secondaryParser,
                            // check for missing secondardy element between two separators
                            If(Not(endOfList), Sequence(requiredSeparatorParser, secondaryElementParser)).Hide());
                    }

                    return Produce(
                        Sequence(
                            requiredPrimaryElementParser,
                            ZeroOrMore(secondaryParser),
                            Optional(separatorParser)),
                        producer);
                }
                else
                {
                    var secondaryParser = Sequence(separatorParser, requiredSecondaryElementParser);

                    if (fnMissingSeparator != null && endOfList != null)
                    {
                        secondaryParser = First(
                            secondaryParser,
                            // check for missing secondary element between two separators
                            If(Not(endOfList), Sequence(requiredSeparatorParser, secondaryElementParser)).Hide());
                    }

                    return Produce(
                        Sequence(
                            requiredPrimaryElementParser,
                            ZeroOrMore(secondaryParser)),
                        producer);
                }
            }
            else
            {
                if (allowTrailingSeparator)
                {
                    var secondaryParser = Sequence(separatorParser, secondaryElementParser);

                    if (fnMissingSeparator != null && endOfList != null)
                    {
                        secondaryParser = First(
                            secondaryParser,
                            // check for missing secondardy element between two separators
                            If(Not(endOfList), Sequence(requiredSeparatorParser, secondaryElementParser)).Hide());
                    }

                    return Optional(
                        Produce(
                            Sequence(
                                First(
                                    If(separatorParser, requiredPrimaryElementParser).Hide(), // check for missing primary element
                                    primaryElementParser),
                                ZeroOrMore(secondaryParser),
                                Optional(separatorParser)),
                            producer),
                        emptyList);
                }
                else
                {
                    var secondaryParser = Sequence(separatorParser, requiredSecondaryElementParser);

                    if (fnMissingSeparator != null && endOfList != null)
                    {
                        secondaryParser = First(
                            secondaryParser,
                            // check for missing secondardy element between two separators
                            If(Not(endOfList), Sequence(requiredSeparatorParser, secondaryElementParser)).Hide());
                    }

                    return Optional(
                        Produce(
                            Sequence(
                                First(
                                    If(separatorParser, requiredPrimaryElementParser).Hide(), // check for missing primary element
                                    primaryElementParser),
                                ZeroOrMore(secondaryParser)),
                            producer),
                        emptyList);
                }
            }
        }

        /// <summary>
        /// Creates a parser that parses a list of elements and separators.
        /// </summary>
        /// <param name="elementParser">The parser for the primary element.</param>
        /// <param name="separatorParser">The parser for each separator.</param>
        /// <param name="secondaryElementParser">The parser for any elements after the first separator.</param>
        /// <param name="fnMissingElement">An optional function that constructs a new element to be used when the element is missing (between two separators).</param>
        /// <param name="fnMissingSeparator">An optional function that constructs a new separator instance to be used when the separator is missing (between two elements).</param>
        /// <param name="fnMissingSecondaryElement">An optional function that constructs a new element to be used when a second element is missing (between two separators).</param>
        /// <param name="endOfList">An optional parser that quickly determines if there are not more elements.</param>
        /// <param name="oneOrMore">If true, the generated parser expects at least one element to exist.</param>
        /// <param name="allowTrailingSeparator">If true, it is legal for a final separator to occur without a following element.</param>
        /// <param name="producer">A function that converts the series of elements and separators into the produced value.</param>
        public static Parser<TInput, TProducer> List<TElement, TSeparator, TProducer>(
            Parser<TInput, TElement> elementParser,
            Parser<TInput, TSeparator> separatorParser,
            Parser<TInput, TElement> secondaryElementParser,
            Func<Source<TInput>, int, TElement> fnMissingElement, // optional
            Func<Source<TInput>, int, TSeparator> fnMissingSeparator, // optional
            Func<Source<TInput>, int, TElement> fnMissingSecondaryElement, // optional
            Parser<TInput> endOfList, // optional
            bool oneOrMore,
            bool allowTrailingSeparator,
            Func<IReadOnlyList<ElementAndSeparator<TElement, TSeparator>>, TProducer> producer)
        {
            return OList(
                elementParser,
                separatorParser,
                secondaryElementParser,
                fnMissingElement,
                fnMissingSeparator,
                fnMissingSecondaryElement,
                endOfList,
                oneOrMore,
                allowTrailingSeparator,
                list => ElementAndSeparatorProducer<TElement, TSeparator, TProducer>.Produce(list, producer)
                );
        }

        /// <summary>
        /// Creates a parser that parses a list of elements and separators.
        /// </summary>
        /// <param name="elementParser">The parser for the primary element.</param>
        /// <param name="separatorParser">The parser for the separator.</param>
        /// <param name="fnMissingElement">An optional function that constructs a new element to be used when the element is missing (between two separators).</param>
        /// <param name="fnMissingSeparator">An optional function that constructs a new separator instance to be used when the separator is missing (between two elements).</param>
        /// <param name="endOfList">An optional parser that quickly determines if there are not more elements.</param>
        /// <param name="oneOrMore">If true, the generated parser expects at least one element to exist.</param>
        /// <param name="allowTrailingSeparator">If true, it is legal for a final separator to occur without a following element.</param>
        /// <param name="producer">A function that converts the series of elements and separators into the produced value.</param>
        public static Parser<TInput, TProducer> List<TElement, TSeparator, TProducer>(
            Parser<TInput, TElement> elementParser,
            Parser<TInput, TSeparator> separatorParser,
            Func<Source<TInput>, int, TElement> fnMissingElement, // optional
            Func<Source<TInput>, int, TSeparator> fnMissingSeparator, // optional
            Parser<TInput> endOfList, // optional
            bool oneOrMore,
            bool allowTrailingSeparator,
            Func<IReadOnlyList<ElementAndSeparator<TElement, TSeparator>>, TProducer> producer)
        {
            return List(
                elementParser,
                separatorParser,
                elementParser,
                fnMissingElement,
                fnMissingSeparator,
                fnMissingElement,
                endOfList,
                oneOrMore,
                allowTrailingSeparator,
                producer);
        }

        /// <summary>
        /// Creates a parser that parses a list of elements and separators.
        /// </summary>
        /// <param name="elementParser">The parser for the primary element.</param>
        /// <param name="separatorParser">The parser for the separator.</param>
        /// <param name="secondaryElementParser">The parser for any elements after the first separator.</param>
        /// <param name="oneOrMore">If true, the generated parser expects at least one element to exist.</param>
        /// <param name="producer">A function that converts the series of elements and separators into the produced value.</param>
        public static Parser<TInput, TProducer> List<TElement, TSeparator, TProducer>(
            Parser<TInput, TElement> elementParser,
            Parser<TInput, TSeparator> separatorParser,
            Parser<TInput, TElement> secondaryElementParser,
            bool oneOrMore,
            Func<IReadOnlyList<ElementAndSeparator<TElement, TSeparator>>, TProducer> producer)
        {
            return List(
                elementParser,
                separatorParser,
                secondaryElementParser,
                fnMissingElement: null,
                fnMissingSeparator: null,
                fnMissingSecondaryElement: null,
                endOfList: null,
                oneOrMore: oneOrMore,
                allowTrailingSeparator: false,
                producer: producer);
        }

        /// <summary>
        /// Creates a parser that parses a list of elements and separators.
        /// </summary>
        /// <param name="elementParser">The parser for the primary element.</param>
        /// <param name="separatorParser">The parser for the separator.</param>
        /// <param name="oneOrMore">If true, the generated parser expects at least one element to exist.</param>
        /// <param name="producer">A function that converts the series of elements and separators into the produced value.</param>
        public static Parser<TInput, TProducer> List<TElement, TSeparator, TProducer>(
            Parser<TInput, TElement> elementParser,
            Parser<TInput, TSeparator> separatorParser,
            bool oneOrMore,
            Func<IReadOnlyList<ElementAndSeparator<TElement, TSeparator>>, TProducer> producer)
        {
            return List(
                elementParser,
                separatorParser,
                elementParser,
                fnMissingElement: null,
                fnMissingSeparator: null,
                fnMissingSecondaryElement: null,
                endOfList: null,
                oneOrMore: oneOrMore,
                allowTrailingSeparator: false,
                producer: producer);
        }

        private class ElementAndSeparatorProducer<TElement, TSeparator, TProducer>
        {
            private static readonly ObjectPool<List<ElementAndSeparator<TElement, TSeparator>>> listPool =
                new ObjectPool<List<ElementAndSeparator<TElement, TSeparator>>>(
                    () => new List<ElementAndSeparator<TElement, TSeparator>>(), list => list.Clear());

            public static TProducer Produce(IReadOnlyList<object> output, Func<IReadOnlyList<ElementAndSeparator<TElement, TSeparator>>, TProducer> producer)
            {
                var list = listPool.AllocateFromPool();
                try
                {
                    for (int i = 0; i < output.Count; i += 2)
                    {
                        var element = (TElement)output[i];
                        var separator = (i < output.Count - 1) ? (TSeparator)output[i + 1] : default(TSeparator);
                        list.Add(new ElementAndSeparator<TElement, TSeparator>(element, separator));
                    }

                    return producer(list);
                }
                finally
                {
                    listPool.ReturnToPool(list);
                }
            }
        }

        /// <summary>
        /// A parser that maps sequences of input values to output values.
        /// </summary>
        public static Parser<TInput, TOutput> Map<TOutput>(IEnumerable<KeyValuePair<IEnumerable<TInput>, TOutput>> keyValuePairs) =>
            new MapParser<TInput, TOutput>(keyValuePairs.ToDictionary(kvp => kvp.Key, kvp => (Func<TOutput>)(() => kvp.Value)));

        /// <summary>
        /// A parser that maps sequences of input values to output values.
        /// </summary>
        public static Parser<TInput, TOutput> Map<TOutput>(IEnumerable<KeyValuePair<IEnumerable<TInput>, Func<TOutput>>> keyValuePairs) =>
            new MapParser<TInput, TOutput>(keyValuePairs);

        /// <summary>
        /// A parser that consumes one input item if it matches the predicate. Does not produce output.
        /// </summary>
        public static Parser<TInput> Match(Func<TInput, bool> predicate) =>
            new MatchParser<TInput>(predicate);

        /// <summary>
        /// A parser that consumes one or more input items.
        /// </summary>
        public static Parser<TInput> Match(SourceConsumer<TInput> consumer) =>
            new MatchParser<TInput>(consumer);

        /// <summary>
        /// A parser that consumes one matching input item.
        /// </summary>
        public static Parser<TInput> Match(TInput item) =>
            Match(item, EqualityComparer<TInput>.Default);

        /// <summary>
        /// A parser that consumes one matching input item.
        /// </summary>
        public static Parser<TInput> Match(TInput item, EqualityComparer<TInput> comparer) =>
            Match(i => comparer.Equals(i, item));

        /// <summary>
        /// A parser that consumes one input item that matches one of the specified items
        /// </summary>
        public static Parser<TInput> MatchAny(IReadOnlyList<TInput> items) =>
            MatchAny(items, EqualityComparer<TInput>.Default);

        /// <summary>
        /// A parser that consumes one input item that matches one of the specified items
        /// </summary>
        public static Parser<TInput> MatchAny(params TInput[] items) =>
            MatchAny(items, EqualityComparer<TInput>.Default);

        /// <summary>
        /// A parser that consumes one input item that matches one of the specified items
        /// </summary>
        public static Parser<TInput> MatchAny(IReadOnlyList<TInput> items, EqualityComparer<TInput> comparer)
        {
            var hashset = new HashSet<TInput>(items, comparer);
            return Match((TInput item) => items.Contains(item));
        }

        /// <summary>
        /// A parser that consumes one or more sequential matching input items.
        /// </summary>
        public static Parser<TInput> MatchSequence(IReadOnlyList<TInput> items) =>
            MatchSequence(items, EqualityComparer<TInput>.Default);

        /// <summary>
        /// A parser that consumes one or more sequential matching input items.
        /// </summary>
        public static Parser<TInput> MatchSequence(IReadOnlyList<TInput> items, EqualityComparer<TInput> comparer) =>
            new MatchParser<TInput>(
                (source, start) =>
                {
                    for (int i = 0; i < items.Count; i++)
                    {
                        if (source.IsEnd(start + i))
                            return ~i;

                        if (!comparer.Equals(items[i], source.Peek(start + i)))
                            return ~i;
                    }

                    return items.Count;
                });

        /// <summary>
        /// A parser that consumes a matching character.
        /// </summary>
        public static Parser<char> Match(char ch, bool ignoreCase = false)
        {
            if (ignoreCase)
            {
                var chUpper = char.ToUpper(ch);
                var chLower = char.ToLower(ch);
                return Parsers<char>.Match(c => c == ch || c == chUpper || c == chLower);
            }
            else
            {
                return Parsers<char>.Match(c => c == ch);
            }
        }

        /// <summary>
        /// A parser that consumes one or more sequential matching characters.
        /// </summary>
        public static Parser<char> Match(string text, bool ignoreCase = false)
        {
            if (text.Length == 1)
            {
                return Match(text[0], ignoreCase);
            }

            if (ignoreCase)
            {
                var lower = text.ToLower();
                var upper = text.ToUpper();

                return Parsers<char>.Match((source, start) =>
                {
                    // check for quick string comparison
                    if (source is TextSource ts)
                    {
                        return ts.Matches(start, text, ignoreCase: true) ? text.Length : -1;
                    }
                    else
                    {
                        // otherwise do it the hard way
                        for (int i = 0; i < text.Length; i++)
                        {
                            var ch = source.Peek(start + i);
                            if (ch != lower[i] && ch != upper[i])
                                return ~i;
                        }

                        return text.Length;
                    }
                });
            }
            else
            {
                return Parsers<char>.Match((source, start) =>
                {
                    // check for quick string comparison
                    if (source is TextSource ts)
                    {
                        return ts.Matches(start, text) ? text.Length : -1;
                    }
                    else
                    {
                        // otherwise do it the hard way
                        for (int i = 0; i < text.Length; i++)
                        {
                            if (source.Peek(start + i) != text[i])
                                return ~i;
                        }

                        return text.Length;
                    }
                });
            }
        }

        /// <summary>
        /// A parser that producers a single output value from a single input value if it matches the predicate.
        /// </summary>
        public static Parser<TInput, TOutput> Match<TOutput>(Func<TInput, bool> predicate, Func<TInput, TOutput> producer) =>
            new MatchParser<TInput, TOutput>(predicate, producer);

        /// <summary>
        /// A parser that producers a single output value from one or more input values.
        /// </summary>
        public static Parser<TInput, TOutput> Match<TOutput>(SourceConsumer<TInput> consumer, SourceProducer<TInput, TOutput> producer) =>
            new MatchParser<TInput, TOutput>(consumer, producer);

        /// <summary>
        /// A parser that consumes one input item if the specified parser scan fails. Does not produce output.
        /// </summary>
        public static Parser<TInput> Not(Parser<TInput> parser) =>
            new NotParser<TInput>(parser);

        /// <summary>
        /// A parser that combines one or more parsed values into a single value.
        /// </summary>
        public static Parser<TInput, TProducer> OneOrMore<TParser, TProducer>(
            Parser<TInput, TParser> parser,
            Func<IReadOnlyList<TParser>, TProducer> producer)
            =>
            Produce(OneOrMore(parser), producer);

        /// <summary>
        /// A parser that combines one or more parsed values into a single value.
        /// </summary>
        public static Parser<TInput, IReadOnlyList<TParser>> OneOrMoreList<TParser>(
            Parser<TInput, TParser> parser)
            =>
            OneOrMore(parser, list => list.ToReadOnly());

        /// <summary>
        /// A parser that parsers one or more values from the specified parser.
        /// </summary>
        public static Parser<TInput> OneOrMore(Parser<TInput> parser) =>
            new OneOrMoreParser<TInput>(parser);

        /// <summary>
        /// A parser that produces the specified parser's result or nothing if the specified parser fails.
        /// </summary>
        public static Parser<TInput> Optional(Parser<TInput> parser) =>
            new ZeroOrMoreParser<TInput>(parser, zeroOrOne: true);

        /// <summary>
        /// A parser that produces the specified parser's result or the default value if the specified parser fails.
        /// </summary>
        public static Parser<TInput, TOutput> Optional<TOutput>(Parser<TInput, TOutput> parser) =>
            new OptionalParser<TInput, TOutput>(parser, () => default(TOutput));

        /// <summary>
        /// A parser that produces the specified parser's result or the value from the producer function if the specified parser fails.
        /// </summary>
        public static Parser<TInput, TOutput> Optional<TOutput>(Parser<TInput, TOutput> parser, Func<TOutput> producer) =>
            new OptionalParser<TInput, TOutput>(parser, producer);

        /// <summary>
        /// A parser that succeeds if any of the specified parsers succeed, consuming the greatest number of input items consumed.
        /// This parser is a synonym for <see cref="Best(Parser{TInput}[])"/>.
        /// </summary>
        public static Parser<TInput> Or(params Parser<TInput>[] parsers) =>
            new BestParser<TInput>(parsers);

        /// <summary>
        /// A parser that combines one or more values produced by a single parser into a new value.
        /// </summary>
        public static Parser<TInput, TOutput> Produce<TOutput>(Parser<TInput> parser, Func<IReadOnlyList<object>, TOutput> producer) =>
            new ProduceParser<TInput, TOutput>(parser, Produce(producer));

        /// <summary>
        /// A parser that combines one or more values produced by a single parser into a new value.
        /// </summary>
        public static Parser<TInput, TOutput> Produce<TElement, TOutput>(Parser<TInput> parser, Func<IReadOnlyList<TElement>, TOutput> producer) =>
            new ProduceParser<TInput, TOutput>(parser, Produce(producer));

        private static Func<List<object>, int, TOutput> Produce<TElement, TOutput>(Func<IReadOnlyList<TElement>, TOutput> producer)
        {
            return (list, start) => ElementProducer<TElement, TOutput>.Produce(list, start, producer);
        }

        private class ElementProducer<TElement, TProducer>
        {
            private static readonly ObjectPool<List<TElement>> listPool =
                new ObjectPool<List<TElement>>(() => new List<TElement>(), list => list.Clear());

            public static TProducer Produce(List<object> output, int outputStart, Func<IReadOnlyList<TElement>, TProducer> producer)
            {
                var list = listPool.AllocateFromPool();
                try
                {
                    for (int i = outputStart; i < output.Count; i++)
                    {
                        list.Add((TElement)output[i]);
                    }

                    return producer(list);
                }
                finally
                {
                    listPool.ReturnToPool(list);
                }
            }
        }

        /// <summary>
        /// A parser that produces the specified parsers result or the value of the producer.
        /// This parser behaves like optional, except that a producer must be specified and has UI behavior differences.
        /// </summary>
        public static Parser<TInput, TOutput> Required<TOutput>(Parser<TInput, TOutput> parser, Func<TOutput> producer) =>
            new RequiredParser<TInput, TOutput>(parser, (source, start) => producer());

        /// <summary>
        /// A parser that produces the specified parsers result or the value of the producer.
        /// This parser behaves like optional, except that a producer must be specified and has UI behavior differences.
        /// </summary>
        public static Parser<TInput, TOutput> Required<TOutput>(Parser<TInput, TOutput> parser, Func<Source<TInput>, int, TOutput> producer) =>
            new RequiredParser<TInput, TOutput>(parser, producer);

        /// <summary>
        /// A parser that converts the output of one parser into a new value.
        /// If the specified parser fails, then this parser fails too.
        /// </summary>
        public static Parser<TInput, TOutput> Rule<TParser1, TOutput>(
            Parser<TInput, TParser1> parser1,
            Func<TParser1, TOutput> producer)
            =>
            new RuleParser<TInput, TOutput>(
                new Parser<TInput>[] { parser1 },
                
                (list, start) =>
                {
                    Ensure.AreEqual(list.Count - start, 1);
                    return producer((TParser1)list[start]);
                },

                (source, start) =>
                {
                    var result1 = parser1.Parse(source, start);

                    if (result1.Length < 0)
                        return new ParseResult<TOutput>(result1.Length, default(TOutput));

                    var produced = producer(result1.Value);
                    return new ParseResult<TOutput>(result1.Length, produced);
                });

        /// <summary>
        /// A parser that combines the output of multiple parsers into a new value.
        /// If either parser fails, then this parser fails.
        /// </summary>
        public static Parser<TInput, TOutput> Rule<TParser1, TParser2, TOutput>(
            Parser<TInput, TParser1> parser1,
            Parser<TInput, TParser2> parser2,
            Func<TParser1, TParser2, TOutput> producer)
            =>
            new RuleParser<TInput, TOutput>(
                new Parser<TInput>[] { parser1, parser2 },

                (list, start) =>
                {
                    Ensure.AreEqual(list.Count - start, 2);
                    return producer(
                        (TParser1)list[start],
                        (TParser2)list[start + 1]);
                },
                
                (source, start) =>
                {
                    var result1 = parser1.Parse(source, start);
                    if (result1.Length < 0)
                        return new ParseResult<TOutput>(result1.Length, default(TOutput));
                    var len = result1.Length;

                    var result2 = parser2.Parse(source, start + len);
                    if (result2.Length < 0)
                        return new ParseResult<TOutput>(-len + result2.Length, default(TOutput));
                    len += result2.Length;

                    var produced = producer(result1.Value, result2.Value);
                    return new ParseResult<TOutput>(len, produced);
                });

        /// <summary>
        /// A parser that combines the output of multiple parsers into a new value.
        /// If any parser in the sequence fails, then this parser fails.
        /// </summary>
        public static Parser<TInput, TOutput> Rule<TParser1, TParser2, TParser3, TOutput>(
            Parser<TInput, TParser1> parser1,
            Parser<TInput, TParser2> parser2,
            Parser<TInput, TParser3> parser3,
            Func<TParser1, TParser2, TParser3, TOutput> producer)
            =>
            new RuleParser<TInput, TOutput>(
                new Parser<TInput>[] { parser1, parser2, parser3 },

                (list, start) =>
                {
                    Ensure.AreEqual(list.Count - start, 3);
                    return producer(
                        (TParser1)list[start],
                        (TParser2)list[start + 1],
                        (TParser3)list[start + 2]);
                },
                
                (source, start) =>
                {
                    var result1 = parser1.Parse(source, start);
                    if (result1.Length < 0)
                        return new ParseResult<TOutput>(result1.Length, default(TOutput));
                    var len = result1.Length;

                    var result2 = parser2.Parse(source, start + len);
                    if (result2.Length < 0)
                        return new ParseResult<TOutput>(-len + result2.Length, default(TOutput));
                    len += result2.Length;

                    var result3 = parser3.Parse(source, start + len);
                    if (result3.Length < 0)
                        return new ParseResult<TOutput>(-len + result3.Length, default(TOutput));
                    len += result3.Length;

                    var produced = producer(result1.Value, result2.Value, result3.Value);
                    return new ParseResult<TOutput>(len, produced);
                });

        /// <summary>
        /// A parser that combines the output of multiple parsers into a new value.
        /// If any parser in the sequence fails, then this parser fails.
        /// </summary>
        public static Parser<TInput, TOutput> Rule<TParser1, TParser2, TParser3, TParser4, TOutput>(
            Parser<TInput, TParser1> parser1,
            Parser<TInput, TParser2> parser2,
            Parser<TInput, TParser3> parser3,
            Parser<TInput, TParser4> parser4,
            Func<TParser1, TParser2, TParser3, TParser4, TOutput> producer)
            =>
            new RuleParser<TInput, TOutput>(
                new Parser<TInput>[] { parser1, parser2, parser3, parser4 },

                (list, start) =>
                {
                    Ensure.AreEqual(list.Count - start, 4);
                    return producer(
                        (TParser1)list[start],
                        (TParser2)list[start + 1],
                        (TParser3)list[start + 2],
                        (TParser4)list[start + 3]);
                },
                
                (source, start) =>
                {
                    var result1 = parser1.Parse(source, start);
                    if (result1.Length < 0)
                        return new ParseResult<TOutput>(result1.Length, default(TOutput));
                    var len = result1.Length;

                    var result2 = parser2.Parse(source, start + len);
                    if (result2.Length < 0)
                        return new ParseResult<TOutput>(-len + result2.Length, default(TOutput));
                    len += result2.Length;

                    var result3 = parser3.Parse(source, start + len);
                    if (result3.Length < 0)
                        return new ParseResult<TOutput>(-len + result3.Length, default(TOutput));
                    len += result3.Length;

                    var result4 = parser4.Parse(source, start + len);
                    if (result4.Length < 0)
                        return new ParseResult<TOutput>(-len + result4.Length, default(TOutput));
                    len += result4.Length;

                    var produced = producer(result1.Value, result2.Value, result3.Value, result4.Value);
                    return new ParseResult<TOutput>(len, produced);
                });

        /// <summary>
        /// A parser that combines the output of multiple parsers into a new value.
        /// If any parser in the sequence fails, then this parser fails.
        /// </summary>
        public static Parser<TInput, TOutput> Rule<TParser1, TParser2, TParser3, TParser4, TParser5, TOutput>(
            Parser<TInput, TParser1> parser1,
            Parser<TInput, TParser2> parser2,
            Parser<TInput, TParser3> parser3,
            Parser<TInput, TParser4> parser4,
            Parser<TInput, TParser5> parser5,
            Func<TParser1, TParser2, TParser3, TParser4, TParser5, TOutput> producer)
            =>
            new RuleParser<TInput, TOutput>(
                new Parser<TInput>[] { parser1, parser2, parser3, parser4, parser5 },

                (list, start) =>
                {
                    Ensure.AreEqual(list.Count - start, 5);
                    return producer(
                        (TParser1)list[start],
                        (TParser2)list[start + 1],
                        (TParser3)list[start + 2],
                        (TParser4)list[start + 3],
                        (TParser5)list[start + 4]);
                },
                
                (source, start) =>
                {
                    var result1 = parser1.Parse(source, start);
                    if (result1.Length < 0)
                        return new ParseResult<TOutput>(result1.Length, default(TOutput));
                    var len = result1.Length;

                    var result2 = parser2.Parse(source, start + len);
                    if (result2.Length < 0)
                        return new ParseResult<TOutput>(-len + result2.Length, default(TOutput));
                    len += result2.Length;

                    var result3 = parser3.Parse(source, start + len);
                    if (result3.Length < 0)
                        return new ParseResult<TOutput>(-len + result3.Length, default(TOutput));
                    len += result3.Length;

                    var result4 = parser4.Parse(source, start + len);
                    if (result4.Length < 0)
                        return new ParseResult<TOutput>(-len + result4.Length, default(TOutput));
                    len += result4.Length;

                    var result5 = parser5.Parse(source, start + len);
                    if (result5.Length < 0)
                        return new ParseResult<TOutput>(-len + result5.Length, default(TOutput));
                    len += result5.Length;

                    var produced = producer(result1.Value, result2.Value, result3.Value, result4.Value, result5.Value);
                    return new ParseResult<TOutput>(len, produced);
                });

        /// <summary>
        /// A parser that combines the output of multiple parsers into a new value.
        /// If any parser in the sequence fails, then this parser fails.
        /// </summary>
        public static Parser<TInput, TOutput> Rule<TParser1, TParser2, TParser3, TParser4, TParser5, TParser6, TOutput>(
            Parser<TInput, TParser1> parser1,
            Parser<TInput, TParser2> parser2,
            Parser<TInput, TParser3> parser3,
            Parser<TInput, TParser4> parser4,
            Parser<TInput, TParser5> parser5,
            Parser<TInput, TParser6> parser6,
            Func<TParser1, TParser2, TParser3, TParser4, TParser5, TParser6, TOutput> producer)
            =>
            new RuleParser<TInput, TOutput>(
                new Parser<TInput>[] { parser1, parser2, parser3, parser4, parser5, parser6 },

                (list, start) =>
                {
                    Ensure.AreEqual(list.Count - start, 6);
                    return producer(
                        (TParser1)list[start],
                        (TParser2)list[start + 1],
                        (TParser3)list[start + 2],
                        (TParser4)list[start + 3],
                        (TParser5)list[start + 4],
                        (TParser6)list[start + 5]);
                },
                
                (source, start) =>
                {
                    var result1 = parser1.Parse(source, start);
                    if (result1.Length < 0)
                        return new ParseResult<TOutput>(result1.Length, default(TOutput));
                    var len = result1.Length;

                    var result2 = parser2.Parse(source, start + len);
                    if (result2.Length < 0)
                        return new ParseResult<TOutput>(-len + result2.Length, default(TOutput));
                    len += result2.Length;

                    var result3 = parser3.Parse(source, start + len);
                    if (result3.Length < 0)
                        return new ParseResult<TOutput>(-len + result3.Length, default(TOutput));
                    len += result3.Length;

                    var result4 = parser4.Parse(source, start + len);
                    if (result4.Length < 0)
                        return new ParseResult<TOutput>(-len + result4.Length, default(TOutput));
                    len += result4.Length;

                    var result5 = parser5.Parse(source, start + len);
                    if (result5.Length < 0)
                        return new ParseResult<TOutput>(-len + result5.Length, default(TOutput));
                    len += result5.Length;

                    var result6 = parser6.Parse(source, start + len);
                    if (result6.Length < 0)
                        return new ParseResult<TOutput>(-len + result6.Length, default(TOutput));
                    len += result6.Length;

                    var produced = producer(result1.Value, result2.Value, result3.Value, result4.Value, result5.Value, result6.Value);
                    return new ParseResult<TOutput>(len, produced);
                });

        /// <summary>
        /// A parser that combines the output of multiple parsers into a new value.
        /// If any parser in the sequence fails, then this parser fails.
        /// </summary>
        public static Parser<TInput, TOutput> Rule<TParser1, TParser2, TParser3, TParser4, TParser5, TParser6, TParser7, TOutput>(
            Parser<TInput, TParser1> parser1,
            Parser<TInput, TParser2> parser2,
            Parser<TInput, TParser3> parser3,
            Parser<TInput, TParser4> parser4,
            Parser<TInput, TParser5> parser5,
            Parser<TInput, TParser6> parser6,
            Parser<TInput, TParser7> parser7,
            Func<TParser1, TParser2, TParser3, TParser4, TParser5, TParser6, TParser7, TOutput> producer)
            =>
            new RuleParser<TInput, TOutput>(
                new Parser<TInput>[] { parser1, parser2, parser3, parser4, parser5, parser6, parser7 },

                (list, start) =>
                {
                    Ensure.AreEqual(list.Count - start, 7);
                    return producer(
                        (TParser1)list[start],
                        (TParser2)list[start + 1],
                        (TParser3)list[start + 2],
                        (TParser4)list[start + 3],
                        (TParser5)list[start + 4],
                        (TParser6)list[start + 5],
                        (TParser7)list[start + 6]);
                },

                (source, start) =>
                {
                    var result1 = parser1.Parse(source, start);
                    if (result1.Length < 0)
                        return new ParseResult<TOutput>(result1.Length, default(TOutput));
                    var len = result1.Length;

                    var result2 = parser2.Parse(source, start + len);
                    if (result2.Length < 0)
                        return new ParseResult<TOutput>(-len + result2.Length, default(TOutput));
                    len += result2.Length;

                    var result3 = parser3.Parse(source, start + len);
                    if (result3.Length < 0)
                        return new ParseResult<TOutput>(-len + result3.Length, default(TOutput));
                    len += result3.Length;

                    var result4 = parser4.Parse(source, start + len);
                    if (result4.Length < 0)
                        return new ParseResult<TOutput>(-len + result4.Length, default(TOutput));
                    len += result4.Length;

                    var result5 = parser5.Parse(source, start + len);
                    if (result5.Length < 0)
                        return new ParseResult<TOutput>(-len + result5.Length, default(TOutput));
                    len += result5.Length;

                    var result6 = parser6.Parse(source, start + len);
                    if (result6.Length < 0)
                        return new ParseResult<TOutput>(-len + result6.Length, default(TOutput));
                    len += result6.Length;

                    var result7 = parser7.Parse(source, start + len);
                    if (result7.Length < 0)
                        return new ParseResult<TOutput>(-len + result7.Length, default(TOutput));
                    len += result7.Length;

                    var produced = producer(result1.Value, result2.Value, result3.Value, result4.Value, result5.Value, result6.Value, result7.Value);
                    return new ParseResult<TOutput>(len, produced);
                });

        /// <summary>
        /// A parser that combines the output of multiple parsers into a new value.
        /// If any parser in the sequence fails, then this parser fails.
        /// </summary>
        public static Parser<TInput, TOutput> Rule<TParser1, TParser2, TParser3, TParser4, TParser5, TParser6, TParser7, TParser8, TOutput>(
            Parser<TInput, TParser1> parser1,
            Parser<TInput, TParser2> parser2,
            Parser<TInput, TParser3> parser3,
            Parser<TInput, TParser4> parser4,
            Parser<TInput, TParser5> parser5,
            Parser<TInput, TParser6> parser6,
            Parser<TInput, TParser7> parser7,
            Parser<TInput, TParser8> parser8,
            Func<TParser1, TParser2, TParser3, TParser4, TParser5, TParser6, TParser7, TParser8, TOutput> producer)
            =>
            new RuleParser<TInput, TOutput>(
                new Parser<TInput>[] { parser1, parser2, parser3, parser4, parser5, parser6, parser7, parser8 },

                (list, start) =>
                {
                    Ensure.AreEqual(list.Count - start, 8);
                    return producer(
                        (TParser1)list[start],
                        (TParser2)list[start + 1],
                        (TParser3)list[start + 2],
                        (TParser4)list[start + 3],
                        (TParser5)list[start + 4],
                        (TParser6)list[start + 5],
                        (TParser7)list[start + 6],
                        (TParser8)list[start + 7]);
                },
                
                (source, start) =>
                {
                    var result1 = parser1.Parse(source, start);
                    if (result1.Length < 0)
                        return new ParseResult<TOutput>(result1.Length, default(TOutput));
                    var len = result1.Length;

                    var result2 = parser2.Parse(source, start + len);
                    if (result2.Length < 0)
                        return new ParseResult<TOutput>(-len + result2.Length, default(TOutput));
                    len += result2.Length;

                    var result3 = parser3.Parse(source, start + len);
                    if (result3.Length < 0)
                        return new ParseResult<TOutput>(-len + result3.Length, default(TOutput));
                    len += result3.Length;

                    var result4 = parser4.Parse(source, start + len);
                    if (result4.Length < 0)
                        return new ParseResult<TOutput>(-len + result4.Length, default(TOutput));
                    len += result4.Length;

                    var result5 = parser5.Parse(source, start + len);
                    if (result5.Length < 0)
                        return new ParseResult<TOutput>(-len + result5.Length, default(TOutput));
                    len += result5.Length;

                    var result6 = parser6.Parse(source, start + len);
                    if (result6.Length < 0)
                        return new ParseResult<TOutput>(-len + result6.Length, default(TOutput));
                    len += result6.Length;

                    var result7 = parser7.Parse(source, start + len);
                    if (result7.Length < 0)
                        return new ParseResult<TOutput>(-len + result7.Length, default(TOutput));
                    len += result7.Length;

                    var result8 = parser8.Parse(source, start + len);
                    if (result8.Length < 0)
                        return new ParseResult<TOutput>(-len + result8.Length, default(TOutput));
                    len += result8.Length;

                    var produced = producer(result1.Value, result2.Value, result3.Value, result4.Value, result5.Value, result6.Value, result7.Value, result8.Value);
                    return new ParseResult<TOutput>(len, produced);
                });

        /// <summary>
        /// A parser that combines the output of multiple parsers into a new value.
        /// If any parser in the sequence fails, then this parser fails.
        /// </summary>
        public static Parser<TInput, TOutput> Rule<TParser1, TParser2, TParser3, TParser4, TParser5, TParser6, TParser7, TParser8, TParser9, TOutput>(
            Parser<TInput, TParser1> parser1,
            Parser<TInput, TParser2> parser2,
            Parser<TInput, TParser3> parser3,
            Parser<TInput, TParser4> parser4,
            Parser<TInput, TParser5> parser5,
            Parser<TInput, TParser6> parser6,
            Parser<TInput, TParser7> parser7,
            Parser<TInput, TParser8> parser8,
            Parser<TInput, TParser9> parser9,
            Func<TParser1, TParser2, TParser3, TParser4, TParser5, TParser6, TParser7, TParser8, TParser9, TOutput> producer)
            =>
            new RuleParser<TInput, TOutput>(
                new Parser<TInput>[] { parser1, parser2, parser3, parser4, parser5, parser6, parser7, parser8, parser9 },

                (list, start) =>
                {
                    Ensure.AreEqual(list.Count - start, 9);
                    return producer(
                        (TParser1)list[start],
                        (TParser2)list[start + 1],
                        (TParser3)list[start + 2],
                        (TParser4)list[start + 3],
                        (TParser5)list[start + 4],
                        (TParser6)list[start + 5],
                        (TParser7)list[start + 6],
                        (TParser8)list[start + 7],
                        (TParser9)list[start + 8]);
                },
                
                (source, start) =>
                {
                    var result1 = parser1.Parse(source, start);
                    if (result1.Length < 0)
                        return new ParseResult<TOutput>(result1.Length, default(TOutput));
                    var len = result1.Length;

                    var result2 = parser2.Parse(source, start + len);
                    if (result2.Length < 0)
                        return new ParseResult<TOutput>(-len + result2.Length, default(TOutput));
                    len += result2.Length;

                    var result3 = parser3.Parse(source, start + len);
                    if (result3.Length < 0)
                        return new ParseResult<TOutput>(-len + result3.Length, default(TOutput));
                    len += result3.Length;

                    var result4 = parser4.Parse(source, start + len);
                    if (result4.Length < 0)
                        return new ParseResult<TOutput>(-len + result4.Length, default(TOutput));
                    len += result4.Length;

                    var result5 = parser5.Parse(source, start + len);
                    if (result5.Length < 0)
                        return new ParseResult<TOutput>(-len + result5.Length, default(TOutput));
                    len += result5.Length;

                    var result6 = parser6.Parse(source, start + len);
                    if (result6.Length < 0)
                        return new ParseResult<TOutput>(-len + result6.Length, default(TOutput));
                    len += result6.Length;

                    var result7 = parser7.Parse(source, start + len);
                    if (result7.Length < 0)
                        return new ParseResult<TOutput>(-len + result7.Length, default(TOutput));
                    len += result7.Length;

                    var result8 = parser8.Parse(source, start + len);
                    if (result8.Length < 0)
                        return new ParseResult<TOutput>(-len + result8.Length, default(TOutput));
                    len += result8.Length;

                    var result9 = parser9.Parse(source, start + len);
                    if (result9.Length < 0)
                        return new ParseResult<TOutput>(-len + result9.Length, default(TOutput));
                    len += result9.Length;

                    var produced = producer(result1.Value, result2.Value, result3.Value, result4.Value, result5.Value, result6.Value, result7.Value, result8.Value, result9.Value);
                    return new ParseResult<TOutput>(len, produced);
                });


        public struct LeftValue<TLeft>
        {
        }

        /// <summary>
        /// A parser that takes only the 'left' side parser's value and converts it into a new value.
        /// </summary>
        public static RightParser<TInput, TOutput> Rule<TLeft, TOutput>(
            LeftValue<TLeft> left,
            Func<TLeft, TOutput> producer)
            =>
            new RightParser<TInput, TOutput>(
                new RuleParser<TInput, TOutput>(
                    new Parser<TInput>[] { },
                    (list, start) =>
                    {
                        Ensure.AreEqual(list.Count - start, 1);
                        return producer(
                            (TLeft)list[start]);
                    }));

        /// <summary>
        /// A parser that combines the 'left' side parser's value with the values from a sequence of parsers into a new value.
        /// This parser fails if any of the parsers in the sequence fail.
        /// </summary>
        public static RightParser<TInput, TOutput> Rule<TLeft, TParser1, TOutput>(
            LeftValue<TLeft> left,
            Parser<TInput, TParser1> parser1,
            Func<TLeft, TParser1, TOutput> producer)
            =>
            new RightParser<TInput, TOutput>(
                new RuleParser<TInput, TOutput>(
                    new Parser<TInput>[] { parser1 },
                    (list, start) =>
                    {
                        Ensure.AreEqual(list.Count - start, 2);
                        return producer(
                            (TLeft)list[start],
                            (TParser1)list[start + 1]);
                    }));

        /// <summary>
        /// A parser that combines the 'left' side parser's value with the values from a sequence of parsers into a new value.
        /// This parser fails if any of the parsers in the sequence fail.
        /// </summary>
        public static RightParser<TInput, TOutput> Rule<TLeft, TParser1, TParser2, TOutput>(
            LeftValue<TLeft> left,
            Parser<TInput, TParser1> parser1,
            Parser<TInput, TParser2> parser2,
            Func<TLeft, TParser1, TParser2, TOutput> producer)
            =>
            new RightParser<TInput, TOutput>(
                new RuleParser<TInput, TOutput>(
                    new Parser<TInput>[] { parser1, parser2 },
                    (list, start) =>
                    {
                        Ensure.AreEqual(list.Count - start, 3);
                        return producer(
                            (TLeft)list[start],
                            (TParser1)list[start + 1],
                            (TParser2)list[start + 2]);
                    }));

        /// <summary>
        /// A parser that combines the 'left' side parser's value with the values from a sequence of parsers into a new value.
        /// This parser fails if any of the parsers in the sequence fail.
        /// </summary>
        public static RightParser<TInput, TOutput> Rule<TLeft, TParser1, TParser2, TParser3, TOutput>(
            LeftValue<TLeft> left,
            Parser<TInput, TParser1> parser1,
            Parser<TInput, TParser2> parser2,
            Parser<TInput, TParser2> parser3,
            Func<TLeft, TParser1, TParser2, TParser3, TOutput> producer)
            =>
            new RightParser<TInput, TOutput>(
                new RuleParser<TInput, TOutput>(
                    new Parser<TInput>[] { parser1, parser2, parser3 },
                    (list, start) =>
                    {
                        Ensure.AreEqual(list.Count - start, 4);
                        return producer(
                            (TLeft)list[start],
                            (TParser1)list[start + 1],
                            (TParser2)list[start + 2],
                            (TParser3)list[start + 3]);
                    }));

        /// <summary>
        /// A parser that parsers a sequence of values into the output.
        /// If any parser in the sequence fails, then this parser fails.
        /// </summary>
        public static Parser<TInput> Sequence(params Parser<TInput>[] parsers) =>
            new SequenceParser<TInput>(parsers);

        /// <summary>
        /// A parser that parsers a sequence of values into the output.
        /// If any parser in the sequence fails, then this parser fails.
        /// </summary>
        public static Parser<TInput> Sequence(IReadOnlyList<Parser<TInput>> parsers) =>
            new SequenceParser<TInput>(parsers);

        /// <summary>
        /// A parser that converts all the successfully scanned input characters into a single output string.
        /// </summary>
        public static Parser<char, string> Text(Parser<char> pattern) =>
            Parsers<char>.Convert(pattern, (Source<char> source, int start, int length) => ((TextSource)source).PeekText(start, length));

        /// <summary>
        /// A parser that consumes the matching text characters and produces the same text string.
        /// </summary>
        public static Parser<char, string> Text(string text) =>
            Parsers<char>.Convert(Match(text), text);

        /// <summary>
        /// A parser that converts all the successfully scanned input characters into a single output string and its starting offset.
        /// </summary>
        public static Parser<char, OffsetValue<string>> TextAndOffset(Parser<char> pattern) =>
            Parsers<char>.Convert(pattern, (source, start, length) => new OffsetValue<string>(start, ((TextSource)source).PeekText(start, length)));

        /// <summary>
        /// A parser that consumes no input but always generates the specified output.
        /// </summary>
        public static Parser<TInput, TOutput> Value<TOutput>(Func<TOutput> fnValue) =>
            Match((source, start) => 0, (source, start, length) => fnValue());

        /// <summary>
        /// A parser that combines zero or more parsed values into a single value.
        /// </summary>
        public static Parser<TInput, TProducer> ZeroOrMore<TParser, TProducer>(
            Parser<TInput, TParser> parser,
            Func<IReadOnlyList<TParser>, TProducer> producer)
            =>
            Produce(ZeroOrMore(parser), producer);

        /// <summary>
        /// A parser that parses zero or more values from the specified parser.
        /// </summary>
        public static Parser<TInput> ZeroOrMore(Parser<TInput> parser) =>
            new ZeroOrMoreParser<TInput>(parser);

        /// <summary>
        /// A parser that parses zero or more values from the specified parser
        /// and produces a list of those values.
        /// </summary>
        public static Parser<TInput, IReadOnlyList<TParser>> ZeroOrMoreList<TParser>(
            Parser<TInput, TParser> parser)
            => ZeroOrMore(parser, list => list.ToReadOnly());

        /// <summary>
        /// A parser that parses zero or one value from the specified parser.
        /// </summary>
        public static Parser<TInput> ZeroOrOne(Parser<TInput> parser) =>
            new ZeroOrMoreParser<TInput>(parser, zeroOrOne: true);



    }

    public struct ElementAndSeparator<TElement, TSeparator>
    {
        public TElement Element { get; }
        public TSeparator Separator { get; }

        public ElementAndSeparator(TElement element, TSeparator separator = default(TSeparator))
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            this.Element = element;
            this.Separator = separator;
        }
    }
}