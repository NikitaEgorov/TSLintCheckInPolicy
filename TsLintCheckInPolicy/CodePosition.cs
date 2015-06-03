using System.Diagnostics.CodeAnalysis;

namespace FileEncodingCheckInPolicy
{
    public class CodePosition
    {
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Reviewed.")]
        public int character { get; set; }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Reviewed.")]
        public int line { get; set; }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Reviewed.")]
        public int position { get; set; }

        public override string ToString()
        {
            return string.Format("[{0}, {1}]", this.line, this.character);
        }
    }
}