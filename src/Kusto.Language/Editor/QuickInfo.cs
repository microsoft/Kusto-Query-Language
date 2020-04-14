using Kusto.Language.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kusto.Language.Editor
{
    public class QuickInfo
    {
        private string _text;

        public IReadOnlyList<QuickInfoItem> Items { get; }

        public QuickInfo(string text)
        {
            this._text = text;
            this.Items = new[] { new QuickInfoItem(QuickInfoKinds.Text, text) };
        }

        public QuickInfo(params QuickInfoItem[] items)
            : this((IEnumerable<QuickInfoItem>)items)
        {
        }

        public QuickInfo(IEnumerable<QuickInfoItem> items)
        {
            this.Items = items.ToReadOnly();
        }

        public string Text
        {
            get
            {
                if (_text == null)
                {
                    _text = BuildText();
                }

                return _text;
            }
        }

        private string BuildText()
        {
            var sb = new StringBuilder();

            foreach (var item in Items)
            {
                if (sb.Length > 0)
                    sb.Append("\n\n");

                var kindText = GetKindText(item.Kind);
                if (kindText != null)
                {
                    sb.Append($"({kindText}) ");
                }

                sb.Append(item.Text);
            }

            return sb.ToString();
        }

        private static string GetKindText(string kind)
        {
            switch (kind)
            {
                case QuickInfoKinds.Text:
                    return null;
                case QuickInfoKinds.BuiltInFunction:
                case QuickInfoKinds.DatabaseFunction:
                case QuickInfoKinds.LocalFunction:
                    return "function";
                default:
                    return kind.ToLower();
            }
        }
   

        public static QuickInfo Empty = new QuickInfo(string.Empty);
    }

    public class QuickInfoItem
    {
        public string Kind { get; }
        public string Text { get; }

        public QuickInfoItem(string kind, string text)
        {
            this.Kind = kind;
            this.Text = text;
        }
    }

    public class QuickInfoKinds
    {
        public const string Text = nameof(Text);
        public const string Error = nameof(Error);
        public const string Warning = nameof(Warning);
        public const string Suggestion = nameof(Suggestion);
        public const string Column = nameof(Column);
        public const string Table = nameof(Table);
        public const string Database = nameof(Database);
        public const string Cluster = nameof(Cluster);
        public const string Literal = nameof(Literal);
        public const string Type = nameof(Type);
        public const string Pattern = nameof(Pattern);
        public const string Parameter = nameof(Parameter);
        public const string Scalar = nameof(Scalar);
        public const string Variable = nameof(Variable);
        public const string LocalFunction = nameof(LocalFunction);
        public const string BuiltInFunction = nameof(BuiltInFunction);
        public const string DatabaseFunction = nameof(DatabaseFunction);
        public const string Operator = nameof(Operator);
        public const string Tuple = nameof(Tuple);
        public const string Group = nameof(Group);
    }
}