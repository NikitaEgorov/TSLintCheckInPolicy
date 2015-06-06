using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text;

using EnvDTE;

using Microsoft.TeamFoundation.VersionControl.Client;
using Microsoft.VisualStudio.Shell;

using TsLintCheckInPolicy.VisualStudio;

using IServiceProvider = Microsoft.VisualStudio.OLE.Interop.IServiceProvider;
using Process = System.Diagnostics.Process;

namespace FileEncodingCheckInPolicy
{
    [Serializable]
    public class TypeScriptCheckInPolicy : PolicyBase
    {
        private string[] _extensions;

        [NonSerialized]
        private ViolationTaskProvider _taskProvider;

        public override bool CanEdit
        {
            get
            {
                return false;
            }
        }

        public override string Description
        {
            get
            {
                return "Detect problem in TypeScript files using TSLint";
            }
        }

        // This is a string that is stored with the policy definition on the source
        // control server. If a user does not have the policy plug-in installed, this string
        // is displayed.  You can use this to explain to the user how they should 
        // install the policy plug-in.
        public override string InstallationInstructions
        {
            get
            {
                return
                    "Install “TFS Power Tools” on every TFS client computer. Create a TFS folder “$/{Project}/TeamProjectConfig/CheckinPolicies” and add and check-in the “*.dll” file into this directory. Go to “Team Explorer|{Project}|Team members|Right-click|Personal settings…” and check “Install downloaded custom components” and uncheck “verify strong names before installing components”. Press “Download Now” and press Ok”.";
            }
        }

        // This string identifies the type of policy. It is displayed in the 
        // policy list when you add a new policy to a Team Project.
        public override string Type
        {
            get
            {
                return "TypeScript Validate Policy: Check TypeScript on Check-In";
            }
        }

        // This string is a description of the type of policy. It is displayed 
        // when you select the policy in the Add Check-in Policy dialog box.
        public override string TypeDescription
        {
            get
            {
                return
                    "TypeScript Validate Policy: This policy will prompt the user to decide whether or not they should be allowed to check in.";
            }
        }

        internal string[] Extensions
        {
            get
            {
                if (this._extensions == null)
                {
                    this._extensions = new[] { ".ts", };
                }

                return this._extensions;
            }

            set
            {
                this._extensions = value;
            }
        }

        public override void Activate(PolicyFailure failure)
        {
            var f = failure as TsLintPolicyFailure;
            if (f != null)
            {
                var zz = f.Violation;
                if (this._taskProvider != null)
                {
                    this._taskProvider.GotoError(zz);
                }
            }

            base.Activate(failure);
        }

        public override bool Edit(IPolicyEditArgs policyEditArgs)
        {
            return true;
        }

        public override PolicyFailure[] Evaluate()
        {
            ////System.Diagnostics.Debugger.Launch();

            PendingChange[] pendingChanges = this.GetCheckedPendingChanges();

            List<Violation> violations = new List<Violation>();
            foreach (var pendingChange in
                pendingChanges.Where(pc => pc.IsAdd || pc.IsEdit).Where(pc => this.IsVerifyableFileType(pc.FileName)))
            {
                violations.AddRange(this.IsValid(pendingChange.LocalItem).ToList());
            }

            if (this._taskProvider != null)
            {
                this._taskProvider.Clear();

                foreach (Violation violation in violations)
                {
                    this._taskProvider.AddTask(violation);
                }
            }

            return violations.Select(v => new TsLintPolicyFailure(v, this)).Cast<PolicyFailure>().ToArray();
        }

        public override void Initialize(IPendingCheckin pendingCheckin)
        {
            if (this._taskProvider != null)
            {
                this._taskProvider.Clear();
            }
            else
            {
                _DTE dte = (_DTE)pendingCheckin.GetService(typeof(_DTE));

                if (dte != null && dte.Application != null)
                {
                    this._taskProvider =
                        new ViolationTaskProvider(new ServiceProvider((IServiceProvider)dte.Application));
                }
            }

            base.Initialize(pendingCheckin);
        }

        protected internal IEnumerable<Violation> IsValid(string fileName)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException("fileName");
            }

            if (File.Exists(fileName) == false)
            {
                yield break;
            }

            var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var node = Path.Combine(basePath, @"Node\node.exe");
            var tslint = Path.Combine(basePath, @"Node\node_modules\tslint\bin\tslint-cli.js");
            var settings = this.GetTslintJsonPath(fileName, fileName);
            var outputFile = Path.GetTempFileName();

            ProcessStartInfo info = new ProcessStartInfo(
                node,
                string.Format("\"{0}\" -f \"{1}\" -c \"{2}\" -o \"{3}\" -t json", tslint, fileName, settings, outputFile));

            info.UseShellExecute = false;
            info.CreateNoWindow = true;
            info.WorkingDirectory = Path.GetDirectoryName(node);

            Violation[] log = null;
            try
            {
                var process = Process.Start(info);
                process.WaitForExit(10000);

                log = LogJavaFailure(outputFile);
            }
            finally
            {
                try
                {
                    File.Delete(outputFile);
                }
                catch
                {
                }
            }
            if (null != log)
            {
                foreach (var rootObject in log)
                {
                    yield return rootObject;
                }
            }
        }

        protected internal bool IsVerifyableFileType(string fileName)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException("fileName");
            }

            if (fileName.ToLowerInvariant().Contains(".generated"))
            {
                return false;
            }

            string extension = Path.GetExtension(fileName).ToLowerInvariant();

            return this.Extensions.Any(s => s == extension);
        }

        protected virtual PendingChange[] GetCheckedPendingChanges()
        {
            return this.PendingCheckin.PendingChanges.CheckedPendingChanges;
        }

        private static Violation[] LogJavaFailure(string outputFileName)
        {
            string errors = File.ReadAllText(outputFileName);
            if (string.IsNullOrWhiteSpace(errors))
            {
                return new Violation[0];
            }

            Violation[] d;
            using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(errors)))
            {
                var serializer = new DataContractJsonSerializer(typeof(Violation[]));
                d = (Violation[])serializer.ReadObject(ms);
            }

            return d;
        }

        private string GetTslintJsonPath(string path, string fileName)
        {
            var s = Path.Combine(path, "tslint.json");

            if (File.Exists(s))
            {
                return s;
            }

            var parent = Directory.GetParent(path);
            if (parent == null)
            {
                throw new Exception(string.Format("For file {0} tslint.json not found!", fileName));
            }

            return this.GetTslintJsonPath(parent.FullName, fileName);
        }
    }
}