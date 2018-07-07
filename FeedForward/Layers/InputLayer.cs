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

        public override void Backpropagate(LayerBase input, LayerBase output, Matrix errors)
        {

        }

        public override void Backpropagate(LayerBase input, LayerBase output)
        {

        }

        public override void initWeights(Random r, LayerBase nextLayer)
        {
            this.weights = new Matrix(nextLayer.nodes, this.nodes);
            weights.randomize(r);
        }
    }
}
