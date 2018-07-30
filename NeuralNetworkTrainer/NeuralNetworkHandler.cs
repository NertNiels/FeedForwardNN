using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Web;
using System.Threading;

using Newtonsoft.Json;

using NeuralNetwork.Core;
using NeuralNetwork.IO;
using NeuralNetwork.Layers;

using NeuralNetworkTrainer.Hubs;

namespace NeuralNetworkTrainer
{
    public static class NeuralNetworkHandler
    {
        public static Model network;

        public static List<float> Loss = new List<float>();

        public static DataKeeper keeper;

        public static Boolean isTraining = false;
        public static Thread train;

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

        public static void TrainNetwork(int epochs)
        {
            isTraining = true;

            Loss = new List<float>();

            int timer = 0;

            for(int e = 0; e < epochs; e++)
            for(int i = 0; i < keeper.DataSet.Length; i++)
            {
                Matrix output = network.FeedForward(keeper.DataSet[i].Inputs);
                network.Backpropagate(output, keeper.DataSet[i].Targets);

                Matrix errors = network.layers.Last().errors;
                float MSE = Activation.MeanSquaredError(errors);

                Loss.Add(MSE);

                timer++;
                if(timer >= 1000)
                {
                    network.Description = "Network's autosave on training iteration: " + i;
                    SaveNetwork(network.Name + "_autosave");
                    timer = 0;
                }
            }

            SaveNetwork(network.Name + "_autosave");

            isTraining = false;
            DataHub.dTraining();
            
        }

        public static String LoadNetwork(String name)
        {
            if (isTraining) return "You can't load a network when there already is one training.";

            String networkId = GoogleDriveHandler.GetFileIdByName(name + "-nn");

            if (String.IsNullOrEmpty(networkId)) return String.Format("No file found with the name {0}-nn.", name);

            String networkContent = GoogleDriveHandler.DownloadGoogleDocument(networkId, "text/plain", Encoding.UTF8);

            network = ModelFileHandler.LoadModelFromString(networkContent);
            network.Name = name;

            return "Network successfully loaded in.";
        }

        public static String SaveNetwork()
        {
            String content = ModelFileHandler.SaveModelToString(network);

            String fileId = GoogleDriveHandler.GetFileIdByName(network.Name + "-nn");
            if (fileId == null) GoogleDriveHandler.UploadGoogleDocument(content, network.Name + "-nn", "application/vnd.google-apps.file", "text/plain");
            else GoogleDriveHandler.UploadGoogleDocument(content, network.Name + "-nn", "application/vnd.google-apps.file", "text/plain", fileId);

            return String.Format("Successfully saved the network to {0}-nn", network.Name);
        }

        public static String SaveNetwork(String name)
        {
            String content = ModelFileHandler.SaveModelToString(network);

            String fileId = GoogleDriveHandler.GetFileIdByName(name + "-nn");
            if(fileId == null) GoogleDriveHandler.UploadGoogleDocument(content, name + "-nn", "application/vnd.google-apps.file", "text/plain");
            else GoogleDriveHandler.UploadGoogleDocument(content, name + "-nn", "application/vnd.google-apps.file", "text/plain", fileId);

            return String.Format("Successfully saved the network to {0}-nn", network.Name);
        }

        public static String RandomizeNetwork()
        {
            if (isTraining) return "You can't randomize a network when there already is one training.";

            if (network == null) return "No network loaded";

            Random r = new Random();

            network.randomizeWeights(r);

            return "Weights randomized";
        }
    }

    public class DataKeeper
    {
        public Data[] DataSet;

        public String Name;

        public void ShuffleDataSet()
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
        

        public static String LoadDataSet(String name)
        {
            if (NeuralNetworkHandler.isTraining) return "You can't load a dataset when there is a network training.";


            String datasetId = GoogleDriveHandler.GetFileIdByName(name + "-ds");

            if (datasetId == null) return String.Format("No file found with the name {0}-ds", name);

            String datasetContent = GoogleDriveHandler.DownloadGoogleDocument(datasetId, "text/plain", Encoding.UTF8);

            NeuralNetworkHandler.keeper = JsonConvert.DeserializeObject<DataKeeper>(datasetContent);
            NeuralNetworkHandler.keeper.ShuffleDataSet();

            return "Dataset successfully loaded in.";
        }

        public static String SaveDataSet()
        {
            String content = JsonConvert.SerializeObject(NeuralNetworkHandler.keeper);

            String fileId = GoogleDriveHandler.GetFileIdByName(NeuralNetworkHandler.keeper.Name);
            if (fileId == null) GoogleDriveHandler.UploadGoogleDocument(content, NeuralNetworkHandler.keeper.Name + "-ds", "application/vnd.google-apps.file", "text/plain");
            else GoogleDriveHandler.UploadGoogleDocument(content, NeuralNetworkHandler.keeper.Name + "-ds", "application/vnd.google-apps.file", "text/plain", fileId);

            return String.Format("Successfully saved the dataset to {0}-ds", NeuralNetworkHandler.keeper.Name);
        }

        public static String SaveDataSet(String name)
        {
            String content = JsonConvert.SerializeObject(NeuralNetworkHandler.keeper);
            NeuralNetworkHandler.keeper.Name = name;

            String fileId = GoogleDriveHandler.GetFileIdByName(name);
            if (fileId == null) GoogleDriveHandler.UploadGoogleDocument(content, name + "-ds", "application/vnd.google-apps.file", "text/plain");
            else GoogleDriveHandler.UploadGoogleDocument(content, name + "-ds", "application/vnd.google-apps.file", "text/plain", fileId);

            return String.Format("Successfully saved the dataset to {0}-ds", name);
        }
    }
    
    public class Data
    {
        public Matrix Inputs;
        public Matrix Targets;
    }
}