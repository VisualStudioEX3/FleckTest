using System;
using FleckTest.Interfaces;
using FleckTest.Services;

namespace FleckTest
{
    /// <summary>
    /// Basic client/server chat program in console mode.
    /// </summary>
    class Program
    {
        #region Methods & Functions
        static void Main(string[] args)
        {
            string serverAddress;

            Logger.ShowFullExceptions = false;

            if (Program.ReadArgs(args, out serverAddress))
            {
                Program.InitializeServices();
                Program.RunClient(serverAddress, () => Program.RunServer(serverAddress));
            }

            Console.Write("\nPress any key to exit...");
            Console.ReadKey();
        }

        /// <summary>
        /// Read app arguments and get the port number.
        /// </summary>
        /// <param name="args">Args list.</param>
        /// <param name="address">Out parameter with the full address created using the port number.</param>
        /// <returns>Returns true if the first argument contains a valid port number.</returns>
        static bool ReadArgs(string[] args, out string address)
        {
            address = string.Empty;

            if (args.Length > 0)
            {
                ushort port;
                if (ushort.TryParse(args[0], out port))
                {
                    address = $"ws://127.0.0.1:{port}"; // For this test, will be only working in localhost to run the server and client instances.
                }
                else
                {
                    Console.WriteLine($"The argument isn't a valid port number [0 - {ushort.MaxValue}]");
                    return false;
                }

                return true;
            }
            else
            {
                string filename = System.IO.Path.GetFileName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
                Console.WriteLine($"Missing argument: port number\nThe correct usage of this program is: {filename} PORT_NUMBER");

                return false;
            }
        }

        /// <summary>
        /// Register and initialize services.
        /// </summary>
        static void InitializeServices()
        {
            ServiceManager.RegisterSingleton<IChatClient, ChatClient>();
            ServiceManager.RegisterSingleton<IChatServer, ChatServer>();
            ServiceManager.RegisterSingleton<IUserInputCommandProcessor, UserInput>();
            ServiceManager.RegisterSingleton<IConsoleChatMessageWriter, ConsoleChatMessageWriter>();

            ServiceManager.InitializeServices();
        }

        /// <summary>
        /// Create and run client instance.
        /// </summary>
        /// <param name="serverAddress">Server address to connect.</param>
        /// <param name="onConnectionFail">Event to run in case of the connection fail.</param>
        static void RunClient(string serverAddress, Action onConnectionFail)
        {
            Console.Title = "Fleck test: client mode";
            using (var client = ServiceManager.GetService<IChatClient>())
            {
                if (client.Connect(serverAddress) == ChatClientResults.ConnectionError)
                {
                    onConnectionFail?.Invoke();
                }
            }
        }

        /// <summary>
        /// Create and run server instance.
        /// </summary>
        /// <param name="address">Address to create the instance.</param>
        static void RunServer(string address)
        {
            Console.Title = "Fleck test: server mode";
            using (var server = ServiceManager.GetService<IChatServer>())
            {
                server.Create(address);
            }
        }
    } 
    #endregion
}
