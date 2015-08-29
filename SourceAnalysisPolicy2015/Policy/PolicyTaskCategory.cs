//--------------------------------------------------------------------------
// <copyright file="PolicyTaskCategory.cs" company="Ralph Jansen">
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

namespace RalphJansen.StyleCopCheckInPolicy.Policy
{
    using System;

    /// <summary>
    /// Defines the policy task categories for violation tasks.
    /// </summary>
    internal enum PolicyTaskCategory
    {
        /// <summary>
        /// No task category.
        /// </summary>
        None,

        /// <summary>
        /// Error task category.
        /// </summary>
        Error,

        /// <summary>
        /// Warning task category.
        /// </summary>
        Warning,

        /// <summary>
        /// Message task category.
        /// </summary>
        Message
    }
}