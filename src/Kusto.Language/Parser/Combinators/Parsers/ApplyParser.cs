using System.Collections.Generic;

namespace Kusto.Language.Parsing
{
    using Utils;

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

        public override int ChildParserCount => 2;

        public override Parser<TInput> GetChildParser(int index)
        {
            switch (index)
            {
                case 0:
                    return this.LeftParser;
                case 1:
                    return this.RightParser;
                default:
                    return null;
            }
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
}