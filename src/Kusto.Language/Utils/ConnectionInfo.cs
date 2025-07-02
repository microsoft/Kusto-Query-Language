using System;
using System.Collections.Generic;

namespace Kusto.Language.Utils
{
    internal class ConnectionInfo
    {
        private readonly string _text;
        private readonly Dictionary<string, string> _parts;

        private ConnectionInfo(string text, Dictionary<string, string> parts)
        {
            _text = text;
            _parts = parts;
        }

        public string GetPart(string name) => 
            _parts.TryGetValue(name, out var value) ? value : "";

        public string Text => _text;
        public string DataSource => GetPart("Data Source");

        public static ConnectionInfo Parse(string text)
        {
            var parts = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            // switch to parsing argument text
            int pos = 0;
            while (pos < text.Length)
            {
                // get name
                var nameStart = pos;
                char ch = '\0';
                while (pos < text.Length && (ch = text[pos]) != '=' && ch != ';')
                {
                    pos++;
                }

                if (pos < text.Length && (ch = text[pos]) == '=')
                {
                    var name = text.Substring(nameStart, pos - nameStart);
                    pos++; // skip =

                    // skip whitespace
                    pos += Parsing.TokenParser.ScanWhitespace(text, pos);

                    var valueStart = pos;

                    // string literal (no escapes)?
                    if (pos < text.Length && ((ch = text[pos]) == '"' || ch == '\''))
                    {
                        pos++;
                        while (pos < text.Length && text[pos] != ch)
                        {
                            pos++;
                        }

                        if (pos < text.Length && text[pos] == ch)
                        {
                            pos++;
                            var value = text.Substring(valueStart + 1, pos - valueStart - 2);
                            parts[name] = value;
                        }
                        else
                        {
                            var value = text.Substring(valueStart + 1, pos - valueStart - 1);
                            parts[name] = value;
                        }
                    }
                    else
                    {
                        // consume to end or semi
                        while (pos < text.Length && (ch = text[pos]) != ';')
                        {
                            pos++;
                        }
                        var value = text.Substring(valueStart, pos - valueStart).Trim();
                        parts[name] = value;
                    }

                    // skip any whitespace after value
                    pos += Parsing.TokenParser.ScanWhitespace(text, pos);
                }
                else
                {
                    // we have a value without a name
                    // assume this is a data source uri
                    var value = text.Substring(nameStart, pos - nameStart).Trim();
                    parts["Data Source"] = value;
                }

                if (pos < text.Length && text[pos] == ';')
                {
                    pos++;
                    continue;
                }

                break;
            }

            return new ConnectionInfo(text, parts);
        }
    }
}