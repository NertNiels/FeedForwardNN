using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNet.SignalR;

using NeuralNetwork.Core;
using NeuralNetwork.Layers;


namespace NeuralNetworkTrainer.Hubs
{
    public class DataHub : Hub
    {

        public static Func<Boolean> dTraining;
        
        public void GetNetworkInformation()
        {
            Model network = NeuralNetworkHandler.network;
            if(network == null)
            {
                Clients.Caller.setNetworkInformation("No Network Training", "No Network Training", "No Network Training", false);
                return;
            }

            Clients.Caller.setNetworkInformation(network.Name, network.Type, "No Discription Available", true);
        }

        public void GetNewTrainingLoss(int currentNumberOfLoss)
        {
            float[] loss = NeuralNetworkHandler.Loss.ToArray();
            if (loss == null || loss.Length - currentNumberOfLoss <= 0) Clients.Caller.giveNewTrainingLoss(null);

            float[] newLoss = new float[loss.Length - currentNumberOfLoss];
            Array.Copy(loss, loss.Length - (loss.Length - currentNumberOfLoss), newLoss, 0, loss.Length - currentNumberOfLoss);

            Clients.Caller.giveNewTrainingLoss(newLoss);
        }

        public void TerminalCommand(String command)
        {
            Clients.All.terminalMessage(command);
            try
            {
                command = command.ToLower();
                if (String.IsNullOrEmpty(command) || String.IsNullOrWhiteSpace(command))
                {
                    Clients.All.terminalError("That's not a valid command... Type \'help\' if you don\'t know what command to use.");
                    return;
                }
                String[] commands = command.Split(' ');
                String head = commands[0];



                if (head == "neuralnetwork")
                {
                    Model network = NeuralNetworkHandler.network;
                    if (commands.Length == 1)
                    {
                        if (network == null) Clients.All.terminalMessage("There is no Network currently loaded.");
                        else
                        {
                            String message =
                                "Current loaded network:\n" +
                                "\tName:\t" + network.Name + "\n" +
                                "\tType:\t" + network.Type;
                            Clients.All.terminalMessage(message);
                        }
                    }
                    else if (commands[1] == "create")
                    {
                        if (network == null)
                        {
                            List<LayerBase> layers = new List<LayerBase>();

                            for (int i = 4; i < commands.Length; i += 2)
                            {
                                LayerBase l = null;
                                if (commands[i] == "input")
                                {
                                    l = new InputLayer();
                                }
                                else if (commands[i] == "lrelu")
                                {
                                    l = new LeakyReluLayer();
                                }
                                else if (commands[i] == "sigmoid")
                                {
                                    l = new SigmoidLayer();
                                }
                                else
                                {
                                    Clients.All.terminalError(String.Format("Layer type {0} does not exist.", commands[i]));
                                    return;
                                }
                                l.nodes = int.Parse(commands[i + 1]);
                                layers.Add(l);
                            }
                            NeuralNetworkHandler.CreateNetwork(layers.ToArray(), commands[2], commands[3]);
                            Clients.All.terminalMessage("Neural Network successfully created.");
                            return;
                        }
                    } else if(commands[1] == "load")
                    {
                        Clients.All.terminalMessage(NeuralNetworkHandler.LoadNetwork(commands[2]));
                    } else if(commands[1] == "save")
                    {
                        if(commands.Length == 2) Clients.All.terminalMessage(NeuralNetworkHandler.SaveNetwork());
                        else Clients.All.terminalMessage(NeuralNetworkHandler.SaveNetwork(commands[2]));
                    } else if(commands[1] == "randomize")
                    {
                        Clients.All.terminalMessage(NeuralNetworkHandler.RandomizeNetwork());
                    } else if(commands[1] == "train")
                    {
                        if (network == null)
                        {
                            Clients.All.terminalMessage("No network loaded.");
                            return;
                        }
                        if (NeuralNetworkHandler.keeper == null)
                        {
                            Clients.All.terminalMessage("No dataset loaded.");
                            return;
                        }

                        int epochs = 1;
                        if (commands.Length != 2) epochs = int.Parse(commands[2]);
                        NeuralNetworkHandler.train = new Thread(() => NeuralNetworkHandler.TrainNetwork(epochs));
                        NeuralNetworkHandler.train.IsBackground = true;
                        NeuralNetworkHandler.train.Name = "Training Thread";

                        dTraining = doneTraining;

                        NeuralNetworkHandler.train.Start();
                    } else if(commands[1] == "learningrate")
                    {
                        if (commands.Length == 2) Clients.All.terminalMessage("Learning rate: " + Model.LearningRate);
                        else Model.LearningRate = float.Parse(commands[2]);
                    }
                } else if (head == "test")
                {

                    NeuralNetworkHandler.keeper = new DataKeeper();
                    Data[] data = new Data[7200];

                    float x = -360;

                    for(int i = 0; i < 7200; i++)
                    {
                        data[i] = new Data();
                        data[i].Inputs = new Matrix(1, 1) { data = new float[1, 1] { { x } } };
                        data[i].Targets = new Matrix(1, 1) { data = new float[1, 1] { { (float) Math.Sin((x * Math.PI) / 180) } } };
                        x +=  (float)0.1;
                    }
                    NeuralNetworkHandler.keeper.DataSet = data;

                    Clients.All.terminalMessage("Done with creating the dataset.");
                } else if(head == "dataset")
                {
                    if(commands.Length == 1)
                    {
                        if (NeuralNetworkHandler.keeper == null) Clients.All.terminalMessage("There is no dataset loaded yet.");
                        else Clients.All.terminalMessage(String.Format("The dataset with the name {0} is currently loaded.", NeuralNetworkHandler.keeper.Name));
                    } else if(commands[1] == "load")
                    {
                        Clients.All.terminalMessage(DataKeeper.LoadDataSet(commands[2]));
                    } else if (commands[1] == "save")
                    {
                        if (commands.Length == 2) Clients.All.terminalMessage(DataKeeper.SaveDataSet());
                        else Clients.All.terminalMessage(DataKeeper.SaveDataSet(commands[2]));
                    } else if( commands[1] == "unload")
                    {
                        NeuralNetworkHandler.keeper = null;
                    }
                } else if(head == "googledrive")
                {
                    if(commands[1] == "login")
                    {
                        GoogleDriveHandler.GoogleDriveLogin(HttpRuntime.AppDomainAppPath + "/credentials.json");
                        Clients.All.terminalMessage("Successfully logged in on Google Drive.");
                    } else if (commands[1] == "logout")
                    {
                        GoogleDriveHandler.GoogleDriveLogout("credentials.json");
                        Clients.All.terminalMessage("Successfully logged out from Google Drive.");
                    } else if (commands[1] == "list")
                    {
                        IList<Google.Apis.Drive.v3.Data.File> list = GoogleDriveHandler.GetFileList();

                        foreach (var file in list)
                        {
                            Clients.All.terminalMessage(String.Format("{0} ({1})", file.Name, file.Id));
                        }
                    }

                    
                }
                else Clients.All.terminalError("That's not a valid command... Type \'help\' if you don\'t know what command to use.");
            } catch (Exception e)
            {
                Clients.All.terminalError("Command resolved in an error.");
                Clients.All.terminalError(e.Message);
                Clients.All.terminalError(e.StackTrace);
            }
        }

        public Boolean doneTraining()
        {
            Clients.All.terminalMessage("Training is done");
            return true;
        }
    }
}