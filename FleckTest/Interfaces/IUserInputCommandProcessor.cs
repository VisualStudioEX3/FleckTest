using System;

namespace FleckTest.Interfaces
{
    /// <summary>
    /// User Input and command processor service interface.
    /// </summary>
    /// <remarks>This service is used to implements a command line to able the user enter messages and commands to operate with the client.</remarks>
    public interface IUserInputCommandProcessor
    {
        #region Methods & Functions
        /// <summary>
        /// Must be implements a loop operation that wait and read user input typed in console and process it.
        /// </summary>
        /// <param name="onMessage">Event raised when the user typed a valid message to send to server. Must be avoid when user enter a defined command.</param>
        void Run(Action<string> onMessage); 
        #endregion
    }
}
