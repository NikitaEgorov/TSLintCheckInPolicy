//--------------------------------------------------------------------------
// <copyright file="WorkItemExclusion.cs" company="Ralph Jansen">
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

namespace RalphJansen.StyleCopCheckInPolicy.Policy.Exclusions.WorkItem
{
    using System;

    /// <summary>
    /// Provides the base implementation for work item based exclusions. This class must be inherited.
    /// </summary>
    internal abstract class WorkItemExclusion : PolicyExclusion
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RalphJansen.StyleCopCheckInPolicy.Policy.Exclusions.WorkItem.WorkItemExclusion"/> class.
        /// </summary>
        protected WorkItemExclusion()
        {
        }

        #endregion
   }
}