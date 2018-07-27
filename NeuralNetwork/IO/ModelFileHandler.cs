using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Newtonsoft.Json;

using NeuralNetwork.Layers;
using NeuralNetwork.Core;

namespace NeuralNetwork.IO
{
    public class ModelFileHandler
    {

        public static Model LoadModel(String path)
        {
            using (StreamReader sr = new StreamReader(path))
            {
                String output = sr.ReadToEnd();

                JsonConverter[] converters = { new LayerConverter() };

                LayerBase[] layers = JsonConvert.DeserializeObject<LayerBase[]>(output, converters);

                Model model = new Model(layers);

                return model;
            }
        }

        public static void SaveModel(String path, Model model)
        {
            JsonSerializer serializer = new JsonSerializer();

            LayerBase[] layers = model.layers;

            using (StreamWriter sw = new StreamWriter(path))
            {
                using (JsonWriter jw = new JsonTextWriter(sw))
                {
                    serializer.Serialize(jw, layers);
                }
            }
        }

        public static Model LoadModelFromString(String output)
        {
            JsonConverter[] converters = { new LayerConverter() };

            LayerBase[] layers = JsonConvert.DeserializeObject<LayerBase[]>(output, converters);

            Model model = new Model(layers);

            return model;
        }

        public static String SaveModelToString(Model model)
        {

            LayerBase[] layers = model.layers;

            return JsonConvert.SerializeObject(layers);
        }

    }
}
