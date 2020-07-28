using System;

namespace FleckTest.Exceptions
{
    public class LoginException : Exception
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="message">Message reason of the login exception.</param>
        public LoginException(string message) : base(message)
        {
        } 
        #endregion
    }
}
