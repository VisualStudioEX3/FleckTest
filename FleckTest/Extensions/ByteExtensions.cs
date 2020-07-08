using System;
using System.Collections.Generic;
using System.Text;

namespace FleckTest.Extensions
{
    /// <summary>
    /// Method extensions for byte types.
    /// </summary>
    public static class ByteExtensions
    {
        /// <summary>
        /// Convert the byte array to UTF8 string representation.
        /// </summary>
        /// <param name="buffer">Byte array instance.</param>
        /// <returns>Returns the UTF8 string representation.</returns>
        public static string GetString(this byte[] buffer)
        {
            return Encoding.UTF8.GetString(buffer);
        }
    }
}
