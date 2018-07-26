using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Web;

using Newtonsoft.Json;

using NeuralNetwork.Core;
using NeuralNetwork.IO;
using NeuralNetwork.Layers;

namespace NeuralNetworkTrainer
{
    public static class NeuralNetworkHandler
    {
        public static Model network;

        public static void CreateNetwork(LayerBase[] layers, String name, String type)
        {
            network = new Model(layers);
            network.Name = name;
            network.Type = type;
        }

        public static LayerBase[] getLayers()
        {
            if (network == null) return null;
            return network.layers;
        }

        public static void Train()
        {
            for(int i = 0; i < DataKeeper.DataSet.Length; i++)
            {

            }
        }

        public static void LoadNetwork(String networkName, String datasetName)
        {
            GoogleDriveHandler.GoogleDriveLogin("credentials.json");

            String networkId = GoogleDriveHandler.GetFileIdByName(networkName + "-nn");
            String datasetId = GoogleDriveHandler.GetFileIdByName(datasetName + "-ds");

            String networkContent = GoogleDriveHandler.DownloadGoogleDocument(networkId, "text/plain", Encoding.UTF8);
            String datasetContent = GoogleDriveHandler.DownloadGoogleDocument(datasetId, "text/plain", Encoding.UTF8);

            network = ModelFileHandler.LoadModelFromString(networkContent);
            DataKeeper.LoadDataSet(datasetContent);
            DataKeeper.ShuffleDataSet();
        }
    }

    public static class DataKeeper
    {
        public static List<float> Loss = new List<float>();

        public static Data[] DataSet;

        public static void ShuffleDataSet()
        {
            if (DataSet == null) return;
            Random r = new Random();
            
            for(int i = 0; i < DataSet.Length; i++)
            {
                int b = r.Next(DataSet.Length - i);
                Data t = DataSet[i];
                DataSet[i] = DataSet[b];
                DataSet[b] = t;
            }
        }
        

        public static void LoadDataSet(String content)
        {
            DataSet = JsonConvert.DeserializeObject<Data[]>(content);
        }



    }

    public class Data
    {
        public Matrix Inputs;
        public Matrix Targets;
    }
}