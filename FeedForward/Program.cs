using System;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FeedForward.Core;

namespace FeedForward
{
    class Program
    {

        public static float LearningRate = 0.01f;

        static void Main(string[] args)
        {
            Model nn = Model.createModel().
                inputLayer(2).
                leakyReluLayer(2).
                leakyReluLayer(2).
                endModel();

            Matrix input = new Matrix(2, 1);
            input.data = new float[,]{ { 2 }, { 1 } };

            Matrix.table(input);

            Matrix output = nn.FeedForward(input);
            Matrix.table(output);

            Console.WriteLine("Done!");

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
