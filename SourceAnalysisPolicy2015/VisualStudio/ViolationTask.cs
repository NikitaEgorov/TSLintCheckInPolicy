//--------------------------------------------------------------------------
// <copyright file="ViolationTask.cs" company="Ralph Jansen">
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

namespace RalphJansen.StyleCopCheckInPolicy.VisualStudio
{
    using System;
    using EnvDTE;
    using Microsoft.VisualStudio.Shell;
    using StyleCop;

    /// <summary>
    /// Represents a violation error task. This class cannot be inherited.
    /// </summary>
    internal sealed class ViolationTask : Microsoft.VisualStudio.Shell.ErrorTask
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RalphJansen.StyleCopCheckInPolicy.VisualStudio.ViolationTask"/> class.
        /// </summary>
        /// <param name="violation">The violation that caused the task.</param>
        /// <param name="provider">The provider used for the task.</param>
        public ViolationTask(Violation violation, IServiceProvider provider)
        {
            this.Violation = violation;
            this.Provider = provider;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the underlying violation.
        /// </summary>
        public Violation Violation
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the provider used by the task.
        /// </summary>
        private IServiceProvider Provider
        {
            get;
            set;
        }

        #endregion

        /// <summary>
        /// Raises the <see cref="Navigate"/> event.
        /// </summary>
        /// <param name="e">An <see cref="System.EventArgs"/> containing event data.</param>
        protected override void OnNavigate(EventArgs e)
        {
            _DTE dte = (_DTE)this.Provider.GetService(typeof(_DTE));

            Window window = dte.OpenFile(EnvDTE.Constants.vsViewKindCode, this.Violation.SourceCode.Path);
            window.Activate();

            TextSelection t = window.Document.Selection as TextSelection;
            t.GotoLine(this.Violation.Line, false);

            base.OnNavigate(e);
        }
    }
}