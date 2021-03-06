﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.TeamFoundation.VersionControl.Client;

namespace FileEncodingCheckInPolicy
{
    [Serializable]
    public class FileEncodingCheckInPolicy : PolicyBase
    {
        private string[] _extensions;

        public override string Description
        {
            get { return "Detect file Encoding and if it's not UTF-8 raise error"; }
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
                    "Install “TFS Power Tools” on every TFS client computer. Then install TsLintCheckinPolicy(http://visualstudiogallery.msdn.microsoft.com/23e05171-62af-437e-a6b2-6d93f3eafa95) or TsLintCheckinPolicy2015(https://visualstudiogallery.msdn.microsoft.com/9a085a1b-f2c7-4839-98cc-3f8cda49c884)";
            }
        }

        // This string identifies the type of policy. It is displayed in the 
        // policy list when you add a new policy to a Team Project.
        public override string Type
        {
            get { return "File Encoding Policy v2: Check file Encoding on Check-In"; }
        }

        // This string is a description of the type of policy. It is displayed 
        // when you select the policy in the Add Check-in Policy dialog box.
        public override string TypeDescription
        {
            get
            {
                return
                    "File Encoding Policy: This policy will prompt the user to decide whether or not they should be allowed to check in.";
            }
        }

        public override bool CanEdit
        {
            get { return false; }
        }

        internal string[] Extensions
        {
            get
            {
                if (this._extensions == null)
                {
                    this._extensions = new[]
                                       {
                                           ".sql", 
                                           ".cs", 
                                           ".ts",
                                           ".vb",
                                           ".js", 
                                           ".sln", 
                                           ".csproj", 
                                           ".config"
                                       };
                }

                return this._extensions;
            }

            set
            {
                this._extensions = value;
            }
        }

        public override bool Edit(IPolicyEditArgs policyEditArgs)
        {
            return true;
        }

        public override PolicyFailure[] Evaluate()
        {
            ////Debugger.Launch();
            var result = new List<PolicyFailure>();
            PendingChange[] pendingChanges = this.PendingCheckin.PendingChanges.CheckedPendingChanges;

            foreach (var pendingChange in pendingChanges
                .Where(pc => pc.IsAdd || pc.IsEdit)
                .Where(pc => !this.IsFileFromPackages(pc))
                .Where(pc => this.IsVerifyableFileType(pc.FileName)))
            {
                if (!this.IsValidEncoding(pendingChange.LocalItem))
                {
                    result.Add(new PolicyFailure(string.Format(
                        "Wrong file encoding for '{0}'. Change encoding to UTF-8 with BOM", 
                        pendingChange.LocalItem)));
                }
            }

            return result.ToArray();
        }

        static readonly List<string> packagesPaths = new List<string>
        {
            "/packages/",
            "/bower_components/",
            "/vendor/"
        }; 

        protected internal bool IsFileFromPackages(PendingChange pendingChange)
        {
            return packagesPaths.Any(path => pendingChange.ServerItem.IndexOf(path, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        protected internal bool IsVerifyableFileType(string fileName)
        {
            string extension = Path.GetExtension(fileName).ToLowerInvariant();
            foreach (string s in this.Extensions)
            {
                if (s == extension)
                {
                    return true;
                }
            }

            return false;
        }

        protected internal bool IsValidEncoding(string fileName)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException("fileName");
            }

            if (File.Exists(fileName) == false)
            {
                return true;
            }

            using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
            {
                if (fs.Length < 4)
                {
                    Trace.WriteLine(string.Format("File Encoding Policy: Local item: {0} is empty; ignoring.", fileName));
                    return true;
                }

                var encoding = TextFileEncodingDetector.DetectEncoding(fs);
                return Encoding.UTF8.Equals(encoding);
            }
        }
    }
}