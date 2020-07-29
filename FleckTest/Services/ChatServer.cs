using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
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
                    this.RemoveSession(socket.ConnectionInfo.Id);
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
                        this.RegisterSession(logReq.Id, newUser, socket);
                        socket.Send(newUser.GetSerializedData().ToArray());

                        this.SendAnouncement($"{logReq.UserName} has entered in conversation.");
                    }
                    catch (Exception ex)
                    {
                        Logger.Error($"OnBinary(): {ex.Message}", ex);
                    }
                };

                // Using OnError to catch forced disconnections from clients and removing their sessions:
                socket.OnError = ex =>
                {
                    if (ex is IOException && 
                        ex.InnerException is SocketException && 
                        (ex.InnerException as SocketException).SocketErrorCode == SocketError.ConnectionReset)
                    {
                        this.RemoveSession(socket.ConnectionInfo.Id);
                    }
                };
            });

            Logger.Warn("Press any key to stop server...");
            Console.ReadKey();
        }

        /// <summary>
        /// Register new session.
        /// </summary>
        /// <param name="id"><see cref="Guid"/> session.</param>
        /// <param name="user"><see cref="UserData"/> information.</param>
        /// <param name="connection">Connection.</param>
        void RegisterSession(Guid id, UserData user, IWebSocketConnection connection)
        {
            this.Users.Add(id, user);
            this.Sockets.Add(id, connection);
        }

        /// <summary>
        /// Remove session.
        /// </summary>
        /// <param name="id"><see cref="Guid"/> session.</param>
        void RemoveSession(Guid id)
        {
            if (this.Users.TryGetValue(id, out UserData user))
            {
                this.SendAnouncement($"{user.Name} has left the conversation.", this.Sockets[id]);

                this.Users.Remove(id);
            }
            else
            {
#if DEBUG
                Logger.Debug($"Error removing session: User with id '{id}' not found. Maybe last loging request was cancelled or failed.");                 
#endif
                Logger.Info($"Removing socket connection with id '{id}'.");
            }

            this.Sockets.Remove(id);
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
