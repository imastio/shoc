using System;
using System.Text;

namespace Shoc.Cli.Utility
{
    /// <summary>
    /// The console input utilities
    /// </summary>
    public static class ShocConsole
    {
        /// <summary>
        /// Gets the password from console input
        /// </summary>
        /// <returns></returns>
        public static string GetPassword()
        {
            // init password
            var password = new StringBuilder();

            // loop for each entered key
            while (true)
            {
                // get the key
                var i = Console.ReadKey(true);

                // in case Enter then exit
                if (i.Key == ConsoleKey.Enter)
                {
                    break;
                }

                // in case backspace and password not empty, delete last char
                if (i.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password.Remove(password.Length - 1, 1);
                    Console.Write("\b \b");
                }
                // KeyChar == '\u0000' if the key pressed does not correspond to a printable character, e.g. F1, Pause-Break, etc
                else if (i.KeyChar != '\u0000')
                {
                    password.Append(i.KeyChar);
                    Console.Write("*");
                }
            }

            // return password string
            return password.ToString();
        }

        /// <summary>
        /// Yes/no confirm in console
        /// </summary>
        /// <returns></returns>
        public static bool Confirm()
        {
            // get the key
            var response = Console.ReadKey(false).Key;

            return response == ConsoleKey.Y;
        }
    }
}