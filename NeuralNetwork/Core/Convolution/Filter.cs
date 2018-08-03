using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.Core.Convolution
{
    public class Filters : List<Filter>
    {
        public int Depth { get { if (this.Count == 0) return 0; return this[0].Count; } }
        public int Width { get { if (this.Count == 0 || this[0].Count == 0 || this[0][0] == null) return 0; return this[0][0].rows; } }
        public int Height { get { if (this.Count == 0 || this[0].Count == 0 || this[0][0] ==  null) return 0; return this[0][0].cols; } }

        public Filters(int count, int width, int height, int depth)
        {
            for(int i = 0; i < count; i++)
            {
                Filter f = new Filter(width, height, depth);
                this.Add(f);
            }
        }

        public void Randomize(Random r)
        {
            for(int i = 0; i < this.Count; i++)
            {
                this[i].Randomize(r);
            }
        }
    }

    public class Filter : List<Matrix>
    {
        public float Bias;

        public Filter(int width, int height, int depth)
        {
            for(int i = 0; i < depth; i++)
            {
                Matrix m = new Matrix(width, height);
                this.Add(m);
            }
        }

        public void Randomize(Random r)
        {
            for(int i = 0; i < this.Count; i++)
            {
                this[i].randomize(r);
            }
            Bias = (float)r.NextDouble();
        }
    }
}
