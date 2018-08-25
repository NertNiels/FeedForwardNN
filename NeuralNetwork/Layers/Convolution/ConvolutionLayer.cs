using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NeuralNetwork.Core;
using NeuralNetwork.Core.Convolution;

namespace NeuralNetwork.Layers.Convolution
{
    public class ConvolutionLayer : LayerBase
    {
        public ConvolutionLayer(int inputCount, int inputWidth, int inputHeight, int prevFilterWidth, int prevFilterHeight, int filterCount, int filterWidth, int filterHeight, int padding, int stride)
        {
            double outputWidth = (inputWidth - prevFilterWidth + (2 * padding)) / stride + 1;
            double outputHeight = (inputHeight - prevFilterHeight + (2 * padding)) / stride + 1;
            if (outputWidth % 1 != 0 || outputHeight % 1 != 0) throw new ArgumentException("Configered padding or stride is invalid for input and filter size.");

            featuremaps = new FeatureMaps(inputCount, (int)outputWidth, (int)outputHeight, padding, stride);
            filters = new Filters(filterCount, filterWidth, filterHeight, inputCount);

            this.layerType = LayerType.Convolution;
        }

        public override void FeedForward(LayerBase input)
        {
            featuremaps.SetZero();
            FeatureMaps features = input.featuremaps;


            for(int f = 0; f < input.filters.Count; f++)
            {
                Filter filter = input.filters[f];

                for(int k = 0; k < filter.Count; k++)
                {
                    Matrix kernel = filter[k].flip();

                    int xOut = 0;

                    for(int x = -input.featuremaps.Padding; x < features.Width + features.Padding - input.filters.Width + 1; x += features.Stride)
                    {
                        int yOut = 0;

                        for (int y = -input.featuremaps.Padding; y < features.Height + features.Padding - input.filters.Height + 1; y += features.Stride)
                        {
                            Matrix sub = features[k].map.subMatrix(x, y, kernel.rows, kernel.cols);
                            Matrix multiplied = Matrix.hadamard(kernel, sub);

                            float sum = Matrix.sum(multiplied);
                            featuremaps[f].map.data[xOut, yOut] = sum + filter.Bias;
                            yOut++;
                        }
                        xOut++;
                    }
                }
            }
        }

        public override void Backpropagate(LayerBase input, Matrix errors)
        {
            input.featuremaps.SetZeroError();
            FeatureMaps features = input.featuremaps;


            for(int f = 0; f < input.filters.Count; f++)
            {
                Filter filter = input.filters[f];
                FeatureMap mapOut = featuremaps[f];

                for (int k = 0; k < filter.Count; k++)
                {
                    FeatureMap mapIn = features[k];
                    Matrix kernel = filter[k];

                    Matrix deltas = new Matrix(input.filters.Width, input.filters.Height);

                    int xOut = 0;
                    for (int x = -features.Padding; x < features.Width + features.Padding - input.filters.Width + 1; x += features.Stride)
                    {
                        int yOut = 0;

                        for (int y = -input.featuremaps.Padding; y < features.Height + features.Padding - input.filters.Height + 1; x += features.Stride)
                        {
                            Matrix sub = Matrix.subMatrix(mapIn.map, x, y, featuremaps.Width, featuremaps.Height);
                            Matrix multiplied = Matrix.hadamard(sub, mapOut.errors.flip());

                            float sum = multiplied.sum();
                            deltas.data[xOut, yOut] = sum;
                        }
                    }

                    xOut = 0;
                    for(int x = -1 + features.Padding; x < featuremaps.Width - input.filters.Width + 2 - features.Padding; x++)
                    {
                        int yOut = 0;
                        for (int y = -1 + features.Padding; y < featuremaps.Height - input.filters.Height + 2 - features.Padding; y++)
                        {
                            Matrix sub = Matrix.subMatrix(mapOut.errors, x, y, input.filters.Width, input.filters.Height);
                            Matrix multiplied = Matrix.hadamard(sub, kernel);

                            float sum = multiplied.sum();

                            input.featuremaps[k].errors.data[xOut, yOut] = sum;                           

                            yOut += features.Stride;
                        }

                        xOut += features.Stride;
                    }



                    filter.Update(deltas, k);
                }
            }
        }

        public override void Backpropagate(LayerBase input, LayerBase output)
        {
            input.featuremaps.SetZeroError();
            FeatureMaps features = input.featuremaps;


            for (int f = 0; f < input.filters.Count; f++)
            {
                Filter filter = input.filters[f];
                FeatureMap mapOut = featuremaps[f];

                for (int k = 0; k < filter.Count; k++)
                {
                    FeatureMap mapIn = features[k];
                    Matrix kernel = filter[k];

                    Matrix deltas = new Matrix(input.filters.Width, input.filters.Height);

                    int xOut = 0;
                    for (int x = -features.Padding; x < features.Width + features.Padding - input.filters.Width + 1; x += features.Stride)
                    {
                        int yOut = 0;

                        for (int y = -input.featuremaps.Padding; y < features.Height + features.Padding - input.filters.Height + 1; y += features.Stride)
                        {
                            Matrix sub = Matrix.subMatrix(mapIn.map, x, y, featuremaps.Width, featuremaps.Height);
                            Matrix multiplied = Matrix.hadamard(sub, mapOut.errors.flip());

                            float sum = multiplied.sum();
                            deltas.data[xOut, yOut] = sum;
                            yOut++;
                        }
                        xOut++;
                    }

                    xOut = 0;
                    for (int x = -1 + features.Padding; x < featuremaps.Width - input.filters.Width + 2 - features.Padding; x++)
                    {
                        int yOut = 0;
                        for (int y = -1 + features.Padding; y < featuremaps.Height - input.filters.Height + 2 - features.Padding; y++)
                        {
                            Matrix sub = Matrix.subMatrix(mapOut.errors, x, y, input.filters.Width, input.filters.Height);
                            Matrix multiplied = Matrix.hadamard(sub, kernel.flip());

                            float sum = multiplied.sum();

                            input.featuremaps[k].errors.data[xOut, yOut] = sum;

                            yOut += features.Stride;
                        }

                        xOut += features.Stride;
                    }



                    filter.Update(deltas, k);
                }
            }
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
