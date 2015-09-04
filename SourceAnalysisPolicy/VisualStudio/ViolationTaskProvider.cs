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

using EnvDTE;

namespace RalphJansen.StyleCopCheckInPolicy.VisualStudio
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Shell.Interop;
    using StyleCop;
    using RalphJansen.StyleCopCheckInPolicy.Policy;

    /// <summary>
    /// This provider is used to provide violation tasks for the Visual Studio Error List window. This class cannot be inherited.
    /// </summary>
    internal sealed class ViolationTaskProvider : ErrorListProvider, IVsTaskProvider
    {
        #region Fields

        /// <summary>
        /// Defines the default provider name.
        /// </summary>
        private const string DefaultProviderName = "SourceAnalysisPolicy";

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RalphJansen.StyleCopCheckInPolicy.VisualStudio.ViolationTaskProvider"/> class.
        /// </summary>
        /// <param name="provider">The underlying provider for the error task provider.</param>
        /// <param name="settings">The policy settings.</param>
        public ViolationTaskProvider(IServiceProvider provider, PolicySettings settings)
            : base(provider)
        {
            this.ProviderGuid = new Guid("{1A896300-D69E-427c-A6A1-9EA4A279A060}");
            this.ProviderName = DefaultProviderName;
            this.Settings = settings;
            this.Provider = provider;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the policy settings.
        /// </summary>
        public PolicySettings Settings
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the underlying service provider.
        /// </summary>
        private IServiceProvider Provider
        {
            get;
            set;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Refreshes the tasks for the provider.
        /// </summary>
        public void Clear()
        {
            this.Tasks.Clear();
            this.Refresh();
        }

        /// <summary>
        /// Adds a new task for the violation.
        /// </summary>
        /// <param name="violation">The <see cref="Violation"/> that caused the task.</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="violation"/> is a null reference (<b>Nothing</b> in Visual Basic).</exception>
        public void AddTask(Violation violation)
        {
            if (violation == null)
            {
                ThrowHelper.ThrowArgumentNullException("violation");
            }

            if (this.Settings.TaskCategory != PolicyTaskCategory.None)
            {
                ViolationTask task = new ViolationTask(violation, this.Provider);
                if (violation.Rule.Warning)
                {
                    task.ErrorCategory = TaskErrorCategory.Warning;
                }
                else
                {
                    switch (this.Settings.TaskCategory)
                    {
                        case PolicyTaskCategory.Error:
                            task.ErrorCategory = TaskErrorCategory.Error;
                            break;

                        case PolicyTaskCategory.Warning:
                            task.ErrorCategory = TaskErrorCategory.Warning;
                            break;

                        case PolicyTaskCategory.Message:
                            task.ErrorCategory = TaskErrorCategory.Message;
                            break;
                    }
                }

                task.CanDelete = true;
                task.Category = TaskCategory.CodeSense;
                task.Document = violation.SourceCode.Name;
                task.Line = violation.Line - 1;
                task.Text = string.Format(CultureInfo.CurrentCulture, "{0}: {1}", violation.Rule.CheckId, violation.Message);

                this.Tasks.Add(task);
            }
        }

        public void GotoError(Violation violation)
        {
            _DTE dte = (_DTE)this.Provider.GetService(typeof(_DTE));

            Window window = dte.OpenFile(EnvDTE.Constants.vsViewKindCode, violation.SourceCode.Path);
            window.Activate();

            TextSelection t = window.Document.Selection as TextSelection;
            t.GotoLine(violation.Line, false);
        }

        #endregion
    }
}