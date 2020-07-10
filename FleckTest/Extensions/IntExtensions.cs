using System;

namespace FleckTest.Extensions
{
    /// <summary>
    /// Method extensions for <see cref="int"/>s.
    /// </summary>
    public static class IntExtensions
    {
        #region Methods & Functions
        /// <summary>
        /// Serialize the <see cref="int"/> value to <see cref="byte"/> array.
        /// </summary>
        /// <param name="value"><see cref="int"/> value.</param>
        /// <returns>Returns the <see cref="byte"/> array representation.</returns>
        public static byte[] ToByteArray(this int value)
        {
            return BitConverter.GetBytes(value);
        } 
        #endregion
    }
}
