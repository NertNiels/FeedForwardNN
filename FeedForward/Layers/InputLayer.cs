using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FeedForward.Core;

namespace FeedForward.Layers
{
    class InputLayer : LayerBase
    {
        public InputLayer(int nodes)
        {
            this.nodes = nodes;
        }

        public override void FeedForward(LayerBase input)
        {
            
        }

        public override void Backpropagate(LayerBase input, Matrix errors)
        {

        }

        public override void Backpropagate(LayerBase input, LayerBase output)
        {

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
