using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.Core
{
    class Filter
    {
        public int depth;
        public int width { get { if (kernels == null || kernels.Length == 0) return 0; else return kernels[0].rows; } }
        public int height { get { if (kernels == null || kernels.Length == 0) return 0; else return kernels[0].cols; } }

        public float bias;

        public Matrix[] kernels;
    }
}
