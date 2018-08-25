using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NeuralNetwork.Core;
using NeuralNetwork.Core.Convolution;

namespace NeuralNetwork.Layers
{
    public class InputLayer : LayerBase
    {
        public InputLayer()
        {
            this.layerType = LayerType.Input;
        }

        public InputLayer(int count, int width, int height, int filterCount, int filterWidth, int filterHeight, int padding, int stride)
        {
            featuremaps = new FeatureMaps(count, width, height, padding, stride);
            filters = new Filters(filterCount, filterWidth, filterHeight, count);

            this.layerType = LayerType.ConvolutionInput;
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
            if (this.LayerType == LayerType.Input)
            {
                this.weights = new Matrix(nextLayer.nodes, this.nodes);
                this.bias = new Matrix(this.nodes, 1);
                weights.randomize(r);
                this.bias.randomize(r);
            } else if(this.LayerType == LayerType.ConvolutionInput)
            {
                filters.Randomize(r);
            }
        }

        public override void initWeights(Random r)
        {
            if (this.LayerType == LayerType.Input)
            {
                this.bias = new Matrix(this.nodes, 1);
            this.bias.randomize(r);
            }
            else if (this.LayerType == LayerType.ConvolutionInput)
            {
                filters.Randomize(r);
            }
        }
    }
}
