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

        public static Matrix targets = new Matrix(2, 1);

        public static float LearningRate = 0.01f;

        static void Main(string[] args)
        {

            targets.data = new float[,] { { 0.5f }, { 2.3f } };
            Model nn = Model.createModel().
                inputLayer(2).
                leakyReluLayer(2).
                leakyReluLayer(2).
                endModel();

            Matrix input = new Matrix(2, 1);
            input.data = new float[,] { { 2 }, { 1 } };
            for (int i = 0; i < 100; i++) {
                Matrix.table(input);

                Matrix output = nn.FeedForward(input);
                Matrix.table(output);

                nn.Backpropagate(output, targets);

                Matrix.table(targets);

                Console.WriteLine("Done Backpropagate!");
            }

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
