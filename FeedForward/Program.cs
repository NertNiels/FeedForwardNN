using System;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedForward
{
    class Program
    {

        public static float LearningRate = 0.01f;

        static void Main(string[] args)
        {
            while (true) ;
        }


        public static void writeErrorLine(String text, [CallerMemberName] String callerMethod = "", [CallerFilePath] String callerPath = "", [CallerLineNumber] int lineNumber = 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error:");
            Console.WriteLine(text);
            Console.WriteLine("StackTrace:\n\tMethod:\t" + callerMethod + "\n\tFile:\t" + callerPath + "\n\tLine:\t" + lineNumber);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}
