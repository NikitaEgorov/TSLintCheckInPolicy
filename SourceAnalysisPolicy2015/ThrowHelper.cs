//--------------------------------------------------------------------------
// <copyright file="ThrowHelper.cs" company="Ralph Jansen">
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

namespace RalphJansen.StyleCopCheckInPolicy
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;

    /// <summary>
    /// Provides methods used to throw exceptions within the assembly.
    /// </summary>
    internal static class ThrowHelper
    {       
        /// <summary>
        /// Throws a new <see cref="System.ArgumentNullException"/> exception.
        /// </summary>
        /// <param name="argumentName">The argument name that caused the exception.</param>
        public static void ThrowArgumentNullException(string argumentName)
        {
            throw new ArgumentNullException(argumentName);
        }

        /////// <summary>
        /////// Throws a new <see cref="System.ArgumentException"/> exception.
        /////// </summary>
        /////// <param name="argumentName">The argument name that caused the exception.</param>
        /////// <param name="resource">An <see cref="System.String"/> to include with the exception message.</param>
        ////public static void ThrowArgumentException(string argumentName, string resource)
        ////{
        ////    object[] args = { argumentName };

        ////    ThrowHelper.ThrowArgumentException(argumentName, resource, args);
        ////}

        /////// <summary>
        /////// Throws a new <see cref="System.ArgumentException"/> exception.
        /////// </summary>
        /////// <param name="argumentName">The argument name that caused the exception.</param>
        /////// <param name="resource">An <see cref="System.String"/> to include with the exception message.</param>
        /////// <param name="args">A <see cref="System.Object"/> array containing zero or more items to format.</param>
        ////public static void ThrowArgumentException(string argumentName, string resource, params object[] args)
        ////{
        ////    throw new ArgumentException(ThrowHelper.FormatResourceString(resource, args), argumentName);
        ////}

        /////// <summary>
        /////// Throws a new <see cref="System.InvalidOperationException"/> exception.
        /////// </summary>
        /////// <param name="resource">An <see cref="System.String"/> to include with the exception message.</param>
        ////public static void ThrowInvalidOperationException(string resource)
        ////{
        ////    throw new InvalidOperationException(resource);
        ////}

        /////// <summary>
        /////// Throws a new <see cref="System.NotSupportedException"/> exception.
        /////// </summary>
        /////// <param name="message">A message that describes the error.</param>
        ////public static void ThrowNotSupportedException(string message)
        ////{
        ////    throw new NotSupportedException(message);
        ////}

        /////// <summary>
        /////// Throws a new <see cref="System.UnauthorizedAccessException"/> exception.
        /////// </summary>
        /////// <param name="message">A message that describes the error.</param>
        ////public static void ThrowUnauthorizedAccessException(string message)
        ////{
        ////    throw new UnauthorizedAccessException(message);
        ////}

        /////// <summary>
        /////// Replaces the format item of the <see cref="System.String"/> resource specified with the equivalent in the <paramref name="args"/> array specified.
        /////// </summary>
        /////// <param name="resource">An <see cref="System.String"/> to format.</param>
        /////// <param name="args">An <see cref="System.Object"/> array containing zero or more items to format.</param>
        /////// <returns>The formatted resource string.</returns>
        ////private static string FormatResourceString(string resource, params object[] args)
        ////{
        ////    return string.Format(CultureInfo.CurrentCulture, resource, args);
        ////}
    }
}