using System;
using FleckTest.Interfaces;

namespace FleckTest.Services
{
    /// <summary>
    /// User input and command processor.
    /// </summary>
    public class UserInput : IUserInputCommandProcessor
    {
        /// <summary>
        /// Starts a loop to read user input.
        /// </summary>
        /// <param name="onMessage">Event raised when the user typed a valid message to send to server.</param>
        public void Run(Action<string> onMessage)
        {
            for(;;)
            {
                //Console.Write("\n>");
                string input = Console.ReadLine().Trim();
                Console.CursorTop--;

                if (input.ToLower() == "exit")
                {
                    return;
                }

                if (input.Length > 0)
                {
                    onMessage?.Invoke(input);
                }
            }
        }
    }
}
