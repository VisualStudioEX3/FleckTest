using FleckTest.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FleckTest.Interfaces
{
    /// <summary>
    /// Chat Server service interface.
    /// </summary>
    interface IChatServer : IDisposable
    {
        #region Properties
        /// <summary>
        /// List of users logged in this server.
        /// </summary>
        Dictionary<Guid, UserData> Users { get; set; }

        /// <summary>
        /// Console writer and formatter service.
        /// </summary>
        IConsoleChatMessageWriter ConsoleWriter { get; set; }
        #endregion

        #region Methods & Functions
        /// <summary>
        /// Implements the server creation operation.
        /// </summary>
        /// <param name="address">Addres to create the server.</param>
        void Create(string address);

        /// <summary>
        /// Implements the close operation.
        /// </summary>
        void Close(); 
        #endregion
    }
}
