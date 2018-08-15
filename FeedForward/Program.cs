using System;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NeuralNetwork.Core;
using NeuralNetwork.IO;
using NeuralNetwork.Layers;
using NeuralNetwork.Layers.Convolution;

namespace FeedForward
{
    class Program
    {

        public static Matrix[] targets = new Matrix[4];

        public static float LearningRate = 0.001f;

        static void Main(string[] args)
        {

            LayerBase[] layers = new LayerBase[]
            {
                new InputLayer(1, 6, 6, 1, 4, 4, 0, 1),
                new ConvolutionLayer(1, 6, 6, 4, 4, 1, 2, 2, 0, 1),
                new NeuralNetwork.Layers.Convolution.LeakyReluLayer(),
                new ConvolutionLayer(1, 3, 3, 2, 2, 0, 0, 0, 0, 1),
                new NeuralNetwork.Layers.Convolution.LeakyReluLayer()
            };

            Matrix input = new Matrix(6, 6)
            {
                data = new float[,]
                {
                    { 0.2f,     0.4f,   0.1f,   0.7f,   0.76f,  0.54f },
                    { 0.2f,     0.4f,   0.54f,  0.1f,   0.7f,   0.76f },
                    { 0.2f,     0.4f,   0.1f,   0.54f,  0.7f,   0.76f },
                    { 0.2f,     0.1f,   0.4f,   0.7f,   0.76f,  0.54f },
                    { 0.2f,     0.4f,   0.1f,   0.7f,   0.76f,  0.54f },
                    { 0.2f,     0.4f,   0.1f,   0.7f,   0.76f,  0.54f },
                }
            };

            Matrix targets = new Matrix(2, 2)
            {
                data = new float[,]
                {
                    { 0.4f,     0.5f },
                    { 0.1f,     0.7f },
                }
            };

            Model model = new Model(layers);
            model.randomizeWeights(new Random());

            Console.WriteLine("Output Before:");
            Matrix.table(model.ConvolutionFeedForward(input));

            for (int i = 0; i < 1000; i++)
            {
                Matrix output = model.ConvolutionFeedForward(input);
                model.Backpropagate(output, targets);
            }

            Console.WriteLine("Output After:");
            Matrix.table(model.ConvolutionFeedForward(input));

            Console.WriteLine("Targets:");
            Matrix.table(targets);
            Console.Read();
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
