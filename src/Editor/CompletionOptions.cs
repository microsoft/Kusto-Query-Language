using System;

namespace Kusto.Language.Editor
{
    public class CompletionOptions
    {
        [Flags]
        private enum OptionFlags
        {
            IncludeSymbols                  = 0b0000_0001,
            IncludeSyntax                   = 0b0000_0010,
            IncludePunctuation              = 0b0000_0100,
            EnableParameterInjection        = 0b0001_0000,
            AutoAppendWhitespace            = 0b0010_0000,

            Default = IncludeSymbols | IncludeSyntax | IncludePunctuation | AutoAppendWhitespace
        }

        private readonly OptionFlags flags;

        public bool IncludeSymbols => (flags & OptionFlags.IncludeSymbols) != 0;

        public bool IncludeSyntax => (flags & OptionFlags.IncludeSyntax) != 0;

        public bool IncludePunctuationOnlySyntax => (flags & OptionFlags.IncludePunctuation) != 0;

        public bool EnableParameterInjection => (flags & OptionFlags.EnableParameterInjection) != 0;

        public bool AutoAppendWhitespace => (flags & OptionFlags.AutoAppendWhitespace) != 0;

        public IncludeFunctionKind IncludeFunctions { get; }

        private CompletionOptions(OptionFlags flags, IncludeFunctionKind includeFunctionKind)
        {
            this.flags = flags;
            this.IncludeFunctions = includeFunctionKind;
        }

        private static OptionFlags SetOption(OptionFlags flags, OptionFlags option, bool value)
        {
            return value ? flags | option : flags & ~option;
        }

        public CompletionOptions WithIncludeSymbols(bool include)
        {
            return new CompletionOptions(SetOption(this.flags, OptionFlags.IncludeSymbols, include), this.IncludeFunctions);
        }

        public CompletionOptions WithIncludeFunctions(IncludeFunctionKind kind)
        {
            return new CompletionOptions(this.flags, kind);
        }

        public CompletionOptions WithIncludeFunctions(bool include)
        {
            return new CompletionOptions(this.flags, include ? IncludeFunctionKind.All : IncludeFunctionKind.None);
        }

        public CompletionOptions WithIncludeSyntax(bool include)
        {
            return new CompletionOptions(SetOption(this.flags, OptionFlags.IncludeSyntax, include), this.IncludeFunctions);
        }

        public CompletionOptions WithIncludePunctuationOnlySyntax(bool include)
        {
            return new CompletionOptions(SetOption(this.flags, OptionFlags.IncludePunctuation, include), this.IncludeFunctions);
        }

        public CompletionOptions WithEnableParameterInjection(bool enable)
        {
            return new CompletionOptions(SetOption(this.flags, OptionFlags.EnableParameterInjection, enable), this.IncludeFunctions);
        }

        public CompletionOptions WithAutoAppendWhitespace(bool enable)
        {
            return new CompletionOptions(SetOption(this.flags, OptionFlags.AutoAppendWhitespace, enable), this.IncludeFunctions);
        }

        public static CompletionOptions Default = new CompletionOptions(
            OptionFlags.Default,
            IncludeFunctionKind.All);
    }
}