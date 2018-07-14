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
    }
}