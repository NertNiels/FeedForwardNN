using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FeedForward.Layers;

namespace FeedForward.Core
{
    class Model
    {
        LayerBase[] layers;



        #region Initializers
        List<LayerBase> initLayers;

        public static Model createModel()
        {
            Model model = new Model();

            return model;
        }

        public Model inputLayer(int nodes)
        {
            InputLayer layer = new InputLayer(nodes);

            initLayers.Add(layer);
            return this;
        }

        public Model leakyReluLayer(int nodes)
        {
            LeakyReluLayer layer = new LeakyReluLayer(nodes);

            initLayers.Add(layer);
            return this;
        }

        public Model endModel()
        {
            layers = initLayers.ToArray();

            Random r = new Random();

            for(int i = 0; i < layers.Length - 1; i++)
            {
                layers[i].initWeights(r, layers[i + 1]);
            }

            return this;
        }
        #endregion

    }
}
