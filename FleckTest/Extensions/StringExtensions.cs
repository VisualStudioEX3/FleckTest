using System;
using System.Text;

namespace FleckTest.Extensions
{
    /// <summary>
    /// Method extensions for <see cref="string"/>s.
    /// </summary>
    public static class StringExtensions
    {
        #region Methods & Functions
        /// <summary>
        /// Creates a new <see cref="ReadOnlyMemory{Byte}"/> over the portion of the target string.
        /// </summary>
        /// <param name="instance">The target string.</param>
        /// <returns>The read-only binary memory representation of the string, or default if text is null.</returns>
        public static ReadOnlyMemory<byte> AsMemoryByte(this string instance)
        {
            return new ReadOnlyMemory<byte>(Encoding.UTF8.GetBytes(instance));
        } 
        #endregion
    }
}
