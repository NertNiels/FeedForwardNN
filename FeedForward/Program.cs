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

        public static Matrix targets = new Matrix(2, 1);

        public static float LearningRate = 0.01f;

        static void Main(string[] args)
        {
            try
            {
                String path = @"C:\Users\drumm\Desktop\NeuralNetwork\jsonFile.json";

                targets.data = new float[,] { { 0.0313f }, { 2.324f } };

                //Model nn = ModelFileHandler.LoadModel(path);

                Model nn = Model.createModel().
                    inputLayer(2).
                    leakyReluLayer(2).
                    leakyReluLayer(2).
                    endModel();




                Matrix input = new Matrix(2, 1);
                input.data = new float[,] { { 2 }, { 1 } };
                for (int i = 0; i < 1000; i++)
                {
                    Matrix.table(input);

                    Matrix output = nn.FeedForward(input);
                    Matrix.table(output);

                    nn.Backpropagate(output, targets);

                    Matrix.table(targets);
                }


                ModelFileHandler.SaveModel(path, nn);
            } catch (Exception e)
            {
                writeErrorStackTrace(e.Message, e.StackTrace);
            }
            
            while (true) ;
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
