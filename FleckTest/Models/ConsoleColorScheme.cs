using System;
using FleckTest.Interfaces;

namespace FleckTest.Models
{
    /// <summary>
    /// Color schemes used with <see cref="ServerMessage"/> and <see cref="IConsoleChatMessageWriter"/> implementations.
    /// </summary>
    public struct ConsoleColorScheme
    {
        #region Constants
        const int MAX_INDEX = 12;
        #endregion

        #region Internal vars
        static int _colorSchemeIndex = 0;
        #endregion

        #region Public vars
        public readonly ConsoleColor text;
        public readonly ConsoleColor background;
        #endregion

        #region Constructor
        static ConsoleColorScheme()
        {
            // Sets the first color scheme randomly:
            ConsoleColorScheme._colorSchemeIndex = new Random().Next(ConsoleColorScheme.MAX_INDEX + 1);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="text">Font color.</param>
        /// <param name="background">Background color.</param>
        public ConsoleColorScheme(ConsoleColor text, ConsoleColor background)
        {
            this.text = text;
            this.background = background;
        }
        #endregion

        #region Methods & Functions
        /// <summary>
        /// Get the next predefined color scheme.
        /// </summary>
        /// <returns>Returns the next <see cref="ConsoleColorScheme"/>.</returns>
        public static ConsoleColorScheme GetNextColorScheme()
        {
            if (++ConsoleColorScheme._colorSchemeIndex == ConsoleColorScheme.MAX_INDEX)
            {
                ConsoleColorScheme._colorSchemeIndex = 0;
            }

            switch (ConsoleColorScheme._colorSchemeIndex)
            {
                case 0:  return new ConsoleColorScheme(ConsoleColor.White, ConsoleColor.DarkBlue);
                case 1:  return new ConsoleColorScheme(ConsoleColor.White, ConsoleColor.DarkGreen);
                case 2:  return new ConsoleColorScheme(ConsoleColor.White, ConsoleColor.DarkCyan);
                case 3:  return new ConsoleColorScheme(ConsoleColor.White, ConsoleColor.DarkRed);
                case 4:  return new ConsoleColorScheme(ConsoleColor.White, ConsoleColor.DarkMagenta);
                case 5:  return new ConsoleColorScheme(ConsoleColor.White, ConsoleColor.DarkYellow);
                case 6:  return new ConsoleColorScheme(ConsoleColor.Black, ConsoleColor.Gray);
                case 7:  return new ConsoleColorScheme(ConsoleColor.White, ConsoleColor.DarkGray);
                case 8:  return new ConsoleColorScheme(ConsoleColor.White, ConsoleColor.Blue);
                case 9:  return new ConsoleColorScheme(ConsoleColor.White, ConsoleColor.Green);
                case 10: return new ConsoleColorScheme(ConsoleColor.Black, ConsoleColor.Cyan);
                case 11: return new ConsoleColorScheme(ConsoleColor.White, ConsoleColor.Red);
                default: return new ConsoleColorScheme(ConsoleColor.Black, ConsoleColor.White);
            }
        } 
        #endregion
    }
}
