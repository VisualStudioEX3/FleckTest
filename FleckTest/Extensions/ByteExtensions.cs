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
        /// Convert the <see cref="byte"/> array to UTF8 string representation.
        /// </summary>
        /// <param name="buffer"><see cref="byte"/> array instance.</param>
        /// <returns>Returns the UTF8 string representation.</returns>
        public static string GetString(this byte[] buffer)
        {
            return Encoding.UTF8.GetString(buffer);
        }

        /// <summary>
        /// Convert the byte array to <see cref="int"/> representation.
        /// </summary>
        /// <param name="buffer"><see cref="byte"/> array instance.</param>
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

        /// <summary>
        /// Compare this <see cref="byte"/> array to another.
        /// </summary>
        /// <param name="buffer"><see cref="byte"/> array instance.</param>
        /// <param name="other"><see cref="byte"/> array to compare.</param>
        /// <returns>Returns true if both arrays has equal values.</returns>
        public static bool Compare(this byte[] buffer, byte[] other)
        {
            if (buffer.Length != other.Length)
            {
                return false;
            }
            else
            {
                for (int i = 0; i < buffer.Length; i++)
                {
                    if (buffer[i] != other[i])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Compare this <see cref="byte"/> array to another.
        /// </summary>
        /// <param name="buffer"><see cref="byte"/> array instance.</param>
        /// <param name="other"><see cref="ReadOnlyMemory{byte}"/> buffer to compare.</param>
        /// <returns>Returns true if both instances has equal values.</returns>
        public static bool Compare(this byte[] buffer, ReadOnlyMemory<byte> other)
        {
            return buffer.Compare(other.ToArray());
        }

        /// <summary>
        /// Compare this <see cref="byte"/> array to another.
        /// </summary>
        /// <param name="buffer"><see cref="ReadOnlyMemory{byte}"/> instance.</param>
        /// <param name="other"><see cref="byte"/> array to compare.</param>
        /// <returns>Returns true if both instances has equal values.</returns>
        public static bool Compare(this ReadOnlyMemory<byte> buffer, byte[] other)
        {
            return buffer.ToArray().Compare(other);
        }

        /// <summary>
        /// Compare this <see cref="ReadOnlyMemory{byte}"/> instance to another.
        /// </summary>
        /// <param name="buffer"><see cref="ReadOnlyMemory{byte}"/> instance.</param>
        /// <param name="other"><see cref="ReadOnlyMemory{byte}"/> buffer to compare.</param>
        /// <returns>Returns true if both instances has equal values.</returns>
        public static bool Compare(this ReadOnlyMemory<byte> buffer, ReadOnlyMemory<byte> other)
        {
            return buffer.Compare(other.ToArray());
        }
        #endregion
    }
}
