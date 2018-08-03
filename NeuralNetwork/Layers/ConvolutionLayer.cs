using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NeuralNetwork.Core;
using NeuralNetwork.Core.Convolution;

namespace NeuralNetwork.Layers
{
    class ConvolutionLayer : LayerBase
    {
        public ConvolutionLayer(int inputCount, int inputWidth, int inputHeight, int filterCount, int filterWidth, int filterHeight, int padding, int stride)
        {
            double outputWidth = (inputWidth - filterWidth + (2 * padding)) / stride + 1;
            double outputHeight = (inputHeight - filterHeight + (2 * padding)) / stride + 1;
            if (outputWidth % 1 != 0 || outputHeight % 1 != 0) throw new ArgumentException("Configered padding or stride is invalid for input and filter size.");

            featuremaps = new FeatureMaps(inputCount, inputWidth, inputHeight, padding, stride);
            filters = new Filters(filterCount, filterWidth, filterHeight, inputCount);
        }

        public override void FeedForward(LayerBase input)
        {
            featuremaps.SetZero();
            FeatureMaps features = input.featuremaps;


            for(int f = 0; f < input.filters.Count; f++)              // Loop thru the (previous) filters
            {
                Filter filter = input.filters[f];

                for(int k = 0; k < filter.Count; k++)                     // Loop thru the (previous) filters' kernels
                {
                    Matrix kernel = filter[k].flip();

                    int xOut = 0;

                    for(int x = -input.featuremaps.Padding; x < features.Width + features.Padding - input.filters.Width + 1; x += features.Stride)
                    {
                        int yOut = 0;

                        for (int y = -input.featuremaps.Padding; y < features.Height + features.Padding - input.filters.Height + 1; x += features.Stride)
                        {
                            Matrix sub = features[k].map.subMatrix(x, y, kernel.rows, kernel.cols);
                            Matrix multiplied = Matrix.hadamard(kernel, sub);

                            float sum = Matrix.sum(multiplied);
                            featuremaps[f].map.data[xOut, yOut] = sum + filter.Bias;
                        }
                    }
                }
            }
        }

        public override void Backpropagate(LayerBase input, Matrix errors)
        {
            input.featuremaps.SetZeroError();


        }

        public override void Backpropagate(LayerBase input, LayerBase output)
        {
            throw new NotSupportedException("Could not backpropagate when a the final layer is a convolutional layer.");
        }
        
        public override void initWeights(Random r, LayerBase nextLayer)
        {
            filters.Randomize(r);
        }

        public override void initWeights(Random r)
        {
            filters.Randomize(r);
        }
    }
}
