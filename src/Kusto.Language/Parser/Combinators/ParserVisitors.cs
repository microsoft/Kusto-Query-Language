using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Kusto.Language.Parsing
{
    using Utils;

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
        public abstract void VisitLimit<TOutput>(LimitParser<TInput, TOutput> parser);
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
        public abstract TResult VisitLimit<TOutput>(LimitParser<TInput, TOutput> parser);
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
        public abstract TResult VisitLimit<TOutput>(LimitParser<TInput, TOutput> parser, TArg arg);
    }


    public class IsParentVisitor : ParserVisitor<LexicalToken, Parser<LexicalToken>, bool>
    {
        public static IsParentVisitor Instance = new IsParentVisitor();

        public override bool VisitApply<TLeft, TOutput>(ApplyParser<LexicalToken, TLeft, TOutput> parser, Parser<LexicalToken> child) =>
            parser.LeftParser == child || parser.RightParser == child;

        public override bool VisitBest<TOutput>(BestParser<LexicalToken, TOutput> parser, Parser<LexicalToken> child) =>
            parser.Parsers.Any(p => p == child);

        public override bool VisitBest(BestParser<LexicalToken> parser, Parser<LexicalToken> child) =>
            parser.Parsers.Any(p => p == child);

        public override bool VisitConvert<TOutput>(ConvertParser<LexicalToken, TOutput> parser, Parser<LexicalToken> child) =>
            parser.Pattern == child;

        public override bool VisitFails(FailsParser<LexicalToken> parser, Parser<LexicalToken> child) =>
            parser.Pattern != child;

        public override bool VisitFirst<TOutput>(FirstParser<LexicalToken, TOutput> parser, Parser<LexicalToken> child) =>
            parser.Parsers.Any(p => p == child);

        public override bool VisitFirst(FirstParser<LexicalToken> parser, Parser<LexicalToken> child) =>
            parser.Parsers.Any(p => p == child);

        public override bool VisitForward<TOutput>(ForwardParser<LexicalToken, TOutput> parser, Parser<LexicalToken> child) =>
            parser.DeferredParser() == child;

        public override bool VisitIf<TOutput>(IfParser<LexicalToken, TOutput> parser, Parser<LexicalToken> child) =>
            parser.Test == child || parser.Parser == child;

        public override bool VisitIf(IfParser<LexicalToken> parser, Parser<LexicalToken> child) =>
            parser.Test == child || parser.Parser == child;

        public override bool VisitMap<TOutput>(MapParser<LexicalToken, TOutput> parser, Parser<LexicalToken> child) =>
            false;

        public override bool VisitMatch(MatchParser<LexicalToken> parser, Parser<LexicalToken> child) =>
            false;

        public override bool VisitMatch<TOutput>(MatchParser<LexicalToken, TOutput> parser, Parser<LexicalToken> child) =>
            false;

        public override bool VisitNot(NotParser<LexicalToken> parser, Parser<LexicalToken> child) =>
            parser.Pattern == child;

        public override bool VisitOneOrMore(OneOrMoreParser<LexicalToken> parser, Parser<LexicalToken> child) =>
            parser.Parser == child;

        public override bool VisitOptional<TOutput>(OptionalParser<LexicalToken, TOutput> parser, Parser<LexicalToken> child) =>
            parser.Parser == child;

        public override bool VisitProduce<TOutput>(ProduceParser<LexicalToken, TOutput> parser, Parser<LexicalToken> child) =>
            parser.Parser == child;

        public override bool VisitRequired<TOutput>(RequiredParser<LexicalToken, TOutput> parser, Parser<LexicalToken> child) =>
            parser.Parser == child;

        public override bool VisitRule<TOutput>(RuleParser<LexicalToken, TOutput> parser, Parser<LexicalToken> child) =>
            parser.Parsers.Any(p => p == child);

        public override bool VisitSequence(SequenceParser<LexicalToken> parser, Parser<LexicalToken> child) =>
            parser.Parsers.Any(p => p == child);

        public override bool VisitZeroOrMore(ZeroOrMoreParser<LexicalToken> parser, Parser<LexicalToken> child) =>
            parser.Parser == child;

        public override bool VisitLimit<TOutput>(LimitParser<LexicalToken, TOutput> parser, Parser<LexicalToken> child) =>
            parser.Limiter == child || parser.Limited == child;
    }


}
