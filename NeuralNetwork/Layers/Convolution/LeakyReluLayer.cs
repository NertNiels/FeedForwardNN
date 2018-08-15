using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuralNetwork.Core;

namespace NeuralNetwork.Layers.Convolution
{
    public class LeakyReluLayer : LayerBase
    {
        public LeakyReluLayer()
        {
            this.layerType = LayerType.ConvolutionLeakyRelu;
        }

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
            featuremaps.SetZeroError();
            for(int i = 0; i < featuremaps.Count; i++)
            {
                featuremaps[i].errors = Matrix.map(Activation.dlrelu, featuremaps[i].map);
            }
        }

        public override void Backpropagate(LayerBase input, LayerBase output)
        {
            featuremaps.SetZeroError();
            for (int i = 0; i < featuremaps.Count; i++)
            {
                featuremaps[i].errors = Matrix.map(Activation.dlrelu, featuremaps[i].map);
            }
        }

        public override void initWeights(Random r, LayerBase nextLayer)
        {

        }

        public override void initWeights(Random r)
        {

        }
    }
}
