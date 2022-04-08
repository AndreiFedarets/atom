using Atom.Runtime;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Atom.Runtime.Extension.Common
{
    public static class ConsoleActions
    {
        [DllImport("kernel32")]
        private static extern bool AllocConsole();

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        private static extern bool FreeConsole();

        [ActionMethod("Open console")]
        public static void ShowConsole()
        {
            AllocConsole();
            //Reset input
            Stream standardInputStream = Console.OpenStandardInput(256);
            TextReader input = new StreamReader(standardInputStream);
            Console.SetIn(input);
            //Reset output
            Stream standardOutputStream = Console.OpenStandardOutput(256);
            TextWriter output = new StreamWriter(standardOutputStream) { AutoFlush = true };
            Console.SetOut(output);
            //Reset error
            Stream standardErrorStream = Console.OpenStandardError(256);
            TextWriter error = new StreamWriter(standardErrorStream) { AutoFlush = true };
            Console.SetError(error);
        }

        [ActionMethod("Close console")]
        public static void CloseConsole()
        {
            FreeConsole();
        }

        [ActionMethod("Write {line} to console")]
        public static void WriteLine([ActionParameter("line")]object line)
        {
            Console.WriteLine(line);
        }

        [ActionMethod("Read {line} from console")]
        public static void ReadLine([ActionParameter("line")]out string line)
        {
            line = Console.ReadLine();
        }
    }
}
