using System.Collections.Generic;

namespace Kusto.Language.Editor
{
    /// <summary>
    /// A directive to be processed by the client before executing the query.
    /// </summary>
    public class ClientDirective
    {
        /// <summary>
        /// The full text of the directive.
        /// </summary>
        public EditString Text { get; }

        /// <summary>
        /// The name of the directive
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// All text after the name
        /// </summary>
        public EditString AfterNameText { get; }

        /// <summary>
        /// The rest of the text on the directive line after the name
        /// </summary>
        public EditString ArgumentsText { get; }

        /// <summary>
        /// The parsed arguments as a list of zero or more optionally-named values.
        /// </summary>
        public IReadOnlyList<ClientDirectiveArgument> Arguments { get; }

        /// <summary>
        /// Any text following the directive line that may contain a command, query or another directive
        /// </summary>
        public EditString AfterDirectiveText { get; }

        public ClientDirective(
            EditString text, 
            string name, 
            EditString afterNameText,
            EditString argumentsText, 
            IReadOnlyList<ClientDirectiveArgument> arguments,
            EditString afterArgumentsText
            )
        {
            this.Text = text;
            this.Name = name;
            this.AfterNameText = afterNameText;
            this.ArgumentsText = argumentsText;
            this.Arguments = arguments ?? Utils.EmptyReadOnlyList<ClientDirectiveArgument>.Instance;
            this.AfterDirectiveText = afterArgumentsText;
        }

        /// <summary>
        /// Parses a directive from text
        /// </summary>
        public static bool TryParse(EditString text, out ClientDirective directive)
        {
            // skip over whitespace and comments looking for directive 
            var pos = Parsing.TokenParser.ScanTrivia(text, 0);

            if (pos >= text.Length 
                || text[pos] != '#')
            {
                directive = null;
                return false;
            }

            // move past #
            pos++;

            // directive name
            var nameLen = Parsing.TokenParser.ScanIdentifier(text, pos);
            if (nameLen < 0)
                nameLen = 0;
            var name = text.Substring(pos, nameLen);
            pos += nameLen;

            // all the text immediately after the name
            var afterNameText = text.Substring(pos, text.Length - pos);

            // determine end of directive line
            var endOfLine = Parsing.TextFacts.GetLineEnd(text, pos);

            // argument text is any remaining text on the directive line (after whitespace)
            pos += Parsing.TokenParser.ScanWhitespace(text, pos);
            var argLen = endOfLine - pos;
            var argumentText = EditString.Empty;
            if (argLen > 0)
            {
                argumentText = text.Substring(pos, argLen);
            }

            // any text after the end of the directive line
            var afterDirectiveStart = Parsing.TextFacts.GetNextLineStart(text, pos);
            var afterDirectiveText = afterDirectiveStart >= pos ? text.Substring(afterDirectiveStart) : EditString.Empty;

            List<ClientDirectiveArgument> args = new List<ClientDirectiveArgument>();
            string argName = null;

            // switch to parsing argument text
            pos = 0;
            while (pos < argumentText.Length)
            {
                argName = null;

                // skip whitespace
                pos += Parsing.TokenParser.ScanWhitespace(argumentText, pos);

                // name= ?
                var len = Parsing.TokenParser.ScanIdentifier(argumentText, pos);
                if (len > 0)
                {
                    // check for name = prefix
                    var lookahead = len + Parsing.TokenParser.ScanWhitespace(argumentText, pos + len);
                    if (pos + lookahead < argumentText.Length && argumentText[pos + lookahead] == '=')
                    {
                        argName = argumentText.Substring(pos, len);
                        pos += lookahead + 1; // extra one for `=`
                    }
                }

                // skip whitespace
                pos += Parsing.TokenParser.ScanWhitespace(argumentText, pos);

                // string literal (no escapes)?
                char ch;
                var argStart = pos;

                // string literal?
                if (pos < argumentText.Length && ((ch = argumentText[pos]) == '"' || ch == '\''))
                {
                    pos++;
                    while (pos < argumentText.Length && argumentText[pos] != ch)
                    {
                        pos++;
                    }

                    if (pos < argumentText.Length && argumentText[pos] == ch)
                    {
                        pos++;
                        var argText = argumentText.Substring(argStart, pos - argStart);
                        var value = argumentText.Substring(argStart + 1, (pos - argStart) - 2).CurrentText;
                        args.Add(new ClientDirectiveArgument(argName, argText, value));
                        argName = null;
                        continue;
                    }
                    else
                    {
                        var argText = argumentText.Substring(argStart, pos - argStart);
                        var value = argumentText.Substring(argStart + 1, (pos - argStart) - 1).CurrentText;
                        args.Add(new ClientDirectiveArgument(argName, argText, value));
                        argName = null;
                        continue;
                    }
                }

                // find sequential text; not whitespace or separators
                while (pos < argumentText.Length 
                    && !Parsing.TextFacts.IsWhitespace(ch = argumentText[pos]) 
                    && ch != ','
                    && ch != ';')
                {
                    pos++;
                }

                argLen = pos - argStart;
                if (argLen > 0)
                {
                    // real number?
                    len = Parsing.TokenParser.ScanRealLiteral(argumentText, argStart);
                    if (len >= 0 && len == argLen)
                    {
                        var argText = argumentText.Substring(argStart, argLen);
                        double.TryParse(argText, out double value);
                        args.Add(new ClientDirectiveArgument(argName, argText, value));
                        argName = null;
                        continue;
                    }

                    // long number?
                    len = Parsing.TokenParser.ScanLongLiteral(argumentText, argStart);
                    if (len > 0 && len == argLen)
                    {
                        var argText = argumentText.Substring(argStart, argLen);
                        long.TryParse(argText, out long value);
                        args.Add(new ClientDirectiveArgument(argName, argText, value));
                        argName = null;
                        continue;
                    }

                    // otherwise, just the text of the arg is the value
                    {
                        var argText = argumentText.Substring(argStart, argLen);
                        args.Add(new ClientDirectiveArgument(argName, argText, argText.CurrentText));
                        argName = null;
                        continue;
                    }
                }

                // name= but no value?
                if (argName != null)
                {
                    args.Add(new ClientDirectiveArgument(argName, "", null));
                    continue;
                }

                // unknown character: skip over it
                pos++;
            }

            directive = new ClientDirective(text, name, afterNameText, argumentText, args, afterDirectiveText);
            return true;
        }
    }
}