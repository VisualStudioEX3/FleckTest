using System;
using FleckTest.Interfaces;
using FleckTest.Models;

namespace FleckTest.Services
{
    /// <summary>
    /// Console writer and formatter service.
    /// </summary>
    /// <remarks>This service is used to print in console the timestamp and username using an user associated colors (to ease identify each user), and the message in normal colors.</remarks>
    public class ConsoleChatMessageWriter : IConsoleChatMessageWriter
    {
        #region Methods & Functions
        /// <summary>
        /// Writes a server message.
        /// </summary>
        /// <param name="data"><see cref="ServerMessage"/> to process.</param>
        public void Write(ServerMessage data)
        {
            UserData user = data.User;
            ConsoleColorScheme color = user.Color;

            Console.BackgroundColor = color.background;
            Console.ForegroundColor = color.text;

            if (data.IsAServerAnouncement)
            {
                Console.WriteLine($"{data.TimeStamp.ToShortTimeString()}: {data.Message}");
            }
            else
            {
                Console.Write($" {data.TimeStamp.ToShortTimeString()} {user.Name} ");

                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = color.background;
                Console.WriteLine($" {data.Message}");
            }

            Console.ResetColor();
        } 
        #endregion
    }
}
