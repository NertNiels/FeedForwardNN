﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuralNetwork.Core;

namespace NeuralNetwork.Layers
{
    public class InputLayer : LayerBase
    {
        public InputLayer()
        {
            this.layerType = LayerType.Input;
        }

        public override void FeedForward(LayerBase input)
        {
            if (dropout != 0f)
            {
                Random r = new Random();
                for(int i = 0; i < nodes; i++)
                {
                    if (r.NextDouble() < dropout) values.data[i, 0] = 0f;
                }
            }
        }

        public override void Backpropagate(LayerBase input, Matrix errors)
        {

        }

        public override void Backpropagate(LayerBase input, LayerBase output)
        {

        }

        public override void initWeights(Random r, LayerBase nextLayer)
        {
            this.weights = new Matrix(nextLayer.nodes, this.nodes);
            this.bias = new Matrix(this.nodes, 1);
            weights.randomize(r);
            this.bias.randomize(r);
        }

        public override void initWeights(Random r)
        {
            this.bias = new Matrix(this.nodes, 1);
            this.bias.randomize(r);
        }
    }
}
