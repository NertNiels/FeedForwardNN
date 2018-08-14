using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using NeuralNetwork.Layers;
using NeuralNetwork.Layers.Convolution;

namespace NeuralNetwork.IO
{
    public class LayerConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(LayerBase);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jo = JObject.Load(reader);

            LayerType lt = (LayerType)jo["LayerType"].Value<int>();

            switch(lt)
            {
                case LayerType.Input:
                    return jo.ToObject<InputLayer>(serializer);
                case LayerType.LeakyRelu:
                    return jo.ToObject<Layers.LeakyReluLayer>(serializer);
                case LayerType.Sigmoid:
                    return jo.ToObject<SigmoidLayer>(serializer);
                case LayerType.ConvolutionInput:
                    return jo.ToObject<InputLayer>(serializer);
                case LayerType.Convolution:
                    return jo.ToObject<ConvolutionLayer>(serializer);
                case LayerType.ConvolutionLeakyRelu:
                    return jo.ToObject<Layers.Convolution.LeakyReluLayer>(serializer);
                default:
                    return null;
            }
        }
        

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
