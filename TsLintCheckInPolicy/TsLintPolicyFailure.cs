using System.IO;

using Microsoft.TeamFoundation.VersionControl.Client;

namespace FileEncodingCheckInPolicy
{
    public class TsLintPolicyFailure : PolicyFailure
    {
        private readonly Violation _violation;

        public TsLintPolicyFailure(string message, IPolicyEvaluation policy)
            : base(message, policy)
        {
        }

        public TsLintPolicyFailure(string message)
            : base(message)
        {
        }

        public TsLintPolicyFailure(Violation violation, IPolicyEvaluation policy)
            : base(
                string.Format(
                    "({0}) {1}{2}: {3}",
                    violation.ruleName,
                    Path.GetFileName(violation.name),
                    violation.startPosition,
                    violation.failure),
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