namespace Kusto.Language.Editor
{
    /// <summary>
    /// A class that details the range of text of a unique block of code in a <see cref="CodeScript"/> and its associated <see cref="CodeService"/>.
    /// </summary>
    public sealed class CodeBlock
    {
        /// <summary>
        /// The text of the <see cref="CodeBlock"/>
        /// </summary>
        public string Text => _codeService.Text;

        /// <summary>
        /// The start of the <see cref="CodeBlock"/> within the <see cref="CodeScript"/>.
        /// </summary>
        public int Start { get; }

        /// <summary>
        /// The length of the <see cref="CodeBlock"/> within the <see cref="CodeScript"/>.
        /// </summary>
        public int Length => this.Text.Length;

        /// <summary>
        /// The end of the <see cref="CodeBlock"/> within the <see cref="CodeScript"/>.
        /// </summary>
        public int End => this.Start + this.Length;

        /// <summary>
        /// The kind of code in the <see cref="CodeBlock"/>.
        /// </summary>
        public string Kind => _codeService.Kind;

        private readonly OffsetCodeService _codeService;

        /// <summary>
        /// The <see cref="CodeService"/> associated with this <see cref="CodeBlock"/>.
        /// </summary>
        public CodeService Service => _codeService;

        internal CodeBlock(int start, CodeService codeService)
        {
            this.Start = start;
            _codeService = codeService is OffsetCodeService ols
                ? ols.WithOffset(start)
                : new OffsetCodeService(codeService, start);
        }

        /// <summary>
        /// Creates a new <see cref="CodeBlock"/> with a modified starting position.
        /// </summary>
        internal CodeBlock WithStart(int start)
        {
            return new CodeBlock(start, this.Service);
        }
    }
}