﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FeedForward.Core;

namespace FeedForward.Layers
{
    class LeakyReluLayer : LayerBase
    {

        public LeakyReluLayer(int nodes)
        {
            this.nodes = nodes;
        }
        
        public override void FeedForward(LayerBase input)
        {
            this.values = Matrix.multiply(input.weights, input.values);
            this.values.add(this.bias);
            this.values.map(Activation.lrelu);
        }

        public override void Backpropagate(LayerBase input, LayerBase output, Matrix errors)
        {
            // Calculating Errors
            this.errors = errors;

            // Calculating Gradients
            Matrix gradients = Matrix.map(Activation.dlrelu, this.values);
            gradients.hadamard(this.errors);
            gradients.multiply(Program.LearningRate);

            // Calculate Deltas
            Matrix inputValues_T = Matrix.transpose(input.values);
            Matrix deltas = Matrix.multiply(gradients, inputValues_T);

            // Updating Weights and Biases
            input.weights.add(deltas);
            this.bias.add(gradients);

        }

        public override void Backpropagate(LayerBase input, LayerBase output)
        {
            // Calculating Errors
            Matrix weights_T = Matrix.transpose(this.weights);
            this.errors = Matrix.multiply(weights_T, output.errors);

            // Calculating Gradients
            Matrix gradients = Matrix.map(Activation.dlrelu, this.values);
            gradients.hadamard(this.errors);
            gradients.multiply(Program.LearningRate);

            // Calculate Deltas
            Matrix inputValues_T = Matrix.transpose(input.values);
            Matrix deltas = Matrix.multiply(gradients, inputValues_T);

            // Updating Weights and Biases
            input.weights.add(deltas);
            this.bias.add(gradients);

        }

        public override void initWeights()
        {
            throw new NotImplementedException();
        }

    }
}