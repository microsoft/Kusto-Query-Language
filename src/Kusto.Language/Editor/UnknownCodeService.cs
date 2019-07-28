namespace Kusto.Language.Editor
{
    /// <summary>
    /// A <see cref="CodeService"/> for an unknown kind of code.
    /// </summary>
    public class UnknownCodeService : CommonCodeService
    {
        public UnknownCodeService(string text)
            : base(text)
        {
        }

        public override string Kind => "Unknown";
    }
}