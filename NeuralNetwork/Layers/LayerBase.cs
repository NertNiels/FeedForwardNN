using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NeuralNetwork.Core;

namespace NeuralNetwork.Layers
{
    public abstract class LayerBase
    {

        protected LayerType layerType;
        public LayerType LayerType { get { return layerType; } }
        

        public int nodes;
        public float dropout = 0f;

        public Matrix values;
        public Matrix weights;
        public Matrix errors;
        public Matrix bias;
        

        public abstract void FeedForward(LayerBase input);
        public abstract void Backpropagate(LayerBase input, Matrix errors);
        public abstract void Backpropagate(LayerBase input, LayerBase output);

        public abstract void initWeights(Random r, LayerBase nextLayer);
        public abstract void initWeights(Random r);

    }

    public enum LayerType
    {
        Input,
        LeakyRelu,
        Sigmoid
    }
}
