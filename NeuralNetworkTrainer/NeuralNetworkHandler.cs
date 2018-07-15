using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using NeuralNetwork.Core;
using NeuralNetwork.Layers;

namespace NeuralNetworkTrainer
{
    public static class NeuralNetworkHandler
    {
        public static Model network;

        public static void CreateNetwork(LayerBase[] layers)
        {
            network = new Model(layers);
        }
        
        public static LayerBase[] getLayers()
        {
            if (network == null) return null;
            return network.layers;
        } 

        public static void Train()
        {
            for(int i = 0; i < DataKeeper.DataSet.Length; i++)
            {

            }
        }
    }

    public static class DataKeeper
    {
        public static List<float> Loss = new List<float>();

        public static Data[] DataSet;

        public static void shuffleDataSet()
        {
            Random r = new Random();
            
            for(int i = 0; i < DataSet.Length; i++)
            {
                int b = r.Next(DataSet.Length - i);
                Data t = DataSet[i];
                DataSet[i] = DataSet[b];
                DataSet[b] = t;
            }
        }
    }

    public class Data
    {
        public Matrix Inputs;
        public Matrix Targets;
    }
}