using System;
using FleckTest.Models;

namespace FleckTest.Interfaces
{
    /// <summary>
    /// Console writer and formatter service interface.
    /// </summary>
    /// <remarks>This service is intended to use to format the console output messages received from server using <see cref="ServerMessage"/> data.</remarks>
    public interface IConsoleChatMessageWriter
    {
        #region Methods & Functions
        /// <summary>
        /// Writes and format a message to console.
        /// </summary>
        /// <param name="data"><see cref="ServerMessage"/> message data from server.</param>
        void Write(ServerMessage data); 
        #endregion
    }
}
