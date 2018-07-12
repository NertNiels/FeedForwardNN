using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Newtonsoft.Json;

using FeedForward.Layers;
using FeedForward.Core;

namespace FeedForward.IO
{
    class ModelFileHandler
    {

        public static Model LoadModel(String path)
        {
            try
            {

                using (StreamReader sr = new StreamReader(path))
                {
                    String output = sr.ReadToEnd();

                    JsonConverter[] converters = { new LayerConverter() };

                    LayerBase[] layers = JsonConvert.DeserializeObject<LayerBase[]>(output, converters);

                    Model model = new Model(layers);

                    return model;
                }
            } catch (Exception e)
            {
                Program.writeErrorLine(e.Message);

                return null;
            }
        }

        public static Boolean SaveModel(String path, Model model)
        {
            try
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
                return false;
            } catch (Exception e)
            {
                Program.writeErrorLine(e.Message);

                return true;
            }
        }

    }
}
