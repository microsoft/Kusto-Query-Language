using Kusto.Language.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Kusto.Language.Parsing
{
    /// <summary>
    /// A parser combinator that can scan, search and parse input.
    /// When parsing, may produce zero or more output values.
    /// </summary>
    [DebuggerDisplay("{GetType().Name}: {Description}")]
    public abstract class Parser<TInput>
    {
        /// <summary>
        /// Create a shallow copy of this parser
        /// </summary>
        protected abstract Parser<TInput> Clone();

        /// <summary>
        /// The name of the parser.
        /// Most parsers have no name.
        /// </summary>
        public string Tag { get; private set; } = string.Empty;

        /// <summary>
        /// Creates a copy of this <see cref="Parser{TInput}"/> with the tag specified.
        /// </summary>
        public Parser<TInput> WithTag(string tag)
        {
            tag = tag ?? string.Empty;

            if (tag != this.Tag)
            {
                var clone = this.Clone();
                clone.Tag = tag;
                clone.Annotations = this.Annotations;
                clone.IsHidden = this.IsHidden;
                return clone;
            }
            else
            {
                return this;
            }
        }

        /// <summary>
        /// Annotations on the parser.
        /// </summary>
        public IReadOnlyList<object> Annotations { get; private set; } = EmptyReadOnlyList<object>.Instance;

        /// <summary>
        /// Creates a copy of this <see cref="Parser{TInput}"/> with the annotations specified.
        /// </summary>
        public Parser<TInput> WithAnnotations(IEnumerable<object> annotations)
        {
            var list = annotations.ToReadOnly();

            if (this.Annotations != list)
            {
                var clone = this.Clone();
                clone.Annotations = list;
                clone.Tag = this.Tag;
                clone.IsHidden = this.IsHidden;
                return clone;
            }
            else
            {
                return this;
            }
        }

        private string description;

        /// <summary>
        /// A description of the parser.
        /// </summary>
        public string Description
        {
            get
            {
                if (this.description == null)
                {
                    if (string.IsNullOrEmpty(this.Tag))
                    {
                        this.description = Describer.Describe(this);
                    }
                    else
                    {
                        this.description = $"{this.Tag}: {Describer.Describe(this.WithTag(null))}";
                    }
                }

                return this.description;
            }
        }

        /// <summary>
        /// True if the parser is hidden from searching.
        /// </summary>
        public bool IsHidden { get; private set; } = false;

        /// <summary>
        /// Creates a copy of this <see cref="Parser{TInput}"/> with the IsHidden property specified.
        /// </summary>
        public Parser<TInput> WithIsHidden(bool isHidden)
        {
            if (isHidden != this.IsHidden)
            {
                var clone = this.Clone();
                clone.IsHidden = isHidden;
                clone.Tag = this.Tag;
                clone.Annotations = this.Annotations;
                return clone;
            }
            else
            {
                return this;
            }
        }

        /// <summary>
        /// Creates a copy of the <see cref="Parser{TInput}"/> that is hidden from searching.
        /// </summary>
        public Parser<TInput> Hide() => this.WithIsHidden(true);

        /// <summary>
        /// True if the parser still succeeds if it does not match anything, 
        /// and instead produces nothing.
        /// </summary>
        public virtual bool IsOptional => false;

        /// <summary>
        /// True if the parser still succeeds if it does not match anything,
        /// but produces a fixed value instead.
        /// </summary>
        public virtual bool IsRequired => false;

        /// <summary>
        /// True if the parser succeeds if any one of many child parsers succeed.
        /// </summary>
        public virtual bool IsAlternation => false;

        /// <summary>
        /// True if the parser succeeds if all of many child parsers succeed in sequence.
        /// </summary>
        public virtual bool IsSequence => false;

        /// <summary>
        /// True if the parser produces multiple outputs from repeatedly parsing a child parser.
        /// </summary>
        public virtual bool IsRepetition => false;

        /// <summary>
        /// True if the parser matches input directly.
        /// </summary>
        public virtual bool IsMatch => false;

        /// <summary>
        /// True if this parser forwards to another parser,
        /// meaning it may be involved in a cycle.
        /// </summary>
        public virtual bool IsForward => false;

        public virtual bool IsNegation => false;

        public virtual bool IsConditional => false;

        /// <summary>
        /// The number of child parsers this parser contains.
        /// </summary>
        public abstract int ChildParserCount { get; }

        /// <summary>
        /// Gets the child parser at the specified index.
        /// </summary>
        public abstract Parser<TInput> GetChildParser(int index);

        /// <summary>
        /// Gets the index of the child parser contained by this parser.
        /// </summary>
        public int GetChildParserIndex(Parser<TInput> childParser)
        {
            var childCount = this.ChildParserCount;
            for (int i = 0; i < childCount; i++)
            {
                if (this.GetChildParser(i) == childParser)
                    return i;
            }

            return -1;
        }

        /// <summary>
        /// Invokes the corresponding <see cref="ParserVisitor{TInput}"/> visit method.
        /// </summary>
        public abstract void Accept(ParserVisitor<TInput> visitor);

        /// <summary>
        /// Invokes the corresponding <see cref="ParserVisitor{TInput, TResult}"/> visit method.
        /// </summary>
        public abstract TResult Accept<TResult>(ParserVisitor<TInput, TResult> visitor);

        /// <summary>
        /// Invokes the corresponding <see cref="ParserVisitor{TInput, TArg, TResult}"/> visit method.
        /// </summary>
        public abstract TResult Accept<TArg, TResult>(ParserVisitor<TInput, TArg, TResult> visitor, TArg arg);

        /// <summary>
        /// Parses input source items and produces zero or more output items.
        /// </summary>
        public abstract int Parse(Source<TInput> input, int inputStart, List<object> output, int outputStart);

        /// <summary>
        /// Returns the number of source items that are successfully matched by this parser, or a negative number indicating failure.
        /// </summary>
        public abstract int Scan(Source<TInput> input, int inputStart);
    }

    /// <summary>
    /// A parser that will produce exactly one output item if it succeeds.
    /// </summary>
    public abstract class Parser<TInput, TOutput> : Parser<TInput>
    {
        /// <summary>
        /// Parses input source items and produces a single output item.
        /// </summary>
        public abstract ParseResult<TOutput> Parse(Source<TInput> input, int inputStart);

        /// <summary>
        /// Creates a copy of this <see cref="Parser{TInput}"/> with the tag specified.
        /// </summary>
        public new Parser<TInput, TOutput> WithTag(string tag) => (Parser<TInput, TOutput>)base.WithTag(tag);

        /// <summary>
        /// Creates a copy of this <see cref="Parser{TInput}"/> with the annotations specified.
        /// </summary>
        public new Parser<TInput, TOutput> WithAnnotations(IEnumerable<object> annotations) => (Parser<TInput, TOutput>)base.WithAnnotations(annotations);

        /// <summary>
        /// Creates a copy of this <see cref="Parser{TInput}"/> with the IsHidden property specified.
        /// </summary>
        public new Parser<TInput, TOutput> WithIsHidden(bool isHidden) => (Parser<TInput, TOutput>)base.WithIsHidden(isHidden);

        /// <summary>
        /// Creates a copy of this <see cref="Parser{TInput}"/> with the IsHidden property set to true.
        /// </summary>
        public new Parser<TInput, TOutput> Hide() => this.WithIsHidden(true);

        /// <summary>
        /// Creates a copy of this <see cref="Parser{TInput, TOutput}"/> that converts its output to the specified type.
        /// </summary>
        /// <typeparam name="TNewOutput">The type to convert the output to.</typeparam>
        public Parser<TInput, TNewOutput> Cast<TNewOutput>() =>
            Parsers<TInput>.Rule(this, o => (TNewOutput)(object)o).WithTag(this.Tag);
    }
}
