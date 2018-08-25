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
            input.featuremaps.SetZeroError();
            
            for(int i = 0; i < featuremaps.Count; i++)
            {
                input.featuremaps[i].errors = Matrix.hadamard(Matrix.map(Activation.dlrelu, featuremaps[i].map), errors);
            }
        }

        public override void Backpropagate(LayerBase input, LayerBase output)
        {
            //input.featuremaps.SetZeroError();

            
            for (int i = 0; i < featuremaps.Count; i++)
            {

                input.featuremaps[i].errors = Matrix.hadamard(Matrix.map(Activation.dlrelu, featuremaps[i].map), featuremaps[i].errors);
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
