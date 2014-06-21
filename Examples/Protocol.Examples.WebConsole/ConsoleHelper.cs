using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protocol.Examples.WebConsole
{
    public class ConsoleHelper
    {
        public static bool ReadString(out String result)
        {
            result = null;
            String buffer = String.Empty;

            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Escape)
                {
                    return false;
                }

                if (key.Key == ConsoleKey.Enter)
                {
                    break;
                }

                if (key.Key == ConsoleKey.Backspace)
                {
                    if (buffer.Length > 1)
                    {
                        buffer = buffer.Substring(0, buffer.Length - 1);
                    }
                }

                if (Char.IsLetterOrDigit(key.KeyChar) || key.KeyChar == ' ' || Char.IsSymbol(key.KeyChar))
                {
                    Console.Write(key.KeyChar);
                    buffer += key.KeyChar;
                }
            }
            result = buffer;

            return true;
        }

        public static bool ReadDecimalNumber(out int result)
        {
            result = -1;
            String buffer = String.Empty;

            while(true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Escape)
                {
                    return false;
                }

                if (key.Key == ConsoleKey.Enter)
                {
                    break;
                }

                if (key.Key == ConsoleKey.Backspace)
                {
                    if (buffer.Length > 1)
                    {
                        buffer = buffer.Substring(0, buffer.Length - 1);
                    }
                }

                if (Char.IsDigit(key.KeyChar))
                {
                    Console.Write(key.KeyChar);
                    buffer += key.KeyChar;
                }
            }

            result = Int32.Parse(buffer);
            return true;
        }
    }
}
