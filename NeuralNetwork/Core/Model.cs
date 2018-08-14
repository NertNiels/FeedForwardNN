using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NeuralNetwork.Layers;
using NeuralNetwork.Layers.Convolution;

namespace NeuralNetwork.Core
{
    public class Model
    {
        public LayerBase[] layers;

        public static float LearningRate = 0.01f;

        public String Name;
        public String Description;
        public String Type;

        public Model() { }

        public Model(LayerBase[] layers)
        {
            this.layers = layers;
        }
        

        public Matrix FeedForward(Matrix input)
        {
            if(!Matrix.checkForDimensions(input, layers[0].nodes, 1))
            {
                throw new ArgumentException("Input Data does not have the right dimension! Please take a look at:\n\tInput:\t" + input.rows + ",\t" + input.cols + "\n\tLayer:\t" + layers[0].values.rows + ",\t" + layers[0].values.cols);
            }

            layers[0].values = input;

            for(int i = 1; i < layers.Length; i++)
            {
                layers[i].FeedForward(layers[i - 1]);
            }

            return layers[layers.Length-1].values;
        }

        public void Backpropagate(Matrix output, Matrix targets)
        {
            Matrix errors = Matrix.subtract(targets, output);

            layers[layers.Length - 1].Backpropagate(layers[layers.Length - 2], errors);

            for(int i = layers.Length-2; i > 0; i--)
            {
                layers[i].Backpropagate(layers[i - 1], layers[i + 1]);
            }
        }

        #region Initializers
        List<LayerBase> initLayers;

        public static Model createModel()
        {
            Model model = new Model();

            return model;
        }

        public Model inputLayer(int nodes, float dropout)
        {
            initLayers = new List<LayerBase>();

            InputLayer layer = new InputLayer();
            layer.nodes = nodes;
            layer.dropout = dropout;

            initLayers.Add(layer);
            return this;
        }

        public Model leakyReluLayer(int nodes, float dropout)
        {
            Layers.Convolution.LeakyReluLayer layer = new Layers.Convolution.LeakyReluLayer();
            layer.nodes = nodes;
            layer.dropout = dropout;

            initLayers.Add(layer);
            return this;
        }

        public Model convolutionLayer(int inputCount, int inputWidth, int inputHeight, int filterCount, int filterWidth, int filterHeight, int padding, int stride)
        {
            ConvolutionLayer layer = new ConvolutionLayer(inputCount, inputWidth, inputHeight, filterCount, filterWidth, filterHeight, padding, stride);

            initLayers.Add(layer);
            return this;
        }

        public Model endModel()
        {
            layers = initLayers.ToArray();

            Random r = new Random();

            randomizeWeights(r);

            return this;
        }

        public void randomizeWeights(Random r)
        {
            for (int i = 0; i < layers.Length; i++)
            {
                if (i + 1 == layers.Length) layers[i].initWeights(r);
                else layers[i].initWeights(r, layers[i + 1]);
            }
        }

        #endregion

    }
}
