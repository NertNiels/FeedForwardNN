using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NeuralNetwork.Core;

namespace NeuralNetwork.Layers
{
    public class SigmoidLayer : LayerBase
    {
        public SigmoidLayer()
        {
            this.layerType = LayerType.Sigmoid;
        }

        public override void FeedForward(LayerBase input)
        {
            this.values = Matrix.multiply(input.weights, input.values);
            this.values.add(this.bias);
            this.values.map(Activation.sigmoid);

            if (dropout != 0f)
            {
                Random r = new Random();
                for (int i = 0; i < nodes; i++)
                {
                    if (r.NextDouble() < dropout) values.data[i, 0] = 0f;
                }
            }
        }

        public override void Backpropagate(LayerBase input, Matrix errors)
        {
            // Calculating Errors
            this.errors = errors;

            // Calculating Gradients
            Matrix gradients = Matrix.map(Activation.dsigmoidY, this.values);
            gradients.hadamard(this.errors);
            gradients.multiply(Model.LearningRate);

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
            Matrix gradients = Matrix.map(Activation.dsigmoidY, this.values);
            gradients.hadamard(this.errors);
            gradients.multiply(Model.LearningRate);

            // Calculate Deltas
            Matrix inputValues_T = Matrix.transpose(input.values);
            Matrix deltas = Matrix.multiply(gradients, inputValues_T);

            // Updating Weights and Biases
            input.weights.add(deltas);
            this.bias.add(gradients);

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
