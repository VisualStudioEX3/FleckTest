using System;
using System.Text;

namespace FleckTest.Extensions
{
    /// <summary>
    /// Method extensions for <see cref="byte"/> and <see cref="ReadOnlyMemory{byte}"/> types.
    /// </summary>
    public static class ByteExtensions
    {
        #region Methods & Functions
        /// <summary>
        /// Convert the byte array to UTF8 string representation.
        /// </summary>
        /// <param name="buffer">Byte array instance.</param>
        /// <returns>Returns the UTF8 string representation.</returns>
        public static string GetString(this byte[] buffer)
        {
            return Encoding.UTF8.GetString(buffer);
        }

        /// <summary>
        /// Convert the byte array to <see cref="int"/> representation.
        /// </summary>
        /// <param name="buffer">Byte array instance.</param>
        /// <returns>Returns the <see cref="int"/> representation.</returns>
        public static int GetInt(this byte[] buffer)
        {
            return BitConverter.ToInt32(buffer);
        }

        /// <summary>
        /// Convert the <see cref="ReadOnlyMemory{byte}"/> data to <see cref="int"/> representation.
        /// </summary>
        /// <param name="buffer"><see cref="ReadOnlyMemory{byte}"/> instance.</param>
        /// <returns>Returns the <see cref="int"/> representation.</returns>
        public static int GetInt(this ReadOnlyMemory<byte> buffer)
        {
            return buffer.ToArray().GetInt();
        } 
        #endregion
    }
}
