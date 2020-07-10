using System;
using System.Collections.Generic;
using Fleck;
using FleckTest.Extensions;
using FleckTest.Interfaces;
using FleckTest.Models;

namespace FleckTest.Services
{
    /// <summary>
    /// Chat server implementation using Fleck.
    /// </summary>
    public class ChatServer : IChatServer
    {
        #region Constants
        static readonly UserData SERVER_USER = new UserData("Server", new ConsoleColorScheme(ConsoleColor.DarkYellow, ConsoleColor.Black));
        #endregion

        #region Internal vars
        WebSocketServer _server;
        #endregion

        #region Properties
        /// <summary>
        /// List of users logged in this server.
        /// </summary>
        public Dictionary<Guid, UserData> Users { get; set; }

        /// <summary>
        /// List of the connection established with this server.
        /// </summary>
        public Dictionary<Guid, IWebSocketConnection> Sockets { get; set; }

        /// <summary>
        /// Console writer and formatter service.
        /// </summary>
        public IConsoleChatMessageWriter ConsoleWriter { get; set; }
        #endregion

        #region Constructor & destructor
        /// <summary>
        /// Constructor (called by IoC container).
        /// </summary>
        /// <param name="consoleWriter">Console writer and formatter service.</param>
        public ChatServer(IConsoleChatMessageWriter consoleWriter)
        {
            this.Users = new Dictionary<Guid, UserData>();
            this.Sockets = new Dictionary<Guid, IWebSocketConnection>();
            this.ConsoleWriter = consoleWriter;
        }

        public void Dispose()
        {
            this.Close();
        }
        #endregion

        #region Methods & Functions
        /// <summary>
        /// Create server instance.
        /// </summary>
        /// <param name="address">Address to create server.</param>
        public void Create(string address)
        {
            Logger.Info("Creating server...");

            this._server = new WebSocketServer(address);
            this._server.Start(socket =>
            {
                // Using OnOpen event to catch the new connections and sends the socket id to start login operations:
                socket.OnOpen = () =>
                {
                    Guid id = socket.ConnectionInfo.Id;
                    Logger.Info($"New conection from {socket.ConnectionInfo.ClientIpAddress}:{socket.ConnectionInfo.ClientPort} with id {id}.");
                    socket.Send(id.ToByteArray());
                };

                // Using OnClose event to catch the connections was closed and to remove the user and socket from the server.
                socket.OnClose = () =>
                {
                    Guid id = socket.ConnectionInfo.Id;
                    UserData user = this.Users[id];

                    this.SendAnouncement($"{user.Name} has left the conversation.", socket);

                    this.Users.Remove(id);
                    this.Sockets.Remove(id);
                };

                // Using OnMessage event to catch any message received from any active connections, to process it, and send back to all connections:
                socket.OnMessage = message =>
                {
                    this.SendServerMessage(new ServerMessage(this.Users[socket.ConnectionInfo.Id], message, false));
                };

                // Using OnBinary to complete the login process started in OnOpen event:
                socket.OnBinary = data =>
                {
                    try
                    {
                        var logReq = new LoginRequest(data);

                        // Check for duplicate usernames.
                        foreach (var user in this.Users.Values)
                        {
                            if (user.Name.ToLower() == logReq.UserName.ToLower())
                            {
                                socket.Send(SharedConstants.SERVER_USERNAME_IN_USE.ToByteArray()); // Notify that the username is already registered.
                                return;
                            }
                        }

                        socket.Send(SharedConstants.SERVER_USERNAME_AVAILABLE.ToByteArray()); // Notify that the username is available.

                        var newUser = new UserData(logReq.UserName, ConsoleColorScheme.GetNextColorScheme());

                        this.Users.Add(logReq.Id, newUser);
                        this.Sockets.Add(logReq.Id, socket);

                        socket.Send(newUser.GetSerializedData().ToArray());

                        this.SendAnouncement($"{logReq.UserName} has entered in conversation.");
                    }
                    catch (Exception ex)
                    {
                        Logger.Error($"OnBinary(): {ex.Message}", ex);
                    }
                };
            });

            // TODO: Run ping task to ping all sockets and watch when any connection is lost (no clossed properly).

            Logger.Warn("Press any key to stop server...");
            Console.ReadKey();
        }

        /// <summary>
        /// Close server instance.
        /// </summary>
        public void Close()
        {
            this._server.Dispose();
        }

        /// <summary>
        /// Sends a special message to all active connections.
        /// </summary>
        /// <param name="message">String value that contains the message.</param>
        /// <param name="ignore">Optional. Connection to ignore the message.</param>
        void SendAnouncement(string message, IWebSocketConnection ignore = null)
        {
            this.SendServerMessage(new ServerMessage(ChatServer.SERVER_USER, message, true), ignore);
        }

        /// <summary>
        /// Sends a message to all active connections.
        /// </summary>
        /// <param name="message">String value that contains the message.</param>
        /// <param name="ignore">Optional. Connection to ignore the message.</param>
        void SendServerMessage(ServerMessage message, IWebSocketConnection ignore = null)
        {
            byte[] data = message.GetSerializedData().ToArray();

            this.ConsoleWriter.Write(message);
            foreach (var socket in this.Sockets.Values)
            {
                if ((ignore is null || ignore?.ConnectionInfo.Id != socket.ConnectionInfo.Id) && socket.IsAvailable)
                {
                    socket.Send(data);
                }
            }
        } 
        #endregion
    }
}
