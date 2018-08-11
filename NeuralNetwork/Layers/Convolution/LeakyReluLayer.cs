using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuralNetwork.Core;

namespace NeuralNetwork.Layers.Convolution
{
    class LeakyReluLayer : LayerBase
    {
        public override void FeedForward(LayerBase input)
        {
            featuremaps = input.featuremaps;
            filters = input.filters;

            for(int i = 0; i < featuremaps.Count; i++)
            {
                featuremaps[i].map.map(Activation.lrelu);
            }
        }

        public override void Backpropagate(LayerBase input, Matrix errors)
        {
            for(int i = 0; i < featuremaps.Count; i++)
            {
                
            }
        }

        public override void Backpropagate(LayerBase input, LayerBase output)
        {
            throw new NotSupportedException("Could not backpropagate when the final layer is a convolutional layer.");
        }

        public override void initWeights(Random r, LayerBase nextLayer)
        {

        }

        public override void initWeights(Random r)
        {

        }
    }
}
