using System;
using Fleck;

namespace FleckTest
{
    /// <summary>
    /// A wrapper of <see cref="FleckLog"/> class to add color in each type of log message and simplified the format.
    /// </summary>
    static class Logger
    {
        #region Properties
        /// <summary>
        /// Sets if the logger must be print all exception data or only the exception message.
        /// </summary>
        public static bool ShowFullExceptions { get; set; } 
        #endregion

        #region Constructor
        static Logger()
        {
            Logger.ShowFullExceptions = true;

            FleckLog.LogAction = (level, message, ex) =>
            {
                lock (new object())
                {
                    Console.BackgroundColor = ConsoleColor.Black;

                    switch (level)
                    {
                        case LogLevel.Debug:
#if DEBUG
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            break;
#else
                            return;
#endif
                        case LogLevel.Error:
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            break;
                        case LogLevel.Warn:
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            break;
                        default:
                            Console.ForegroundColor = ConsoleColor.White;
                            break;
                    }

                    if (!Logger.ShowFullExceptions && ex != null)
                    {
                        message = $"{message} : {ex.Message}";
                        ex = null;
                    }

                    Console.WriteLine($"[{DateTime.Now.ToShortTimeString()}] {message} {(ex is null ? string.Empty : ex.ToString())}");
                    Console.ResetColor(); 
                }
            };
        } 
        #endregion

        #region Methods & Functions
        static void Log(string message, LogLevel level, Exception ex = null)
        {
            switch (level)
            {
                case LogLevel.Debug:
                    FleckLog.Debug(message, ex);
                    break;
                case LogLevel.Info:
                    FleckLog.Info(message, ex);
                    break;
                case LogLevel.Warn:
                    FleckLog.Warn(message, ex);
                    break;
                case LogLevel.Error:
                    FleckLog.Error(message, ex);
                    break;
            }

            Console.ResetColor();
        }

        /// <summary>
        /// Log debug message to console.
        /// </summary>
        /// <param name="message">Message to log.</param>
        /// <param name="ex">Optional. Exception to log.</param>
        public static void Debug(string message, Exception ex = null)
        {
            Logger.Log(message, LogLevel.Debug, ex);
        }

        /// <summary>
        /// Log information message to console.
        /// </summary>
        /// <param name="message">Message to log.</param>
        /// <param name="ex">Optional. Exception to log.</param>
        public static void Info(string message, Exception ex = null)
        {
            Logger.Log(message, LogLevel.Info, ex);
        }

        /// <summary>
        /// Log warning message to console.
        /// </summary>
        /// <param name="message">Message to log.</param>
        /// <param name="ex">Optional. Exception to log.</param>
        public static void Warn(string message, Exception ex = null)
        {
            Logger.Log(message, LogLevel.Warn, ex);
        }

        /// <summary>
        /// Log error message to console.
        /// </summary>
        /// <param name="message">Message to log.</param>
        /// <param name="ex">Optional. Exception to log.</param>
        public static void Error(string message, Exception ex = null)
        {
            Logger.Log(message, LogLevel.Error, ex);
        } 
        #endregion
    }
}
