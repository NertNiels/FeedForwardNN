﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using FeedForward.Layers;

namespace FeedForward.IO
{
    class LayerConverter : JsonConverter
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
                    return jo.ToObject<LeakyReluLayer>(serializer);
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