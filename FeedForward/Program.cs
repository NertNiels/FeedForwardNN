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
            while (true)
            {
                try
                {
                    String path = @"C:\Users\drumm\Desktop\NeuralNetwork\jsonFile.json";

                    targets[0] = new Matrix(1, 1);
                    targets[0].data = new float[,] { { 0 } };
                    targets[1] = new Matrix(1, 1);
                    targets[1].data = new float[,] { { 0 } };
                    targets[2] = new Matrix(1, 1);
                    targets[2].data = new float[,] { { 1 } };
                    targets[3] = new Matrix(1, 1);
                    targets[3].data = new float[,] { { 1 } };



                    //Model nn = ModelFileHandler.LoadModel(path);



                    Model nn = Model.createModel().
                        inputLayer(2, 0f).
                        leakyReluLayer(2, 0f).
                        leakyReluLayer(1, 0f).
                        endModel();




                    Matrix[] inputs = new Matrix[4];

                    inputs[0] = new Matrix(2, 1);
                    inputs[0].data = new float[,] { { 0 }, { 0 } };
                    inputs[1] = new Matrix(2, 1);
                    inputs[1].data = new float[,] { { 1 }, { 1 } };
                    inputs[2] = new Matrix(2, 1);
                    inputs[2].data = new float[,] { { 1 }, { 0 } };
                    inputs[3] = new Matrix(2, 1);
                    inputs[3].data = new float[,] { { 0 }, { 1 } };

                    Random r = new Random();
                    for (int i = 0; i < 2000; i++)
                    {
                        int index = (int)Math.Round(r.NextDouble() * 3);

                        Console.WriteLine("=======================================");
                        Console.WriteLine("Iteration " + (i + 1) + "\n");
                        Console.WriteLine("Input:");
                        Matrix.table(inputs[index]);

                        Matrix output = nn.FeedForward(inputs[index]);
                        Console.WriteLine("Output:");
                        Matrix.table(output);

                        nn.Backpropagate(output, targets[index]);

                        Console.WriteLine("Targets:");
                        Matrix.table(targets[index]);
                    }


                }
                catch (Exception e)
                {
                    writeErrorStackTrace(e.Message, e.StackTrace);
                }


                Console.Read();
                Console.Clear();
            }
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
