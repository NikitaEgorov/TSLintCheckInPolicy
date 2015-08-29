//--------------------------------------------------------------------------
// <copyright file="PathExclusionType.cs" company="Ralph Jansen">
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

namespace RalphJansen.StyleCopCheckInPolicy.Policy.Exclusions.FileSystem
{
    using System;

    /// <summary>
    /// Defines the types of path exclusions.
    /// </summary>
    internal enum PathExclusionType
    {
        /// <summary>
        /// The path exclusion is a literal value.
        /// </summary>
        Literal,

        /// <summary>
        /// The path exclusion is a regular expression.
        /// </summary>
        Regex
    }
}