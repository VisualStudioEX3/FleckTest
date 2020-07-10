using System;
using System.Threading;
using System.Threading.Tasks;
using FleckTest.Interfaces;

namespace FleckTest.Services
{
    /// <summary>
    /// User input and command processor.
    /// </summary>
    public class UserInput : IUserInputCommandProcessor
    {
        #region Constants
        const string PROMPT = "@:>";
        #endregion

        #region Internal vars
        CancellationTokenSource _tokenSource;
        CancellationToken _token;
        #endregion

        #region Methods & Functions
        /// <summary>
        /// Starts a loop to read user input.
        /// </summary>
        /// <param name="onMessage">Event raised when the user typed a valid message to send to server.</param>
        public void Run(Action<string> onMessage)
        {
            this._tokenSource = new CancellationTokenSource();
            this._token = this._tokenSource.Token;

            // Create a parallel task to print the prompt and update his position when the console prints new lines:
            Task.Factory.StartNew(() =>
            {
                int row = Console.CursorTop;

                while (!this._token.IsCancellationRequested)
                {
                    Console.CursorLeft = 0;
                    Console.Write(UserInput.PROMPT);

                    while (row == Console.CursorTop) { }
                    row = Console.CursorTop;
                }
            }, this._token);

            while (!this._token.IsCancellationRequested)
            {
                Console.CursorLeft = UserInput.PROMPT.Length;
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

        public void Stop()
        {
            this._tokenSource?.Cancel();
        }
        #endregion
    }
}
