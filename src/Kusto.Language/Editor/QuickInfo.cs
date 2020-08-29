using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Kusto.Language.Editor
{
    using Utils;

    public class QuickInfo
    {
        private string _text;

        public IReadOnlyList<QuickInfoItem> Items { get; }

        public QuickInfo(string text)
        {
            this._text = text;
            this.Items = new[] { new QuickInfoItem(QuickInfoKind.Text, text) };
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

        private static string GetKindText(QuickInfoKind kind)
        {
            switch (kind)
            {
                case QuickInfoKind.Text:
                default:
                    return null;
                case QuickInfoKind.Table:
                    return "table";
                case QuickInfoKind.Database:
                    return "database";
                case QuickInfoKind.Cluster:
                    return "cluster";
                case QuickInfoKind.Literal:
                    return "literal";
                case QuickInfoKind.Type:
                    return "type";
                case QuickInfoKind.Pattern:
                    return "pattern";
                case QuickInfoKind.Parameter:
                    return "parameter";
                case QuickInfoKind.Scalar:
                    return "scalar";
                case QuickInfoKind.Variable:
                    return "variable";
                case QuickInfoKind.LocalFunction:
                case QuickInfoKind.BuiltInFunction:
                case QuickInfoKind.DatabaseFunction:
                    return "function";
                case QuickInfoKind.Operator:
                    return "operator";
            }
        }  

        public static QuickInfo Empty = new QuickInfo((IEnumerable<QuickInfoItem>)null);
    }

    public class QuickInfoItem
    {
        public QuickInfoKind Kind { get; }
        public IReadOnlyCollection<ClassifiedText> Parts { get; }

        public QuickInfoItem(QuickInfoKind kind, string text)
            : this(kind, new ClassifiedText(text))
        {
        }

        public QuickInfoItem(QuickInfoKind kind, params ClassifiedText[] parts)
            : this(kind, (IEnumerable<ClassifiedText>)parts)
        {
        }

        public QuickInfoItem(QuickInfoKind kind, IEnumerable<ClassifiedText> parts)
        {
            this.Kind = kind;
            this.Parts = parts.ToReadOnly();
        }

        private string _text;
        public string Text
        {
            get
            {
                if (_text == null)
                {
                    _text = string.Concat(this.Parts.Select(s => s.Text));
                }

                return _text;
            }
        }
    }

    public enum QuickInfoKind
    {
        /// <summary>
        /// The <see cref="QuickInfoItem"/> is general text.
        /// </summary>
        Text,

        /// <summary>
        /// The <see cref="QuickInfoItem"/> is an error diagnostic.
        /// </summary>
        Error,

        /// <summary>
        /// The <see cref="QuickInfoItem"/> is a warning diagnostic.
        /// </summary>
        Warning,

        /// <summary>
        /// The <see cref="QuickInfoItem"/> is a suggestion diagnostic.
        /// </summary>
        Suggestion,

        /// <summary>
        /// The <see cref="QuickInfoItem"/> is a column name reference.
        /// </summary>
        Column,

        /// <summary>
        /// The <see cref="QuickInfoItem"/> is a table name reference.
        /// </summary>
        Table,

        /// <summary>
        /// The <see cref="QuickInfoItem"/> is a database name reference.
        /// </summary>
        Database,

        /// <summary>
        /// The <see cref="QuickInfoItem"/> is a cluster name reference.
        /// </summary>
        Cluster,

        /// <summary>
        /// The <see cref="QuickInfoItem"/> is a literal value.
        /// </summary>
        Literal,

        /// <summary>
        /// The <see cref="QuickInfoItem"/> is a scalar type name.
        /// </summary>
        Type,

        /// <summary>
        /// The <see cref="QuickInfoItem"/> is a pattern name reference.
        /// </summary>
        Pattern,

        /// <summary>
        /// The <see cref="QuickInfoItem"/> is a parameter name reference.
        /// </summary>
        Parameter,

        /// <summary>
        /// The <see cref="QuickInfoItem"/> is a scalar variable name reference.
        /// </summary>
        Scalar,

        /// <summary>
        /// The <see cref="QuickInfoItem"/> is a variable name reference.
        /// </summary>
        Variable,

        /// <summary>
        /// The <see cref="QuickInfoItem"/> is a local function name reference.
        /// </summary>
        LocalFunction,

        /// <summary>
        /// The <see cref="QuickInfoItem"/> is a built in function name reference.
        /// </summary>
        BuiltInFunction,

        /// <summary>
        /// The <see cref="QuickInfoItem"/> is a database function name reference.
        /// </summary>
        DatabaseFunction,

        /// <summary>
        /// The <see cref="QuickInfoItem"/> is a scalar operator reference.
        /// </summary>
        Operator,

        /// <summary>
        /// The <see cref="QuickInfoItem"/> is a control command.
        /// </summary>
        Command,

        /// <summary>
        /// The <see cref="QuickInfoItem"/> is a query option.
        /// </summary>
        Option
    }

    public class ClassifiedText
    {
        /// <summary>
        /// The <see cref="ClassificationKind"/> of the text.
        /// </summary>
        public ClassificationKind Kind { get; }

        /// <summary>
        /// The text itself.
        /// </summary>
        public string Text { get; }

        public ClassifiedText(string text)
            : this(ClassificationKind.PlainText, text)
        {
        }

        public ClassifiedText(ClassificationKind kind, string text)
        {
            this.Kind = kind;
            this.Text = text;
        }
    }
}