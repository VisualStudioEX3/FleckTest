﻿using System;
using System.Threading;
using System.Threading.Tasks;
using System.Net.WebSockets;
using FleckTest.Interfaces;
using FleckTest.Extensions;
using FleckTest.Models;

namespace FleckTest.Services
{
    /// <summary>
    /// Chat client implementation.
    /// </summary>
    public class ChatClient : IChatClient
    {
        #region Internal vars
        ClientWebSocket _client;
        CancellationTokenSource _listenerCancellationTokenSource;
        Task _listener;
        #endregion

        #region Properties
        /// <summary>
        /// User data for this client instance.
        /// </summary>
        public UserData UserData { get; set; }

        /// <summary>
        /// User input and command processor service.
        /// </summary>
        public IUserInputCommandProcessor UserInput { get; set; }

        /// <summary>
        /// Console write formatter service.
        /// </summary>
        public IConsoleChatMessageWriter ConsoleWriter { get; set; }

        #endregion

        #region Constructor & destructor
        /// <summary>
        /// Constructor (called by IoC container).
        /// </summary>
        /// <param name="userInput">User Input service.</param>
        /// <param name="consoleWriter">Console writer and formatter service.</param>
        public ChatClient(IUserInputCommandProcessor userInput, IConsoleChatMessageWriter consoleWriter)
        {
            this.UserInput = userInput;
            this.ConsoleWriter = consoleWriter;

            this._listenerCancellationTokenSource = new CancellationTokenSource();
        }
        
        public void Dispose()
        {
            this.Close();
        }
        #endregion

        #region Methods & Functions
        /// <summary>
        /// Start connection with server.
        /// </summary>
        /// <param name="address">Server address.</param>
        /// <returns>Returns true if the connection is established.</returns>
        /// <remarks>After sucesfull connection to server, this function request the user name and log in to server.</remarks>
        public bool Connect(string address)
        {
            return this.ConnectAsync(address).Result;
        }

        async Task<bool> ConnectAsync(string address)
        {
            this._client = new ClientWebSocket();

            try
            {
                Fleck.FleckLog.Info("Connecting to server...");
                await this._client.ConnectAsync(new Uri(address), CancellationToken.None);

                Fleck.FleckLog.Info("Connection established!");
                this.UserData = await this.Login();

                this._listener = this.StartListener();

                // TODO: Start ping task to server.
                // TODO: Start pong task from server.

                return true;
            }
            catch (WebSocketException ex)
            {
                Fleck.FleckLog.Error($"Connection error: {ex.Message}", ex);
                return false;
            }
        }

        /// <summary>
        /// Close connection with server.
        /// </summary>
        public void Close()
        {
            this.CloseAsync();
        }

        async void CloseAsync()
        {
            try
            {
                if (this._client.State == WebSocketState.Open)
                {
                    this._listenerCancellationTokenSource.Cancel();
                    this._listener.Dispose();

                    await this._client.CloseAsync(WebSocketCloseStatus.NormalClosure, "Clossing connection with server.", CancellationToken.None);
                }
            }
            finally
            {
                // Avoid exceptions by pending transfers between server and client during the closing process and them are not would cancelled already.
            }
        }

        /// <summary>
        /// Request user name.
        /// </summary>
        /// <returns>Returns the user name typed by user in the command line.</returns>
        string RequestUserName()
        {
            string userName = string.Empty;
            while (string.IsNullOrEmpty(userName))
            {
                Console.Write("Enter a name to log in the chat room: ");
                userName = Console.ReadLine().Trim();
            }

            return userName;
        }

        /// <summary>
        /// Login to server.
        /// </summary>
        /// <returns>Returns the <see cref="UserData"/> information validated by the server.</returns>
        async Task<UserData> Login()
        {
            string userName = this.RequestUserName();
            ReadOnlyMemory<byte> data;

            // Receive the socket id from server as id to login request:
            data = await this.ReceiveDataAsync();
            Guid id = new Guid(data.ToArray());

            // Create LoginRequest with id and username and sending to server:
            var req = new LoginRequest(id, userName);
            this.SendData(req.GetSerializedData());

            // Wait and receive the UserData information from the server session created:
            data = await this.ReceiveDataAsync();
            return new UserData(data);
        }

        /// <summary>
        /// Sends a text message to server.
        /// </summary>
        /// <param name="message">String value that contains the message.</param>
        public void SendMessage(string message)
        {
            this.SendAsync(message.AsMemoryByte(), WebSocketMessageType.Text);
        }

        /// <summary>
        /// Sends a binary data to server.
        /// </summary>
        /// <param name="data"><see cref="ReadOnlyMemory{byte}"/> that contain the data.</param>
        public void SendData(ReadOnlyMemory<byte> data)
        {
            this.SendAsync(data, WebSocketMessageType.Binary);
        }

        async void SendAsync(ReadOnlyMemory<byte> data, WebSocketMessageType type)
        {
            await this._client.SendAsync(data, type, true, CancellationToken.None);
        }

        /// <summary>
        /// Starts a listener in a separate thread.
        /// </summary>
        /// <returns>Returns the <see cref="Task"/> instance of the listener.</returns>
        public Task StartListener()
        {
            return Task.Factory.StartNew(async () =>
            {
                while (this._client.State == WebSocketState.Open)
                {
                    ReadOnlyMemory<byte> buffer = await this.ReceiveDataAsync(this._listenerCancellationTokenSource.Token);
                    this.ConsoleWriter.Write(new ServerMessage(buffer));
                }
            }, this._listenerCancellationTokenSource.Token);
        }

        /// <summary>
        /// Receive data from server.
        /// </summary>
        /// <returns>Returns a <see cref="ReadOnlyMemory{byte}"/> instance with the data received.</returns>
        async Task<ReadOnlyMemory<byte>> ReceiveDataAsync()
        {
            return await this.ReceiveDataAsync(CancellationToken.None);
        }

        /// <summary>
        /// Receive data from server.
        /// </summary>
        /// <param name="token"><see cref="CancellationToken"/> token to cancel the operation.</param>
        /// <returns>Returns a <see cref="ReadOnlyMemory{byte}"/> instance with the data received.</returns>
        async Task<ReadOnlyMemory<byte>> ReceiveDataAsync(CancellationToken token)
        {
            var buffer = new Memory<byte>(new byte[1024]);

            ValueWebSocketReceiveResult valueTask = await this._client.ReceiveAsync(buffer, token);

            if (valueTask.Count > 0)
            {
                var data = new byte[valueTask.Count];
                Buffer.BlockCopy(buffer.ToArray(), 0, data, 0, data.Length);

                return new ReadOnlyMemory<byte>(data);
            }

            throw new WebSocketException("ChatClient.ReceiveData(): Error receiving data from server.");
        }
        #endregion
    }
}
