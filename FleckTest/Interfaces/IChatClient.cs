using System;
using FleckTest.Models;

namespace FleckTest.Interfaces
{
    #region Enums
    public enum ChatClientResults
    {
        Ok,
        ConnectionError,
        LoginCancelled
    }
    #endregion

    /// <summary>
    /// Chat Client service interface.
    /// </summary>
    interface IChatClient : IDisposable
    {
        #region Properties
        /// <summary>
        /// User data for this client instance.
        /// </summary>
        UserData UserData { get; set; }

        /// <summary>
        /// User input and command processor service.
        /// </summary>
        IUserInputCommandProcessor UserInput { get; set; }

        /// <summary>
        /// Console write formatter service.
        /// </summary>
        IConsoleChatMessageWriter ConsoleWriter { get; set; }
        #endregion

        #region Methods & Functions
        /// <summary>
        /// Implements the connection operation to server.
        /// </summary>
        /// <param name="address">Server address.</param>
        /// <returns>Must be return true when the connection is established, and false if occurs any error.</returns>
        ChatClientResults Connect(string address);

        /// <summary>
        /// Implements the close operation.
        /// </summary>
        void Close();

        /// <summary>
        /// Implements the basic send message operation.
        /// </summary>
        /// <param name="message">String value that contains the message.</param>
        void SendMessage(string message); 
        #endregion
    }
}
