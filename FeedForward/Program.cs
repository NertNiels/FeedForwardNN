using System;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NeuralNetwork.Core;
using NeuralNetwork.IO;

namespace FeedForward
{
    class Program
    {

        public static Matrix[] targets = new Matrix[4];

        public static float LearningRate = 0.001f;

        static void Main(string[] args)
        {

        }
        


        public static void writeErrorLine(String message, [CallerMemberName] String callerMethod = "", [CallerFilePath] String callerPath = "", [CallerLineNumber] int lineNumber = 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error:");
            Console.WriteLine(message);
            Console.WriteLine("Stack Trace:\n\tMethod:\t" + callerMethod + "\n\tFile:\t" + callerPath + "\n\tLine:\t" + lineNumber);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public static void writeErrorStackTrace(String message, String stackTrace)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error:");
            Console.WriteLine(message);
            Console.WriteLine("Stack Trace:");
            Console.WriteLine(stackTrace);

        }
    }
}
