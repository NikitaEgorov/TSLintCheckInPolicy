//--------------------------------------------------------------------------
// <copyright file="ViolationTaskProvider.cs" company="Ralph Jansen">
//      Copyright (c) Ralph Jansen. All rights reserved.
//
//      The use and distribution terms for this software is covered by the
//      Microsoft Public License (Ms-PL) which can be found in the License.rtf 
//      at the root of this distribution.
//      By using this software in any fashion, you are agreeing to be bound by
//      the terms of this license.
//
//      You must not remove this notice, or any other, from this software.
// </copyright>
//--------------------------------------------------------------------------

using System;

using EnvDTE;

using FileEncodingCheckInPolicy;

using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

using Constants = Microsoft.VisualStudio.Shell.Interop.Constants;

namespace TsLintCheckInPolicy.VisualStudio
{
    /// <summary>
    ///     This provider is used to provide violation tasks for the Visual Studio Error List window. This class cannot be
    ///     inherited.
    /// </summary>
    internal sealed class ViolationTaskProvider : ErrorListProvider, IVsTaskProvider
    {
        /// <summary>
        ///     Defines the default provider name.
        /// </summary>
        private const string DefaultProviderName = "TsLintSourceAnalysisPolicy";

        /// <summary>
        ///     Initializes a new instance of the <see cref="ViolationTaskProvider" /> class.
        /// </summary>
        /// <param name="provider">The underlying provider for the error task provider.</param>
        /// <param name="settings">The policy settings.</param>
        public ViolationTaskProvider(IServiceProvider provider)
            : base(provider)
        {
            this.ProviderGuid = new Guid("{a27b7013-7a64-4f2f-a0f4-83173184aa5a}");
            this.ProviderName = DefaultProviderName;
            this.Provider = provider;
        }

        /// <summary>
        ///     Gets or sets the underlying service provider.
        /// </summary>
        private IServiceProvider Provider { get; set; }

        /// <summary>
        ///     Adds a new task for the violation.
        /// </summary>
        /// <param name="violation">The <see cref="Violation" /> that caused the task.</param>
        /// <exception cref="System.ArgumentNullException">
        ///     <paramref name="violation" /> is a null reference (<b>Nothing</b> in
        ///     Visual Basic).
        /// </exception>
        public void AddTask(Violation violation)
        {
            ViolationTask task = new ViolationTask(violation, this.Provider);
            task.ErrorCategory = TaskErrorCategory.Error;
            task.CanDelete = true;
            task.Category = TaskCategory.CodeSense;
            task.Document = violation.name;
            task.Line = violation.startPosition.line - 1;
            task.Column = violation.startPosition.character;

            task.Text = string.Format("({0}) {1}", violation.ruleName, violation.failure);

            this.Tasks.Add(task);
        }

        public void GotoError(Violation violation)
        {
            _DTE dte = (_DTE)this.Provider.GetService(typeof(_DTE));

            Window window = dte.OpenFile(EnvDTE.Constants.vsViewKindCode, violation.name);
            window.Activate();

            TextSelection t = window.Document.Selection as TextSelection;
            t.GotoLine(violation.startPosition.line, false);
        }

        /// <summary>
        ///     Refreshes the tasks for the provider.
        /// </summary>
        public void Clear()
        {
            this.Tasks.Clear();
            this.Refresh();
        }
    }
}