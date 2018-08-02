using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuralNetwork.Core;

namespace NeuralNetwork.Layers
{
    class ConvolutionLayer : LayerBase
    {
        public override void Backpropagate(LayerBase input, Matrix errors)
        {
            throw new NotImplementedException();
        }

        public override void Backpropagate(LayerBase input, LayerBase output)
        {
            throw new NotSupportedException("Could not backpropagate when a the final layer is a convolutional layer.");
        }

        public override void FeedForward(LayerBase input)
        {
            throw new NotImplementedException();
        }

        public override void initWeights(Random r, LayerBase nextLayer)
        {
            throw new NotImplementedException();
        }

        public override void initWeights(Random r)
        {
            throw new NotImplementedException();
        }
    }
}
