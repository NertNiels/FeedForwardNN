using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.Core.Convolution
{
    public class FeatureMaps : List<FeatureMap>
    {
        public int Width { get { if (this.Count == 0 || this[0] == null || this[0].map == null) return 0; return this[0].map.rows; } }
        public int Height { get { if (this.Count == 0 || this[0] == null || this[0].map == null) return 0; return this[0].map.cols; } }

        public int Padding { get; private set; }
        public int Stride { get; private set; }

        public FeatureMaps(int count, int width, int height, int padding, int stride)
        {
            Padding = padding;
            Stride = stride;

            for(int i = 0; i < count; i++)
            {
                FeatureMap fm = new FeatureMap(width, height);
                this.Add(fm);
            }
        }

        public void SetZero()
        {
            this.ForEach((fm) =>
            {
                fm.map.multiply(0);
            });
        }
        
        public void SetZeroError()
        {
            this.ForEach((fm) =>
            {
                fm.errors.multiply(0);
            });
        }
    }

    public class FeatureMap
    {
        public Matrix map;

        public Matrix errors;

        public FeatureMap(int width, int height)
        {
            map = new Matrix(width, height);
            errors = new Matrix(width, height);
        }
        
    }
}
