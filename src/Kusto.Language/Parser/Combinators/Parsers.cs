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
        public static Parser<TInput, TOutput> Best<TOutput>(Parser<TInput, TOutput>[] parsers, Func<TOutput, TOutput, int> fnBetter) =>
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
                missingElement: null, 
                oneOrMore: oneOrMore, 
                producer: producer);
        }

        /// <summary>
        /// Creates a parser that parses a list of elements.
        /// </summary>
        /// <param name="elementParser">The parser for each element.</param>
        /// <param name="missingElement">An optional function that constructs a new element to be used when an expected element is missing.</param>
        /// <param name="oneOrMore">If true, the generated parser expects at least one element to exist.</param>
        /// <param name="producer">A function that converts the series of elements into the produced value.</param>
        public static Parser<TInput, TProducer> List<TElement, TProducer>(
            Parser<TInput, TElement> elementParser,
            Func<TElement> missingElement,
            bool oneOrMore,
            Func<IReadOnlyList<TElement>, TProducer> producer)
        {
            if (oneOrMore)
            {
                if (missingElement != null)
                {
                    var requiredElement = Required(elementParser, missingElement);

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
        /// <param name="missingElement">An optional function that constructs a new element to be used when the element is missing (between two separators).</param>
        /// <param name="missingSeparator">An optional function that constructs a new separator instance to be used when the separator is missing (between two elements).</param>
        /// <param name="endOfList">An optional parser that quickly determines if there are no more elements.</param>
        /// <param name="oneOrMore">If true, the generated parser expects at least one element to exist.</param>
        /// <param name="allowTrailingSeparator">If true, it is legal for a final separator to occur without a following element.</param>
        /// <param name="producer">A function that converts the series of elements and separators into the produced value.</param>
        public static Parser<TInput, TProducer> OList<TElement, TSeparator, TProducer>(
            Parser<TInput, TElement> elementParser,
            Parser<TInput, TSeparator> separatorParser,
            Func<TElement> missingElement,
            Func<TSeparator> missingSeparator,
            Parser<TInput> endOfList,
            bool oneOrMore,
            bool allowTrailingSeparator,
            Func<IReadOnlyList<object>, TProducer> producer)
        {
            return OList(
                elementParser,
                separatorParser,
                elementParser,
                missingElement,
                missingSeparator,
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
        /// <param name="missingElement">An optional function that constructs a new element to be used when the element is missing (between two separators).</param>
        /// <param name="missingSeparator">An optional function that constructs a new separator instance to be used when the separator is missing (between two elements).</param>
        /// <param name="endOfList">An optional parser that quickly determines if there are not more elements.</param>
        /// <param name="oneOrMore">If true, the generated parser expects at least one element to exist.</param>
        /// <param name="allowTrailingSeparator">If true, it is legal for a final separator to occur without a following element.</param>
        /// <param name="producer">A function that converts the series of elements and separators into the produced value.</param>
        public static Parser<TInput, TProducer> OList<TElement, TSeparator, TProducer>(
            Parser<TInput, TElement> primaryElementParser,
            Parser<TInput, TSeparator> separatorParser,
            Parser<TInput, TElement> secondaryElementParser,
            Func<TElement> missingElement, // optional
            Func<TSeparator> missingSeparator, // optional
            Parser<TInput> endOfList, // optional
            bool oneOrMore,
            bool allowTrailingSeparator,
            Func<IReadOnlyList<object>, TProducer> producer)
        {
            Ensure.ArgumentNotNull(primaryElementParser, nameof(primaryElementParser));
            Ensure.ArgumentNotNull(separatorParser, nameof(separatorParser));

            if (secondaryElementParser == null)
                secondaryElementParser = primaryElementParser;

            var requiredPrimaryElementParser = missingElement != null ? Required(primaryElementParser, missingElement) : primaryElementParser;
            var requiredSecondaryElementParser = missingElement != null ? Required(secondaryElementParser, missingElement) : secondaryElementParser;
            var requiredSeparatorParser = missingSeparator != null ? Required(separatorParser, missingSeparator) : separatorParser;
            Func<TProducer> emptyList = () => producer(new object[] { });

            if (oneOrMore)
            {
                if (allowTrailingSeparator)
                {
                    var secondaryParser = Sequence(separatorParser, secondaryElementParser);

                    if (missingSeparator != null && endOfList != null)
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

                    if (missingSeparator != null && endOfList != null)
                    {
                        secondaryParser = First(
                            secondaryParser,
                            // check for missing secondardy element between two separators
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

                    if (missingSeparator != null && endOfList != null)
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

                    if (missingSeparator != null && endOfList != null)
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
        /// <param name="missingElement">An optional function that constructs a new element to be used when the element is missing (between two separators).</param>
        /// <param name="missingSeparator">An optional function that constructs a new separator instance to be used when the separator is missing (between two elements).</param>
        /// <param name="endOfList">An optional parser that quickly determines if there are not more elements.</param>
        /// <param name="oneOrMore">If true, the generated parser expects at least one element to exist.</param>
        /// <param name="allowTrailingSeparator">If true, it is legal for a final separator to occur without a following element.</param>
        /// <param name="producer">A function that converts the series of elements and separators into the produced value.</param>
        public static Parser<TInput, TProducer> List<TElement, TSeparator, TProducer>(
            Parser<TInput, TElement> elementParser,
            Parser<TInput, TSeparator> separatorParser,
            Parser<TInput, TElement> secondaryElementParser,
            Func<TElement> missingElement, // optional
            Func<TSeparator> missingSeparator, // optional
            Parser<TInput> endOfList, // optional
            bool oneOrMore,
            bool allowTrailingSeparator,
            Func<IReadOnlyList<ElementAndSeparator<TElement, TSeparator>>, TProducer> producer)
        {
            return OList(
                elementParser,
                separatorParser,
                secondaryElementParser,
                missingElement,
                missingSeparator,
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
        /// <param name="missingElement">An optional function that constructs a new element to be used when the element is missing (between two separators).</param>
        /// <param name="missingSeparator">An optional function that constructs a new separator instance to be used when the separator is missing (between two elements).</param>
        /// <param name="endOfList">An optional parser that quickly determines if there are not more elements.</param>
        /// <param name="oneOrMore">If true, the generated parser expects at least one element to exist.</param>
        /// <param name="allowTrailingSeparator">If true, it is legal for a final separator to occur without a following element.</param>
        /// <param name="producer">A function that converts the series of elements and separators into the produced value.</param>
        public static Parser<TInput, TProducer> List<TElement, TSeparator, TProducer>(
            Parser<TInput, TElement> elementParser,
            Parser<TInput, TSeparator> separatorParser,
            Func<TElement> missingElement, // optional
            Func<TSeparator> missingSeparator, // optional
            Parser<TInput> endOfList, // optional
            bool oneOrMore,
            bool allowTrailingSeparator,
            Func<IReadOnlyList<ElementAndSeparator<TElement, TSeparator>>, TProducer> producer)
        {
            return List(
                elementParser,
                separatorParser,
                elementParser,
                missingElement,
                missingSeparator,
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
                missingElement: null,
                missingSeparator: null,
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
                missingElement: null,
                missingSeparator: null,
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
        /// A parser that consumes one or more sequential matching input items.
        /// </summary>
        public static Parser<TInput> Match(IReadOnlyList<TInput> items) =>
            Match(items, EqualityComparer<TInput>.Default);

        /// <summary>
        /// A parser that consumes one or more sequential matching input items.
        /// </summary>
        public static Parser<TInput> Match(IReadOnlyList<TInput> items, EqualityComparer<TInput> comparer) =>
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

        /// <summary>
        /// A parser that combines one or more values produced by a single parser into a new value.
        /// </summary>
        private static Parser<TInput, TOutput> Produce<TElement, TOutput>(Parser<TInput> parser, Func<List<object>, int, TOutput> producer) =>
            new ProduceParser<TInput, TOutput>(parser, producer);

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

#region Parsers

    public class MatchParser<TInput> : Parser<TInput>
    {
        public SourceConsumer<TInput> Consumer { get; }

        public MatchParser(SourceConsumer<TInput> consumer)
        {
            this.Consumer = consumer;
        }

        public MatchParser(Func<TInput, bool> predicate)
            : this((source, start) => !source.IsEnd(start) && predicate(source.Peek(start)) ? 1 : -1)
        {
        }

        protected override Parser<TInput> Clone()
        {
            return new MatchParser<TInput>(this.Consumer);
        }

        public override void Accept(ParserVisitor<TInput> visitor)
        {
            visitor.VisitMatch(this);
        }

        public override TResult Accept<TResult>(ParserVisitor<TInput, TResult> visitor)
        {
            return visitor.VisitMatch(this);
        }

        public override TResult Accept<TArg, TResult>(ParserVisitor<TInput, TArg, TResult> visitor, TArg arg)
        {
            return visitor.VisitMatch(this, arg);
        }

        public override int Scan(Source<TInput> source, int start)
        {
            return this.Consumer(source, start);
        }

        public override int Parse(Source<TInput> source, int inputStart, List<object> output, int outputStart)
        {
            return Scan(source, inputStart);
        }
    }

    public class MatchParser<TInput, TOutput> : Parser<TInput, TOutput>
    {
        public SourceConsumer<TInput> Consumer { get; }
        public SourceProducer<TInput, TOutput> Producer { get; }

        public MatchParser(SourceConsumer<TInput> consumer, SourceProducer<TInput, TOutput> producer)
        {
            Ensure.ArgumentNotNull(consumer, nameof(consumer));
            Ensure.ArgumentNotNull(producer, nameof(producer));

            this.Consumer = consumer;
            this.Producer = producer;
        }

        public MatchParser(Func<TInput, bool> predicate, Func<TInput, TOutput> producer)
            : this(
                  (source, start) => !source.IsEnd(start) && predicate(source.Peek(start)) ? 1 : -1,
                  (source, start, length) => producer(source.Peek(start)))
        {
        }

        protected override Parser<TInput> Clone()
        {
            return new MatchParser<TInput, TOutput>(this.Consumer, this.Producer);
        }

        public override void Accept(ParserVisitor<TInput> visitor)
        {
            visitor.VisitMatch(this);
        }

        public override TResult Accept<TResult>(ParserVisitor<TInput, TResult> visitor)
        {
            return visitor.VisitMatch(this);
        }

        public override TResult Accept<TArg, TResult>(ParserVisitor<TInput, TArg, TResult> visitor, TArg arg)
        {
            return visitor.VisitMatch(this, arg);
        }

        public override int Scan(Source<TInput> source, int start)
        {
            return this.Consumer(source, start);
        }

        public override int Parse(Source<TInput> source, int inputStart, List<object> output, int outputStart)
        {
            var length = this.Consumer(source, inputStart);

            if (length >= 0)
            {
                var value = this.Producer(source, inputStart, length);
                output.Add(value);
            }

            return length;
        }

        public override ParseResult<TOutput> Parse(Source<TInput> source, int start = 0)
        {
            var length = this.Consumer(source, start);
            
            if (length > 0)
            {
                var value = this.Producer(source, start, length);
                return new ParseResult<TOutput>(length, value);
            }

            return new ParseResult<TOutput>(length, default(TOutput));
        }
    }

    public sealed class NotParser<TInput> : Parser<TInput>
    {
        public Parser<TInput> Pattern { get; }

        public NotParser(Parser<TInput> parser)
        {
            this.Pattern = parser;
        }

        protected override Parser<TInput> Clone()
        {
            return new NotParser<TInput>(this.Pattern);
        }

        public override void Accept(ParserVisitor<TInput> visitor)
        {
            visitor.VisitNot(this);
        }

        public override TResult Accept<TResult>(ParserVisitor<TInput, TResult> visitor)
        {
            return visitor.VisitNot(this);
        }

        public override TResult Accept<TArg, TResult>(ParserVisitor<TInput, TArg, TResult> visitor, TArg arg)
        {
            return visitor.VisitNot(this, arg);
        }

        public override int Parse(Source<TInput> source, int inputStart, List<object> output, int outputStart)
        {
            return Scan(source, inputStart);
        }

        public override int Scan(Source<TInput> source, int start)
        {
            // EOF never scans successfully
            if (source.IsEnd(start))
                return -1;

            // if scanning succeeds then fail
            if (this.Pattern.Scan(source, start) >= 0)
                return -1;

            return 1;
        }
    }

    public sealed class FailsParser<TInput> : Parser<TInput>
    {
        public Parser<TInput> Pattern { get; }

        public FailsParser(Parser<TInput> pattern)
        {
            this.Pattern = pattern;
        }

        protected override Parser<TInput> Clone()
        {
            return new FailsParser<TInput>(this.Pattern);
        }

        public override void Accept(ParserVisitor<TInput> visitor)
        {
            visitor.VisitFails(this);
        }

        public override TResult Accept<TResult>(ParserVisitor<TInput, TResult> visitor)
        {
            return visitor.VisitFails(this);
        }

        public override TResult Accept<TArg, TResult>(ParserVisitor<TInput, TArg, TResult> visitor, TArg arg)
        {
            return visitor.VisitFails(this, arg);
        }

        public override int Parse(Source<TInput> source, int inputStart, List<object> output, int outputStart)
        {
            return this.Scan(source, inputStart);
        }

        public override int Scan(Source<TInput> source, int start)
        {
            // At end is a succeess here
            if (source.IsEnd(start))
                return 0;

            if (this.Pattern.Scan(source, start) >= 0)
                return -1; // if pattern scan succeeds then this scan fails

            return 0;
        }
    }

    /// <summary>
    /// A parser with result based parsing implemented over output-list based parsing.
    /// </summary>
    public abstract class ListPrimaryParser<TInput, TOutput> : Parser<TInput, TOutput>
    {
        /// <summary>
        /// Common pool of output lists: don't allocate temporary lists!
        /// </summary>
        private static readonly ObjectPool<List<object>> s_outputListPool =
            new ObjectPool<List<object>>(() => new List<object>(), list => list.Clear(), size: 50);

        public override ParseResult<TOutput> Parse(Source<TInput> source, int start)
        {
            var list = s_outputListPool.AllocateFromPool();
            try
            {
                var n = this.Parse(source, start, list, 0);

                return new ParseResult<TOutput>(n,
                    n >= 0 && list.Count > 0
                        ? (TOutput)list[0]
                        : default(TOutput));
            }
            finally
            {
                s_outputListPool.ReturnToPool(list);
            }
        }
    }

    /// <summary>
    /// A parser with output list based parsing implemented over result based parsing.
    /// These parsers should *not* wrap other parsers.
    /// </summary>
    public abstract class ResultPrimaryParser<TInput, TOutput> : Parser<TInput, TOutput>
    {
        public override int Parse(Source<TInput> source, int inputStart, List<object> output, int outputStart)
        {
            var result = this.Parse(source, inputStart);

            if (result.Length >= 0)
            {
                output.Add(result.Value);
            }

            return result.Length;
        }
    }

    public sealed class RuleParser<TInput, TProducer> : ListPrimaryParser<TInput, TProducer>
    {
        private readonly Parser<TInput>[] _parsers;
        public IReadOnlyList<Parser<TInput>> Parsers => _parsers;

        public Func<List<object>, int, TProducer> ListProducer { get; }
        public Func<Source<TInput>, int, ParseResult<TProducer>> ResultProducer { get; }

        public RuleParser(
            IReadOnlyList<Parser<TInput>> parsers,
            Func<List<object>, int, TProducer> listProducer,
            Func<Source<TInput>, int, ParseResult<TProducer>> resultProducer = null)
        {
            _parsers = parsers.ToArray();
            this.ListProducer = listProducer;
            this.ResultProducer = resultProducer;
        }

        public override void Accept(ParserVisitor<TInput> visitor)
        {
            visitor.VisitRule(this);
        }

        public override TResult Accept<TResult>(ParserVisitor<TInput, TResult> visitor)
        {
            return visitor.VisitRule(this);
        }

        public override TResult Accept<TArg, TResult>(ParserVisitor<TInput, TArg, TResult> visitor, TArg arg)
        {
            return visitor.VisitRule(this, arg);
        }

        protected override Parser<TInput> Clone()
        {
            return new RuleParser<TInput, TProducer>(this.Parsers, this.ListProducer, this.ResultProducer);
        }

        public override int Scan(Source<TInput> source, int start)
        {
            var len = 0;

            for (int i = 0; i < _parsers.Length; i++)
            {
                var parser = _parsers[i];
                var n = parser.Scan(source, start + len);

                if (n < 0)
                {
                    return n - len;
                }

                len += n;
            }

            return len;
        }

        public override ParseResult<TProducer> Parse(Source<TInput> source, int start)
        {
            if (this.ResultProducer != null)
            {
                return this.ResultProducer(source, start);
            }
            else
            {
                return base.Parse(source, start);
            }
        }

        public override int Parse(Source<TInput> input, int inputStart, List<object> output, int outputStart)
        {
            int length = 0;
            int originalOutputCount = output.Count;

            // invoke each parser in sequence.. if one fails then the whole is in error
            for (int i = 0; i < _parsers.Length; i++)
            {
                var parser = _parsers[i];
                int n = parser.Parse(input, inputStart + length, output, output.Count);
                if (n < 0)
                {
                    output.SetCount(originalOutputCount);
                    return n - length;
                }

                length += n;
            }

            var value = this.ListProducer(output, outputStart);
            output.SetCount(outputStart);
            output.Add(value);

            return length;
        }
    }

    public class ProduceParser<TInput, TProducer> : ListPrimaryParser<TInput, TProducer>
    {
        public Parser<TInput> Parser { get; }
        public Func<List<object>, int, TProducer> Producer { get; }

        public ProduceParser(Parser<TInput> parser, Func<List<object>, int, TProducer> producer)
        {
            Ensure.ArgumentNotNull(parser, nameof(parser));
            Ensure.ArgumentNotNull(producer, nameof(producer));

            this.Parser = parser;
            this.Producer = producer;
        }

        public override void Accept(ParserVisitor<TInput> visitor)
        {
            visitor.VisitProduce(this);
        }

        public override TResult Accept<TResult>(ParserVisitor<TInput, TResult> visitor)
        {
            return visitor.VisitProduce(this);
        }

        public override TResult Accept<TArg, TResult>(ParserVisitor<TInput, TArg, TResult> visitor, TArg arg)
        {
            return visitor.VisitProduce(this, arg);
        }

        protected override Parser<TInput> Clone()
        {
            return new ProduceParser<TInput, TProducer>(this.Parser, this.Producer);
        }

        public override int Parse(Source<TInput> source, int inputStart, List<object> output, int outputStart)
        {
            int originalOutputCount = output.Count;
            var length = this.Parser.Parse(source, inputStart, output, outputStart);
            return Produce(output, outputStart, originalOutputCount, length);
        }

        private int Produce(List<object> output, int outputStart, int originalOutputCount, int inputLength)
        {
            if (inputLength >= 0)
            {
                var value = this.Producer(output, outputStart);
                output.SetCount(outputStart);
                output.Add(value);
            }
            else
            {
                output.SetCount(originalOutputCount);
            }

            return inputLength;
        }

        public override int Scan(Source<TInput> source, int start)
        {
            return Parser.Scan(source, start);
        }
    }

    public class OptionalParser<TInput, TOutput> : Parser<TInput, TOutput>
    {
        public Parser<TInput, TOutput> Parser { get; }
        public Func<TOutput> Producer { get; }

        public OptionalParser(Parser<TInput, TOutput> parser, Func<TOutput> producer)
        {
            Ensure.ArgumentNotNull(parser, nameof(parser));
            Ensure.ArgumentNotNull(producer, nameof(producer));
            this.Parser = parser;
            this.Producer = producer;
        }

        public override bool IsOptional => true;

        public override void Accept(ParserVisitor<TInput> visitor)
        {
            visitor.VisitOptional(this);
        }

        public override TResult Accept<TResult>(ParserVisitor<TInput, TResult> visitor)
        {
            return visitor.VisitOptional(this);
        }

        public override TResult Accept<TArg, TResult>(ParserVisitor<TInput, TArg, TResult> visitor, TArg arg)
        {
            return visitor.VisitOptional(this, arg);
        }

        protected override Parser<TInput> Clone()
        {
            return new OptionalParser<TInput, TOutput>(this.Parser, this.Producer);
        }

        public override ParseResult<TOutput> Parse(Source<TInput> source, int start)
        {
            var result = this.Parser.Parse(source, start);
            if (result.Length < 0)
            {
                return new ParseResult<TOutput>(0, Producer());
            }
            else
            {
                return result;
            }
        }

        public override int Parse(Source<TInput> source, int inputStart, List<object> output, int outputStart)
        {
            var originalOutputCount = output.Count;
            int length = Parser.Parse(source, inputStart, output, output.Count);

            if (length < 0 || output.Count == originalOutputCount)
            {
                output.SetCount(originalOutputCount);
                output.Add(Producer());
                return 0;
            }

            return length;
        }

        public override int Scan(Source<TInput> source, int start)
        {
            var n = Parser.Scan(source, start);

            if (n < 0)
            {
                return 0;
            }

            return n;
        }
    }

    public sealed class RequiredParser<TInput, TOutput> : Parser<TInput, TOutput>
    {
        public Parser<TInput, TOutput> Parser { get; }
        public Func<TOutput> Producer { get; }

        public RequiredParser(Parser<TInput, TOutput> parser, Func<TOutput> producer)
        {
            Ensure.ArgumentNotNull(parser, nameof(parser));
            Ensure.ArgumentNotNull(producer, nameof(producer));
            this.Parser = parser;
            this.Producer = producer;
        }

        public override void Accept(ParserVisitor<TInput> visitor)
        {
            visitor.VisitRequired(this);
        }

        public override TResult Accept<TResult>(ParserVisitor<TInput, TResult> visitor)
        {
            return visitor.VisitRequired(this);
        }

        public override TResult Accept<TArg, TResult>(ParserVisitor<TInput, TArg, TResult> visitor, TArg arg)
        {
            return visitor.VisitRequired(this, arg);
        }

        protected override Parser<TInput> Clone()
        {
            return new RequiredParser<TInput, TOutput>(this.Parser, this.Producer);
        }

        public override ParseResult<TOutput> Parse(Source<TInput> source, int start)
        {
            var result = Parser.Parse(source, start);
            if (result.Length < 0)
            {
                return new ParseResult<TOutput>(0, Producer());
            }
            else
            {
                return result;
            }
        }

        public override int Parse(Source<TInput> source, int inputStart, List<object> output, int outputStart)
        {
            var originalOutputCount = output.Count;

            var length = this.Parser.Parse(source, inputStart, output, output.Count);
            if (length < 0 || output.Count == originalOutputCount)
            {
                output.SetCount(originalOutputCount);
                output.Add(Producer());
                return 0;
            }

            return length;
        }

        public override int Scan(Source<TInput> source, int start)
        {
            var len = this.Parser.Scan(source, start);
            return (len < 0) ? 0 : len;
        }
    }

    public class FirstParser<TInput, TOutput> : Parser<TInput, TOutput>
    {
        private readonly Parser<TInput, TOutput>[] _parsers;
        public IReadOnlyList<Parser<TInput, TOutput>> Parsers => _parsers;

        public FirstParser(IReadOnlyList<Parser<TInput, TOutput>> parsers)
        {
            Ensure.ArgumentNotNull(parsers, nameof(parsers));
            Ensure.ElementsNotNull(parsers, nameof(parsers));
            _parsers = parsers.ToArray();
        }

        public override void Accept(ParserVisitor<TInput> visitor)
        {
            visitor.VisitFirst(this);
        }

        public override TResult Accept<TResult>(ParserVisitor<TInput, TResult> visitor)
        {
            return visitor.VisitFirst(this);
        }

        public override TResult Accept<TArg, TResult>(ParserVisitor<TInput, TArg, TResult> visitor, TArg arg)
        {
            return visitor.VisitFirst(this, arg);
        }

        protected override Parser<TInput> Clone()
        {
            return new FirstParser<TInput, TOutput>(this.Parsers);
        }

        public override ParseResult<TOutput> Parse(Source<TInput> source, int start)
        {
            int minLength = -1;

            for (int i = 0; i < _parsers.Length; i++)
            {
                var parser = _parsers[i];
                var result = parser.Parse(source, start);

                if (result.Length >= 0)
                {
                    Ensure.IsTrue(result.Length > 0 || i == this.Parsers.Count - 1, "zero consuming parsers should only occur at end");
                    return result;
                }

                if (result.Length < minLength)
                {
                    minLength = result.Length;
                }
            }

            return new ParseResult<TOutput>(minLength, default(TOutput));
        }

        public override int Parse(Source<TInput> source, int inputStart, List<object> output, int outputStart)
        {
            int minLength = -1;
            var originalOutputCount = output.Count;

            for (int i = 0; i < _parsers.Length; i++)
            {
                var parser = _parsers[i];
                output.SetCount(originalOutputCount);

                var length = parser.Parse(source, inputStart, output, outputStart);
                if (length >= 0)
                {
                    return length;
                }

                if (length < minLength)
                {
                    minLength = length;
                }
            }

            return minLength;
        }

        public override int Scan(Source<TInput> source, int start)
        {
            int minLength = -1;

            for (int i = 0; i < _parsers.Length; i++)
            {
                var parser = _parsers[i];
                var n = parser.Scan(source, start);
                if (n >= 0)
                    return n;

                if (n < minLength)
                {
                    minLength = n;
                }
            }

            return minLength;
        }
    }

    public sealed class BestParser<TInput> : Parser<TInput>
    {
        private readonly Parser<TInput>[] _parsers;
        public IReadOnlyList<Parser<TInput>> Parsers => _parsers;

        public BestParser(IReadOnlyList<Parser<TInput>> parsers)
        {
            Ensure.ArgumentNotNull(parsers, nameof(parsers));
            Ensure.ElementsNotNull(parsers, nameof(parsers));
            _parsers = parsers.ToArray();
        }

        public override void Accept(ParserVisitor<TInput> visitor)
        {
            visitor.VisitBest(this);
        }

        public override TResult Accept<TResult>(ParserVisitor<TInput, TResult> visitor)
        {
            return visitor.VisitBest(this);
        }

        public override TResult Accept<TArg, TResult>(ParserVisitor<TInput, TArg, TResult> visitor, TArg arg)
        {
            return visitor.VisitBest(this, arg);
        }

        protected override Parser<TInput> Clone()
        {
            return new BestParser<TInput>(this.Parsers);
        }

        public override int Parse(Source<TInput> source, int inputStart, List<object> output, int outputStart)
        {
            // must be true: until this is rewritten for use as RightParser
            Ensure.AreEqual(output.Count, outputStart);

            int minLength = -1;
            int maxLength = -1;
            int bestParser = -1;

            // figure out which parser will consume most input
            for (int i = 0; i < _parsers.Length; i++)
            {
                var parser = _parsers[i];
                var length = parser.Scan(source, inputStart);

                if (length > maxLength)
                {
                    maxLength = length;
                    bestParser = i;
                }
                else if (length < minLength)
                {
                    minLength = length;
                }
            }

            if (maxLength >= 0)
            {
                _parsers[bestParser].Parse(source, inputStart, output, outputStart);
            }

            return maxLength;
        }

        public override int Scan(Source<TInput> source, int start)
        {
            int max = -1;
            int min = -1;

            for (int i = 0; i < _parsers.Length; i++)
            {
                var parser = _parsers[i];
                var n = parser.Scan(source, start);
                if (n > max)
                    max = n;
                else if (n < min)
                    min = n;
            }

            return max >= 0 ? max : min;
        }
    }

    public sealed class BestParser<TInput, TOutput> : Parser<TInput, TOutput>
    {
        private readonly Parser<TInput, TOutput>[] _parsers;
        private readonly Func<TOutput, TOutput, int> _fnBetter;

        public IReadOnlyList<Parser<TInput, TOutput>> Parsers => _parsers;
        public Func<TOutput, TOutput, int> Better => _fnBetter;

        public BestParser(
            IReadOnlyList<Parser<TInput, TOutput>> parsers, 
            Func<TOutput, TOutput, int> fnBetter = null)
        {
            Ensure.ArgumentNotNull(parsers, nameof(parsers));
            Ensure.ElementsNotNull(parsers, nameof(parsers));
            _parsers = parsers.ToArray();
            _fnBetter = fnBetter;
        }

        public override void Accept(ParserVisitor<TInput> visitor)
        {
            visitor.VisitBest(this);
        }

        public override TResult Accept<TResult>(ParserVisitor<TInput, TResult> visitor)
        {
            return visitor.VisitBest(this);
        }

        public override TResult Accept<TArg, TResult>(ParserVisitor<TInput, TArg, TResult> visitor, TArg arg)
        {
            return visitor.VisitBest(this, arg);
        }

        protected override Parser<TInput> Clone()
        {
            return new BestParser<TInput, TOutput>(this.Parsers);
        }

        public override ParseResult<TOutput> Parse(Source<TInput> source, int start)
        {
            int minLength = -1;
            int maxLength = -1;
            int bestParser = -1;
            List<Parser<TInput, TOutput>> candidates = null;

            // figure out which parser will consume most input
            for (int i = 0; i < _parsers.Length; i++)
            {
                var parser = _parsers[i];
                var length = parser.Scan(source, start);

                if (length > maxLength)
                {
                    maxLength = length;
                    bestParser = i;

                    if (candidates != null)
                    {
                        candidates.Clear();
                    }
                }
                else if (length == maxLength && bestParser >= 0 && _fnBetter != null)
                {
                    if (candidates == null)
                    {
                        candidates = new List<Parser<TInput, TOutput>>();
                    }

                    candidates.Add(_parsers[i]);
                }
                else if (length < minLength)
                {
                    minLength = length;
                }
            }

            if (maxLength >= 0)
            {
                var bestP = _parsers[bestParser];

                if (candidates != null && candidates.Count > 0)
                {
                    var bestV = bestP.Parse(source, start).Value;

                    for (int i = 0; i < candidates.Count; i++)
                    {
                        var otherV = candidates[i].Parse(source, start).Value;
                        if (_fnBetter(otherV, bestV) > 0)
                        {
                            bestV = otherV;
                        }
                    }

                    return new ParseResult<TOutput>(maxLength, bestV);
                }
                else
                {
                    var result = bestP.Parse(source, start);
                    return new ParseResult<TOutput>(maxLength, result.Value);
                }
            }
            else
            {
                return new ParseResult<TOutput>(minLength, default(TOutput));
            }
        }

        public override int Parse(Source<TInput> source, int inputStart, List<object> output, int outputStart)
        {
            int minLength = -1;
            int maxLength = -1;
            int bestParser = -1;
            List<Parser<TInput, TOutput>> candidates = null;

            // figure out which parser will consume most input
            for (int i = 0; i < _parsers.Length; i++)
            {
                var parser = _parsers[i];
                var length = parser.Scan(source, inputStart);

                if (length > maxLength)
                {
                    maxLength = length;
                    bestParser = i;

                    if (candidates != null)
                    {
                        candidates.Clear();
                    }
                }
                else if (length == maxLength && bestParser >= 0 && _fnBetter != null)
                {
                    if (candidates == null)
                    {
                        candidates = new List<Parser<TInput, TOutput>>();
                    }

                    candidates.Add(_parsers[i]);
                }
                else if (length < minLength)
                {
                    minLength = length;
                }
            }

            if (maxLength >= 0)
            {
                var bestP = _parsers[bestParser];

                if (candidates != null && candidates.Count > 0)
                {
                    bestP.Parse(source, inputStart, output, outputStart);
                    var bestV = (TOutput)output[outputStart];
                    output.SetCount(outputStart);

                    for (int i = 0; i < candidates.Count; i++)
                    {
                        candidates[i].Parse(source, inputStart, output, outputStart);
                        var otherV = (TOutput)output[outputStart];
                        output.SetCount(outputStart);

                        if (_fnBetter(otherV, bestV) > 0)
                        {
                            bestV = otherV;
                        }
                    }

                    output.Add(bestV);
                }
                else
                {
                    bestP.Parse(source, inputStart, output, outputStart);
                }
            }

            return maxLength;
        }

        public override int Scan(Source<TInput> source, int start)
        {
            int max = -1;
            int min = -1;

            for (int i = 0; i < _parsers.Length; i++)
            {
                var parser = _parsers[i];
                var n = parser.Scan(source, start);
                if (n > max)
                    max = n;
                else if (n < min)
                    min = n;
            }

            return max >= 0 ? max : min;
        }
    }

    /// <summary>
    /// A function that consumes input items by returning the number of input items consumed.
    /// </summary>
    public delegate int SourceConsumer<TInput>(Source<TInput> source, int start);

    /// <summary>
    /// A function that converts one or more input elements into a single output element.
    /// </summary>
    /// <typeparam name="TInput">The type of the input elements.</typeparam>
    /// <typeparam name="TOutput">The type of the produced output.</typeparam>
    /// <param name="source">The source of the input elements.</param>
    /// <param name="start">The starting offset of the first input element.</param>
    /// <param name="length">The number of input elements successfully scanned.</param>
    public delegate TOutput SourceProducer<TInput, TOutput>(Source<TInput> source, int start, int length);

    /// <summary>
    /// A parser that converts a series of scanned input items into an output item
    /// </summary>
    public sealed class ConvertParser<TInput, TOutput> : ResultPrimaryParser<TInput, TOutput>
    {
        public Parser<TInput> Pattern { get; }
        public SourceProducer<TInput, TOutput> ListProducer { get; }
        public Func<TInput, TOutput> SingleProducer { get; }

        private ConvertParser(
            Parser<TInput> pattern,
            SourceProducer<TInput, TOutput> listProducer,
            Func<TInput, TOutput> singleProducer)
        {
            Ensure.ArgumentNotNull(pattern, nameof(pattern));
            Ensure.IsTrue(listProducer != null || singleProducer != null);

            this.Pattern = pattern;
            this.ListProducer = listProducer;
            this.SingleProducer = singleProducer;
        }

        public ConvertParser(
            Parser<TInput> pattern,
            SourceProducer<TInput, TOutput> producer)
            : this(pattern, producer, null)
        {
        }

        public ConvertParser(
            Parser<TInput> pattern,
            Func<IReadOnlyList<TInput>, TOutput> producer)
            : this(pattern, 
                  (Source<TInput> source, int start, int length) =>
                  {
                      var values = s_inputListPool.AllocateFromPool();
                      try
                      {
                          for (int i = 0; i < length; i++)
                          {
                              values.Add(source.Peek(start + i));
                          }

                          return producer(values);
                      }
                      finally
                      {
                          s_inputListPool.ReturnToPool(values);
                      }
                  }, null)
        {
        }

        public ConvertParser(
            Parser<TInput> pattern,
            Func<TInput, TOutput> producer)
            : this(pattern, null, producer)
        {
        }

        public override void Accept(ParserVisitor<TInput> visitor)
        {
            visitor.VisitConvert(this);
        }

        public override TResult Accept<TResult>(ParserVisitor<TInput, TResult> visitor)
        {
            return visitor.VisitConvert(this);
        }

        public override TResult Accept<TArg, TResult>(ParserVisitor<TInput, TArg, TResult> visitor, TArg arg)
        {
            return visitor.VisitConvert(this, arg);
        }

        protected override Parser<TInput> Clone()
        {
            return new ConvertParser<TInput, TOutput>(this.Pattern, this.ListProducer, this.SingleProducer);
        }

        public override ParseResult<TOutput> Parse(Source<TInput> source, int start)
        {
            int length = this.Pattern.Scan(source, start);
            if (length < 0)
                return new ParseResult<TOutput>(length, default(TOutput));

            var value = Produce(source, start, length);
            return new ParseResult<TOutput>(length, value);
        }

        /// <summary>
        /// Common pool of input lists: don't allocate temporary lists!
        /// </summary>
        private static readonly ObjectPool<List<TInput>> s_inputListPool =
            new ObjectPool<List<TInput>>(() => new List<TInput>(), list => list.Clear());

        private TOutput Produce(Source<TInput> source, int start, int length)
        {
            if (this.SingleProducer != null)
            {
                return this.SingleProducer(source.Peek(start));
            }
            else
            {
                return this.ListProducer(source, start, length);
            }
        }

        public override int Scan(Source<TInput> source, int start)
        {
            return this.Pattern.Scan(source, start);
        }
    }

    public sealed class IfParser<TInput, TOutput> : Parser<TInput, TOutput>
    {
        public Parser<TInput> Test { get; }
        public Parser<TInput, TOutput> Parser { get; }

        public IfParser(Parser<TInput> test, Parser<TInput, TOutput> parser)
        {
            Ensure.ArgumentNotNull(test, nameof(test));
            Ensure.ArgumentNotNull(parser, nameof(parser));
            this.Test = test;
            this.Parser = parser;
        }

        public override void Accept(ParserVisitor<TInput> visitor)
        {
            visitor.VisitIf(this);
        }

        public override TResult Accept<TResult>(ParserVisitor<TInput, TResult> visitor)
        {
            return visitor.VisitIf(this);
        }

        public override TResult Accept<TArg, TResult>(ParserVisitor<TInput, TArg, TResult> visitor, TArg arg)
        {
            return visitor.VisitIf(this, arg);
        }

        protected override Parser<TInput> Clone()
        {
            return new IfParser<TInput, TOutput>(this.Test, this.Parser);
        }

        public override ParseResult<TOutput> Parse(Source<TInput> source, int start)
        {
            var length = this.Test.Scan(source, start);
            if (length < 0)
                return new ParseResult<TOutput>(length, default(TOutput));

            return this.Parser.Parse(source, start);
        }

        public override int Parse(Source<TInput> source, int inputStart, List<object> output, int outputStart)
        {
            var length = this.Test.Scan(source, inputStart);

            if (length < 0)
                return length;

            return this.Parser.Parse(source, inputStart, output, outputStart);
        }

        public override int Scan(Source<TInput> source, int start)
        {
            var length = this.Test.Scan(source, start);
            if (length < 0)
                return length;

            return this.Parser.Scan(source, start);
        }
    }

    public sealed class ForwardParser<TInput, TOutput> : ListPrimaryParser<TInput, TOutput>
    {
        public Func<Parser<TInput, TOutput>> DeferredParser;

        public ForwardParser(Func<Parser<TInput, TOutput>> deferredParser)
        {
            Ensure.ArgumentNotNull(deferredParser, nameof(deferredParser));
            this.DeferredParser = deferredParser;
        }

        public override void Accept(ParserVisitor<TInput> visitor)
        {
            visitor.VisitForward(this);
        }

        public override TResult Accept<TResult>(ParserVisitor<TInput, TResult> visitor)
        {
            return visitor.VisitForward(this);
        }

        public override TResult Accept<TArg, TResult>(ParserVisitor<TInput, TArg, TResult> visitor, TArg arg)
        {
            return visitor.VisitForward(this, arg);
        }

        protected override Parser<TInput> Clone()
        {
            return new ForwardParser<TInput, TOutput>(this.DeferredParser);
        }

        // Sanitiy check recursive call depth to catch run-away parsing/scanning on deeply nested function calls, etc
        [ThreadStatic]
        private static int s_callDepth;
        private const int MaxCallDepth = 30;

        public override ParseResult<TOutput> Parse(Source<TInput> source, int start)
        {
            try
            {
                s_callDepth++;

                if (s_callDepth > MaxCallDepth)
                {
                    return base.Parse(source, start);
                }

                return this.DeferredParser().Parse(source, start);
            }
            finally
            {
                s_callDepth--;
            }
        }

        public override int Parse(Source<TInput> source, int inputStart, List<object> output, int outputStart)
        {
            try
            {
                s_callDepth++;

                if (s_callDepth > MaxCallDepth)
                {
                    return SafeParser.ParseSafe(this.DeferredParser(), source, inputStart, output, outputStart);
                }

                return this.DeferredParser().Parse(source, inputStart, output, outputStart);
            }
            finally
            {
                s_callDepth--;
            }
        }

        public override int Scan(Source<TInput> source, int start)
        {
            try
            {
                s_callDepth++;

                if (s_callDepth > MaxCallDepth)
                {
                    return SafeScanner.ScanSafe(this.DeferredParser(), source, start);
                }

                return this.DeferredParser().Scan(source, start);
            }
            finally
            {
                s_callDepth--;
            }
        }
    }

    public sealed class MapParser<TInput, TOutput> : ResultPrimaryParser<TInput, TOutput>
    {
        private readonly Node root;

        private MapParser(Node root)
        {
            this.root = root;

        }
        public MapParser(IEnumerable<KeyValuePair<IEnumerable<TInput>, Func<TOutput>>> keyValuePairs)
            : this(Node.From(keyValuePairs))
        {
        }

        public override void Accept(ParserVisitor<TInput> visitor)
        {
            visitor.VisitMap(this);
        }

        public override TResult Accept<TResult>(ParserVisitor<TInput, TResult> visitor)
        {
            return visitor.VisitMap(this);
        }

        public override TResult Accept<TArg, TResult>(ParserVisitor<TInput, TArg, TResult> visitor, TArg arg)
        {
            return visitor.VisitMap(this, arg);
        }

        protected override Parser<TInput> Clone()
        {
            return new MapParser<TInput, TOutput>(this.root);
        }

        public override ParseResult<TOutput> Parse(Source<TInput> source, int start)
        {
            var node = root;
            var length = 0;

            var bestLength = -1;
            Func<TOutput> bestOutput = null;

            while (true)
            {
                if (source.IsEnd(start + length))
                    break;

                var input = source.Peek(start + length);

                Node subNode;
                if (node.TryGetValueNode(input, out subNode))
                {
                    length++;

                    if (subNode.HasValue)
                    {
                        bestLength = length;
                        bestOutput = subNode.Value;
                    }

                    node = subNode;
                }
                else
                {
                    break;
                }
            }

            if (bestLength > 0)
            {
                var bestValue = bestOutput();
                return new ParseResult<TOutput>(bestLength, bestValue);
            }
            else
            {
                return new ParseResult<TOutput>(bestLength, default(TOutput));
            }
        }

        public override int Scan(Source<TInput> source, int start)
        {
            var node = root;
            var length = 0;

            var bestLength = -1;

            while (true)
            {
                if (source.IsEnd(start + length))
                    break;

                var input = source.Peek(start + length);

                Node subNode;
                if (node.TryGetValueNode(input, out subNode))
                {
                    length++;

                    if (subNode.HasValue)
                    {
                        bestLength = length;
                    }

                    node = subNode;
                }
                else
                {
                    break;
                }
            }

            return bestLength;
        }

        private class Node
        {
            public bool HasValue { get; private set; }

            public Func<TOutput> Value { get; private set; }

            private Dictionary<TInput, Node> map;

            private Node()
            {
            }

            public static Node From(IEnumerable<KeyValuePair<IEnumerable<TInput>, Func<TOutput>>> keyValuePairs)
            {
                var node = new Node();

                foreach (var pair in keyValuePairs)
                {
                    node.Add(pair.Key, 0, pair.Value);
                }

                return node;
            }

            private void Add(IEnumerable<TInput> sequence, int pos, Func<TOutput> value)
            {
                var node = this;

                foreach (var item in sequence)
                {
                    if (node.map == null)
                    {
                        node.map = new Dictionary<TInput, Node>();
                    }

                    Node subNode;
                    if (!node.map.TryGetValue(item, out subNode))
                    {
                        subNode = new Node();
                        node.map.Add(item, subNode);
                    }

                    node = subNode;
                }

                node.HasValue = true;
                node.Value = value;
            }

            public bool TryGetValueNode(TInput key, out Node node)
            {
                if (this.map != null)
                {
                    return this.map.TryGetValue(key, out node);
                }

                node = null;
                return false;
            }
        }
    }

    public enum ApplyKind
    {
        One,
        ZeroOrOne,
        ZeroOrMore
    }

    public sealed class ApplyParser<TInput, TLeft, TOutput> : ListPrimaryParser<TInput, TOutput>
    {
        public Parser<TInput, TLeft> LeftParser { get; }

        public Parser<TInput, TOutput> RightParser { get; }

        public ApplyKind ApplyKind { get; }

        private ApplyParser(ApplyKind kind, Parser<TInput, TLeft> leftParser, Parser<TInput, TOutput> rightParser)
        {
            Ensure.ArgumentNotNull(leftParser, nameof(leftParser));
            Ensure.ArgumentNotNull(rightParser, nameof(rightParser));

            this.LeftParser = leftParser;
            this.RightParser = rightParser;
            this.ApplyKind = kind;
        }

        public ApplyParser(ApplyKind kind, Parser<TInput, TLeft> leftParser, RightParser<TInput, TOutput> rightParser)
            : this(kind, leftParser, rightParser.Parser)
        {
        }

        public override void Accept(ParserVisitor<TInput> visitor)
        {
            visitor.VisitApply(this);
        }

        public override TResult Accept<TResult>(ParserVisitor<TInput, TResult> visitor)
        {
            return visitor.VisitApply(this);
        }

        public override TResult Accept<TArg, TResult>(ParserVisitor<TInput, TArg, TResult> visitor, TArg arg)
        {
            return visitor.VisitApply(this, arg);
        }

        protected override Parser<TInput> Clone()
        {
            return new ApplyParser<TInput, TLeft, TOutput>(this.ApplyKind, this.LeftParser, this.RightParser);
        }

        public override int Parse(Source<TInput> source, int inputStart, List<object> output, int outputStart)
        {
            var leftOriginalOutputCount = output.Count;

            var leftLength = LeftParser.Parse(source, inputStart, output, outputStart);
            if (leftLength < 0)
            {
                return leftLength;
            }

            var rightOriginalOutputCount = output.Count;

            while (true)
            {
                var rightLength = RightParser.Parse(source, inputStart + leftLength, output, outputStart);
                if (rightLength < 0)
                {
                    if (this.ApplyKind == ApplyKind.One)
                    {
                        output.SetCount(leftOriginalOutputCount);
                        return -leftLength + rightLength;
                    }
                    else
                    {
                        output.SetCount(rightOriginalOutputCount);
                        return leftLength;
                    }
                }
                else
                {
                    leftLength += rightLength;

                    if (this.ApplyKind != ApplyKind.ZeroOrMore)
                    {
                        return leftLength;
                    }
                }
            }
        }

        public override int Scan(Source<TInput> source, int start)
        {
            var leftLen = this.LeftParser.Scan(source, start);
            if (leftLen < 0)
            {
                return leftLen;
            }

            while (true)
            {
                var rightLen = this.RightParser.Scan(source, start + leftLen);
                if (rightLen < 0)
                {
                    if (this.ApplyKind == ApplyKind.One)
                    {
                        // failed to be applied once
                        return -leftLen + rightLen;
                    }
                    else
                    {
                        return leftLen;
                    }
                }
                else
                {
                    leftLen += rightLen;

                    if (this.ApplyKind != ApplyKind.ZeroOrMore)
                    {
                        return leftLen;
                    }
                }
            }
        }
    }

    public sealed class SequenceParser<TInput> : Parser<TInput>
    {
        private readonly Parser<TInput>[] _parsers;
        public IReadOnlyList<Parser<TInput>> Parsers => _parsers;

        public SequenceParser(IReadOnlyList<Parser<TInput>> parsers)
        {
            Ensure.ArgumentNotNull(parsers, nameof(parsers));
            Ensure.ElementsNotNull(parsers, nameof(parsers));
            _parsers = parsers.ToArray();
        }

        public override void Accept(ParserVisitor<TInput> visitor)
        {
            visitor.VisitSequence(this);
        }

        public override TResult Accept<TResult>(ParserVisitor<TInput, TResult> visitor)
        {
            return visitor.VisitSequence(this);
        }

        public override TResult Accept<TArg, TResult>(ParserVisitor<TInput, TArg, TResult> visitor, TArg arg)
        {
            return visitor.VisitSequence(this, arg);
        }

        protected override Parser<TInput> Clone()
        {
            return new SequenceParser<TInput>(this.Parsers);
        }

        public override int Parse(Source<TInput> source, int inputStart, List<object> output, int outputStart)
        {
            int length = 0;
            var originalOutputCount = output.Count;

            for (int i = 0; i < _parsers.Length; i++)
            {
                var parser = _parsers[i];
                var len = parser.Parse(source, inputStart + length, output, output.Count);

                if (len < 0)
                {
                    output.SetCount(originalOutputCount);
                    return -length + len;
                }

                length += len;
            }

            return length;
        }

        public override int Scan(Source<TInput> source, int start)
        {
            var len = 0;

            for (int i = 0; i < _parsers.Length; i++)
            {
                var parser = _parsers[i];
                var n = parser.Scan(source, start + len);

                if (n < 0)
                {
                    return n - len;
                }

                len += n;
            }

            return len;
        }
    }

    public sealed class OneOrMoreParser<TInput> : Parser<TInput>
    {
        public Parser<TInput> Parser { get; }

        public OneOrMoreParser(Parser<TInput> parser)
        {
            Ensure.ArgumentNotNull(parser, nameof(parser));
            this.Parser = parser;
        }

        public override void Accept(ParserVisitor<TInput> visitor)
        {
            visitor.VisitOneOrMore(this);
        }

        public override TResult Accept<TResult>(ParserVisitor<TInput, TResult> visitor)
        {
            return visitor.VisitOneOrMore(this);
        }

        public override TResult Accept<TArg, TResult>(ParserVisitor<TInput, TArg, TResult> visitor, TArg arg)
        {
            return visitor.VisitOneOrMore(this, arg);
        }

        protected override Parser<TInput> Clone()
        {
            return new OneOrMoreParser<TInput>(this.Parser);
        }

        public override int Parse(Source<TInput> source, int inputStart, List<object> output, int outputStart)
        {
            var firstLen = this.Parser.Parse(source, inputStart, output, output.Count);
            if (firstLen < 0)
            {
                return firstLen;
            }

            var length = firstLen;

            while (true)
            {
                var len = Parser.Parse(source, inputStart + length, output, output.Count);
                if (len <= 0)
                {
                    return length;
                }
                else
                {
                    length += len;
                }
            }
        }

        public override int Scan(Source<TInput> source, int start)
        {
            var n = Parser.Scan(source, start);
            if (n < 0)
            {
                return n;
            }

            var len = n;

            while (true)
            {
                n = Parser.Scan(source, start + len);
                if (n <= 0)
                    break;
                len += n;
            }

            return len;
        }
    }

    public sealed class ZeroOrMoreParser<TInput> : Parser<TInput>
    {
        public Parser<TInput> Parser { get; }

        public bool ZeroOrOne { get; }

        public ZeroOrMoreParser(Parser<TInput> parser, bool zeroOrOne = false)
        {
            Ensure.ArgumentNotNull(parser, nameof(parser));
            this.Parser = parser;
            this.ZeroOrOne = zeroOrOne;
        }

        public override bool IsOptional => true;

        public override void Accept(ParserVisitor<TInput> visitor)
        {
            visitor.VisitZeroOrMore(this);
        }

        public override TResult Accept<TResult>(ParserVisitor<TInput, TResult> visitor)
        {
            return visitor.VisitZeroOrMore(this);
        }

        public override TResult Accept<TArg, TResult>(ParserVisitor<TInput, TArg, TResult> visitor, TArg arg)
        {
            return visitor.VisitZeroOrMore(this, arg);
        }

        protected override Parser<TInput> Clone()
        {
            return new ZeroOrMoreParser<TInput>(this.Parser, this.ZeroOrOne);
        }

        public override int Parse(Source<TInput> source, int inputStart, List<object> output, int outputStart)
        {
            var length = 0;

            while (true)
            {
                var len = Parser.Parse(source, inputStart + length, output, output.Count);
                if (len > 0)
                {
                    length += len;
                }

                if (len <= 0 || this.ZeroOrOne)
                    return length;
            }
        }

        public override int Scan(Source<TInput> source, int start)
        {
            var len = 0;

            while (true)
            {
                var n = Parser.Scan(source, start + len);
                if (n > 0)
                {
                    len += n;
                }

                if (n <= 0 || this.ZeroOrOne)
                    break;
            }

            return len;
        }
    }

    public sealed class FirstParser<TInput> : Parser<TInput>
    {
        private readonly Parser<TInput>[] _parsers;
        public IReadOnlyList<Parser<TInput>> Parsers => _parsers;

        public FirstParser(IReadOnlyList<Parser<TInput>> parsers)
        {
            Ensure.ArgumentNotNull(parsers, nameof(parsers));
            Ensure.ElementsNotNull(parsers, nameof(parsers));
            _parsers = parsers.ToArray();
        }

        public override void Accept(ParserVisitor<TInput> visitor)
        {
            visitor.VisitFirst(this);
        }

        public override TResult Accept<TResult>(ParserVisitor<TInput, TResult> visitor)
        {
            return visitor.VisitFirst(this);
        }

        public override TResult Accept<TArg, TResult>(ParserVisitor<TInput, TArg, TResult> visitor, TArg arg)
        {
            return visitor.VisitFirst(this, arg);
        }

        protected override Parser<TInput> Clone()
        {
            return new FirstParser<TInput>(this.Parsers);
        }

        public override int Parse(Source<TInput> source, int inputStart, List<object> output, int outputCount)
        {
            int minLength = -1;

            for (int i = 0; i < _parsers.Length; i++)
            {
                var parser = _parsers[i];
                var length = parser.Parse(source, inputStart, output, outputCount);
                if (length >= 0)
                {
                    Ensure.IsTrue(length > 0 || i == this.Parsers.Count - 1, "zero consuming parsers should only occur at end");
                    return length;
                }

                if (length < minLength)
                {
                    minLength = length;
                }
            }

            return minLength;
        }

        public override int Scan(Source<TInput> source, int start)
        {
            int minLength = -1;

            for (int i = 0; i < _parsers.Length; i++)
            {
                var parser = _parsers[i];
                var n = parser.Scan(source, start);
                if (n >= 0)
                    return n;

                if (n < minLength)
                {
                    minLength = n;
                }
            }

            return minLength;
        }
    }

    public sealed class IfParser<TInput> : Parser<TInput>
    {
        public Parser<TInput> Test { get; }
        public Parser<TInput> Parser { get; }

        public IfParser(Parser<TInput> test, Parser<TInput> parser)
        {
            Ensure.ArgumentNotNull(test, nameof(test));
            Ensure.ArgumentNotNull(parser, nameof(parser));

            this.Test = test;
            this.Parser = parser;
        }

        public override void Accept(ParserVisitor<TInput> visitor)
        {
            visitor.VisitIf(this);
        }

        public override TResult Accept<TResult>(ParserVisitor<TInput, TResult> visitor)
        {
            return visitor.VisitIf(this);
        }

        public override TResult Accept<TArg, TResult>(ParserVisitor<TInput, TArg, TResult> visitor, TArg arg)
        {
            return visitor.VisitIf(this, arg);
        }

        protected override Parser<TInput> Clone()
        {
            return new IfParser<TInput>(this.Test, this.Parser);
        }

        public override int Parse(Source<TInput> source, int inputStart, List<object> output, int outputStart)
        {
            var length = this.Test.Scan(source, inputStart);
            if (length < 0)
                return length;

            return this.Parser.Parse(source, inputStart, output, outputStart);
        }

        public override int Scan(Source<TInput> source, int start)
        {
            var length = this.Test.Scan(source, start);
            if (length < 0)
                return length;

            return this.Parser.Scan(source, start);
        }
    }

    public abstract class ParserVisitor<TInput>
    {
        public abstract void VisitApply<TLeft, TOutput>(ApplyParser<TInput, TLeft, TOutput> parser);
        public abstract void VisitBest<TOutput>(BestParser<TInput, TOutput> parser);
        public abstract void VisitBest(BestParser<TInput> parser);
        public abstract void VisitConvert<TOutput>(ConvertParser<TInput, TOutput> parser);
        public abstract void VisitFails(FailsParser<TInput> parser);
        public abstract void VisitFirst<TOutput>(FirstParser<TInput, TOutput> parser);
        public abstract void VisitFirst(FirstParser<TInput> parser);
        public abstract void VisitForward<TOutput>(ForwardParser<TInput, TOutput> parser);
        public abstract void VisitIf<TOutput>(IfParser<TInput, TOutput> parser);
        public abstract void VisitIf(IfParser<TInput> parser);
        public abstract void VisitMap<TOutput>(MapParser<TInput, TOutput> parser);
        public abstract void VisitMatch(MatchParser<TInput> parser);
        public abstract void VisitMatch<TOutput>(MatchParser<TInput, TOutput> parser);
        public abstract void VisitNot(NotParser<TInput> parser);
        public abstract void VisitOneOrMore(OneOrMoreParser<TInput> parser);
        public abstract void VisitOptional<TOutput>(OptionalParser<TInput, TOutput> parser);
        public abstract void VisitProduce<TOutput>(ProduceParser<TInput, TOutput> parser);
        public abstract void VisitRequired<TOutput>(RequiredParser<TInput, TOutput> parser);
        public abstract void VisitRule<TOutput>(RuleParser<TInput, TOutput> parser);
        public abstract void VisitSequence(SequenceParser<TInput> parser);
        public abstract void VisitZeroOrMore(ZeroOrMoreParser<TInput> parser);
    }

    public abstract class ParserVisitor<TInput, TResult>
    {
        public abstract TResult VisitApply<TLeft, TOutput>(ApplyParser<TInput, TLeft, TOutput> parser);
        public abstract TResult VisitBest<TOutput>(BestParser<TInput, TOutput> parser);
        public abstract TResult VisitBest(BestParser<TInput> parser);
        public abstract TResult VisitConvert<TOutput>(ConvertParser<TInput, TOutput> parser);
        public abstract TResult VisitFails(FailsParser<TInput> parser);
        public abstract TResult VisitFirst<TOutput>(FirstParser<TInput, TOutput> parser);
        public abstract TResult VisitFirst(FirstParser<TInput> parser);
        public abstract TResult VisitForward<TOutput>(ForwardParser<TInput, TOutput> parser);
        public abstract TResult VisitIf<TOutput>(IfParser<TInput, TOutput> parser);
        public abstract TResult VisitIf(IfParser<TInput> parser);
        public abstract TResult VisitMap<TOutput>(MapParser<TInput, TOutput> parser);
        public abstract TResult VisitMatch(MatchParser<TInput> parser);
        public abstract TResult VisitMatch<TOutput>(MatchParser<TInput, TOutput> parser);
        public abstract TResult VisitNot(NotParser<TInput> parser);
        public abstract TResult VisitOneOrMore(OneOrMoreParser<TInput> parser);
        public abstract TResult VisitOptional<TOutput>(OptionalParser<TInput, TOutput> parser);
        public abstract TResult VisitProduce<TOutput>(ProduceParser<TInput, TOutput> parser);
        public abstract TResult VisitRequired<TOutput>(RequiredParser<TInput, TOutput> parser);
        public abstract TResult VisitRule<TOutput>(RuleParser<TInput, TOutput> parser);
        public abstract TResult VisitSequence(SequenceParser<TInput> parser);
        public abstract TResult VisitZeroOrMore(ZeroOrMoreParser<TInput> parser);
    }

    public abstract class ParserVisitor<TInput, TArg, TResult>
    {
        public abstract TResult VisitApply<TLeft, TOutput>(ApplyParser<TInput, TLeft, TOutput> parser, TArg arg);
        public abstract TResult VisitBest<TOutput>(BestParser<TInput, TOutput> parser, TArg arg);
        public abstract TResult VisitBest(BestParser<TInput> parser, TArg arg);
        public abstract TResult VisitConvert<TOutput>(ConvertParser<TInput, TOutput> parser, TArg arg);
        public abstract TResult VisitFails(FailsParser<TInput> parser, TArg arg);
        public abstract TResult VisitFirst<TOutput>(FirstParser<TInput, TOutput> parser, TArg arg);
        public abstract TResult VisitFirst(FirstParser<TInput> parser, TArg arg);
        public abstract TResult VisitForward<TOutput>(ForwardParser<TInput, TOutput> parser, TArg arg);
        public abstract TResult VisitIf<TOutput>(IfParser<TInput, TOutput> parser, TArg arg);
        public abstract TResult VisitIf(IfParser<TInput> parser, TArg arg);
        public abstract TResult VisitMap<TOutput>(MapParser<TInput, TOutput> parser, TArg arg);
        public abstract TResult VisitMatch(MatchParser<TInput> parser, TArg arg);
        public abstract TResult VisitMatch<TOutput>(MatchParser<TInput, TOutput> parser, TArg arg);
        public abstract TResult VisitNot(NotParser<TInput> parser, TArg arg);
        public abstract TResult VisitOneOrMore(OneOrMoreParser<TInput> parser, TArg arg);
        public abstract TResult VisitOptional<TOutput>(OptionalParser<TInput, TOutput> parser, TArg arg);
        public abstract TResult VisitProduce<TOutput>(ProduceParser<TInput, TOutput> parser, TArg arg);
        public abstract TResult VisitRequired<TOutput>(RequiredParser<TInput, TOutput> parser, TArg arg);
        public abstract TResult VisitRule<TOutput>(RuleParser<TInput, TOutput> parser, TArg arg);
        public abstract TResult VisitSequence(SequenceParser<TInput> parser, TArg arg);
        public abstract TResult VisitZeroOrMore(ZeroOrMoreParser<TInput> parser, TArg arg);
    }

    #endregion
}