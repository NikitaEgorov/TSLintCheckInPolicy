using System.IO;

using Microsoft.TeamFoundation.VersionControl.Client;

using StyleCop;

namespace RalphJansen.StyleCopCheckInPolicy
{
    public class ExtendPolicyFailure : PolicyFailure
    {
        private readonly Violation _violation;

        public ExtendPolicyFailure(string message, IPolicyEvaluation policy)
            : base(message, policy)
        {
        }

        public ExtendPolicyFailure(string message)
            : base(message)
        {
        }

        public ExtendPolicyFailure(Violation violation, IPolicyEvaluation policy)
            : base(
                string.Format(
                    "({0}) {1}:{2} {3}",
                    violation.Rule.CheckId,
                    Path.GetFileName(violation.SourceCode.Path),
                    violation.Line,
                    violation.Message),
                policy)
        {
            this._violation = violation;
        }

        public Violation Violation
        {
            get
            {
                return this._violation;
            }
        }
    }
}