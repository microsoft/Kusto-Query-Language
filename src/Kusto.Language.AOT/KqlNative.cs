using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Kusto.Language;

namespace Kusto.Language.AOT;

public static class KqlNative
{
    [UnmanagedCallersOnly(EntryPoint = "kql_parse")]
    public static IntPtr Parse(IntPtr queryPtr)
    {
        var query = Marshal.PtrToStringUTF8(queryPtr) ?? string.Empty;
        var code = KustoCode.Parse(query);
        var diagnostics = code.GetDiagnostics();
        var result = diagnostics.Count == 0 ? "OK" : diagnostics[0].Message;
        return Marshal.StringToCoTaskMemUTF8(result);
    }

    [UnmanagedCallersOnly(EntryPoint = "kql_get_diagnostics")]
    public static IntPtr GetDiagnostics(IntPtr queryPtr)
    {
        var query = Marshal.PtrToStringUTF8(queryPtr) ?? string.Empty;
        var code = KustoCode.Parse(query);
        var diagnostics = code.GetDiagnostics();

        if (diagnostics.Count == 0)
            return Marshal.StringToCoTaskMemUTF8("[]");

        var sb = new StringBuilder();
        sb.Append('[');
        for (int i = 0; i < diagnostics.Count; i++)
        {
            if (i > 0) sb.Append(',');
            var d = diagnostics[i];
            sb.Append($"{{\"start\":{d.Start},\"length\":{d.Length},\"severity\":\"{d.Severity}\",\"message\":\"{Escape(d.Message)}\"}}");
        }
        sb.Append(']');
        return Marshal.StringToCoTaskMemUTF8(sb.ToString());
    }

    [UnmanagedCallersOnly(EntryPoint = "kql_get_syntax_tree")]
    public static IntPtr GetSyntaxTree(IntPtr queryPtr)
    {
        var query = Marshal.PtrToStringUTF8(queryPtr) ?? string.Empty;
        var code = KustoCode.Parse(query);

        var sb = new StringBuilder();
        WriteSyntaxNode(code.Syntax, sb, indent: 0);
        return Marshal.StringToCoTaskMemUTF8(sb.ToString());
    }

    [UnmanagedCallersOnly(EntryPoint = "kql_get_syntax_json")]
    public static IntPtr GetSyntaxJson(IntPtr queryPtr)
    {
        var query = Marshal.PtrToStringUTF8(queryPtr) ?? string.Empty;
        var code = KustoCode.Parse(query);

        var sb = new StringBuilder();
        WriteSyntaxJson(code.Syntax, sb);
        return Marshal.StringToCoTaskMemUTF8(sb.ToString());
    }

    [UnmanagedCallersOnly(EntryPoint = "kql_get_syntax_kind")]
    public static IntPtr GetSyntaxKind(IntPtr queryPtr)
    {
        var query = Marshal.PtrToStringUTF8(queryPtr) ?? string.Empty;
        var code = KustoCode.Parse(query);
        return Marshal.StringToCoTaskMemUTF8(code.Kind.ToString());
    }

    [UnmanagedCallersOnly(EntryPoint = "kql_free")]
    public static void Free(IntPtr ptr)
    {
        Marshal.FreeCoTaskMem(ptr);
    }

    private static void WriteSyntaxNode(Syntax.SyntaxElement node, StringBuilder sb, int indent)
    {
        sb.Append(' ', indent * 2);
        sb.Append(node.Kind);
        if (node.Width > 0 && node.ChildCount == 0)
            sb.Append($" \"{Escape(node.ToString(Syntax.IncludeTrivia.Minimal))}\"");
        sb.AppendLine();
        for (int i = 0; i < node.ChildCount; i++)
        {
            var child = node.GetChild(i);
            if (child != null)
                WriteSyntaxNode(child, sb, indent + 1);
        }
    }

    private static string Escape(string s) =>
        s.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\n", "\\n").Replace("\r", "\\r");

    private static void WriteSyntaxJson(Syntax.SyntaxElement node, StringBuilder sb)
    {
        sb.Append('{');
        sb.Append($"\"kind\":\"{node.Kind}\"");
        sb.Append($",\"start\":{node.TextStart}");
        sb.Append($",\"length\":{node.Width}");

        if (node.Width > 0 && node.ChildCount == 0)
            sb.Append($",\"text\":\"{Escape(node.ToString(Syntax.IncludeTrivia.Minimal))}\"");

        if (node.ChildCount > 0)
        {
            sb.Append(",\"children\":[");
            bool first = true;
            for (int i = 0; i < node.ChildCount; i++)
            {
                var child = node.GetChild(i);
                if (child == null) continue;
                if (!first) sb.Append(',');
                first = false;
                WriteSyntaxJson(child, sb);
            }
            sb.Append(']');
        }

        sb.Append('}');
    }
}
