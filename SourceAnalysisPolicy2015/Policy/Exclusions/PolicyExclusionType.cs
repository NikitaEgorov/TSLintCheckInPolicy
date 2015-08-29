//--------------------------------------------------------------------------
// <copyright file="PolicyExclusionType.cs" company="Ralph Jansen">
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

namespace RalphJansen.StyleCopCheckInPolicy.Policy.Exclusions
{
    using System;

    /// <summary>
    /// Defines the policy exclusions available.
    /// </summary>
    internal enum PolicyExclusionType
    {
        /// <summary>
        /// No policy exclusion.
        /// </summary>
        None,

        /// <summary>
        /// The check-in is excluded because of the directory.
        /// </summary>
        [Exclusion(typeof(RalphJansen.StyleCopCheckInPolicy.Policy.Exclusions.FileSystem.DirectoryPolicyExclusion),
            "DirectoryPolicyExclusionName",
            "DirectoryPolicyExclusionDesc",
            typeof(RalphJansen.StyleCopCheckInPolicy.UI.Forms.Editors.DirectoryEditorDialog))]
        Directory,

        /// <summary>
        /// The check-in is excluded because of the name of the file.
        /// </summary>
        [Exclusion(typeof(RalphJansen.StyleCopCheckInPolicy.Policy.Exclusions.FileSystem.FilePolicyExclusion),
            "FilePolicyExclusionName",
            "FilePolicyExclusionDesc",
            typeof(RalphJansen.StyleCopCheckInPolicy.UI.Forms.Editors.FileEditorDialog))]
        FileName,

        /// <summary>
        /// The check-in is excluded because of an associated work item with a matching work item id.
        /// </summary>
        [Exclusion(typeof(RalphJansen.StyleCopCheckInPolicy.Policy.Exclusions.WorkItem.WorkItemIdExclusion), 
            "WorkItemIdExclusionName", 
            "WorkItemIdExclusionDesc", 
            typeof(RalphJansen.StyleCopCheckInPolicy.UI.Forms.Editors.WorkItemIdEditorDialog))]
        WorkItemId,

        /// <summary>
        /// The check-in is excluded because of an associated work item whose field has been set.
        /// </summary>
        [Exclusion(typeof(RalphJansen.StyleCopCheckInPolicy.Policy.Exclusions.WorkItem.WorkItemFieldExclusion), 
            "WorkItemFieldExclusionName", 
            "WorkItemFieldExclusionDesc", 
            typeof(RalphJansen.StyleCopCheckInPolicy.UI.Forms.Editors.WorkItemFieldEditorDialog))]
        WorkItemField
    }
}