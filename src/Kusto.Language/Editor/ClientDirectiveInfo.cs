using System.Collections.Generic;

namespace Kusto.Language.Editor
{
    /// <summary>
    /// The breakdown of a command or query text with directives.
    /// </summary>
    public class ClientDirectiveInfo
    {
        /// <summary>
        /// The original text of the entire block.
        /// </summary>
        public EditString Text { get; }

        /// <summary>
        /// The directives preceding the query or command
        /// </summary>
        public IReadOnlyList<ClientDirective> Directives { get; }

        /// <summary>
        /// The remaining text after the last directive containing the command or query, if any.
        /// </summary>
        public EditString RemainingText { get; }

        public ClientDirectiveInfo(EditString text, IReadOnlyList<ClientDirective> directives, EditString remainingText)
        {
            this.Text = text;
            this.Directives = directives;
            this.RemainingText = remainingText;
        }

        /// <summary>
        /// Parses a <see cref="ClientDirectiveInfo"/> from a command or query text that may contain directives.
        /// </summary>
        public static ClientDirectiveInfo Parse(EditString text)
        {
            var directives = new List<ClientDirective>();

            // parse all directives
            var pos = 0;
            while (true)
            {
                var ws = Parsing.TokenParser.ScanWhitespace(text, pos);
                if (pos + ws < text.Length && text[pos + ws] == '#')
                {
                    var lineStart = Parsing.TextFacts.GetLineStart(text, pos);
                    var lineLength = Parsing.TextFacts.GetLineLength(text, lineStart);
                    var directiveText = text.Substring(lineStart, lineLength);
                    if (ClientDirective.TryParse(directiveText, out var directive))
                    {
                        directives.Add(directive);
                    }
                    pos = Parsing.TextFacts.GetNextLineStart(text, lineStart);
                    continue;
                }
                break;
            }

            var remainingText = text.Substring(pos).Trim();

            return new ClientDirectiveInfo(text, directives, remainingText);
        }
    }
}