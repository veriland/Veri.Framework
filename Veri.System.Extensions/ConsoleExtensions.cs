using System;
using System.Collections.Generic;
using System.Text;

namespace Veri.System.Extensions
{
    public static class ConsoleExtensions
    {
        private static int c = 0;
        private static string progress = @"/-\|";

        public static void ShowLoading()
        {
            Console.CursorVisible = false;
            if (c > 3)
            {
                c = 0;
            }
            Console.Write(" {0}", progress[c++]);
            Console.SetCursorPosition(Console.CursorLeft - 2, Console.CursorTop);
        }
    }
}
